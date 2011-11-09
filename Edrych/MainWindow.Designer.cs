namespace Edrych
{
    using Edrych.Helpers;
    using Edrych.Properties;

    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStripContainer2 = new System.Windows.Forms.ToolStripContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStripContainer3 = new System.Windows.Forms.ToolStripContainer();
            this.treeViewAdv1 = new Aga.Controls.Tree.TreeViewAdv();
            this.treeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newQueryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.contextRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.expandingIcon1 = new Aga.Controls.Tree.NodeControls.ExpandingIcon();
            this._icon = new Aga.Controls.Tree.NodeControls.NodeIcon();
            this._name = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.connectToolStrip = new System.Windows.Forms.ToolStripButton();
            this.disconnectToolStrip = new System.Windows.Forms.ToolStripButton();
            this.tabControl1 = new Edrych.Helpers.TabControlExt();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.connectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.queryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.queryConnectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.queryDisconnectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.newToolStrip = new System.Windows.Forms.ToolStripButton();
            this.openToolStrip = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.queryConnectToolStrip = new System.Windows.Forms.ToolStripButton();
            this.queryDisconnectToolStrip = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.databaseDropDown = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripContainer1.SuspendLayout();
            this.toolStripContainer2.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer2.ContentPanel.SuspendLayout();
            this.toolStripContainer2.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStripContainer3.ContentPanel.SuspendLayout();
            this.toolStripContainer3.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer3.SuspendLayout();
            this.treeContextMenu.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(903, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(150, 150);
            this.toolStripContainer1.Location = new System.Drawing.Point(8, 8);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(150, 175);
            this.toolStripContainer1.TabIndex = 2;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer2
            // 
            // 
            // toolStripContainer2.BottomToolStripPanel
            // 
            this.toolStripContainer2.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer2.ContentPanel
            // 
            this.toolStripContainer2.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer2.ContentPanel.Size = new System.Drawing.Size(903, 453);
            this.toolStripContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer2.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer2.Name = "toolStripContainer2";
            this.toolStripContainer2.Size = new System.Drawing.Size(903, 524);
            this.toolStripContainer2.TabIndex = 3;
            this.toolStripContainer2.Text = "toolStripContainer2";
            // 
            // toolStripContainer2.TopToolStripPanel
            // 
            this.toolStripContainer2.TopToolStripPanel.Controls.Add(this.menuStrip1);
            this.toolStripContainer2.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.toolStripContainer3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(903, 453);
            this.splitContainer1.SplitterDistance = 232;
            this.splitContainer1.TabIndex = 0;
            // 
            // toolStripContainer3
            // 
            // 
            // toolStripContainer3.ContentPanel
            // 
            this.toolStripContainer3.ContentPanel.Controls.Add(this.treeViewAdv1);
            this.toolStripContainer3.ContentPanel.Size = new System.Drawing.Size(232, 428);
            this.toolStripContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer3.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer3.Name = "toolStripContainer3";
            this.toolStripContainer3.Size = new System.Drawing.Size(232, 453);
            this.toolStripContainer3.TabIndex = 1;
            this.toolStripContainer3.Text = "toolStripContainer3";
            // 
            // toolStripContainer3.TopToolStripPanel
            // 
            this.toolStripContainer3.TopToolStripPanel.Controls.Add(this.toolStrip2);
            // 
            // treeViewAdv1
            // 
            this.treeViewAdv1.AsyncExpanding = true;
            this.treeViewAdv1.BackColor = System.Drawing.SystemColors.Window;
            this.treeViewAdv1.ContextMenuStrip = this.treeContextMenu;
            this.treeViewAdv1.DefaultToolTipProvider = null;
            this.treeViewAdv1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewAdv1.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeViewAdv1.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeViewAdv1.LoadOnDemand = true;
            this.treeViewAdv1.Location = new System.Drawing.Point(0, 0);
            this.treeViewAdv1.Model = null;
            this.treeViewAdv1.Name = "treeViewAdv1";
            this.treeViewAdv1.NodeControls.Add(this.expandingIcon1);
            this.treeViewAdv1.NodeControls.Add(this._icon);
            this.treeViewAdv1.NodeControls.Add(this._name);
            this.treeViewAdv1.SelectedNode = null;
            this.treeViewAdv1.Size = new System.Drawing.Size(232, 428);
            this.treeViewAdv1.TabIndex = 0;
            this.treeViewAdv1.Text = "treeViewAdv1";
            // 
            // treeContextMenu
            // 
            this.treeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newQueryToolStripMenuItem,
            this.toolStripSeparator9,
            this.contextRefresh});
            this.treeContextMenu.Name = "treeContextMenu";
            this.treeContextMenu.Size = new System.Drawing.Size(153, 76);
            this.treeContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenu_Opening);
            // 
            // newQueryToolStripMenuItem
            // 
            this.newQueryToolStripMenuItem.Image = global::Edrych.Properties.Resources.newIcon;
            this.newQueryToolStripMenuItem.Name = "newQueryToolStripMenuItem";
            this.newQueryToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newQueryToolStripMenuItem.Text = "New Query";
            this.newQueryToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(149, 6);
            // 
            // contextRefresh
            // 
            this.contextRefresh.Image = global::Edrych.Properties.Resources.refresh;
            this.contextRefresh.Name = "contextRefresh";
            this.contextRefresh.Size = new System.Drawing.Size(152, 22);
            this.contextRefresh.Text = "Refresh";
            this.contextRefresh.Click += new System.EventHandler(this.contextRefresh_Click);
            // 
            // expandingIcon1
            // 
            this.expandingIcon1.LeftMargin = 0;
            this.expandingIcon1.ParentColumn = null;
            // 
            // _icon
            // 
            this._icon.DataPropertyName = "Icon";
            this._icon.LeftMargin = 1;
            this._icon.ParentColumn = null;
            this._icon.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
            // 
            // _name
            // 
            this._name.DataPropertyName = "Name";
            this._name.IncrementalSearchEnabled = true;
            this._name.LeftMargin = 3;
            this._name.ParentColumn = null;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStrip,
            this.disconnectToolStrip});
            this.toolStrip2.Location = new System.Drawing.Point(3, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(58, 25);
            this.toolStrip2.TabIndex = 0;
            // 
            // connectToolStrip
            // 
            this.connectToolStrip.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.connectToolStrip.Image = global::Edrych.Properties.Resources.connect;
            this.connectToolStrip.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.connectToolStrip.Name = "connectToolStrip";
            this.connectToolStrip.Size = new System.Drawing.Size(23, 22);
            this.connectToolStrip.Text = "toolStripButton1";
            this.connectToolStrip.ToolTipText = "Add a connection to the Browser";
            this.connectToolStrip.Click += new System.EventHandler(this.connectMenuItem_Click);
            // 
            // disconnectToolStrip
            // 
            this.disconnectToolStrip.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.disconnectToolStrip.Enabled = false;
            this.disconnectToolStrip.Image = global::Edrych.Properties.Resources.disconnect;
            this.disconnectToolStrip.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.disconnectToolStrip.Name = "disconnectToolStrip";
            this.disconnectToolStrip.Size = new System.Drawing.Size(23, 22);
            this.disconnectToolStrip.Text = "toolStripButton2";
            this.disconnectToolStrip.ToolTipText = "Remove a connection from the Browser";
            this.disconnectToolStrip.Click += new System.EventHandler(this.disconnectMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(667, 453);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.queryToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(903, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator6,
            this.connectMenuItem,
            this.disconnectMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.printToolStripMenuItem,
            this.printPreviewToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = global::Edrych.Properties.Resources.newIcon;
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::Edrych.Properties.Resources.open;
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(143, 6);
            // 
            // connectMenuItem
            // 
            this.connectMenuItem.Image = global::Edrych.Properties.Resources.connect;
            this.connectMenuItem.Name = "connectMenuItem";
            this.connectMenuItem.Size = new System.Drawing.Size(146, 22);
            this.connectMenuItem.Text = "Connect";
            this.connectMenuItem.Click += new System.EventHandler(this.connectMenuItem_Click);
            // 
            // disconnectMenuItem
            // 
            this.disconnectMenuItem.Enabled = false;
            this.disconnectMenuItem.Image = global::Edrych.Properties.Resources.disconnect;
            this.disconnectMenuItem.Name = "disconnectMenuItem";
            this.disconnectMenuItem.Size = new System.Drawing.Size(146, 22);
            this.disconnectMenuItem.Text = "Disconnect";
            this.disconnectMenuItem.Click += new System.EventHandler(this.disconnectMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(143, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripMenuItem.Image")));
            this.printToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.printToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.printToolStripMenuItem.Text = "&Print";
            // 
            // printPreviewToolStripMenuItem
            // 
            this.printPreviewToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printPreviewToolStripMenuItem.Image")));
            this.printPreviewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
            this.printPreviewToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.printPreviewToolStripMenuItem.Text = "Print Pre&view";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(143, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator3,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator4,
            this.selectAllToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.undoToolStripMenuItem.Text = "&Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.redoToolStripMenuItem.Text = "&Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(161, 6);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripMenuItem.Image")));
            this.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.cutToolStripMenuItem.Text = "Cu&t";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
            this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.copyToolStripMenuItem.Text = "&Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripMenuItem.Image")));
            this.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.pasteToolStripMenuItem.Text = "&Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(161, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.selectAllToolStripMenuItem.Text = "Select &All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // queryToolStripMenuItem
            // 
            this.queryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.queryConnectMenuItem,
            this.queryDisconnectMenuItem});
            this.queryToolStripMenuItem.Name = "queryToolStripMenuItem";
            this.queryToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.queryToolStripMenuItem.Text = "&Query";
            this.queryToolStripMenuItem.Visible = false;
            // 
            // queryConnectMenuItem
            // 
            this.queryConnectMenuItem.Image = global::Edrych.Properties.Resources.connect;
            this.queryConnectMenuItem.Name = "queryConnectMenuItem";
            this.queryConnectMenuItem.Size = new System.Drawing.Size(133, 22);
            this.queryConnectMenuItem.Text = "Connect";
            this.queryConnectMenuItem.Click += new System.EventHandler(this.queryConnectMenuItem_Click);
            // 
            // queryDisconnectMenuItem
            // 
            this.queryDisconnectMenuItem.Image = global::Edrych.Properties.Resources.disconnect;
            this.queryDisconnectMenuItem.Name = "queryDisconnectMenuItem";
            this.queryDisconnectMenuItem.Size = new System.Drawing.Size(133, 22);
            this.queryDisconnectMenuItem.Text = "Disconnect";
            this.queryDisconnectMenuItem.Click += new System.EventHandler(this.queryDisconnectMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.optionsToolStripMenuItem.Text = "&Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(12, 20);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStrip,
            this.openToolStrip,
            this.toolStripSeparator7,
            this.queryConnectToolStrip,
            this.queryDisconnectToolStrip,
            this.toolStripSeparator8,
            this.databaseDropDown});
            this.toolStrip1.Location = new System.Drawing.Point(3, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(239, 25);
            this.toolStrip1.TabIndex = 1;
            // 
            // newToolStrip
            // 
            this.newToolStrip.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newToolStrip.Image = global::Edrych.Properties.Resources.newIcon;
            this.newToolStrip.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStrip.Name = "newToolStrip";
            this.newToolStrip.Size = new System.Drawing.Size(23, 22);
            this.newToolStrip.Text = "toolStripButton1";
            this.newToolStrip.ToolTipText = "Create a new query";
            this.newToolStrip.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStrip
            // 
            this.openToolStrip.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolStrip.Image = global::Edrych.Properties.Resources.open;
            this.openToolStrip.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStrip.Name = "openToolStrip";
            this.openToolStrip.Size = new System.Drawing.Size(23, 22);
            this.openToolStrip.Text = "toolStripButton2";
            this.openToolStrip.ToolTipText = "Open a query";
            this.openToolStrip.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // queryConnectToolStrip
            // 
            this.queryConnectToolStrip.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.queryConnectToolStrip.Enabled = false;
            this.queryConnectToolStrip.Image = global::Edrych.Properties.Resources.connect;
            this.queryConnectToolStrip.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.queryConnectToolStrip.Name = "queryConnectToolStrip";
            this.queryConnectToolStrip.Size = new System.Drawing.Size(23, 22);
            this.queryConnectToolStrip.Text = "toolStripButton1";
            this.queryConnectToolStrip.ToolTipText = "Connect the query";
            this.queryConnectToolStrip.Click += new System.EventHandler(this.queryConnectMenuItem_Click);
            // 
            // queryDisconnectToolStrip
            // 
            this.queryDisconnectToolStrip.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.queryDisconnectToolStrip.Enabled = false;
            this.queryDisconnectToolStrip.Image = global::Edrych.Properties.Resources.disconnect;
            this.queryDisconnectToolStrip.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.queryDisconnectToolStrip.Name = "queryDisconnectToolStrip";
            this.queryDisconnectToolStrip.Size = new System.Drawing.Size(23, 22);
            this.queryDisconnectToolStrip.Text = "toolStripButton2";
            this.queryDisconnectToolStrip.ToolTipText = "Disconnect the query";
            this.queryDisconnectToolStrip.Click += new System.EventHandler(this.queryDisconnectMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // databaseDropDown
            // 
            this.databaseDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.databaseDropDown.Enabled = false;
            this.databaseDropDown.Name = "databaseDropDown";
            this.databaseDropDown.Size = new System.Drawing.Size(121, 25);
            this.databaseDropDown.Sorted = true;
            this.databaseDropDown.SelectedIndexChanged += new System.EventHandler(this.databaseDropDown_SelectionChanged);
            this.databaseDropDown.KeyUp += new System.Windows.Forms.KeyEventHandler(this.databaseDropDown_KeyUp);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(903, 524);
            this.Controls.Add(this.toolStripContainer2);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.Text = "Edrych";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Window_Closing);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStripContainer2.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer2.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer2.ContentPanel.ResumeLayout(false);
            this.toolStripContainer2.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer2.TopToolStripPanel.PerformLayout();
            this.toolStripContainer2.ResumeLayout(false);
            this.toolStripContainer2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStripContainer3.ContentPanel.ResumeLayout(false);
            this.toolStripContainer3.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer3.TopToolStripPanel.PerformLayout();
            this.toolStripContainer3.ResumeLayout(false);
            this.toolStripContainer3.PerformLayout();
            this.treeContextMenu.ResumeLayout(false);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printPreviewToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private TabControlExt tabControl1;
        private Aga.Controls.Tree.TreeViewAdv treeViewAdv1;
        private Aga.Controls.Tree.NodeControls.NodeTextBox _name;
        private Aga.Controls.Tree.NodeControls.NodeIcon _icon;
        private Aga.Controls.Tree.NodeControls.ExpandingIcon expandingIcon1;
        private System.Windows.Forms.ContextMenuStrip treeContextMenu;
        private System.Windows.Forms.ToolStripMenuItem contextRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem connectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disconnectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem queryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem queryConnectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem queryDisconnectMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton queryConnectToolStrip;
        private System.Windows.Forms.ToolStripButton queryDisconnectToolStrip;
        private System.Windows.Forms.ToolStripButton newToolStrip;
        private System.Windows.Forms.ToolStripButton openToolStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripComboBox databaseDropDown;
        private System.Windows.Forms.ToolStripContainer toolStripContainer3;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton connectToolStrip;
        private System.Windows.Forms.ToolStripButton disconnectToolStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem newQueryToolStripMenuItem;
    }
}

