<%@ Page Title="Manage Metadata [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="ManageMetadataValues.aspx.cs" Inherits="FocusOPEN.Website.Admin.ManageMetadataValues" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg"> 
		<div class="Left W700"> 
			<div class="AdmAppHdr">Manage Metadata</div>
			<br />
			<div class="AppCtrlMrg">
				<span class="Bold BodyTxt">
					- Click 'edit' to edit the metadata category
					|
					<app:HelpHyperLink HelpPage="9_Manage_Content.htm#manage_metadata" CssClass="BodyTxt" Text="help" runat="server" /><br />
				</span>
			</div>
		</div>
		<div class="Right W200"></div>
		<div class="AdmTblPanel Clear">
			<div class="Left">
				<asp:PlaceHolder ID="BrandSelector" runat="server">
					<div style="margin:15px 0px 0px 15px;">
						<span class="PanelTxt Bold" style="margin-right:6px;">Brand: </span>
						<app:BrandDropDownList ID="BrandDropDownList1" class="formInput" OmitBlankItem="true" AutoPostBack="true" OnSelectedIndexChanged="BrandDropDownList1_SelectedIndexChanged" runat="server" />
					</div>
				</asp:PlaceHolder>
			</div>
			<div class="Clear">
				<br />
				<asp:Panel ID="NoMetadataPanel" Visible="false" runat="server">
					<strong>No metadata available. Please <asp:HyperLink NavigateUrl="~/Admin/Utils/ConfigureMetadata.aspx" runat="server">configure metadata</asp:HyperLink> first.</strong>
				</asp:Panel>
				<asp:Repeater ID="MetadataGroupsRepeater" OnItemDataBound="MetadataGroupsRepeater_ItemDataBound" runat="server">
					<HeaderTemplate>
						<table ID="MetadataTable" width="910" border="0" cellpadding="5" cellspacing="0" class="AutoStripe TblOutline AppTblMrg">
						<tr>
							<td class="PanelTxt Bold">Metadata</td>
							<td class="W75">&nbsp;</td>
						</tr>
					</HeaderTemplate>
					<FooterTemplate>
						</table>
					</FooterTemplate>
					<ItemTemplate>
						<tr>
							<td class="PanelTxt"><asp:HyperLink ID="MetadataNameHyperLink" CssClass="PanelTxt" runat="server" /></td>
							<td class="PanelTxt alignCentre"><asp:HyperLink ID="EditMetadataGroupHyperLink" CssClass="PanelTxt" Text="edit" runat="server" /></td>
						</tr>
					</ItemTemplate>
				</asp:Repeater>
				<br />
			</div>
		</div>
	</div>
</asp:Content>