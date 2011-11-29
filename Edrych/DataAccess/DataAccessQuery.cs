﻿using System;
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
                rs = ProcessInternalQuery(_dab, Query);
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
            Regex reg = new Regex(pattern);
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
        private ResultSet ProcessInternalQuery(DataAccessBase Dab, string Query)
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
                LoadResult(rs.Data, reader);

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

        private void LoadResult(DataTable dt, IDataReader reader)
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

            dt.BeginLoadData();
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
            }
            dt.EndLoadData();
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
            string pattern = @"^insert\s+into\s+([a-zA-Z0-9\._\-#]+?)\s+(\(\s*([a-zA-Z0-9\,\s_\-]+?)\s*\)\s+)*?" + BASE_PATTERN + @"\s?;\s?$";
            Match mt = RunRegEx(pattern, Query);
            if (mt.Success)
            {
                string insertTable = mt.Groups[1].Value;
                string server = mt.Groups[4].Value;
                string database = mt.Groups[5].Value;
                string colList = mt.Groups[3].Value;
                string externalQuery = mt.Groups[6].Value;

                rs = InsertExternalData(server, database, externalQuery, insertTable, colList);
            }
            else
            {
                throw new Exception("Invalid extraserver query syntax.");
            }

            return rs;
        }

        /// <summary>Inserts data from an external source to a local source</summary>
        /// <param name="server">External server to get data from</param>
        /// <param name="database">Database to use on the external server</param>
        /// <param name="externalQuery">Query to use to read the data from an external server</param>
        /// <param name="insertTable">Local table to insert the data into</param>
        /// <param name="colList">Column list from the local table to </param>
        /// <returns>ResultSet object containing data and messages as a result of the given query</returns>
        private ResultSet InsertExternalData(string server, string database, string externalQuery, string insertTable, string colList)
        {
            ResultSet externalRs = null;

            try
            {
                string oldDb = string.Empty;

                //Get the server and database
                if (_browser.Tree.Cache.ContainsKey("ROOT"))
                {
                    ServerItem si = _browser.Tree.Cache["ROOT"].FirstOrDefault(s => s.Name == server) as ServerItem;
                    _externalDab = si.DataAccess;
                    oldDb = _externalDab.SelectedDatabase;
                    _externalDab.SetDatabase(database);
                    if (_externalDab.SelectedDatabase.ToUpper() != database.ToUpper())
                    {
                        _externalDab.SetDatabase(oldDb);
                        throw new Exception("Could not set the database context for the extraserver query.");
                    }
                }
                else
                {
                    throw new Exception("I'm sorry, I don't know what server that is.");
                }

                //Grab the externalQuery from database on server
                externalRs = ProcessInternalQuery(_externalDab, externalQuery);
                if (externalRs.Data.Rows.Count == 0)
                {
                    throw new Exception("No external data to insert!");
                }

                //Check for existence of table -- create if necessary
                if (!InsertTableExists(insertTable))
                {
                    //TODO: Universal create table
                    throw new NotImplementedException();
                }

                if (_cancelPending)
                {
                    throw new Exception("Operation cancelled");
                }

                //Build the insert statement
                StringBuilder insertQuery = new StringBuilder("insert into " + insertTable + " ");
                if (!string.IsNullOrEmpty(colList))
                {
                    insertQuery.Append("(" + colList + ") ");
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
                if (string.IsNullOrEmpty(colList))
                {
                    selectQuery.Append("* ");
                }
                else
                {
                    selectQuery.Append(colList + " ");
                }
                selectQuery.Append("from ");
                selectQuery.Append(insertTable);
                ResultSet rs = ProcessInternalQuery(_dab, selectQuery.ToString());
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

        #endregion
    }
}
