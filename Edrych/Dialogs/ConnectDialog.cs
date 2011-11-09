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

        public void InitiatlizeData(ConnectionType ConnType, string DataSource, AuthType Auth, string Username, string Password)
        {
            this._dataAccess = DataAccessFactory.GetDataAccess(ConnType, DataSource, Auth, Username, Password);
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
                this.cbConnectionType.Items.Add(source);
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
                foreach (DataAccessConnection conn in _settings.RecentConnections.Where(r => r.Connection == this.SelectedConnectionType))
                {
                    this.cbDataSource.Items.Add(conn.DataSource);
                }
            }

            this.cbDataSource.Items.Add("Browse for more...");
            this.cbDataSource.Focus();
        }

        private void SetAvailableOptions()
        {
            ConnectionSource source = this.cbConnectionType.SelectedItem as ConnectionSource;
            this.cbAuthType.Items.Clear();
            foreach (AuthType auth in source.AuthTypes)
            {
                this.cbAuthType.Items.Add(auth.ToString());
            }
            this.cbAuthType.SelectedIndex = 0;
        }

        #endregion

        #region Private Properties

        private ConnectionType SelectedConnectionType
        {
            get
            {
                ConnectionType type = ConnectionType.None;
                ConnectionSource source = this.cbConnectionType.SelectedItem as ConnectionSource;
                if (source != null)
                {
                    type = source.ConnType;
                }
                return type;
            }
        }

        private AuthType SelectedAuthType
        {
            get
            {
                AuthType type = AuthType.None;
                Enum.TryParse<AuthType>(this.cbAuthType.SelectedItem.ToString(), out type);
                return type;
            }
        }

        #endregion

        #region Private Methods - Event Handlers

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                InitiatlizeData(this.SelectedConnectionType, this.cbDataSource.Text, this.SelectedAuthType, this.tbUsername.Text, this.tbPassword.Text);

                DataAccessConnection current = new DataAccessConnection(this.SelectedConnectionType, this.cbDataSource.Text);

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
            catch (Exception ex)
            {
                MessageBox.Show("Could not open the connection!\r\n" + ex.Message, "Invalid Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _dataAccess = null;
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void cbConnectionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateDataSource();
            SetAvailableOptions();
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

        private void cbAuthType_SelectedIndexChanged(object sender, EventArgs e)
        {
            AuthType auth;
            ConnectionSource source = this.cbConnectionType.SelectedItem as ConnectionSource;
            if (source != null && Enum.TryParse<AuthType>(this.cbAuthType.SelectedItem.ToString(), out auth) && auth == AuthType.Basic)
            {
                this.tbUsername.Enabled = source.AcceptsUsername;
                this.tbPassword.Enabled = source.AcceptsPassword;
            }
            else
            {
                this.tbUsername.Enabled = false;
                this.tbPassword.Enabled = false;
            }
        }

        #endregion
    }
}
