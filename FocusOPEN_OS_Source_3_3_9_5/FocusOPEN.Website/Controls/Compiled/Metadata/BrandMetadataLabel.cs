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
using System.Web.UI.WebControls;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public class BrandMetadataLabel : Label
	{
		#region Public Accessors

		/// <summary>
		/// Gets or sets the field name
		/// </summary>
		public string FieldName { get; set; }

		/// <summary>
		/// Gets or sets the brand control ID
		/// If not set, the current brand is used
		/// (based on the logged in user or URL)
		/// </summary>
		public string BrandDropDownListId { get; set; }

		#endregion

		#region Private Accessors

		/// <summary>
		/// Gets the brand to be used for setting this label text
		/// </summary>
		private Brand Brand
		{
			get
			{
				// Get the current brand
				Brand brand = WebsiteBrandManager.GetBrand();

				// Check for a brand control ID
				// If specified, then get the brand based on that
				if (!string.IsNullOrEmpty(BrandDropDownListId))
				{
					ListControl dropdown = SiteUtils.FindControlRecursiveUp(this, BrandDropDownListId) as ListControl;

					if (dropdown != null)
					{
						int selectedId = NumericUtils.ParseInt32(dropdown.SelectedValue, Int32.MinValue);

						if (selectedId != Int32.MinValue)
						{
							Brand b = BrandCache.Instance.GetById(selectedId);

							if (!b.IsNull)
								brand = b;
						}
					}
				}

				return brand;
			}
		}

		#endregion

		#region Overrides

		protected override void OnPreRender(EventArgs e)
		{
			BrandMetadataSetting setting = Brand.GetMetadataSetting(FieldName);

			if (setting.IsNull)
				setting = WebsiteBrandManager.GetMasterBrand().GetMetadataSetting(FieldName);

			if (setting.IsNull || StringUtils.IsBlank(setting.FieldName))
			{
				Text = GeneralUtils.SplitIntoSentence(FieldName);
				return;
			}

			Text = setting.FieldName;
		}

		#endregion
	}
}