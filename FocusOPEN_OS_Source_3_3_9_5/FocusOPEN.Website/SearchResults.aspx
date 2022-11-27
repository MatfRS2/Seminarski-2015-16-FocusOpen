<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="SearchResults.aspx.cs" Inherits="FocusOPEN.Website.SearchResults" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title>Search Results [<%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().ApplicationName%>]</title>
	<app:BrandHeader ID="BrandHeader1" Css="Basic|Menus|Styles|Other" runat="server" />
	<script src="Includes/Javascript/jQuery/jquery-1.4.1.min.js" type="text/javascript"></script>
	<script src="Includes/Javascript/jQuery/jquery-ui-1.7.2.custom.min.js" type="text/javascript"></script>
	<script src="Includes/Javascript/Global.js" type="text/javascript"></script>
	<link rel="Stylesheet" type="text/css" href="Includes/Css/jQuery-UI/smoothness/jquery-ui-1.7.2.custom.css" />
</head>
<body>
	<form id="form1" runat="server">
		<aspajax:ScriptManager ID="ScriptManager1"  runat="server" />
		<div class="CatWrapper">
			<app:PageHeader ID="PageHeader1" runat="server" />
			<div class="SrcBkg">
				<div class="CatAppPanel">
					<div class="Left"> 
						<div class="AppHdr">
							<div class="SrcGrpHdr Left"></div>
							<span class="PanelTxt">current view: <span class="Bold" id="CurrentViewLabel" runat="server">[thumbnails or list]</span> 
							[</span><asp:LinkButton ID="ToggleViewLinkButton" CssClass="PanelTxt" Text="View as [view]" OnClick="ToggleViewLinkButton_Click" runat="server" /><span class="PanelTxt">]</span>
							<asp:LinkButton ID="ListViewImageLinkButton" OnCommand="ChangeViewImageButton_Command" CommandArgument="list" runat="server"><asp:Image ID="ListViewImage" AlternateText="view as list" runat="server" /></asp:LinkButton>
							<asp:LinkButton ID="ThumbnailViewImageLinkButton" OnCommand="ChangeViewImageButton_Command" CommandArgument="thumbnails" runat="server"><asp:Image ID="ThumbnailViewImage" AlternateText="view as thumbnails"  runat="server" /></asp:LinkButton>
						</div>
						<div class="AppResults PanelTxt">total results: <span class="Bold"><%=AssetCount%></span> assets [displaying page <%=CurrentPage%> of <%=TotalPages%>]</div>
					</div>
					<div class="Right">
						<div class="AppPageView">
							<span class="PanelTxt">show</span>
							<app:PageSizeDropDownList ID="PageSizeDropDownList1" OmitBlankItem="true" CssClass="formInput" AutoPostBack="true" OnSelectedIndexChanged="PageSize_SelectedIndexChanged" runat="server" />
							<div class="AppPaging">
								<app:SimplePager ID="TopPager" OnCommand="Page_IndexChanged" runat="server"/>
							</div>
						</div>
					</div>
				</div>
				<div class="content">

					<app:FeedbackLabel ID="LargeSearchResultFeedbackLabel" runat="server" />

					<app:FeedbackLabel ID="FeedbackLabel1" runat="server" />

					<asp:DataList ID="SearchResultThumbnailsDataList" OnItemDataBound="SearchResultThumbnailsDataList_ItemDataBound" RepeatColumns="4" RepeatDirection="Horizontal" Width="749" BorderStyle="None" BorderWidth="0" CellSpacing="1" CellPadding="0" runat="server">
						<ItemStyle CssClass="SrcGridBkg" />
						<ItemTemplate>
							<asp:Panel ID="AssetInfoPanel" runat="server">
								<app:AssetThumbnail ID="AssetThumbnail1" LineBreaks="1" UseMarginCssClass="true" runat="server" />
								<span class="PanelTxt Bold"><asp:HyperLink ID="ReferenceHyperLink" CssClass="BodyTxt" runat="server" /></span><br />
								<span class="PanelTxt">(<asp:Label ID="AssetTypeLabel" runat="server" />)</span><br />
								<div class="ImgIcons"><app:AssetButtons ID="AssetButtons1" MessageLabelControlId="FeedbackLabel1" OnCartButtonClicked="AssetButtons1_CartButtonClicked" OnLightboxButtonClicked="AssetButtons1_LightboxButtonClicked" runat="server" /></div>
							</asp:Panel>
							<app:SpacerImage ID="SpacerImage1" Width="1" Height="1" runat="server" />
						</ItemTemplate>
					</asp:DataList>
					
					<app:SortableRepeater ID="SearchResultListRepeater" DefaultSortExpression="AssetId" DefaultSortAscending="true" ColumnCount="5" OnItemDataBound="SearchResultListRepeater_ItemDataBound" Visible="false" runat="server">
						<HeaderTemplate>
							<table width="750" border="0" cellspacing="0" cellpadding="2" class="TblOutline AutoStripe">
							<tr>
								<td id="HeaderCell1" runat="server" class="W50 alignCentre"><asp:LinkButton ID="LinkButton1" CssClass="BodyTxt Bold" Text="File" OnCommand="SortList_Command" CommandArgument="FileExtension,AssetId" runat="server" /></td>
								<td id="HeaderCell2" runat="server" class="W50 alignCentre"><asp:LinkButton ID="LinkButton2" CssClass="BodyTxt Bold" Text="Ref #" OnCommand="SortList_Command" CommandArgument="AssetId" runat="server" /></td>
								<td id="HeaderCell3" runat="server"><asp:LinkButton ID="LinkButton3" CssClass="BodyTxt Bold" Text="Title" OnCommand="SortList_Command" CommandArgument="Title" runat="server" /></td>
								<td id="HeaderCell4" runat="server" class="W75 alignCentre"><asp:LinkButton ID="LinkButton4" CssClass="BodyTxt Bold" OnCommand="SortList_Command" Text="Date" CommandArgument="ShadowDate" runat="server" /></td>
								<td id="HeaderCell5" runat="server" class="W75 alignCentre"><asp:LinkButton ID="LinkButton5" CssClass="BodyTxt Bold" OnCommand="SortList_Command" Text="File Size" CommandArgument="FileSize" runat="server" /></td>
								<td id="HeaderCell6" runat="server" class="W50 alignCentre"><asp:LinkButton ID="LinkButton6" CssClass="BodyTxt Bold" OnCommand="SortList_Command" Text="Type" CommandArgument="AssetTypeName" runat="server" /></td>
								<td id="HeaderCell7" runat="server" class="W100"><span class="BodyTxt Bold">Icons</span></td>
							</tr>
						</HeaderTemplate>
						<ItemTemplate>
							<tr>
								<td class="BodyTxt alignCentre"><asp:Image ID="FileTypeIconImage" runat="server" /></td>
								<td class="BodyTxt alignCentre"><asp:HyperLink ID="ReferenceHyperLink" CssClass="BodyTxt" runat="server" /></td>
								<td class="BodyTxt"><asp:HyperLink ID="TitleHyperLink" CssClass="BodyTxt" runat="server" /></td>
								<td class="BodyTxt alignCentre"><asp:Label ID="ProductionDateLabel" runat="server" /></td>
								<td class="BodyTxt alignCentre"><asp:Label ID="FileSizeLabel" runat="server" /></td>
								<td class="BodyTxt alignCentre"><asp:Label ID="AssetTypeLabel" runat="server" /></td>
								<td class="BodyTxt"><app:AssetButtons ID="AssetButtons1" MessageLabelControlId="FeedbackLabel1" OnCartButtonClicked="AssetButtons1_CartButtonClicked" OnLightboxButtonClicked="AssetButtons1_LightboxButtonClicked" runat="server" /></td>
							</tr>
						</ItemTemplate>
						<FooterTemplate>
							</table>
						</FooterTemplate>
					</app:SortableRepeater>
					
					<asp:Panel ID="NoSearchResultsPanel" Visible="false" runat="server">
						<app:FeedbackLabel ID="NoSearchResultsMessageLabel" MessageType="Negative" Header="no search results found" Visible="true" Pinned="true" runat="server" />
						<div class="noResults">
							<app:HelpHyperLink ID="HelpHyperLink1" HelpPage="2_Searching.htm#search_tips" CssClass="BodyTxt Bold H20" Text="view search tips" runat="server" />
							<span class="BodyTxt">|</span>
							<app:HelpHyperLink ID="HelpHyperLink2" HelpPage="2_Searching.htm" CssClass="BodyTxt H20" Text="help" runat="server" />
						</div>
					</asp:Panel>
					
					<div style="padding-top:5px;text-align:right"><asp:HyperLink ID="PermalinkHyperLink" Text="link to search" CssClass="PanelTxt" runat="server" /></div>
				</div>
				<div class="CatAppFtr">
					<div class="Left">
						<div class="AppResults PanelTxt">total results: <span class="Bold"><%=AssetCount%></span> assets [displaying page <%=CurrentPage%> of <%=TotalPages%>]</div>
					</div>
					<div class="Right">
						<div class="AppPageView AppPaging">
							<app:SimplePager ID="BottomPager" OnCommand="Page_IndexChanged" runat="server"/> 
						</div>
					</div>
				</div>
			</div>
			<app:PageFooter ID="PageFooter1" HelpPage="index.htm#catalogue" runat="Server" />
		</div>
		<app:PersistentCartLightbox
			ID="PersistentCartLightbox1"
			MoveToLastPage="true"
			OnLightboxChanged="PersistentCartLightbox1_Action"
			OnLightboxAssetsCopiedToCart="PersistentCartLightbox1_Action"
			OnEmptyCartLinkButtonClicked="PersistentCartLightbox1_Action"
			OnCartButtonClicked="PersistentCartLightbox1_Action"
			OnLightboxButtonClicked="PersistentCartLightbox1_Action"
			runat="server" />
	</form>
</body>
</html>