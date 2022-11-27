<%@ Page Title="Asset Popularity Report [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="AssetPopularity.aspx.cs" Inherits="FocusOPEN.Website.Admin.AssetPopularity" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg"> 
		<div class="Left AdmLeftPanel"> 
			<div class="AdmAppHdr">Asset Popularity Report</div>
			<br />
			<br />
			<span class="AppCtrlMrg BodyTxt Bold">Select the asset type, brand, market and timeframe for this report</span> 
			<span class="BodyTxt">|</span>
			<app:HelpHyperLink ID="HelpHyperLink1" Text="help" CssClass="BodyTxt" HelpPage="11_Reports.htm#asset_popularity_reports" runat="server" /><br />
			<div class="AppCtrlMrg AppCtrlPanel W500">
				<table border="0" cellspacing="0" cellpadding="2" class="W500">
				<tr> 
					<td class="alignRight PanelTxt Bold">Type</td>
					<td><app:AssetTypeDropDownList ID="AssetTypeDropDownList1" BlankText="All" CssClass="formInput W125" runat="server" /></td>
				</tr>
				<tr> 
					<td class="alignRight PanelTxt Bold">Brand</td>
					<td><app:BrandDropDownList ID="BrandDropDownList" BlankText="All" CssClass="formInput W180" runat="server" /></td>
				</tr>
				<tr> 
					<td class="alignRight PanelTxt Bold">Timeframe</td>
					<td><app:TimeFrameDropDownList ID="TimeFrameDropDownList1" OmitBlankItem="true" CssClass="formInput W180" runat="server" /></td>
				</tr>
				</table>
				<br />
				<div class="alignRight Clear W500">
					<app:CancelButton ID="ResetButton" Text="reset" CssClass="button" Prompt="" RedirectUrl="AssetPopularity.aspx" runat="server" />
					<asp:Button ID="GenerateReportButton" Text="generate report" CssClass="button" OnClick="GenerateReportButton_Click" runat="server" />
				</div>
			</div>
		</div>
		<div class="Right AdmRightPanel"></div>
		
		<asp:Panel ID="InitialPanel" runat="server">
			<div class="AdmTblPanel Clear noResultsAdm"><br /></div>
		</asp:Panel>
		
		<asp:Panel ID="ResultsPanel" Visible="false" runat="server">
			<br />
			<div class="AdmTblPanel Clear">
				<div class="Left">
					<div class="AdmAppHdr">Report Summary</div>
					<br />
					<div class="AppResults">
						<span class="PanelTxt">top <span class="Bold"><%=TopRecordsDropDownList1.SelectedValue%></span> assets | </span>
						<asp:LinkButton ID="DownloadCSVButton1" CssClass="PanelTxt" Text="download raw data as csv" OnClick="DownloadCSVButton_Click" runat="server" />
					<span class="PanelTxt">|</span> 
					<app:HelpHyperLink ID="HelpHyperLink2" Text="help" HelpPage="11_Reports.htm#asset_popularity_reports" CssClass="PanelTxt" runat="server" />
					</div>
				</div>
				<div class="Right">
					<div class="AppPageView">
						<br />
						<span class="PanelTxt">show</span>
						<app:TopRecordsDropDownList ID="TopRecordsDropDownList1" OmitBlankItem="true" AutoPostBack="true" OnSelectedIndexChanged="TopRecordsDropDownList1_SelectedIndexChanged" CssClass="formInput" runat="server" />
					</div>
				</div>
				<div class="Clear">
					<br />
						<app:SortableRepeater ID="AssetRepeater" OnItemDataBound="AssetRepeater_OnItemDataBound" DefaultSortExpression="PopularityRankInTimeFrame" DefaultSortAscending="true" ColumnCount="6" runat="server">
							<HeaderTemplate>
								<table width="910" border="0" cellspacing="0" cellpadding="2" class="TblOutline AppTblMrg AutoStripe">
								<tr>
									<td id="HeaderCell1" runat="server" nowrap="nowrap" class="Bold alignCentre"><asp:LinkButton ID="LinkButton1" CssClass="BodyTxt Bold" Text="Rank" OnCommand="SortListLinkButton_Command" CommandArgument="PopularityRankInTimeFrame" runat="server" /></td>
									<td id="HeaderCell2" runat="server" nowrap="nowrap" class="Bold"><asp:LinkButton ID="LinkButton2" CssClass="BodyTxt Bold" Text="Ref #" OnCommand="SortListLinkButton_Command" CommandArgument="AssetId" runat="server" /></td>
									<td id="HeaderCell3" runat="server" nowrap="nowrap" class="Bold alignCentre"><asp:LinkButton ID="LinkButton3" CssClass="BodyTxt Bold" Text="Type" OnCommand="SortListLinkButton_Command" CommandArgument="AssetTypeName" runat="server" /></td>
									<td id="HeaderCell4" runat="server" nowrap="nowrap" class="Bold"><asp:LinkButton ID="LinkButton4" CssClass="BodyTxt Bold" Text="Upload User" OnCommand="SortListLinkButton_Command" CommandArgument="UploadedByUserName" runat="server" /></td>
									<td id="HeaderCell5" runat="server" nowrap="nowrap" class="Bold"><asp:LinkButton ID="LinkButton5" CssClass="BodyTxt Bold" Text="Title" OnCommand="SortListLinkButton_Command" CommandArgument="Title" runat="server" /></td>
									<td id="HeaderCell6" runat="server" nowrap="nowrap" class="Bold alignCentre"><asp:LinkButton ID="LinkButton6" CssClass="BodyTxt Bold" Text="Downloads" OnCommand="SortListLinkButton_Command" CommandArgument="DownloadCountInTimeFrame" runat="server" /></td>
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
									<td class="BodyTxt alignCentre" id="DownloadCountCell" runat="server"></td>
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
				</div>
				<br />
			</div>
		</asp:Panel>
	</div>
	<div class="AdmAppFtr">
		<asp:Panel ID="InitialFooterPanel" runat="server">
			&nbsp;
		</asp:Panel>
		<asp:Panel ID="ResultsFooterPanel" Visible="false" runat="server">
			<div class="Left">
				<div class="AppResults PanelTxt">
					<span class="PanelTxt">top: <span class="Bold"><%=TopRecordsDropDownList1.SelectedValue%></span> assets | </span>
					<asp:LinkButton ID="DownloadCSVButton2" CssClass="PanelTxt" Text="download raw data as csv" OnClick="DownloadCSVButton_Click" runat="server" />
					<span class="PanelTxt">|</span> 
					<app:HelpHyperLink ID="HelpHyperLink3" Text="help" HelpPage="11_Reports.htm#asset_popularity_reports" CssClass="PanelTxt" runat="server" />
				</div>
			</div>
		</asp:Panel>
	</div>
</asp:Content>