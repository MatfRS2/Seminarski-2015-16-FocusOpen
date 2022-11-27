<%@ Page Title="Manage Asset Types [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="ManageAssetTypes.aspx.cs" Inherits="FocusOPEN.Website.Admin.ManageAssetTypes" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg"> 
		<div class="Left AdmLeftPanel"> 
			<div class="AdmAppHdr">Manage Asset Types</div>
			<br />
			<app:FeedbackLabel ID="MessageLabel1" UseContainer="true" runat="server" />
			<br />
			<div class="AppCtrlMrg">
				<span class="Bold BodyTxt">Choose an asset type to edit below or add a new one at the bottom of the list | </span>
				<app:HelpHyperLink HelpPage="9_Manage_Content.htm#manage_assettypes" CssClass="BodyTxt" Text="help" runat="server" /><br />
				<br />
			</div>
		</div>
		<div class="Right AdmRightPanel"></div>
		<div class="AdmTblPanel Clear">
			<div class="Left">
				<br /><br />
				<div class="AppResults PanelTxt">total results: <span class="Bold"><%=RecordCount%></span> asset types [displaying page <%=CurrentPage%> of <%=TotalPages%>]</div>
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
				
				<app:SortableDataGrid ID="EntityDataGrid" DefaultSortExpression="Name" DefaultSortAscending="true" ShowAddButtonInFooterEditColumn="true" AllowDelete="true" runat="server">
					<HeaderStyle Height="27px" />
					<ItemStyle Height="27px" />
					<AlternatingItemStyle Height="27px" />
					<FooterStyle Height="27px" />
					<Columns>
						<asp:TemplateColumn HeaderText="Name" SortExpression="Name">
							<HeaderStyle CssClass="W300" />
							<ItemTemplate>
								<asp:Label ID="NameLabel" CssClass="BodyTxt" runat="server" />
							</ItemTemplate>
							<EditItemTemplate>
								<asp:TextBox ID="NameTextBox" CssClass="formInput" runat="server" />
							</EditItemTemplate>
							<FooterTemplate>
								<asp:TextBox ID="NewNameTextBox" CssClass="formInput W140" runat="server" />
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="File Extensions" SortExpression="FileExtensions">
							<ItemTemplate>
								<asp:PlaceHolder ID="FileExtensionsPlaceHolder" runat="server" />
							</ItemTemplate>
							<FooterTemplate>
								<span style="color:#fff;font-size:0.7em;margin-left:5px;">(Extensions can be specified after the asset type has been added)</span>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Visible" SortExpression="IsVisible">
							<HeaderStyle CssClass="alignCentre" />
							<ItemStyle CssClass="alignCentre" />
							<FooterStyle CssClass="alignCentre" />
							<ItemTemplate>
								<asp:Label ID="IsVisibleLabel" CssClass="BodyTxt" runat="server" />
							</ItemTemplate>
							<EditItemTemplate>
								<asp:CheckBox ID="IsVisibleCheckBox" CssClass="formInput" runat="server" />
							</EditItemTemplate>
							<FooterTemplate>
								<asp:CheckBox ID="NewIsVisibleCheckBox" Checked="true" runat="server"/>
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
			<div class="AppResults PanelTxt">total results: <span class="Bold"><%=RecordCount%></span> asset types [displaying page <%=CurrentPage%> of <%=TotalPages%>]</div>
		</div>
		<div class="Right">
			<app:SimplePager ID="BottomPager" OnCommand="SimplePager_OnCommand" runat="server" />
		</div>
	</div>
</asp:Content>