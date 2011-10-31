using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLiteBrowser.Resources;

namespace SQLiteBrowser.DataAccess
{
    public enum ConnectionType
    {
        SQLite
    }

    public class DataAccessFactory
    {
        public static DataAccessBase GetDataAccess()
        {
            return GetDataAccess(ConnectionType.SQLite, DataAccessResources.DefaultConnectionString);
        }

        public static DataAccessBase GetDataAccess(ConnectionType ConnectionType, string DataSource)
        {
            return GetDataAccess(ConnectionType, DataSource, null, null);
        }

        public static DataAccessBase GetDataAccess(ConnectionType ConnectionType, string DataSource, string Username, string Password)
        {
            DataAccessBase dab;

            switch (ConnectionType)
            {
                case ConnectionType.SQLite:
                    dab = new SQLiteDataAccess();
                    break;
                default:
                    dab = new SQLiteDataAccess();
                    break;
            }

            dab.DataSource = DataSource;
            dab.Username = Username;
            dab.Password = Password;
            dab.Open();

            return dab;
        }
    }
}
