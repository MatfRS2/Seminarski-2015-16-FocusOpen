<%@ Page Title="Manage Brand Detail [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="ManageBrandsForm.aspx.cs" Inherits="FocusOPEN.Website.Admin.Content.ManageBrandsForm" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg">
		<div class="Left AdmLeftPanel"> 
			<div class="AdmAppHdr">Manage Brands</div>
			<br />
			<asp:Panel ID="FormPanel" runat="server">
				<a class="BodyTxt Bold AppCtrlMrg" href="ManageBrands.aspx">&laquo; Return to brand list</a>
				<br /><br />
				<div class="AppCtrlMrg"><app:FeedbackLabel ID="MessageLabel1" LineBreaks="1" runat="server" /></div>
				<div class="AppCtrlMrg AppCtrlPanel W550">
					<table width="100%" border="0" cellspacing="0" cellpadding="2">
					<tr>
						<td class="W280 alignRight PanelTxt">Full brand name <span class="ReqField">*</span></td>
						<td><asp:TextBox ID="FullBrandNameTextBox" CssClass="formInput W225" MaxLength="250" runat="server"/></td>
					</tr>
					<tr>
						<td class="alignRight PanelTxt">Short brand name <span class="ReqField">*</span></td>
						<td><asp:TextBox ID="ShortBrandNameTextBox" CssClass="formInput W225" MaxLength="50" runat="server"/></td>
					</tr>
					<tr>
						<td class="alignRight PanelTxt">Application name <span class="ReqField">*</span></td>
						<td><asp:TextBox ID="ApplicationNameTextBox" CssClass="formInput W225" MaxLength="300" runat="server"/></td>
					</tr>
					<tr>
						<td class="alignRight PanelTxt">Organisation name <span class="ReqField">*</span></td>
						<td><asp:TextBox ID="OrganisationNameTextBox" CssClass="formInput W225" MaxLength="300" runat="server"/></td>
					</tr>
					<tr>
						<td class="alignRight PanelTxt">Website URL <span class="ReqField">*</span></td>
						<td><asp:TextBox ID="WebsiteUrlTextBox" CssClass="formInput W225" MaxLength="255" runat="server"/></td>
					</tr>
					<tr>
						<td class="alignRight PanelTxt">Email From <span class="ReqField">*</span></td>
						<td><asp:TextBox ID="EmailFromTextBox" CssClass="formInput W225" MaxLength="100" runat="server" /></td>
					</tr>
					<tr>
						<td class="alignRight PanelTxt">Allow brand selection during registration</td>
						<td><asp:CheckBox ID="AllowBrandSelectionDuringRegistrationCheckBox" CssClass="formInput" runat="server" /></td>
					</tr>
					<tr>
						<td class="alignRight PanelTxt" valign="top">Brand file pack</td>
						<td>
							<asp:FileUpload ID="BrandFilePackUpload" CssClass="formInput" runat="server" /><br />
							<asp:Panel ID="BrandFilePackActionPanel" CssClass="BodyTxt" Visible="false" runat="server">
								<asp:LinkButton ID="DownloadBrandFilePackLinkButton" OnClick="DownloadBrandFilePackLinkButton_Click" CssClass="BodyTxt" Text="download" runat="server" />
								&nbsp;|&nbsp;
								<asp:HyperLink ID="EditBrandFilesHyperLink" NavigateUrl="#" CssClass="BodyTxt" Text="edit" runat="server" />
								<asp:PlaceHolder Visible="False" runat="Server">
									&nbsp;|&nbsp;
									<asp:HyperLink ID="BrowseBrandFolderHyperLink" NavigateUrl="#" CssClass="BodyTxt" Text="browse" Target="_blank" runat="server" />
								</asp:PlaceHolder>
							</asp:Panel>
						</td>
					</tr>
					<tr>
						<td class="alignRight PanelTxt" valign="top">Watermark Image (for image and video previews)</td>
						<td>
							<asp:PlaceHolder ID="WatermarkImageWrapper" Visible="false" runat="server">
								<asp:Image ID="PreviewWatermarkImage" style="border:1px solid #ccc;margin-bottom:5px;background:#eee" runat="server" />
								<br />
								<asp:LinkButton ID="DownloadWatermarkLinkButton" CssClass="BodyTxt" Text="download watermark" OnClick="DownloadWatermarkLinkButton_Click" runat="server" />
								<br />
							</asp:PlaceHolder>
							<asp:FileUpload ID="PreviewWatermarkImageUpload" CssClass="formInput" runat="server" /><br />
							<span style="font-size:0.7em">Recommended dimensions: 320 (w) x 240 (h)</span>
						</td>
					</tr>
					
					<tr>
						<td class="alignRight PanelTxt Top">Enable direct download of assets</td>
						<td><asp:CheckBox ID="EnableDirectDownloadCheckBox" CssClass="formInput" runat="server" /></td>
					</tr>
					
					<tr>
						<td class="alignRight PanelTxt Top">Login Page (HTML allowed) <span class="ReqField">*</span></td>
						<td>
							<span class="BodyTxt">Above login fields:</span><br />
							<app:RichTextBox ID="LoginPageUpperCopyTextBox" CssClass="formInput W225" Height="90px" ExtraConfig="toolbar:'FocusOPENToolbar'" runat="server" />
							<br />
							<span class="BodyTxt">Below login fields:</span><br />
							<app:RichTextBox ID="LoginPageLowerCopyTextBox" CssClass="formInput W225" Height="90px" runat="server" />
						</td>
					</tr>
					
					<tr>
						<td class="alignRight PanelTxt Top">Default usage restriction notice (HTML allowed) <span class="ReqField">*</span></td>
						<td><app:RichTextBox ID="DefaultUsageRestrictionsCopyTextBox" CssClass="formInput W225" Height="125px" runat="server" /></td>
					</tr>
					<tr>
						<td class="alignRight PanelTxt Top">My Account copy (HTML allowed) <span class="ReqField">*</span></td>
						<td><app:RichTextBox ID="MyAccountCopyTextBox" CssClass="formInput W225" Height="125px" runat="server" /></td>
					</tr>
					<tr>
						<td class="alignRight PanelTxt Top">Admin copy (HTML allowed) <span class="ReqField">*</span></td>
						<td><app:RichTextBox ID="AdminCopyTextBox" CssClass="formInput W225" Height="125px" runat="server" /></td>
					</tr>
					<tr>
						<td class="alignRight PanelTxt Top">Terms &amp; Conditions copy (HTML allowed) <span class="ReqField">*</span></td>
						<td><app:RichTextBox ID="TermsConditionsCopyTextBox" CssClass="formInput W225" Height="125px" runat="server" /></td>
					</tr>
					<tr>
						<td class="alignRight PanelTxt Top">Privacy Policy (HTML allowed) <span class="ReqField">*</span></td>
						<td><app:RichTextBox ID="PrivacyPolicyCopyTextBox" CssClass="formInput W225" Height="125px" runat="server" /></td>
					</tr>
					</table>
					<div style="margin-left:287px;margin-top:20px;">
						<asp:Button ID="SaveButton" CssClass="button" Text="save" OnClick="SaveButton_Click" runat="server" />
						<app:CancelButton ID="CancelButton" CssClass="button" Text="cancel" Prompt="Are you sure? Brand details will not be saved" RedirectUrl="ManageBrands.aspx" runat="server" />
						&nbsp;&nbsp;
						<app:PromptButton ID="DeleteButton" CssClass="delete button" Text="delete brand" OnCommand="DeleteButton_Command" Prompt="Are you sure you want to delete this brand?  This action cannot be undone!" Visible="false" runat="server" />
					</div>
				</div>
			</asp:Panel>
			<asp:Panel ID="SuccessPanel" Visible="false" runat="server">
				<div class="AppCtrlMrg">
					<app:FeedbackLabel ID="FeedbackLabel2" UseContainer="true" LineBreaks="2" MessageType="Positive" Pinned="true" Header="Brand Saved" Text="The brand information was updated successfully" runat="server" />
					<span class="BodyTxt Bold">What do you want to do now?</span>
					<ul>
						<li class="BodyTxt"><a href="ManageBrands.aspx">Return to the brand list</a></li>
					</ul>
				</div>
			</asp:Panel>
		</div>
		<div class="Right AdmRightPanel"></div>
		<div class="Clear"><br /></div>
	</div>
	<br />
</asp:Content>