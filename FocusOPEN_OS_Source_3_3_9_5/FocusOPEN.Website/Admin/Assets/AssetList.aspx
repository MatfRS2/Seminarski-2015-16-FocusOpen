<%@ Page Title="View Asset List [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="True" CodeBehind="AssetList.aspx.cs" Inherits="FocusOPEN.Website.Admin.AssetList" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

	<script type="text/javascript">
		$(function() {
			$(".reassigncategorybutton").click(function() {
				id = parseInt($("#<%=CategoryDropDownList.ClientID%>").val());

				if (id <= 0) {
					alert("A search restricted by category must be performed before this feature can be used");
					return false;
				}
			});
		});
	</script>

	<div class="AdmBkg"> 
		<div class="Left AdmLeftPanel"> 
			<div class="AdmAppHdr">View Asset List</div>
			
			<br />
			<app:FeedbackLabel ID="MessageLabel1" UseContainer="true" runat="server" />
			<br />
			
			<span class="AppCtrlMrg BodyTxt Bold">Find an asset to view</span> 
			<span class="BodyTxt">|</span> <app:HelpHyperLink ID="HelpHyperLink1" HelpPage="8_Manage_Assets.htm#editing_assets" CssClass="BodyTxt" Text="help" runat="server" /><br />
			<div class="AppCtrlMrg AppCtrlPanel W500">
				<table border="0" cellspacing="0" cellpadding="2">
				<tr>
					<td class="W100 alignRight PanelTxt Bold">Search</td>
					<td>
						<asp:TextBox ID="SearchTermTextBox" CssClass="formInput W180" runat="Server" />
						<app:AssetTypeDropDownList ID="AssetTypeDropDownList1" CssClass="formInput W75" BlankText="All" runat="server" />
					</td>
				</tr>

				<asp:PlaceHolder ID="BrandSelectorWrapper" runat="server">
				<tr> 
					<td class="alignRight PanelTxt Bold">Brand</td>
					<td>
						<app:BrandDropDownList ID="BrandIdDropdown" CssClass="formInput W180" BlankText="All" AutoPostBack="true" OnSelectedIndexChanged="BrandIdDropdown_SelectedIndexChanged" runat="server" />
					</td>
				</tr>
				</asp:PlaceHolder>
				
				<tr>
					<td class="W100 alignRight PanelTxt Bold">Category</td>
					<td><app:NestedDataCategoryDropDownList ID="CategoryDropDownList" CssClass="formInput W180" BrandId="0" BlankText="All" BlankValue="0" runat="server" /></td>
				</tr>
				
				<app:SecuredPlaceHolder ID="UploadUserSelectorWrapper" AllowedRoles="BrandAdministrator,SuperAdministrator" runat="server">
					<tr>
						<td class="alignRight PanelTxt Bold">Upload User</td>
						<td>
							<app:UserDropDownList ID="UploadUserDropDownList" BindOnInit="false" BlankText="All" CssClass="formInput W180" runat="server" />
						</td>
					</tr>
				</app:SecuredPlaceHolder>
				
				<tr>
					<td class="W100 alignRight PanelTxt Bold">Publication Status</td>
					<td><app:AssetPublicationStatusDropDownList ID="AssetPublicationStatusDropDownList1" CssClass="formInput W180" runat="server" /></td>
				</tr>
				
				<tr>
					<td class="W100 alignRight PanelTxt Bold" valign="top">Show Thumbnails</td>
					<td><asp:CheckBox ID="ShowThumbnailsCheckBox" runat="server" /></td>
				</tr>
				
                <tr>
					<td class="alignRight PanelTxt Bold">Output</td>
					<td class="PanelTxt">
						<asp:RadioButton ID="ViewOnPageRadioButton" GroupName="ReportOutput" Checked="true" runat="server" /> view below 
						<asp:RadioButton ID="DownloadCSVRadioButton" GroupName="ReportOutput" runat="server" /> download as csv
                        <asp:RadioButton ID="DownloadXMLRadioButton" GroupName="ReportOutput" runat="server" /> download as XML
					</td>
				</tr>

				</table>
				<br />
				<div class="alignRight Clear">
					<asp:Button ID="ResetButton" Text="reset" CssClass="button" OnClick="ResetButton_Click" runat="server" />
					<asp:Button ID="SearchButton" Text="search" CssClass="button" OnClick="SearchButton_Click" runat="server" />
				</div>
			</div>
		</div>
		<div class="Right AdmRightPanel"></div>
		<div class="AdmTblPanel Clear">
			<div class="Left">
				<br /><br />
				<div class="AppResults PanelTxt">total results: <span class="Bold"><%=RecordCount%></span> assets [displaying page <%=CurrentPage%> of <%=TotalPages%>]</div>
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
				<asp:Panel ID="GridPanel" runat="server">
					<div style="margin-left:10px;">
						<asp:Button ID="SelectAllButton1" Text="select all" CssClass="button" OnClick="SelectAllButton_Click" runat="server" />
						<asp:Button ID="DeselectAllButton1" Text="deselect all" CssClass="button" OnClick="DeselectAllButton_Click" runat="server" />
						<app:SecuredPlaceHolder AllowedRoles="SuperAdministrator" runat="server">
							&rarr;
							<asp:Button ID="BulkRenewButton1" Text="renew" CssClass="button" OnClick="BulkRenewButton_Click" runat="server" />
							<asp:Button ID="BulkDelegateButton1" Text="delegate" CssClass="button" OnClick="BulkDelegateButton_Click" runat="server" />
							<asp:Button ID="BulkDeleteButton1" Text="delete" CssClass="button" OnClick="BulkDeleteButton_Click" runat="server" />
							<asp:Button ID="BulkChangeCategoryButton1" Text="reassign categories" CssClass="reassigncategorybutton button" OnClick="BulkChangeCategoryButton_Click" runat="server" />
							<asp:Button ID="BulkChangePermissionsButton1" Text="change permissions" CssClass="button" OnClick="BulkChangePermissionsButton_Click" runat="server" />
						</app:SecuredPlaceHolder>
						<asp:Button ID="BulkTemplateCatalogueButton1" Text="template catalogue selected" CssClass="button" OnClick="BulkTemplateCatalogueButton_Click" runat="server" OnClientClick="return confirm('Warning: re-cataloguing assets using template cataloguing will replace or remove any existing data, are you sure you wish to proceed?')" />
						<asp:Button ID="BulkRegeneratePreviewsButton1" Text="regenerate previews" CssClass="button" OnClick="BulkRegeneratePreviewsButton_Click" runat="server" OnClientClick="return confirm('This may take some time and will cause the existing previews &amp; thumbnails to be overwritten, are you sure you wish to proceed?')" />
					</div>
					<br />
					
					<app:AssetList ID="AssetList1" ShowCheckBoxes="true" ShowThumbnails="false" runat="server" />
					
					<div style="margin-left:10px;">
						<br />
						<asp:Button ID="SelectAllButton2" Text="select all" CssClass="button" OnClick="SelectAllButton_Click" runat="server" />
						<asp:Button ID="DeselectAllButton2" Text="deselect all" CssClass="button" OnClick="DeselectAllButton_Click" runat="server" />
						<app:SecuredPlaceHolder AllowedRoles="SuperAdministrator" runat="server">
							&rarr;
							<asp:Button ID="BulkRenewButton2" Text="renew" CssClass="button" OnClick="BulkRenewButton_Click" runat="server" />
							<asp:Button ID="BulkDelegateButton2" Text="delegate" CssClass="button" OnClick="BulkDelegateButton_Click" runat="server" />
							<asp:Button ID="BulkDeleteButton2" Text="delete" CssClass="button" OnClick="BulkDeleteButton_Click" runat="server" />
							<asp:Button ID="BulkChangeCategoryButton2" Text="reassign categories" CssClass="reassigncategorybutton button" OnClick="BulkChangeCategoryButton_Click" runat="server" />
							<asp:Button ID="BulkChangePermissionsButton2" Text="change permissions" CssClass="button" OnClick="BulkChangePermissionsButton_Click" runat="server" />
						</app:SecuredPlaceHolder>
						<asp:Button ID="BulkTemplateCatalogueButton2" Text="template catalogue selected" CssClass="button" OnClick="BulkTemplateCatalogueButton_Click" runat="server" OnClientClick="return confirm('Warning: re-cataloguing assets using template cataloguing will replace or remove any existing data, are you sure you wish to proceed?')" />
						<asp:Button ID="BulkRegeneratePreviewsButton2" Text="regenerate previews" CssClass="button" OnClick="BulkRegeneratePreviewsButton_Click" runat="server" OnClientClick="return confirm('This may take some time and will cause the existing previews &amp; thumbnails to be overwritten, are you sure you wish to proceed?')" />
					</div>
				</asp:Panel>
				
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
			<div class="AppResults PanelTxt">total results: <span class="Bold"><%=RecordCount%></span> assets [displaying page <%=CurrentPage%> of <%=TotalPages%>]</div>
		</div>
		<div class="Right">
			<app:SimplePager ID="BottomPager" OnCommand="SimplePager_OnCommand" runat="server" />
		</div>
	</div>
</asp:Content>