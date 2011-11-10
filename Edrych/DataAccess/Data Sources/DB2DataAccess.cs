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
        internal override System.Data.IDbConnection GetDbConnection()
        {
            DB2Connection conn = new DB2Connection(this.ConnectionString);
            conn.Open();
            return conn;
        }

        internal override System.Data.IDbCommand GetDbCommand()
        {
            return new DB2Command();
        }

        internal override System.Data.IDbDataParameter GetDbParameter(string Name, object Value)
        {
            return new DB2Parameter(Name, Value);
        }

        internal override List<Database> GetDatabases()
        {
            Database db = new Database();
            db.Name = this.SelectedDatabase;
            return new List<Database>() { db };
        }

        internal override List<TableView> GetTables()
        {
            return GetTablesOrViews(DataAccessResources.DB2_FindTables);
        }

        internal override List<TableView> GetViews()
        {
            return GetTablesOrViews(DataAccessResources.DB2_FindViews);
        }

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
