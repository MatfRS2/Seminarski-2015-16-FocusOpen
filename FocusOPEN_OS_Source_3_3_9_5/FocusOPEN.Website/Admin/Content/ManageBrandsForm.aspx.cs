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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using ICSharpCode.SharpZipLib.Zip;
using log4net;

namespace FocusOPEN.Website.Admin.Content
{
	public partial class ManageBrandsForm : BaseAdminPage
	{
		#region Private Variables

		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		#endregion

		#region Handled Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				Brand brand = Brand.Get(WebUtils.GetIntRequestParam("BrandId", 0));

				if (!brand.IsNull)
				{
					// Populate the user interface
					FullBrandNameTextBox.Text = brand.Name;
					ShortBrandNameTextBox.Text = brand.ShortName;
					ApplicationNameTextBox.Text = brand.ApplicationName;
					OrganisationNameTextBox.Text = brand.OrganisationName;
					WebsiteUrlTextBox.Text = brand.WebsiteUrl;
					EmailFromTextBox.Text = brand.EmailFrom;
					AllowBrandSelectionDuringRegistrationCheckBox.Checked = brand.IsBrandSelectionAllowed;
					LoginPageUpperCopyTextBox.Text = brand.LoginPageUpperCopy;
					LoginPageLowerCopyTextBox.Text = brand.LoginPageLowerCopy;
					DefaultUsageRestrictionsCopyTextBox.Text = brand.DefaultUsageRestrictionsCopy;
					MyAccountCopyTextBox.Text = brand.MyAccountCopy;
					AdminCopyTextBox.Text = brand.AdminCopy;
					TermsConditionsCopyTextBox.Text = brand.TermsConditionsCopy;
					PrivacyPolicyCopyTextBox.Text = brand.PrivacyPolicyCopy;
					EnableDirectDownloadCheckBox.Checked = brand.DirectDownloadEnabled;

					if (DisabledPoweredByFooterCheckBox != null)
						DisabledPoweredByFooterCheckBox.Checked = brand.DisablePoweredByLogo;

					// Brand folder
					string brandFolder = string.Format("~/Brands/Brand_{0}/", brand.BrandId);
					string absBrandFolder = Server.MapPath(brandFolder);

					if (Directory.Exists(absBrandFolder))
					{
						// Get the watermark image extension
						string extension = (from f in Directory.GetFiles(absBrandFolder)
						                    where (Path.GetFileName(f) ?? string.Empty).StartsWith("watermark")
						                    select Path.GetExtension(f)).FirstOrDefault();

						// Set the watermark image url
						PreviewWatermarkImage.ImageUrl = string.Format("{0}watermark{1}", brandFolder, extension);

						// Hide preview for non-existent watermark
						WatermarkImageWrapper.Visible = File.Exists(Server.MapPath(PreviewWatermarkImage.ImageUrl));
					}

					// UI folder relative path
					string uiPath = Path.Combine(absBrandFolder, "UI");

					// Setup UI management controls
					BrandFilePackActionPanel.Visible = Directory.Exists(uiPath);
					BrowseBrandFolderHyperLink.NavigateUrl = string.Concat(brandFolder, "UI/");
					EditBrandFilesHyperLink.NavigateUrl = "~/Admin/Utils/SkinEditor.aspx?BrandId=" + brand.BrandId;

					// Count assets assigned to this brand
					AssetFinder finder1 = new AssetFinder {BrandId = brand.BrandId.GetValueOrDefault()};
					int count = Asset.GetCount(finder1);

					// Count users assigned to this brand if there are no assets assigned to it
					// We only need to know that NO users or assets are assigned, so no point doing
					// this check and hammering the database if we already know there are assets
					if (count == 0)
					{
						UserBrandFinder finder2 = new UserBrandFinder { BrandId = brand.BrandId.GetValueOrDefault() };
						count = UserBrand.GetCount(finder2);
					}

					// Only show the delete button if there are no users or assets
					// assigned.  Otherwise, the brand cannot be deleted.
					DeleteButton.Visible = (count == 0);
					DeleteButton.CommandArgument = brand.BrandId.ToString();
				}
			}
		}

		protected void DownloadBrandFilePackLinkButton_Click(object sender, EventArgs e)
		{
			// Get the brand ID
			int brandId = WebUtils.GetIntRequestParam("BrandId", 0);

			// Get the brand
			Brand brand = BrandCache.Instance.GetById(brandId);

			// Brand folder path
			string relPath = string.Format("~/Brands/Brand_{0}/", brandId);
			string absPath = Server.MapPath(relPath);

			// Exit if brand folder doesn't exist
			if (!Directory.Exists(absPath))
				return;

			// Temp path to zip file
			string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".zip");

			// Zip up the brand folder
			FastZip fz = new FastZip();
			fz.CreateZip(path, absPath, true, null);

			// Send it
			string downloadFilename = brand.ShortName + ".zip";
			FileUtils.TransmitFile(path, downloadFilename, HttpContext.Current);
		}

		protected void SaveButton_Click(object sender, EventArgs e)
		{
			Brand brand = Brand.Empty;

			if (WebUtils.GetIntRequestParam("BrandId", 0) != 0)
				brand = Brand.Get(WebUtils.GetIntRequestParam("BrandId", 0));

			if (brand.IsNull)
				brand = Brand.New();

			brand.Name = FullBrandNameTextBox.Text.Trim();
			brand.ShortName = ShortBrandNameTextBox.Text.Trim();
			brand.ApplicationName = ApplicationNameTextBox.Text.Trim();
			brand.OrganisationName = OrganisationNameTextBox.Text.Trim();
			brand.WebsiteUrl = WebsiteUrlTextBox.Text.Trim();
			brand.EmailFrom = EmailFromTextBox.Text.Trim();
			brand.IsBrandSelectionAllowed = AllowBrandSelectionDuringRegistrationCheckBox.Checked;
			brand.DisablePoweredByLogo = (DisabledPoweredByFooterCheckBox != null) ? DisabledPoweredByFooterCheckBox.Checked : false;
			brand.LoginPageUpperCopy = LoginPageUpperCopyTextBox.Text.Trim();
			brand.LoginPageLowerCopy = LoginPageLowerCopyTextBox.Text.Trim();
			brand.DefaultUsageRestrictionsCopy = DefaultUsageRestrictionsCopyTextBox.Text.Trim();
			brand.MyAccountCopy = MyAccountCopyTextBox.Text.Trim();
			brand.AdminCopy = AdminCopyTextBox.Text.Trim();
			brand.TermsConditionsCopy = TermsConditionsCopyTextBox.Text.Trim();
			brand.PrivacyPolicyCopy = PrivacyPolicyCopyTextBox.Text.Trim();
			brand.DirectDownloadEnabled = EnableDirectDownloadCheckBox.Checked;

			BinaryFile filePack = new BinaryFile(BrandFilePackUpload.PostedFile);
			BinaryFile watermarkImage = new BinaryFile(PreviewWatermarkImageUpload.PostedFile);

			try
			{
				if (!IsBrandFolderWriteable())
				{
					MessageLabel1.SetErrorMessage("Error saving brand", "Brand folder not writeable - please change the permissions in the brands folder");
					return;
				}

				// Temp flag to check if brand is new
				// Do not inline. This will be cleared when brand is saved.
				bool isNew = brand.IsNew;

				// Validate and save the brand info to the database
				BrandManager.Save(brand, filePack, watermarkImage);

				// Update log
				AuditUserAction action = (isNew) ? AuditUserAction.AddBrand : AuditUserAction.ModifyBrand;
				AuditLogManager.LogUserAction(CurrentUser, action, string.Format("Saved brand: {0}. Brand ID: {1}", brand.Name, brand.BrandId));

				// Get the relative brand folder
				string brandFolderPath = string.Format("~/Brands/Brand_{0}/", brand.BrandId);

				// Convert brand folder to absolute path
				string absBrandFolderPath = Server.MapPath(brandFolderPath);

				// Create the brand folder if it doesn't exist
				if (!Directory.Exists(absBrandFolderPath))
				{
					Directory.CreateDirectory(absBrandFolderPath);
					m_Logger.DebugFormat("Created brand folder path: {0}", absBrandFolderPath);
				}

				// Ensure specific brand folder is writeable
				string tmpPath = Path.Combine(absBrandFolderPath, Guid.NewGuid() + ".tmp");

				try
				{
					File.WriteAllText(tmpPath, "Folder is writeable");
					File.Delete(tmpPath);
				}
				catch (Exception ex)
				{
					Debug.WriteLine("Error checking if brand folder is writeable: " + ex.Message);
					MessageLabel1.SetErrorMessage(string.Format("Brand folder not writeable (~/Brands/Brand_{0}/) - please change the permissions in the brands folder", brand.BrandId));
					return;
				}

				// Save watermark
				if (!watermarkImage.IsEmpty)
				{
					// Get any old watermark files
					var files = (from f in Directory.GetFiles(absBrandFolderPath)
					             where (Path.GetFileName(f) ?? string.Empty).ToLower().StartsWith("watermark")
					             select f);

					// Delete them
					foreach (string file in files)
						File.Delete(file);

					// Save new watermark
					string watermarkPath = Path.Combine(absBrandFolderPath, "watermark." + watermarkImage.FileExtension);
					watermarkImage.SaveAs(watermarkPath);
				}

				if (!filePack.IsEmpty)
				{
					// Save the zip file to disk
					string tempZipPath = Path.GetTempFileName() + ".zip";
					filePack.SaveAs(tempZipPath);

					// Extract the uploaded zip into the brand folder
					FastZip fz = new FastZip();
					fz.ExtractZip(tempZipPath, absBrandFolderPath, FastZip.Overwrite.Always, null, string.Empty, string.Empty, true);

					// Delete the temp zip file
					File.Delete(tempZipPath);
				}
				
				if (filePack.IsEmpty && isNew)
				{
					Brand masterBrand = WebsiteBrandManager.GetMasterBrand();
					string masterBrandFolder = Server.MapPath(string.Format("~/Brands/Brand_{0}/", masterBrand.BrandId));

					try
					{
						CopyFolder(masterBrandFolder, absBrandFolderPath);
						m_Logger.DebugFormat("Copied folder {0} to {1}", masterBrandFolder, absBrandFolderPath);
					}
					catch (Exception ex)
					{
						m_Logger.Error(string.Format("Error copying Brand folder from master brand to new brand: {0}. Error: {1}", brand.Name, ex.Message), ex);
					}
				}

				Response.Redirect("ManageBrands.aspx?message=BrandSaved", false);
			}
			catch (InvalidBrandException ibex)
			{
				MessageLabel1.SetErrorMessage("Error saving brand", ibex.Errors);
			}
			catch (Exception ex)
			{
				MessageLabel1.SetErrorMessage("An unknown error occurred while saving brand", ex.Message);
				m_Logger.ErrorFormat("Error saving brand: " + ex.Message, ex);
				ExceptionHandler.HandleException(ex, "Error saving brand");
			}
		}

		protected void DeleteButton_Command(object sender, CommandEventArgs e)
		{
			try
			{
				// Get brand Id
				int brandId = NumericUtils.ParseInt32(e.CommandArgument.ToString(), 0);

				// Get the brand
				Brand brand = BrandCache.Instance.GetById(brandId);

				// Delete brand from database
				BrandManager.Delete(brandId);

				// Update log
				AuditLogManager.LogUserAction(CurrentUser, AuditUserAction.DeleteBrand, string.Format("Deleted brand: {0}. Brand ID: {1}", brand.Name, brand.BrandId));

				// Delete brand folder.
				string brandFolderPath = Server.MapPath(string.Format("~/Brands/Brand_{0}/", brandId));

				if (Directory.Exists(brandFolderPath))
				{
					Directory.Delete(brandFolderPath, true);
					m_Logger.InfoFormat("Deleted brand folder: {0}", brandFolderPath);
				}
				else
				{
					m_Logger.InfoFormat("Brand folder not deleted, does not exist: {0}", brandFolderPath);
				}

				// Redirect back to brand list
				Response.Redirect("ManageBrands.aspx?message=BrandDeletedSuccessfully");
			}
			catch (Exception ex)
			{
				MessageLabel1.SetErrorMessage("Error deleting brand", ex.Message);
			}
		}

		protected void DownloadWatermarkLinkButton_Click(object sender, EventArgs e)
		{
			string url = PreviewWatermarkImage.ImageUrl;
			string path = Server.MapPath(url);
			FileUtils.TransmitFile(path, HttpContext.Current);
		}

		#endregion

		#region Private Helper Methods

		private bool IsBrandFolderWriteable()
		{
			string path = Server.MapPath("~/Brands/" + Guid.NewGuid());

			try
			{
				Directory.CreateDirectory(path);
				Directory.Delete(path);
				return true;
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Error creating folder to check brand folder permissions: " + ex.Message);
			}

			return false;
		}

		private static void CopyFolder(string sourceFolder, string destFolder)
		{
			if (!Directory.Exists(destFolder))
				Directory.CreateDirectory(destFolder);

			string[] files = Directory.GetFiles(sourceFolder);

			foreach (string file in files)
			{
				string name = Path.GetFileName(file) ?? string.Empty;

				if (StringUtils.IsBlank(name))
					continue;

				string dest = Path.Combine(destFolder, name);
				File.Copy(file, dest);
			}

			string[] folders = Directory.GetDirectories(sourceFolder);
			
			foreach (string folder in folders)
			{
				string name = Path.GetFileName(folder) ?? string.Empty;

				if (StringUtils.IsBlank(name))
					continue;

				string dest = Path.Combine(destFolder, name);
				CopyFolder(folder, dest);
			}
		}

		#endregion
	}
}