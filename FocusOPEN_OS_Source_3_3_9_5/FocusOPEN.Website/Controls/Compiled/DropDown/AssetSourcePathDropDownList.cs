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
using System.Collections.Generic;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public class AssetSourcePathDropDownList : AbstractDropDownList
	{

		#region Fields

		IList<AssetSourcePath> m_InternalData = null;

		#endregion

		#region AbstractDropDownList Implementation

		public override object GetDataSource()
		{
            if (m_InternalData == null)
            {
            	m_InternalData = AssetSourcePaths.Paths;
                // See if we need to add the default Browser upload option
                if (m_InternalData.Count == 0 || m_InternalData[0].Path != "#BROWSERUPLOAD#")
                {
                    AssetSourcePath path = new AssetSourcePath() { Name = "Browser upload", Path = "#BROWSERUPLOAD#" };
                    m_InternalData.Insert(0, path);
                }
            }

            return m_InternalData;
		}

		public override string GetDataTextField()
		{
			return "Name";
		}

		public override string GetDataValueField()
		{
			return "Path";
		}

		#endregion
	}
}