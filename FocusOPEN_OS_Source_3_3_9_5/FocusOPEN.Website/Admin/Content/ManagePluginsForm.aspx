<%@ Page Title="Manage Preview Plug-ins [ADMIN]" Language="C#" ValidateRequest="false" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="ManagePluginsForm.aspx.cs" Inherits="FocusOPEN.Website.Admin.Content.ManagePluginsForm" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

	<div class="AdmBkg">
		<div class="Left AdmLeftPanel"> 
			<div class="AdmAppHdr">Manage Preview Plug-ins</div>
			<br />
			<asp:Panel ID="FormPanel" runat="server">
                <span class="BodyTxt AppCtrlMrg">Use this to register, modify and test the selected plug-in 				|
				<app:HelpHyperLink ID="HelpHyperLink1" HelpPage="9_Manage_Content.htm#manage_plugins" CssClass="BodyTxt" Text="help" runat="server" />
                &nbsp;&nbsp;<a class="BodyTxt Bold AppCtrlMrg" href="ManagePlugins.aspx">&laquo; Return to preview plug-ins list</a>
				</span>
				<br /><br />
				<div class="AppCtrlMrg AppCtrlPanel">
                    <span class="BodyTxt Bold">Plug-in Details</span>
                    <aspajax:UpdatePanel ID="PluginsUpdatePanel" UpdateMode="Conditional" runat="server">
				    <ContentTemplate>	
					    <table border="0" cellspacing="0" cellpadding="2">
					    <tr>
						    <td class="W200 PanelTxt">Plug-in Name</td>
						    <td><asp:Label ID="PluginNameLabel" CssClass="BodyTxt" runat="server"></asp:Label></td>
					    </tr>
					    <tr>
						    <td class="PanelTxt">Version</td>
						    <td><asp:Label ID="PluginVersionLabel" CssClass="BodyTxt" runat="server"></asp:Label></td>
					    </tr>
					    <tr>
						    <td class="PanelTxt" valign="top">Plug-in Folder</td>
                            <td><asp:Label ID="PluginFolderLabel" CssClass="BodyTxt" runat="server"></asp:Label></td>
					    </tr>
                        <tr>
						    <td class="PanelTxt" valign="top">Registration GUID</td>
                            <td>
                                <asp:Label ID="PluginGuidLabel" CssClass="BodyTxt" runat="server"></asp:Label>
                                <asp:PlaceHolder ID="PluginUnregisterPlaceHolder" runat="server" Visible="false">
                                &nbsp;<app:PromptLinkButton runat="server" ID="PluginUnregisterLinkButton" OnClick="PluginUnregisterLinkButton_Click" Prompt="Are you sure you wish to unregister this plug-in?" Text="unregister" CssClass="BodyTxt" />
                                </asp:PlaceHolder>
                            </td>
					    </tr>
                        <tr>
						    <td class="PanelTxt" valign="top">Plug-in Pack</td>
                            <td><asp:LinkButton ID="DownloadPluginPackLinkButton" OnClick="DownloadPluginPackLinkButton_Click" CssClass="BodyTxt" Text="download" runat="server" /></td>
					    </tr>
                        <tr>
						    <td class="PanelTxt" valign="top">Require all files?</td>
                            <td><asp:Label ID="RequireAllFilesLabel" CssClass="BodyTxt" runat="server"></asp:Label></td>
					    </tr>
                        <tr>
						    <td class="PanelTxt" valign="top">Missing files?</td>
                            <td><asp:Label ID="MissingFilesLabel" CssClass="BodyTxt" runat="server"></asp:Label></td>
					    </tr>
                        <tr>
						    <td>&nbsp;</td>
						    <td><asp:LinkButton ID="ViewManifestLinkButton" OnClick="ViewManifestLinkButton_Click" CssClass="BodyTxt" Text="[+] View Manifest" runat="server" /></td>
					    </tr>
                        <asp:PlaceHolder ID="ViewManifestPlaceHolder" Visible="false" runat="server">
                            <asp:Repeater ID="ViewManifestRepeater"  runat="server">
                                <ItemTemplate>
                                    <tr>
						                <td>&nbsp;</td>
						                <td><asp:Label ID="ManifestFileLabel" Text="<%# Container.DataItem %>" CssClass="BodyTxt" runat="server" /></td>
					                </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:PlaceHolder ID="NoManifestPlaceHolder" Visible="false" runat="server">
					            <tr>
                                <td>&nbsp;</td>
						        <td><app:FeedbackLabel ID="NoResultsMessageLabel" MessageType="Negative" Header="no files found" Pinned="true" Visible="true" runat="server" /></td>
					            </tr>
				            </asp:PlaceHolder>
                        </asp:PlaceHolder>
					    <tr>
						    <td class="PanelTxt">Active</td>
						    <td><asp:CheckBox ID="PluginActiveCheckbox" CssClass="BodyTxt" runat="server" Checked="true"  /></td>
					    </tr>
                         </table>				
                    </ContentTemplate>
                    <Triggers>
                       <aspajax:PostBackTrigger ControlID="DownloadPluginPackLinkButton" />
                       <aspajax:AsyncPostBackTrigger ControlID="ViewManifestLinkButton" />                       
                    </Triggers>
                    </aspajax:UpdatePanel>
                    <br />
                    <span class="BodyTxt Bold">Plug-in Contexts</span>
                    <asp:Repeater ID="ContextsRepeater"  runat="server" OnItemDataBound="ContextsRepeater_ItemDataBound">
                        <HeaderTemplate>
                            <table border="0" cellspacing="0" cellpadding="2">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
						        <td><asp:Label ID="ContextTypeLabel" Text="<%# Container.DataItem %>" CssClass="BodyTxt" runat="server" /></td>
                                <td><asp:LinkButton ID="TestContextLinkButton" OnCommand="TestContextLinkButton_Command" CommandArgument="<%# Container.DataItem %>" CssClass="BodyTxt" Text="test" runat="server" /></td>
                                <td><asp:LinkButton ID="EditContextLinkButton" OnCommand="EditContactLinkButton_Command" CommandArgument="<%# Container.DataItem %>" CssClass="BodyTxt" Text="edit" runat="server" /></td>
                                <td><app:PluginContextDropDownList CssClass="formInput W150" ID="ContextNamesDropDownList" runat="server"></app:PluginContextDropDownList></td>
					        </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>

					<div style="margin-left:10px;margin-top:10px;">
						<asp:Button ID="SaveButton" CssClass="button" Text="save" OnClick="SaveButton_Click" runat="server" />
						<app:CancelButton ID="CancelButton" CssClass="button" Text="cancel" Prompt="Are you sure? Changes will not be saved" RedirectUrl="ManagePlugins.aspx" runat="server" />
					</div>
				</div>
			</asp:Panel>
			<asp:PlaceHolder ID="FeedbackPlaceholder" Visible="false" runat="server">
				<div class="AppCtrlMrg">
					<app:FeedbackLabel ID="SaveFeedbackLabel" UseContainer="true" LineBreaks="2" Pinned="true" runat="server" />
				</div>
			</asp:PlaceHolder>
            <asp:PlaceHolder ID="ContextEditorPlaceHolder" Visible="false" runat="server">
                 <div class="AppCtrlMrg AppCtrlPanel">
                    <asp:Label ID="ContextEditorCaptionLabel" CssClass="BodyTxt" runat="server"></asp:Label>
                    <br />
                    <app:TextArea runat="server" ID="ContextEditorTextArea" Width="400" Rows="5" CssClass="formInput W400" Wrap="true" />
                 </div>
                 <div style="margin-left:10px;margin-top:10px;">
					<asp:Button ID="EditorSaveCloseButton" CssClass="button" Text="save & close" OnClick="EditorSaveCloseButton_Click" runat="server" />
                    &nbsp;<asp:Button ID="EditorSaveTestButton" CssClass="button" Text="save & test" OnClick="EditorSaveTestButton_Click" runat="server" />
                    &nbsp;<app:PromptButton ID="EditorCancelButton" Prompt="Any changes made will not be saved. Are you sure you wish to continue?" CssClass="button" Text="cancel" OnClick="EditorCancelButton_Click" runat="server" />
				</div>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="ContextTesterPlaceHolder" Visible="false" runat="server">
                 <div class="AppCtrlMrg AppCtrlPanel">
                     <div class="Right">
					    <span class="PanelTxt">Asset ID</span>&nbsp;<app:NumericTextBox ID="ContextTesterAssetIDTextbox" MaxLength="10" Width="40px" CssClass="formInput W40" runat="server"></app:NumericTextBox><asp:LinkButton ID="SwapTesterAssetLinkButton" OnClick="SwapTesterAssetLinkButton_Click" CssClass="BodyTxt" Text="swap" runat="server" />
                     </div>

                     <app:AssetPreviewTester runat="server" UseMarginCssClass="false" ID="ContextTesterAssetPreview" />

                     <div style="margin-left:10px;margin-top:10px;">
					    <asp:Button ID="ContextTesterEditButton" CssClass="button" Text="edit" OnClick="ContextTesterEditButton_Click" runat="server" />
                        &nbsp;<asp:Button ID="ContextTesterCancelButton" CssClass="button" Text="cancel" OnClick="ContextTesterCancelButton_Click" runat="server" />             
				    </div>
                 </div>                        
            </asp:PlaceHolder>
		</div>
		<div class="Right AdmRightPanel"></div>
		<div class="Clear"><br /></div>
	</div>
	<br />

</asp:Content>
