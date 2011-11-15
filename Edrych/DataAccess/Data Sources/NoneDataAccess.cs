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
        internal override System.Data.IDbConnection GetDbConnection()
        {
            throw new NotImplementedException();
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetDbCommand"/></summary>
        internal override System.Data.IDbCommand GetDbCommand()
        {
            throw new NotImplementedException();
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetDbParameter"/></summary>
        internal override System.Data.IDbDataParameter GetDbParameter(string Name, object Value)
        {
            throw new NotImplementedException();
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetDatabases"/></summary>
        internal override List<Database> GetDatabases()
        {
            throw new NotImplementedException();
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetTables"/></summary>
        internal override List<TableView> GetTables()
        {
            throw new NotImplementedException();
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetViews"/></summary>
        internal override List<TableView> GetViews()
        {
            throw new NotImplementedException();
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.GetColumns"/></summary>
        internal override List<Column> GetColumns(string TableName)
        {
            throw new NotImplementedException();
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.SetDatabase"/></summary>
        internal override void SetDatabase(string DatabaseName)
        {
            throw new NotImplementedException();
        }

        /// <summary><see cref="Edrych.DataAccess.DataAccessBase.BuildConnectionString"/></summary>
        internal override string BuildConnectionString()
        {
            throw new NotImplementedException();
        }
    }
}
