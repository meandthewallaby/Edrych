using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Teradata.Client.Provider;
using Edrych.Properties;

namespace Edrych.DataAccess
{
    class TeradataDataAccess : DataAccessBase
    {
        private Dictionary<string, string> COLUMN_TYPE_LOOKUP = new Dictionary<string, string>()
        {
            {"AT", "TIME"},
            {"BF", "BYTE"},
            {"BO", "BLOB"},
            {"BV", "VARBYTE"},
            {"CF", "CHAR"},
            {"CO", "CLOB"},
            {"CV", "VARCHAR"},
            {"D", "DECIMAL"},
            {"DA", "DATE"},
            {"DH", "INTERVAL DAY TO HOUR"},
            {"DM", "INTERVAL DAY TO MINUTE"},
            {"DS", "INTERVAL DAY TO SECOND"},
            {"DY", "INTERVAL DAY"},
            {"F", "FLOAT"},
            {"HM", "INTERVAL HOUR TO MINUTE"},
            {"HR", "INTERVAL HOUR"},
            {"HS", "INTERVAL HOUR TO SECOND"},
            {"I1", "BYTEINT"},
            {"I2", "SMALLINT"},
            {"I8", "BIGINT"},
            {"I", "INTEGER"},
            {"MI", "INTERVAL MINUTE"},
            {"MO", "INTERVAL MONTH"},
            {"MS", "INTERVAL MINUTE TO SECOND"},
            {"PD", "PERIOD(DATE)"},
            {"PM", "PERIOD (TIMESTAMP(n) WITH TIME ZONE)"},
            {"PS", "PERIOD (TIMESTAMP(n))"},
            {"PT", "PERIOD (TIME(n))"},
            {"PZ", "PERIOD (TIME(n))"},
            {"SC", "INTERVAL SECOND"},
            {"SZ", "TIMESTAMP WITH TIME ZONE"},
            {"TS", "TIMESTAMP"},
            {"TZ", "TIME WITH TIME ZONE"},
            {"YM", "INTERVAL YEAR TO MONTH"},
            {"YR", "INTERVAL YEAR"},
            {"UT", "UDT Type"}
        };

        private string _database;

        private string ActiveDatabase
        {
            get { return _database; }
        }


        internal override System.Data.IDbConnection GetDbConnection()
        {
            TdConnection conn = new TdConnection(this.ConnectionString);
            conn.Open();
            return conn;
        }

        internal override System.Data.IDbCommand GetDbCommand()
        {
            return new TdCommand();
        }

        internal override System.Data.IDbDataParameter GetDbParameter(string Name, object Value)
        {
            return new TdParameter(Name, Value);
        }

        internal override List<Database> GetDatabases()
        {
            List<Database> databases = new List<Database>();
            IDataReader reader = this.ExecuteReader(DataAccessResources.Teradata_FindDatabases);
            while (reader.Read())
            {
                Database db = new Database();
                db.Name = reader["DatabaseName"].ToString();
                databases.Add(db);
            }
            reader.Close();
            return databases;
        }

        internal override List<TableView> GetTables()
        {
            List<TableView> tables = new List<TableView>();
            this.ClearParameters();
            this.AddParameter("@DatabaseName", this.ActiveDatabase);
            IDataReader reader = this.ExecuteReader(DataAccessResources.Teradata_FindTables);

            while (reader.Read())
            {
                TableView table = new TableView();
                table.Name = reader["TableName"].ToString();

                tables.Add(table);
            }

            reader.Close();
            this.ClearParameters();
            return tables;
        }

        internal override List<TableView> GetViews()
        {
            List<TableView> views = new List<TableView>();
            this.ClearParameters();
            this.AddParameter("@DatabaseName", this.ActiveDatabase);
            IDataReader reader = this.ExecuteReader(DataAccessResources.Teradata_FindViews);

            while (reader.Read())
            {
                TableView view = new TableView();
                view.Name = reader["TableName"].ToString();

                views.Add(view);
            }

            reader.Close();
            this.ClearParameters();
            return views;
        }

        internal override List<Column> GetColumns(string TableName)
        {
            List<Column> columns = new List<Column>();
            this.ClearParameters();
            string sql = DataAccessResources.Teradata_FindColumns.Replace("@DatabaseName", this.ActiveDatabase).Replace("@TableName", TableName);
            IDataReader reader = this.ExecuteReader(sql);

            while (reader.Read())
            {
                Column col = new Column();
                col.Name = reader["Column Name"].ToString().Trim();
                //TODO: Fix this to actually have a column type!
                string dataType = COLUMN_TYPE_LOOKUP[reader["Type"].ToString().Trim()];
                string maxLength = reader["Max Length"].ToString().Trim();
                string scale = reader["Decimal Total Digits"].ToString().Trim();
                string precision = reader["Decimal Fractional Digits"].ToString().Trim();
                if(dataType.Contains("CHAR") && !string.IsNullOrEmpty(maxLength))
                {
                    col.DataType = dataType + " (" + maxLength + ")";
                }
                else if (dataType == "DECIMAL" && !string.IsNullOrEmpty(scale) && !string.IsNullOrEmpty(precision))
                {
                    col.DataType = dataType + " (" + scale + ", " + precision + ")";
                }
                else
                {
                    col.DataType = dataType;
                }
                col.IsNullable = reader["Nullable"].ToString() == "Y";

                columns.Add(col);
            }

            reader.Close();
            this.ClearParameters();
            return columns;
        }

        internal override void SetDatabase(string DatabaseName)
        {
            _database = DatabaseName;
        }

        internal override string BuildConnectionString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Data Source=" + this.DataSource + ";");
            sb.Append("User ID=" + this.Username + ";");
            sb.Append("Password=" + this.Password + ";");
            return sb.ToString();
        }
    }
}
