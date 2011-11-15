using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Edrych.Properties;

namespace Edrych.DataAccess
{
    /// <summary>Factory that creates Data Access objects</summary>
    public class DataAccessFactory
    {
        /// <summary>Gets the default connection type from the user's settings</summary>
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

        /// <summary>Gets the data access object for the specified connection information</summary>
        /// <param name="ConnectionType">Type of database server to connect to</param>
        /// <param name="DataSource">Server to connect to</param>
        /// <param name="InitialCatalog">Initial database</param>
        /// <param name="Auth">Authentication method to use</param>
        /// <param name="Username">Username for basic authentication</param>
        /// <param name="Password">Password for basic authentication</param>
        /// <returns>DataAccessBase object for the specified connection type</returns>
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

        /// <summary>Get a list of connection types along with their properties</summary>
        /// <returns>List of ConnectionSource objects representing the available connection types</returns>
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

        /// <summary>Update a user's settings to change their default connection type</summary>
        /// <param name="ConnectionType">ConnectionType value to set as the default</param>
        public static void SetDefaultType(ConnectionType ConnectionType)
        {
            Settings.Default.DefaultConnection = ConnectionType.ToString();
            Settings.Default.Save();
        }

        /// <summary>Builds a ConnectionSource object give a connection type</summary>
        /// <param name="ConnectionType">ConnectionType value to build a ConnectionSource for</param>
        /// <returns>ConnectionSource object for the given ConnectionType</returns>
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

        /// <summary>Creates a DataAccess object for the specific connection type</summary>
        /// <param name="ConnectionType">ConnectionType value to get the data access object for</param>
        /// <returns>DataAccessBase object for the connection type</returns>
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
