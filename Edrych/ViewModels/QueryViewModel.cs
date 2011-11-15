using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Edrych.DataAccess;
using Edrych.Dialogs;
using Edrych.Helpers;

namespace Edrych.ViewModels
{
    public class QueryViewModel : INotifyPropertyChanged, IDisposable
    {
        #region Private/Global Variables

        private DataAccessBase _dab;
        private ServerBrowserViewModel _browser;
        private ResultSet _results = new ResultSet();
        private BindingSource _dataBinding = new BindingSource();
        private string _messages = string.Empty;
        private List<Database> _databases;

        private string _fileName = string.Empty;
        private string _safeFileName = "New Query";
        private bool _isSaved = false;

        private const string FILE_TYPES = "SQL file (*.sql)|*.sql|Text file (*.txt)|*.txt|All files (*.*)|*.*";

        #endregion

        #region Constructor(s)

        public QueryViewModel(ServerBrowserViewModel Browser)
        {
            _dab = Browser.ActiveConnection;
            _browser = Browser;
        }

        #endregion

        #region Public Properties
        
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

        public string Messages
        {
            get { return _messages; }
        }

        public string SafeFileName
        {
            get { return _safeFileName; }
        }

        public bool IsSaved
        {
            get { return _isSaved; }
            set { _isSaved = value; }
        }

        public List<Database> Databases
        {
            get { return _databases; }
        }

        #endregion

        #region Public Methods

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

        public void RunQuery(string Query)
        {
            this.RunQueryAsync(Query);
        }

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

        public void Connect()
        {
            this.Disconnect();
            this.InitQuery(false);
        }

        public void Disconnect()
        {
            if (this._dab != null)
            {
                this._dab.Dispose();
                this._dab = null;
            }
        }

        public void SetDatabase(string DatabaseName)
        {
            this.Data.SetDatabase(DatabaseName);
        }

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

        #region Public Events

        public event EventHandler BeginQuery;
        public event EndQueryEventHandler EndQuery;
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnBeginQuery()
        {
            if (BeginQuery != null)
                BeginQuery(this, new EventArgs());
        }

        private void OnEndQuery(bool IsError)
        {
            if (EndQuery != null)
                EndQuery(this, new EndQueryEventArgs(IsError));
        }

        private void NotifyPropertyChanged(string Property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
            }
        }

        #endregion

        #region Private Methods

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

        private delegate ResultSet RunQueryDelegate(string Query);
        private event RunQueryCompletedEventHandler RunQueryCompleted;

        protected virtual void OnRunQueryCompleted(CustomEventArgs e)
        {
            if (RunQueryCompleted != null)
            {
                RunQueryCompleted(this, e);
            }
        }

        private void RunQueryAsync(string Query)
        {
            RunQueryCompleted += this.RunQuery_Completed;
            RunQueryDelegate dl = new RunQueryDelegate(RunQueryWorker);
            AsyncOperation async = AsyncOperationManager.CreateOperation(null);

            OnBeginQuery();
            IAsyncResult ar = dl.BeginInvoke(Query, new AsyncCallback(RunQueryCallback), async);
        }

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
                res = _dab.GetDataSet(Query, _browser);
            }

            return res;
        }

        private void RunQueryCallback(IAsyncResult ar)
        {
            RunQueryDelegate dl = (RunQueryDelegate)((System.Runtime.Remoting.Messaging.AsyncResult)ar).AsyncDelegate;
            AsyncOperation async = (AsyncOperation)ar.AsyncState;
            ResultSet results;
            CustomEventArgs completedArgs = new CustomEventArgs(null, false, null);

            try
            {
                results = dl.EndInvoke(ar);
                completedArgs.Results = results;
            }
            catch(Exception ex)
            {
                completedArgs = new CustomEventArgs(ex, false, null);
            }
            finally
            {
                async.PostOperationCompleted(delegate(object e) { OnRunQueryCompleted((CustomEventArgs)e); }, completedArgs);
            }
        }

        private void RunQuery_Completed(object sender, CustomEventArgs e)
        {
            bool isError = true;
            if (e.Error == null)
            {
                _results = e.Results;
                isError = false;
            }
            else
            {
                _results = new ResultSet();
                _results.Messages = e.Error.Message;
            }

            _dataBinding.DataSource = _results.Data;
            _messages = _results.Messages;
            NotifyPropertyChanged("Messages");
            RunQueryCompleted -= this.RunQuery_Completed;
            OnEndQuery(isError);
        }

        #endregion
    }
}
