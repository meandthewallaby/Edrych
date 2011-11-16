using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aga.Controls.Tree;
using Edrych.DataAccess;
using Edrych.ViewModels;

namespace Edrych.Models
{
    /// <summary>Model to house the server browser structure. Inherits the ITreeModel interace of TreeViewAdv.</summary>
    public class ServerBrowserModel : ITreeModel
    {
        #region Private/Global Variables

        private Dictionary<string, List<BaseItem>> _cache;
        private ServerBrowserViewModel _viewModel;

        #endregion

        #region Constructor(s)

        /// <summary>Creates the instance of the model</summary>
        /// <param name="ViewModel">The ViewModel that initializes it</param>
        public ServerBrowserModel(ServerBrowserViewModel ViewModel)
        {
            _cache = new Dictionary<string, List<BaseItem>>();
            _viewModel = ViewModel;
        }

        #endregion

        #region Internal Properties

        /// <summary>List of items in the tree</summary>
        internal Dictionary<string, List<BaseItem>> Cache
        {
            get { return _cache; }
            set { if (value != _cache) _cache = value; }
        }

        #endregion

        #region Public Methods - ITreeModel Interface

        /// <summary>Handler for the TreeViewAdv object to get the children</summary>
        /// <param name="treePath">Path of the node to get the children of</param>
        /// <returns>IEnumerable object representing the children of the given path</returns>
        public IEnumerable GetChildren(TreePath treePath)
        {
            return _viewModel.GetChildren(treePath);
        }

        /// <summary>Handler for the TreeViewAdv to test if an object is a leaf</summary>
        /// <param name="treePath">Path of the node to get the children of</param>
        /// <returns>Boolean whether the item is a leaf item or not</returns>
        public bool IsLeaf(TreePath treePath)
        {
            return _viewModel.IsLeaf(treePath);
        }

        #endregion
        
        #region Public Events

        /// <summary>Event that's fired when the existing nodes are updated</summary>
        public event EventHandler<TreeModelEventArgs> NodesChanged;

        /// <summary>Event that's fired when nodes are inserted into the tree</summary>
        public event EventHandler<TreeModelEventArgs> NodesInserted;

        /// <summary>Event that's fired when nodes are removed from the tree</summary>
        public event EventHandler<TreeModelEventArgs> NodesRemoved;

        /// <summary>Event that's fired when the structure of the tree is changed</summary>
        public event EventHandler<TreePathEventArgs> StructureChanged;

        #endregion

        #region Public Event Triggers

        /// <summary>Trigger for the NodesChanged event</summary>
        /// <param name="Tree">Path of the parent of the changed items</param>
        /// <param name="Indices">Indices (0-based) where to add the children changed</param>
        /// <param name="Children">Children objects to be changed under the Tree</param>
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

        /// <summary>Trigger for the NodesInserted event</summary>
        /// <param name="Tree">Path of the parent of the inserted items</param>
        /// <param name="Indices">Indices (0-based) where to add the children inserted</param>
        /// <param name="Children">Children objects to be inserted under the Tree</param>
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

        /// <summary>Trigger for the NodesRemoved event</summary>
        /// <param name="Tree">Path of the parent of the removed items</param>
        /// <param name="Indices">Indices (0-based) where to add the children removed</param>
        /// <param name="Children">Children objects to be removed under the Tree</param>
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

        /// <summary>Trigger for the StructureChanged event</summary>
        /// <param name="Tree">Path of the parent of the changed items</param>
        public void OnStructureChanged(TreePath Tree)
        {
            if (StructureChanged != null)
            {
                StructureChanged(this, new TreePathEventArgs(Tree));
            }
        }

        #endregion

        #region Private Methods

        /// <summary>Builds the array of indices for a given length</summary>
        /// <param name="Length">Size of the array to build</param>
        /// <returns>Array of integer indices</returns>
        private int[] BuildIndices(int Length)
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
