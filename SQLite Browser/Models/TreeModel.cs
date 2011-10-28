using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Aga.Controls;
using Aga.Controls.Tree;
using SQLiteBrowser.DataAccess;
using SQLiteBrowser.Resources;

namespace SQLiteBrowser.Models
{
    public class TreeModel : ITreeModel
    {
        #region Private/Global Variables

        private Dictionary<string, List<BaseItem>> _cache = new Dictionary<string, List<BaseItem>>();

        #endregion

        #region Public Methods

        public void AddDatabase(DataAccessBase database)
        {
            DatabaseItem item = new DatabaseItem(database.Name, null);
            item.Database = database;
            if(_cache.ContainsKey("ROOT"))
            {
                _cache["ROOT"].Add(item);
            }
            else
            {
                List<BaseItem> items = new List<BaseItem>();
                items.Add(item);
                _cache.Add("ROOT", items);
            }
            OnNodesInserted(TreePath.Empty, _cache["ROOT"].ToArray());
        }

        public System.Collections.IEnumerable GetChildren(TreePath treePath)
        {
            List<BaseItem> items = null;
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
                DatabaseItem database = treePath.FirstNode as DatabaseItem;
                BaseItem parent = treePath.LastNode as BaseItem;
                if (parent != null && database != null)
                {
                    //Populate the items based on the parent
                    switch (parent.Type)
                    {
                        case ItemType.Database:
                            items = GetDatabaseChildren(parent);
                            break;
                        case ItemType.Folder:
                            items = GetFolderChildren(parent, database);
                            break;
                        case ItemType.Table:
                        case ItemType.View:
                            items = GetColumns(parent, database);
                            break;
                        default:
                            items = new List<BaseItem>();
                            break;
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
            TreePath path = new TreePath(SelectedNode.Parent.Tag);
            List<BaseItem> children = (List<BaseItem>)GetChildren(path);
            OnStructureChanged(path, children.ToArray());
        }

        #endregion

        #region Public Events

        public event EventHandler<TreeModelEventArgs> NodesChanged;

        public event EventHandler<TreeModelEventArgs> NodesInserted;

        public event EventHandler<TreeModelEventArgs> NodesRemoved;

        public event EventHandler<TreePathEventArgs> StructureChanged;

        #endregion

        #region Private Methods

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
