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
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Components
{
	public abstract class BaseAdminWorkflowPage : BaseAdminPage
	{
		#region Accessors

		private AssetWorkflow m_AssetWorkflow = null;
		private int m_AssetWorkflowId = 0;

		#endregion

		#region Accessors

		public int AssetWorkflowId
		{
			get
			{
				return m_AssetWorkflowId;
			}
		}

		public AssetWorkflow AssetWorkflow
		{
			get
			{
				if (m_AssetWorkflow == null || m_AssetWorkflow.AssetWorkflowId.GetValueOrDefault() != AssetWorkflowId)
				{
					if (Context.Items["AssetWorkflow"] is AssetWorkflow)
					{
						m_AssetWorkflow = (AssetWorkflow) Context.Items["AssetWorkflow"];
					}
					else
					{
						m_AssetWorkflow = AssetWorkflow.Get(AssetWorkflowId);
					}
				}

				return m_AssetWorkflow;
			}
		}

		#endregion

		protected override void OnLoad(EventArgs e)
		{
			m_AssetWorkflowId = WebUtils.GetIntRequestParam("AssetWorkflowId", 0);

			if (!Page.IsPostBack)
			{
				if (AssetWorkflowId == 0)
				{
					Response.Redirect("~/Admin/Assets/AssetList.aspx");
					return;
				}

				AssetWorkflow assetWorkflow = AssetWorkflow.Get(AssetWorkflowId);

				if (assetWorkflow.IsNull)
				{
					Response.Redirect("~/Admin/Assets/AssetList.aspx");
					return;
				}
			}

			base.OnLoad(e);
		}
	}
}