using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SQLiteBrowser.DataAccess;
using SQLiteBrowser.Dialogs;
using SQLiteBrowser.Helpers;
using SQLiteBrowser.Models;

namespace SQLiteBrowser.ViewModels
{
    public class TreeViewModel
    {
        #region Private/Global Variables

        private DataAccessBase _dataAccess;
        private TreeModel _tree;

        #endregion

        #region Constructor(s)

        public TreeViewModel()
        {
            InitializeData();
            _tree = new TreeModel();
            if(this._dataAccess != null)
                _tree.AddServer(this._dataAccess);
        }

        #endregion

        #region Public Properties

        public DataAccessBase DataAccess
        {
            get { return _dataAccess; }
        }

        public TreeModel Tree
        {
            get { return _tree; }
        }

        #endregion

        #region Public Methods

        public void RefreshNode(Aga.Controls.Tree.TreeNodeAdv Node)
        {
            _tree.RefreshNode(Node);
        }

        #endregion

        #region Private Methods

        private void InitializeData()
        {
            ConnectDialog cd = new ConnectDialog();
            cd.ShowDialog();
            this._dataAccess = cd.DataAccess;
        }

        #endregion
    }
}
