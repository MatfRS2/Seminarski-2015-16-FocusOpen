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
	/// This object represents the properties and methods of a AssetWorkflowCommenter.
	/// </summary>
	public partial class AssetWorkflowCommenter
	{
		#region Constructor
		
		protected AssetWorkflowCommenter()
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
		
		public static AssetWorkflowCommenter New ()
		{
			return new AssetWorkflowCommenter() ;
		}

		public static AssetWorkflowCommenter Empty
		{
			get { return NullAssetWorkflowCommenter.Instance; }
		}

		public static AssetWorkflowCommenter Get (Nullable <Int32> AssetWorkflowCommenterId)
		{
			AssetWorkflowCommenter AssetWorkflowCommenter = AssetWorkflowCommenterMapper.Instance.Get (AssetWorkflowCommenterId);
			return AssetWorkflowCommenter ?? Empty;
		}

		public static AssetWorkflowCommenter Update (AssetWorkflowCommenter assetWorkflowCommenter)
		{
			return AssetWorkflowCommenterMapper.Instance.Update(assetWorkflowCommenter) ;
		}

		public static void Delete (Nullable <Int32> AssetWorkflowCommenterId)
		{
			AssetWorkflowCommenterMapper.Instance.Delete (AssetWorkflowCommenterId);
		}

		public static EntityList <AssetWorkflowCommenter> FindMany (AssetWorkflowCommenterFinder finder, int Page, int PageSize)
		{
			return AssetWorkflowCommenterMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <AssetWorkflowCommenter> FindMany (AssetWorkflowCommenterFinder finder)
		{
			return AssetWorkflowCommenterMapper.Instance.FindMany (finder);
		}

		public static AssetWorkflowCommenter FindOne (AssetWorkflowCommenterFinder finder)
		{
			AssetWorkflowCommenter AssetWorkflowCommenter = AssetWorkflowCommenterMapper.Instance.FindOne(finder);
			return AssetWorkflowCommenter ?? Empty;
		}

		public static int GetCount (AssetWorkflowCommenterFinder finder)
		{
			return AssetWorkflowCommenterMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
