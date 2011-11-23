using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;
using Edrych.Properties;

namespace Edrych.DataAccess
{
    /// <summary>SQLite Data Access object</summary>
    class SQLiteDataAccess : DataAccessBase
    {
        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetDbConnection"/></summary>
        protected override IDbConnection GetDbConnection()
        {
            SQLiteConnection conn = new SQLiteConnection(this.ConnectionString);
            conn.Open();
            return conn;
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetDbCommand"/></summary>
        protected override IDbCommand GetDbCommand()
        {
            return new SQLiteCommand();
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetDbParameter"/></summary>
        protected override IDbDataParameter GetDbParameter(string Name, object Value)
        {
            return new SQLiteParameter(Name, Value);
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetDatabases"/></summary>
        public override List<Database> GetDatabases()
        {
            List<Database> databases = new List<Database>();
            Database db = new Database();
            db.Name = this.SelectedDatabase;
            databases.Add(db);
            return databases;
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetTables"/></summary>
        public override List<TableView> GetTables()
        {
            return GetTablesOrViews(DataAccessResources.SQLite_FindTables);
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetViews"/></summary>
        public override List<TableView> GetViews()
        {
            return GetTablesOrViews(DataAccessResources.SQLite_FindViews);
        }

        /// <summary>Gets the tables or views in a database.</summary>
        /// <param name="Sql">SQL query to get the tables or views</param>
        /// <returns>List of TableView objects returned by the passed SQL</returns>
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

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetColumns"/></summary>
        public override List<Column> GetColumns(string TableName)
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

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.SetDatabase"/></summary>
        public override void SetDatabase(string DatabaseName)
        {
            
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.BuildConnectionString"/></summary>
        protected override string BuildConnectionString()
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
