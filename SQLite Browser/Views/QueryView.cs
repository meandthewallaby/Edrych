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
        private int _numLines = 0;

        public QueryView()
        {
            InitializeComponent();
            _queryViewModel = new QueryViewModel();
            ConnectDialog cd = new ConnectDialog(_queryViewModel);
            DialogResult dr = cd.ShowDialog();
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
            else if ((e.Modifiers & ModifierKeys) == Keys.Control && e.KeyCode == Keys.R)
            {
                this.splitContainer1.Panel2Collapsed = !this.splitContainer1.Panel2Collapsed;
            }
        }

        private void RunQuery()
        {
            string query = (string.IsNullOrEmpty(tbQuery.SelectedText.Trim()) ? tbQuery.Text : tbQuery.SelectedText).Trim();

            _queryViewModel.RunQuery(query);
        }

        private void UpdateLineNumbers()
        {
            int d = this.tbQuery.GetPositionFromCharIndex(0).Y %
                              (this.tbQuery.Font.Height + 1);
            this.tbLines.Location = new Point(0, d);

            int firstCharIndex = this.tbQuery.GetCharIndexFromPosition(new Point(0, 0));
            int lineNumber = this.tbQuery.GetLineFromCharIndex(firstCharIndex);
            int newLines = this.tbQuery.Lines.Count();
            List<string> lines = new List<string>();
            for (int i = lineNumber + 1; i <= newLines; i++)
            {
                lines.Add(i.ToString());
            }
            this.tbLines.Lines = lines.ToArray();
        }

        private void QueryView_QueryChanged(object sender, EventArgs e)
        {
            int newLines = this.tbQuery.Lines.Count();

            if (_numLines != newLines)
            {
                UpdateLineNumbers();
            }

            _numLines = newLines;

            App.IsUndoEnabled = this.tbQuery.CanUndo;
            App.IsRedoEnabled = this.tbQuery.CanRedo;
        }

        private void QueryView_Scrolling(object sender, EventArgs e)
        {
            UpdateLineNumbers();
        }

        private void QueryView_Load(object sender, EventArgs e)
        {
            this.tbQuery.Focus();
        }

        private void Query_Focus(object sender, EventArgs e)
        {
            if (this.tbQuery.Focused)
            {
                App.IsCopyEnabled = true;
                App.IsPasteEnabled = true;
                App.IsUndoEnabled = this.tbQuery.CanUndo;
                App.IsRedoEnabled = this.tbQuery.CanRedo;
            }
        }

        private void Query_UnFocus(object sender, EventArgs e)
        {
            App.IsCopyEnabled = false;
            App.IsPasteEnabled = false;
            App.IsUndoEnabled = false;
            App.IsRedoEnabled = false;
        }
    }
}
