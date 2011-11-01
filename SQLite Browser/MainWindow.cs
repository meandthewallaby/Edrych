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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            App.OnSave(sender, e);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            App.OnSaveAs(sender, e);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Menu Item Handling - Edit Menu

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            App.OnCut(sender, e);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            App.OnCopy(sender, e);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            App.OnPaste(sender, e);
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            App.OnUndo(sender, e);
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            App.OnRedo(sender, e);
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            App.OnSelectAll(sender, e);
        }

        #endregion

        #region Menu Item Handling - Query Menu

        private void queryConnectMenuItem_Click(object sender, EventArgs e)
        {
            App.OnQueryConnect(sender, e);
        }

        private void queryDisconnectMenuItem_Click(object sender, EventArgs e)
        {
            App.OnQueryDisconnect(sender, e);
        }

        #endregion

        #region Event Handlers

        private void Property_Changed(object sender, PropertyChangedEventArgs e)
        {
            this.cutToolStripMenuItem.Enabled = App.IsCutEnabled;
            this.copyToolStripMenuItem.Enabled = App.IsCopyEnabled;
            this.pasteToolStripMenuItem.Enabled = App.IsPasteEnabled;
            this.undoToolStripMenuItem.Enabled = App.IsUndoEnabled;
            this.redoToolStripMenuItem.Enabled = App.IsRedoEnabled;
            this.selectAllToolStripMenuItem.Enabled = App.IsSelectAllEnabled;
            this.queryConnectMenuItem.Enabled = App.IsQueryConnectEnabled;
            this.queryDisconnectMenuItem.Enabled = App.IsQueryDisconnectEnabled;
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
            this.tabControl1.ResizeTabs();
        }

        private void InitializeMenus()
        {
            App.PropertyChanged += this.Property_Changed;

            App.IsCopyEnabled = false;
            App.IsPasteEnabled = false;
            App.IsUndoEnabled = false;
            App.IsRedoEnabled = false;
            App.IsSelectAllEnabled = false;
            App.IsQueryConnectEnabled = false;
            App.IsQueryDisconnectEnabled = false;
        }

        private void InitializeTreeView()
        {
            _treeViewModel = new TreeViewModel();
            this.treeViewAdv1.Model = _treeViewModel.Tree;
        }

        private void connectMenuItem_Click(object sender, EventArgs e)
        {
            _treeViewModel.CreateConnection();
        }

        #endregion
    }
}
