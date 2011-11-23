using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edrych.DataAccess
{
    /// <summary>Data Access object for "None"</summary>
    class NoneDataAccess : DataAccessBase
    {
        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetDbConnection"/></summary>
        protected override System.Data.IDbConnection GetDbConnection()
        {
            throw new NotImplementedException();
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetDbCommand"/></summary>
        protected override System.Data.IDbCommand GetDbCommand()
        {
            throw new NotImplementedException();
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetDbParameter"/></summary>
        protected override System.Data.IDbDataParameter GetDbParameter(string Name, object Value)
        {
            throw new NotImplementedException();
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetDatabases"/></summary>
        public override List<Database> GetDatabases()
        {
            throw new NotImplementedException();
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetTables"/></summary>
        public override List<TableView> GetTables()
        {
            throw new NotImplementedException();
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetViews"/></summary>
        public override List<TableView> GetViews()
        {
            throw new NotImplementedException();
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetColumns"/></summary>
        public override List<Column> GetColumns(string TableName)
        {
            throw new NotImplementedException();
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.SetDatabase"/></summary>
        public override void SetDatabase(string DatabaseName)
        {
            throw new NotImplementedException();
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.BuildConnectionString"/></summary>
        protected override string BuildConnectionString()
        {
            throw new NotImplementedException();
        }
    }
}
