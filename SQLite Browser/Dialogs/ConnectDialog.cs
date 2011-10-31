using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SQLiteBrowser.DataAccess;
using SQLiteBrowser.Properties;
using SQLiteBrowser.ViewModels;

namespace SQLiteBrowser.Dialogs
{
    public partial class ConnectDialog : Form
    {
        #region Private/Global Variables

        private DataAccessBase _dataAccess;
        private ConnectionType _connType;
        private string _dataSource;
        
        #endregion

        #region Constructor(s)
        
        public ConnectDialog()
        {
            InitializeComponent();
            PopulateConnectionType();
            PopulateDataSource();
        }

        #endregion

        #region Public Properties

        public DataAccessBase DataAccess
        {
            get { return _dataAccess; }
        }

        #endregion

        #region Private Methods

        private void PopulateConnectionType()
        {
            this.cbConnectionType.Items.Clear();
            foreach (var connType in Enum.GetValues(typeof(ConnectionType)))
            {
                this.cbConnectionType.Items.Add(connType.ToString());
            }
        }

        private void PopulateDataSource()
        {
            this.cbDataSource.Items.Clear();

            if (Settings.Default.RecentConnections != null)
            {
                foreach (string source in Settings.Default.RecentConnections)
                {
                    this.cbDataSource.Items.Add(source);
                }
            }

            this.cbDataSource.Items.Add("Browse for more...");
        }

        private void Open()
        {
            _connType = (DataAccess.ConnectionType)Enum.Parse(typeof(DataAccess.ConnectionType), this.cbConnectionType.Text);
            _dataSource = this.cbDataSource.Text;
                
            try
            {
                InitiatlizeData();

                if (Settings.Default.RecentConnections == null)
                {
                    Settings.Default.RecentConnections = new StringCollection();
                }

                if (Settings.Default.RecentConnections.Contains(_dataSource))
                {
                    Settings.Default.RecentConnections.Remove(_dataSource);
                }
                Settings.Default.RecentConnections.Insert(0, _dataSource);
                Settings.Default.Save();

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not open the connection!\r\n" + e.Message, "Invalid Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Cancel()
        {
            _dataAccess = null;
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        public void InitiatlizeData()
        {
            this._dataAccess = DataAccessFactory.GetDataAccess(_connType, _dataSource);
        }

        #endregion

        #region Private Methods - Event Handlers

        private void btnOpen_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        private void cbDataSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbDataSource.Text == "Browse for more...")
            {
                if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string fileName = this.openFileDialog1.FileName;
                    this.cbDataSource.Items.Insert(0, fileName);
                    this.cbDataSource.SelectedItem = fileName;
                }
            }
        }

        #endregion
    }
}
