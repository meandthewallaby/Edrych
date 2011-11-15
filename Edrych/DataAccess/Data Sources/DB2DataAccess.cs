using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using IBM.Data.DB2;
using Edrych.Properties;

namespace Edrych.DataAccess
{
    class DB2DataAccess : DataAccessBase
    {
        /// <summary>Opens up the connection to a DB2 database</summary>
        /// <returns>DB2Connection, which inherits the IDbConnection interface</returns>
        internal override System.Data.IDbConnection GetDbConnection()
        {
            DB2Connection conn = new DB2Connection(this.ConnectionString);
            conn.Open();
            return conn;
        }

        /// <summary>Creates a new DB2Command object</summary>
        /// <returns>DB2Command, which inherits the IDbCommand interface</returns>
        internal override System.Data.IDbCommand GetDbCommand()
        {
            return new DB2Command();
        }

        /// <summary>Creates a DB2Parameter based on the Name and value passed</summary>
        /// <param name="Name">Name of the parameter to add</param>
        /// <param name="Value">Value of the parameter to add</param>
        /// <returns>DB2Parameter, which inherits the IDbDataParameter interface</returns>
        internal override System.Data.IDbDataParameter GetDbParameter(string Name, object Value)
        {
            return new DB2Parameter(Name, Value);
        }

        /// <summary>Gets the databases on the server. In the case of DB2, this is only the active database.</summary>
        /// <returns>List of Database objects representing the databases</returns>
        internal override List<Database> GetDatabases()
        {
            Database db = new Database();
            db.Name = this.SelectedDatabase;
            return new List<Database>() { db };
        }

        /// <summary>Gets the tables in a database.</summary>
        /// <returns>List of TableView objects representing the tables in the selected database</returns>
        internal override List<TableView> GetTables()
        {
            return GetTablesOrViews(DataAccessResources.DB2_FindTables);
        }

        /// <summary>Gets the views in a database.</summary>
        /// <returns>List of TableView objects representing the views in the selected database</returns>
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

        internal override void SetDatabase(string DatabaseName)
        {
            
        }

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
