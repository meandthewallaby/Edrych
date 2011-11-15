using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using IBM.Data.DB2;
using Edrych.Properties;

namespace Edrych.DataAccess
{
    /// <summary>DB2 Data Access object</summary>
    class DB2DataAccess : DataAccessBase
    {
        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetDbConnection"/></summary>
        internal override System.Data.IDbConnection GetDbConnection()
        {
            DB2Connection conn = new DB2Connection(this.ConnectionString);
            conn.Open();
            return conn;
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetDbCommand"/></summary>
        internal override System.Data.IDbCommand GetDbCommand()
        {
            return new DB2Command();
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetDbParameter"/></summary>
        internal override System.Data.IDbDataParameter GetDbParameter(string Name, object Value)
        {
            return new DB2Parameter(Name, Value);
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetDatabases"/></summary>
        internal override List<Database> GetDatabases()
        {
            Database db = new Database();
            db.Name = this.SelectedDatabase;
            return new List<Database>() { db };
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetTables"/></summary>
        internal override List<TableView> GetTables()
        {
            return GetTablesOrViews(DataAccessResources.DB2_FindTables);
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetViews"/></summary>
        internal override List<TableView> GetViews()
        {
            return GetTablesOrViews(DataAccessResources.DB2_FindViews);
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
                    tv.Name = reader["SCHEMA"].ToString() + "." + reader["NAME"].ToString();
                    return tv;
                });
            return tableViews;
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetColumns"/></summary>
        internal override List<Column> GetColumns(string TableName)
        {
            List<Column> cols = new List<Column>();
            int dotIndex = TableName.IndexOf('.');
            if (dotIndex > 0 && dotIndex < TableName.Length - 1)
            {
                this.ClearParameters();
                this.AddParameter("@SchemaName", TableName.Substring(0, dotIndex));
                this.AddParameter("@TableName", TableName.Substring(dotIndex + 1));
                cols = GetDbItems<Column>(DataAccessResources.DB2_FindColumns,
                    (reader) =>
                    {
                        Column col = new Column();
                        col.Name = reader["COLNAME"].ToString();
                        col.DataType = reader["COLTYPE"].ToString();
                        col.IsNullable = reader["NULLS"].ToString() == "Y";
                        return col;
                    });
                this.ClearParameters();
            }

            return cols;
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.SetDatabase"/></summary>
        internal override void SetDatabase(string DatabaseName)
        {
            
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.BuildConnectionString"/></summary>
        internal override string BuildConnectionString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Server=" + this.DataSource + ";");
            sb.Append("Database=" + this.InitialCatalog + ";");
            sb.Append("UID=" + this.Username + ";");
            sb.Append("PWD=" + this.Password + ";");
            return sb.ToString();
        }
    }
}
