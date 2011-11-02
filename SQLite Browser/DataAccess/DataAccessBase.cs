using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SQLiteBrowser.DataAccess
{
    public abstract class DataAccessBase
    {
        private IDbConnection _conn;
        private IDbCommand _comm;
        private IDbTransaction _tran;

        public DataAccessBase()
        {
        }

        public ConnectionType ConnectionType { get; set; }
        public string ConnectionString { get; set; }
        public string DataSource { get; set; }
        public string InitialCatalog { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AuthType { get; set; }
        public string Name { get { return _conn.Database; } }

        public void Open()
        {
            PrepareConnection();
        }

        public void Close()
        {
            if (this._tran != null)
                _tran.Rollback();
            if (this._conn != null && this._conn.State == ConnectionState.Open)
                _conn.Close();
        }

        public ResultSet GetDataSet(string sqlQuery)
        {
            IDataReader reader = this.ExecuteReader(sqlQuery);
            ResultSet rs = new ResultSet();

            rs.Messages = reader.RecordsAffected + " rows affected";
            rs.Data.Load(reader);

            reader.Close();

            return rs;
        }

        public IDataReader ExecuteReader(string sqlQuery)
        {
            try
            {
                PrepareConnection();
                _comm.CommandText = sqlQuery;
                IDataReader reader = _comm.ExecuteReader();
                return reader;
            }
            catch
            {
                throw;
            }
        }

        public int ExecuteNonQuery(string sqlQuery)
        {
            try
            {
                PrepareConnection();
                _comm.CommandText = sqlQuery;
                return _comm.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
        }

        public void BeginTransaction()
        {
            PrepareConnection();
            _tran = _conn.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (_tran != null)
            {
                _tran.Commit();
            }
            if (_conn.State == ConnectionState.Open)
            {
                _conn.Close();
            }
        }

        public void AddParameter(string Name, object Value)
        {
            _comm.Parameters.Add(GetDbParameter(Name, Value));
        }

        public void ClearParameters()
        {
            _comm.Parameters.Clear();
        }

        private void PrepareConnection()
        {
            if (_conn == null)
            {
                _conn = GetDbConnection();
            }

            if (_conn.State == ConnectionState.Closed)
            {
                _conn.Open();
            }

            if (_comm == null)
            {
                _comm = GetDbCommand();
                _comm.Connection = _conn;
            }
        }

        internal abstract IDbConnection GetDbConnection();
        internal abstract IDbCommand GetDbCommand();
        internal abstract IDbDataParameter GetDbParameter(string Name, object Value);
        internal abstract List<Database> GetDatabases();
        internal abstract List<TableView> GetTables();
        internal abstract List<TableView> GetViews();
        internal abstract List<Column> GetColumns(string TableName);
        internal abstract void SetDatabase(string DatabaseName);
    }
}
