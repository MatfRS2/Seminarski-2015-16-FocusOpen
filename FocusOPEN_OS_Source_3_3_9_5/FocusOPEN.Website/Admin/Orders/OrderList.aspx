<%@ Page Title="Manage Orders [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="OrderList.aspx.cs" Inherits="FocusOPEN.Website.Admin.OrderList" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg"> 
		<div class="Left AdmLeftPanel"> 
			<div class="AdmAppHdr">View Order List</div>
			<br />
			<br />
			<span class="AppCtrlMrg BodyTxt Bold">Find an order to review</span> <span class="BodyTxt">|</span> 
			<app:HelpHyperLink ID="HelpHyperLink1" HelpPage="7_Manage_Orders.htm#finding_orders" CssClass="BodyTxt" Text="help" runat="server" /><br />
			<div class="AppCtrlMrg AppCtrlPanel W470">
				<table border="0" cellspacing="0" cellpadding="2">
				<tr> 
					<td class="alignRight PanelTxt Bold Top W80">Order Ref #</td>
					<td class="BodyTxt">
						<asp:TextBox ID="GeneralKeywordTextBox" CssClass="formInput W200" runat="server" /><br />
						Enter a user's email above to see all orders for that user
					</td>
				</tr>
				
				<asp:PlaceHolder ID="BrandSelectorPanel" runat="server">
					<tr>
						<td class="alignRight PanelTxt Bold">Brand</td>
						<td><app:BrandDropDownList ID="BrandIdDropdown" OnSelectedIndexChanged="BrandIdDropdown_SelectedIndexChanged" CssClass="formInput W180" BlankText="All" AutoPostBack="true" runat="server" /></td>
					</tr>
				</asp:PlaceHolder>
				
				<app:SecuredPlaceHolder ID="ApplicantSelectorPanel" AllowedRoles="BrandAdministrator,SuperAdministrator" runat="server">
					<tr>
						<td class="alignRight PanelTxt Bold">Applicant</td>
						<td><asp:DropDownList ID="ApplicantDropDownList" CssClass="formInput W200" runat="server" /></td>
					</tr>
				</app:SecuredPlaceHolder>
				
				</table>
				
				<div class="BodyTxt AdmSubCkMrg">
					<asp:CheckBox ID="PendingOrdersOnlyCheckBox" runat="server" />
					<span class="BaseLine">show pending orders only</span>
				</div>
				<div class="alignRight">
					<asp:Button ID="ResetButton" Text="reset" CssClass="button" OnClick="ResetButton_Click" runat="server" />
					<asp:Button ID="SearchButton" Text="search" CssClass="button" OnClick="SearchButton_Click" runat="server" />
				</div>

			</div>
		</div>
		<div class="Right AdmRightPanel"></div>
		<div class="AdmTblPanel Clear">
			<div class="Left">
				<br /><br />
				<div class="AppResults PanelTxt">total results: <span class="Bold"><%=RecordCount%></span> orders [displaying page <%=CurrentPage%> of <%=TotalPages%>]</div>
			</div>
			<div class="Right">
				<div class="AppPageView">
					<span class="PanelTxt">show</span>
					<app:PageSizeDropDownList ID="PageSizeDropDownList1" OmitBlankItem="true" AutoPostBack="true" OnSelectedIndexChanged="PageSizeDropDownList1_SelectedIndexChanged" Mode="Admin" CssClass="formInput" runat="server" />
					<div class="AppPaging">
						<app:SimplePager ID="TopPager" OnCommand="SimplePager_OnCommand" runat="server" />
					</div>
				</div>
			</div>
			<div class="Clear">
				<br />
				<app:SortableRepeater ID="OrdersRepeater" DefaultSortExpression="OrderId" DefaultSortAscending="false" ColumnCount="7" OnItemDataBound="OrdersRepeater_ItemDataBound" runat="server">
					<HeaderTemplate>
						<table width="910" border="0" cellspacing="0" cellpadding="2" class="TblOutline AppTblMrg AutoStripe">
						<tr> 
							<td id="HeaderCell1" runat="server" class="Bold alignCentre"><asp:LinkButton ID="LinkButton1" CssClass="BodyTxt Bold" Text="Order Ref #" CommandArgument="OrderId" OnCommand="SortListLinkButton_Command"  runat="server" /></td>
							<td id="HeaderCell2" runat="server" class="Bold alignCentre"><asp:LinkButton ID="LinkButton2" CssClass="BodyTxt Bold" Text="No. of Assets" CommandArgument="AssetCount" OnCommand="SortListLinkButton_Command"  runat="server" /></td>
							<td id="HeaderCell3" runat="server" class="Bold"><asp:LinkButton ID="LinkButton3" CssClass="BodyTxt Bold" Text="Applicant" CommandArgument="UserName" OnCommand="SortListLinkButton_Command"  runat="server" /></td>
							<td id="HeaderCell4" runat="server" class="Bold"><asp:LinkButton ID="LinkButton4" CssClass="BodyTxt Bold" Text="Brand" CommandArgument="UserPrimaryBrandName" OnCommand="SortListLinkButton_Command"  runat="server" /></td>
							<td id="HeaderCell5" runat="server" class="Bold alignCentre"><asp:LinkButton ID="LinkButton5" CssClass="BodyTxt Bold" Text="Pending" CommandArgument="IsPending" OnCommand="SortListLinkButton_Command"  runat="server" /></td>
							<td id="HeaderCell6" runat="server" class="Bold alignCentre"><asp:LinkButton ID="LinkButton6" CssClass="BodyTxt Bold" Text="Request Date" CommandArgument="OrderDate" OnCommand="SortListLinkButton_Command"  runat="server" /></td>
							<td id="HeaderCell7" runat="server" class="Bold alignCentre"><asp:LinkButton ID="LinkButton7" CssClass="BodyTxt Bold" Text="Request Deadline" CommandArgument="DeadlineDate" OnCommand="SortListLinkButton_Command"  runat="server" /></td>
						</tr>
					</HeaderTemplate>
					<ItemTemplate>
						<tr> 
							<td class="alignCentre BodyTxt"><asp:HyperLink ID="OrderIdHyperLink" CssClass="BodyTxt" runat="server" /></td>
							<td class="alignCentre BodyTxt"><asp:Label ID="AssetCountLabel" runat="server" /></td>
							<td class="BodyTxt"><asp:Label ID="UserNameLabel" runat="server" /></td>
							<td class="BodyTxt"><asp:Label ID="UserBrandLabel" runat="server" /></td>
							<td class="alignCentre BodyTxt"><asp:Label ID="IsPendingLabel" runat="server" /></td>
							<td class="alignCentre BodyTxt"><asp:Label ID="RequestDateLabel" runat="server" /></td>
							<td class="alignCentre BodyTxt"><asp:Label ID="DeadlineDateLabel" runat="server" /></td>
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
				<br />
			</div>
		</div>
	</div>
	<div class="AdmAppFtr">
		<div class="Left">
			<div class="AppResults PanelTxt">total results: <span class="Bold"><%=RecordCount%></span> orders [displaying page <%=CurrentPage%> of <%=TotalPages%>]</div>
		</div>
		<div class="Right">
			<div class="AppPageView AppPaging">
				<app:SimplePager ID="BottomPager" OnCommand="SimplePager_OnCommand" runat="server" />
			</div>
		</div>
	</div>
</asp:Content>