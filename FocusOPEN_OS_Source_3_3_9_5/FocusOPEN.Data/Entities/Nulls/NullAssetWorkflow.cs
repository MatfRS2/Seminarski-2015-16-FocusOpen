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
	/// This object represents a null AssetWorkflow.
	/// </summary>
	[Serializable]
	public class NullAssetWorkflow : AssetWorkflow
	{
		#region Singleton implementation

		private NullAssetWorkflow()
		{
		}

		private static readonly NullAssetWorkflow m_instance = new NullAssetWorkflow();

		public static NullAssetWorkflow Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the AssetId of the AssetWorkflow object.
		/// </summary>
		public override int AssetId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the SubmittedByUserId of the AssetWorkflow object.
		/// </summary>
		public override int SubmittedByUserId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the CreateDate of the AssetWorkflow object.
		/// </summary>
		public override DateTime CreateDate
		{
			get { return DateTime.MinValue; }
			set { ; }
		}

		/// <summary>
		/// Returns the IsComplete of the AssetWorkflow object.
		/// </summary>
		public override bool IsComplete
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

