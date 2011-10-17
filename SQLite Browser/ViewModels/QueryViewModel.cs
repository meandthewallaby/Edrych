using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SQLiteBrowser.DataAccess;
using SQLiteBrowser.Dialogs;
using SQLiteBrowser.Helpers;

namespace SQLiteBrowser.ViewModels
{
    public class QueryViewModel
    {
        #region Private/Global Variables

        private string _connType;
        private string _dataSource;
        private DataAccessBase _dab;
        private ResultSet _results = new ResultSet();
        private BindingSource _dataBinding = new BindingSource();

        private string _fileName = string.Empty;
        private string _safeFileName = "New Query";
        private bool _isSaved = false;

        #endregion

        #region Constructor(s)

        public QueryViewModel()
        {
        }

        #endregion

        #region Public Properties

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

        public string SafeFileName
        {
            get { return _safeFileName; }
        }

        public bool IsSaved
        {
            get { return _isSaved; }
            set { _isSaved = value; }
        }

        #endregion

        #region Public Methods

        public void InitiatlizeData()
        {
            DataAccess.ConnectionType ct = (DataAccess.ConnectionType)Enum.Parse(typeof(DataAccess.ConnectionType), this.ConnectionType);
            //May need to build the connection string a little differently...
            this._dab = DataAccessFactory.GetDataAccess(ct, "Data Source=" + this.DataSource);
        }

        public string InitQuery(bool OpenQuery)
        {
            string queryText = string.Empty;

            if (OpenQuery)
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Multiselect = false;

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

            ConnectDialog cd = new ConnectDialog(this);
            cd.ShowDialog();

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
                saveDialog.Filter = "SQL file (*.sql)|*.sql|Text file (*.txt)|*.txt|All files (*.*)|*.*";

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

        protected virtual void OnRunQueryCompleted(RunQueryCompletedEventArgs e)
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

            IAsyncResult ar = dl.BeginInvoke(Query, new AsyncCallback(RunQueryCallback), async);
        }

        private ResultSet RunQueryWorker(string Query)
        {
            ResultSet res = null;
            if (!string.IsNullOrEmpty(Query))
            {
                res = _dab.GetDataSet(Query);
            }

            return res;
        }

        private void RunQueryCallback(IAsyncResult ar)
        {
            RunQueryDelegate dl = (RunQueryDelegate)((System.Runtime.Remoting.Messaging.AsyncResult)ar).AsyncDelegate;
            AsyncOperation async = (AsyncOperation)ar.AsyncState;
            ResultSet results = dl.EndInvoke(ar);
            RunQueryCompletedEventArgs completedArgs = new RunQueryCompletedEventArgs(null, false, null);
            completedArgs.Results = results;
            async.PostOperationCompleted(delegate(object e) { OnRunQueryCompleted((RunQueryCompletedEventArgs)e); }, completedArgs);
        }

        private void RunQuery_Completed(object sender, RunQueryCompletedEventArgs e)
        {
            _results = e.Results;
            _dataBinding.DataSource = _results.Data;
            RunQueryCompleted -= this.RunQuery_Completed;
        }

        #endregion
    }
}
