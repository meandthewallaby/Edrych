using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Aga.Controls.Tree;
using Edrych.DataAccess;
using Edrych.Dialogs;
using Edrych.Models;

namespace Edrych.ViewModels
{
    /// <summary>ViewModel that handles the browser tree model</summary>
    class ServerBrowserViewModel : IDisposable
    {
        #region Private/Global Variables

        private DataAccessBase _activeConnection;
        private ServerBrowserModel _tree;

        #endregion

        #region Constructor(s)

        /// <summary>Initializes the browser and opens a connection</summary>
        public ServerBrowserViewModel()
        {
            _tree = new ServerBrowserModel(this);
            CreateConnection();
        }

        #endregion

        #region public Properties

        /// <summary>Gets the active data access object</summary>
        public DataAccessBase ActiveConnection
        {
            get { return _activeConnection; }
        }

        /// <summary>Gets the model for the browser</summary>
        public ServerBrowserModel Tree
        {
            get { return _tree; }
        }

        #endregion

        #region Public Methods - Called from View

        /// <summary>Creates a connection on the browser</summary>
        public void CreateConnection()
        {
            ConnectDialog cd = new ConnectDialog();
            cd.ShowDialog();
            if (cd.DataAccess != null)
            {
                if (!this._tree.Cache.ContainsKey("ROOT") || this._tree.Cache["ROOT"].FirstOrDefault(d => d.Name == cd.DataAccess.DataSource) == null)
                {
                    this.AddServer(cd.DataAccess);
                    this._activeConnection = cd.DataAccess;
                }
                else
                {
                    MessageBox.Show("This server is already added!");
                }
            }
        }

        /// <summary>Refreshes the node in the tree</summary>
        /// <param name="SelectedNode">Node to refresh</param>
        public void RefreshNode(TreeNodeAdv SelectedNode)
        {
            if (SelectedNode != null)
            {
                BaseItem item = SelectedNode.Tag as BaseItem;
                List<BaseItem> items = new List<BaseItem>();
                items.Add(item);
                BaseItem parent = item.Parent;
                while (parent != null)
                {
                    items.Insert(0, parent);
                    parent = parent.Parent;
                }

                TreePath path = new TreePath(items.ToArray());
                RemoveCachedItems(_tree.Cache.Keys.Where(k => k.StartsWith(item.ItemPath)).ToList());

                _tree.OnStructureChanged(path);
            }
        }

        /// <summary>Removes a server from the tree</summary>
        /// <param name="SelectedNode">Node to remove</param>
        public void RemoveConnection(TreeNodeAdv SelectedNode)
        {
            //Find the server
            ServerItem server = null;
            while (server == null)
            {
                if (SelectedNode == null)
                {
                    break;
                }
                BaseItem item = SelectedNode.Tag as BaseItem;

                if (item.Type == ItemType.Server)
                {
                    server = SelectedNode.Tag as ServerItem;
                    break;
                }

                SelectedNode = SelectedNode.Parent;
            }

            if (server != null)
            {
                //Next, clear the cache
                int index = SelectedNode.Index;
                _tree.Cache["ROOT"].Remove(server as BaseItem);
                List<BaseItem> serverChildren = _tree.Cache.ContainsKey(server.ItemPath) ? serverChildren = _tree.Cache[server.ItemPath] : new List<BaseItem>();
                RemoveCachedItems(_tree.Cache.Keys.Where(k => k.StartsWith(server.ItemPath)).ToList());
                server.Disposal();

                //Finally, fire the event off
                _tree.OnNodesRemoved(TreePath.Empty, new int[] { index }, new object[] { server });
            }
        }

        /// <summary>Updates the active connection with the selected database</summary>
        /// <param name="Node">Node that drives the update</param>
        public void UpdateActiveConnection(TreeNodeAdv Node)
        {
            TreePath path = GetNodePath(Node);
            if (path.IsEmpty() == false)
            {
                ServerItem server = path.FirstNode as ServerItem;
                BaseItem db = null;
                if (path.FullPath.GetLength(0) > 1)
                {
                     db = path.FullPath[1] as BaseItem;
                }
                if (server != null && server.DataAccess != null)
                {
                    if (server.DataAccess.DataSource != this._activeConnection.DataSource)
                        this._activeConnection = server.DataAccess;
                    if (db != null)
                        this._activeConnection.SetDatabase(db.Name);
                }
            }
        }

        /// <summary>Grabs the name of the selected node</summary>
        /// <param name="Node">Node that's selected</param>
        /// <returns>Name of the given node</returns>
        public string GetNodeName(TreeNodeAdv Node)
        {
            return ((BaseItem)Node.Tag).Name;
        }

        #endregion

        #region Public Methods - Called from Model

        /// <summary>Grabs the children of the passed path</summary>
        /// <param name="treePath">Path to get the children of</param>
        /// <returns>IEnumerable collection with the child items</returns>
        public IEnumerable GetChildren(TreePath treePath)
        {
            List<BaseItem> items = new List<BaseItem>();
            if (treePath.IsEmpty())
            {
                if (_tree.Cache.ContainsKey("ROOT"))
                {
                    items = _tree.Cache["ROOT"];
                }
                else
                {
                    items = new List<BaseItem>();
                    _tree.Cache.Add("ROOT", items);
                }
            }
            else
            {
                ServerItem server = treePath.FirstNode as ServerItem;
                BaseItem database = treePath.FullPath.FirstOrDefault(i => (i as BaseItem).Type == ItemType.Database) as BaseItem;
                BaseItem parent = treePath.LastNode as BaseItem;
                if (parent != null && server != null)
                {
                    if (_tree.Cache.ContainsKey(parent.ItemPath))
                    {
                        items = _tree.Cache[parent.ItemPath];
                    }
                    else
                    {
                        //Populate the items based on the parent
                        switch (parent.Type)
                        {
                            case ItemType.Server:
                                items = GetDatabases(server);
                                break;
                            case ItemType.Database:
                                items = GetDatabaseChildren(parent);
                                break;
                            case ItemType.Folder:
                                if (database != null)
                                    items = GetFolderChildren(parent, server, database.Name);
                                break;
                            case ItemType.Table:
                            case ItemType.View:
                                items = GetTableViewChildren(parent);
                                break;
                            default:
                                break;
                        }

                        _tree.Cache.Add(parent.ItemPath, items);
                    }

                    parent.IsLoaded = true;
                }
            }

            return items;
        }

        /// <summary>Determines whether or not a node is a leaf node</summary>
        /// <param name="treePath">Path to test</param>
        /// <returns>Boolean whether the node is a leaf</returns>
        public bool IsLeaf(TreePath treePath)
        {
            return treePath.LastNode != null && ((BaseItem)treePath.LastNode).Type == ItemType.Column;
        }

        #endregion

        #region Public Methods

        /// <summary>Disposes of the item</summary>
        public void Dispose()
        {
            if (_activeConnection != null)
                _activeConnection.Dispose();
        }

        #endregion

        #region Private Methods - Get Children

        /// <summary>Gets the databases for a server</summary>
        /// <param name="server">Server to get the databases of</param>
        /// <returns>List of database items</returns>
        private List<BaseItem> GetDatabases(ServerItem server)
        {
            List<BaseItem> items = new List<BaseItem>();
            List<Database> databases = server.DataAccess.GetDatabases();

            foreach (Database db in databases)
            {
                BaseItem dataItem = new BaseItem(ItemType.Database, db.Name, server);
                items.Add(dataItem);
            }

            return items;
        }

        /// <summary>Gets the folders under a database</summary>
        /// <param name="parent">Database to get the children of</param>
        /// <returns>List of folders</returns>
        private List<BaseItem> GetDatabaseChildren(BaseItem parent)
        {
            List<BaseItem> items = new List<BaseItem>();

            items.Add(new BaseItem(ItemType.Folder, "Tables", parent));
            items.Add(new BaseItem(ItemType.Folder, "Views", parent));

            return items;
        }

        /// <summary>Gets the folders under a table or view</summary>
        /// <param name="parent">Table or view to get the children of</param>
        /// <returns>List of folders</returns>
        private List<BaseItem> GetTableViewChildren(BaseItem parent)
        {
            List<BaseItem> items = new List<BaseItem>();

            items.Add(new BaseItem(ItemType.Folder, "Columns", parent));
            items.Add(new BaseItem(ItemType.Folder, "Keys", parent));

            return items;
        }

        /// <summary>Gets the tables or views</summary>
        /// <param name="parent">Folder that was expanded</param>
        /// <param name="server">Server item the tables are under</param>
        /// <param name="DatabaseName">Name of the database to look in</param>
        /// <returns>List of table or view items</returns>
        private List<BaseItem> GetFolderChildren(BaseItem parent, ServerItem server, string DatabaseName)
        {
            List<BaseItem> items = new List<BaseItem>();

            string oldDatabase = server.DataAccess.SelectedDatabase;
            server.DataAccess.SetDatabase(DatabaseName);

            switch (parent.Name)
            {
                case "Tables":
                    items = GetTableViews(ItemType.Table, parent, server.DataAccess.GetTables());
                    break;
                case "Views":
                    items = GetTableViews(ItemType.View, parent, server.DataAccess.GetViews());
                    break;
                case "Columns":
                    items = GetColumns(parent, server.DataAccess.GetColumns(parent.Parent.Name));
                    break;
                case "Keys":
                    items = GetKeys(parent, server.DataAccess.GetKeys(parent.Parent.Name));
                    break;
            }

            server.DataAccess.SetDatabase(oldDatabase);

            return items;
        }

        /// <summary>Gets the columns in a table or view</summary>
        /// <param name="parent">Table or view that was expanded</param>
        /// <param name="server">Server item the tables are under</param>
        /// <param name="DatabaseName">Name of the database to look in</param>
        /// <returns>List of column items</returns>
        private List<BaseItem> GetTableViews(ItemType type, BaseItem parent, List<TableView> tableViews)
        {
            List<BaseItem> items = new List<BaseItem>();
            foreach (TableView table in tableViews)
            {
                items.Add(new BaseItem(type, table.Name, parent));
            }
            return items;
        }

        /// <summary>Gets the columns in a table or view</summary>
        /// <param name="parent">Table or view that was expanded</param>
        /// <param name="server">Server item the tables are under</param>
        /// <param name="DatabaseName">Name of the database to look in</param>
        /// <returns>List of column items</returns>
        private List<BaseItem> GetColumns(BaseItem parent, List<Column> columns)
        {
            List<BaseItem> items = new List<BaseItem>();
            foreach (Column col in columns)
            {
                items.Add(new ColumnKeyItem(col.Name + " (" + col.DataType + ", " + (col.IsNullable ? "null" : "not null") + ")", parent, col.Key));
            }

            return items;
        }

        /// <summary>Gets the keys in a table or view</summary>
        /// <param name="parent">Table or view that was expanded</param>
        /// <param name="server">Server item the tables are under</param>
        /// <param name="DatabaseName">Name of the database to look in</param>
        /// <returns>List of column items</returns>
        private List<BaseItem> GetKeys(BaseItem parent, List<Key> keys)
        {
            List<BaseItem> items = new List<BaseItem>();
            foreach (Key key in keys)
            {
                items.Add(new ColumnKeyItem(key.Name, parent, key.Type));
            }

            return items;
        }

        #endregion

        #region Private Methods

        /// <summary>Adds a server to the tree</summary>
        /// <param name="dataAccess">Data access object to associate with the server</param>
        private void AddServer(DataAccessBase dataAccess)
        {
            ServerItem item = new ServerItem(dataAccess.DataSource);
            item.DataAccess = dataAccess;
            if (_tree.Cache.ContainsKey("ROOT"))
            {
                _tree.Cache["ROOT"].Add(item);
            }
            else
            {
                List<BaseItem> items = new List<BaseItem>();
                items.Add(item);
                _tree.Cache.Add("ROOT", items);
            }

            _tree.OnNodesInserted(TreePath.Empty, new int[] { _tree.Cache["ROOT"].Count - 1 }, new object[] { item });
        }

        /// <summary>Removes items from the cache</summary>
        /// <param name="oldCache">Keys to remove from the cache</param>
        private void RemoveCachedItems(List<string> oldCache)
        {
            foreach (string key in oldCache)
            {
                foreach (BaseItem oldItem in _tree.Cache[key])
                {
                    if (oldItem.Type == ItemType.Server)
                        ((ServerItem)oldItem).Disposal();
                    else
                        oldItem.Dispose();
                }
                _tree.Cache.Remove(key);
            }
        }

        /// <summary>Gets the path of a given node</summary>
        /// <param name="Node">Node to trace</param>
        /// <returns>TreePath object representing the total path of the node</returns>
        private TreePath GetNodePath(TreeNodeAdv Node)
        {
            BaseItem item = Node.Tag as BaseItem;
            List<BaseItem> items = new List<BaseItem>();
            while (item != null)
            {
                items.Insert(0, item);
                item = item.Parent;
            }

            return new TreePath(items.ToArray());
        }
       
        #endregion
    }
}