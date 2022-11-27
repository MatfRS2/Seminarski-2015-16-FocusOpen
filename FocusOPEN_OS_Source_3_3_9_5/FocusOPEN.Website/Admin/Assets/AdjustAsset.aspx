<%@ Page Title="Adjust Asset [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="AdjustAsset.aspx.cs" Inherits="FocusOPEN.Website.Admin.AdjustAsset" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg"> 
		<div class="Left">
			<div class="AdmAppHdr">Adjust Asset</div>
			<br />
			<div class="AppCtrlMrg BodyTxt">
				You can change the preview plug-in, processing status, replace the asset thumbnail or preview below
				<span class="BodyTxt">|</span>
				<asp:HyperLink ID="ReturnToAssetHyperLink1" Text="return to asset" CssClass="BodyTxt" runat="server" />
				<span class="BodyTxt">|</span>
				<app:HelpHyperLink ID="HelpHyperLink1" HelpPage="8_Manage_Assets.htm#editing_assets" CssClass="PanelTxt" Text="help" runat="server" />
			</div>
			<br />
		</div>
		<div class="Right AdmRightPanel"></div>
		<div class="AppCtrlMrg AdmTblPanel Clear">
            <aspajax:UpdatePanel ID="AdjustUpdatePanel" UpdateMode="Conditional" runat="server">
		    <ContentTemplate>	
                <div class="Clear">
                    <app:FeedbackLabel ID="FeedbackLabel2" LineBreaks="1" runat="server" />
                    <table cellpadding="2" cellspacing="0" border="0">
                    <tr>
                        <td valign="top" class="BodyTxt Bold W120">Asset File</td>
                        <td><asp:Label ID="AssetFileLabel" CssClass="BodyTxt" runat="server" /></td>
                    </tr>
                    <tr>
                        <td valign="top" class="BodyTxt Bold">Preview Plug-in:</td>
                        <td><app:PluginDropDownList ID="PreviewPluginDropDownList" AutoPostBack="true" OnSelectedIndexChanged="PreviewPluginDropDownList_SelectedIndexChanged" ShowSupportedOnly="false" BlankText="Default Plug-in" CssClass="formInput W225" runat="server" /></td>
                    </tr>
                    <tr>
                        <td valign="top" class="BodyTxt Bold">Plug-in Status:</td>
                        <td><app:PluginStatusLabel TextClass="BodyTxt" ID="PluginStatusLabel" Visible="true" runat="server" /></td>
                    </tr>
                    <tr>
                        <td valign="top" class="BodyTxt Bold">Processing Status:</td>
                        <td>
                            <asp:RadioButton ID="ProcessingStatusTrueRadioButton" CssClass="formInput" Text="True" GroupName="ProcessingStatus" runat="server" />
                            <asp:RadioButton ID="ProcessingStatusFalseRadioButton" CssClass="formInput" Text="False" GroupName="ProcessingStatus" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td><asp:Button ID="SavePreviewSettingsButton" Text="Save" CssClass="button" OnClick="SavePreviewSettingsButton_Click" runat="server" /></td>
                    </tr>
                    </table>
                </div>
                <br />
			    <div class="Clear">
				    <app:FeedbackLabel ID="FeedbackLabel1" LineBreaks="1" runat="server" />
				    <table cellpadding="2" cellspacing="0" border="0">
                    <tr>
                        <td valign="top" class="BodyTxt Bold W120">&nbsp;</td>
                        <td>
							<asp:LinkButton ID="RegeneratePreviewAndThumbnailLinkButton" CssClass="BodyTxt Bold" Text="Regenerate preview and thumbnail" OnClick="RegeneratePreviewAndThumbnailLinkButton_Click" runat="server" />
							<br />
							<asp:LinkButton ID="RegenerateAssetBitmapsLinkButton" CssClass="BodyTxt Bold" Text="Regenerate bitmap files" OnClick="RegenerateAssetBitmapsButton_Click" runat="server" />
							<br />
						</td>
                    </tr>
				    <tr>
					    <td valign="top" class="BodyTxt Bold">Preview:</td>
					    <td>
						    <app:AssetPreview ID="AssetPreview1" UseMarginCssClass="false" runat="server" />
						    <br />
						    <asp:Label ID="PreviewExtensionLabel" runat="server" CssClass="BodyTxt" />
                            <br />
						    <asp:LinkButton ID="RegeneratePreview" Text="Regenerate Preview" CssClass="BodyTxt" OnClick="RegeneratePreview_Click" runat="server" />
						    <br />
						    <asp:LinkButton ID="DeletePreview" Text="Delete Preview" CssClass="BodyTxt" OnClick="DeletePreview_Click" runat="server" />
						    <br /><br />
						    <asp:Label ID="AcceptedPreviewFormatLabel" runat="server" CssClass="BodyTxt" /><br />
						    <asp:FileUpload ID="PreviewFileUpload" CssClass="formInput" runat="server" />
						    <asp:Button ID="SavePreviewButton" Text="Upload Preview" CssClass="button" OnClick="SavePreviewButton_Click" runat="server" />
					    </td>
				    </tr>
				    <tr>
					    <td colspan="2">&nbsp;</td>
				    </tr>
				    <tr>
					    <td valign="top" class="BodyTxt Bold">Thumbnail:</td>
					    <td>
						    <app:AssetThumbnail ID="AssetThumbnail1" UseCacheBuster="true" HighlightIfRestricted="false" ImageLinksToAssetInfo="false" ShowInfoOnHover="false" UseMarginCssClass="false" runat="server" />
						    <br />
						    <asp:LinkButton ID="RegenerateThumbnail" Text="Regenerate Thumbnail" CssClass="BodyTxt" OnClick="RegenerateThumbnail_Click" runat="server" />
						    <br />
						    <asp:LinkButton ID="DeleteThumbnail" Text="Delete Thumbnail" CssClass="BodyTxt" OnClick="DeleteThumbnail_Click" runat="server" />
						    <br /><br />
                            <asp:Label ID="AcceptedPreviewFormatThumbnailLabel" runat="server" CssClass="BodyTxt" />
                            <br />
						    <asp:FileUpload ID="ThumbnailFileUpload" CssClass="formInput" runat="server" />
						    <asp:Button ID="SaveThumbnailButton" Text="Upload Thumbnail" CssClass="button" OnClick="SaveThumbnailButton_Click" runat="server" />
					    </td>
				    </tr>
								
				    <asp:PlaceHolder ID="ReturnToAssetPlaceHolder" runat="server">
					    <tr>
						    <td>&nbsp;</td>
						    <td>
							    <br />
							    <asp:HyperLink ID="ReturnToAssetHyperLink2" Text="&laquo; Return To Asset" CssClass="BodyTxt Bold" runat="server" />
						    </td>
					    </tr>
				    </asp:PlaceHolder>
				    </table>
                    <br />
			    </div>
            </ContentTemplate>
            <Triggers>
            <aspajax:AsyncPostBackTrigger ControlID="PreviewPluginDropDownList" />
            <aspajax:PostBackTrigger ControlID="SavePreviewButton" />
            <aspajax:PostBackTrigger ControlID="SaveThumbnailButton" />
            </Triggers>
            </aspajax:UpdatePanel>
		</div>
    </div>
    <br />
</asp:Content>