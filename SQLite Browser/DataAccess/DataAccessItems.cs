﻿namespace SQLiteBrowser.DataAccess
{
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
}
