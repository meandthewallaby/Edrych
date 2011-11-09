using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edrych.DataAccess
{
    class NoneDataAccess : DataAccessBase
    {
        internal override System.Data.IDbConnection GetDbConnection()
        {
            throw new NotImplementedException();
        }

        internal override System.Data.IDbCommand GetDbCommand()
        {
            throw new NotImplementedException();
        }

        internal override System.Data.IDbDataParameter GetDbParameter(string Name, object Value)
        {
            throw new NotImplementedException();
        }

        internal override List<Database> GetDatabases()
        {
            throw new NotImplementedException();
        }

        internal override List<TableView> GetTables()
        {
            throw new NotImplementedException();
        }

        internal override List<TableView> GetViews()
        {
            throw new NotImplementedException();
        }

        internal override List<Column> GetColumns(string TableName)
        {
            throw new NotImplementedException();
        }

        internal override void SetDatabase(string DatabaseName)
        {
            throw new NotImplementedException();
        }

        internal override string BuildConnectionString()
        {
            throw new NotImplementedException();
        }
    }
}
