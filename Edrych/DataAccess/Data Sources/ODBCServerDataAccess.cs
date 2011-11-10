using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Text;
using Edrych.Properties;

namespace Edrych.DataAccess
{
    class ODBCServerDataAccess : DataAccessBase
    {
        //TODO: Change up Connection String and SQL to fall in line w/ ODBC connections
        internal override IDbConnection GetDbConnection()
        {
            OdbcConnection conn = new OdbcConnection(this.ConnectionString);
            conn.Open();
            return conn;
        }

        internal override IDbCommand GetDbCommand()
        {
            return new OdbcCommand();
        }

        internal override IDbDataParameter GetDbParameter(string Name, object Value)
        {
            return new OdbcParameter(Name, Value);
        }

        internal override List<Database> GetDatabases()
        {
            List<Database> databases = GetDbItems<Database>(DataAccessResources.SQLServer_FindDatabases,
                (reader) =>
                {
                    Database db = new Database();
                    db.Name = reader["name"].ToString();
                    return db;
                });
            return databases;
        }

        internal override List<TableView> GetTables()
        {
            return GetTablesOrViews(DataAccessResources.ANSI_FindTables);
        }

        internal override List<TableView> GetViews()
        {
            return GetTablesOrViews(DataAccessResources.ANSI_FindViews);
        }

        private List<TableView> GetTablesOrViews(string Sql)
        {
            List<TableView> tableViews = GetDbItems<TableView>(Sql,
                (reader) =>
                {
                    TableView tv = new TableView();
                    tv.Name = reader["TABLE_NAME"].ToString();
                    return tv;
                });

            return tableViews;
        }

        internal override List<Column> GetColumns(string TableName)
        {
            this.ClearParameters();
            this.AddParameter("@TableName", TableName);
            List<Column> cols = GetDbItems<Column>(DataAccessResources.SQLServer_FindColumns,
                (reader) =>
                {
                    Column col = new Column();
                    col.Name = reader["name"].ToString();
                    col.DataType = reader["type"].ToString();
                    col.IsNullable = reader["IS_NULLABLE"].ToString() == "YES";
                    return col;
                });
            this.ClearParameters();
            return cols;
        }

        internal override void SetDatabase(string DatabaseName)
        {
            string sql = DataAccessResources.SQLServer_SetDatabase.Replace("@DatabaseReplaceName", DatabaseName);
            this.ClearParameters();
            this.AddParameter("@DatabaseName", DatabaseName);
            this.ExecuteNonQuery(sql);
            this.ClearParameters();
        }

        internal override string BuildConnectionString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Driver={ODBC};Server=");
            sb.Append(this.DataSource);
            sb.Append(";");
            switch (this.Authentication)
            {
                case AuthType.Integrated:
                    sb.Append("Trusted_Connection=yes;");
                    break;
                case AuthType.Basic:
                    sb.Append("Uid=" + this.Username + ";Pwd=" + this.Password + ";");
                    break;
            }
            return sb.ToString();
        }
    }
}
