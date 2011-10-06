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
        private bool _hideResults = true;

        public QueryView()
        {
            InitializeComponent();
            _queryViewModel = new QueryViewModel();
            ConnectDialog cd = new ConnectDialog(_queryViewModel);
            DialogResult dr = cd.ShowDialog();
            if (dr == DialogResult.OK)
            {
            }
            _queryViewModel.DataBinding = new BindingSource();
            this.dgResults.DataSource = _queryViewModel.DataBinding;
        }

        private void QueryView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                //Need to figure out how to do this async
                RunQuery();
            }
        }

        private void RunQuery()
        {
            string query = (string.IsNullOrEmpty(tbQuery.SelectedText.Trim()) ? tbQuery.Text : tbQuery.SelectedText).Trim();

            _queryViewModel.RunQuery(query);
        }
    }
}
