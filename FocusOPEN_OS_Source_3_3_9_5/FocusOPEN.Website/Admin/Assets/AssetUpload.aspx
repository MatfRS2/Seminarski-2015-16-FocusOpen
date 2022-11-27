<%@ Page Title="Upload Assets [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="AssetUpload.aspx.cs" Inherits="FocusOPEN.Website.Admin.AssetUpload" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="AdmBkg">
        <div class="Left AdmLeftPanel">
            <div class="AdmAppHdr">
                Upload Assets</div>
            <br />
            <br />
            <asp:Panel ID="FormPanel" runat="server">
                <div class="AppCtrlMrg">
                    <span class="BodyTxt Bold">Upload assets.</span>
                    <br />
                    <span class="BodyTxt">Many files of the same type can be uploaded at once if zipped. |</span>
                    <app:HelpHyperLink ID="HelpHyperLink1" HelpPage="8_Manage_Assets.htm#batch_uploads" CssClass="BodyTxt" Text="Uploading zip files" runat="server" /><br />
                    <span class="BodyTxt">Note: fields marked <span class="ReqField">*</span> are required</span>
                    <span class="BodyTxt">|</span> <a href="AssetsInSession.aspx" class="BodyTxt">view assets to be catalogued</a> <span class="BodyTxt">|</span> <a href="AssetList.aspx" class="BodyTxt">view all assets</a> <span class="BodyTxt">|</span>
                    <app:HelpHyperLink ID="HelpHyperLink2" HelpPage="8_Manage_Assets.htm#uploading_assets" CssClass="BodyTxt" Text="help" runat="server" />
                </div>
                <div class="AppCtrlMrg AppCtrlPanel W520">
                    <app:FeedbackLabel ID="MessageLabel1" LineBreaks="1" runat="server" />
                    <table width="470" border="0" cellspacing="0" cellpadding="2" style="table-layout: fixed">
                        <tr>
                            <td class="W100 alignRight PanelTxt">
                                <span class="Bold">Select File</span> <span class="ReqField">*</span>
                            </td>
                            <td>
                                <upload:InputFile ID="AssetFileUpload" CssClass="formInput W150" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <aspajax:UpdatePanel ID="UpdatePanel1" ChildrenAsTriggers="true" runat="server">
                        <ContentTemplate>
                            <table width="470" border="0" cellspacing="0" cellpadding="2" style="table-layout: fixed">
                                <tr>
                                    <td class="W100 alignRight PanelTxt">
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="ToggleExtendedOptionsLinkButton" OnClick="ToggleExtendedOptionsLinkButton_Click" CssClass="Black Bold BodyTxt" Text="[+] show extended upload options" runat="server" />
                                    </td>
                                </tr>
                                <asp:Panel ID="ExtendedOptionsPanel" runat="server" Visible="false">
                                    <tr>
                                        <td class="W100 alignRight PanelTxt">
                                            <span class="Bold">Asset Type</span>
                                        </td>
                                        <td>
                                            <app:AssetTypeDropDownList ID="AssetTypeDropDownList" CssClass="formInput W200" OmitBlankItem="true" ShowSystemChooseOption="true" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                    <td></td>
                                        <td>
                                            <asp:Panel runat="server">
												<asp:CheckBox ID="PreserveZipFileCheckBox" runat="server" /><span class="BaseLine BodyTxt">not a batch (preserve zip as generic file - only applies to zip files)</span>
												<br />    
												<asp:CheckBox ID="DoNotSubmitForPreviewCheckBox" runat="server" /><span class="BaseLine BodyTxt">do not submit assets for preview processing</span>
											</asp:Panel>
                                        </td>
                                    </tr>
                                    <div id="AssetFileSourceDiv" visible="false" runat="server">
                                    <tr>
                                        <td class="W100 alignRight PanelTxt">
                                            <span class="Bold">Asset file source</span>
                                        </td>
                                        <td class="alignLeft">
                                            <app:AssetSourcePathDropDownList ID="AssetSourcePathDropDownList" CssClass="formInput W200" OmitBlankItem="true" runat="server" OnSelectedIndexChanged="AssetSourcePathDropDownList_SelectedIndexChanged" AutoPostBack="true" />
											<asp:TextBox runat="server" ID="AssetSourceSubfolderTextBox" CssClass="formInput W100" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Panel runat="server">
                                                <div class="UpdSubCkMrg">
                                                    <asp:RadioButtonList runat="server" ID="MoveOrCopyButtonList" RepeatDirection="Horizontal" CssClass="BaseLine BodyTxt" >
                                                        <asp:ListItem Text="Move" Value="M" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Copy" Value="C"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    </div>
                                    <tr>
                                        <td class="W100 alignRight PanelTxt">
                                            <span class="Bold">Target category</span>
                                        </td>
                                        <td class="alignLeft">
                                            <app:NestedDataCategoryDropDownList ID="CategoryDropDownList" CssClass="formInput W200" IncludeSpecifyDuringCataloguing="true" OmitBlankItem="true" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="UpdSubCkMrg">
                                                <asp:CheckBox ID="SubFolderCheckBox" runat="server" /><span class="BaseLine BodyTxt">automatically generate categories from sub-folders</span>
                                            </div>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </table>
                        </ContentTemplate>
                    </aspajax:UpdatePanel>
                    <table width="470" border="0" cellspacing="0" cellpadding="2" style="table-layout: fixed">
                        <tr>
                            <td>
                                <div class="UpdSubCkMrg">
                                    <asp:CheckBox ID="NotifyWhenProcessedCheckBox" runat="server" /><span class="BaseLine BodyTxt">email me when video previews have been processed</span>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div id="ProgressBarWrapper" style="display: none">
                        <div class="HozLine">
                        </div>
                        <upload:ProgressBar ID="UploadProgressBar" Width="100%" Height="40px" runat="server" Inline="true" />
                    </div>
                    <div class="HozLine">
                    </div>
                    <app:IconImage ID="IconImage1" Filename="fbkNegative.gif" AlternateText="[exclamation]" runat="server" />
                    <span class="NegTxt"><span class="Bold">Copyright Warning:</span> Please ensure that the copyright and usage rights are available before uploading your assets. All copyright, usage and download rights must be specified before assets can be published to the system.<br /></span>
                    <div class="alignRight"><asp:Button ID="UploadButton" Text="upload" OnClick="UploadButton_Click" CssClass="uploadbutton button" runat="server" /></div>
                </div>
            </asp:Panel>
            <asp:Panel ID="SuccessPanel" Visible="false" runat="server">
                <div class="AppCtrlMrg" style="margin-bottom: 20px;">
                    <app:FeedbackLabel ID="SingleUploadSuccessInfoLabel" runat="server" />
                    <asp:Repeater ID="UploadedFileListRepeater" OnItemDataBound="UploadedFileListRepeater_ItemDataBound" runat="server">
                        <ItemTemplate>
                            <app:FeedbackLabel ID="UploadedFileInfoFeedbackLabel" runat="server" />
                            <br />
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div class="AppCtrlMrg">
                    <span class="BodyTxt Bold">What do you want to do now?</span>
                    <ul>
                        <li><asp:HyperLink ID="CatalogueAssetsHyperLink" NavigateUrl="AssetsInSession.aspx?action=autoselect" CssClass="BodyTxt" runat="server">View assets to be catalogued</asp:HyperLink></li>
                        <li><a class="BodyTxt" href="AssetList.aspx">View all assets</a></li>
                        <li><a class="BodyTxt" href="AssetUpload.aspx">Upload asset(s)</a></li>
                    </ul>
                </div>
            </asp:Panel>
        </div>
        <div class="Right AdmRightPanel"></div>
        <div class="Clear"><br /></div>
    </div>
    <br />
</asp:Content>
