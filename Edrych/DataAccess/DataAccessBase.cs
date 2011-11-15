using System;
using System.Collections.Generic;
using System.Data;

namespace Edrych.DataAccess
{
    public abstract class DataAccessBase : IDisposable
    {
        #region Private/Global Variables

        private IDbConnection _conn;
        private IDbCommand _comm;
        private IDbTransaction _tran;

        #endregion

        #region Constructor(s)

        public DataAccessBase()
        {
        }

        #endregion

        #region Public Properties

        public ConnectionType ConnectionType { get; set; }
        public string ConnectionString { get { return this.BuildConnectionString(); } }
        public string DataSource { get; set; }
        public string InitialCatalog { get; set; }
        public AuthType Authentication { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SelectedDatabase { get { return this._conn.Database; } }
        public bool IsConnected { get { return _conn != null && _conn.State != ConnectionState.Closed && _conn.State != ConnectionState.Broken; } }

        #endregion

        #region Public Methods

        public bool TestAvailability()
        {
            bool isAvailable = false;

            try
            {
                IDbCommand param = this.GetDbCommand();
                isAvailable = true;
            }
            catch
            {
                isAvailable = false;
            }

            return isAvailable;
        }

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

        public ResultSet GetDataSet(string sqlQuery, ViewModels.ServerBrowserViewModel Browser)
        {
            DataAccessQuery daq = new DataAccessQuery(this, Browser);
            return daq.RunQuery(sqlQuery);
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

        public void Dispose()
        {
            this.Close();
            if(_tran != null)
                _tran.Dispose();
            if(_comm != null)
                _comm.Dispose();
            if(_conn != null)
                _conn.Dispose();
        }

        #endregion

        #region Private Methods

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

        #endregion

        #region Abstract Methods

        internal abstract IDbConnection GetDbConnection();
        internal abstract IDbCommand GetDbCommand();
        internal abstract IDbDataParameter GetDbParameter(string Name, object Value);
        internal abstract List<Database> GetDatabases();
        internal abstract List<TableView> GetTables();
        internal abstract List<TableView> GetViews();
        internal abstract List<Column> GetColumns(string TableName);
        internal abstract void SetDatabase(string DatabaseName);
        internal abstract string BuildConnectionString();

        #endregion

        #region Protected Methods
        
        protected delegate object ReadItem(IDataReader reader);

        protected List<T> GetDbItems<T>(string Sql, ReadItem Read)
        {
            List<T> collection = new List<T>();
            IDataReader reader = this.ExecuteReader(Sql);
            while (reader.Read())
            {
                T item = (T)Read(reader);
                collection.Add(item);
            }
            reader.Close();

            return collection;
        }

        #endregion
    }
}
