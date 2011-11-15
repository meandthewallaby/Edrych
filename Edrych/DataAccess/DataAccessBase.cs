using System;
using System.Collections.Generic;
using System.Data;

namespace Edrych.DataAccess
{
    /// <summary>Base class for the Data Access layer</summary>
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

        /// <summary>The type of connection to create</summary>
        public ConnectionType ConnectionType { get; set; }
        /// <summary>Connection string to use to connect to the server</summary>
        public string ConnectionString { get { return this.BuildConnectionString(); } }
        /// <summary>Server to connect to</summary>
        public string DataSource { get; set; }
        /// <summary>Database to connect to at first</summary>
        public string InitialCatalog { get; set; }
        /// <summary>Authentication mode used</summary>
        public AuthType Authentication { get; set; }
        /// <summary>Username for basic authentication</summary>
        public string Username { get; set; }
        /// <summary>Password for basic authentication</summary>
        public string Password { get; set; }
        /// <summary>Returns the active database</summary>
        public string SelectedDatabase { get { return this._conn.Database; } }
        /// <summary>Returns whether or not the connection is actively connected</summary>
        public bool IsConnected { get { return _conn != null && _conn.State != ConnectionState.Closed && _conn.State != ConnectionState.Broken; } }

        #endregion

        #region Public Methods

        /// <summary>Test whether the specified database type has the .NET libraries available.</summary>
        /// <returns>Boolean representing whether the connection type is viable.</returns>
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

        /// <summary>Opens the connection</summary>
        public void Open()
        {
            PrepareConnection();
        }

        /// <summary>Closes the connection and rolls back any open transactions</summary>
        public void Close()
        {
            if (this._tran != null)
                _tran.Rollback();
            if (this._conn != null && this._conn.State == ConnectionState.Open)
                _conn.Close();
        }

        /// <summary>Runs the query and returns the dataset responsible</summary>
        /// <param name="sqlQuery">Query to run</param>
        /// <param name="Browser">The browser viewmodel that's currently active in the window</param>
        /// <returns>ResultSet, containing returned data and messages</returns>
        public ResultSet GetDataSet(string sqlQuery, ViewModels.ServerBrowserViewModel Browser)
        {
            DataAccessQuery daq = new DataAccessQuery(this, Browser);
            return daq.RunQuery(sqlQuery);
        }

        /// <summary>Runs a query that returns rows to be read</summary>
        /// <param name="sqlQuery">Query to run</param>
        /// <returns>IDataReader object that reads the result of the query</returns>
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

        /// <summary>Runs a query that returns no rows (such as an insert, update, or delete)</summary>
        /// <param name="sqlQuery">Query to run</param>
        /// <returns>Integer representing the number of affected rows</returns>
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

        /// <summary>Opens a transaction</summary>
        public void BeginTransaction()
        {
            PrepareConnection();
            _tran = _conn.BeginTransaction();
        }

        /// <summary>Commits an open transaction</summary>
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

        /// <summary>Adds a parameter to the command object</summary>
        /// <param name="Name">Name of the parameter</param>
        /// <param name="Value">Object representing the value of the parameter</param>
        public void AddParameter(string Name, object Value)
        {
            _comm.Parameters.Add(GetDbParameter(Name, Value));
        }

        /// <summary>Clears parameters from the command object</summary>
        public void ClearParameters()
        {
            _comm.Parameters.Clear();
        }

        /// <summary>Closes and disposes of all connection objects</summary>
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

        /// <summary>Opens the connection and establishes the command object, if they haven't been initialized already.</summary>
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

        /// <summary>Opens up the connection to a database</summary>
        /// <returns>The native connection object, which inherits the IDbConnection interface</returns>
        internal abstract IDbConnection GetDbConnection();

        /// <summary>Creates a new command object</summary>
        /// <returns>The native command object, which inherits the IDbCommand interface</returns>
        internal abstract IDbCommand GetDbCommand();

        /// <summary>Creates a parameter to pass to the query</summary>
        /// <param name="Name">Name of the parameter to add</param>
        /// <param name="Value">Value of the parameter to add</param>
        /// <returns>The native parameter object, which inherits the IDbDataParameter interface</returns>
        internal abstract IDbDataParameter GetDbParameter(string Name, object Value);

        /// <summary>Gets the databases on the server.</summary>
        /// <returns>List of Database objects representing the databases</returns>
        internal abstract List<Database> GetDatabases();

        /// <summary>Gets the tables in a database.</summary>
        /// <returns>List of TableView objects representing the tables in the selected database</returns>
        internal abstract List<TableView> GetTables();

        /// <summary>Gets the views in a database.</summary>
        /// <returns>List of TableView objects representing the views in the selected database</returns>
        internal abstract List<TableView> GetViews();

        /// <summary>Gets the columns in a table or view, along with data type and nullable info</summary>
        /// <param name="TableName">Table or View name to get column info about</param>
        /// <returns>List of Column objects</returns>
        internal abstract List<Column> GetColumns(string TableName);

        /// <summary>Sets the active database</summary>
        /// <param name="DatabaseName">Name of the database to set the active connection to</param>
        internal abstract void SetDatabase(string DatabaseName);

        /// <summary>Builds the connection string necessary for connection to the database</summary>
        /// <returns>String representing the connection string</returns>
        internal abstract string BuildConnectionString();

        #endregion

        #region Protected Methods
        
        /// <summary>Delegate for the function on how the database should read the metadata items</summary>
        /// <param name="reader">IDataReader that is actively open to read from</param>
        /// <returns>Database item (such as a TableView or a Column)</returns>
        protected delegate object ReadItem(IDataReader reader);

        /// <summary>Queries the database for the item of the specified type</summary>
        /// <typeparam name="T">Type of database item to return</typeparam>
        /// <param name="Sql">Query to get the items</param>
        /// <param name="Read">Function that will read the items from the result set</param>
        /// <returns>List of the database items</returns>
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
