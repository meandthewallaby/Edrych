using System;
using System.Linq;
using System.Windows.Forms;
using Edrych.DataAccess;
using Edrych.Helpers;

namespace Edrych.Dialogs
{
    public partial class ConnectDialog : Form
    {
        #region Private/Global Variables

        private DataAccessBase _dataAccess;
        private ConnectionType _connType;
        private string _dataSource;
        private Settings _settings;
        
        #endregion

        #region Constructor(s)
        
        public ConnectDialog()
        {
            InitializeComponent();
            _settings = new Settings();
            PopulateConnectionType();
        }

        #endregion

        #region Public Properties

        public DataAccessBase DataAccess
        {
            get { return _dataAccess; }
        }

        #endregion

        #region Public Methods

        public void InitiatlizeData()
        {
            this._dataAccess = DataAccessFactory.GetDataAccess(_connType, _dataSource);
        }

        #endregion

        #region Private Methods

        private void PopulateConnectionType()
        {
            this.cbConnectionType.Items.Clear();
            int i = 0;
            int index = 0;
            foreach (ConnectionSource source in DataAccessFactory.GetSources())
            {
                this.cbConnectionType.Items.Add(source.Name);
                if (source.Name == DataAccessFactory.DefaultType.ToString())
                {
                    index = i;
                }
                i++;
            }
            if (this.cbConnectionType.Items.Count > 0)
            {
                this.cbConnectionType.SelectedIndex = index;
            }
        }

        private void PopulateDataSource()
        {
            this.cbDataSource.Items.Clear();

            if (_settings.RecentConnections != null)
            {
                foreach (DataAccessConnection source in _settings.RecentConnections.Where(r => r.Connection == this.SelectedConnectionType))
                {
                    this.cbDataSource.Items.Add(source.DataSource);
                }
            }

            this.cbDataSource.Items.Add("Browse for more...");
        }

        private void Open()
        {
            _connType = this.SelectedConnectionType;
            _dataSource = this.cbDataSource.Text;
                
            try
            {
                InitiatlizeData();

                DataAccessConnection current = new DataAccessConnection(_connType, _dataSource);

                int currIndex = _settings.RecentConnections.IndexOf(_settings.RecentConnections.FirstOrDefault(c => c.Connection == current.Connection && c.DataSource == current.DataSource));
                if (currIndex >= 0)
                {
                    _settings.RecentConnections.RemoveAt(currIndex);
                }

                _settings.RecentConnections.Insert(0, current);
                _settings.Save();

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

        #endregion

        #region Private Properties

        private ConnectionType SelectedConnectionType
        {
            get
            {
                ConnectionType type = ConnectionType.None;
                if (!string.IsNullOrEmpty(this.cbConnectionType.Text))
                {
                    type = (DataAccess.ConnectionType)Enum.Parse(typeof(DataAccess.ConnectionType), this.cbConnectionType.Text);
                }
                return type;
            }
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

        private void cbConnectionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateDataSource();
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
