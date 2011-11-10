using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;
using Edrych.Properties;

namespace Edrych.DataAccess
{
    public class SQLiteDataAccess : DataAccessBase
    {
        internal override IDbConnection GetDbConnection()
        {
            SQLiteConnection conn = new SQLiteConnection(this.ConnectionString);
            conn.Open();
            return conn;
        }

        internal override IDbCommand GetDbCommand()
        {
            return new SQLiteCommand();
        }

        internal override IDbDataParameter GetDbParameter(string Name, object Value)
        {
            return new SQLiteParameter(Name, Value);
        }

        internal override List<Database> GetDatabases()
        {
            List<Database> databases = new List<Database>();
            Database db = new Database();
            db.Name = this.SelectedDatabase;
            databases.Add(db);
            return databases;
        }

        internal override List<TableView> GetTables()
        {
            return GetTablesOrViews(DataAccessResources.SQLite_FindTables);
        }

        internal override List<TableView> GetViews()
        {
            return GetTablesOrViews(DataAccessResources.SQLite_FindViews);
        }

        private List<TableView> GetTablesOrViews(string Sql)
        {
            List<TableView> tableViews = GetDbItems<TableView>(Sql,
                (reader) =>
                {
                    TableView tv = new TableView();
                    tv.Name = reader["name"].ToString();
                    return tv;
                });

            return tableViews;
        }

        internal override List<Column> GetColumns(string TableName)
        {
            string sql = DataAccessResources.SQLite_FindColumns.Replace("@TableName", TableName);
            List<Column> cols = GetDbItems<Column>(sql,
                (reader) =>
                {
                    Column col = new Column();
                    col.Name = reader["name"].ToString();
                    col.DataType = reader["type"].ToString();
                    col.IsNullable = reader["notnull"].ToString() == "0";
                    return col;
                });
            return cols;
        }

        internal override void SetDatabase(string DatabaseName)
        {
            
        }

        internal override string BuildConnectionString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Data Source=");
            sb.Append(this.DataSource);
            sb.Append(";");
            switch (this.Authentication)
            {
                case AuthType.Basic:
                    sb.Append("Password=" + this.Password + ";");
                    break;
            }
            return sb.ToString();
        }
    }
}
