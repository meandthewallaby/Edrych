using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SQLiteBrowser.Properties;
using SQLiteBrowser.ViewModels;

namespace SQLiteBrowser.Dialogs
{
    public partial class ConnectDialog : Form
    {
        private QueryViewModel _query;

        public ConnectDialog(QueryViewModel Query)
        {
            InitializeComponent();
            _query = Query;
            PopulateConnectionType();
            PopulateDataSource();
        }

        private void PopulateConnectionType()
        {
            this.cbConnectionType.Items.Clear();
            this.cbConnectionType.Items.Add("SQLite");
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
            _query.ConnectionType = this.cbConnectionType.Text;
            _query.DataSource = this.cbDataSource.Text;
                
            //TODO: Should probably try the connection here...
            try
            {
                _query.InitiatlizeData();

                if (Settings.Default.RecentConnections == null)
                {
                    Settings.Default.RecentConnections = new StringCollection();
                }

                if (Settings.Default.RecentConnections.Contains(_query.DataSource))
                {
                    Settings.Default.RecentConnections.Remove(_query.DataSource);
                }
                Settings.Default.RecentConnections.Insert(0, _query.DataSource);
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
            _query.ConnectionType = null;
            _query.DataSource = null;
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

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
    }
}
