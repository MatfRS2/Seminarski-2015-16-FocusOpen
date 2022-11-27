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
using Daydream.Data;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using FocusOPEN.Website.Controls;

namespace FocusOPEN.Website
{
	public partial class SearchResults : BasePageablePage
	{
		#region Constructors

		static SearchResults()
		{
			ShowFiltersCount = 1000;
		}

		protected SearchResults()
		{
			PageStatePersisterType = PersisterType.ZipCompressed;
		}

		#endregion

		#region Accessors

		/// <summary>
		/// Gets or sets the number of assets in a search that triggers automatically
		/// showing the filter panel.
		/// </summary>
		public static int ShowFiltersCount { get; set; }

		/// <summary>
		/// Shortcut to the saved asset search in the session
		/// </summary>
		private static SavedUserAssetSearch SavedUserAssetSearch
		{
			get
			{
				return SessionInfo.Current.UserSessionInfo.SavedUserAssetSearch;
			}
		}

        protected override ContextType AssetContext
        {
            get
            {
                return ContextType.SearchResults;
            }
        }

		#endregion

		#region Handled Events

		protected void Page_Init(object sender, EventArgs e)
		{
			// Only enable the scroll position save feature for Firefox or IE7+, as it doesnt work properly in IE6.
			MaintainScrollPositionOnPostBack = (Request.Browser.Browser == "Firefox" || Request.Browser.Browser == "IE" && Request.Browser.MajorVersion >= 7);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			FeedbackLabel1.Visible = false;

			if (!Page.IsPostBack)
			{
				PageSizeDropDownList1.SafeSelectValue(SavedUserAssetSearch.PageSize);
				ChangeSearchResultsView(SavedUserAssetSearch.SearchResultsView);
				Bind(SavedUserAssetSearch.Page);
			}
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			PermalinkHyperLink.NavigateUrl = SearchUrlMapper.GetUrlForSearch(SessionInfo.Current.UserSessionInfo.SavedUserAssetSearch);
		}

		protected void SearchResultThumbnailsDataList_ItemDataBound(object sender, DataListItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case (ListItemType.Item):
				case (ListItemType.AlternatingItem):

					Asset asset = (Asset) e.Item.DataItem;

					Panel AssetInfoPanel = (Panel) e.Item.FindControl("AssetInfoPanel");

					if (asset.IsNull)
					{
						AssetInfoPanel.Visible = false;

						// Clear the CSS class so the background doesn't show
						WebControl wc = (WebControl) AssetInfoPanel.Parent;
						wc.CssClass = string.Empty;
					}
					else
					{
						AssetThumbnail AssetThumbnail1 = (AssetThumbnail)e.Item.FindControl("AssetThumbnail1");
						AssetThumbnail1.Initialise(asset);

						AssetButtons AssetButtons1 = (AssetButtons)e.Item.FindControl("AssetButtons1");
						AssetButtons1.LightboxId = PersistentCartLightbox.SelectedLightboxId;
						AssetButtons1.Initialise(asset);
						
						Label AssetTypeLabel = (Label)e.Item.FindControl("AssetTypeLabel");
						AssetTypeLabel.Text = asset.AssetType.Name;

						HyperLink ReferenceHyperLink = (HyperLink)e.Item.FindControl("ReferenceHyperLink");
						ReferenceHyperLink.Text = asset.AssetId.ToString();
						ReferenceHyperLink.NavigateUrl = SiteUtils.GetShowAssetInfoUrl(asset.AssetId.GetValueOrDefault());
					}

					break;
			}
		}

		protected void SearchResultListRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case (ListItemType.Item):
				case (ListItemType.AlternatingItem):

					Asset asset = (Asset) e.Item.DataItem;

					AssetButtons AssetButtons1 = (AssetButtons) e.Item.FindControl("AssetButtons1");
					Image FileTypeIconImage = (Image) e.Item.FindControl("FileTypeIconImage");
					HyperLink ReferenceHyperLink = (HyperLink)e.Item.FindControl("ReferenceHyperLink");
					HyperLink TitleHyperLink = (HyperLink)e.Item.FindControl("TitleHyperLink");
					Label ProductionDateLabel = (Label) e.Item.FindControl("ProductionDateLabel");
					Label FileSizeLabel = (Label) e.Item.FindControl("FileSizeLabel");
					Label AssetTypeLabel = (Label) e.Item.FindControl("AssetTypeLabel");

					AssetButtons1.LightboxId = PersistentCartLightbox.SelectedLightboxId;
					AssetButtons1.Initialise(asset);

					FileTypeIconImage.ImageUrl = SiteUtils.GetFileTypeImageUrl(asset.FileExtension);
					FileTypeIconImage.ToolTip = asset.Filename;

					ReferenceHyperLink.Text = asset.AssetId.ToString();
					ReferenceHyperLink.NavigateUrl = SiteUtils.GetShowAssetInfoUrl(asset.AssetId.GetValueOrDefault());
					TitleHyperLink.Text = asset.Title;
					TitleHyperLink.NavigateUrl = ReferenceHyperLink.NavigateUrl;
					ProductionDateLabel.Text = asset.GetProductionDate();
					FileSizeLabel.Text = FileUtils.FriendlyFileSize(asset.FileSize);
					AssetTypeLabel.Text = asset.AssetType.Name;

					string caption = SiteUtils.MakeJsSafe(SiteUtils.GetAssetHoverCaption(asset, string.Empty));
					string text = SiteUtils.MakeJsSafe(SiteUtils.GetAssetHoverText(asset, string.Empty));

					TitleHyperLink.Attributes["OnMouseOver"] = string.Format("showToolTip('{0}', '{1}')", caption, text);
					TitleHyperLink.Attributes["OnMouseOut"] = "hideToolTip()";

					if (StringUtils.IsBlank(TitleHyperLink.Text))
					{
						TitleHyperLink.Text = "not available";
						TitleHyperLink.CssClass += " Italics";
					}

					break;
			}
		}

		protected void PageSize_SelectedIndexChanged(object sender, EventArgs e)
		{
			Bind(1);
		}

		protected void Page_IndexChanged(object sender, CommandEventArgs e)
		{
			int pg = Convert.ToInt32(e.CommandArgument);
			Bind(pg);
		}

		protected void ToggleViewLinkButton_Click(object sender, EventArgs e)
		{
			SearchResultsView view = (SavedUserAssetSearch.SearchResultsView == SearchResultsView.Thumbnails) ? SearchResultsView.List : SearchResultsView.Thumbnails;
			ChangeSearchResultsView(view);
			Bind(CurrentPage, false);
		}

		protected void ChangeViewImageButton_Command(object sender, CommandEventArgs e)
		{
			SearchResultsView searchResultsView = GeneralUtils.ParseEnum(e.CommandArgument.ToString(), SearchResultsView.Thumbnails);
			ChangeSearchResultsView(searchResultsView);
			Bind(CurrentPage, false);
		}

		protected void SortList_Command(object sender, CommandEventArgs e)
		{
			SearchResultListRepeater.UpdateSortExpression(e.CommandArgument.ToString());
			SavedUserAssetSearch.AssetFinder.OrderBy = OrderBy.Custom;
			SavedUserAssetSearch.AssetFinder.SortExpressions.Clear();
			SavedUserAssetSearch.AssetFinder.SortExpressions.AddRange(SearchResultListRepeater.GetSortExpressions());
			Bind(1);
		}

		protected void AssetButtons1_CartButtonClicked(object sender, AssetButtonEventArgs e)
		{
			if (SessionInfo.Current.UserSessionInfo.PersistentLightboxCartInfo.PersistentCartLightboxMode == PersistentCartLightboxMode.Cart)
			{
				PersistentCartLightbox1.Bind();
			}
		}

		protected void AssetButtons1_LightboxButtonClicked(object sender, AssetButtonEventArgs e)
		{
			if (SessionInfo.Current.UserSessionInfo.PersistentLightboxCartInfo.PersistentCartLightboxMode == PersistentCartLightboxMode.Lightbox)
			{
				PersistentCartLightbox1.Bind();
			}
		}

		protected void PersistentCartLightbox1_Action(object sender, EventArgs e)
		{
			Bind(CurrentPage, false);
		}

		#endregion

		#region Private Helper Methods

		private void ChangeSearchResultsView(SearchResultsView searchResultsView)
		{
			switch (searchResultsView)
			{
				case (SearchResultsView.Thumbnails):

					CurrentViewLabel.InnerText = "thumbnails";
					ToggleViewLinkButton.Text = "view as list";
					ListViewImage.ImageUrl = SiteUtils.GetIconPath("list_view0.gif");
					ListViewImageLinkButton.Enabled = true;
					ThumbnailViewImage.ImageUrl = SiteUtils.GetIconPath("thumb_view2.gif");
					ThumbnailViewImageLinkButton.Enabled = false;

					break;

				case (SearchResultsView.List):

					CurrentViewLabel.InnerText = "list";
					ToggleViewLinkButton.Text = "view as thumbnails";
					ThumbnailViewImage.ImageUrl = SiteUtils.GetIconPath("thumb_view0.gif");
					ThumbnailViewImageLinkButton.Enabled = true;
					ListViewImage.ImageUrl = SiteUtils.GetIconPath("list_view2.gif");
					ListViewImageLinkButton.Enabled = false;

					break;

				default:
					throw new Exception(string.Format("Unknown SearchResultsView: {0}", searchResultsView));
			}

			SavedUserAssetSearch.SearchResultsView = searchResultsView;
			CookieManager.SetValue("SearchResultsView", searchResultsView);
		}

		private void Bind(int page)
		{
			Bind(page, true);
		}

		private void Bind(int page, bool updateAuditLog)
		{
			AssetFinder finder = SavedUserAssetSearch.AssetFinder;
			EntityList<Asset> assets = Asset.FindMany(finder, page - 1, PageSize);

			if (assets.Count == 0)
			{
				NoSearchResultsPanel.Visible = true;
				SearchResultListRepeater.Visible = false;
				SearchResultThumbnailsDataList.Visible = false;
			}
			else
			{
				// Hide the no results panel; we have some assets
				NoSearchResultsPanel.Visible = false;

				// Update page size
				SavedUserAssetSearch.PageSize = PageSize;

				// Update page
				SavedUserAssetSearch.Page = page;

				// Show filters panel if a search term was entered, we're on page 1, and have more than the specified number of results
				if (!StringUtils.IsBlank(finder.GeneralKeyword) && (page == 1) && (ShowFiltersCount > 0) && (assets.PagingInfo.TotalRecords > ShowFiltersCount))
				{
					PageHeader pageHeader = (PageHeader) SiteUtils.FindControlRecursive(Page, "PageHeader1");
                    pageHeader.ToggleSearchVisibility(SavedUserAssetSearch.FilterOpen, SavedUserAssetSearch.CategoriesOpen);

					string message = string.Format("Your search has returned more than {0} results. Use the filters above to refine your search.", ShowFiltersCount);
					LargeSearchResultFeedbackLabel.SetErrorMessage(message);
					LargeSearchResultFeedbackLabel.Pinned = true;
				}
				else
				{
					LargeSearchResultFeedbackLabel.Visible = false;
					LargeSearchResultFeedbackLabel.Pinned = false;
				}

				// Log which assets were returned as part of this search
				if (updateAuditLog)
				{
					int index = 0;

					foreach (Asset asset in assets)
					{
						index++;

						AuditLogManager.LogAssetInSearchResult(SavedUserAssetSearch.AuditAssetSearchId, asset.AssetId.GetValueOrDefault());
						AuditLogManager.LogAssetAction(asset, CurrentUser, AuditAssetAction.ReturnedInSearch, string.Format("Search Criteria: {0}. Total assets: {1}. Displayed on page: {2}, asset number: {3}", finder.GeneralKeyword, assets.PagingInfo.TotalRecords, assets.PagingInfo.CurrentPage + 1, index));
					}
				}

				// The current view is thumbnails, so ensure we have at last a single row and toggle
				// the relevant data controls (ie. hide the list repeater and show the thumbnail datalist)
				if (SavedUserAssetSearch.SearchResultsView == SearchResultsView.Thumbnails)
				{
					// In thumbnail view, we need to make sure we have at least one row of assets, or the
					// layout gets really messed up.  Here, we're doing a check, and adding in empty assets
					// to pad out the asset list to at least a single row.
					while (assets.Count < SearchResultThumbnailsDataList.RepeatColumns)
					{
						assets.Add(Asset.Empty);
					}

					SearchResultListRepeater.Visible = false;
					SearchResultThumbnailsDataList.Visible = true;

					SearchResultThumbnailsDataList.DataSource = assets;
					SearchResultThumbnailsDataList.DataBind();
				}
				else
				{
					SearchResultListRepeater.Visible = true;
					SearchResultThumbnailsDataList.Visible = false;

					SearchResultListRepeater.DataSource = assets;
					SearchResultListRepeater.DataBind();
				}
			}

			TopPager.CurrentPage = assets.PagingInfo.CurrentPage + 1;
			TopPager.PageSize = assets.PagingInfo.PageSize;
			TopPager.PageCount = assets.PagingInfo.TotalPages;
			TopPager.ItemCount = assets.PagingInfo.TotalRecords;

			BottomPager.CurrentPage = TopPager.CurrentPage;
			BottomPager.PageSize = TopPager.PageSize;
			BottomPager.PageCount = TopPager.PageCount;
			BottomPager.ItemCount = TopPager.ItemCount;

			CurrentPage = page;
			TotalPages = assets.PagingInfo.TotalPages;
			AssetCount = assets.PagingInfo.TotalRecords;
		}

		#endregion
	}
}