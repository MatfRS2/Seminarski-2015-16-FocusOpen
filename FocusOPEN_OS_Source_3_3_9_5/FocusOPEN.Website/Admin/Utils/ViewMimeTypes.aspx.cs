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
using System.IO;
using System.Web.UI.WebControls;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Admin
{
	public partial class ViewMimeTypes : BaseAdminPage
	{
		protected ViewMimeTypes()
		{
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				List<MimeTypeManager.MimeType> mimeTypeList = MimeTypeManager.MimeTypeList;
				mimeTypeList.Sort((m1, m2) => Comparer<String>.Default.Compare(m1.ContentType, m2.ContentType));

				MimeTypesDataGrid.DataSource = mimeTypeList;
				MimeTypesDataGrid.DataBind();
			}
		}

		protected void MimeTypesDataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case (ListItemType.Item):
				case (ListItemType.AlternatingItem):

					MimeTypeManager.MimeType mimeType = (MimeTypeManager.MimeType) e.Item.DataItem;

					Label MimeTypeNameLabel = (Label) e.Item.FindControl("MimeTypeNameLabel");
					Label FileExtensionsLabel = (Label) e.Item.FindControl("FileExtensionsLabel");

					MimeTypeNameLabel.Text = mimeType.ContentType;

					JoinableList jList = new JoinableList(", ");

					foreach (string fileExt in mimeType.FileExtensions)
						jList.Add(fileExt);

					FileExtensionsLabel.Text = jList.ToString();

					break;
			}
		}

		protected void UploadButton_Click(object sender, EventArgs e)
		{
			if (FileUpload1.HasFile)
				MimeTypeManager.AddMimeType(Path.GetExtension(FileUpload1.PostedFile.FileName), FileUpload1.PostedFile.ContentType);

			Response.Redirect("ViewMimeTypes.aspx");
		}
	}
}