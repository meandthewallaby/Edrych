﻿using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Edrych.Properties;

namespace Edrych.DataAccess
{
    /// <summary>SQL Server Data Access object</summary>
    class SQLServerDataAccess : DataAccessBase
    {
        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetDbConnection"/></summary>
        protected override IDbConnection GetDbConnection()
        {
            SqlConnection conn = new SqlConnection(this.ConnectionString);
            conn.Open();
            return conn;
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetDbCommand"/></summary>
        protected override IDbCommand GetDbCommand()
        {
            return new SqlCommand();
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetDbParameter"/></summary>
        protected override IDbDataParameter GetDbParameter(string Name, object Value)
        {
            return new SqlParameter(Name, Value);
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetDatabases"/></summary>
        public override List<Database> GetDatabases()
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

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetTables"/></summary>
        public override List<TableView> GetTables()
        {
            return GetTablesOrViews(DataAccessResources.ANSI_FindTables);
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetViews"/></summary>
        public override List<TableView> GetViews()
        {
            return GetTablesOrViews(DataAccessResources.ANSI_FindViews);
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
                    tv.Name = reader["TABLE_SCHEMA"].ToString() + "." + reader["TABLE_NAME"].ToString();
                    return tv;
                });

            return tableViews;
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetColumns"/></summary>
        public override List<Column> GetColumns(string TableName)
        {
            this.ClearParameters();
            int dotIndex = TableName.IndexOf(".");
            string schema = "dbo";
            string table;
            if (dotIndex > 0)
            {
                schema = TableName.Substring(0, TableName.IndexOf("."));
                table = TableName.Substring(TableName.IndexOf(".") + 1);
            }
            else
            {
                table = TableName;
            }
            this.AddParameter("@SchemaName", schema);
            this.AddParameter("@TableName", table);
            List<Column> cols = GetDbItems<Column>(DataAccessResources.SQLServer_FindColumns,
                (reader) =>
                {
                    Column col = new Column();
                    col.Name = reader["name"].ToString();
                    col.DataType = reader["type"].ToString();
                    col.IsNullable = reader["IS_NULLABLE"].ToString() == "YES";
                    col.Key = (int)reader["PKCount"] > 0 ? KeyType.Primary : (int)reader["FKCount"] > 0 ? KeyType.Foreign : KeyType.None;
                    return col;
                });
            this.ClearParameters();
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

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.SetDatabase"/></summary>
        public override void SetDatabase(string DatabaseName)
        {
            string sql = DataAccessResources.SQLServer_SetDatabase.Replace("@DatabaseReplaceName", DatabaseName);
            this.ClearParameters();
            this.AddParameter("@DatabaseName", DatabaseName);
            this.ExecuteNonQuery(sql);
            this.ClearParameters();
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
                case AuthType.Integrated:
                    sb.Append("Integrated Security=SSPI;");
                    break;
                case AuthType.Basic:
                    sb.Append("User Id=" + this.Username + ";Password=" + this.Password + ";");
                    break;
            }
            return sb.ToString();
        }
    }
}
