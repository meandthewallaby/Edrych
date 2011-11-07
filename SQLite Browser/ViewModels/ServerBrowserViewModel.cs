using System;
using System.Collections.Generic;
using Aga.Controls.Tree;
using SQLiteBrowser.DataAccess;
using SQLiteBrowser.Dialogs;
using SQLiteBrowser.Models;

namespace SQLiteBrowser.ViewModels
{
    public class ServerBrowserViewModel : IDisposable
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

        public void Dispose()
        {
            if (_activeConnection != null)
                _activeConnection.Dispose();
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
