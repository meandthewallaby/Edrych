using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
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
                    col.Key = int.Parse(reader["pk"].ToString()) > 0 ? KeyType.Primary : KeyType.None;
                    return col;
                });
            List<string> fks = GetDbItems<string>(DataAccessResources.SQLite_FindForeignKeys.Replace("@TableName", TableName),
                (reader) =>
                {
                    return reader["from"].ToString();
                });

            foreach(Column col in cols.Where(c => c.Key != KeyType.Primary && fks.Contains(c.Name)))
            {
                col.Key = KeyType.Foreign;
            }

            return cols;
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetKeys"/></summary>
        public override List<Key> GetKeys(string TableName)
        {
            List<Key> keys = new List<Key>();
            List<Column> cols = this.GetColumns(TableName);
            foreach (Column col in cols.Where(c => c.Key != KeyType.None).OrderByDescending(c => c.Key.ToString()).ThenBy(c => c.Name))
            {
                keys.Add(new Key() { Name = col.Name, Type = col.Key });
            }
            return keys;
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

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.BuildKeywords"/></summary>
        public override void BuildKeywords()
        {
            //Keywords loaded from http://www.sqlite.org/lang_keywords.html
            LoadKeywords(DataAccessResources.SQLite_Keywords, this._keywords);
            LoadKeywords(DataAccessResources.ANSI_Operators, this._operators);
        }
    }
}
