using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aga.Controls.Tree;
using SQLiteBrowser.DataAccess;

namespace SQLiteBrowser.Models
{
    public class ServerBrowserModel : ITreeModel
    {
        #region Private/Global Variables

        private Dictionary<string, List<BaseItem>> _cache = new Dictionary<string, List<BaseItem>>();

        #endregion

        #region Public Methods - ITreeModel Interface

        public IEnumerable GetChildren(TreePath treePath)
        {
            List<BaseItem> items = new List<BaseItem>();
            if (treePath.IsEmpty())
            {
                if (_cache.ContainsKey("ROOT"))
                {
                    items = _cache["ROOT"];
                }
                else
                {
                    items = new List<BaseItem>();
                    _cache.Add("ROOT", items);
                }
            }
            else
            {
                ServerItem server = treePath.FirstNode as ServerItem;
                BaseItem database = treePath.FullPath.FirstOrDefault(i => (i as BaseItem).Type == ItemType.Database) as BaseItem;
                BaseItem parent = treePath.LastNode as BaseItem;
                if (parent != null && server != null)
                {
                    if (_cache.ContainsKey(parent.ItemPath))
                    {
                        items = _cache[parent.ItemPath];
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
                                if(database != null)
                                    items = GetFolderChildren(parent, server, database.Name);
                                break;
                            case ItemType.Table:
                            case ItemType.View:
                                if (database != null)
                                    items = GetColumns(parent, server, database.Name);
                                break;
                            default:
                                break;
                        }

                        _cache.Add(parent.ItemPath, items);
                    }

                    parent.IsLoaded = true;
                }
            }

            return items;
        }

        public bool IsLeaf(TreePath treePath)
        {
            return treePath.LastNode != null && ((BaseItem)treePath.LastNode).Type == ItemType.Column;
        }

        #endregion

        #region Public Methods - Manipulating Nodes

        public void AddServer(DataAccessBase dataAccess)
        {
            ServerItem item = new ServerItem(dataAccess.DataSource);
            item.DataAccess = dataAccess;
            if (_cache.ContainsKey("ROOT"))
            {
                _cache["ROOT"].Add(item);
            }
            else
            {
                List<BaseItem> items = new List<BaseItem>();
                items.Add(item);
                _cache.Add("ROOT", items);
            }
            OnStructureChanged(TreePath.Empty);
        }

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
                RemoveCachedItems(_cache.Keys.Where(k => k.StartsWith(item.ItemPath)).ToList());
                
                OnStructureChanged(path);
            }
        }

        public void RemoveServer(TreeNodeAdv SelectedNode)
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
                _cache["ROOT"].Remove(server as BaseItem);
                List<BaseItem> serverChildren = _cache.ContainsKey(server.ItemPath) ? serverChildren = _cache[server.ItemPath] : new List<BaseItem>();
                RemoveCachedItems(_cache.Keys.Where(k => k.StartsWith(server.ItemPath)).ToList());
                server.Dispose();

                //Finally, fire the event off
                OnStructureChanged(TreePath.Empty);
            }
        }

        #endregion

        #region Public Events

        public event EventHandler<TreeModelEventArgs> NodesChanged;

        public event EventHandler<TreeModelEventArgs> NodesInserted;

        public event EventHandler<TreeModelEventArgs> NodesRemoved;

        public event EventHandler<TreePathEventArgs> StructureChanged;

        #endregion

        #region Private Methods

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

        private List<BaseItem> GetDatabaseChildren(BaseItem parent)
        {
            List<BaseItem> items = new List<BaseItem>();

            BaseItem tables = new BaseItem(ItemType.Folder, "Tables", parent);
            BaseItem views = new BaseItem(ItemType.Folder, "Views", parent);

            items.Add(tables);
            items.Add(views);

            return items;
        }

        private List<BaseItem> GetFolderChildren(BaseItem parent, ServerItem server, string DatabaseName)
        {
            List<BaseItem> items = new List<BaseItem>();
            ItemType type = ItemType.Table;
            List<TableView> tableViews = null;

            string oldDatabase = server.DataAccess.SelectedDatabase;
            server.DataAccess.SetDatabase(DatabaseName);
            if (parent.Name == "Tables")
            {
                tableViews = server.DataAccess.GetTables();
                type = ItemType.Table;
            }
            else if (parent.Name == "Views")
            {
                tableViews = server.DataAccess.GetViews();
                type = ItemType.View;
            }
            server.DataAccess.SetDatabase(oldDatabase);

            if (tableViews != null)
            {
                foreach (TableView table in tableViews)
                {
                    items.Add(new BaseItem(type, table.Name, parent));
                }
            }
            
            return items;
        }

        private List<BaseItem> GetColumns(BaseItem parent, ServerItem server, string DatabaseName)
        {
            List<BaseItem> items = new List<BaseItem>();
            string oldDatabase = server.DataAccess.SelectedDatabase;
            server.DataAccess.SetDatabase(DatabaseName);
            foreach (Column col in server.DataAccess.GetColumns(parent.Name))
            {
                items.Add(new BaseItem(ItemType.Column, col.Name + " (" + col.DataType + ")", parent));
            }
            server.DataAccess.SetDatabase(oldDatabase);

            return items;
        }

        private void RemoveCachedItems(List<string> oldCache)
        {
            foreach (string key in oldCache)
            {
                foreach (BaseItem oldItem in _cache[key])
                {
                    if (oldItem.Type == ItemType.Server)
                        ((ServerItem)oldItem).Dispose();
                    else
                        oldItem.Dispose();
                }
                _cache.Remove(key);
            }
        }

        #endregion

        #region Private Event Triggers

        private void OnNodesInserted(TreePath Tree, object[] Children)
        {
            if (NodesInserted != null)
            {
                NodesInserted(this, new TreeModelEventArgs(Tree, Children));
            }
        }

        private void OnNodesChanged(TreePath Tree, object[] Children)
        {
            if (NodesChanged != null)
            {
                NodesChanged(this, new TreeModelEventArgs(Tree, Children));
            }
        }

        private void OnNodesRemoved(TreePath Tree, object[] Children)
        {
            if (NodesRemoved != null)
            {
                NodesRemoved(this, new TreeModelEventArgs(Tree, Children));
            }
        }

        private void OnStructureChanged(TreePath Tree)
        {
            if (StructureChanged != null)
            {
                StructureChanged(this, new TreePathEventArgs(Tree));
            }
        }

        #endregion
    }
}
