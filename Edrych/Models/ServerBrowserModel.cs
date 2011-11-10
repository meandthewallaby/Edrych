using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aga.Controls.Tree;
using Edrych.DataAccess;
using Edrych.ViewModels;

namespace Edrych.Models
{
    public class ServerBrowserModel : ITreeModel
    {
        #region Private/Global Variables

        private Dictionary<string, List<BaseItem>> _cache;
        private ServerBrowserViewModel _viewModel;

        #endregion

        #region Constructor(s)

        public ServerBrowserModel(ServerBrowserViewModel ViewModel)
        {
            _cache = new Dictionary<string, List<BaseItem>>();
            _viewModel = ViewModel;
        }

        #endregion

        #region Public Properties

        public Dictionary<string, List<BaseItem>> Cache
        {
            get { return _cache; }
            set { if (value != _cache) _cache = value; }
        }

        #endregion

        #region Public Methods - ITreeModel Interface

        public IEnumerable GetChildren(TreePath treePath)
        {
            return _viewModel.GetChildren(treePath);
        }

        public bool IsLeaf(TreePath treePath)
        {
            return _viewModel.IsLeaf(treePath);
        }

        #endregion
        
        #region Public Events

        public event EventHandler<TreeModelEventArgs> NodesChanged;

        public event EventHandler<TreeModelEventArgs> NodesInserted;

        public event EventHandler<TreeModelEventArgs> NodesRemoved;

        public event EventHandler<TreePathEventArgs> StructureChanged;

        #endregion

        #region Public Event Triggers

        public void OnNodesInserted(TreePath Tree, int[] Indices, object[] Children)
        {
            if (NodesInserted != null && Children != null)
            {
                if (Indices == null)
                {
                    Indices = BuildIndices(Children.Length);
                }
                NodesInserted(this, new TreeModelEventArgs(Tree, Indices, Children));
            }
        }

        public void OnNodesChanged(TreePath Tree, int[] Indices, object[] Children)
        {
            if (NodesChanged != null && Children != null)
            {
                if (Indices == null)
                {
                    Indices = BuildIndices(Children.Length);
                }
                NodesChanged(this, new TreeModelEventArgs(Tree, Indices, Children));
            }
        }

        public void OnNodesRemoved(TreePath Tree, int[] Indices, object[] Children)
        {
            if (NodesRemoved != null && Children != null)
            {
                if (Indices == null)
                {
                    Indices = BuildIndices(Children.Length);
                }
                NodesRemoved(this, new TreeModelEventArgs(Tree, Indices, Children));
            }
        }

        public void OnStructureChanged(TreePath Tree)
        {
            if (StructureChanged != null)
            {
                StructureChanged(this, new TreePathEventArgs(Tree));
            }
        }

        public int[] BuildIndices(int Length)
        {
            int[] indices = new int[Length];
            for (int i = 0; i < Length; i++)
            {
                indices[i] = i;
            }
            return indices;
        }

        #endregion
    }
}
