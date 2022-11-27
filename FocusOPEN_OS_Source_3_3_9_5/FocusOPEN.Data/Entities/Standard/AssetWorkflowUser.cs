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
	/// This object represents the properties and methods of a AssetWorkflowUser.
	/// </summary>
	public partial class AssetWorkflowUser
	{
		#region Constructor
		
		protected AssetWorkflowUser()
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
		
		public static AssetWorkflowUser New ()
		{
			return new AssetWorkflowUser() ;
		}

		public static AssetWorkflowUser Empty
		{
			get { return NullAssetWorkflowUser.Instance; }
		}

		public static AssetWorkflowUser Get (Nullable <Int32> AssetWorkflowUserId)
		{
			AssetWorkflowUser AssetWorkflowUser = AssetWorkflowUserMapper.Instance.Get (AssetWorkflowUserId);
			return AssetWorkflowUser ?? Empty;
		}

		public static AssetWorkflowUser Update (AssetWorkflowUser assetWorkflowUser)
		{
			return AssetWorkflowUserMapper.Instance.Update(assetWorkflowUser) ;
		}

		public static void Delete (Nullable <Int32> AssetWorkflowUserId)
		{
			AssetWorkflowUserMapper.Instance.Delete (AssetWorkflowUserId);
		}

		public static EntityList <AssetWorkflowUser> FindMany (AssetWorkflowUserFinder finder, int Page, int PageSize)
		{
			return AssetWorkflowUserMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <AssetWorkflowUser> FindMany (AssetWorkflowUserFinder finder)
		{
			return AssetWorkflowUserMapper.Instance.FindMany (finder);
		}

		public static AssetWorkflowUser FindOne (AssetWorkflowUserFinder finder)
		{
			AssetWorkflowUser AssetWorkflowUser = AssetWorkflowUserMapper.Instance.FindOne(finder);
			return AssetWorkflowUser ?? Empty;
		}

		public static int GetCount (AssetWorkflowUserFinder finder)
		{
			return AssetWorkflowUserMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
