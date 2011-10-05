using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SQLiteBrowser.Dialogs;
using SQLiteBrowser.ViewModels;

namespace SQLiteBrowser.Views
{
    public partial class QueryView : UserControl
    {
        private QueryViewModel _queryViewModel;

        public QueryView()
        {
            InitializeComponent();
            _queryViewModel = new QueryViewModel();
            this.splitContainer1.Panel2Collapsed = true;
            ConnectDialog cd = new ConnectDialog(_queryViewModel);
            DialogResult dr = cd.ShowDialog();
            if (dr == DialogResult.OK)
            {
            }
        }

        private void QueryView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                RunQuery();
            }
        }

        private void RunQuery()
        {
            string query = this.tbQuery.Text.Trim();
            if (!string.IsNullOrEmpty(query))
            {
                string message = string.Empty;
                try
                {
                    message = _queryViewModel.Data.ExecuteNonQuery(query).ToString() + " rows affected";
                }
                catch(Exception e)
                {
                    message = e.Message;
                }

                MessageBox.Show(message);
            }
        }
    }
}
