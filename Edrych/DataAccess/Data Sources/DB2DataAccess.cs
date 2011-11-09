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
            List<TableView> tables = new List<TableView>();
            IDataReader reader = this.ExecuteReader(DataAccessResources.DB2_FindTables);
            while (reader.Read())
            {
                TableView table = new TableView();
                table.Name = reader["TABSCHEMA"].ToString() + "." + reader["TABNAME"].ToString();
                tables.Add(table);
            }
            reader.Close();
            return tables;
        }

        internal override List<TableView> GetViews()
        {
            List<TableView> views = new List<TableView>();
            IDataReader reader = this.ExecuteReader(DataAccessResources.DB2_FindViews);
            while (reader.Read())
            {
                TableView table = new TableView();
                table.Name = reader["VIEWSCHEMA"].ToString() + "." + reader["VIEWNAME"].ToString();
                views.Add(table);
            }
            reader.Close();
            return views;
        }

        internal override List<Column> GetColumns(string TableName)
        {
            List<Column> cols = new List<Column>();
            int dotIndex = TableName.IndexOf('.');
            this.ClearParameters();
            if (dotIndex > 0 && dotIndex < TableName.Length - 1)
            {
                this.AddParameter("@SchemaName", TableName.Substring(0, dotIndex));
                this.AddParameter("@TableName", TableName.Substring(dotIndex + 1));
                IDataReader reader = this.ExecuteReader(DataAccessResources.DB2_FindColumns);

                while (reader.Read())
                {
                    Column col = new Column();
                    col.Name = reader["COLNAME"].ToString();
                    col.DataType = reader["COLTYPE"].ToString();
                    col.IsNullable = reader["NULLS"].ToString() == "Y";
                    cols.Add(col);
                }

                reader.Close();
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
