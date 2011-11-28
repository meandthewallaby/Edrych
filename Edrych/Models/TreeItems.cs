using System;
using System.Drawing;
using Edrych.DataAccess;
using Edrych.Properties;

namespace Edrych.Models
{
    /// <summary>Types of items that can go into a tree</summary>
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

    /// <summary>Base class for database items</summary>
    class BaseItem : IDisposable
    {
        protected Image _icon;
        private String _name;
        private BaseItem _parent;
        private ItemType _type;
        private string _itemPath;

        /// <summary>Constructor to build the base item</summary>
        /// <param name="Type">Type of item to build</param>
        /// <param name="Name">Name of the item</param>
        /// <param name="Parent">Parent of the item</param>
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

        /// <summary>Icon to represent the item in the tree</summary>
        public Image Icon
        {
            get { return _icon; }
        }

        /// <summary>Name of the item</summary>
        public String Name
        {
            get { return _name; }
        }

        /// <summary>Parent item of the item</summary>
        public BaseItem Parent
        {
            get { return _parent; }
        }

        /// <summary>Type of the item</summary>
        public ItemType Type
        {
            get { return _type; }
        }

        /// <summary>Pipe-delimited path of the item</summary>
        public string ItemPath
        {
            get { return _itemPath; }
        }

        /// <summary>Whether or not the item has been loaded in the tree</summary>
        public bool IsLoaded { get; set; }

        /// <summary>Sets the icon based on item type</summary>
        protected virtual void SetIcon()
        {
            switch(this._type)
            {
                case ItemType.Server:
                    _icon = Resources.server;
                    break;
                case ItemType.Database:
                    _icon = Resources.database;
                    break;
                case ItemType.Folder:
                    _icon = Resources.folder;
                    break;
                case ItemType.Table:
                    _icon = Resources.table;
                    break;
                case ItemType.View:
                    _icon = Resources.view;
                    break;
                case ItemType.Column:
                    _icon = Resources.column;
                    break;
                default:
                    _icon = Resources.folder;
                    break;
            }
        }

        /// <summary>Disposes of the item</summary>
        public void Dispose()
        {
            if (_icon != null)
                _icon.Dispose();
        }
    }

    /// <summary>Item representing a server</summary>
    class ServerItem : BaseItem
    {
        private DataAccessBase _dataAccess;

        /// <summary>Constructor</summary>
        /// <param name="Name">Name of the server</param>
        public ServerItem(string Name)
            : base(ItemType.Server, Name, null)
        {
        }

        /// <summary>Data Access object associated with the server</summary>
        public DataAccessBase DataAccess
        {
            get { return _dataAccess; }
            set { _dataAccess = value; }
        }

        /// <summary>Dispose of the item</summary>
        public void Disposal()
        {
            if (_dataAccess != null)
                _dataAccess.Dispose();
            base.Dispose();
        }
    }

    /// <summary>Item representing a column</summary>
    class ColumnKeyItem : BaseItem
    {
        KeyType _key = KeyType.None;

        /// <summary>Constructor</summary>
        /// <param name="Name">Name of the column</param>
        /// <param name="Parent">Parent item of the column</param>
        public ColumnKeyItem(string Name, BaseItem Parent, KeyType Key)
            : base(ItemType.Column, Name, Parent)
        {
            _key = Key;
            SetIcon();
        }

        protected override void SetIcon()
        {
            switch (_key)
            {
                case KeyType.Primary:
                    _icon = Resources.primary_key;
                    break;
                case KeyType.Foreign:
                    _icon = Resources.foreign_key;
                    break;
                default:
                    _icon = Resources.column;
                    break;
            }
        }
    }
}
