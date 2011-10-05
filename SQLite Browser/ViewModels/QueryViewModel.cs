using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLiteBrowser.DataAccess;

namespace SQLiteBrowser.ViewModels
{
    public class QueryViewModel
    {
        private string _connType;
        private string _dataSource;
        private DataAccessBase _dab;

        public QueryViewModel()
        {
        }

        public string ConnectionType
        {
            get { return _connType; }
            set { _connType = value; }
        }

        public string DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; }
        }

        public DataAccessBase Data
        {
            get { return _dab; }
        }

        public void InitiatlizeData()
        {
            DataAccess.ConnectionType ct = (DataAccess.ConnectionType)Enum.Parse(typeof(DataAccess.ConnectionType), this.ConnectionType);
            //May need to build the connection string a little differently...
            _dab = DataAccessFactory.GetDataAccess(ct, "Data Source=" + this.DataSource);
        }
    }
}
