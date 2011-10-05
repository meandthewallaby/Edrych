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

        internal override IDbConnection GetDbConnection()
        {
            return new SQLiteConnection(this.ConnectionString);
        }

        internal override IDbCommand GetDbCommand()
        {
            return new SQLiteCommand();
        }
    }
}
