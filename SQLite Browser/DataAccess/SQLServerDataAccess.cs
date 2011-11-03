﻿using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SQLiteBrowser.Properties;

namespace SQLiteBrowser.DataAccess
{
    class SQLServerDataAccess : DataAccessBase
    {

        internal override IDbConnection GetDbConnection()
        {
            this.ConnectionString = 
                "Data Source=" + this.DataSource + ";" +
                (this.Password != null ? "Username=" + this.Username + ";Password=" + this.Password : "Integrated Security=SSPI;");
            SqlConnection conn = new SqlConnection(this.ConnectionString);
            conn.Open();
            return conn;
        }

        internal override IDbCommand GetDbCommand()
        {
            return new SqlCommand();
        }

        internal override IDbDataParameter GetDbParameter(string Name, object Value)
        {
            return new SqlParameter(Name, Value);
        }

        internal override List<Database> GetDatabases()
        {
            List<Database> databases = new List<Database>();
            IDataReader reader = this.ExecuteReader(DataAccessResources.SQLServer_FindDatabases);
            while (reader.Read())
            {
                Database db = new Database();
                db.Name = reader["name"].ToString();
                databases.Add(db);
            }
            reader.Close();
            return databases;
        }

        internal override List<TableView> GetTables()
        {
            List<TableView> tables = new List<TableView>();

            IDataReader reader = this.ExecuteReader(DataAccessResources.ANSI_FindTables);

            while (reader.Read())
            {
                TableView table = new TableView();
                table.Name = reader["TABLE_NAME"].ToString();

                tables.Add(table);
            }

            reader.Close();

            return tables;
        }

        internal override List<TableView> GetViews()
        {
            List<TableView> views = new List<TableView>();

            IDataReader reader = this.ExecuteReader(DataAccessResources.ANSI_FindViews);

            while (reader.Read())
            {
                TableView view = new TableView();
                view.Name = reader["TABLE_NAME"].ToString();

                views.Add(view);
            }

            reader.Close();

            return views;
        }

        internal override List<Column> GetColumns(string TableName)
        {
            List<Column> cols = new List<Column>();
            this.ClearParameters();
            this.AddParameter("@TableName", TableName);
            IDataReader reader = this.ExecuteReader(DataAccessResources.SQLServer_FindColumns);

            while (reader.Read())
            {
                Column col = new Column();
                col.Name = reader["name"].ToString();
                col.DataType = reader["type"].ToString();
                cols.Add(col);
            }

            reader.Close();
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
    }
}
