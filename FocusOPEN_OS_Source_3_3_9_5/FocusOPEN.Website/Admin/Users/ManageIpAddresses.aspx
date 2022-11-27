<%@ Page Title="Manage IP Addresses [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="ManageIpAddresses.aspx.cs" Inherits="FocusOPEN.Website.Admin.ManageIpAddresses" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg"> 
		<div class="Left AdmLeftPanel"> 
			<div class="AdmAppHdr">Manage IP Addresses</div>
			<br />
			<app:FeedbackLabel ID="MessageLabel1" UseContainer="true" runat="server" />
			<br />
			<div class="AppCtrlMrg"><span class="Bold BodyTxt">Click on 'edit' to edit an IP address, or 'delete' to remove an IP address.<br />New IP addresses can be added at the end of the list | </span><app:HelpHyperLink ID="HelpHyperLink1" HelpPage="6_User_Admin.htm#manage_ip_addresses" CssClass="BodyTxt" Text="help" runat="server" /><br /></div>
		</div>
		<div class="Right AdmRightPanel"></div>
		<div class="AdmTblPanel Clear">
			<div class="Left">
				<br /><br />
				<div class="AppResults PanelTxt">total results: <span class="Bold"><%=RecordCount%></span> IP Addresses [displaying page <%=CurrentPage%> of <%=TotalPages%>]</div>
			</div>
			<div class="Right">
				<div class="AppPageView">
					<span class="PanelTxt">show</span>
					<app:PageSizeDropDownList ID="PageSizeDropDownList1" Mode="admin" CssClass="formInput" OmitBlankItem="true" runat="server" />
					<div class="AppPaging">
						<app:SimplePager ID="TopPager" OnCommand="SimplePager_OnCommand" runat="server" />
					</div>
				</div>
			</div>
			<div class="Clear">
				<br />
				<app:SortableDataGrid ID="EntityDataGrid" DefaultSortExpression="IpAddressValue" DefaultSortAscending="true" AllowDelete="true" runat="server">
					<HeaderStyle Height="25px" />
					<ItemStyle Height="25px" />
					<Columns>
						<asp:TemplateColumn HeaderText="IP Address" SortExpression="IpAddressValue">
							<ItemTemplate>
								<asp:Label ID="EntityLabel" CssClass="BodyTxt" runat="server" />
							</ItemTemplate>
							<EditItemTemplate>
								<asp:TextBox ID="EntityTextBox" CssClass="formInput W225" runat="server" />
							</EditItemTemplate>
							<FooterTemplate>
								<span class="AdmHdrTxt Bold">Add IP Address:</span>
								<asp:TextBox ID="NewEntityTextBox" CssClass="formInput W225" runat="server" />
								<asp:Button ID="AddEntityButton" Text="add" CommandName="add" CssClass="button W50" runat="server" />
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:EditCommandColumn ButtonType="LinkButton" EditText="edit" UpdateText="update" CancelText="cancel" />
					</Columns>
				</app:SortableDataGrid>
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
			<div class="AppResults PanelTxt">total results: <span class="Bold"><%=RecordCount%></span> IP addresses [displaying page <%=CurrentPage%> of <%=TotalPages%>]</div>
		</div>
		<div class="Right">
			<app:SimplePager ID="BottomPager" OnCommand="SimplePager_OnCommand" runat="server" />
		</div>
	</div>
</asp:Content>
