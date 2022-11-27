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
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.Services;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using log4net;

namespace FocusOPEN.Website.Admin
{
	public partial class ManageMetadataValuesDetail : BaseAdminPage
	{
		#region Private Variables

		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		#endregion

		#region Accessors

		protected int BrandId
		{
			get
			{
				return WebUtils.GetIntRequestParam("BrandId", 0);
			}
		}

		private static int GroupNumber
		{
			get
			{
				return WebUtils.GetIntRequestParam("GroupNumber", 0);
			}
		}

		#endregion

		#region WebMethods

		[WebMethod]
		public static string GetMetadata(int metadataId)
		{
			Metadata metadata = Metadata.Get(metadataId);
			return GetJsonResponse(metadata);
		}

		[WebMethod]
		public static string GetAllMetadata(int brandId)
		{
			IList<Metadata> metadata = GetParentMetadata(brandId, GroupNumber);
			return GetJsonResponse(metadata);
		}

		[WebMethod]
		public static DeleteMetadataResponse DeleteMetadata(int metadataId)
		{
			try
			{
				MetadataManager.Delete(metadataId);
				return new DeleteMetadataResponse {MetadataId = metadataId, Success = true, Message = "Metadata item deleted successfully"};
			}
			catch (Exception ex)
			{
				return new DeleteMetadataResponse {MetadataId = metadataId, Success = false, Message = ex.Message};
			}
		}

		[WebMethod]
		public static void MoveMetadata(int metadataId, int parentId, int position)
		{
		    MetadataManager.Move(metadataId, parentId, CurrentUser, position);
		}

		#endregion

		#region Constructor

		protected ManageMetadataValuesDetail()
		{
			PageId = "Content.ManageMetadata";
		}

		#endregion

		#region Page Methods

		protected void Page_Init(object sender, EventArgs e)
		{
			// Ensure brand admins can only access their own brand
			if (CurrentUser.UserRole.Equals(UserRole.BrandAdministrator) && CurrentUser.PrimaryBrandId != BrandId)
				Response.Redirect(string.Format("ManageMetadataValuesDetail.aspx?BrandId={0}&GroupNumber={1}", CurrentUser.PrimaryBrandId, GroupNumber));
		}

		protected void Page_Load(object sender, EventArgs e)
		{
            BrandMetadataSettingCache.Instance.InvalidateCache();
            BrandCache.Instance.InvalidateCache();

			Brand brand = BrandCache.Instance.GetById(BrandId);
            BrandMetadataSetting setting = brand.GetCustomMetadataSetting(GroupNumber);
            
			// Set page header and title
			PageHeader.InnerText = string.Format("Manage {0} Metadata", setting.FieldName);
			SetPageTitle(PageHeader.InnerText);

			// Setup return links
			ManageMetadataHyperlink1.NavigateUrl = ManageMetadataHyperlink2.NavigateUrl = string.Format("ManageMetadataValues.aspx?BrandId={0}", brand.BrandId);

			// Reload the cache
			CacheManager.InvalidateCache("Metadata", CacheType.Local);

			// First ensure brand has a root metadata
			MetadataManager.EnsureRootExists(BrandId, GroupNumber, setting.FieldName, CurrentUser);

		    PreviewContainerDiv.Visible = false;

			if (!Page.IsPostBack)
			{
			    ucEditDetails.SelectableSetting = setting.SelectableSetting; 
                ucEditDetails.DataBind();
			}
		}

		protected void UpdateButton_Click(object sender, EventArgs e)
		{
			int metadataId = NumericUtils.ParseInt32(MetadataIdTextBox.Value, 0);

			if (metadataId == 0)
			{
				int parentId = NumericUtils.ParseInt32(ParentIdTextBox.Value, 0);
				CreateMetadata(parentId);
			}           
			else
			{
				UpdateMetadata(metadataId);
			}
		}

		protected void SaveSettingsButton_Click(object sender, EventArgs e)
		{
            var brand = BrandCache.Instance.GetById(BrandId);
            var setting = brand.GetCustomMetadataSetting(GroupNumber);
		    var selectable = ucEditDetails.RetrieveSetting();
		    var updateVals = false;

            if (!setting.IsNew && !setting.IsNull && setting.UiControlType == (int)BrandMetadataUiControlType.Select)
            {//check if the type is not changing which would require updating all metadata vals entries of all assets in the database
                //as that will no longer be reflecting the right values
                updateVals = setting.SelectableSetting.SelectableType != selectable.SelectableType;
            }

		    selectable.BrandMetadataSettingId = setting.BrandMetadataSettingId.GetValueOrDefault();

		    BrandMetadataSelectableSetting.Update(selectable);

		    ucEditDetails.BrandMetadataSelectableSettingId = selectable.BrandMetadataSelectableSettingId.GetValueOrDefault();

            if(updateVals)
            {
                var assets = Asset.FindMany(new AssetFinder() {IsDeleted = false});
                foreach (var asset in assets)
                {
                    asset.MetadataList.ToString();
                    asset.MetadataTextFieldsList.ToString();
                    asset.MetadataTextAreasList.ToString();

                    Asset.SaveAssetMetadata(asset);    
                }
            }

		    var msg = "Metadata selection options updated successfully" + (updateVals? ".<br />Asset search metadata updated successfully." : "");

			MessageLabel1.SetSuccessMessage(msg);

			CacheManager.InvalidateCache("BrandMetadata", CacheType.All);
		}

        protected void PreviewControlHyperlink_Click(object sender, EventArgs e)
        {
            var brand = BrandCache.Instance.GetById(BrandId);
            var brandMetaSetting = brand.GetMetadataSetting(BrandMetadataSettings.METADATA_PREFIX + GroupNumber);
            brandMetaSetting.SetSelectableSetting(ucEditDetails.RetrieveSetting());
            PreviewMetadataInputWrapper.SetTempBrandMetaSetting(brandMetaSetting);

            PreviewMetadataInputWrapper.IncludeJqueryReference = false;
            PreviewMetadataInputWrapper.GroupNumber = GroupNumber;
            PreviewMetadataInputWrapper.BrandId = BrandId;
            PreviewMetadataInputWrapper.ResetState();
            PreviewMetadataInputWrapper.InitInput(true);
            PreviewMetadataInputWrapper.RefreshFromBrandAndSelet(BrandId, new int[]{}, string.Empty);

            PreviewContainerDiv.Visible = true;

            ucEditDetails.OpenDetails();
        }
		
		#endregion

		#region Private Methods

		private static IList<Metadata> GetParentMetadata(int brandId, int groupNumber)
		{
			return (from m in MetadataCache.Instance.GetList(brandId, groupNumber)
			        where !m.ParentMetadataId.HasValue
			        select m).ToList();
		}

		/// <summary>
		/// Generates a Json response as expected by jsTree
		/// </summary>
		private static string GetJsonResponse(IList<Metadata> metadata)
		{
		    metadata = metadata.OrderBy(m => m.MetadataOrder).ToList();

			var response = new StringBuilder();
			response.Append("[");

			for (int i = 0; i < metadata.Count; i++)
			{
				Metadata m = metadata[i];

				response.Append("{");
                response.AppendFormat("attributes: {{ id: \"{0}\", metadataOrder:\"{1}\"}}", m.MetadataId, m.MetadataOrder);
				response.AppendFormat(", data: \"{0}\"", m.Name);

				if (m.Children.Count > 0)
				{
                    var childrenOrdered = m.Children.OrderBy(c => c.MetadataOrder).ToList();
                    response.AppendFormat(", children: {0}", GetJsonResponse(childrenOrdered));
				}

				response.Append("}");

				if (i + 1 < metadata.Count)
					response.Append(", ");
			}

			response.Append("]");
			return response.ToString();
		}

		private static string GetJsonResponse(Metadata metadata)
		{
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			return serializer.Serialize(GetMetadataForJson(metadata));
		}

		private static MetadataForJson GetMetadataForJson(Metadata metadata)
		{
			return new MetadataForJson { MetadataId = metadata.MetadataId, Name = metadata.Name, ExternalRef = metadata.ExternalRef, synonyms = metadata.Synonyms };
		}

		private void CreateMetadata(int parentId)
		{
			try
			{
				string name = NameTextBox.Text;
				string reference = ExternalRefTextBox.Text;
				string synonyms = SynonymsTextBox.Text;

				Metadata o = MetadataManager.Add(name, parentId, BrandId, GroupNumber, reference, synonyms, CurrentUser);

				MetadataIdTextBox.Value = o.MetadataId.ToString();
				MessageLabel1.SetSuccessMessage("metadata created successfully");
			}
			catch (BaseEntityException<Metadata> ex)
			{
				if (ex.Errors.Count > 0)
				{
					MessageLabel1.SetErrorMessage("Unable to create metadata; The following errors occured:", ex.Errors);
				}
				else
				{
					MessageLabel1.SetErrorMessage(ex.Message);
				}
			}
			catch (Exception ex)
			{
				m_Logger.Fatal(string.Format("Error creating metadata with parent id: {0}", parentId), ex);
				MessageLabel1.SetErrorMessage("An unhandled error occured", ex.ToString());
			}
		}

		private void UpdateMetadata(int metadataId)
		{
			try
			{
				string name = NameTextBox.Text;
				string reference = ExternalRefTextBox.Text;
				string synonyms = SynonymsTextBox.Text;

				MetadataManager.Update(metadataId, name, reference, synonyms, CurrentUser);
				MessageLabel1.SetSuccessMessage("Metadata updated successfully");
			}
			catch (BaseEntityException<Metadata> ex)
			{
				if (ex.Errors.Count > 0)
				{
					MessageLabel1.SetErrorMessage("Unable to update metadata; The following errors occured:", ex.Errors);
				}
				else
				{
					MessageLabel1.SetErrorMessage(ex.Message);
				}
			}
			catch (Exception ex)
			{
				m_Logger.Fatal(string.Format("Error editing metadata with id: {0}", metadataId), ex);
				MessageLabel1.SetErrorMessage("An unhandled error occured", ex.ToString());
			}
		}

		#endregion
	}

	[Serializable]
	public struct DeleteMetadataResponse
	{
		public int MetadataId { get; set; }
		public bool Success { get; set; }
		public string Message { get; set; }
	}

	[Serializable]
	public struct MetadataForJson
	{
		public int? MetadataId { get; set; }
		public string ExternalRef { get; set; }
		public string Name { get; set; }
		public string synonyms { get; set; }
	}
}