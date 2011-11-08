using System;
using System.Drawing;
using Edrych.DataAccess;
using Edrych.Properties;

namespace Edrych.Models
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

    class BaseItem : IDisposable
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

        public void Dispose()
        {
            if (_icon != null)
                _icon.Dispose();
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

        public void Dispose()
        {
            if (_dataAccess != null)
                _dataAccess.Dispose();
            base.Dispose();
        }
    }
}
