using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace SQLiteBrowser.DataAccess
{
    public class ResultSet
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DataTable _dt;
        private string _messages;

        public ResultSet()
        {
            _dt = new DataTable();
        }

        public DataTable Data
        {
            get { return _dt; }
        }

        public string Messages
        {
            get { return _messages; }
            set { _messages = value; }
        }
    }
}
