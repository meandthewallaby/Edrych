namespace Edrych.DataAccess
{
    using System.Collections.Generic;

    public class TableView
    {
        public string Name { get; set; }
    }

    public class Column
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public bool IsNullable { get; set; }
    }

    public class Database
    {
        public string Name { get; set; }
    }

    public class ConnectionSource
    {
        public string Name { get; set; }
        public ConnectionType ConnType { get; set; }
        public bool IsAvailable { get; set; }
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
        ODBC,
        SQLite,
        SQLServer
    }
}
