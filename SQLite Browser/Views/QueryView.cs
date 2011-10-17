using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SQLiteBrowser.Dialogs;
using SQLiteBrowser.Helpers;
using SQLiteBrowser.ViewModels;

namespace SQLiteBrowser.Views
{
    public partial class QueryView : TabPage
    {
        #region Private/Global Variables

        private QueryViewModel _queryViewModel;
        private int _numLines = 0;

        #endregion

        #region Constructor(s)

        public QueryView()
        {
            InitializeComponent();
            _queryViewModel = new QueryViewModel();
            this.dgResults.DataSource = _queryViewModel.DataBinding;
            this.Name = Guid.NewGuid().ToString();
        }

        #endregion

        #region Public Properties

        public string TabName
        {
            get { return _queryViewModel.SafeFileName; }
        }

        public bool IsSaved
        {
            get { return _queryViewModel.IsSaved; }
        }

        #endregion

        #region Public Methods

        public void CreateQueryView(bool OpenQuery)
        {
            TabControlExt tc = this.Parent as TabControlExt;
            if (tc != null)
            {
                tc.Closing += this.TabClosing;
            }
            this.tbQuery.Text = _queryViewModel.InitQuery(OpenQuery);
            this.ResetTabName();
            this.tbQuery.Focus();
        }

        #endregion

        #region Private Methods

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

        private void ResetTabName()
        {
            this.Text = this.TabName;
            if (_queryViewModel.IsSaved == false)
            {
                this.Text = "*" + this.Text;
            }
        }

        #endregion

        #region Event Handlers

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

            if (_queryViewModel.IsSaved && this.tbQuery.CanUndo)
            {
                _queryViewModel.IsSaved = false;
                ResetTabName();
            }
        }

        private void QueryView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                string query = (string.IsNullOrEmpty(tbQuery.SelectedText.Trim()) ? tbQuery.Text : tbQuery.SelectedText).Trim();
                _queryViewModel.RunQuery(query);
            }
            else if ((e.Modifiers & ModifierKeys) == Keys.Control && e.KeyCode == Keys.R)
            {
                this.splitContainer1.Panel2Collapsed = !this.splitContainer1.Panel2Collapsed;
            }
        }

        private void QueryView_Scrolling(object sender, EventArgs e)
        {
            UpdateLineNumbers();
        }

        private void QueryView_Load(object sender, EventArgs e)
        {
            this.tbQuery.Focus();
        }

        private void QueryView_Focus(object sender, EventArgs e)
        {
            App.Save += this.QueryView_Save;
            App.SaveAs += this.QueryView_SaveAs;
        }

        private void QueryView_Leave(object sender, EventArgs e)
        {
            App.Save -= this.QueryView_Save;
            App.SaveAs -= this.QueryView_SaveAs;
        }

        private void Query_Focus(object sender, EventArgs e)
        {
            App.IsCopyEnabled = true;
            App.IsCutEnabled = true;
            App.IsPasteEnabled = true;
            App.IsUndoEnabled = this.tbQuery.CanUndo;
            App.IsRedoEnabled = this.tbQuery.CanRedo;
            App.IsSelectAllEnabled = true;

            App.Cut += this.Query_Cut;
            App.Copy += this.Query_Copy;
            App.Paste += this.Query_Paste;
            App.Undo += this.Query_Undo;
            App.Redo += this.Query_Redo;
            App.SelectAll += this.Query_SelectAll;
        }

        private void Results_Focus(object sender, EventArgs e)
        {
            App.IsCopyEnabled = true;
            App.IsSelectAllEnabled = true;

            App.Copy += this.Results_Copy;
            App.SelectAll += this.Results_SelectAll;
        }

        private void QueryOrResults_Leave(object sender, EventArgs e)
        {
            App.IsCutEnabled = false;
            App.IsCopyEnabled = false;
            App.IsPasteEnabled = false;
            App.IsUndoEnabled = false;
            App.IsRedoEnabled = false;

            App.Cut -= this.Query_Cut;
            App.Copy -= this.Query_Copy;
            App.Paste -= this.Query_Paste;
            App.Undo -= this.Query_Undo;
            App.Redo -= this.Query_Redo;
            App.SelectAll -= this.Query_SelectAll;

            App.Copy -= this.Results_Copy;
            App.SelectAll -= this.Results_SelectAll;
        }

        private void QueryView_Save(object sender, EventArgs e)
        {
            _queryViewModel.SaveQuery(this.tbQuery.Text, false);
            this.ResetTabName();
        }

        private void QueryView_SaveAs(object sender, EventArgs e)
        {
            _queryViewModel.SaveQuery(this.tbQuery.Text, true);
            this.ResetTabName();
        }

        private void Query_Cut(object sender, EventArgs e)
        {
            this.tbQuery.Cut();
        }

        private void Query_Copy(object sender, EventArgs e)
        {
            this.tbQuery.Copy();
        }

        private void Query_Paste(object sender, EventArgs e)
        {
            this.tbQuery.Paste();
        }

        private void Query_Undo(object sender, EventArgs e)
        {
            this.tbQuery.Undo();
        }

        private void Query_Redo(object sender, EventArgs e)
        {
            this.tbQuery.Redo();
        }

        private void Query_SelectAll(object sender, EventArgs e)
        {
            this.tbQuery.SelectAll();
        }

        private void Results_Copy(object sender, EventArgs e)
        {
            this.dgResults.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            DataObject d = dgResults.GetClipboardContent();
            Clipboard.SetDataObject(d);
        }

        private void Results_SelectAll(object sender, EventArgs e)
        {
            this.dgResults.SelectAll();
        }

        private void TabClosing(object sender, CloseEventArgs e)
        {
            TabControlExt tc = this.Parent as TabControlExt;
            if (tc != null && tc.TabPages[e.TabIndex].Name == this.Name)
            {
                DialogResult result = DialogResult.Yes;
                
                if (_queryViewModel.IsSaved == false)
                {
                    result = MessageBox.Show("This query is unsaved! Would you like to save all your hard work?", "Unsaved Query", MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                        _queryViewModel.SaveQuery(this.tbQuery.Text, false);
                    }
                }
                
                if (result != DialogResult.Cancel)
                {
                    tc.TabPages.RemoveAt(e.TabIndex);
                }
            }
        }

        #endregion
    }
}
