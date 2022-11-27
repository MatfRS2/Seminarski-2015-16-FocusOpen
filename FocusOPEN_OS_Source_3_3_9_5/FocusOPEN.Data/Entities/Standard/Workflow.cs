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
using Daydream.Data;

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object represents the properties and methods of a Workflow.
	/// </summary>
	public partial class Workflow
	{
		#region Constructor
		
		protected Workflow()
		{
		}
		
		#endregion
		
		#region INullable Implementation
		
		public override bool IsNull
		{
			get
			{
				return false;
			}
		}
		
		#endregion
		
		#region ICloneable Implementation
	
		public object Clone()
		{
			return MemberwiseClone();
		}
		
		#endregion

		#region Static Members
		
		public static Workflow New ()
		{
			return new Workflow() ;
		}

		public static Workflow Empty
		{
			get { return NullWorkflow.Instance; }
		}

		public static Workflow Get (Nullable <Int32> WorkflowId)
		{
			Workflow Workflow = WorkflowMapper.Instance.Get (WorkflowId);
			return Workflow ?? Empty;
		}

		public static Workflow Update (Workflow workflow)
		{
			Workflow wf = WorkflowMapper.Instance.Update(workflow);

			if (wf.IsWorkflowUserListLoaded)
			{
				WorkflowUserMapper.Instance.DeleteWorkflowUsers(wf.WorkflowId.GetValueOrDefault());

				foreach (WorkflowUser wfu in wf.WorkflowUserList)
				{
					wfu.WorkflowId = wf.WorkflowId.GetValueOrDefault();
					WorkflowUser.Update(wfu);
				}
			}

			return wf;
		}

		public static void Delete (Nullable <Int32> WorkflowId)
		{
			WorkflowMapper.Instance.Delete (WorkflowId);
		}

		public static EntityList <Workflow> FindMany (WorkflowFinder finder, int Page, int PageSize)
		{
			return WorkflowMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <Workflow> FindMany (WorkflowFinder finder)
		{
			return WorkflowMapper.Instance.FindMany (finder);
		}

		public static Workflow FindOne (WorkflowFinder finder)
		{
			Workflow Workflow = WorkflowMapper.Instance.FindOne(finder);
			return Workflow ?? Empty;
		}

		public static int GetCount (WorkflowFinder finder)
		{
			return WorkflowMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
