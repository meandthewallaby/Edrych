using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Aga.Controls;
using Aga.Controls.Tree;
using SQLiteBrowser.DataAccess;
using SQLiteBrowser.Properties;

namespace SQLiteBrowser.Models
{
    public class TreeModel : ITreeModel
    {
        #region Private/Global Variables

        private Dictionary<string, List<BaseItem>> _cache = new Dictionary<string, List<BaseItem>>();

        #endregion

        #region Public Methods

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
            OnStructureChanged(TreePath.Empty, _cache["ROOT"].ToArray());
        }

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
                DatabaseItem database = treePath.FullPath.FirstOrDefault(i => (i as BaseItem).Type == ItemType.Database) as DatabaseItem;
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
                                    items = GetFolderChildren(parent, database);
                                break;
                            case ItemType.Table:
                            case ItemType.View:
                                if (database != null)
                                    items = GetColumns(parent, database);
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
                List<string> oldCache = _cache.Keys.Where(k => k.StartsWith(item.ItemPath)).ToList();
                foreach (string key in oldCache)
                {
                    _cache.Remove(key);
                }
                List<BaseItem> children = (List<BaseItem>)GetChildren(path);
                OnStructureChanged(path, children.ToArray());
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
                DatabaseItem dataItem = new DatabaseItem(db.Name, server);
                dataItem.Database = server.DataAccess;
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

        private List<BaseItem> GetFolderChildren(BaseItem parent, DatabaseItem database)
        {
            List<BaseItem> items = new List<BaseItem>();
            string findSql = string.Empty;
            ItemType type = ItemType.Table;
            List<TableView> tableViews = null;

            if (parent.Name == "Tables")
            {
                tableViews = database.Database.GetTables();
                type = ItemType.Table;
            }
            else if (parent.Name == "Views")
            {
                tableViews = database.Database.GetViews();
                type = ItemType.View;
            }

            if (tableViews != null)
            {
                foreach (TableView table in tableViews)
                {
                    items.Add(new BaseItem(type, table.Name, parent));
                }
            }
            
            return items;
        }

        private List<BaseItem> GetColumns(BaseItem parent, DatabaseItem database)
        {
            List<BaseItem> items = new List<BaseItem>();

            foreach (Column col in database.Database.GetColumns(parent.Name))
            {
                items.Add(new BaseItem(ItemType.Column, col.Name + " (" + col.DataType + ")", parent));
            }

            return items;
        }

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

        private void OnStructureChanged(TreePath Tree, object[] Children)
        {
            if (StructureChanged != null)
            {
                StructureChanged(this, new TreeModelEventArgs(Tree, Children));
            }
        }

        #endregion
    }
}
