using System;
using System.Linq;
using System.Windows.Forms;
using Edrych.DataAccess;
using Edrych.Helpers;

namespace Edrych.Dialogs
{
    /// <summary>Dialog window that creates a connection</summary>
    partial class ConnectDialog : Form
    {
        #region Private/Global Variables

        private DataAccessBase _dataAccess;
        private Settings _settings;
        
        #endregion

        #region Constructor(s)
        
        /// <summary>Creates the dialog</summary>
        public ConnectDialog()
        {
            InitializeComponent();
            _settings = new Settings();
            PopulateConnectionType();
        }

        #endregion

        #region public Properties

        /// <summary>Data Access object that the dialog creates</summary>
        public DataAccessBase DataAccess
        {
            get { return _dataAccess; }
        }

        #endregion

        #region Private Methods

        /// <summary>Grabs the available connection types and puts them in the drop down</summary>
        private void PopulateConnectionType()
        {
            this.cbConnectionType.Items.Clear();
            int i = 0;
            int index = 0;
            foreach (ConnectionSource source in DataAccessFactory.GetSources())
            {
                this.cbConnectionType.Items.Add(source);
                if (source.ConnType == DataAccessFactory.DefaultType)
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

        /// <summary>Grabs the available connections of the connection type</summary>
        private void PopulateDataSource()
        {
            this.cbDataSource.Items.Clear();
            ConnectionSource source = this.cbConnectionType.SelectedItem as ConnectionSource;

            if (source != null)
            {
                if (source.ConnType == ConnectionType.ODBC)
                {
                    AddOdbcConnections();
                }
                else
                {
                    AddRecentConnections();
                }

                if (source.AllowBrowse)
                {
                    this.cbDataSource.Items.Add("Browse for more...");
                }
            }
        }

        /// <summary>Adds recent connections to the data source drop down</summary>
        private void AddRecentConnections()
        {
            if (_settings.RecentConnections != null)
            {
                foreach (DataAccessConnection conn in _settings.RecentConnections.Where(r => r.Connection == this.SelectedConnectionType))
                {
                    this.cbDataSource.Items.Add(conn.DataSource);
                }
            }
        }

        /// <summary>Adds ODBC connections to the data source drop down</summary>
        private void AddOdbcConnections()
        {
            if (_settings.OdbcConnections != null)
            {
                foreach (DataAccessConnection conn in _settings.OdbcConnections)
                {
                    this.cbDataSource.Items.Add(conn.DataSource);
                }
            }
        }

        /// <summary>Enables controls in the dialog based on the connection type and authentication mode</summary>
        private void SetAvailableOptions()
        {

            ConnectionSource source = this.cbConnectionType.SelectedItem as ConnectionSource;
            this.tbDatabase.Enabled = source.AcceptsDatabase;
            this.cbAuthType.Items.Clear();
            foreach (AuthType auth in source.AuthTypes)
            {
                this.cbAuthType.Items.Add(auth.ToString());
            }
            this.cbAuthType.SelectedIndex = 0;
        }

        /// <summary>Updates the dialog with info from saved connectios (either ODBC or a recent connection)</summary>
        /// <param name="dac">Saved connection to use to populate the dialog</param>
        private void ApplySavedConnection(DataAccessConnection dac)
        {
            if (dac != null)
            {
                this.tbDatabase.Text = dac.Database;
                this.cbAuthType.SelectedItem = dac.Auth.ToString();
                this.tbUsername.Text = dac.Username;
                this.tbPassword.Text = dac.Password;
                this.cbxSavePassword.Checked = !string.IsNullOrEmpty(dac.Password);
            }
        }

        #endregion

        #region Private Properties

        /// <summary>Property which returns the connection type of the selected data source</summary>
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

        /// <summary>Returns the selected authentication mode</summary>
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

        /// <summary>Handles when Open is clicked. Creates the data connection from the supplied information</summary>
        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                this._dataAccess =
                   DataAccessFactory.GetDataAccess(
                       this.SelectedConnectionType,
                       this.cbDataSource.Text,
                       this.tbDatabase.Text,
                       this.SelectedAuthType,
                       this.tbUsername.Text,
                       this.tbPassword.Text
                   );

                DataAccessConnection current = new DataAccessConnection(
                    this.SelectedConnectionType,
                    this.cbDataSource.Text,
                    this.tbDatabase.Text,
                    this.SelectedAuthType,
                    this.tbUsername.Text,
                    (this.cbxSavePassword.Enabled && this.cbxSavePassword.Checked ? this.tbPassword.Text : null)
                    );

                int currIndex = _settings.RecentConnections.IndexOf(
                    _settings.RecentConnections.FirstOrDefault(c => c.Connection == current.Connection && c.DataSource == current.DataSource)
                    );
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

        /// <summary>Clears out the info and closes the dialog when the Cancel button is clicked.</summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            _dataAccess = null;
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        /// <summary>Updates the data sources when a connection type is selected</summary>
        private void cbConnectionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cbDataSource.Text = null;
            this.tbDatabase.Text = null;
            this.tbUsername.Text = null;
            this.tbPassword.Text = null;
            PopulateDataSource();
            SetAvailableOptions();
        }

        /// <summary>Updates the info when a data source is selected.</summary>
        private void cbDataSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbDataSource.Text == "Browse for more...")
            {
                if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string fileName = this.openFileDialog1.FileName;
                    this.cbDataSource.Items.Insert(0, fileName);
                    this.cbDataSource.Text = fileName;
                }
            }
            else if (this.SelectedConnectionType == ConnectionType.ODBC)
            {
                ApplySavedConnection(_settings.OdbcConnections.FirstOrDefault(c => c.DataSource == this.cbDataSource.Text));
            }
            else
            {
                ApplySavedConnection(_settings.RecentConnections.FirstOrDefault(c => c.Connection == this.SelectedConnectionType && c.DataSource == this.cbDataSource.Text));
            }
        }

        /// <summary>Handles the UI changes when the authentication mode is changed</summary>
        private void cbAuthType_SelectedIndexChanged(object sender, EventArgs e)
        {
            AuthType auth;
            ConnectionSource source = this.cbConnectionType.SelectedItem as ConnectionSource;
            if (source != null && Enum.TryParse<AuthType>(this.cbAuthType.SelectedItem.ToString(), out auth) && auth == AuthType.Basic)
            {
                this.tbUsername.Enabled = source.AcceptsUsername;
                this.tbPassword.Enabled = source.AcceptsPassword;
                this.cbxSavePassword.Enabled = source.AcceptsPassword;
            }
            else
            {
                this.tbUsername.Enabled = false;
                this.tbPassword.Enabled = false;
                this.cbxSavePassword.Enabled = false;
            }
        }

        #endregion
    }
}
