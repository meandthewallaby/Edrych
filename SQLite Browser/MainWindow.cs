using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SQLiteBrowser.Helpers;
using SQLiteBrowser.Views;
using SQLiteBrowser.ViewModels;

namespace SQLiteBrowser
{
    public partial class MainWindow : Form
    {
        private TreeViewModel _treeViewModel;

        public MainWindow()
        {
            InitializeComponent();
            InitializeMenus();
            InitializeTreeView();
        }

        #region Menu Item Handling - File Menu

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateQueryTab(false);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateQueryTab(true);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Event Handlers

        private void CutEnabled_Changed(object sender, EventArgs e)
        {
            this.cutToolStripMenuItem.Enabled = App.IsCutEnabled;
        }

        private void CopyEnabled_Changed(object sender, EventArgs e)
        {
            this.copyToolStripMenuItem.Enabled = App.IsCopyEnabled;
        }

        private void PasteEnabled_Changed(object sender, EventArgs e)
        {
            this.pasteToolStripMenuItem.Enabled = App.IsPasteEnabled;
        }

        private void UndoEnabled_Changed(object sender, EventArgs e)
        {
            this.undoToolStripMenuItem.Enabled = App.IsUndoEnabled;
        }

        private void RedoEnabled_Changed(object sender, EventArgs e)
        {
            this.redoToolStripMenuItem.Enabled = App.IsRedoEnabled;
        }

        private void SelectAllEnabled_Changed(object sender, EventArgs e)
        {
            this.selectAllToolStripMenuItem.Enabled = App.IsSelectAllEnabled;
        }

        private void Window_Closing(object sender, FormClosingEventArgs e)
        {
            List<QueryView> unsavedQueries = new List<QueryView>();
            StringBuilder files = new StringBuilder();
            DialogResult result = System.Windows.Forms.DialogResult.No;

            foreach (QueryView query in this.tabControl1.TabPages)
            {
                if (query.IsSaved == false)
                {
                    unsavedQueries.Add(query);
                    files.AppendLine(query.TabName);
                }
            }

            if (unsavedQueries.Count > 0)
            {
                result = MessageBox.Show("You're closing tabs that have some work you haven't saved yet. Do you want to save the following files?\r\n" + files.ToString(), "Unsaved Queries", MessageBoxButtons.YesNoCancel);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    foreach (QueryView query in unsavedQueries)
                    {
                        query.Select();
                        query.Focus();
                        App.OnSave(this, new EventArgs());
                        this.tabControl1.TabPages.RemoveByKey(query.Name);
                    }
                }
            }

            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        private void contextRefresh_Click(object sender, EventArgs e)
        {
            this.treeViewAdv1.SelectedNode.Collapse();
            _treeViewModel.RefreshNode(this.treeViewAdv1.SelectedNode);
        }

        #endregion

        #region Private Methods

        private void CreateQueryTab(bool IsOpen)
        {
            QueryView qp = new QueryView(_treeViewModel.DataAccess);
            this.tabControl1.TabPages.Insert(0, qp);
            qp.CreateQueryView(IsOpen);
            this.tabControl1.SelectedIndex = 0;
        }

        private void InitializeMenus()
        {
            App.CutEnabledChanged += this.CutEnabled_Changed;
            App.CopyEnabledChanged += this.CopyEnabled_Changed;
            App.PasteEnabledChanged += this.PasteEnabled_Changed;
            App.UndoEnabledChanged += this.UndoEnabled_Changed;
            App.RedoEnabledChanged += this.RedoEnabled_Changed;
            App.SelectAllEnabledChanged += this.SelectAllEnabled_Changed;

            App.IsCopyEnabled = false;
            App.IsPasteEnabled = false;
            App.IsUndoEnabled = false;
            App.IsRedoEnabled = false;
            App.IsSelectAllEnabled = false;
        }

        private void InitializeTreeView()
        {
            _treeViewModel = new TreeViewModel();
            this.treeViewAdv1.Model = _treeViewModel.Tree;
        }

        #endregion
    }
}
