<%@ Page Title="Asset Stats [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="AssetStats.aspx.cs" Inherits="FocusOPEN.Website.Admin.AssetStats" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg"> 
		<div class="Left W550"> 
			<div class="AdmAppHdr">Asset Statistics</div>
			<br />
			<br />
			<div class="AppCtrlMrg">
				<span class="BodyTxt Bold">View asset statistics below.</span> 
				<asp:HyperLink ID="AssetListHyperLink" CssClass="BodyTxt" NavigateUrl="~/Admin/Assets/AssetList.aspx" Text="view all assets" runat="server" />
				<span class="BodyTxt">|</span> 
				<a href="javascript:window.print()" class="BodyTxt">print this page</a>
				<span class="BodyTxt">|</span> 
				<app:HelpHyperLink ID="HelpHyperLink1" CssClass="BodyTxt" HelpPage="11_Reports.htm#asset_stats" Text="help" runat="server" />
			</div>
			<div class="AppCtrlMrg AppCtrlPanel W550">
				<div class="alignCentre">
					<app:AssetPreview ID="AssetPreview1" runat="server" />
					<br />
				</div>
				<br />
				<table border="0" cellspacing="0" cellpadding="2" class="W550 TblOutline">
				<tr>
					<td class="alignRight Bold PanelTxt TblCell2 W225 Top">Asset Reference</td>
					<td class="PanelTxt TblCell1"><asp:Label ID="AssetReferenceLabel" runat="server" /></td>
				</tr>
				<tr> 
					<td class="alignRight Bold PanelTxt TblCell2 W225 Top">Type</td>
					<td class="PanelTxt TblCell1"><asp:Label ID="AssetTypeLabel" runat="server" /></td>
				</tr>
				<tr> 
					<td class="alignRight Bold PanelTxt TblCell2 W225 Top">Title/Project</td>
					<td class="PanelTxt TblCell1"><asp:Label ID="TitleLabel" runat="server" /></td>
				</tr>
				<tr> 
					<td class="alignRight Bold PanelTxt TblCell2 W225 Top">Brand</td>
					<td class="PanelTxt TblCell1"><asp:Label ID="BrandLabel" runat="server" /></td>
				</tr>
				<tr> 
					<td class="alignRight Bold PanelTxt TblCell2 W225 Top">Restricted Download</td>
					<td class="PanelTxt TblCell1"><asp:Label ID="RestrictedDownloadLabel" runat="server" /></td>
				</tr>
				<tr> 
					<td class="alignRight Bold PanelTxt TblCell2 W225 Top">Usage Restrictions</td>
					<td class="PanelTxt TblCell1"><asp:Label ID="UsageRestrictionsLabel" runat="server" /></td>
				</tr>
				<tr> 
					<td class="alignRight Bold PanelTxt TblCell2 W225 Top">Upload User</td>
					<td class="TblCell1"><asp:HyperLink ID="UploadUserHyperLink" CssClass="PanelTxt" runat="server" /></td>
				</tr>
				<tr> 
					<td class="alignRight Bold PanelTxt TblCell2 W225 Top">Date Uploaded</td>
					<td class="TblCell1 PanelTxt"><asp:Label ID="DateUploadedLabel" runat="server" /></td>
				</tr>
				<tr> 
					<td class="alignRight Bold PanelTxt TblCell2 W225 Top">Date Published</td>
					<td class="TblCell1 PanelTxt"><asp:Label ID="DatePublishedLabel" runat="server" /></td>
				</tr>
				<tr>
					<td class="alignRight Bold PanelTxt TblCell2 W225 Top">Date Last Edited </td>
					<td class="TblCell1 PanelTxt"><asp:Label ID="DateLastEditedLabel" runat="server" /></td>
				</tr>
				</table>
				<br />
				<div class="CatHozLine"></div>
				<table border="0" cellspacing="0" cellpadding="2" class="W550">
				<tr> 
					<td class="alignRight PanelTxt W225 Top">Total Search Hits</td>
					<td class="PanelTxt Bold "><asp:Label ID="TotalNumberOfSearchHitsLabel" runat="server" /></td>
				</tr>
				<tr> 
					<td class="alignRight PanelTxt W225 Top">Total No. Of Views</td>
					<td class="Bold PanelTxt"><asp:Label ID="TotalNumberOfViewsLabel" runat="server" /></td>
				</tr>
				<tr> 
					<td class="alignRight PanelTxt W225 Top">Total No. Added To Lightbox</td>
					<td class="Bold PanelTxt"><asp:Label ID="TotalNumberAddedToLightboxLabel" runat="server" /></td>
				</tr>
				<tr> 
					<td class="alignRight PanelTxt W225 Top">Total No. Added To Cart</td>
					<td class="Bold PanelTxt"><asp:Label ID="TotalNumberAddedToCartLabel" runat="server" /></td>
				</tr>
				<tr> 
					<td class="alignRight PanelTxt W225 Top">Total No. Of Order Requests</td>
					<td class="Bold PanelTxt"><asp:Label ID="TotalNumberOfOrderRequestsLabel" runat="server" /></td>
				</tr>
				<tr> 
					<td class="alignRight PanelTxt W225 Top">Total No. Of Downloads</td>
					<td class="Bold PanelTxt"><asp:Label ID="TotalNumberOfDownloadsLabel" runat="server" /></td>
				</tr>
				<tr>
					<td class="alignRight PanelTxt W225 Top">Popularity Based On Downloads</td>
					<td class="Bold PanelTxt"><asp:Label ID="PopularityBasedOnDownloadsLabel" Text="xx/[Total] assets" runat="server" /></td>
				</tr>
				<tr>
					<td class="alignRight PanelTxt W225 Bold">Generate Chart For Timeframe</td>
					<td class="Bold PanelTxt"><app:TimeFrameDropDownList ID="TimeFrameDropDownList1" CssClass="formInput W180" OnSelectedIndexChanged="TimeFrameDropDownList1_SelectedIndexChanged" AutoPostBack="true" runat="server" /></td>
				</tr>
				</table>
				<br />
				<div class="CatHozLine"></div>
				<asp:Panel ID="TimeBasedStatisticsPanel" runat="server">
					<div class="BodyTxt Bold alignCentre W500">Statistics For Asset Ref# <%=Request.QueryString.Get("AssetId") %>, <%=TimeFrameDropDownList1.SelectedItem.Text%></div>  
					<div class="alignCentre">
						<asp:Literal ID="ChartLiteral" Text="[Chart]" runat="server" />
					</div>
					<br />
					
					<aspajax:UpdatePanel ID="AssetStatsUpdatePanel" runat="server">
						<ContentTemplate>
							<div class="AppCtrlMrg alignCentre">
								<asp:LinkButton ID="ActivitySummaryLinkButton" CssClass="BodyTxt" Text="activity summary" OnCommand="DisplayPanel_Click" CommandArgument="ActivitySummary" runat="server" />
								<span class="BodyTxt">|</span> 
								<asp:LinkButton ID="TopSearchTermsLinkButton" CssClass="BodyTxt" Text="top 10 search terms" OnCommand="DisplayPanel_Click" CommandArgument="TopSearchTerms" runat="server" />
								<span class="BodyTxt">|</span> 
								<asp:LinkButton ID="LastUsersToDownloadLinkButton" CssClass="BodyTxt" Text="last 10 users to download" OnCommand="DisplayPanel_Click" CommandArgument="LastUsersToDownload" runat="server" />
								<app:EnhancedHyperLink ID="ViewAuditTrailHyperlink" ShowLeftDivider="true" ShowRightDivider="false" Prefix="" Suffix="" CssClass="BodyTxt" Text="view audit trail" runat="server" />
							</div>
							<br />
							<div class="CatHozLine"></div>
							<asp:Panel ID="PanelWrapper" runat="server">
								<asp:Panel ID="ActivitySummaryPanel" Visible="false" runat="server">
									<div class="BodyTxt Bold alignCentre">Activity Summary</div><br />
									<table class="W550 TblOutline" border="0" cellspacing="0" cellpadding="2">
									<tr> 
										<td class="alignCentre TblCell2 Bold PanelTxt W260">Activity</td>
										<td	class="alignCentre TblCell2 Bold PanelTxt">Total In Selected Timeframe</td>
									</tr>
									<tr> 
										<td class="alignCentre TblCell1 PanelTxt">Downloads</td>
										<td class="alignCentre TblCell1 PanelTxt" id="DownloadsCell" runat="server">9/253 (4%)</td>
									</tr>
									<tr> 
										<td class="alignCentre TblCell1 PanelTxt">Order Requests</td>
										<td class="alignCentre TblCell1 PanelTxt" id="OrderRequestsCell" runat="server">18/253 (8%)</td>
									</tr>
									<tr> 
										<td class="alignCentre TblCell1 PanelTxt">Views</td>
										<td class="alignCentre TblCell1 PanelTxt" id="ViewsCell" runat="server">10/40 (25%)</td>
									</tr>
									<tr> 
										<td class="alignCentre TblCell1 PanelTxt">Search Hits</td>
										<td class="alignCentre TblCell1 PanelTxt" id="SearchHitsCell" runat="server">30/300 (10%)</td>
									</tr>
									<tr> 
										<td class="alignCentre TblCell1 PanelTxt">Add To Cart</td>
										<td class="alignCentre TblCell1 PanelTxt" id="AddToCartCell" runat="server">2/100 (2%)</td>
									</tr>
									<tr> 
										<td class="alignCentre TblCell1 PanelTxt">Add To Lightbox</td>
										<td class="alignCentre TblCell1 PanelTxt" id="AddToLightboxCell" runat="server">12/260 (4%)</td>
									</tr>
									</table>
								</asp:Panel>
								<asp:Panel ID="TopSearchTermsPanel" Visible="false" runat="server">
									<div class="BodyTxt Bold alignCentre">Top 10 Search Terms</div><br />
									<asp:Panel ID="NoSearchTermsPanel" Visible="false" runat="server">
										<div class="alignCentre"><span class="FbkNegTxt">This asset has not been returned in any searches</span></div>
									</asp:Panel>
									<asp:Repeater ID="TopSearchTermsRepeater" runat="server">
										<HeaderTemplate>
											<table class="W550 TblOutline AutoStripe" border="0" cellspacing="0" cellpadding="2">
											<tr> 
												<td class="alignCentre Bold PanelTxt W260">Rank</td>
												<td	class="alignCentre Bold PanelTxt">Search Term</td>
												<td	class="alignCentre Bold PanelTxt">Count</td>
											</tr>
										</HeaderTemplate>
										<FooterTemplate>
											</table>
										</FooterTemplate>
										<ItemTemplate>
											<tr> 
												<td class="alignCentre PanelTxt"><%# DataBinder.Eval (Container.DataItem, "Rank") %></td>
												<td class="alignCentre PanelTxt"><%# DataBinder.Eval (Container.DataItem, "SearchKeyword") %>&nbsp;</td>
												<td class="alignCentre PanelTxt"><%# DataBinder.Eval (Container.DataItem, "Count") %></td>
											</tr>
										</ItemTemplate>
									</asp:Repeater>
								</asp:Panel>
								<asp:Panel ID="LastUsersToDownloadPanel" Visible="false" runat="server">
									<div class="BodyTxt Bold alignCentre">Last 10 Users To Download</div><br />
									<asp:Panel ID="NoDownloadsPanel" Visible="false" runat="server">
										<div class="alignCentre"><span class="FbkNegTxt">No users have downloaded this asset</span></div>
									</asp:Panel>
									<asp:Repeater ID="LastUsersToDownloadRepeater" OnItemDataBound="LastUsersToDownloadRepeater_ItemDataBound" runat="server">
										<HeaderTemplate>
											<table class="W550 TblOutline AutoStripe" border="0" cellspacing="0" cellpadding="2">
											<tr> 
												<td class="alignCentre Bold PanelTxt">Rank</td>
												<td	class="alignCentre Bold PanelTxt">Email</td>
												<td	class="alignCentre Bold PanelTxt">Date</td>
											</tr>
										</HeaderTemplate>
										<FooterTemplate>
											</table>
										</FooterTemplate>
										<ItemTemplate>
											<tr> 
												<td class="alignCentre PanelTxt"><%# DataBinder.Eval (Container.DataItem, "Rank") %></td>
												<td class="alignCentre PanelTxt"><app:EmailHyperLink ID="EmailHyperLink1" CssClass="PanelTxt" runat="server" /></td>
												<td class="alignCentre PanelTxt"><asp:Label ID="DateLabel" runat="server" /></td>
											</tr>
										</ItemTemplate>
									</asp:Repeater>
								</asp:Panel>
							</asp:Panel>
						</ContentTemplate>
					</aspajax:UpdatePanel>
				</asp:Panel>
			</div>
		</div>
		<div class="Right AdmRightPanel"></div>
		<div class="Clear"><br /></div>
	</div>
	<br />
</asp:Content>