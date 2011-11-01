using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using SQLiteBrowser.Properties;

namespace SQLiteBrowser.DataAccess
{
    public class SQLiteDataAccess : DataAccessBase
    {
        public SQLiteDataAccess()
        {
        }

        internal override IDbConnection GetDbConnection()
        {
            this.ConnectionString = "Data Source=" + this.DataSource + (this.Password != null ? ";Password=" + this.Password : "");
            SQLiteConnection conn = new SQLiteConnection(this.ConnectionString);
            this.InitialCatalog = conn.Database;
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
            db.Name = this.InitialCatalog;
            databases.Add(db);
            return databases;
        }

        internal override List<TableView> GetTables()
        {
            List<TableView> tables = new List<TableView>();

            IDataReader reader = this.ExecuteReader(DataAccessResources.SQLite_FindTables);

            while (reader.Read())
            {
                TableView table = new TableView();
                table.Name = reader["name"].ToString();

                tables.Add(table);
            }

            reader.Close();

            return tables;
        }

        internal override List<TableView> GetViews()
        {
            List<TableView> views = new List<TableView>();

            IDataReader reader = this.ExecuteReader(DataAccessResources.SQLite_FindViews);

            while (reader.Read())
            {
                TableView view = new TableView();
                view.Name = reader["name"].ToString();

                views.Add(view);
            }

            reader.Close();

            return views;
        }

        internal override List<Column> GetColumns(string TableName)
        {
            List<Column> cols = new List<Column>();
            string sql = DataAccessResources.SQLite_FindColumns.Replace("@TableName", TableName);
            IDataReader reader = this.ExecuteReader(sql);

            while (reader.Read())
            {
                Column col = new Column();
                col.Name = reader["name"].ToString();
                col.DataType = reader["type"].ToString();
                cols.Add(col);
            }

            reader.Close();

            return cols;
        }
    }
}
