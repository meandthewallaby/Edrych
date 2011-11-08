using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Edrych.Properties;

namespace Edrych.DataAccess
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
        public static ConnectionType DefaultType
        {
            get 
            {
                ConnectionType type;
                if (!Enum.TryParse<ConnectionType>(Settings.Default.DefaultConnection, out type))
                {
                    type = ConnectionType.None;
                }
                return type; 
            }
        }

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

        public static void SetDefaultType(ConnectionType ConnectionType)
        {
            Settings.Default.DefaultConnection = ConnectionType.ToString();
            Settings.Default.Save();
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
