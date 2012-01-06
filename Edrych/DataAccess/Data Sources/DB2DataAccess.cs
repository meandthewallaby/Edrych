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
        protected override System.Data.IDbConnection GetDbConnection()
        {
            DB2Connection conn = new DB2Connection(this.ConnectionString);
            conn.Open();
            return conn;
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetDbCommand"/></summary>
        protected override System.Data.IDbCommand GetDbCommand()
        {
            return new DB2Command();
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetDbParameter"/></summary>
        protected override System.Data.IDbDataParameter GetDbParameter(string Name, object Value)
        {
            return new DB2Parameter(Name, Value);
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetDatabases"/></summary>
        public override List<Database> GetDatabases()
        {
            Database db = new Database();
            db.Name = this.SelectedDatabase;
            return new List<Database>() { db };
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetTables"/></summary>
        public override List<TableView> GetTables()
        {
            return GetTablesOrViews(DataAccessResources.DB2_FindTables);
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetViews"/></summary>
        public override List<TableView> GetViews()
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
        public override List<Column> GetColumns(string TableName)
        {
            List<Column> cols = new List<Column>();
            if (AddTableParameters(TableName))
            {
                cols = GetDbItems<Column>(DataAccessResources.DB2_FindColumns,
                    (reader) =>
                    {
                        Column col = new Column();
                        col.Name = reader["COLNAME"].ToString();
                        col.DataType = reader["COLTYPE"].ToString();
                        col.IsNullable = reader["NULLS"].ToString() == "Y";
                        col.Key = (int)reader["IsPrimaryKey"] > 0 ? KeyType.Primary : (int)reader["FKCount"] > 0 ? KeyType.Foreign : KeyType.None;
                        return col;
                    });
                this.ClearParameters();
            }

            return cols;
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetKeys"/></summary>
        public override List<Key> GetKeys(string TableName)
        {
            List<Key> keys = new List<Key>();
            if (AddTableParameters(TableName))
            {
                keys = GetDbItems<Key>(DataAccessResources.DB2_FindKeys,
                    (reader) =>
                    {
                        Key myKey = new Key();
                        myKey.Name = reader["CONSTNAME"].ToString();
                        int myType = (int)reader["KeyType"];
                        myKey.Type = myType == 1 ? KeyType.Primary : myType == 2 ? KeyType.Foreign : KeyType.None;
                        return myKey;
                    });
            }
            return keys;
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.BuildConnectionString"/></summary>
        protected override string BuildConnectionString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Server=" + this.DataSource + ";");
            sb.Append("Database=" + this.InitialCatalog + ";");
            sb.Append("UID=" + this.Username + ";");
            sb.Append("PWD=" + this.Password + ";");
            return sb.ToString();
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.BuildKeywords"/></summary>
        public override void BuildKeywords()
        {
            //Keywords loaded from http://publib.boulder.ibm.com/infocenter/db2luw/v9/index.jsp?topic=%2Fcom.ibm.db2.udb.admin.doc%2Fdoc%2Fr0001095.htm
            LoadKeywords(DataAccessResources.DB2_Keywords, this._keywords);
            LoadKeywords(DataAccessResources.ANSI_Operators, this._operators);
            LoadKeywords(DataAccessResources.DB2_Functions, this._functions);
        }

        private bool AddTableParameters(string TableName)
        {
            bool ret = false;
            int dotIndex = TableName.IndexOf('.');
            if (dotIndex > 0 && dotIndex < TableName.Length - 1)
            {
                this.ClearParameters();
                this.AddParameter("@SchemaName", TableName.Substring(0, dotIndex));
                this.AddParameter("@TableName", TableName.Substring(dotIndex + 1));
                ret = true;
            }
            return ret;
        }
    }
}
