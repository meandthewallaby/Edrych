using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Edrych.Models;
using Edrych.ViewModels;

namespace Edrych.DataAccess
{
    /// <summary>Handles the query aspect of the Data Access layer</summary>
    class DataAccessQuery
    {
        #region Private/Global Variables

        private const string BASE_PATTERN = @"\[([a-zA-Z0-9:\\\._\-]*?)\]\.\[([a-zA-Z0-9:\\\._\-]*?)\]\s?\{(.*?)\}";

        private DataAccessBase _dab;
        private DataAccessBase _externalDab;
        private ServerBrowserViewModel _browser;

        private readonly object _sync = new object();
        private bool _cancelPending = false;

        #endregion

        #region Constructor(s)

        /// <summary>Creates the query</summary>
        /// <param name="Dab">Data Access object to run the queries</param>
        /// <param name="Browser">Server tree from the active browser</param>
        public DataAccessQuery(DataAccessBase Dab, ref ServerBrowserViewModel Browser)
        {
            _dab = Dab;
            _browser = Browser;
        }

        #endregion

        #region Private Properties

        private bool CancelPending { get { lock (_sync) { return _cancelPending; } } }

        #endregion

        #region public Methods

        /// <summary>The main entry point to running a query. This determines whether the query is an External Query or an public Query</summary>
        /// <param name="Query">Query to run</param>
        /// <returns>ResultSet object containing data and messages as a result of the given query</returns>
        public ResultSet RunQuery(string Query)
        {
            ResultSet rs;
            if (IsExternalQuery(Query))
            {
                rs = ProcessExternalQuery(Query);
            }
            else
            {
                rs = ProcessInternalQuery(_dab, Query, true);
            }
            return rs;
        }

        /// <summary>Cancels the current query</summary>
        public void Cancel()
        {
            lock (_sync)
            {
                _cancelPending = true;
                if(_externalDab != null)
                    _externalDab.Cancel();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>Basic method to get a Match from a regular expression pattern and input</summary>
        /// <param name="pattern">RegEx pattern to apply</param>
        /// <param name="input">String to match against the pattern</param>
        /// <returns>Match object with the RegEx results</returns>
        private Match RunRegEx(string pattern, string input)
        {
            Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);
            input = input.Replace("\r\n", " ");
            input = input.Replace("\n", " ");
            return reg.Match(input);
        }

        /// <summary>Tests whether the given query is an external or public query</summary>
        /// <param name="Query">Query to parse</param>
        /// <returns>Boolean representing whether the given query is an external query</returns>
        private bool IsExternalQuery(string Query)
        {
            Match mt = RunRegEx(BASE_PATTERN, Query);
            return mt.Success;
        }

        /// <summary>Processes an public query, relative to the data access object</summary>
        /// <param name="Dab">DataAccessBase object to execute the query against</param>
        /// <param name="Query">Query to run</param>
        /// <returns>ResultSet object containing data and messages as a result of the given query</returns>
        private ResultSet ProcessInternalQuery(DataAccessBase Dab, string Query, bool ReportProgress)
        {
            IDataReader reader = null;

            try
            {
                if (_cancelPending)
                {
                    throw new Exception("Operation cancelled");
                }
                reader = Dab.ExecuteReader(Query);
                ResultSet rs = new ResultSet();

                rs.Messages = reader.RecordsAffected + " rows affected";
                LoadResult(rs.Data, reader, Dab, ReportProgress);

                return rs;
            }
            catch
            {
                CheckCancel();
                throw;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        /// <summary>Throws an exception if the operation was cancelled.</summary>
        private void CheckCancel()
        {
            if (_cancelPending)
            {
                throw new OperationCanceledException();
            }
        }

        private void LoadResult(DataTable dt, IDataReader reader, DataAccessBase Dab, bool ReportProgress)
        {
            DataTable schema = reader.GetSchemaTable();
            foreach (DataRow row in schema.Rows)
            {
                Type t = (Type)row["DataType"];
                if (t == typeof(byte[]))
                {
                    t = typeof(string);
                }
                DataColumn col = new DataColumn(row["ColumnName"].ToString(), t);
                dt.Columns.Add(col);
            }

            int rowCount = 0;

            while (reader.Read())
            {
                object[] vals = new object[dt.Columns.Count];
                foreach (DataColumn col in dt.Columns)
                {
                    if (reader[col.Ordinal].GetType() == typeof(byte[]))
                        vals[col.Ordinal] = BitConverter.ToString((byte[])reader[col.Ordinal]);
                    else
                        vals[col.Ordinal] = reader[col.Ordinal];
                }

                dt.LoadDataRow(vals, false);

                if (ReportProgress && ++rowCount == 1000)
                    Dab.OnRunQueryRowCreated(dt.AsDataView().ToTable());
            }
        }

        #endregion

        #region Private Methods - Extraserver Query Parsing

        /// <summary>Parses and executes an external query</summary>
        /// <param name="Query">Query to process</param>
        /// <returns>ResultSet object containing data and messages as a result of the given query</returns>
        private ResultSet ProcessExternalQuery(string Query)
        {
            CheckCancel();
            ResultSet rs = new ResultSet();
            ExternalQueryInfo myQuery = new ExternalQueryInfo();
            string pattern = string.Empty;
            if (Query.Trim().Substring(0, 6).ToLower() == "select")
            {
                pattern =
                    @"^\s*select(.*?)from\s*\(\s*(select.*?)\)\s*([a-z0-9]+)\s+(inner|outer|left|right|cross|full)\s+join\s+\(" +
                    BASE_PATTERN + @"\)\s*([a-z0-9]+)\s+on(.*?)$";
                Match mt = RunRegEx(pattern, Query);
                if (mt.Success)
                {
                    myQuery.Server = mt.Groups[5].Value;
                    myQuery.Database = mt.Groups[6].Value;
                    myQuery.ColumnList = mt.Groups[1].Value;
                    myQuery.InternalQuery = mt.Groups[2].Value;
                    myQuery.InternalAlias = mt.Groups[3].Value;
                    myQuery.JoinType = mt.Groups[4].Value;
                    myQuery.ExternalQuery = mt.Groups[7].Value;
                    myQuery.ExternalAlias = mt.Groups[8].Value;
                    myQuery.JoinClause = mt.Groups[9].Value;

                    rs = SelectExternalData(myQuery);
                }
            }
            else if (Query.Trim().Substring(0, 6).ToLower() == "insert")
            {
                pattern = @"^\s*insert\s+into\s+([a-z0-9\._\-#]+?)\s+(\(\s*([a-z0-9\,\s_\-]+?)\s*\)\s+)*?" + BASE_PATTERN + @"\s?;\s?$";
                Match mt = RunRegEx(pattern, Query);
                if (mt.Success)
                {
                    myQuery.Server = mt.Groups[4].Value;
                    myQuery.Database = mt.Groups[5].Value;
                    myQuery.ColumnList = mt.Groups[3].Value;
                    myQuery.InternalQuery = mt.Groups[1].Value;
                    myQuery.ExternalQuery = mt.Groups[6].Value;

                    rs = InsertExternalData(myQuery);
                }
            }
            else
            {
                throw new Exception("Invalid extraserver query syntax.");
            }

            return rs;
        }

        /// <summary>Inserts data from an external source to a local source</summary>
        /// <param name="Query">ExternalQuery object which contains all the necessary variables</param>
        private ResultSet InsertExternalData(ExternalQueryInfo Query)
        {
            ResultSet externalRs = null;

            try
            {
                string oldDb = string.Empty;

                //Get the server and database
                SetExternalDatabase(Query);

                //Grab the externalQuery from database on server
                externalRs = ProcessInternalQuery(_externalDab, Query.ExternalQuery, false);
                if (externalRs.Data.Rows.Count == 0)
                {
                    throw new Exception("No external data to insert!");
                }

                //Check for existence of table -- create if necessary
                if (!InsertTableExists(Query.InternalQuery))
                {
                    //TODO: Universal create table
                    throw new NotImplementedException();
                }

                if (_cancelPending)
                {
                    throw new Exception("Operation cancelled");
                }

                //Build the insert statement
                StringBuilder insertQuery = new StringBuilder("insert into " + Query.InternalQuery + " ");
                if (!string.IsNullOrEmpty(Query.ColumnList))
                {
                    insertQuery.Append("(" + Query.ColumnList + ") ");
                }
                insertQuery.Append("values (");

                List<string> columns = new List<string>();
                foreach (DataColumn col in externalRs.Data.Columns)
                {
                    insertQuery.Append("@" + col.ColumnName + ", ");
                    columns.Add(col.ColumnName);
                }

                insertQuery.Remove(insertQuery.Length - 2, 2);
                insertQuery.Append(")");

                //Start looping through them
                int rowsAffected = 0;
                foreach (DataRow row in externalRs.Data.Rows)
                {
                    CheckCancel();
                    _dab.ClearParameters();
                    foreach (string col in columns)
                    {
                        _dab.AddParameter("@" + col, row[col]);
                    }
                    rowsAffected += _dab.ExecuteNonQuery(insertQuery.ToString());
                }

                StringBuilder selectQuery = new StringBuilder("select ");
                if (string.IsNullOrEmpty(Query.ColumnList))
                {
                    selectQuery.Append("* ");
                }
                else
                {
                    selectQuery.Append(Query.ColumnList + " ");
                }
                selectQuery.Append("from ");
                selectQuery.Append(Query.InternalQuery);
                ResultSet rs = ProcessInternalQuery(_dab, selectQuery.ToString(), true);
                rs.Messages = rowsAffected.ToString() + " rows inserted\r\n\r\n" + rs.Messages;
                return rs;
            }
            finally
            {
                if(externalRs != null)
                    externalRs.Dispose();
                if (_externalDab != null)
                    _externalDab.Dispose();
            }
        }

        private ResultSet SelectExternalData(ExternalQueryInfo Query)
        {
            ResultSet rs = null;

            try
            {
                //Make the queries and then argh!
                rs = new ResultSet();
                ResultSet internalRs = ProcessInternalQuery(_dab, Query.InternalQuery, false);
                SetExternalDatabase(Query);
                ResultSet externalRs = ProcessInternalQuery(_externalDab, Query.ExternalQuery, false);
                DataSet joinDs = new DataSet();
                joinDs.Tables.Add(internalRs.Data);
                joinDs.Tables.Add(externalRs.Data);
                joinDs.Tables[0].TableName = Query.InternalAlias;
                joinDs.Tables[1].TableName = Query.ExternalAlias;

                List<QueryColumn> cols = BuildColumns(Query, rs.Data, internalRs.Data, externalRs.Data);

                JoinClause jc = BuildJoinClause(Query, internalRs.Data, externalRs.Data);

                if (Query.JoinType.ToLower() == "inner" && jc != null)
                {
                    var rows =
                        from a in externalRs.Data.AsEnumerable()
                        from b in internalRs.Data.AsEnumerable()
                        where a[jc.InternalCol].Equals(b[jc.ExternalCol])
                        select new { a, b };
                    foreach (var row in rows)
                    {
                        List<object> Data = new List<object>();
                        foreach (QueryColumn col in cols)
                        {
                            if (col.Table.Equals(internalRs.Data))
                                Data.Add(row.b[col.Name]);
                            else
                                Data.Add(row.a[col.Name]);
                        }

                        rs.Data.LoadDataRow(Data.ToArray(), false);
                    }
                }
                
                return rs;
            }
            catch
            {
                throw;
            }
            finally
            {
            }
        }

        /// <summary>Determines if the local table exists</summary>
        /// <param name="TableName">Name of the table to check</param>
        /// <returns>Boolean representing whether the local table exists</returns>
        private bool InsertTableExists(string TableName)
        {
            CheckCancel();
            IDataReader reader = null;

            try
            {
                reader = _dab.ExecuteReader("select 1 from " + TableName);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
        }

        /// <summary>Sets the external data access object</summary>
        /// <param name="Query">ExternalQuery object holding all parameters</param>
        private void SetExternalDatabase(ExternalQueryInfo Query)
        {
            if (_browser.Tree.Cache.ContainsKey("ROOT"))
            {
                ServerItem si = _browser.Tree.Cache["ROOT"].FirstOrDefault(s => s.Name == Query.Server) as ServerItem;
                _externalDab = 
                    DataAccessFactory.GetDataAccess(si.DataAccess.ConnectionType, si.DataAccess.DataSource, si.DataAccess.SelectedDatabase, si.DataAccess.Authentication, si.DataAccess.Username, si.DataAccess.Password);
                string oldDb = _externalDab.SelectedDatabase;
                _externalDab.SetDatabase(Query.Database);
                if (_externalDab.SelectedDatabase.ToUpper() != Query.Database.ToUpper())
                {
                    _externalDab.SetDatabase(oldDb);
                    throw new Exception("Could not set the database context for the extraserver query.");
                }
            }
            else
            {
                throw new Exception("I'm sorry, I don't know what server that is.");
            }
        }

        private List<QueryColumn> BuildColumns(ExternalQueryInfo Query, DataTable dt, DataTable Internal, DataTable External)
        {
            List<QueryColumn> cols = new List<QueryColumn>();
            foreach (string column in Query.ColumnList.Split(','))
            {
                QueryColumn qc = new QueryColumn();
                string col = column.Trim();
                int periodIndex = col.IndexOf('.');
                if (periodIndex > 0 && periodIndex + 1 < col.Length)
                {
                    qc.Name = col.Substring(periodIndex + 1);
                    qc.Table = col.Substring(0, periodIndex) == Query.InternalAlias ? Internal : col.Substring(0, periodIndex) == Query.ExternalAlias ? External : null;
                }
                else
                {
                    qc.Name = col;
                    qc.Table = Internal.Columns.Contains(col) && !External.Columns.Contains(col) ? Internal : !Internal.Columns.Contains(col) && External.Columns.Contains(col) ? External : null;
                }

                if (qc.Table != null)
                {
                    cols.Add(qc);
                    dt.Columns.Add(col);
                }
            }

            return cols;
        }

        private JoinClause BuildJoinClause(ExternalQueryInfo Query, DataTable Internal, DataTable External)
        {
            JoinClause jc = new JoinClause();
            foreach (string colDef in Query.JoinClause.Split('='))
            {
                string col = colDef.Trim();
                int periodIndex = col.IndexOf('.');
                if (periodIndex > 0 && periodIndex + 1 < col.Length)
                {
                    if (col.Substring(0, periodIndex) == Query.InternalAlias)
                        jc.InternalCol = col.Substring(periodIndex + 1);
                    else if (col.Substring(0, periodIndex) == Query.ExternalAlias)
                        jc.ExternalCol = col.Substring(periodIndex + 1);
                    else
                        jc = null;
                }
                else
                {
                    if (Internal.Columns.Contains(col) && !External.Columns.Contains(col))
                        jc.InternalCol = col;
                    else if (!Internal.Columns.Contains(col) && External.Columns.Contains(col))
                        jc.ExternalCol = col;
                    else
                        jc = null;
                }
            }
            return jc;
        }

        #endregion
    }
}
