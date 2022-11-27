<%@ Page Title="Manage File Extensions [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="ManageFileExtensionsForm.aspx.cs" Inherits="FocusOPEN.Website.Admin.Content.ManageFileExtensionsForm" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript">
		$(function() {
			$(".extensionTextBox").blur(function() {
				$(this).val($(this).val().toLowerCase());
			});
		});
	</script>
	<div class="AdmBkg">
		<div class="Left AdmLeftPanel"> 
			<div class="AdmAppHdr">Manage File Extensions</div>
			<br />
			<asp:Panel ID="FormPanel" runat="server">
				<a class="BodyTxt Bold AppCtrlMrg" href="ManageFileExtensions.aspx">&laquo; Return to file extensions list</a>
				<br /><br />
				<div class="AppCtrlMrg"><app:FeedbackLabel ID="MessageLabel1" runat="server" /></div>
				<br />
				<div class="AppCtrlMrg AppCtrlPanel">
                    <aspajax:UpdatePanel ID="ExtensionsUpdatePanel" UpdateMode="Conditional" runat="server">
				    <ContentTemplate>	
					    <table border="0" cellspacing="0" cellpadding="2">
					    <tr>
						    <td class="W160 alignRight PanelTxt">Extension <span class="ReqField">*</span></td>
						    <td><asp:TextBox ID="ExtensionTextBox" CssClass="extensionTextBox formInput W50" MaxLength="4" runat="server"/></td>
					    </tr>
					    <tr>
						    <td class="alignRight PanelTxt">File Type Name <span class="ReqField">*</span></td>
						    <td><asp:TextBox ID="FileTypeNameTextBox" CssClass="formInput W225" runat="server"/></td>
					    </tr>
					    <tr>
						    <td class="alignRight PanelTxt" valign="top">Icon &nbsp;</td>
						    <td>
							    <asp:FileUpload ID="IconFileUpload" CssClass="formInput W200" runat="server" />
							    <asp:Panel ID="IconImagePanel" Visible="false" runat="server">
								    <table border="0" cellpadding="3" cellspacing="0">
								    <tr>
									    <td><asp:Image ID="IconImage" runat="server" /></td>
									    <td><span style="font-size:0.6em" >(right click, save-as to download)</span></td>
								    </tr>
								    </table>
							    </asp:Panel>
						    </td>
					    </tr>
					    <tr>
						    <td class="alignRight PanelTxt">Asset Type <span class="ReqField">*</span></td>
						    <td><app:AssetTypeDropDownList ID="AssetTypeDropDownList" DisplayAll="true" CssClass="formInput W225" runat="server" /></td>
					    </tr>
					    <tr>
						    <td class="alignRight PanelTxt">Visible &nbsp;</td>
						    <td><asp:CheckBox ID="IsVisibleCheckBox" Checked="true" runat="server" /></td>
					    </tr>
					
					    <asp:PlaceHolder ID="ProcessingSupportPlaceHolder" Visible="false" runat="server">
					    <tr>
						    <td class="alignRight PanelTxt">Processing Support?</td>
						    <td><asp:Label ID="ProcessingSupportLabel" CssClass="BodyTxt" runat="server" /></td>
					    </tr>
					    <tr>
						    <td class="alignRight PanelTxt">Preview File Format</td>
						    <td><asp:Label ID="PreviewFileFormatLabel" CssClass="BodyTxt" runat="server" /></td>
					    </tr>
                        <tr>
						    <td class="alignRight PanelTxt">Show Supported Plug-ins Only</td>
						    <td><asp:CheckBox ID="ShowSupportedPluginsCheckbox" runat="server" Checked="true" OnCheckedChanged="ShowSupportedPluginsCheckbox_CheckChanged"  AutoPostBack="true"  /></td>
					    </tr>
                        </asp:PlaceHolder>
                        <tr>
                            <td class="alignRight PanelTxt">Preview Plug-in</td>
                            <td><app:PluginDropDownList ID="PreviewPluginDropDownList" CssClass="formInput W225" runat="server" /></td>
                        </tr>
                         </table>
                    </ContentTemplate>
                    <Triggers>
                    <aspajax:AsyncPostBackTrigger ControlID="ShowSupportedPluginsCheckbox" />
                    </Triggers>
                    </aspajax:UpdatePanel>
                    
					<div style="margin-left:110px;margin-top:20px;">
						<asp:Button ID="SaveButton" CssClass="button" Text="save" OnClick="SaveButton_Click" runat="server" />
						<app:CancelButton ID="CancelButton" CssClass="button" Text="cancel" Prompt="Are you sure? Changes will not be saved" RedirectUrl="ManageFileExtensions.aspx" runat="server" />
						<app:PromptButton ID="DuplicateButton" CssClass="button" Text="save &amp; duplicate" OnClick="DuplicateButton_Click" Prompt="Are you sure you want to duplicate this file extension?" runat="server" />
						<app:PromptButton ID="DeleteButton" CssClass="delete button" Text="delete" OnClick="DeleteButton_Click" Prompt="Are you sure you want to delete this file extension?  This action cannot be undone!" Visible="false" runat="server" />
					</div>
				</div>
			</asp:Panel>
			<asp:Panel ID="SuccessPanel" Visible="false" runat="server">
				<div class="AppCtrlMrg">
					<app:FeedbackLabel ID="MessageLabel2" UseContainer="true" LineBreaks="2" MessageType="Positive" Pinned="true" Header="File Extension Saved" Text="The file extension information was saved successfully" runat="server" />
					<span class="BodyTxt Bold">What do you want to do now?</span>
					<ul>
						<li class="BodyTxt"><a href="ManageFileExtensions.aspx">View the file extensions list</a></li>
						<li class="BodyTxt"><a href="ManageAssetTypes.aspx">View the asset types list</a></li>
					</ul>
				</div>
			</asp:Panel>
		</div>
		<div class="Right AdmRightPanel"></div>
		<div class="Clear"><br /></div>
	</div>
	<br />
</asp:Content>