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
using System.Collections.Generic;
using System.Linq;

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object represents the properties and methods of a AssetType.
	/// </summary>
	public partial class AssetType
	{
		#region Lazy Loads

		private List<AssetTypeFileExtension> m_AssetTypeFileExtensionList;

		public List<AssetTypeFileExtension> AssetTypeFileExtensionList
		{
			get
			{
				if (m_AssetTypeFileExtensionList == null)
					m_AssetTypeFileExtensionList = AssetTypeFileExtensionCache.Instance.GetByAssetType(AssetTypeId.GetValueOrDefault());

				return m_AssetTypeFileExtensionList;
			}
		}

		#endregion


		public List<string> FileExtensionList
		{
			get
			{
				return (from atfe in AssetTypeFileExtensionList
				        select atfe.Extension).ToList();
			}
		}
	}
}