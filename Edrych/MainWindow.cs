using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Edrych.Helpers;
using Edrych.Views;
using Edrych.ViewModels;

namespace Edrych
{
    public partial class MainWindow : Form
    {
        #region Private/Global Variables

        private ServerBrowserViewModel _browserViewModel;
        private QueryView _activeQuery;

        #endregion

        #region Constructor(s)

        public MainWindow()
        {
            InitializeComponent();
            InitializeMenus();
            InitializeTreeView();
        }

        #endregion

        #region Menu Item Handling - File Menu

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateQueryTab(false);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateQueryTab(true);
        }

        private void connectMenuItem_Click(object sender, EventArgs e)
        {
            _browserViewModel.CreateConnection();
        }

        private void disconnectMenuItem_Click(object sender, EventArgs e)
        {
            if (this.treeViewAdv1.SelectedNode != null)
            {
                _browserViewModel.RemoveConnection(this.treeViewAdv1.SelectedNode);
            }
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

        #region Menu Item Handling - Tool Strip

        private void databaseDropDown_SelectionChanged(object sender, EventArgs e)
        {
            if (App.LoadingDatabases)
                return;
            if(_activeQuery != null)
                _activeQuery.SetDatabase(this.databaseDropDown.SelectedItem as string);
        }

        private void databaseDropDown_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                _activeQuery.Focus();
        }

        #endregion

        #region Menu Item Handling - Tree Context Menu

        private void contextMenu_Opening(object sender, CancelEventArgs e)
        {
            this.contextRefresh.Enabled = this.treeViewAdv1.SelectedNode != null;
        }

        private void contextRefresh_Click(object sender, EventArgs e)
        {
            if(this.treeViewAdv1.SelectedNode != null)
            {
                this.treeViewAdv1.SelectedNode.Collapse();
                _browserViewModel.RefreshNode(this.treeViewAdv1.SelectedNode);
            }
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
            this.databaseDropDown.Enabled = App.IsDatabaseDropDownEnabled;
            this.queryConnectToolStrip.Enabled = App.IsQueryConnectEnabled;
            this.queryDisconnectToolStrip.Enabled = App.IsQueryDisconnectEnabled;
            this.queryToolStripMenuItem.Visible = App.IsQueryMenuVisible;
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
            else
            {
                this._browserViewModel.Dispose();
            }
        }
        
        private void TreeSelection_Changed(object sender, EventArgs e)
        {
            bool isDisconnectEnabled = this.treeViewAdv1.SelectedNode != null;
            this.disconnectMenuItem.Enabled = isDisconnectEnabled;
            this.disconnectToolStrip.Enabled = isDisconnectEnabled;
            if (this.treeViewAdv1.SelectedNode != null)
            {
                _browserViewModel.UpdateActiveConnection(this.treeViewAdv1.SelectedNode);
            }
        }

        private void QueryConnection_Changed(object sender, ConnectionChangedEventArgs e)
        {
            App.LoadingDatabases = true;
            this.databaseDropDown.Items.Clear();
            foreach (DataAccess.Database db in e.Databases.OrderBy(d => d.Name))
            {
                this.databaseDropDown.Items.Add(db.Name);
            }
            this.databaseDropDown.SelectedItem = e.SelectedDatabase;
            App.LoadingDatabases = false;
        }

        private void ActiveQuery_Changed(object sender, EventArgs e)
        {
            this._activeQuery = sender as QueryView;
        }

        private void SwitchDatabases(object sender, EventArgs e)
        {
            this.databaseDropDown.Focus();
        }

        #endregion

        #region Private Methods

        private void CreateQueryTab(bool IsOpen)
        {
            QueryView qp = new QueryView(_browserViewModel.ActiveConnection);
            this.tabControl1.TabPages.Insert(0, qp);
            qp.CreateQueryView(IsOpen);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.ResizeTabs();
        }

        private void InitializeMenus()
        {
            App.PropertyChanged += this.Property_Changed;
            this.treeViewAdv1.SelectionChanged += this.TreeSelection_Changed;
            App.ConnectionChanged += this.QueryConnection_Changed;
            App.ActiveQueryChanged += this.ActiveQuery_Changed;
            App.SwitchDatabases += this.SwitchDatabases;

            App.IsCopyEnabled = false;
            App.IsPasteEnabled = false;
            App.IsUndoEnabled = false;
            App.IsRedoEnabled = false;
            App.IsSelectAllEnabled = false;
            App.IsQueryMenuVisible = false;
            App.IsQueryConnectEnabled = false;
            App.IsQueryDisconnectEnabled = false;
        }

        private void InitializeTreeView()
        {
            _browserViewModel = new ServerBrowserViewModel();
            this.treeViewAdv1.Model = _browserViewModel.Tree;
        }

        #endregion
    }
}
