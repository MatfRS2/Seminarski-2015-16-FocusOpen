<%@ Page Title="Manage Companies [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="ManageCompanies.aspx.cs" Inherits="FocusOPEN.Website.Admin.ManageCompanies" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg"> 
		<div class="Left AdmLeftPanel"> 
			<div class="AdmAppHdr">Manage Companies</div>
			<br />
			<app:FeedbackLabel ID="MessageLabel1" UseContainer="true" runat="server" />
			<br />
			<div class="AppCtrlMrg">
				<span class="Bold BodyTxt">
					Click on 'edit' to edit a company.<br /> 
					Click on 'delete' to remove a company<br />
					New companies can be added at the end of the list |
				</span>
				<app:HelpHyperLink ID="HelpHyperLink1" HelpPage="6_User_Admin.htm#manage_companies" CssClass="BodyTxt" Text="help" runat="server" /><br />
			</div>
		</div>
		<div class="Right AdmRightPanel"></div>
		<div class="AdmTblPanel Clear">
			<div class="Left">
				<br /><br />
				<div class="AppResults PanelTxt">total results: <span class="Bold"><%=RecordCount%></span> companies [displaying page <%=CurrentPage%> of <%=TotalPages%>]</div>
			</div>
			<div class="Right">
				<div class="AppPageView">
					<span class="PanelTxt">show</span>
					<app:PageSizeDropDownList ID="PageSizeDropDownList1" CssClass="formInput" Mode="Admin" runat="server" />
					<div class="AppPaging">
						<app:SimplePager ID="TopPager" OnCommand="SimplePager_OnCommand" runat="server" />
					</div>
				</div>
			</div>
			<div class="Clear">
				<br />
				
				<app:SortableDataGrid ID="EntityDataGrid" AllowDelete="true" DeletePrompt="Are you sure you want to delete this company?\nUsers will no longer be able to register using this email domain." DefaultSortExpression="Name" DefaultSortAscending="true" ShowAddButtonInFooterEditColumn="true" OnItemCreated="EntityDataGrid_ItemCreated" runat="server">
				<Columns>
					<asp:TemplateColumn HeaderText="Company Name" SortExpression="Name">
						<HeaderStyle Height="25px" />
						<ItemStyle Height="25px" />
						<FooterStyle VerticalAlign="Top" />
						<ItemTemplate>
							<asp:Label ID="NameLabel" CssClass="BodyTxt" runat="server" />
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox ID="NameTextBox" CssClass="formInput W225" runat="server" />
						</EditItemTemplate>
						<FooterTemplate>
							<span class="AdmHdrTxt Bold" style="margin-left:0px;">Company Name:</span><br />
							<asp:TextBox ID="NewNameTextBox" CssClass="formInput W225" runat="server" />
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Brands" SortExpression="Brands">
						<FooterStyle VerticalAlign="Top" />
						<ItemTemplate>
							<asp:Label ID="BrandsLabel" CssClass="BodyTxt" runat="server" />
						</ItemTemplate>
						<EditItemTemplate>
							<app:BrandCheckBoxList ID="BrandsCheckBoxList" Restrict="true" CssClass="BodyTxt White" runat="server" />
						</EditItemTemplate>
						<FooterTemplate>
							<span class="AdmHdrTxt Bold" style="margin-left:0px;">Brands:</span><br />
							<app:BrandCheckBoxList ID="NewBrandsCheckBoxList" Restrict="true" CssClass="BodyTxt White" runat="server" />
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Email Domain" SortExpression="Domain">
						<FooterStyle VerticalAlign="Top" />
						<ItemTemplate>
							<asp:Label ID="DomainLabel" CssClass="BodyTxt" runat="server" />
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox ID="DomainTextBox" CssClass="formInput W225" runat="server" />
						</EditItemTemplate>
						<FooterTemplate>
							<span class="AdmHdrTxt Bold" style="margin-left:0px;">Email Domain:</span><br />
							<asp:TextBox ID="NewDomainTextBox" CssClass="formInput W225" runat="server" />
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Internal" SortExpression="IsInternal">
						<HeaderStyle CssClass="alignCentre" />
						<ItemStyle CssClass="alignCentre" />
						<FooterStyle CssClass="alignCentre" />
						<ItemTemplate>
							<asp:Label ID="InternalLabel" CssClass="BodyTxt" runat="server" />
						</ItemTemplate>
						<EditItemTemplate>
							<asp:CheckBox ID="InternalCheckBox" runat="server" />
							<asp:Label ID="InternalCheckBoxLabel" Visible="false" Text="N" CssClass="BodyTxt" runat="server" />
						</EditItemTemplate>
						<FooterTemplate>
							<asp:CheckBox ID="NewInternalCheckBox" runat="server" />
							<asp:Label ID="NewInternalCheckBoxLabel" Visible="false" Text="N" CssClass="BodyTxt" runat="server" />
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:EditCommandColumn ButtonType="LinkButton" EditText="edit" UpdateText="update" CancelText="cancel"/>
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
			<div class="AppResults PanelTxt">total results: <span class="Bold"><%=RecordCount%></span> companies [displaying page <%=CurrentPage%> of <%=TotalPages%>]</div>
		</div>
		<div class="Right">
			<app:SimplePager ID="BottomPager" OnCommand="SimplePager_OnCommand" runat="server" />
		</div>
	</div>
</asp:Content>