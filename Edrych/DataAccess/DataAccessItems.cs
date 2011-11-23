namespace Edrych.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>Object to use to track recent connections and build a connection from the Connect Dialog</summary>
    class DataAccessConnection
    {
        /// <summary>Base constructor that gives an empty DataAccessConnection</summary>
        public DataAccessConnection()
        {
        }

        /// <summary>Constructor to get a built out DataAccessConnectio</summary>
        /// <param name="Type">Connection Type to use</param>
        /// <param name="DataSource">Server to connect to</param>
        /// <param name="Database">Initial database</param>
        /// <param name="Auth">Authentication mode</param>
        /// <param name="Username">Username for basic authentication</param>
        /// <param name="Password">Password for basic authentication</param>
        public DataAccessConnection(ConnectionType Type, string DataSource, string Database, AuthType Auth, string Username, string Password)
        {
            this.Connection = Type;
            this.DataSource = DataSource;
            this.Database = Database;
            this.Auth = Auth;
            this.Username = Username;
            this.Password = Password;
        }

        /// <summary>Connection type</summary>
        public ConnectionType Connection { get; set; }
        /// <summary>Server to connect to</summary>
        public string DataSource { get; set; }
        /// <summary>Initial database</summary>
        public string Database { get; set; }
        /// <summary>Authentication mode</summary>
        public AuthType Auth { get; set; }
        /// <summary>Username for basic authentication</summary>
        public string Username { get; set; }
        /// <summary>Password for basic authentication</summary>
        public string Password { get; set; }
    }

    /// <summary>All queries return a result set, consisting of data and messages about the query</summary>
    class ResultSet : IDisposable
    {
        private DataTable _dt;

        /// <summary>Constructor</summary>
        public ResultSet()
        {
            _dt = new DataTable();
        }

        /// <summary>DataTable that houses all the data</summary>
        public DataTable Data { get { return _dt; } }
        /// <summary>String of messages from the query execution</summary>
        public string Messages { get; set; }

        /// <summary>Disposes the data</summary>
        public void Dispose()
        {
            if (_dt != null)
                _dt.Dispose();
        }
    }

    /// <summary>Base database item</summary>
    class BaseDbItem
    {
        /// <summary>Name of the item</summary>
        public string Name { get; set; }
    }

    /// <summary>Item for databases</summary>
    class Database : BaseDbItem { }

    /// <summary>Item for tables and views</summary>
    class TableView : BaseDbItem { }

    /// <summary>Item for columns</summary>
    class Column : BaseDbItem
    {
        /// <summary>Data Type of the column</summary>
        public string DataType { get; set; }
        /// <summary>Whether the column accepts nulls</summary>
        public bool IsNullable { get; set; }
    }

    /// <summary>Source info for a connection that the connect dialog builds</summary>
    class ConnectionSource : BaseDbItem
    {
        /// <summary>Type of connection</summary>
        public ConnectionType ConnType { get; set; }
        /// <summary>Whether the connection type is available to the user</summary>
        public bool IsAvailable { get; set; }
        /// <summary>Whether the connection type accepts a Database argument</summary>
        public bool AcceptsDatabase { get; set; }
        /// <summary>Authentication mode</summary>
        public List<AuthType> AuthTypes { get; set; }
        /// <summary>Whether the connection type accepts a username</summary>
        public bool AcceptsUsername { get; set; }
        /// <summary>Whether the connection type accepts a password</summary>
        public bool AcceptsPassword { get; set; }
        /// <summary>Whether the user can browse for a connection</summary>
        public bool AllowBrowse { get; set; }
    }

    /// <summary>Enumeration of the different authentication modes available</summary>
    enum AuthType
    {
        None,
        Integrated,
        Basic
    }

    /// <summary>Enumeration of the different connection types supported</summary>
    enum ConnectionType
    {
        None,
        DB2,
        ODBC,
        SQLite,
        SQL_Server,
        Teradata
    }
}
