namespace Edrych.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class ResultSet : IDisposable
    {
        private DataTable _dt;

        public ResultSet()
        {
            _dt = new DataTable();
        }

        public DataTable Data { get { return _dt; } }
        public string Messages { get; set; }

        public void Dispose()
        {
            if (_dt != null)
                _dt.Dispose();
        }
    }

    public class BaseDbItem
    {
        public string Name { get; set; }
    }

    public class Database : BaseDbItem { }

    public class TableView : BaseDbItem { }

    public class Column : BaseDbItem
    {
        public string DataType { get; set; }
        public bool IsNullable { get; set; }
    }

    public class ConnectionSource : BaseDbItem
    {
        public ConnectionType ConnType { get; set; }
        public bool IsAvailable { get; set; }
        public bool AcceptsDatabase { get; set; }
        public List<AuthType> AuthTypes { get; set; }
        public bool AcceptsUsername { get; set; }
        public bool AcceptsPassword { get; set; }
    }

    public enum AuthType
    {
        None,
        Integrated,
        Basic
    }

    public enum ConnectionType
    {
        None,
        DB2,
        ODBC,
        SQLite,
        SQL_Server
    }
}
