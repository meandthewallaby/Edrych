using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLiteBrowser.Models
{
    public enum InfoType
    {
        Database,
        Folder,
        Table,
        View,
        Column
    }

    public class InfoBase
    {
        #region Private/Global Variables

        private string _name;
        private List<InfoBase> _children;

        #endregion

        #region Constructor(s)

        public InfoBase()
        {
        }

        #endregion

        #region Public Properties

        public string Name
        {
            get { return _name; }
            set { if(_name != value) _name = value; }
        }

        public List<InfoBase> Children
        {
            get { return _children; }
            set { if (_children != value) _children = value; }
        }

        #endregion

        #region Private Methods
        #endregion
    }
}
