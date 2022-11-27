<%@ Page Title="Manage Brands [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="ManageBrands.aspx.cs" Inherits="FocusOPEN.Website.Admin.ManageBrands" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg"> 
		<div class="Left AdmLeftPanel" style="margin-bottom:10px;"> 
			<div class="AdmAppHdr">Manage Brands</div>
			<br />
			<div class="AppCtrlMrg">
				<span class="Bold BodyTxt">Click on 'edit' to edit a brand, or 'delete' to remove a brand.<br />
				<a href="ManageBrandsForm.aspx" class="Black">Add new brands here</a>
				|
				<asp:LinkButton ID="OpenChangeMasterBrandLinkButton" Text="change master brand" CssClass="Black" OnClick="OpenChangeMasterBrandLinkButton_Click" runat="server" />
				|
				</span><app:HelpHyperLink HelpPage="9_Manage_Content.htm#manage_homepage" CssClass="BodyTxt" Text="help" runat="server" /><br />
			</div>
		</div>

		<asp:PlaceHolder ID="ChangeMasterBrandWrapper" Visible="false" runat="server">
			<div style="float:left;margin:5px 5px 10px 5px;padding:5px;width:100%;clear:both;">
				<span class="BodyTxt">Master Brand:</span>
				<app:BrandDropDownList ID="MasterBrandDropDownList" CssClass="formInput W200" OmitBlankItem="true" runat="server" />
				<asp:Button ID="ChangeMasterBrandButton" OnClick="ChangeMasterBrandButton_Click" Text="change" CssClass="button" style="margin-left:5px;" runat="server" />
				<asp:Button ID="CancelChangeMasterBrandButton" OnClick="OpenChangeMasterBrandLinkButton_Click" Text="cancel" CssClass="button" style="margin-left:5px;" runat="server" />
			</div>
		</asp:PlaceHolder>

		<div class="Right AdmRightPanel"></div>
		<div class="AdmTblPanel Clear">
			<div class="Left">
				<div class="AppResults PanelTxt">total results: <span class="Bold"><%=RecordCount%></span> brands [displaying page <%=CurrentPage%> of <%=TotalPages%>]</div>
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
				<app:SortableRepeater ID="BrandsRepeater" DefaultSortExpression="Name" DefaultSortAscending="true" ColumnCount="1" OnItemDataBound="BrandsRepeater_ItemDataBound" runat="server">
					<HeaderTemplate>
						<table width="910" border="0" cellspacing="0" cellpadding="2" class="TblOutline AppTblMrg AutoStripe">
						<tr>
							<td id="HeaderCell1" runat="server" class="Bold"><asp:LinkButton ID="LinkButton1" CssClass="BodyTxt Bold" Text="Brand" OnCommand="SortListLinkButton_Command" CommandArgument="Name" runat="server" /></td>
							<td class="Bold">&nbsp;</td>
						</tr>
					</HeaderTemplate>
					<ItemTemplate>
						<tr> 
							<td>
								<asp:HyperLink ID="BrandNameHyperLink" CssClass="BodyTxt" runat="server" />
								<asp:Label ID="IsMasterLabel" CssClass="BodyTxt" Text="(master brand)" runat="server" />
							</td>
							<td class="W75 alignCentre"><asp:HyperLink ID="BrandEditHyperLink" CssClass="BodyTxt" Text="edit" runat="server" /></td>
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
			<div class="AppResults PanelTxt">total results: <span class="Bold"><%=RecordCount%></span> brands [displaying page <%=CurrentPage%> of <%=TotalPages%>]</div>
		</div>
		<div class="Right">
			<div class="AppPageView AppPaging">
				<app:SimplePager ID="BottomPager" OnCommand="SimplePager_OnCommand" runat="server" />
			</div>
		</div>
	</div>
</asp:Content>