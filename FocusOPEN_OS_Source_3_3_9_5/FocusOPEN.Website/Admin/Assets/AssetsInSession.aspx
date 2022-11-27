<%@ Page Title="Catalogue Assets: Step One [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="AssetsInSession.aspx.cs" Inherits="FocusOPEN.Website.Admin.AssetsInSession" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>


<asp:Content ContentPlaceHolderID="MainContent" runat="server">

	<script type="text/javascript">
		function viewAsset()
		{
			var e = document.getElementById("<%=AssetTemplateIdTextBox.ClientID%>");
			var id = e.value;
			
			if (!id)
			{
				alert("Missing asset id");
				return;
			}
			
			showAssetInfo(id);
		}
	</script>

	<div class="AdmBkg"> 
		<div class="Left AdmLeftPanel"> 
			<div class="AdmAppHdr">Catalogue Assets: Step One</div>
			<br />
			<asp:Panel ID="FormPanel" Visible="true" runat="server">
				<aspajax:UpdatePanel runat="server">
				<ContentTemplate>
					<app:FeedbackLabel ID="FeedbackLabel1" UseContainer="true" runat="server" />
					<br />
					<div class="AppCtrlMrg">
						<span class="BodyTxt Bold">Select uploaded assets to catalogue.<br /></span>
						<span class="BodyTxt">Note: fields marked <span class="ReqField">*</span> are required</span>
						<span class="BodyTxt">|</span>
						<a href="AssetUpload.aspx" class="BodyTxt">upload more assets</a>
						<span class="BodyTxt">|</span> 
						<a href="AssetList.aspx" class="BodyTxt">view all assets</a>
						<span class="BodyTxt">|</span> 
						<app:HelpHyperLink ID="HelpHyperLink1" HelpPage="8_Manage_Assets.htm#cataloguing_assets1" CssClass="BodyTxt" Text="help" runat="server" />
					</div>
					<div class="AppCtrlMrg AppCtrlPanel W470">
					<table width="470" border="0" cellspacing="0" cellpadding="2">
					<tr>
						<td class="W130 alignRight PanelTxt">
							<span class="Bold">Type</span>
							<span class="ReqField">*</span>
						</td>
						<td colspan="2">
							<app:AssetTypeDropDownList ID="AssetTypeDropDownList1" CssClass="formInput" Width="140px" OmitBlankItem="false" BlankText="All" BlankValue="0" AutoPostBack="true" OnSelectedIndexChanged="AssetTypeDropDownList1_SelectedIndexChanged" runat="server" />
						</td>
					</tr>
					<tr> 
					<td class="alignRight PanelTxt Top W130">
						<span class="Bold">Reference Number</span>
						<span class="ReqField">*</span>
						<br />
						All assets uploaded by you which have yet to be catalogued. (CTRL + click to select more than one.)
					</td>
						<td class="PanelTxt Top W130"><asp:ListBox ID="SessionAssetsListBox" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="SessionAssetsListBox_SelectedIndexChanged" CssClass="formInput" Width="140px" Height="180px" runat="server"/></td>
						<td class="PanelTxt Top">
							<asp:Panel ID="AssetControlsPanel" runat="server">
								<asp:Label ID="MultipleSelectionLabel" Visible="false" Text="(Multiple assets have been selected)" runat="server"/>
								<asp:Label ID="NoPreviewLabel" Visible="false" Text="(Nothing to preview)" runat="server"/>
								<asp:Panel ID="PreviewPanel" runat="server">
									<span id="AssetThumbnailHeaderLabel" runat="server" class="Bold AppCtrlMrg">Thumbnail<br /></span>
									<app:AssetThumbnail ID="AssetThumbnail1" ShowInfoOnHover="false" ImageLinksToAssetInfo="false" UseMarginCssClass="false" OtherCssClass="AppCtrlMrg" runat="server" />
									<br />
									<div style="margin:10px;">
										<asp:Label ID="AssetFilenameLabel" runat="server" />
										<br /><br />
										<app:PromptLinkButton ID="DeleteAssetLinkButton" OnClick="DeleteAssetLinkButton_Click" Text="Delete Asset" CssClass="BodyTxt Black" Prompt="Are you sure? This cannot be undone." runat="server" />
									</div>
								</asp:Panel>
							</asp:Panel>
						</td>
					</tr>
					</table>
					<div class="CatSubCkMrg">
						<span class="BaseLine">
							<asp:LinkButton ID="SelectAllLinkButton" runat="server" CssClass="BodyTxt bold" Text="select all assets" style="margin-left:35px" OnClick="SelectAllLinkButton_Click" />
						</span>
					</div>
						<div class="HozLine"></div>
						<table width="470" border="0" cellspacing="0" cellpadding="2">
						<tr> 
							<td class="alignRight Top W120">
								<span class="PanelTxt Bold">Select Template</span><br />
								<app:HelpHyperLink ID="HelpHyperLink3" HelpPage="8_Manage_Assets.htm#about_templates" CssClass="PanelTxt" Text="[what are templates?]" runat="server" />
							</td>
							<td>
								<asp:RadioButton ID="FirstAssetRadioButton" GroupName="UseTemplateRadioButtonGroup" runat="server" />
								<span class="PanelTxt">Use first as a template for others</span><br />
								
								<asp:RadioButton ID="UseTemplateRadioButton" GroupName="UseTemplateRadioButtonGroup" runat="server" />
								<span class="PanelTxt">Use another as a template:</span>
								<asp:TextBox ID="AssetTemplateIdTextBox" CssClass="formInput W100" runat="server" />
								<a href="javascript:viewAsset()" class="PanelTxt">[view]</a><br />
								
								<asp:RadioButton ID="NoTemplateRadioButton" Checked="true" GroupName="UseTemplateRadioButtonGroup" runat="server" />
								<span class="PanelTxt">Don't use templates</span><br /> 
							</td>
						</tr>
						</table>
						<div class="HozLine"></div>
						<div class="alignRight">
							<app:CancelButton ID="CancelButton" CssClass="button" Text="&laquo; cancel" Prompt="Are you sure you don't want to catalogue assets at this time?" RedirectUrl="AssetList.aspx" runat="server" />
							<asp:Button ID="CatalogueSelectedAssetsButton" CssClass="button" Text="catalogue selected &raquo;" OnClick="CatalogueSelectedAssetsButton_Click" runat="server" />
						</div>
					</div>
				</ContentTemplate>
				<Triggers>
					<aspajax:PostBackTrigger ControlID="DeleteAssetLinkButton" />
				</Triggers>
				</aspajax:UpdatePanel>
			</asp:Panel>
			<asp:Panel ID="NoAssetsPanel" Visible="false" runat="server">
				<br />
				<div class="AppCtrlMrg">
					<app:IconImage ID="IconImage1" Filename="fbkNegative.gif" AlternateText="[exclamation]" runat="server"/>
					<span class="FbkNegTxt">There are no uploaded assets to catalogue.</span><br /> 
					<a href="AssetUpload.aspx" class="BodyTxt">upload assets</a>
					<span class="BodyTxt">|</span>
					<a href="AssetList.aspx" class="BodyTxt">view all assets</a> 
					<span class="BodyTxt">|</span>
					<app:HelpHyperLink ID="HelpHyperLink2" HelpPage="8_Manage_Assets.htm#cataloguing_assets1" CssClass="BodyTxt" Text="help" runat="server" />
				</div>
				<div class="noResults"></div>
			</asp:Panel>
		</div>
		<div class="Right AdmRightPanel"></div>
		<div class="Clear"><br /></div>
	</div>
	<br />
</asp:Content>