<%@ Page Title="Edit Asset [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="AssetForm.aspx.cs" Inherits="FocusOPEN.Website.Admin.AssetForm" MaintainScrollPositionOnPostback="true" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="aspajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <script type="text/javascript" src="<%=ResolveUrl("~/Includes/Javascript/jQuery/jquery.tinysort.min.js")%>"></script>
    
    <script type="text/javascript">
    
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(pageLoaded);

        function pageLoaded() {
        	$("div.TreeViewContainer input:checkbox").click(function() {
        		var href = $(this).next("a").attr("href");
        		eval(href);
        	});
        	
        	setupDatePicker();
        }

        $(document).ready(function() {

            SortMetas();

            // Setup toggle metadata link
            $("#ToggleAssetFileMetadataLink").click(function() {
                var container = $("#AssetFileMetadataTable");
                container.toggle();
                $(this).html(container.is(":visible") ? "Hide asset file metadata [-]" : "Show asset file metadata [+]");
            });

            // Clear first attached file
            $("#ClearAttachedFile_1").click(function() {
                if (confirm("Are you sure? File will not be uploaded"))
                    $("#AttachedFile_1_Wrapper").html($("#AttachedFile_1_Wrapper").html());
                return false;
            });

            // Add file input when button is clicked
            $("#AddAttachedFileButton").click(function() {
                var inputCount = $(".AttachedFileWrapper").find(".FileWrapper").length;
                var lastInput = $(".AttachedFileWrapper").find(".FileWrapper").filter(":last");
                lastInput.after("<div class='FileWrapper' style='clear:both'><input id='AttachedFile_" + (inputCount + 1) + "' name='AttachedFile_" + (inputCount + 1) + "' class='formInput' type='file'/><input class='DeleteButton' value='Delete' type='button' style='font-size:11px'/></div>");
            });

            // Remove file input when delete button is clicked
            $(".AttachedFileWrapper").find(".DeleteButton").live('click', function() {
                $(this).parent().remove();
            });

            // Ensure video player previews are centered
            if ($("#AssetPreviewContainer").find("a").length == 1) {
                $("#AssetPreviewContainer").attr("style", "padding-left:105px;padding-bottom:10px");
            }

            // Change button text while ajax callback happens
            $(".AddLinkedAssetButton").live('click', function() {
                $(this).val("Please wait...");
            });

            // Remove the row before the ajax callback removes it to
            // make delete feel instant, while the server processes
            // things on the back-end.
            $(".DeleteLinkedAssetButton").live('click', function() {

                if (!confirm("Are you sure you want to delete the link relationship to this asset?"))
                    return false;

                // Use add button to show status
                $(".AddLinkedAssetButton").val("Deleting... Please wait").attr("disabled", "disabled");
            });

        });

        function SortMetas() {
            if ('<%= metadataSortingOn %>'.toLowerCase() == 'true') {
                $("ul#theUl>li").tsort({ attr: "class" });
                $('#loadingContainer').fadeOut(300, function() {
                    $('#containerX').fadeIn(300);
                });
            }
        }
	</script>
	
	<div class="AdmBkg">
        <div class="Left W550">
            <div id="HeaderTextDiv" runat="server" class="AdmAppHdr">Edit Asset</div>
            <br />
            <asp:PlaceHolder ID="AssetFormPlaceHolder" runat="server">
                <app:FeedbackLabel ID="MessageLabel1" UseContainer="true" runat="server" />
                <br />
                <div class="AppCtrlMrg">
                    <asp:PlaceHolder ID="EditAssetHeaderPlaceHolder" runat="server"><span class="BodyTxt">
                        <span class="Bold">Edit asset details below.</span> Note: fields marked <span class="ReqField">*</span> are required.</span><br />
                        <span class="BodyTxt">Roll over field titles for a explanation of each.</span> <span class="BodyTxt">|</span> <a href="AssetList.aspx" class="BodyTxt">view asset list</a>
                        <span class="BodyTxt">|</span>
                        <app:HelpHyperLink ID="HelpHyperLink3" HelpPage="8_Manage_Assets.htm#editing_assets" CssClass="BodyTxt" Text="help" runat="server" />
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="CatalogueAssetHeaderPlaceHolder" Visible="false" runat="server">
                        <span class="BodyTxt Bold">Complete the following information for this asset.<br /></span>
                        <span class="BodyTxt">Note: fields marked <span class="ReqField">*</span> are required</span>. <span class="BodyTxt">Roll over field titles for a explanation of each.</span>
                        <span class="BodyTxt">|</span>
                        <app:HelpHyperLink ID="HelpHyperLink1" HelpPage="8_Manage_Assets.htm#cataloguing_assets2" CssClass="BodyTxt" Text="help" runat="server" />
                    </asp:PlaceHolder>
                </div>
                <div class="AppCtrlMrg AppCtrlPanel W550">
                    <div class="alignCentre">
						<div id="AssetPreviewContainer" style="margin-bottom:10px;">
							<app:AssetPreview ID="AssetPreview1" UseMarginCssClass="false" runat="Server" />
                        </div>
                        <div class="BodyTxt">
							<app:EnhancedHyperLink ID="DownloadAssetFileHyperLink" Prefix="" Suffix="" CssClass="BodyTxt" Text="download original" runat="server"/>
							<app:EnhancedLinkButton ID="ReplaceAssetLinkButton" Prefix=" | " Suffix="" CssClass="BodyTxt" OnClick="ReplaceAssetLinkButton_Click" Text="replace asset file" runat="server" />
							<app:EnhancedHyperLink ID="AdjustAssetHyperLink1" Prefix=" | " Suffix="" CssClass="BodyTxt adjustAssetLink" OnClick="return confirm('Any unsaved changes to the asset metadata will be lost. Are you sure you want to continue?')" runat="server">change preview settings</app:EnhancedHyperLink>
							<app:EnhancedLinkButton ID="DeleteAssetLinkButton" Prefix=" | " Suffix="" OnClick="DeleteAssetLinkButton_Click" OnClientClick="return confirm('Are you sure you want to delete this asset? This action cannot be undone')" CssClass="BodyTxt" Text="delete asset" runat="server" />
							<app:EnhancedHyperLink ID="ViewAssetAuditLogHyperLink" CssClass="BodyTxt" Prefix=" | " Suffix="" NavigateUrl="#" Text="audit log" runat="server" />
                        </div>
                        <br />
                        <div id="AdjustAssetWarningContainer" visible="false" class="alignCentre" runat="server">
                            <app:IconImage Filename="fbkNegative.gif" AlternateText="[exclamation]" runat="server"/>
                            <span class="NegTxt"><span class="Bold">Warning:</span> It may not be possible to automatically generate a preview and thumbnail for this file type.<br />
                            Select <asp:HyperLink ID="AdjustAssetHyperLink2" CssClass="Red adjustAssetLink" runat="server">adjust this asset</asp:HyperLink> to upload your own. </span>
                            <br />
                        </div>
                    </div>
                    <asp:Panel ID="ReplaceFilePanelWrapper" runat="server">
                        <asp:Panel ID="ReplaceFilePanel" Visible="false" runat="server">
                            <br />
                            <div class="alignLeft W470">
                                <span class="BodyTxt Bold">Select a file to replace the current asset with</span><br />
                                <span class="BodyTxt">Note: You must select file of the same type as the original asset.</span>
                            </div>
                            <div class="AppCtrlPanel">
                                <app:FeedbackLabel ID="ReplaceFileMessageLabel" LineBreaks="1" runat="server" />
                                <table width="470" border="0" cellspacing="0" cellpadding="2">
                                    <tr>
                                        <td class="W100 alignRight PanelTxt"><span class="Bold">Select File</span></td>
                                        <td>
											<Upload:InputFile ID="NewAssetFileUpload" CssClass="formInput W300" runat="server" />
											<Upload:UnloadConfirmer ID="AssetFileUploadUnloadConfirmer" Text="There appears to be a file upload in progress.  Leaving this page will cancel the upload." runat="server" />
										</td>
                                    </tr>
                                </table>
                                <asp:Panel ID="ReplaceNotifyPanel" runat="server">
                                    <div class="UpdSubCkMrg">
                                        <asp:CheckBox ID="NotifyWhenProcessedCheckBox" CssClass="videoNotify" runat="server" />
                                        <span class="BaseLine BodyTxt">email me when video preview has been processed</span>
                                    </div>
                                </asp:Panel>
                                <div id="ProgressBarWrapper" style="display:none">
									<div class="HozLine"></div>
									<Upload:ProgressBar id="UploadProgressBar" Width="100%" Height="40px" runat="server" inline="true" />
								</div>
                                <div class="HozLine"></div>
                                <app:IconImage ID="IconImage1" Filename="fbkNegative.gif" AlternateText="[exclamation]" runat="server"/>
                                <span class="NegTxt"><span class="Bold">Copyright Warning:</span> Please ensure that
                                    the copyright and usage rights are available before uploading your assets. All copyright,
                                    usage and download rights must be specified before assets can be published to the
                                    system.<br />
                                </span>
                                <div class="alignRight">
                                    <asp:Button ID="ReplaceFileButton" Text="replace" CssClass="uploadButton button" OnClick="ReplaceFileButton_Click" runat="server" />
                                    &nbsp;
                                    <asp:Button ID="CancelFileUploadButton" Text="cancel" CssClass="cancelButton button" OnClick="CancelFileUploadButton_Click" runat="server" />
                                </div>
                            </div>
                        </asp:Panel>
                    </asp:Panel>
                    <div class="CatHozLine"></div>
                    <table width="550" border="0" cellspacing="0" cellpadding="2">
                        <tr>
                            <td class="W150 alignRight"><a href="#" title="The reference number for the asset" class="PanelTxt Bold">Asset Reference:</a></td>
                            <td class="PanelTxt Bold"><asp:Label ID="AssetIdLabel" Text="[AssetId]" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="W150 alignRight"><app:BrandAdminMetadataLabel BrandDropDownListId="BrandDropDownList1" FieldName="DateUploaded" ShowRequiredFlag="false" runat="server" /></td>
                            <td class="PanelTxt Bold"><asp:Label ID="UploadDateLabel" Text="[UploadDate]" runat="server" /></td>
                        </tr>
                        <tr id="DimensionsRow" runat="server">
                            <td class="W150 alignRight"><a href="#" title="The width and height of the asset in pixels" class="PanelTxt Bold">Dimensions (pixels):</a></td>
                            <td><asp:Label ID="DimensionsLabel" CssClass="PanelTxt Bold" Text="[Dimensions]" runat="server" /></td>
                        </tr>
                        <tr id="OrientationRow" runat="server">
                            <td class="W150 alignRight"><a href="#" title="The orientation of the asset (landscape, portrait or square)" class="PanelTxt Bold">Orientation:</a></td>
                            <td><asp:Label ID="OrientationLabel" CssClass="PanelTxt Bold" Text="[Orientation]" runat="server" /></td>
                        </tr>
                        <tr id="DurationRow" runat="server">
                            <td class="W150 alignRight"><a href="#" title="The running time of the asset" class="PanelTxt Bold">Duration:</a></td>
                            <td><asp:Label ID="DurationLabel" CssClass="PanelTxt Bold" Text="[Duration]" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="W150 alignRight"><app:BrandAdminMetadataLabel BrandDropDownListId="BrandDropDownList1" FieldName="Filename" ShowRequiredFlag="false" runat="server" /></td>
                            <td class="PanelTxt Bold"><asp:Label ID="FilenameLabel" runat="server" /></td>
                        </tr>
                        <tr id="FileHashRow" runat="server">
                            <td class="W150 alignRight"><app:BrandAdminMetadataLabel BrandDropDownListId="BrandDropDownList1" FieldName="FileHash" ShowRequiredFlag="false" runat="server" /></td>
                            <td class="PanelTxt Bold"><asp:Label ID="FileHashLabel" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="W150 alignRight"><app:BrandAdminMetadataLabel BrandDropDownListId="BrandDropDownList1" FieldName="FileSize" ShowRequiredFlag="false" runat="server" /></td>
                            <td class="PanelTxt Bold"><asp:Label ID="SizeLabel" Text="[Size]" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="W150 alignRight"><a href="#" title="Whether or not this asset is used as a template for assets" class="PanelTxt Bold">Template Asset:</a></td>
                            <td class="PanelTxt Bold"><asp:Label ID="TemplateIdLabel" Text="[TemplateAssetId]" runat="server" /></td>
                        </tr>
                    </table>
                    <div class="CatHozLine"></div>
                    <app:MetadataVisibilityWrapper FieldName="ShowFileMetadata" Section="AssetForm" runat="server">
	                    <div id="NoAssetFileMetadataWrapper" runat="server" class="alignCentre"><a href="#" class="BodyTxt Bold">No asset file metadata is available</a></div>
						<asp:Panel ID="AssetFileMetadataPanel" runat="server">
							<div class="alignCentre"><a id="ToggleAssetFileMetadataLink" href="#" class="BodyTxt Bold">View asset file metadata [+]</a></div>
							<asp:Repeater ID="AssetFileMetadataRepeater" OnItemDataBound="AssetFileMetadataRepeater_ItemDataBound" runat="server">
								<HeaderTemplate>
									<table id="AssetFileMetadataTable" width="550" border="0" cellspacing="0" cellpadding="2" style="display:none">
								</HeaderTemplate>
								<FooterTemplate>
									</table>
								</FooterTemplate>
								<ItemTemplate>
									<tr>
										<td class="W150 alignRight alignTop"><a href="#" class="PanelTxt Bold"><asp:Literal ID="MetadataNameLiteral" runat="server" /></td>
										<td class="PanelTxt Bold alignTop"><asp:Literal ID="MetadataValueLiteral" runat="server" /></td>
									</tr>
								</ItemTemplate>
							</asp:Repeater>
						</asp:Panel>
	                    <div class="CatHozLine"></div>
					</app:MetadataVisibilityWrapper>
                    <table width="550" border="0" cellspacing="0" cellpadding="2">
                        <tr id="WorkflowSelectorRow" runat="server">
                            <td class="W150 alignRight"><a href="#" class="PanelTxt Bold">Workflow</a> <span class="ReqField">*</span></td>
                            <td><app:WorkflowDropDownList ID="WorkflowDropDownList" CssClass="formInput W225" OmitBlankItem="false" BlankText="None" BlankValue="0" runat="server" /></td>
                        </tr>
                    </table>
					<div id="loadingContainer" style="margin: 10px; <%= !metadataSortingOn ? "display: none;" : "" %>">
                        <img src="<%=ResolveUrl("~/Images/Icons/ajax-loader.gif")%>" alt="sorting" /> sorting metadata inputs...
	                </div>
					<div id="containerX" style="<%= metadataSortingOn ? "display: none;" : "" %>">
	                    <ul id="theUl">
		                    <li class="orderLi" runat="server" id="LiAssetType">
								<table>
									<tr>
										<td class="W150 alignRight"><app:BrandAdminMetadataLabel BrandDropDownListId="BrandDropDownList1" FieldName="AssetType" ShowRequiredFlag="true" runat="server" /></td>
										<td><app:AssetTypeDropDownList ID="AssetTypeDropDownList1" CssClass="formInput W225" OmitBlankItem="true" runat="server" /></td>
									</tr>
								</table>
							</li>
		                    <app:MetadataVisibilityWrapper BrandDropDownListId="BrandDropDownList1" FieldName="DateProduced" runat="server">
								<li class="orderLi" runat="server" id="LiDateProduced">
									<table>
									<tr>
										<td class="W150 alignRight"><app:BrandAdminMetadataLabel BrandDropDownListId="BrandDropDownList1" FieldName="DateProduced" runat="server" /></td>
										<td>
											<app:DayDropDownList ID="ProductionDayDropDownList" CssClass="formInput W67" OmitBlankItem="false" BlankText="Unknown" BlankValue="0" runat="server" />
											<app:MonthDropDownList ID="ProductionMonthDropDownList" CssClass="formInput W75" OmitBlankItem="false" BlankText="Unknown" BlankValue="0" runat="server" />
											<app:YearDropDownList ID="ProductionYearDropDownList" CssClass="formInput W75" OmitBlankItem="false" BlankText="Unknown" BlankValue="0" runat="server" />
										</td>
			                        </tr>
									</table>
								</li>
							</app:MetadataVisibilityWrapper>
	                        <app:MetadataVisibilityWrapper BrandDropDownListId="BrandDropDownList1" FieldName="Originator" runat="server">
								<li class="orderLi" runat="server" id="LiOriginator">
									<table>
									<tr>
										<td class="W150 alignRight">
											<app:BrandAdminMetadataLabel FieldName="Originator" runat="server" />
										</td>
										<td>
											<asp:TextBox ID="OriginatorTextBox" CssClass="formInput W225" MaxLength="150" runat="server" />
										</td>
									</tr>
									</table>
								</li>
							</app:MetadataVisibilityWrapper>
	                        <app:MetadataVisibilityWrapper BrandDropDownListId="BrandDropDownList1" FieldName="Title" Section="AssetForm" runat="server">
								<li class="orderLi" runat="server" id="LiTitle">
									<table>
										<tr>
											<td class="W150 alignRight">
												<app:BrandAdminMetadataLabel BrandDropDownListId="BrandDropDownList1" FieldName="Title" runat="server" />
												<br />
											</td>
											<td>
												<asp:TextBox ID="TitleTextBox" CssClass="formInput W225" MaxLength="255" runat="server" />
											</td>
										</tr>
									</table>
								</li>
							</app:MetadataVisibilityWrapper>
							<app:MetadataVisibilityWrapper BrandDropDownListId="BrandDropDownList1" FieldName="ProjectCode" Section="AssetForm" runat="server">
								<li class="orderLi" runat="server" id="LiProjectCode">
									<table>
										<tr>
											<td class="W150 alignRight">
												<app:BrandAdminMetadataLabel BrandDropDownListId="BrandDropDownList1" FieldName="ProjectCode" runat="server" />
											</td>
											<td>
												<asp:TextBox ID="ProjectCodeTextBox" CssClass="formInput W225" MaxLength="100" runat="server" />
											</td>
										</tr>
									</table>
								</li>
							</app:MetadataVisibilityWrapper>
							<!-- Brand Selector DropDownList -->
						   <li class="orderLi" runat="server" id="LiBrand">
								<table>
									<tr id="BrandSelectorWrapper" runat="server">
										<td class="W150 alignRight Top">
											<app:BrandAdminMetadataLabel BrandDropDownListId="BrandDropDownList1" FieldName="Brand" runat="server" />
										</td>
										<td>
											<app:BrandDropDownList ID="BrandDropDownList1" Restrict="true" OmitBlankItem="true" CssClass="formInput W225" AutoPostBack="true" OnSelectedIndexChanged="BrandDropDownList1_SelectedIndexChanged" runat="server" />
										</td>
									</tr>
								</table>
							</li>
							<asp:Repeater ID="MetadataRepeater" OnItemDataBound="MetadataRepeater_ItemDataBound" runat="server" EnableViewState="true">
								<ItemTemplate>
									 <app:MetadataVisibilityWrapper ID="MetadataVisibilityWrapper" BrandDropDownListId="BrandDropDownList1" Section="AssetForm" runat="server">
											<li class="orderLi" runat="server" id="LiMetaRepeater">
											<table>
											<tr>
												<td class="W150 alignRight Top">
													<!--Metadata 1-->
													<app:BrandAdminMetadataLabel ID="BrandAdminMetadataLabel" BrandDropDownListId="BrandDropDownList1" runat="server" />
												</td>
												<td>
													<app:MetadataInputWrapper ID="MetadataInput" runat="server" />
												</td>
											</tr>
											 </table>
											<asp:HiddenField ID="FieldIdHidden" runat="server" /> 
											</li>
									 </app:MetadataVisibilityWrapper>
								</ItemTemplate>
							</asp:Repeater>
							<li class="orderLi" runat="server" id="LiCats">
								<aspajax:UpdatePanel ID="CategoriesUpdatePanel" runat="server" RenderMode="Block">
								<ContentTemplate>
                    				<table width="550" border="0" cellspacing="0" cellpadding="2">
										<tr>
											<td class="W150 alignRight Top">
												<a href="#" title="Select categories to associate" class="PanelTxt Bold">Categories</a> <span class="ReqField">*</span>
												<br />
												<span class="PanelTxt">(Choose at least one category to file this asset under)</span>
											</td>
											<td valign="top" align="left">
												<div class="TreeViewContainer">
													<app:CategoriesTreeView ID="CategoriesTreeView" BindingMode="Manual" ShowCheckBoxes="All" NodeStyle-CssClass="PanelTxt" OnTreeNodeCheckChanged="CategoriesTreeView_TreeNodeCheckChanged" runat="server"/>
												</div>
											</td>
										</tr>
										<tr>
											<td class="W150 alignRight Top">&nbsp;</td>
											<td>
												<span class="BodyTxt">Selected categories</span><br />
												<asp:ListBox runat="server" ID="CategoriesListBox" CssClass="formInput W225"/>
											</td>
										</tr>
										<tr>
											<td class="W150 alignRight Top">&nbsp;</td>
											<td>
												<asp:Button ID="MakePrimaryCategoryButton"  OnClick="MakePrimaryCategoryButton_Click" CssClass="button" Text="make primary" runat="server" />
												&nbsp;
												<asp:Button ID="RemoveCategoryButton" OnClick="RemoveCategoryButton_Click" CssClass="button" Text="remove" runat="server" />
											</td>
										</tr>
										<tr>
											<td class="W150 alignRight Top">
												<app:BrandAdminMetadataLabel BrandDropDownListId="BrandDropDownList1" FieldName="PrimaryCategory" runat="server" />
												<br />
											</td>
											<td>
												<asp:TextBox ID="PrimaryCategoryTextBox" runat="server" CssClass="formInput W225" Enabled="false"/>
												<asp:HiddenField ID="PrimaryCategoryIdHiddenField" runat="server" />
											</td>
										</tr>
									</table>
			                    </ContentTemplate>
							</aspajax:UpdatePanel>
		                    <div class="CatHozLine"></div>
	                    </li>
	                    <app:MetadataVisibilityWrapper BrandDropDownListId="BrandDropDownList1" FieldName="ShortDescription" Section="AssetForm" runat="server">
							<li class="orderLi" runat="server" id="LiShortDescription">
								<table>
								<tr>
									<td class="W150 alignRight Top"><app:BrandAdminMetadataLabel BrandDropDownListId="BrandDropDownList1" FieldName="ShortDescription" runat="server" /></td>
									<td><app:TextArea ID="ShortDescriptionTextBox" Rows="5" CssClass="formInput W225" Wrap="true" MaxLength="400" runat="server" /></td>
								</tr>
								</table>
							</li>
                        </app:MetadataVisibilityWrapper>
                        <app:MetadataVisibilityWrapper BrandDropDownListId="BrandDropDownList1" FieldName="AdditionalKeywords" Section="AssetForm" runat="server">
							<li class="orderLi" runat="server" id="LiAdditionalKeywords">
								<table>
									<tr>
										<td class="W150 alignRight Top"><app:BrandAdminMetadataLabel BrandDropDownListId="BrandDropDownList1" FieldName="AdditionalKeywords" runat="server" /></td>
										<td>
											<span class="BodyTxt">Enter each term per line</span><br />
											<app:TextArea ID="KeywordsTextBox" Rows="5" CssClass="formInput W225" Wrap="true" MaxLength="1000" runat="server" />
										</td>
									</tr>
								</table>
							</li>
                        </app:MetadataVisibilityWrapper>
						<li class="orderLi" runat="server" id="LiCopyrightOwner">
		                    <div class="CatHozLine"></div>
							<span class="PanelTxt">Note: copyright and usage restrictions must be <span class="Bold">completed in full</span> before an asset can be published to the system | </span>
							<app:HelpHyperLink ID="HelpHyperLink2" HelpPage="8_Manage_Assets.htm#rights" CssClass="PanelTxt" Text="help" runat="server" /><br />
							<br />
	                        <app:MetadataVisibilityWrapper BrandDropDownListId="BrandDropDownList1" FieldName="CopyrightOwner" Section="AssetForm" runat="server">
								<table width="550" border="0" cellspacing="0" cellpadding="2">
								<tr>
									<td class="W150 alignRight Top"><app:BrandAdminMetadataLabel BrandDropDownListId="BrandDropDownList1" FieldName="CopyrightOwner" runat="server" /></td>
									<td>
										<span class="PanelTxt">
											<asp:RadioButtonList ID="CopyrightOwnerRadioButtonList" CssClass="PanelTxt" RepeatDirection="horizontal" runat="server">
												<asp:ListItem Text="[OrganisationName]" />
												<asp:ListItem Text="Other (Please specify below)" />
											</asp:RadioButtonList>
										</span>
										<asp:TextBox ID="CopyrightOwnerTextBox" CssClass="formInput W225" runat="server" />
										<script type="text/javascript">
											toggleCopyrightOwner(<%=(CopyrightOwnerRadioButtonList.SelectedIndex==0).ToString().ToLower()%>);
										</script>
									</td>
								</tr>
								</table>
							</app:MetadataVisibilityWrapper>
						 </li>
                        <app:MetadataVisibilityWrapper BrandDropDownListId="BrandDropDownList1" FieldName="UsageRestrictions" Section="AssetForm" runat="server">
							<li class="orderLi" runat="server" id="LiUsageRestrictions">
								<table>
								<tr>
									<td class="W150 alignRight Top"><app:BrandAdminMetadataLabel BrandDropDownListId="BrandDropDownList1" FieldName="UsageRestrictions" runat="server" /></td>
									<td><app:TextArea ID="UsageRestrictionsTextBox" CssClass="formInput W225" Rows="5" Wrap="true" MaxLength="1000" runat="server" /></td>
								</tr>
								</table>
							</li>
                        </app:MetadataVisibilityWrapper>
                        <app:MetadataVisibilityWrapper BrandDropDownListId="BrandDropDownList1" FieldName="AttachedFiles" Section="AssetForm" runat="server">
	                        <li class="orderLi" runat="server" id="LiAttachedFiles">
								<table>
								<tr>
									<td class="W150 alignRight Top PanelTxt"><app:BrandAdminMetadataLabel BrandDropDownListId="BrandDropDownList1" FieldName="AttachedFiles" runat="server" /></td>
									<td class="BodyTxt">
										<asp:Repeater ID="AttachedFileRepeater" OnItemDataBound="AttachedFileRepeater_ItemDataBound" runat="server">
											<HeaderTemplate>
												<table style="width:260px">
											</HeaderTemplate>
											<FooterTemplate>
												</table>
											</FooterTemplate>
											<ItemTemplate>
												<tr>
													<td class="BodyTxt"><asp:Label ID="FilenameLabel" CssClass="AttachedFile" runat="server" /></td>
													<td class="BodyTxt" nowrap="nowrap">
														<asp:LinkButton ID="DownloadAttachedFileLinkButton" OnCommand="DownloadAttachedFileLinkButton_Command" CssClass="BodyTxt" Text="download" runat="server" />
														|
														<app:PromptLinkButton ID="DeleteAttachedFileLinkButton" Prompt="Are you sure you want to delete this file?" OnCommand="DeleteAttachedFileLinkButton_Command" CssClass="BodyTxt" Text="delete" runat="server" />
													</td>
												</tr>
											</ItemTemplate>
										</asp:Repeater>
										<div id="AttachedFileWrapper" class="AttachedFileWrapper" runat="server">
											<div class="FileWrapper">
												<span id="AttachedFile_1_Wrapper">
													<input id="AttachedFile_1" name="AttachedFile_1" class="formInput" type="file" /><button id="ClearAttachedFile_1" style="font-size:11px">Clear</button>
												</span>
											</div>
											<br />
											<input id="AddAttachedFileButton" type="button" class="button" value="Add File" />
										</div>
										<br />
										<asp:CheckBox ID="RestrictAttachedFilesCheckBox" Text="Restrict Attached Files" CssClass="BodyTxt" runat="server" />
									</td>
								</tr>
								</table>
							</li>
						</app:MetadataVisibilityWrapper>
                        <app:MetadataVisibilityWrapper BrandDropDownListId="BrandDropDownList1" FieldName="LinkedAssets" Section="AssetForm" runat="server">
	                        <li class="orderLi" runat="server" id="LiLinkedAssets">
			                    <table>
		                        <tr>
				                    <td class="W150 alignRight Top PanelTxt"><app:BrandAdminMetadataLabel BrandDropDownListId="BrandDropDownList1" FieldName="LinkedAssets" runat="server" /></td>
									<td>
										<aspajax:UpdatePanel ID="LinkedAssetsRepeaterUpdatePanel" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:Repeater ID="LinkedAssetsRepeater" runat="server" OnItemDataBound="LinkedAssetsRepeater_ItemDataBound">
													<HeaderTemplate>
														<table id="LinkedAssetsTable" border="0" cellspacing="0" cellpadding="2">
													</HeaderTemplate>
													<ItemTemplate>
														<tr class="LinkedAssetRow">
															<td class="BodyTxt">
																<aspajax:UpdatePanel runat="server" ID="UpdatePanel1">
																	<ContentTemplate>
																		<asp:TextBox ID="LinkedAssetIdTextBox" runat="server" MaxLength="7" CssClass="formInput W50" Wrap="true" OnTextChanged="LinkedAssetIdTextBox_TextChanged" AutoPostBack="true"/>
																		<asp:PlaceHolder ID="LinkedAssetActionLinks" Visible="false" runat="server">
																			&nbsp;
																			<asp:HyperLink ID="LinkedAssetNameLinkButton" runat="server" CssClass="formInput"/>
																			&nbsp;
																			[<asp:HyperLink ID="LinkedAssetViewHyperLink" runat="server" CssClass="formInput" Text="view" /> | <asp:LinkButton ID="LinkedAssetDeleteLinkButton" runat="server" CssClass="DeleteLinkedAssetButton formInput" Prompt="Are you sure you want to delete this linked asset?" Text="delete" OnCommand="LinkedAssetDeleteLinkButton_Command"/>]
																		</asp:PlaceHolder>
																		&nbsp;
																		<app:FeedbackLabel ID="InvalidAssetIdMessage" Pinned="true" Visible="false" runat="server" />
																	</ContentTemplate>
																	<Triggers>
																		<aspajax:AsyncPostBackTrigger ControlID="LinkedAssetIdTextBox" EventName="TextChanged" />
																		<aspajax:AsyncPostBackTrigger ControlID="LinkedAssetDeleteLinkButton" />
																	</Triggers>
																</aspajax:UpdatePanel>
															</td>
														</tr>
													</ItemTemplate>
													<FooterTemplate>
														</table>
													</FooterTemplate>
												</asp:Repeater>
												<asp:Button runat="server" ID="AddLinkedAssetButton" CssClass="AddLinkedAssetButton button" Text="Add Linked Asset" OnClick="AddLinkedAssetButton_Click" />
											</ContentTemplate>
										</aspajax:UpdatePanel>
									</td>
								</tr>
								</table>
							</li>
						</app:MetadataVisibilityWrapper>
                        <app:MetadataVisibilityWrapper BrandDropDownListId="BrandDropDownList1" FieldName="DownloadRestrictions" Section="AssetForm" runat="server">
	                        <li class="orderLi" runat="server" id="LiDownloadRestrictions">
								<table>
								<tr>
									<td class="W150 alignRight Top">
										<a href="#" class="PanelTxt Bold">Watermark Preview</a>
										<br />
									</td>
									<td class="PanelTxt">
										<asp:CheckBox ID="WatermarkPreviewCheckBox" runat="server" />
									</td>
								</tr>
								<%-------------------------------------------------------------------------------------%>
								<%-- Internal Users Restrictions ------------------------------------------------------%>
								<%-------------------------------------------------------------------------------------%>
								<tr>
									<td class="W150 alignRight Top">
										<a class="PanelTxt Bold" href="#">Internal Users</a>
										<br />
									</td>
									<td class="PanelTxt">
										<asp:RadioButtonList ID="InternalUsersRestrictionsRadioButtonList" CssClass="BodyTxt" RepeatDirection="Vertical" runat="server">
											<asp:ListItem Value="None" Text="No download restrictions" Selected="True" />
											<asp:ListItem Value="NotVisible" Text="Not Visible" />
											<asp:ListItem Value="ApprovalRequired" Text="Approval required to download this asset" />
										</asp:RadioButtonList>
									</td>
								</tr>

								<%-------------------------------------------------------------------------------------%>
								<%-- External Users Restrictions ------------------------------------------------------%>
								<%-------------------------------------------------------------------------------------%>
								<tr>
									<td class="W150 alignRight Top">
										<a class="PanelTxt Bold" href="#">External Users</a>
										<br />
									</td>
									<td class="PanelTxt">
										<asp:RadioButtonList ID="ExternalUsersRestrictionsRadioButtonList" CssClass="BodyTxt" RepeatDirection="Vertical" runat="server">
											<asp:ListItem Value="None" Text="No download restrictions" Selected="True" />
											<asp:ListItem Value="NotVisible" Text="Not Visible" />
											<asp:ListItem Value="ApprovalRequired" Text="Approval required to download this asset" />
										</asp:RadioButtonList>
									</td>
								</tr>
								</table>
							</li>
                        </app:MetadataVisibilityWrapper>
                        <app:MetadataVisibilityWrapper BrandDropDownListId="BrandDropDownList1" runat="server" FieldName="ContactEmail" Section="AssetForm">
	                        <li class="orderLi" runat="server" id="LiContactEmail">
								<table>
								<tr>
									<td class="W150 alignRight Top"><app:BrandAdminMetadataLabel BrandDropDownListId="BrandDropDownList1" FieldName="ContactEmail" runat="server" /></td>
									<td>
										<asp:TextBox ID="ContactEmailTextBox" CssClass="formInput W225" runat="server" /><br />
										<span class="Bold PanelTxt">Note: changing the contact email will delegate the asset to this user</span>
									</td>
								</tr>
								</table>
								<div class="CatHozLine"></div>
							</li>
                        </app:MetadataVisibilityWrapper>
						<app:MetadataVisibilityWrapper BrandDropDownListId="BrandDropDownList1" FieldName="PublicationDate" Section="AssetForm" runat="server">
							<li class="orderLi" runat="server" id="LiPublicationDate">
								<table>	
									<tr>
										<td class="W150 alignRight Top"><app:BrandAdminMetadataLabel BrandDropDownListId="BrandDropDownList1" FieldName="PublicationDate" runat="server" /></td>
										<td><app:DatePicker ID="PublicationDatePicker" AllowDateBeforeToday="true" runat="server" /></td>
									</tr>
								</table>
							</li>
						</app:MetadataVisibilityWrapper>
						<app:MetadataVisibilityWrapper BrandDropDownListId="BrandDropDownList1" FieldName="ExpiryDate" Section="AssetForm" runat="server">
							<li class="orderLi" runat="server" id="LiExpiryDate">
								<aspajax:UpdatePanel ID="DateUpdatePanel" ChildrenAsTriggers="true" RenderMode="Inline" UpdateMode="Always" runat="server">
								<ContentTemplate>
									<table>
									<tr>
										<td class="W150 alignRight Top">
											<a href="#" title="Specifies if the asset has an expiry date" class="PanelTxt Bold">
												Asset Expires</a>
											<br />
										</td>
										<td>
											<asp:RadioButtonList ID="AssetExpiresRadioButtonList" RepeatDirection="Horizontal" CssClass="PanelTxt" AutoPostBack="true" OnSelectedIndexChanged="AssetExpiresRadioButtonList_SelectedIndexChanged" runat="server">
												<asp:ListItem Text="Yes" Value="true"/>
												<asp:ListItem Text="No" Value="false" Selected="True"/>
											</asp:RadioButtonList>
										</td>
									</tr>
									<tr>
										<td class="W150 alignRight Top"><app:BrandAdminMetadataLabel BrandDropDownListId="BrandDropDownList1" FieldName="ExpiryDate" runat="server" /></td>
										<td><app:DatePicker ID="ExpiryDatePicker" AllowDateBeforeToday="true" runat="server" /></td>
									</tr>
									 </table>
								</ContentTemplate>
								</aspajax:UpdatePanel>
							</li>
						</app:MetadataVisibilityWrapper>
					</ul>
					<div class="CatHozLine"></div>
					<asp:PlaceHolder ID="PublishWrapper" runat="server">
						<table width="550" border="0" cellspacing="0" cellpadding="2">
						<tr>
							<td class="W150 alignRight"><a href="#" class="PanelTxt Bold">Publish This Asset</a></td>
							<td class="PanelTxt Bold"><asp:CheckBox ID="IsPublishedCheckBox" runat="server" /></td>
						</tr>
						</table>
						<br />
					</asp:PlaceHolder>
                    <asp:PlaceHolder ID="ExternalPublishWrapper" runat="server">
						<table width="550" border="0" cellspacing="0" cellpadding="2">
						<tr>
							<td class="W150 alignRight"><a href="#" class="PanelTxt Bold">Publish this asset to externally accessible area</a></td>
							<td class="PanelTxt Bold">
								<asp:CheckBox ID="IsAssetPublishedToExternalFolderCheckBox" runat="server" />
								<asp:Label ID="IsAssetPublishedInfoLabel" CssClass="PanelTxt" Text="[dynamic]" runat="server" />
							</td>
						</tr>
						</table>
					</asp:PlaceHolder>
                    <asp:PlaceHolder ID="SubmitToWorkflowWrapper" runat="server">
                        <table width="550" border="0" cellspacing="0" cellpadding="2">
                            <tr>
                                <td class="W150 alignRight">
                                    <a href="#" class="PanelTxt Bold">Submit to workflow</a>
                                </td>
                                <td class="PanelTxt Bold">
                                    <asp:CheckBox ID="SubmitToWorkflowCheckbox" runat="server" />
                                </td>
                            </tr>
                        </table>
                        <br />
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="WorkflowInfoWrapper" runat="server">
                        <div class="alignCentre">
                            <app:AssetWorkflowLog ID="AssetWorkflowLog" runat="server" />
                        </div>
                        <br />
                    </asp:PlaceHolder>
				</div>
				<div class="CatHozLine"></div>
				<div class="alignCentre">
					<asp:Button ID="SaveAndPreviousButton" OnCommand="SaveButton_Command" CommandName="Previous" CssClass="button W150" Text="&laquo; save &amp; prev [AssetId]" runat="server" />
					<app:SpacerImage ID="spacer1" Width="2" runat="server" />
					<asp:Button ID="SaveAndNextAssetButton" OnCommand="SaveButton_Command" CommandName="Next" CssClass="button W150" Text="save &amp; next [AssetId] &raquo;" runat="server" />
					<app:SpacerImage ID="SpacerImage1" Width="2" runat="server" />
					<asp:Button ID="SaveAndExitButton" OnCommand="SaveButton_Command" CommandName="Exit" CssClass="button" Text="save &amp; exit" runat="server" />
					<app:SpacerImage ID="SpacerImage2" Width="2" runat="server" />
					<app:CancelButton ID="CancelButton" CssClass="button" Text="cancel" Prompt="Exit from the cataloguing system? Changes to the information above will not be saved" RedirectUrl="AssetList.aspx" runat="server" />
				</div>
			</div>
		</asp:PlaceHolder>
		<asp:PlaceHolder ID="SuccessPlaceHolder" Visible="false" runat="server">
			<div class="AppCtrlMrg">
				<app:FeedbackLabel ID="AssetSavedFeedbackLabel" UseContainer="false" LineBreaks="1" runat="server" />
				<span class="BodyTxt Bold">What do you want to do now?</span>
				<ul>
					<asp:PlaceHolder ID="ReEditAssetLinkWrapper" runat="server">
						<li><a class="BodyTxt" href="AssetForm.aspx?assetId=<%=FocusOPEN.Shared.WebUtils.GetIntRequestParam("assetId", 0)%>&source=<%=FocusOPEN.Shared.WebUtils.GetRequestParam("source", "unknown") %>">Edit the same asset again</a></li>
						<li><a class="BodyTxt" href="javascript:showAssetInfo(<%=FocusOPEN.Shared.WebUtils.GetIntRequestParam("assetId", 0)%>)">View asset info popup</a></li>
					</asp:PlaceHolder>
					<li><a class="BodyTxt" href="AssetsInSession.aspx">View assets to be catalogued</a></li>
					<li><a class="BodyTxt" href="AssetList.aspx">View all assets</a></li>
					<li><a class="BodyTxt" href="AssetUpload.aspx">Upload asset(s)</a></li>
				</ul>
				<asp:PlaceHolder ID="ExternalPublishInfoWrapper" runat="server">
					<p><span class="BodyTxt">Reference to use the asset: <asp:Label ID="AssetExternalReferenceLabel" runat="server" /> </span></p>
				</asp:PlaceHolder>
			</div>
		</asp:PlaceHolder>
        </div>
        <div class="Right AdmRightPanel"></div>
        <div class="Clear"><br /></div>
    </div>
    <br />
</asp:Content>