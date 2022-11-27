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
using FocusOPEN.Data;

namespace FocusOPEN.Website.Controls
{
	public class AssetImageSizeDropDownList : AbstractDropDownList
	{
		#region AbstractDropDownList Implementation

		public override object GetDataSource()
		{
			return AssetImageSizeCache.Instance.GetList();
		}

		public override string GetDataTextField()
		{
			return AssetImageSize.Columns.Description.ToString();
		}

		public override string GetDataValueField()
		{
			return AssetImageSize.Columns.AssetImageSizeId.ToString();
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			SafeSelectValue(0);
		}

		#endregion
	}
}