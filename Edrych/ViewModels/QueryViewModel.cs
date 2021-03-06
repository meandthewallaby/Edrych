﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Edrych.DataAccess;
using Edrych.Dialogs;
using Edrych.Helpers;

namespace Edrych.ViewModels
{
    /// <summary>ViewModel for the query and results window</summary>
    class QueryViewModel : INotifyPropertyChanged, IDisposable
    {
        #region Private/Global Variables

        private DataAccessBase _dab;
        private ServerBrowserViewModel _browser;
        private ResultSet _results = new ResultSet();
        private BindingSource _dataBinding = new BindingSource();
        private string _messages = string.Empty;
        private List<Database> _databases;
        private string _oldDatabase;
        AsyncOperation _async = AsyncOperationManager.CreateOperation(null);

        private string _fileName = string.Empty;
        private string _safeFileName = "New Query";
        private bool _isSaved = false;

        private const string FILE_TYPES = "SQL file (*.sql)|*.sql|Text file (*.txt)|*.txt|All files (*.*)|*.*";

        #endregion

        #region Constructor(s)

        /// <summary>Initializes the query with the reference to the browser tree</summary>
        /// <param name="Browser">ViewModel of the browser tree</param>
        public QueryViewModel(ref ServerBrowserViewModel Browser)
        {
            _dab = DataAccessFactory.GetDataAccess(
                Browser.ActiveConnection.ConnectionType,
                Browser.ActiveConnection.DataSource,
                Browser.ActiveConnection.SelectedDatabase,
                Browser.ActiveConnection.Authentication,
                Browser.ActiveConnection.Username,
                Browser.ActiveConnection.Password
                );
            _dab.BuildKeywords();
            _browser = Browser;
        }

        #endregion

        #region Public Properties
        
        /// <summary>Returns the query's data access object</summary>
        public DataAccessBase Data
        {
            get { return _dab; }
        }

        /// <summary>Returns the query's results object</summary>
        public ResultSet Results
        {
            get { return _results; }
        }

        /// <summary>Returns the binding source which is used to bind the results to the UI</summary>
        public BindingSource DataBinding
        {
            get { return _dataBinding; }
            set { _dataBinding = value; }
        }

        /// <summary>Returns the messages of the results. Used for binding to the UI</summary>
        public string Messages
        {
            get { return _messages; }
        }

        /// <summary>Returns the safe file name of the query</summary>
        public string SafeFileName
        {
            get { return _safeFileName; }
        }

        /// <summary>Whether or not the query's been saved</summary>
        public bool IsSaved
        {
            get { return _isSaved; }
            set { _isSaved = value; }
        }

        /// <summary>Returns the list of available databases in the connection</summary>
        public List<Database> Databases
        {
            get { return _databases; }
        }

        #endregion

        #region Public Methods

        /// <summary>Initialize the query</summary>
        /// <param name="OpenQuery">Whether or not to open an existing query</param>
        /// <returns>String representing the initial text of the query</returns>
        public string InitQuery(bool OpenQuery)
        {
            string queryText = string.Empty;

            if (OpenQuery)
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Multiselect = false;
                open.Filter = FILE_TYPES;

                if (open.ShowDialog() == DialogResult.OK)
                {
                    this._fileName = open.FileName;
                    this._safeFileName = open.SafeFileName;
                    FileStream query = new FileStream(this._fileName, FileMode.Open, FileAccess.Read);
                    StreamReader reader = new StreamReader(query);
                    queryText = reader.ReadToEnd();
                    reader.Close();
                    reader.Dispose();
                    query.Close();
                    query.Dispose();

                    this._isSaved = true;
                }
            }

            if (this._dab == null)
            {
                ConnectDialog cd = new ConnectDialog();
                cd.ShowDialog();
                this._dab = cd.DataAccess;
            }

            this._databases = this._dab.GetDatabases();

            return queryText;
        }

        /// <summary>Runs a query</summary>
        /// <param name="Query">Text of the query to run</param>
        public void RunQuery(string Query)
        {
            App.IsStopQueryEnabled = true;
            DataTable oldTable = _results.Data;
            _results.Data = new DataTable();
            oldTable.Dispose();
            _results.OnPropertyChanged("Data");
            this.RunQueryAsync(Query);
        }

        /// <summary>Cancels the running query</summary>
        public void CancelQuery()
        {
            lock (_dab)
            {
                _dab.Cancel();
            }
        }

        /// <summary>Save the query</summary>
        /// <param name="Query">Text to save</param>
        /// <param name="IsSaveAs">Whether or not to save as</param>
        public void SaveQuery(string Query, bool IsSaveAs)
        {
            if(string.IsNullOrEmpty(this._fileName) || IsSaveAs)
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.OverwritePrompt = true;
                saveDialog.DefaultExt = "sql";
                saveDialog.AddExtension = true;
                saveDialog.Filter = FILE_TYPES;

                if(saveDialog.ShowDialog() == DialogResult.OK)
                {
                    this._fileName = saveDialog.FileName;
                    this._safeFileName = this._fileName.Substring(this._fileName.LastIndexOf('\\')+1);
                    CommitSave(Query);
                }
            }
            else if (string.IsNullOrEmpty(this._fileName) == false)
            {
                CommitSave(Query);
            }
        }

        /// <summary>Connects the query to a data source</summary>
        public void Connect()
        {
            this.Disconnect();
            this.InitQuery(false);
        }

        /// <summary>Disconnects the query from a data source</summary>
        public void Disconnect()
        {
            if (this._dab != null)
            {
                this._dab.Dispose();
                this._dab = null;
            }
        }

        /// <summary>Sets the active database of the query</summary>
        /// <param name="DatabaseName">Name of the database to select</param>
        public void SetDatabase(string DatabaseName)
        {
            this.Data.SetDatabase(DatabaseName);
        }

        /// <summary>Disposes of the query</summary>
        public void Dispose()
        {
            if (_dataBinding != null)
                _dataBinding.Dispose();
            if (_results != null)
                _results.Dispose();
            if (_dab != null)
                _dab.Dispose();
        }

        #endregion

        #region Events

        /// <summary>Event to fire when the query begins</summary>
        public event EventHandler BeginQuery;
        /// <summary>Event to fire when the query ends</summary>
        public event EndQueryEventHandler EndQuery;
        /// <summary>INotifyPropertyChanged interface event to notify data bound properties of changes</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Triggers the BeginQuery event</summary>
        private void OnBeginQuery()
        {
            if (BeginQuery != null)
                BeginQuery(this, new EventArgs());
        }

        /// <summary>Triggers the EndQuery event</summary>
        /// <param name="IsError">Whether or not there's an error</param>
        private void OnEndQuery(bool IsError)
        {
            if (EndQuery != null)
                EndQuery(this, new EndQueryEventArgs(IsError));
        }

        /// <summary>Triggers the PropertyChanged event</summary>
        /// <param name="Property">Name of the property to notify of the change</param>
        private void NotifyPropertyChanged(string Property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
            }
        }

        #endregion

        #region Private Methods

        /// <summary>Saves the query</summary>
        /// <param name="Query">Query to save</param>
        private void CommitSave(string Query)
        {
            FileStream saveFile = new FileStream(this._fileName, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(saveFile);
            writer.Write(Query);
            writer.Close();
            writer.Dispose();
            saveFile.Close();
            saveFile.Dispose();
            this._isSaved = true;
        }

        #endregion

        #region Private/Protected Async

        /// <summary>Delegate to run the query</summary>
        /// <param name="Query">Query to run</param>
        /// <returns>ResultSet that is the result of the given query</returns>
        private delegate ResultSet RunQueryDelegate(string Query);

        /// <summary>Event that signals the query is complete</summary>
        private event RunQueryCompletedEventHandler RunQueryCompleted;

        /// <summary>Triggers the RunQueryCompleted event</summary>
        protected virtual void OnRunQueryCompleted(RunQueryCompletedEventArgs e)
        {
            if (RunQueryCompleted != null)
            {
                RunQueryCompleted(this, e);
            }
        }

        /// <summary>Begins the async process to run the query</summary>
        /// <param name="Query">Query to execute</param>
        private void RunQueryAsync(string Query)
        {
            _async = AsyncOperationManager.CreateOperation(null);
            RunQueryCompleted += this.RunQuery_Completed;
            _dab.RunQueryRowCreated += this.RunQuery_InitialRowsCreatedAsync;
            RunQueryDelegate dl = new RunQueryDelegate(RunQueryWorker);
            
            _oldDatabase = this.Data.SelectedDatabase;

            OnBeginQuery();
            IAsyncResult ar = dl.BeginInvoke(Query, new AsyncCallback(RunQueryCallback), _async);
        }

        /// <summary>Worker function which actually runs the query</summary>
        /// <param name="Query">Query to execute</param>
        /// <returns>ResultSet that is the result of the given query</returns>
        private ResultSet RunQueryWorker(string Query)
        {
            ResultSet res = null;
            if (_dab == null)
            {
                res = new ResultSet();
                res.Messages = "No active connection";
            }

            if (!string.IsNullOrEmpty(Query) && res == null)
            {
                res = _dab.GetDataSet(Query, ref _browser);
            }

            return res;
        }

        /// <summary>Callback method for the async thread</summary>
        /// <param name="ar">Async result object</param>
        private void RunQueryCallback(IAsyncResult ar)
        {
            RunQueryDelegate dl = (RunQueryDelegate)((System.Runtime.Remoting.Messaging.AsyncResult)ar).AsyncDelegate;
            AsyncOperation async = (AsyncOperation)ar.AsyncState;
            ResultSet results;
            RunQueryCompletedEventArgs completedArgs = new RunQueryCompletedEventArgs(null, false, null);

            try
            {
                results = dl.EndInvoke(ar);
                completedArgs.Results = results;
            }
            catch(Exception ex)
            {
                completedArgs = new RunQueryCompletedEventArgs(ex, false, null);
            }
            finally
            {
                async.PostOperationCompleted(delegate(object e) { OnRunQueryCompleted((RunQueryCompletedEventArgs)e); }, completedArgs);
            }
        }

        /// <summary>Sets all the properties after the query completes</summary>
        private void RunQuery_Completed(object sender, RunQueryCompletedEventArgs e)
        {
            bool isError = true;
            if (e.Error == null)
            {
                _results.Data = e.Results.Data;
                _results.Messages = e.Results.Messages;
                isError = false;
            }
            else
            {
                _results.Messages = e.Error.Message;
            }

            if (this.Data != null && this.Data.SelectedDatabase != _oldDatabase)
            {
                App.OnDatabaseChanged(this, new ConnectionChangedEventArgs(null, this.Data.SelectedDatabase));
            }

            _results.OnPropertyChanged("Data");
            _results.OnPropertyChanged("Messages");
            _dab.RunQueryRowCreated -= this.RunQuery_InitialRowsCreatedAsync;
            RunQueryCompleted -= this.RunQuery_Completed;
            App.IsStopQueryEnabled = false;
            OnEndQuery(isError);
        }

        private void LoadRemainingRows(DataTable table)
        {
            for (int i = _results.Data.Rows.Count; i < table.Rows.Count; i++)
            {
                _results.Data.ImportRow(table.Rows[i]);
            }
        }

        #endregion

        #region Private/Protected Background Worker

        private void RunQuery_InitialRowsCreatedAsync(object sender, RunQueryInitialRowsCreatedEventArgs e)
        {
            _async.Post(new System.Threading.SendOrPostCallback(this.RunQuery_InitialRowsCreated), e.Table);
        }

        private void RunQuery_InitialRowsCreated(object data)
        {
            _results.Data = (DataTable)data;
            _results.OnPropertyChanged("Data");
        }

        #endregion
    }
}
