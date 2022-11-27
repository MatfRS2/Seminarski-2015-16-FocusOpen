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
	public class MetadataVisibilityWrapper : PlaceHolder
	{
		public enum Sections
		{
			AssetForm = 1,
			AssetDetail = 2,
		}

		#region Private Variables

		private Sections m_Section = Sections.AssetDetail;

		#endregion

		#region Public Accessors

	    private string fieldName;
		/// <summary>
		/// Gets or sets the field name
		/// </summary>
		public string FieldName 
        { 
            get { return string.IsNullOrEmpty(fieldName) ? (ViewState["FieldName"] ?? "").ToString() : fieldName; } 
            set { ViewState["FieldName"] = fieldName = value; } 
        }

		/// <summary>
		/// Gets or sets the brand control ID
		/// If not set, the current brand is used
		/// (based on the logged in user or URL)
		/// </summary>
		public string BrandDropDownListId { get; set; }

		/// <summary>
		/// Gets or sets the section
		/// </summary>
		public Sections Section
		{
			get
			{
				return m_Section;
			}
			set
			{
				m_Section = value;
			}
		}

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

		protected override void OnLoad(EventArgs e)
		{
			// Make this visible on load. We'll update the visibility on PreRender
			// to figure out whether this should actually be rendered
			Visible = true;

			base.OnLoad(e);
		}

		protected override void OnPreRender(EventArgs e)
		{
			// Assume not visible
			bool visible = false;

			// Get the brand setting
			BrandMetadataSetting setting = Brand.GetMetadataSetting(FieldName);

			// Display if we need to use the Asset Form setting and it's enabled
			if (Section == Sections.AssetForm && setting.OnAssetForm)
				visible = true;

			// Display if we need to use the Asset Detail setting and it's enabled
			if (Section == Sections.AssetDetail && setting.OnAssetDetail)
				visible = true;
			
			// Update visibility
			Visible = visible;
		}

		#endregion
	}
}