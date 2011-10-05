﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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
            this.cbDataSource.Items.Add("Browse for more...");
        }

        private void Open()
        {
            if (this.cbConnectionType.Items.Contains(this.cbConnectionType.Text))
            {
                _query.ConnectionType = this.cbConnectionType.Text;
                _query.DataSource = this.cbDataSource.Text;
                
                //TODO: Should probably try the connection here...
                _query.InitiatlizeData();

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("You need to select a valid connection type", "Invalid Connection Type", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
