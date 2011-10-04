using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLiteBrowser.ViewModels
{
    public class QueryViewModel
    {
        private string _connType;
        private string _dataSource;

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
    }
}
