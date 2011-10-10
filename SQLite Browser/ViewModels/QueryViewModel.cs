using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SQLiteBrowser.DataAccess;
using SQLiteBrowser.Helpers;

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
            this.RunQueryAsync(Query);
        }

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
