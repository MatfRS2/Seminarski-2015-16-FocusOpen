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
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Admin.Utils
{
	public partial class SkinEditor : BaseAdminPage
	{
		#region Accessors

		private static int BrandId
		{
			get
			{
				return WebUtils.GetIntRequestParam("BrandId", 0);
			}
		}

		private string AbsoluteSkinFolder
		{
			get
			{
				return Server.MapPath(string.Format("~/Brands/Brand_{0}/UI/", BrandId));
			}
		}

		private string EditingFilename
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "EditingFilename", string.Empty);
			}
			set
			{
				ViewState["EditingFilename"] = value;
			}
		}

		#endregion

		#region Page Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (BrandId == 0 || !Directory.Exists(AbsoluteSkinFolder))
				Response.Redirect("~/Content/ManageBrands.aspx");

			PageHeaderDiv.InnerText = "Edit skin for: " + BrandCache.Instance.GetById(BrandId).Name;

			if (!Page.IsPostBack)
			{
				BindFileList();
				SelectFile("Admin.css");
			}
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			FileEditorPanel.Visible = (!StringUtils.IsBlank(EditingFilename));
			NoFileSelectedPanel.Visible = !FileEditorPanel.Visible;

			foreach (RepeaterItem ri in FileRepeater.Items)
			{
				LinkButton lb = (LinkButton)ri.FindControl("FileLinkButton");
				lb.Font.Bold = StringUtils.IgnoreCaseCompare(lb.CommandArgument, EditingFilename);
			}
		}

		protected void FileRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			LinkButton lb = (LinkButton)e.Item.FindControl("FileLinkButton");
			lb.Text = lb.CommandArgument = e.Item.DataItem.ToString();
		}

		protected void FileLinkButton_Command(object sender, CommandEventArgs e)
		{
			string filename = e.CommandArgument.ToString();
			SelectFile(filename);
		}

		protected void SaveButton_Click(object sender, EventArgs e)
		{
			string path = Path.Combine(AbsoluteSkinFolder, EditingFilename);

			if (!File.Exists(path))
			{
				FeedbackLabel.SetErrorMessage("File not found");
				return;
			}

			try
			{
				File.WriteAllText(path, FileContentTextBox.Text);
				FeedbackLabel.SetSuccessMessage(string.Format("File '{0}' saved successfully", EditingFilename));
				ResetUI();
			}
			catch (Exception ex)
			{
				FeedbackLabel.SetErrorMessage("Unable to save: " + ex.Message);
			}
		}

		protected void CancelButton_Click(object sender, EventArgs e)
		{
			ResetUI();
		}

		#endregion

		#region Private Helper Methods

		private void BindFileList()
		{
			var fileList = from f in Directory.GetFiles(AbsoluteSkinFolder, "*.css")
			               orderby f
			               select Path.GetFileName(f);

			FileRepeater.DataSource = fileList;
			FileRepeater.DataBind();
		}

		private void SelectFile(string filename)
		{
			string path = Path.Combine(AbsoluteSkinFolder, filename);

			if (!File.Exists(path))
			{
				FeedbackLabel.SetErrorMessage("File not found");
				return;
			}

			EditingFilename = filename;
			FileContentTextBox.Text = File.ReadAllText(path);
		}

		private void ResetUI()
		{
			EditingFilename = string.Empty;
			FileContentTextBox.Text = string.Empty;
		}

		#endregion
	}
}