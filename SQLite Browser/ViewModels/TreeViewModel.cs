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
        private List<InfoBase> _treeItems = new List<InfoBase>();

        #endregion

        #region Constructor(s)

        public TreeViewModel()
        {
            InitializeData();
            AddDatabase();
        }

        #endregion

        #region Public Properties

        public DataAccessBase DataAccess
        {
            get { return _dataAccess; }
        }

        #endregion

        #region Private Methods

        private void InitializeData()
        {
            ConnectDialog cd = new ConnectDialog();
            cd.ShowDialog();
            this._dataAccess = cd.DataAccess;
        }

        private void AddDatabase()
        {
            InfoBase database = new InfoBase();
            database.Name = this._dataAccess.DataSource;
            List<InfoBase> folders = new List<InfoBase>();
            folders.Add(new InfoBase() { Name = "Tables" });
            folders.Add(new InfoBase() { Name = "Views" });
            database.Children = folders;
            _treeItems.Add(database);
        }

        #endregion
    }
}
