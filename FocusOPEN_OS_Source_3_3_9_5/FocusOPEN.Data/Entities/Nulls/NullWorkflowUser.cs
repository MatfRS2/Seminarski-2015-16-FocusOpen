/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2010, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System;

namespace FocusOPEN.Data
{
	[Serializable]
	public class NullWorkflowUser : WorkflowUser
	{
		#region Singleton implementation

		private NullWorkflowUser()
		{
		}

		private static readonly NullWorkflowUser m_instance = new NullWorkflowUser();

		public static NullWorkflowUser Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the WorkflowId of the WorkflowUser object.
		/// </summary>
		public override int WorkflowId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the UserId of the WorkflowUser object.
		/// </summary>
		public override int UserId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the Position of the WorkflowUser object.
		/// </summary>
		public override int Position
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the DateAdded of the WorkflowUser object.
		/// </summary>
		public override DateTime DateAdded
		{
			get { return DateTime.MinValue; }
			set { ; }
		}
		#endregion
	
		public override bool IsNull
		{
			get { return true; }
		}
	}
}

