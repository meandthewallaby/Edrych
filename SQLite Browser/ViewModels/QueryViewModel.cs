using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SQLiteBrowser.DataAccess;

namespace SQLiteBrowser.ViewModels
{
    public class QueryViewModel
    {
        private string _connType;
        private string _dataSource;
        private DataAccessBase _dab;
        private ResultSet _results;
        private BindingSource _dataBinding;

        public QueryViewModel()
        {
            _results = new ResultSet();
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

        public ResultSet Results
        {
            get { return _results; }
        }

        public BindingSource DataBinding
        {
            get { return _dataBinding; }
            set { _dataBinding = value; }
        }

        public void InitiatlizeData()
        {
            DataAccess.ConnectionType ct = (DataAccess.ConnectionType)Enum.Parse(typeof(DataAccess.ConnectionType), this.ConnectionType);
            //May need to build the connection string a little differently...
            _dab = DataAccessFactory.GetDataAccess(ct, "Data Source=" + this.DataSource);
        }

        public void RunQuery(string Query)
        {
            if (!string.IsNullOrEmpty(Query))
            {
                _results = _dab.GetDataSet(Query);
                _dataBinding.DataSource = _results.Data;
            }
        }
    }
}
