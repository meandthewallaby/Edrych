using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Edrych.DataAccess;

namespace Edrych.Dialogs
{
    /// <summary>Dialog containing the tool's options</summary>
    partial class OptionsDialog : Form
    {
        private bool _saved = false;

        /// <summary>Creates the dialog</summary>
        public OptionsDialog()
        {
            InitializeComponent();
            PopulateConnectionType();
        }

        #region Private Methods

        /// <summary>Populates a drop down list with the available connection types</summary>
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

        /// <summary>Saves the options selected</summary>
        private void SaveOptions()
        {
            ConnectionType type;
            if (this.cbConnectionType.SelectedText != null && Enum.TryParse<ConnectionType>(this.cbConnectionType.Text.Replace(" ", "_"), out type))
            {
                DataAccessFactory.SetDefaultType(type);
                _saved = true;
            }
            else
            {
                MessageBox.Show("Invalid Connection Type selected!", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _saved = false;
            }
        }

        #endregion

        #region Private Methods - Event Handlers

        /// <summary>Saves the settings and closes the window when the OK button is clicked</summary>
        private void btnOkay_Click(object sender, EventArgs e)
        {
            SaveOptions();
            if(_saved)
                this.Close();
        }

        /// <summary>Cancels the changes and closes the window when the Cancel button is clicked</summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        /// <summary>Apply the settings changes when the Apply button is clicked</summary>
        private void btnApply_Click(object sender, EventArgs e)
        {
            SaveOptions();
        }

        #endregion
    }
}
