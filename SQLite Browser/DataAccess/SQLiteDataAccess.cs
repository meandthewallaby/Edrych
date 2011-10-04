using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace SQLiteBrowser.DataAccess
{
    public class SQLiteDataAccess : DataAccessBase
    {
        public SQLiteDataAccess()
        {
        }

        public SQLiteConnection GetDbConnection()
        {
            return new SQLiteConnection(this.ConnectionString);
        }

        public SQLiteCommand GetDbCommand()
        {
            return new SQLiteCommand();
        }
    }
}
