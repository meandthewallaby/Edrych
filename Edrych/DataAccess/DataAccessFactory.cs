using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Edrych.Properties;

namespace Edrych.DataAccess
{
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
            return GetDataAccess(ConnectionType.SQLite, DataAccessResources.DefaultConnectionString, AuthType.None);
        }

        public static DataAccessBase GetDataAccess(ConnectionType ConnectionType, string DataSource)
        {
            return GetDataAccess(ConnectionType, DataSource, null, AuthType.None, null, null);
        }

        public static DataAccessBase GetDataAccess(ConnectionType ConnectionType, string DataSource, AuthType Auth)
        {
            return GetDataAccess(ConnectionType, DataSource, null, Auth, null, null);
        }

        public static DataAccessBase GetDataAccess(ConnectionType ConnectionType, string DataSource, string InitialCatalog, AuthType Auth, string Username, string Password)
        {
            DataAccessBase dab = GetConnection(ConnectionType);

            dab.DataSource = DataSource;
            dab.InitialCatalog = InitialCatalog;
            dab.Authentication = Auth;
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
                ConnectionSource source = BuildConnectionSource(type);
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

        private static ConnectionSource BuildConnectionSource(ConnectionType ConnectionType)
        {
            ConnectionSource source = new ConnectionSource();
            source.Name = ConnectionType.ToString().Replace("_", " ");
            source.ConnType = ConnectionType;
            switch (ConnectionType)
            {
                case ConnectionType.ODBC:
                case ConnectionType.SQL_Server:
                    source.AuthTypes = new List<AuthType>() { AuthType.Integrated, AuthType.Basic };
                    source.AcceptsDatabase = false;
                    source.AcceptsUsername = true;
                    source.AcceptsPassword = true;
                    source.AllowBrowse = false;
                    break;
                case ConnectionType.SQLite:
                    source.AuthTypes = new List<AuthType>() { AuthType.None, AuthType.Basic };
                    source.AcceptsDatabase = false;
                    source.AcceptsUsername = false;
                    source.AcceptsPassword = true;
                    source.AllowBrowse = true;
                    break;
                case ConnectionType.DB2:
                    source.AuthTypes = new List<AuthType>() { AuthType.Basic };
                    source.AcceptsDatabase = true;
                    source.AcceptsUsername = true;
                    source.AcceptsPassword = true;
                    source.AllowBrowse = false;
                    break;
                case ConnectionType.Teradata:
                    source.AuthTypes = new List<AuthType>() { AuthType.Basic };
                    source.AcceptsDatabase = false;
                    source.AcceptsUsername = true;
                    source.AcceptsPassword = true;
                    source.AllowBrowse = false;
                    break;
                case ConnectionType.None:
                default:
                    source.AuthTypes = new List<AuthType>() { AuthType.None };
                    source.AcceptsDatabase = false;
                    source.AcceptsUsername = false;
                    source.AcceptsPassword = false;
                    source.AllowBrowse = false;
                    break;
            }
            DataAccessBase db = GetConnection(ConnectionType);
            source.IsAvailable = db.TestAvailability();
            db.Dispose();
            return source;
        }

        private static DataAccessBase GetConnection(ConnectionType ConnectionType)
        {
            DataAccessBase dab;

            switch (ConnectionType)
            {
                case ConnectionType.None:
                    dab = new NoneDataAccess();
                    break;
                case ConnectionType.DB2:
                    dab = new DB2DataAccess();
                    break;
                case ConnectionType.ODBC:
                    dab = new ODBCServerDataAccess();
                    break;
                case ConnectionType.SQLite:
                    dab = new SQLiteDataAccess();
                    break;
                case ConnectionType.SQL_Server:
                    dab = new SQLServerDataAccess();
                    break;
                case ConnectionType.Teradata:
                    dab = new TeradataDataAccess();
                    break;
                default:
                    dab = new NoneDataAccess();
                    break;
            }

            return dab;
        }
    }
}
