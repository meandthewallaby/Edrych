using System;
using System.Data;

namespace Edrych.DataAccess
{
    public class ResultSet : IDisposable
    {
        private DataTable _dt;
        private string _messages = string.Empty;

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

        public void Dispose()
        {
            if (_dt != null)
                _dt.Dispose();
        }
    }
}
