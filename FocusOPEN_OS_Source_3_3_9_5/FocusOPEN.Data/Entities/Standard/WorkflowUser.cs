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
	/// This object represents the properties and methods of a WorkflowUser.
	/// </summary>
	public partial class WorkflowUser
	{
		#region Constructor
		
		protected WorkflowUser()
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
		
		public static WorkflowUser New ()
		{
			return new WorkflowUser() ;
		}

		public static WorkflowUser Empty
		{
			get { return NullWorkflowUser.Instance; }
		}

		public static WorkflowUser Get (Nullable <Int32> WorkflowUserId)
		{
			WorkflowUser WorkflowUser = WorkflowUserMapper.Instance.Get (WorkflowUserId);
			return WorkflowUser ?? Empty;
		}

		public static WorkflowUser Update (WorkflowUser workflowUser)
		{
			return WorkflowUserMapper.Instance.Update(workflowUser) ;
		}

		public static void Delete (Nullable <Int32> WorkflowUserId)
		{
			WorkflowUserMapper.Instance.Delete (WorkflowUserId);
		}

		public static EntityList <WorkflowUser> FindMany (WorkflowUserFinder finder, int Page, int PageSize)
		{
			return WorkflowUserMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <WorkflowUser> FindMany (WorkflowUserFinder finder)
		{
			return WorkflowUserMapper.Instance.FindMany (finder);
		}

		public static WorkflowUser FindOne (WorkflowUserFinder finder)
		{
			WorkflowUser WorkflowUser = WorkflowUserMapper.Instance.FindOne(finder);
			return WorkflowUser ?? Empty;
		}

		public static int GetCount (WorkflowUserFinder finder)
		{
			return WorkflowUserMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
