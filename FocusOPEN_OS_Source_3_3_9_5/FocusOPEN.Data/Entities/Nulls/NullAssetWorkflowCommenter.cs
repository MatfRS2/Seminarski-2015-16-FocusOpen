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

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object represents a null AssetWorkflowCommenter.
	/// </summary>
	[Serializable]
	public class NullAssetWorkflowCommenter : AssetWorkflowCommenter
	{
		#region Singleton implementation

		private NullAssetWorkflowCommenter()
		{
		}

		private static readonly NullAssetWorkflowCommenter m_instance = new NullAssetWorkflowCommenter();

		public static NullAssetWorkflowCommenter Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the UserId of the AssetWorkflowCommenter object.
		/// </summary>
		public override int UserId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the InvitingAssetWorkflowUserId of the AssetWorkflowCommenter object.
		/// </summary>
		public override int InvitingAssetWorkflowUserId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the InvitingUserMessage of the AssetWorkflowCommenter object.
		/// </summary>
		public override string InvitingUserMessage
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Comments of the AssetWorkflowCommenter object.
		/// </summary>
		public override string Comments
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the CreateDate of the AssetWorkflowCommenter object.
		/// </summary>
		public override DateTime CreateDate
		{
			get { return DateTime.MinValue; }
			set { ; }
		}

		/// <summary>
		/// Returns the LastUpdate of the AssetWorkflowCommenter object.
		/// </summary>
		public override DateTime LastUpdate
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

