using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aga.Controls.Tree;
using Edrych.Dialogs;
using Edrych.Helpers;
using Edrych.Views;
using Edrych.ViewModels;

namespace Edrych
{
    /// <summary>Main window of the application</summary>
    partial class MainWindow : Form
    {
        #region Private/Global Variables

        private ServerBrowserViewModel _browserViewModel;
        private QueryView _activeQuery;
        private bool _treeMouseDown = false;

        #endregion

        #region Constructor(s)

        /// <summary>Initializes the windw</summary>
        public MainWindow()
        {
            InitializeComponent();
            InitializeMenus();
            InitializeServerBrowser();
        }

        #endregion

        #region public Properties

        /// <summary>Gets the browser view model</summary>
        public ServerBrowserViewModel Browser
        {
            get { return _browserViewModel; }
        }

        #endregion

        #region Public Methods

        /// <summary>Disposes of the window</summary>
        public void Disposal()
        {
            if(_browserViewModel != null)
                _browserViewModel.Dispose();
            if (this.tabControl1 != null && this.tabControl1.TabCount > 0)
            {
                foreach (QueryView query in this.tabControl1.TabPages)
                {
                    tabControl1.TabPages.Remove(query);
                }
            }
            this.Dispose(true);
        }

        #endregion

        #region Menu Item Handling - File Menu

        /// <summary>Handles the new item being clicked</summary>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateQueryTab(false);
        }

        /// <summary>Handles the open item being clicked</summary>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateQueryTab(true);
        }

        /// <summary>Handles the connect item being clicked</summary>
        private void connectMenuItem_Click(object sender, EventArgs e)
        {
            _browserViewModel.CreateConnection();
        }

        /// <summary>Handles the disconnect item being clicked</summary>
        private void disconnectMenuItem_Click(object sender, EventArgs e)
        {
            if (this.treeViewAdv1.SelectedNode != null)
            {
                _browserViewModel.RemoveConnection(this.treeViewAdv1.SelectedNode);
            }
        }

        /// <summary>Handles the save item being clicked</summary>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            App.OnSave(sender, e);
        }

        /// <summary>Handles the save as item being clicked</summary>
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            App.OnSaveAs(sender, e);
        }

        /// <summary>Handles the exit item being clicked</summary>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Menu Item Handling - Edit Menu

        /// <summary>Handles the cut item being clicked</summary>
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            App.OnCut(sender, e);
        }

        /// <summary>Handles the copy item being clicked</summary>
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            App.OnCopy(sender, e);
        }

        /// <summary>Handles the paste item being clicked</summary>
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            App.OnPaste(sender, e);
        }

        /// <summary>Handles the undo item being clicked</summary>
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            App.OnUndo(sender, e);
        }

        /// <summary>Handles the redo item being clicked</summary>
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            App.OnRedo(sender, e);
        }

        /// <summary>Handles the select all item being clicked</summary>
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            App.OnSelectAll(sender, e);
        }

        #endregion

        #region Menu Item Handling - Query Menu

        /// <summary>Handles the query connect item being clicked</summary>
        private void queryConnectMenuItem_Click(object sender, EventArgs e)
        {
            App.OnQueryConnect(sender, e);
        }

        /// <summary>Handles the query disconnect item being clicked</summary>
        private void queryDisconnectMenuItem_Click(object sender, EventArgs e)
        {
            App.OnQueryDisconnect(sender, e);
        }

        #endregion

        #region Menu Item Handling - Tools Menu

        /// <summary>Handles the options item being clicked</summary>
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsDialog options = new OptionsDialog();
            options.ShowDialog();
        }

        #endregion

        #region Menu Item Handling - Help Menu

        /// <summary>Opens up the CHM help file</summary>
        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "Resources/Edrych.chm");
        }

        /// <summary>Handles the about item being clicked</summary>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutDialog about = new AboutDialog();
            about.ShowDialog();
        }

        #endregion

        #region Menu Item Handling - Tool Strip

        /// <summary>Handles the database drop down changing selection</summary>
        private void databaseDropDown_SelectionChanged(object sender, EventArgs e)
        {
            if (App.LoadingDatabases)
                return;
            if(_activeQuery != null)
                _activeQuery.SetDatabase(this.databaseDropDown.SelectedItem as string);
        }

        /// <summary>Handles the a key being pressed on the database drop down</summary>
        private void databaseDropDown_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                _activeQuery.Focus();
        }

        /// <summary>Executes the active query</summary>
        private void executeQueryButton_Click(object sender, EventArgs e)
        {
            if(this._activeQuery != null)
                this._activeQuery.RunQuery();
        }

        /// <summary>Stops the active query</summary>
        private void stopQueryButton_Click(object sender, EventArgs e)
        {
            if (this._activeQuery != null)
                this._activeQuery.CancelQuery();
        }

        /// <summary>Handles when the outdent button is clicked</summary>
        private void outdentToolStrip_Click(object sender, EventArgs e)
        {
            if (this._activeQuery != null)
                this._activeQuery.OutdentLines(true);
        }

        /// <summary>Handles when the indent button is clicked</summary>
        private void indentToolStrip_Click(object sender, EventArgs e)
        {
            if (this._activeQuery != null)
                this._activeQuery.IndentLines(true);
        }

        #endregion

        #region Menu Item Handling - Tree Context Menu

        /// <summary>Handles the context menu opening</summary>
        private void contextMenu_Opening(object sender, CancelEventArgs e)
        {
            this.contextRefresh.Enabled = this.treeViewAdv1.SelectedNode != null;
        }

        /// <summary>Handles the context menu refresh item being clicked</summary>
        private void contextRefresh_Click(object sender, EventArgs e)
        {
            if(this.treeViewAdv1.SelectedNode != null)
            {
                this.treeViewAdv1.SelectedNode.Collapse();
                _browserViewModel.RefreshNode(this.treeViewAdv1.SelectedNode);
            }
        }

        #endregion

        #region Private Methods - Event Handlers

        /// <summary>Handles a <see cref="Edrych.App.PropertyChanged"/> event</summary>
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
            this.queryExecuteMenuItem.Enabled = App.IsQueryConnectEnabled;
            this.executeQueryButton.Enabled = App.IsQueryConnectEnabled;
            this.queryStopMenuItem.Enabled = App.IsStopQueryEnabled;
            this.stopQueryButton.Enabled = App.IsStopQueryEnabled;
            this.indentToolStrip.Enabled = App.IsQueryConnectEnabled;
            this.outdentToolStrip.Enabled = App.IsQueryConnectEnabled;
        }

        /// <summary>Handles the window closing</summary>
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

        /// <summary>Handles the browser changing selections</summary>
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

        /// <summary>Handles a <see cref="Edrych.App.ConnectionChanged"/> event</summary>
        private void QueryConnection_Changed(object sender, ConnectionChangedEventArgs e)
        {
            App.LoadingDatabases = true;
            string selectedDb = string.Empty;
            this.databaseDropDown.Items.Clear();
            foreach (DataAccess.Database db in e.Databases.OrderBy(d => d.Name))
            {
                this.databaseDropDown.Items.Add(db.Name.Trim());
                if (db.Name.Trim().ToUpper() == e.SelectedDatabase.ToUpper())
                    selectedDb = db.Name.Trim();
            }
            this.databaseDropDown.SelectedItem = selectedDb;
            App.LoadingDatabases = false;
        }

        /// <summary>Handles an <see cref="Edrych.App.ActiveQueryChanged"/> event</summary>
        private void ActiveQuery_Changed(object sender, EventArgs e)
        {
            this._activeQuery = sender as QueryView;
        }

        /// <summary>Handles a <see cref="Edrych.App.SwitchDatabases"/> event</summary>
        private void SwitchDatabases(object sender, EventArgs e)
        {
            this.databaseDropDown.Focus();
        }

        /// <summary>Handles a <see cref="Edrych.App.DatabaseChanged"/> event</summary>
        private void DatabaseChanged(object sender, ConnectionChangedEventArgs e)
        {
            if (e != null)
            {
                this.databaseDropDown.SelectedItem = e.SelectedDatabase;
            }
        }

        #endregion

        #region Private Methods - Drag & Drop

        private void TreeView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left) return;

            TreeViewAdv tree = sender as TreeViewAdv;
            TreeNodeAdv node = tree.GetNodeAt(e.Location);
            if (node != null)
            {
                tree.DoDragDrop(this._browserViewModel.GetNodeName(node), DragDropEffects.Copy | DragDropEffects.Move);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>Creates a new query tab</summary>
        /// <param name="IsOpen">Whether or not the query is from an existing saved query</param>
        private void CreateQueryTab(bool IsOpen)
        {
            QueryView qp = new QueryView(ref _browserViewModel);
            this.tabControl1.TabPages.Insert(0, qp);
            qp.CreateQueryView(IsOpen);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.ResizeTabs();
        }

        /// <summary>Adds all event handlers and establishes state of the menus</summary>
        private void InitializeMenus()
        {
            App.PropertyChanged += this.Property_Changed;
            this.treeViewAdv1.SelectionChanged += this.TreeSelection_Changed;
            App.ConnectionChanged += this.QueryConnection_Changed;
            App.ActiveQueryChanged += this.ActiveQuery_Changed;
            App.SwitchDatabases += this.SwitchDatabases;
            App.DatabaseChanged += this.DatabaseChanged;

            App.IsCopyEnabled = false;
            App.IsPasteEnabled = false;
            App.IsUndoEnabled = false;
            App.IsRedoEnabled = false;
            App.IsSelectAllEnabled = false;
            App.IsQueryMenuVisible = false;
            App.IsQueryConnectEnabled = false;
            App.IsQueryDisconnectEnabled = false;
        }

        /// <summary>Adds all the event handlers and states of the browser</summary>
        private void InitializeServerBrowser()
        {
            _browserViewModel = new ServerBrowserViewModel();
            this.treeViewAdv1.Model = _browserViewModel.Tree;
        }

        #endregion

    }
}
