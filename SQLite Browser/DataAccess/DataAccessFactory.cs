using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SQLiteBrowser.Properties;

namespace SQLiteBrowser.DataAccess
{
    public enum ConnectionType
    {
        None,
        ODBC,
        SQLite,
        SQLServer
    }

    public class DataAccessConnection
    {
        public DataAccessConnection(ConnectionType Type, string ConnectionString)
        {
            this.Connection = Type;
            this.DataSource = ConnectionString;
        }

        public ConnectionType Connection { get; set; }
        public string DataSource { get; set; }
    }

    public class DataAccessFactory
    {
        public const ConnectionType DefaultType = ConnectionType.SQLServer;

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
            DataAccessBase dab = GetConnection(ConnectionType);

            dab.DataSource = DataSource;
            dab.Username = Username;
            dab.Password = Password;
            dab.Open();

            return dab;
        }

        public static List<ConnectionSource> GetSources()
        {
            List<ConnectionSource> sources = new List<ConnectionSource>();

            foreach (ConnectionType type in Enum.GetValues(typeof(ConnectionType)))
            {
                ConnectionSource source = new ConnectionSource();
                source.Name = type.ToString();
                DataAccessBase db = GetConnection(type);
                source.IsAvailable = db.TestAvailability();
                db.Dispose();
                if(source.IsAvailable)
                    sources.Add(source);
            }

            return sources;
        }

        private static DataAccessBase GetConnection(ConnectionType ConnectionType)
        {
            DataAccessBase dab;

            switch (ConnectionType)
            {
                case ConnectionType.None:
                    dab = new NoneDataAccess();
                    break;
                case ConnectionType.ODBC:
                    dab = new ODBCServerDataAccess();
                    break;
                case ConnectionType.SQLite:
                    dab = new SQLiteDataAccess();
                    break;
                case ConnectionType.SQLServer:
                    dab = new SQLServerDataAccess();
                    break;
                default:
                    dab = new NoneDataAccess();
                    break;
            }

            return dab;
        }
    }
}
