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
    public partial class OptionsDialog : Form
    {
        private bool _saved = false;

        public OptionsDialog()
        {
            InitializeComponent();
            PopulateConnectionType();
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            SaveOptions();
            if(_saved)
                this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            SaveOptions();
        }

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

        private void SaveOptions()
        {
            ConnectionType type;
            if (this.cbConnectionType.SelectedText != null && Enum.TryParse<ConnectionType>(this.cbConnectionType.SelectedItem.ToString(), out type))
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
    }
}
