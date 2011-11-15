using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Teradata.Client.Provider;
using Edrych.Properties;

namespace Edrych.DataAccess
{
    /// <summary>Teradata Data Access object</summary>
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

        private string _activeDatabase;

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetDbConnection"/></summary>
        internal override System.Data.IDbConnection GetDbConnection()
        {
            TdConnection conn = new TdConnection(this.ConnectionString);
            conn.Open();
            return conn;
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetDbCommand"/></summary>
        internal override System.Data.IDbCommand GetDbCommand()
        {
            return new TdCommand();
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetDbParameter"/></summary>
        internal override System.Data.IDbDataParameter GetDbParameter(string Name, object Value)
        {
            return new TdParameter(Name, Value);
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetDatabases"/></summary>
        internal override List<Database> GetDatabases()
        {
            List<Database> databases = GetDbItems<Database>(DataAccessResources.Teradata_FindDatabases,
                (reader) =>
                {
                    Database db = new Database();
                    db.Name = reader["DatabaseName"].ToString();
                    return db;
                });
            return databases;
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetTables"/></summary>
        internal override List<TableView> GetTables()
        {
            return GetTablesOrViews(DataAccessResources.Teradata_FindTables);
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetViews"/></summary>
        internal override List<TableView> GetViews()
        {
            return GetTablesOrViews(DataAccessResources.Teradata_FindViews);
        }

        /// <summary>Gets the tables or views in a database.</summary>
        /// <param name="Sql">SQL query to get the tables or views</param>
        /// <returns>List of TableView objects returned by the passed SQL</returns>
        private List<TableView> GetTablesOrViews(string Sql)
        {
            this.ClearParameters();
            this.AddParameter("@DatabaseName", _activeDatabase);
            List<TableView> tableViews = GetDbItems<TableView>(Sql,
                (reader) =>
                {
                    TableView tv = new TableView();
                    tv.Name = reader["TableName"].ToString();
                    return tv;
                });
            this.ClearParameters();
            return tableViews;
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetColumns"/></summary>
        internal override List<Column> GetColumns(string TableName)
        {
            string sql = DataAccessResources.Teradata_FindColumns.Replace("@DatabaseName", _activeDatabase).Replace("@TableName", TableName);
            List<Column> columns = GetDbItems<Column>(sql,
                (reader) =>
                {
                    Column col = new Column();
                    col.Name = reader["Column Name"].ToString().Trim();
                    string dataType = COLUMN_TYPE_LOOKUP[reader["Type"].ToString().Trim()];
                    string maxLength = reader["Max Length"].ToString().Trim();
                    string scale = reader["Decimal Total Digits"].ToString().Trim();
                    string precision = reader["Decimal Fractional Digits"].ToString().Trim();
                    if (dataType.Contains("CHAR") && !string.IsNullOrEmpty(maxLength))
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

                    return col;
                });
            return columns;
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.SetDatabase"/></summary>
        internal override void SetDatabase(string DatabaseName)
        {
            _activeDatabase = DatabaseName;
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.BuildConnectionString"/></summary>
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
