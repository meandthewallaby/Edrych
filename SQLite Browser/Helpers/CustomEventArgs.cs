using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using SQLiteBrowser.DataAccess;

namespace SQLiteBrowser.Helpers
{
    public delegate void RunQueryCompletedEventHandler (object sender, CustomEventArgs e);

    public class CustomEventArgs : AsyncCompletedEventArgs
    {
        public CustomEventArgs(Exception error, bool cancelled, object userState) : base (error, cancelled, userState)
        {
        }

        public ResultSet Results { get; set; }
    }

    public delegate void EndQueryEventHandler(object sender, EndQueryEventArgs e);

    public class EndQueryEventArgs : EventArgs
    {
        public EndQueryEventArgs(bool IsError)
        {
            this.HasError = IsError;
        }

        public bool HasError { get; set; }
    }
}
