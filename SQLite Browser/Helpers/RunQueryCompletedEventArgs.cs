using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using SQLiteBrowser.DataAccess;

namespace SQLiteBrowser.Helpers
{
    public delegate void RunQueryCompletedEventHandler (object sender, RunQueryCompletedEventArgs e);

    public class RunQueryCompletedEventArgs : AsyncCompletedEventArgs
    {
        public RunQueryCompletedEventArgs(Exception error, bool cancelled, object userState) : base (error, cancelled, userState)
        {
        }

        public ResultSet Results { get; set; }
    }
}
