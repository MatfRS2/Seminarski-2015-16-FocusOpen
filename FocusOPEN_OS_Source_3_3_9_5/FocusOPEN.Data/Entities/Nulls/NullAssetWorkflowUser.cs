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
	/// This object represents a null AssetWorkflowUser.
	/// </summary>
	[Serializable]
	public class NullAssetWorkflowUser : AssetWorkflowUser
	{
		#region Singleton implementation

		private NullAssetWorkflowUser()
		{
		}

		private static readonly NullAssetWorkflowUser m_instance = new NullAssetWorkflowUser();

		public static NullAssetWorkflowUser Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the AssetWorkflowId of the AssetWorkflowUser object.
		/// </summary>
		public override int AssetWorkflowId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the UserId of the AssetWorkflowUser object.
		/// </summary>
		public override int UserId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the Position of the AssetWorkflowUser object.
		/// </summary>
		public override int Position
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the Comments of the AssetWorkflowUser object.
		/// </summary>
		public override string Comments
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the AssetWorkflowUserStatusId of the AssetWorkflowUser object.
		/// </summary>
		public override int AssetWorkflowUserStatusId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the CreateDate of the AssetWorkflowUser object.
		/// </summary>
		public override DateTime CreateDate
		{
			get { return DateTime.MinValue; }
			set { ; }
		}

		/// <summary>
		/// Returns the LastUpdate of the AssetWorkflowUser object.
		/// </summary>
		public override DateTime LastUpdate
		{
			get { return DateTime.MinValue; }
			set { ; }
		}

		/// <summary>
		/// Returns the IsDeleted of the AssetWorkflowUser object.
		/// </summary>
		public override bool IsDeleted
		{
			get { return false; }
			set { ; }
		}
		#endregion
	
		public override bool IsNull
		{
			get { return true; }
		}
	}
}

