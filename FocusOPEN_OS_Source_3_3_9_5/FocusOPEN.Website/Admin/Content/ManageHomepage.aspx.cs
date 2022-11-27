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
using System.Web.UI.WebControls;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Admin
{
	public partial class ManageHomepage : BaseAdminPage
	{
		#region Constructor

		protected ManageHomepage()
		{
		}

		#endregion

		#region Accessors

		private int HomepageId
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "HomepageId", 0);
			}
			set
			{
				ViewState["HomepageId"] = value;
			}
		}

		private int EditingBrandId
		{
			get
			{
				if (CurrentUser.UserRole == UserRole.SuperAdministrator)
					return BrandDropDownList1.SelectedId;

				return CurrentUser.PrimaryBrandId;
			}
		}

		#endregion

		#region Page Methods

		protected void Page_Init(object sender, EventArgs e)
		{
			// Only super-admins can edit the homepages for multiple brands
			// so hide the brand selector all non super-admin users
			BrandSelectorPanel.Visible = (CurrentUser.UserRole == UserRole.SuperAdministrator && BrandCache.Instance.GetList().Count > 1);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			MessageLabel1.Visible = false;

			if (!Page.IsPostBack)
			{
				DisplayHomepage();
			}
		}

		protected void BrandDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
		{
			DisplayHomepage();
		}

		protected void HomepageTemplateTypeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
		{
			HomepageType homepageType = HomepageTypeCache.Instance.GetById(HomepageTemplateTypeDropDownList.SelectedId);
			StandardTemplateEditorPanel.Visible = (homepageType.ShortName.ToLower() == "standard");
			CustomHtmlTemplateEditorPanel.Visible = (!StandardTemplateEditorPanel.Visible);
		}

		protected void RemoveCategoryButton_Click(object sender, EventArgs e)
		{
			if (SelectedCategoriesListBox.SelectedIndex < 0)
				return;

			string selectedValue = SelectedCategoriesListBox.SelectedValue;

			foreach (ListItem li in SelectedCategoriesListBox.Items)
			{
				if (li.Value == selectedValue)
				{
					SelectedCategoriesListBox.Items.Remove(li);
					CategoriesTreeView.UncheckNode(NumericUtils.ParseInt32(selectedValue, 0));
					break;
				}
			}
		}

		protected void PreviewButton_Click(object sender, EventArgs e)
		{
			// Get the homepage being edited
			Homepage homepage = Homepage.Get(HomepageId);

			// Create a copy if the homepage is published
			if (homepage.IsPublished)
				homepage = HomepageManager.GetCopy(homepage);

			// List of images
			IList<BinaryFile> imageList = new List<BinaryFile>();

			// Get the homepage and images
			GetHomepageAndImages(ref homepage, ref imageList);

			try
			{
				HomepageManager.PreviewHomepage(CurrentUser, homepage, imageList);
				MessageLabel1.SetSuccessMessage("Homepage preview created successfully.");
				DisplayHomepage(homepage);

				string script = "window.open('" + ResolveUrl("~/Default.aspx?HideBumper=1&PreviewHomepageId=") + homepage.HomepageId + "', 'HomepagePreview');";
				Page.ClientScript.RegisterStartupScript(GetType(), "PreviewHomepageScript", script, true);
			}
			catch (InvalidHomepageException ihex)
			{
				MessageLabel1.SetErrorMessage("the following errors occured:", ihex.Errors);
			}
		}

		protected void PublishButton_Click(object sender, EventArgs e)
		{
			// Get the homepage being edited
			Homepage homepage = Homepage.Get(HomepageId);

			// If the homepage being edited is not for the brand being edited
			// then create a copy of it, which will become the brand homepage.
			if (homepage.BrandId != EditingBrandId)
				homepage = HomepageManager.GetCopy(homepage);

			// List of images
			IList<BinaryFile> imageList = new List<BinaryFile>();

			// Get the homepage and images
			GetHomepageAndImages(ref homepage, ref imageList);

			try
			{
				HomepageManager.PublishHomepage(CurrentUser, homepage, imageList);
				MessageLabel1.SetSuccessMessage("Homepage published successfully");
				DisplayHomepage(homepage);
			}
			catch (InvalidHomepageException ihex)
			{
				MessageLabel1.SetErrorMessage("the following errors occured:", ihex.Errors);
			}
		}

		protected void CategoriesTreeView_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
		{
			int categoryId = NumericUtils.ParseInt32(e.Node.Value, 0);

			if (e.Node.Checked)
			{
				Category category = CategoryCache.Instance.GetById(categoryId);
				ListItem li = new ListItem(SiteUtils.GetFullCategoryName(category), categoryId.ToString());
				SelectedCategoriesListBox.Items.Add(li);
			}
			else
			{
				foreach (ListItem li in SelectedCategoriesListBox.Items)
				{
					if (li.Value == categoryId.ToString())
					{
						SelectedCategoriesListBox.Items.Remove(li);
						break;
					}
				}
			}
		}

		protected void MoveUpCategoryButton_Click(object sender, EventArgs e)
		{
			if (SelectedCategoriesListBox.SelectedIndex <= 0)
				return;

			SelectedCategoriesListBox.Items.Insert(SelectedCategoriesListBox.SelectedIndex + 1, SelectedCategoriesListBox.Items[SelectedCategoriesListBox.SelectedIndex - 1]);
			SelectedCategoriesListBox.Items.RemoveAt(SelectedCategoriesListBox.SelectedIndex - 1);
		}

		protected void MoveDownCategoryButton_Click(object sender, EventArgs e)
		{
			if (SelectedCategoriesListBox.SelectedIndex < 0)
				return;

			if (SelectedCategoriesListBox.SelectedIndex == SelectedCategoriesListBox.Items.Count - 1)
				return;

			SelectedCategoriesListBox.Items.Insert(SelectedCategoriesListBox.SelectedIndex, SelectedCategoriesListBox.Items[SelectedCategoriesListBox.SelectedIndex + 1]);
			SelectedCategoriesListBox.Items.RemoveAt(SelectedCategoriesListBox.SelectedIndex + 1);
		}

		protected void IncludeBumperPage_CheckChanged(object sender, EventArgs e)
		{
			BumperPageHtmlWrapper.Visible = IncludeBumperPageCheckBox.Checked;
		}

		#endregion

		#region Private Helper Methods

		private void GetHomepageAndImages(ref Homepage homepage, ref IList<BinaryFile> imageList)
		{
			// Set the brand ID for th brand being edited
			homepage.BrandId = EditingBrandId;

			// Add the metadata
			homepage.IntroText = IntroTextBox.Text ?? string.Empty;
			homepage.Url1 = HomepageImageEditor1.Url;
			homepage.Url2 = HomepageImageEditor2.Url;
			homepage.Url3 = HomepageImageEditor3.Url;
			homepage.Url4 = HomepageImageEditor4.Url;

			// Bumper page settings
			homepage.BumperPageSkip = SkipBumperPageCheckBox.Checked;
			homepage.BumperPageHtml = (IncludeBumperPageCheckBox.Checked) ? BumperPageTextBox.Text : string.Empty;
			
			// Custom HTML
			homepage.CustomHtml = CustomHtmlTextBox.Text ?? string.Empty;

			// Other settings
			homepage.HomepageTypeId = HomepageTemplateTypeDropDownList.SelectedId;
			homepage.LastModifiedByUserId = CurrentUser.UserId.GetValueOrDefault();
			homepage.LastModifiedDate = DateTime.Now;

			// Add image information
			imageList = new List<BinaryFile>
			            	{
								new BinaryFile(HomepageImageEditor1.ImageUpload.PostedFile),
								new BinaryFile(HomepageImageEditor2.ImageUpload.PostedFile),
								new BinaryFile(HomepageImageEditor3.ImageUpload.PostedFile),
								new BinaryFile(HomepageImageEditor4.ImageUpload.PostedFile)
			            	};

			// Clear old categories
			homepage.HomepageCategoryList.Clear();

			// Add selected categories
			for (int i = 0; i < SelectedCategoriesListBox.Items.Count; i++)
			{
				HomepageCategory hc = HomepageCategory.New();
				hc.CategoryId = Convert.ToInt32(SelectedCategoriesListBox.Items[i].Value);
				hc.OrderBy = i;
				homepage.HomepageCategoryList.Add(hc);
			}
		}

		private void DisplayHomepage()
		{
			Homepage homepage = Homepage.Empty;

			if (WebUtils.GetIntRequestParam("HomepageId", 0) != 0)
				homepage = Homepage.Get(WebUtils.GetIntRequestParam("HomepageId", 0));

			if (homepage.IsNull)
				homepage = HomepageManager.GetCurrentHomepage(EditingBrandId);

			// Get the homepage ID
			DisplayHomepage(homepage);
		}

		private void DisplayHomepage(Homepage homepage)
		{
			if (homepage.BrandId != EditingBrandId)
			{
				MessageLabel1.Pinned = true;
				MessageLabel1.SetErrorMessage("This brand does not have its own homepage and is using the homepage from the master brand.");
			}

			HomepageId = homepage.HomepageId.GetValueOrDefault();

			// Rebind the categories list			
			CategoriesTreeView.BrandId = EditingBrandId;
			CategoriesTreeView.BindList();
			CategoriesTreeView.ExpandAll();

			// Clear the selected categories
			SelectedCategoriesListBox.Items.Clear();

			// Update bumper page settings
			IncludeBumperPageCheckBox.Checked = homepage.HasBumperPage;
			SkipBumperPageCheckBox.Checked = homepage.BumperPageSkip;
			BumperPageTextBox.Text = homepage.BumperPageHtml;
			BumperPageHtmlWrapper.Visible = IncludeBumperPageCheckBox.Checked;

			// Update the homepage type selector
			HomepageTemplateTypeDropDownList.SafeSelectValue(homepage.HomepageTypeId);
			StandardTemplateEditorPanel.Visible = (homepage.HomepageType.ShortName.ToLower() == "standard");
			CustomHtmlTemplateEditorPanel.Visible = (!StandardTemplateEditorPanel.Visible);

			// Update the user interface
			IntroTextBox.Text = homepage.IntroText;
			HomepageImageEditor1.Url = homepage.Url1;
			HomepageImageEditor2.Url = homepage.Url2;
			HomepageImageEditor3.Url = homepage.Url3;
			HomepageImageEditor4.Url = homepage.Url4;
			HomepageImageEditor1.Image.HomepageId = HomepageId;
			HomepageImageEditor2.Image.HomepageId = HomepageId;
			HomepageImageEditor3.Image.HomepageId = HomepageId;
			HomepageImageEditor4.Image.HomepageId = HomepageId;
			CustomHtmlTextBox.Text = homepage.CustomHtml;
			LastModifiedLabel.Text = string.Format("Last modified by {0} on {1}", homepage.LastModifiedByUser.FullName, homepage.LastModifiedDate.ToString(Global.DateFormat));

			// Select all categories in the UI and update the selected categories listbox
			// But only do this if the homepage being viewed is for the correct brand
			// Ie. if we're viewing the master homepage for another brand because we want to copy it
			// when published, then don't display the category bindings as the homepage being edited
			// won't have access to another brand's categories.
			if (homepage.BrandId == EditingBrandId)
			{
				foreach (Category category in homepage.CategoryList)
				{
					CategoriesTreeView.CheckNode(category.CategoryId.GetValueOrDefault());

					string categoryName = SiteUtils.GetFullCategoryName(category);
					string categoryId = category.CategoryId.ToString();
					ListItem listItem = new ListItem(categoryName, categoryId);

					SelectedCategoriesListBox.Items.Add(listItem);
				}
			}

			// Can only cancel if homepage is not published
			CancelButton.Visible = (!homepage.IsPublished);

			if (!homepage.IsPublished)
			{
				MessageLabel1.SetErrorMessage("This homepage is currently in draft mode.  To make live, click on 'publish'.");
				MessageLabel1.Pinned = true;
			}
		}

		#endregion
	}
}