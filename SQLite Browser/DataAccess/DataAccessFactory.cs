using SQLiteBrowser.Properties;

namespace SQLiteBrowser.DataAccess
{
    public enum ConnectionType
    {
        None,
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
                case ConnectionType.SQLServer:
                    dab = new SQLServerDataAccess();
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
