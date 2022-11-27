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
	/// This object represents the properties and methods of a AssetWorkflow.
	/// </summary>
	public partial class AssetWorkflow
	{
		#region Constructor
		
		protected AssetWorkflow()
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
		
		public static AssetWorkflow New ()
		{
			return new AssetWorkflow() ;
		}

		public static AssetWorkflow Empty
		{
			get { return NullAssetWorkflow.Instance; }
		}

		public static AssetWorkflow Get (Nullable <Int32> AssetWorkflowId)
		{
			AssetWorkflow AssetWorkflow = AssetWorkflowMapper.Instance.Get (AssetWorkflowId);
			return AssetWorkflow ?? Empty;
		}

		public static AssetWorkflow Update (AssetWorkflow assetWorkflow)
		{
			return AssetWorkflowMapper.Instance.Update(assetWorkflow) ;
		}

		public static void Delete (Nullable <Int32> AssetWorkflowId)
		{
			AssetWorkflowMapper.Instance.Delete (AssetWorkflowId);
		}

		public static EntityList <AssetWorkflow> FindMany (AssetWorkflowFinder finder, int Page, int PageSize)
		{
			return AssetWorkflowMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <AssetWorkflow> FindMany (AssetWorkflowFinder finder)
		{
			return AssetWorkflowMapper.Instance.FindMany (finder);
		}

		public static AssetWorkflow FindOne (AssetWorkflowFinder finder)
		{
			AssetWorkflow AssetWorkflow = AssetWorkflowMapper.Instance.FindOne(finder);
			return AssetWorkflow ?? Empty;
		}

		public static int GetCount (AssetWorkflowFinder finder)
		{
			return AssetWorkflowMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
