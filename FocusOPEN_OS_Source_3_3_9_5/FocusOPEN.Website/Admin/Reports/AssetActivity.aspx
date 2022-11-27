<%@ Page Title="Asset Activity Report [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="AssetActivity.aspx.cs" Inherits="FocusOPEN.Website.Admin.AssetActivity" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg"> 
		<div class="Left AdmLeftPanel"> 
			<div class="AdmAppHdr">Asset Activity Report</div>
			<br />
			<app:FeedbackLabel ID="FeedbackLabel1" UseContainer="true" runat="server" />
			<br />
			<span class="AppCtrlMrg BodyTxt Bold">1. Use the checkboxes below to select the activities to include in this report</span>
			<span class="BodyTxt">|</span>
			<app:HelpHyperLink ID="HelpHyperLink1" HelpPage="11_Reports.htm#asset_activity_reports" Text="help" CssClass="BodyTxt" runat="server" /><br />
			<div class="AppCtrlMrg AppCtrlPanel W500">
				<table border="0" cellspacing="0" cellpadding="2" class="W500">
				<tr>
					<td>
						<asp:CheckBox ID="UploadedCheckBox" CssClass="BodyTxt" Text="Uploads" runat="server" /><br />
						<asp:CheckBox ID="DownloadedCheckBox" CssClass="BodyTxt" Text="Downloads" runat="server" /><br />
						<asp:CheckBox ID="OrderRequestsCheckBox" CssClass="BodyTxt" Text="Order Requests" runat="server" /><br />
						<asp:CheckBox ID="ViewsCheckBox" CssClass="BodyTxt" Text="Views" runat="server" /><br />
					</td>
					<td valign="top"> 
						<asp:CheckBox ID="SearcHitsCheckBox" CssClass="BodyTxt" Text="Search Hits" runat="server" /><br />
						<asp:CheckBox ID="CartCheckBox" CssClass="BodyTxt" Text="Add To Cart" runat="server" /><br />
						<asp:CheckBox ID="LightboxCheckBox" CssClass="BodyTxt" Text="Add To Lightbox" runat="server" /><br />
					</td>
				</tr>
				</table>
			</div>
			<br />
			<span class="AppCtrlMrg BodyTxt Bold">2. Select the brand, market and timeframe for this report</span> 
			<span class="BodyTxt">|</span>
			<app:HelpHyperLink ID="HelpHyperLink2" HelpPage="11_Reports.htm#asset_activity_reports" Text="help" CssClass="BodyTxt" runat="server" /><br />
			<div class="AppCtrlMrg AppCtrlPanel W500">
				<table border="0" cellspacing="0" cellpadding="2" class="W500">
				<tr> 
					<td class="alignRight PanelTxt Bold">Brand</td>
					<td>
						<app:BrandDropDownList ID="BrandDropDownList" BlankText="All" CssClass="formInput W180" runat="server" />
					</td>
				</tr>
				<tr>
					<td class="alignRight PanelTxt Bold">Timeframe</td>
					<td><app:TimeFrameDropDownList ID="TimeFrameDropDownList1" OmitBlankItem="true" CssClass="formInput W180" runat="server" /></td>
				</tr>
				</table>
				<br />
				<div class="alignRight Clear W500">
					<app:CancelButton ID="ResetButton" Text="reset" CssClass="button" Prompt="" RedirectUrl="AssetActivity.aspx" runat="server" />
					<asp:Button ID="GenerateReportButton" Text="generate report" CssClass="button" OnClick="GenerateReportButton_Click" runat="server" />
				</div>
			</div>
		</div>
		<div class="Right AdmRightPanel"></div>
		
		<asp:Panel ID="InitialPanel" runat="server">
			<div class="AdmTblPanel Clear"><br /></div>
		</asp:Panel>
		
		<asp:Panel ID="ResultsHeaderPanel" Visible="false" runat="server">
			<div class="AdmTblPanel Clear">
				<div class="Left">
					<div class="AdmAppHdr">Report Summary</div>
					<br /> 
					<div class="AppResults">
						<span class="PanelTxt">total results: <span class="Bold"><%=RecordCount%></span> assets | </span>
						<asp:LinkButton ID="ViewAllAssetsLinkButton1" CssClass="PanelTxt" Text="view all assets" OnCommand="ToggleViewLinkButton_Command" CommandArgument="assetlist" runat="server" />
						<span class="PanelTxt">|</span>
						<asp:LinkButton ID="ViewActivitySummaryLinkButton1" CssClass="PanelTxt" Text="view activity summary" OnCommand="ToggleViewLinkButton_Command" CommandArgument="activitysummary" runat="server" />
						<span class="PanelTxt">|</span>
						<asp:LinkButton ID="DownloadCSVButton1" CssClass="PanelTxt" Text="download raw data as csv" OnClick="DownloadCSVButton_Click" runat="server" />
						<span class="PanelTxt">|</span> 
						<app:HelpHyperLink ID="HelpHyperLink3" HelpPage="11_Reports.htm#asset_activity_reports" Text="help" CssClass="BodyTxt" runat="server" />
					</div>
				</div>
				<div class="Right">
					<asp:Panel ID="TopPageControlsPanel" runat="server">
						<div class="AppPageView">
							<br />
							<span class="PanelTxt">show</span>
							<app:PageSizeDropDownList ID="PageSizeDropDownList1" OmitBlankItem="true" AutoPostBack="true" OnSelectedIndexChanged="PageSizeDropDownList1_SelectedIndexChanged" Mode="Admin" CssClass="formInput" runat="server" />
							<div class="AppPaging">
								<app:SimplePager ID="TopPager" OnCommand="SimplePager_OnCommand" runat="server" />
							</div> 
						</div>
					</asp:Panel>
				</div>
			</div>
		</asp:Panel>
		<asp:Panel ID="ResultsPanel" Visible="false" runat="server">
			<div class="Clear">
				<asp:Panel ID="AssetListPanel" runat="server">
					<br />
					<app:SortableRepeater ID="AssetRepeater" OnItemDataBound="AssetRepeater_OnItemDataBound" DefaultSortExpression="PopularityRank" DefaultSortAscending="true" ColumnCount="5" runat="server">
						<HeaderTemplate>
							<table width="910" border="0" cellspacing="0" cellpadding="2" class="TblOutline AppTblMrg AutoStripe">
							<tr>
								<td id="HeaderCell1" runat="server" nowrap="nowrap" class="Bold alignCentre"><asp:LinkButton ID="LinkButton1" CssClass="BodyTxt Bold" Text="Rank" OnCommand="SortListLinkButton_Command" CommandArgument="PopularityRank" runat="server" /></td>
								<td id="HeaderCell2" runat="server" nowrap="nowrap" class="Bold"><asp:LinkButton ID="LinkButton2" CssClass="BodyTxt Bold" Text="Ref #" OnCommand="SortListLinkButton_Command" CommandArgument="AssetId" runat="server" /></td>
								<td id="HeaderCell3" runat="server" nowrap="nowrap" class="Bold alignCentre"><asp:LinkButton ID="LinkButton3" CssClass="BodyTxt Bold" Text="Type" OnCommand="SortListLinkButton_Command" CommandArgument="AssetTypeName" runat="server" /></td>
								<td id="HeaderCell4" runat="server" nowrap="nowrap" class="Bold"><asp:LinkButton ID="LinkButton4" CssClass="BodyTxt Bold" Text="Upload User" OnCommand="SortListLinkButton_Command" CommandArgument="UploadedByUserName" runat="server" /></td>
								<td id="HeaderCell5" runat="server" nowrap="nowrap" class="Bold"><asp:LinkButton ID="LinkButton5" CssClass="BodyTxt Bold" Text="Title" OnCommand="SortListLinkButton_Command" CommandArgument="Title" runat="server" /></td>
								<td class="Bold">&nbsp;</td>
							</tr>
						</HeaderTemplate>
						<ItemTemplate>
							<tr> 
								<td class="BodyTxt Bold alignCentre" id="PopularityRankCell" runat="server"></td>
								<td class="BodyTxt"><asp:HyperLink ID="AssetIdHyperLink" CssClass="BodyTxt" runat="server" /></td>
								<td class="BodyTxt alignCentre" id="AssetTypeNameCell" runat="server"></td>
								<td class="BodyTxt" id="UploadedByUserNameCell" runat="server"></td>
								<td class="BodyTxt" id="TitleCell" runat="server"></td>
								<td class="BodyTxt alignCentre"><asp:HyperLink ID="StatsHyperLink" CssClass="BodyTxt" Text="stats" runat="server" /></td>
							</tr>
						</ItemTemplate>
						<FooterTemplate>
							</table>
						</FooterTemplate>
					</app:SortableRepeater>
					<asp:Panel ID="NoResultsPanel" Visible="false" runat="server">
						<div class="noResultsAdm AppCtrlMrg">
							<app:FeedbackLabel ID="NoResultsMessageLabel" MessageType="Negative" Header="no results found" Pinned="true" Visible="true" runat="server" />
						</div>
					</asp:Panel>
				</asp:Panel>
				<asp:Panel ID="ActivitySummaryPanel" Visible="false" runat="server">
					<div class="AppCtrlMrg AppCtrlPanel W500">
						<br />
						<div class="BodyTxt Bold alignCentre W500">
							Asset Activity Report For<br />
							<asp:Label ID="ChartBrandHeaderLabel" Text="[Brand]" runat="server" />
							in
							<asp:Label ID="ChartServiceHeaderLabel" Text="[Service market]" runat="server" />,
							<%=TimeFrameDropDownList1.SelectedItem.Text%>
						</div>
						<div class="alignCentre W500">
							[Chart]
						</div>
					</div>
					<br />
					<table class="W520 TblOutline AppTblMrg" border="0" cellspacing="0" cellpadding="2">
					<tr>
						<td class="alignCentre TblCell2 Bold PanelTxt">Activity</td>
						<td class="alignCentre TblCell2 Bold PanelTxt">Total</td>
					</tr>
					<tr> 
						<td class="alignCentre TblCell1 PanelTxt">Uploads</td>
						<td class="alignCentre TblCell1 PanelTxt" id="UploadsCell" runat="server">[Uploads]</td>
					</tr>
					<tr> 
						<td class="alignCentre TblCell1 PanelTxt">Downloads</td>
						<td class="alignCentre TblCell1 PanelTxt" id="DownloadsCell" runat="server">[Downloads]</td>
					</tr>
					<tr> 
						<td class="alignCentre TblCell1 PanelTxt">Order Requests</td>
						<td class="alignCentre TblCell1 PanelTxt" id="OrderRequestsCell" runat="server">[Order Requests]</td>
					</tr>
					<tr> 
						<td class="alignCentre TblCell1 PanelTxt">Views</td>
						<td class="alignCentre TblCell1 PanelTxt" id="ViewsCell" runat="server">[Views]</td>
					</tr>
					<tr> 
						<td class="alignCentre TblCell1 PanelTxt">Search Hits</td>
						<td class="alignCentre TblCell1 PanelTxt" id="SearchHitsCell" runat="server">[Search Hits]</td>
					</tr>
					<tr>
						<td class="alignCentre TblCell1 PanelTxt">Add To Cart</td>
						<td class="alignCentre TblCell1 PanelTxt" id="AddToCartCell" runat="server">[Add To Cart Cell]</td>
					</tr>
					<tr> 
						<td class="alignCentre TblCell1 PanelTxt">Add To Lightbox</td>
						<td class="alignCentre TblCell1 PanelTxt" id="AddToLightboxCell" runat="server">[Add To Lightbox Cell]</td>
					</tr>
					</table>
				</asp:Panel>
			</div>
			<br />
		</asp:Panel>
	</div>
	<div class="AdmAppFtr">
		<asp:Panel ID="InitialFooterPanel" runat="server">
			&nbsp;
		</asp:Panel>
		<asp:Panel ID="ResultsFooterPanel" Visible="false" runat="server">
			<div class="Left">
				<div class="AppResults PanelTxt">
					<span class="PanelTxt">total results: <span class="Bold"><%=RecordCount%></span> assets | </span>
					<asp:LinkButton ID="ViewAllAssetsLinkButton2" CssClass="PanelTxt" Text="view all assets" OnCommand="ToggleViewLinkButton_Command" CommandArgument="assetlist" runat="server" />
					<span class="PanelTxt">|</span>
					<asp:LinkButton ID="ViewActivitySummaryLinkButton2" CssClass="PanelTxt" Text="view activity summary" OnCommand="ToggleViewLinkButton_Command" CommandArgument="activitysummary" runat="server" />
					<span class="PanelTxt">|</span>
					<asp:LinkButton ID="DownloadCSVButton2" CssClass="PanelTxt" Text="download raw data as csv" OnClick="DownloadCSVButton_Click" runat="server" />
					<span class="PanelTxt">|</span> 
					<app:HelpHyperLink ID="HelpHyperLink4" Text="help" HelpPage="11_Reports.htm#asset_popularity_reports" CssClass="PanelTxt" runat="server" />
				</div>
			</div>
			<div class="Right">
				<asp:Panel ID="BottomPageControlsPanel" runat="server">
					<div class="AppPageView AppPaging">
						<app:SimplePager ID="BottomPager" OnCommand="SimplePager_OnCommand" runat="server" />
					</div>
				</asp:Panel>
			</div>
		</asp:Panel>
	</div>
</asp:Content>