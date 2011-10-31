using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using SQLiteBrowser.DataAccess;
using SQLiteBrowser.Resources;

namespace SQLiteBrowser.Models
{
    enum ItemType
    {
        Server,
        Database,
        Folder,
        Table,
        View,
        Column,
        None
    }

    class BaseItem
    {
        private Image _icon;
        private String _name;
        private BaseItem _parent;
        private ItemType _type;
        private string _itemPath;

        public BaseItem(ItemType Type, string Name, BaseItem Parent)
        {
            _type = Type;
            _name = Name;
            _parent = Parent;
            if (_parent != null)
                _itemPath = _parent.ItemPath + "|" + _name;
            else
                _itemPath = "|" + _name;
            SetIcon();
        }

        public Image Icon
        {
            get { return _icon; }
        }

        public String Name
        {
            get { return _name; }
        }

        public BaseItem Parent
        {
            get { return _parent; }
        }

        public ItemType Type
        {
            get { return _type; }
        }

        public string ItemPath
        {
            get { return _itemPath; }
        }

        public bool IsLoaded { get; set; }

        private void SetIcon()
        {
            switch(this._type)
            {
                case ItemType.Server:
                    _icon = Icons.server;
                    break;
                case ItemType.Database:
                    _icon = Icons.database;
                    break;
                case ItemType.Folder:
                    _icon = Icons.folder;
                    break;
                case ItemType.Table:
                    _icon = Icons.table;
                    break;
                case ItemType.View:
                    _icon = Icons.view;
                    break;
                case ItemType.Column:
                    _icon = Icons.column;
                    break;
                default:
                    _icon = Icons.folder;
                    break;
            }
        }
    }

    class DatabaseItem : BaseItem
    {
        private DataAccessBase _database;

        public DatabaseItem (string Name, BaseItem Parent) 
            : base(ItemType.Database, Name, Parent)
        {
        }

        public DataAccessBase Database
        {
            get { return _database; }
            set { _database = value; }
        }
    }

    class ServerItem : BaseItem
    {
        private DataAccessBase _dataAccess;

        public ServerItem(string Name)
            : base(ItemType.Server, Name, null)
        {
        }

        public DataAccessBase DataAccess
        {
            get { return _dataAccess; }
            set { _dataAccess = value; }
        }
    }
}
