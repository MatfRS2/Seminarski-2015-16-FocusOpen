/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Daydream.Data;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public abstract class BaseTreeView<T> : TreeView where T : AbstractEntity
	{
		#region Protected Abstract Methods

		protected abstract IEnumerable<T> GetRootList();
		protected abstract IEnumerable<T> GetSubEntitities(T entity);
		protected abstract TreeNode GetNodeFromEntity(T entity);

		#endregion

		#region Properties

		/// <summary>
		/// Boolean value specifying whether the root node is checkable if it's the only one
		/// </summary>
		protected bool SingleRootNodeCheckable { get; set; }

		/// <summary>
		/// Gets or sets the binding mode which controls when the list is automatically bound
		/// </summary>
		public BindingModeType BindingMode { get; set; }

		/// <summary>
		/// Gets or sets the BrandId
		/// </summary>
		public int BrandId
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "BrandId", 0);
			}
			set
			{
				ViewState["BrandId"] = value;
			}
		}


		#endregion

		#region Constructor

		protected BaseTreeView()
		{
			BindingMode = BindingModeType.OnInit;
			SingleRootNodeCheckable = false;
		}

		#endregion

		/// <summary>
		/// Binding Mode Type
		/// </summary>
		public enum BindingModeType
		{
			/// <summary>
			/// Binds the list in the OnInit handler
			/// </summary>
			OnInit,

			/// <summary>
			/// Binds the list in the OnLoad handler
			/// </summary>
			OnLoad,
			
			/// <summary>
			/// Never binds the list. Must be done manually by calling the BindList() method
			/// </summary>
			Manual
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			if (BindingMode == BindingModeType.OnInit)
				BindList();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (BindingMode == BindingModeType.OnLoad && !Page.IsPostBack)
				BindList();
		}

		private void AddEntity(T entity, TreeNode node)
		{
			var newNode = GetNodeFromEntity(entity);
			
#if DEBUG
			newNode.Expanded = true;
#else
			newNode.Expanded = false;
#endif

			TreeNodeCollection tree = (node == null) ? Nodes : node.ChildNodes;
            
            //we need this so that control doesn't postback on click of a node
            //this is a temporary fix until microsoft fix their treeview in a 
            //next release of the .net framework or we find a better workaround
		    newNode.SelectAction = TreeNodeSelectAction.Expand;
			
            tree.Add(newNode);

			foreach (T subentity in GetSubEntitities(entity))
				AddEntity(subentity, newNode);
		}

		public void CheckNode(int id)
		{
			foreach (TreeNode node in Nodes)
				ToggleNode(id, node, true);
		}

		public void UncheckNode(int id)
		{
			foreach (TreeNode node in Nodes)
				ToggleNode(id, node, false);
		}

		public void ToggleNode(int id, bool check)
		{
			if (check)
			{
				CheckNode(id);
			}
			else
			{
				UncheckNode(id);
			}
		}

		private static void ToggleNode(int id, TreeNode node, bool check)
		{
			if (node == null)
				return;

			if (node.Value == id.ToString())
			{
				node.Checked = check;
				return;
			}

			foreach (TreeNode n in node.ChildNodes)
				ToggleNode(id, n, check);
		}

		public List<TreeNode> GetSelectedNodes()
		{
			var list = new List<TreeNode>();

			foreach (TreeNode node in Nodes)
				list.AddRange(GetSelectedNodes(node));

			return list;
		}

		private static IEnumerable<TreeNode> GetSelectedNodes(TreeNode node)
		{
			var list = new List<TreeNode>();

			if (node.Checked)
				list.Add(node);

			foreach (TreeNode n in node.ChildNodes)
				list.AddRange(GetSelectedNodes(n));

			return list;
		}

		public void BindList()
		{
			Nodes.Clear();

			IEnumerable<T> list = GetRootList();

			foreach (T entity in list)
				AddEntity(entity, null);

			// If there's any only root element, we don't want
			// this to be checkable, so don't show the checkbox
			if (Nodes.Count == 1 && !SingleRootNodeCheckable)
			{
			    Nodes[0].ShowCheckBox = false;
                Nodes[0].NavigateUrl = "javascript:return false;";
			}
		}

		public void RefreshFromBrandAndSelect(int brandId, IEnumerable<int> idList)
		{
			BrandId = brandId;
			BindList();

			foreach (int id in idList)
				CheckNode(id);
		}
	}
}