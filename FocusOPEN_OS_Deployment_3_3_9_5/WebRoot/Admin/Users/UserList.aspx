<%@ Page Title="Manage Users [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="FocusOPEN.Website.Admin.UserList" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

	<div class="AdmBkg"> 
		<div class="Left AdmLeftPanel"> 
			<div class="AdmAppHdr">Manage Users</div>
			<br />
			<br />
			<span class="AppCtrlMrg BodyTxt Bold">
				Find a user record to view or edit
				|
				<a href="UserForm.aspx" class="BodyTxt">Add a new user</a>
			</span> 
			<span class="BodyTxt">|</span>
			<app:HelpHyperLink ID="HelpHyperLink1" HelpPage="6_User_Admin.htm#manage_users" CssClass="BodyTxt" Text="help" runat="server" /><br />
			<div class="AppCtrlMrg AppCtrlPanel W470">
				<table width="455" border="0" cellspacing="0" cellpadding="2">
				<tr>
					<td class="W80 alignRight PanelTxt Bold">User Email</td>
					<td><asp:TextBox ID="EmailTextBox" CssClass="formInput W180" runat="Server" /></td>
				</tr>
				<tr id="BrandRow" runat="server">
					<td class="W80 alignRight PanelTxt Bold">Brand</td>
					<td><app:BrandDropDownList ID="BrandIdDropdown" ShowNoneOption="true" CssClass="formInput W180" OmitBlankItem="false" BlankText="All" runat="server" /></td>
				</tr>
				</table>
				<div class="BodyTxt AdmSubCkMrg">
					<asp:CheckBox ID="ExpiredAndSuspendedAccountsOnlyCheckBox" runat="server" />
					<span class="BaseLine">show expired and suspended accounts only</span>
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
				<div class="AppResults PanelTxt">total results: <span class="Bold"><%=RecordCount%></span> users [displaying page <%=CurrentPage%> of <%=TotalPages%>]</div>
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
				<app:SortableRepeater ID="UsersRepeater" DefaultSortExpression="FullName" DefaultSortAscending="true" ColumnCount="8" OnItemDataBound="UsersRepeater_ItemDataBound" runat="server">
					<HeaderTemplate>
						<table width="910" border="0" cellspacing="0" cellpadding="2" class="TblOutline AppTblMrg AutoStripe">
						<tr>
							<td id="HeaderCell1" runat="server" class="Bold"><asp:LinkButton ID="LinkButton1" CssClass="BodyTxt Bold" Text="User Name" OnCommand="SortListLinkButton_Command" CommandArgument="FullName" runat="server" /></td>
							<td id="HeaderCell2" runat="server" class="Bold"><asp:LinkButton ID="LinkButton2" CssClass="BodyTxt Bold" Text="User Email" OnCommand="SortListLinkButton_Command" CommandArgument="Email" runat="server" /></td>
							<td id="HeaderCell3" runat="server" class="Bold"><asp:LinkButton ID="LinkButton3" CssClass="BodyTxt Bold" Text="User Type" OnCommand="SortListLinkButton_Command" CommandArgument="UserRoleName" runat="server" /></td>
							<td id="HeaderCell4" runat="server" class="Bold"><asp:LinkButton ID="LinkButton4" CssClass="BodyTxt Bold" Text="Company" OnCommand="SortListLinkButton_Command" CommandArgument="CompanyName" runat="server" /></td>
							<td id="HeaderCell5" runat="server" class="Bold"><asp:LinkButton ID="LinkButton5" CssClass="BodyTxt Bold" Text="Brand" OnCommand="SortListLinkButton_Command" CommandArgument="BrandName" runat="server" /></td>
							<td id="HeaderCell6" runat="server" class="Bold alignCentre"><asp:LinkButton ID="LinkButton6" CssClass="BodyTxt Bold" Text="Expired" OnCommand="SortListLinkButton_Command" CommandArgument="IsAccountExpired" runat="server" /></td>
							<td id="HeaderCell7" runat="server" class="Bold alignCentre"><asp:LinkButton ID="LinkButton7" CssClass="BodyTxt Bold" Text="Suspended" OnCommand="SortListLinkButton_Command" CommandArgument="IsSuspended" runat="server" /></td>
							<td id="HeaderCell8" runat="server" class="Bold"><asp:LinkButton ID="LinkButton8" CssClass="BodyTxt Bold" Text="Last Login" OnCommand="SortListLinkButton_Command" CommandArgument="LastLoginDate" runat="server" /></td>
							<td class="Bold">&nbsp;</td>
						</tr>
					</HeaderTemplate>
					<ItemTemplate>
						<tr>
							<td><asp:HyperLink ID="EditUserHyperLink" Text="[User Name]" CssClass="BodyTxt" runat="server" /></td>
							<td><a href="mailto:<%# DataBinder.Eval (Container.DataItem, "Email") %>" class="BodyTxt"><%# DataBinder.Eval (Container.DataItem, "Email") %></a></td>
							<td class="BodyTxt"><asp:Label ID="UserTypeNameLabel" runat="server" /></td>
							<td class="BodyTxt"><%# DataBinder.Eval (Container.DataItem, "CompanyName") %></td>
							<td id="BrandNameRow" runat="server" class="BodyTxt"><asp:Label ID="BrandNameLabel" runat="server" /></td>
							<td class="alignCentre BodyTxt"><asp:Label ID="IsExpiredLabel" runat="server" /></td>
							<td class="alignCentre BodyTxt"><asp:Label ID="IsSuspendedLabel" runat="server" /></td>
							<td class="BodyTxt"><asp:Label ID="LastLoginDateLabel" runat="server" /></td>
							<td id="StatsEditCell" runat="server">
								<app:EnhancedHyperLink ID="StatsHyperLink" ShowRightDivider="true" Text="stats" CssClass="BodyTxt" runat="server" />
								<a href="UserForm.aspx?userId=<%# DataBinder.Eval (Container.DataItem, "UserId") %>" class="BodyTxt">edit</a>
							</td>
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
			<div class="AppResults PanelTxt">total results: <span class="Bold"><%=RecordCount%></span> users [displaying page <%=CurrentPage%> of <%=TotalPages%>]</div>
		</div>
		<div class="Right">
			<div class="AppPageView AppPaging">
				<app:SimplePager ID="BottomPager" OnCommand="SimplePager_OnCommand" runat="server" />
			</div>
		</div>
	</div>
</asp:Content>