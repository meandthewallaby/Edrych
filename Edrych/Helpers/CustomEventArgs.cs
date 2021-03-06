﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Edrych.DataAccess;

namespace Edrych.Helpers
{
    /// <summary>Delegate to handle the RunQueryCompleted event</summary>
    delegate void RunQueryCompletedEventHandler (object sender, RunQueryCompletedEventArgs e);

    /// <summary>Event arguments which carry a ResultSet object</summary>
    class RunQueryCompletedEventArgs : AsyncCompletedEventArgs
    {
        /// <summary>Initializes the event args</summary>
        /// <param name="error">Exception for the event</param>
        /// <param name="cancelled">Whether the action was cancelled</param>
        /// <param name="userState">User state of the event</param>
        public RunQueryCompletedEventArgs(Exception error, bool cancelled, object userState) : base (error, cancelled, userState)
        {
        }

        /// <summary>ResultSet the event fires with</summary>
        public ResultSet Results { get; set; }
    }

    /// <summary>Delegate to handle a row being read</summary>
    delegate void RunQueryInitialRowsCreatedEventHandler(object sender, RunQueryInitialRowsCreatedEventArgs e);

    /// <summary>Event arguments carrying a data row</summary>
    class RunQueryInitialRowsCreatedEventArgs : EventArgs
    {
        public RunQueryInitialRowsCreatedEventArgs(DataTable Table) : base()
        {
            this.Table = Table;
        }

        public DataTable Table { get; private set; }
    }

    /// <summary>Delegate to handle the EndQuery event</summary>
    delegate void EndQueryEventHandler(object sender, EndQueryEventArgs e);

    /// <summary>Event arguments for the end of a long-running event</summary>
    class EndQueryEventArgs : EventArgs
    {
        /// <summary>Builds the event args with the flag to indicate if there was an error or not.</summary>
        /// <param name="IsError">Whether the event errorred</param>
        public EndQueryEventArgs(bool IsError)
        {
            this.HasError = IsError;
        }

        /// <summary>Boolean representing whether the event errorred.</summary>
        public bool HasError { get; set; }
    }

    /// <summary>Delegate to handle the ConnectionChanged event</summary>
    delegate void ConnectionChangedEventHandler(object sender, ConnectionChangedEventArgs e);

    /// <summary>Event arguments for the ConnectionChanged event</summary>
    class ConnectionChangedEventArgs : EventArgs
    {
        private List<Database> _databases;
        private string _selectedDatabase;

        /// <summary>Builds the arguments</summary>
        /// <param name="DatabaseList">The list of available databases</param>
        /// <param name="SelectedDB">The name of the selected database</param>
        public ConnectionChangedEventArgs(List<Database> DatabaseList, string SelectedDB)
        {
            _databases = DatabaseList;
            _selectedDatabase = SelectedDB;
        }

        /// <summary>The list of available databases</summary>
        public List<Database> Databases { get { return _databases; } }
        /// <summary>The name of the selected database</summary>
        public string SelectedDatabase { get { return _selectedDatabase; } }
    }

    /// <summary>Event Arguments from the Closing event</summary>
    class CloseEventArgs : EventArgs
    {
        private int nTabIndex = -1;

        /// <summary>Constructor to set the index of the tab being closed</summary>
        /// <param name="nTabIndex">Index of the tab being closed</param>
        public CloseEventArgs(int nTabIndex)
        {
            this.nTabIndex = nTabIndex;
        }

        /// <summary>Returns the index of the tab closing</summary>
        public int TabIndex { get { return this.nTabIndex; } }
    }
}
