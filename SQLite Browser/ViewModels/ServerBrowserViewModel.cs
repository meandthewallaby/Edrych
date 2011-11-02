using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aga.Controls.Tree;
using SQLiteBrowser.DataAccess;
using SQLiteBrowser.Dialogs;
using SQLiteBrowser.Helpers;
using SQLiteBrowser.Models;

namespace SQLiteBrowser.ViewModels
{
    public class ServerBrowserViewModel
    {
        #region Private/Global Variables

        private DataAccessBase _activeConnection;
        private ServerBrowserModel _tree;

        #endregion

        #region Constructor(s)

        public ServerBrowserViewModel()
        {
            _tree = new ServerBrowserModel();
            CreateConnection();
        }

        #endregion

        #region Public Properties

        public DataAccessBase ActiveConnection
        {
            get { return _activeConnection; }
        }

        public ServerBrowserModel Tree
        {
            get { return _tree; }
        }

        #endregion

        #region Public Methods

        public void CreateConnection()
        {
            ConnectDialog cd = new ConnectDialog();
            cd.ShowDialog();
            if (cd.DataAccess != null)
            {
                _tree.AddServer(cd.DataAccess);
                this._activeConnection = cd.DataAccess;
            }
        }

        public void RefreshNode(TreeNodeAdv Node)
        {
            _tree.RefreshNode(Node);
        }

        public void RemoveConnection(TreeNodeAdv Node)
        {
            _tree.RemoveServer(Node);
        }

        public void UpdateActiveConnection(TreeNodeAdv Node)
        {
            TreePath path = GetNodePath(Node);
            ServerItem server = path.FirstNode as ServerItem;
            if (server.DataAccess.DataSource != this._activeConnection.DataSource)
            {
                this._activeConnection = server.DataAccess;
            }
        }

        #endregion

        #region Private Methods

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
