<%@ Page Title="Asset Audit Trail [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="AssetAuditTrail.aspx.cs" Inherits="FocusOPEN.Website.Admin.AssetAuditTrail" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg"> 
		<div class="Left AdmLeftPanel"> 
			<div class="AdmAppHdr">Asset Audit Trail</div>
			<br />
			<br />
			<span class="AppCtrlMrg BodyTxt Bold">Select the user, event, IP address or date range for this report</span> 
			<span class="BodyTxt">|</span> <app:HelpHyperLink ID="HelpHyperLink1" HelpPage="11_Reports.htm#audit_trial" Text="help" CssClass="BodyTxt" runat="server" /><br />
			<div class="AppCtrlMrg AppCtrlPanel W500">
				<table border="0" cellspacing="0" cellpadding="2" class="W500">
				<tr> 
					<td class="alignRight PanelTxt Bold">Asset ID</td>
					<td><asp:TextBox ID="AssetIdTextBox" CssClass="formInput W180" runat="server" /></td>
				</tr>
				<tr> 
					<td class="alignRight PanelTxt Bold">User Email</td>
					<td><asp:TextBox ID="UserEmailTextBox" CssClass="formInput W180" runat="server" /></td>
				</tr>
				<tr> 
					<td class="alignRight PanelTxt Bold">Event</td>
					<td><app:AuditAssetActionDropDownList ID="AuditAssetActionDropDownList1" BlankText="All" CssClass="formInput W180" runat="server" /></td>
				</tr>
				<tr> 
					<td class="alignRight PanelTxt Bold">IP Address</td>
					<td><asp:TextBox ID="IpAddressTextBox" CssClass="formInput W180" runat="server" /></td>
				</tr>
				<tr> 
					<td class="alignRight PanelTxt Bold">Date Range</td>
					<td class="PanelTxt">
						<app:DatePicker ID="StartDatePicker" AllowDateBeforeToday="true" runat="server" />
						to
						<app:DatePicker ID="EndDatePicker" AllowDateBeforeToday="true" runat="server" />
					</td>
				</tr>
				</table>
				<br />
				<div class="alignRight Clear W500">
					<asp:Button ID="ResetButton" Text="reset" CssClass="button" OnClick="ResetButton_Click" runat="server" />
					<asp:Button ID="GenerateReportButton" Text="generate report" CssClass="button" OnClick="SearchButton_Click" runat="server" />
				</div>
			</div>
		</div>
		<div class="Right AdmRightPanel"></div>
		
		<asp:Panel ID="InitialPanel" runat="server">
			<div class="AdmTblPanel Clear noResultsAdm"><br /></div>
		</asp:Panel>
		
		<asp:Panel ID="ResultsPanel" Visible="false" runat="server">
			<div class="AdmTblPanel Clear">
				<div class="Left">
					<div class="AdmAppHdr">Audit Trail</div>
					<br />
					<div class="AppResults">
						<span class="PanelTxt">total results: <span class="Bold"><%=RecordCount%></span> entries</span>
					</div>
				</div>
				<div class="Right">
					<div class="AppPageView">
						<br />
						<span class="PanelTxt">show</span>
						<app:PageSizeDropDownList ID="PageSizeDropDownList1" OmitBlankItem="true" AutoPostBack="true" OnSelectedIndexChanged="PageSizeDropDownList1_SelectedIndexChanged" Mode="Admin" CssClass="formInput" runat="server" />
						<div class="AppPaging">
							<app:SimplePager ID="TopPager" OnCommand="SimplePager_OnCommand" runat="server" />
						</div> 
					</div>
				</div>
			</div>
			<div class="Clear">
				<br /> 
				<app:SortableRepeater ID="AuditAssetHistoryRepeater" OnItemDataBound="AuditAssetHistoryRepeater_OnItemDataBound" DefaultSortExpression="Date,AuditAssetHistoryId" DefaultSortAscending="false" ColumnCount="5" runat="server">
					<HeaderTemplate>
						<table width="910" border="0" cellspacing="0" cellpadding="2" class="TblOutline AppTblMrg AutoStripe">
						<tr>
							<td id="HeaderCell1" runat="server" nowrap="nowrap" class="Bold W50 alignCentre"><asp:LinkButton ID="LinkButton1" CssClass="BodyTxt Bold" Text="Asset ID" OnCommand="SortListLinkButton_Command" CommandArgument="AssetId" runat="server" /></td>
							<td id="HeaderCell2" runat="server" nowrap="nowrap" class="Bold W150"><asp:LinkButton ID="LinkButton2" CssClass="BodyTxt Bold" Text="User" OnCommand="SortListLinkButton_Command" CommandArgument="UserEmail" runat="server" /></td>
							<td id="HeaderCell3" runat="server" nowrap="nowrap" class="Bold W150"><asp:LinkButton ID="LinkButton3" CssClass="BodyTxt Bold" Text="Event" OnCommand="SortListLinkButton_Command" CommandArgument="Description" runat="server" /></td>
							<td id="HeaderCell4" runat="server" nowrap="nowrap" class="Bold W100 alignCentre"><asp:LinkButton ID="LinkButton4" CssClass="BodyTxt Bold" Text="IP Address" OnCommand="SortListLinkButton_Command" CommandArgument="IpAddress" runat="server" /></td>
							<td id="HeaderCell5" runat="server" nowrap="nowrap" class="Bold W150"><asp:LinkButton ID="LinkButton5" CssClass="BodyTxt Bold" Text="Date & Time" OnCommand="SortListLinkButton_Command" CommandArgument="Date,AuditAssetHistoryId" runat="server" /></td>
							<td id="HeaderCell6" runat="server" nowrap="nowrap" class="Bold"><asp:LinkButton ID="LinkButton6" CssClass="BodyTxt Bold" Text="Notes" OnCommand="SortListLinkButton_Command" CommandArgument="Notes" runat="server" /></td>
						</tr>
					</HeaderTemplate>
					<ItemTemplate>
						<tr>
							<td class="BodyTxt alignCentre"><asp:HyperLink ID="AssetHyperlink" CssClass="BodyTxt" runat="server" /></td>
							<td><app:EmailHyperLink ID="UserEmailHyperLink" CssClass="BodyTxt" runat="server" /></td>
							<td class="BodyTxt" nowrap="nowrap"><asp:Label ID="EventDescriptionLabel" runat="server" /></td>
							<td class="BodyTxt alignCentre" nowrap="nowrap"><asp:Label ID="IpAddressLabel" runat="server" /></td>
							<td class="BodyTxt" nowrap="nowrap"><asp:Label ID="DateTimeLabel" runat="server" /></td>
							<td class="BodyTxt"><asp:Label ID="NotesLabel" runat="server" /></td>
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
				
				<asp:PlaceHolder ID="ReturnToAssetListWrapper" runat="server">
					<br />
					<div class="AppResults">
						<asp:HyperLink ID="ReturnToAssetListHyperLink" CssClass="BodyTxt Bold" NavigateUrl="~/Admin/Assets/AssetList.aspx" Text="&laquo; Back To Asset List" runat="server" />
					</div>
				</asp:PlaceHolder>
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
					<span class="PanelTxt">total results <span class="Bold"><%=RecordCount %></span> entries</span>
				</div>
			</div>
			<div class="Right">
				<div class="AppPageView AppPaging">
					<app:SimplePager ID="BottomPager" OnCommand="SimplePager_OnCommand" runat="server" />
				</div>
			</div>
		</asp:Panel>
	</div>
</asp:Content>