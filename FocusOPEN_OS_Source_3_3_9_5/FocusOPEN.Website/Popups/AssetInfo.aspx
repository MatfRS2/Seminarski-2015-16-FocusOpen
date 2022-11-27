<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssetInfo.aspx.cs" Inherits="FocusOPEN.Website.AssetInfoPopup" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register TagPrefix="app" Namespace="FocusOPEN.Website.Controls" Assembly="FocusOPEN.Website" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Details [<%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().ApplicationName%>]</title>
    <app:BrandHeader ID="BrandHeader1" Css="Basic|Styles" runat="server" />
    <script src="../Includes/Javascript/jQuery/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../Includes/Javascript/jQuery/jquery-ui-1.7.2.custom.min.js" type="text/javascript"></script>
    <script src="../Includes/Javascript/jQuery/jquery.tinysort.min.js" type="text/javascript"></script>
    <script src="../Includes/Javascript/Global.js" type="text/javascript"></script>
    <link rel="Stylesheet" type="text/css" href="../Includes/Css/jQuery-UI/smoothness/jquery-ui-1.7.2.custom.css" />
    <script type="text/javascript">
        
        $(document).ready(function() {
            
            
            SortMetas();
            
         });
         
         function SortMetas() {
                
                $("#metaTable>tbody>tr").tsort({ attr: "class"});
                
        }
        
		function pageLoaded() {
			$(".sendfeedbackbutton").bind('click', function() {
				if ($(".assetfeedbacktextbox").val().trim() == "") {
					alert("Please enter feedback");
					return false;
				}
				$(this).attr("disabled", "").val("Sending... Please wait");
			});
		}
		$(function() {
			Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(pageLoaded);
		});
		var rootUrl = "<%= ResolveUrl("~/") %>";
		function ResolveUrl(url) {
            if (url.indexOf("~/") == 0) {
                url = rootUrl + url.substring(2);
                }
            return url;
         }
         function GetParentWindow()
         {
			var win = window;
			
			while (win.opener != null)
				win = win.opener;
				
			return win;
         }
         
         
         
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <aspajax:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:Label ID="Label1" runat="server" />
    <div class="DetailsWrapper">
        <div class="DetailsHdrPanel">
        </div>
        <div style="margin: 0px 10px 10px 3px">
            <app:CategoryBreadcrumb runat="server" ID="AssetBreadcrumb" CategoryId="0" OnSelectCategory="Breadcrumb_SelectCategory" />
        </div>
        <div class="Left DetailsThbPanel">
            <div align="center">
                <table border="0" cellspacing="0" cellpadding="5">
                    <tr>
                        <td class="Top">
                            <app:AssetPreview ID="AssetPreview1" runat="server" />
                        </td>
					</tr>
                   <tr>
                       <td>
                           <app:FeedbackLabel ID="AssetButtonsMessageLabel" runat="server" />
                           <div class="Left">
                               <div class="ImgIcons">
                                   <app:AssetButtons ID="AssetButtons1" MessageLabelControlId="AssetButtonsMessageLabel" ShowButtons="Cart,Lightbox,Download,Restricted" OnCartButtonClicked="AssetButtons_Click" OnLightboxButtonClicked="AssetButtons_Click" runat="server" />
                                   <asp:Image ID="FileTypeIconImage" runat="server" />
                               </div>
                           </div>
                           <div class="Right BodyTxt" id="AssetLinksContainer" runat="server">
                               <asp:HyperLink ID="EditHyperLink" NavigateUrl="#" Text="edit" CssClass="BodyTxt" Target="_parent" runat="server" />
                               <app:EnhancedHyperLink ID="StatsHyperLink" Prefix="| " Suffix="" NavigateUrl="#" Text="stats" CssClass="BodyTxt" Target="_parent" runat="server" />
                               <app:EnhancedLinkButton ID="DeleteLinkButton" Prefix="| " Suffix="" OnClick="DeleteLinkButton_Click" OnClientClick="return confirm('Are you sure you want to delete this asset? This action cannot be undone')" CssClass="BodyTxt" Text="delete" runat="server" />
                               <app:EnhancedHyperLink ID="LogHyperLink" Prefix="| " Suffix="" CssClass="BodyTxt" NavigateUrl="#" Text="log" runat="server" />
                           </div>
                           <div class="Clear"></div>
                       </td>
                   </tr>
                </table>
            </div>
            <div id="AssetDescriptionContainer" class="BodyTxt alignLeft DetailsTxtMrg" runat="server">[Asset Description]</div>
            <div id="AssetCategoriesContainer" class="BodyTxt alignLeft DetailsTxtMrg" visible="false" runat="server">
                <br />
                <table>
                    <tr>
                        <td class="Bold">
                            Categories
                        </td>
                    </tr>
                    <tr>
                        <td class="Top">
                            <asp:Repeater ID="CategoriesRepeater" OnItemDataBound="CategoriesRepeater_ItemDataBound" OnItemCommand="CategoriesRepeater_ItemCommand" runat="server">
                                <ItemTemplate>
                                    <asp:LinkButton ID="CategoryLinkButton" CssClass="BodyTxt" runat="server" /><br />
                                </ItemTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                </table>
            </div>
            <app:MetadataVisibilityWrapper ID="MetadataVisibilityWrapper1" FieldName="FeedbackForm" Section="AssetDetail" runat="server">
                <div class="BodyTxt alignLeft DetailsTxtMrg">
                    <aspajax:UpdatePanel ID="AssetFeedbackUpdatePanel" ChildrenAsTriggers="true" runat="server">
                        <ContentTemplate>
                            <br />
                            <br />
                            <asp:LinkButton ID="AssetFeedbackFormToggleLink" OnClick="AssetFeedbackFormToggleLink_Click" CssClass="BodyTxt Bold" runat="server" />
                            <asp:Panel ID="AssetFeedbackFormPanel" Visible="false" runat="server">
                                <br />
                                Enter comments about this asset and associated information and they will be sent to an administrator for review.
                                <app:TextArea ID="AssetFeedbackTextBox" CssClass="assetfeedbacktextbox formInput" Style="margin: 5px 5px 5px 0px" Width="100%" Height="100px" runat="server" />
                                <br />
                                <app:PromptButton ID="AssetFeedbackCancelButton" CssClass="button" Text="cancel" Prompt="Are you sure? Feedback will not be sent" OnClick="AssetFeedbackCancelButton_Click" runat="server" />
                                <asp:Button ID="AssetFeedbackSendButton" CssClass="sendfeedbackbutton button" Text="send feedback" OnClick="AssetFeedbackSendButton_Click" runat="server" />
                            </asp:Panel>
                        </ContentTemplate>
                    </aspajax:UpdatePanel>
                </div>
            </app:MetadataVisibilityWrapper>
        </div>
        <div class="Right">
            <aspajax:UpdatePanel Id="TabUpdatePanel" ChildrenAsTriggers="true" runat="server">
                <ContentTemplate>
                    <div>
                        <asp:LinkButton ID="MetaDataLinkButton" CssClass="NoDecoration Black Bold BodyTxt" Text="Metadata" runat="server" OnCommand="PanelChoiceCommand_Click" CommandName="ShowMetadata" />
                        <span runat="server" id="FileMetadataLinkDivider" class="BodyTxt">|</span>
                        <asp:LinkButton ID="FileMetadataLinkButton" CssClass="Black BodyTxt" Text="File Metadata" runat="server" OnCommand="PanelChoiceCommand_Click" CommandName="ShowFileMetadata" />
                        <span runat="server" id="OrderHistoryDivider" class="BodyTxt">|</span>
                        <asp:LinkButton ID="OrderHistoryLinkButton" CssClass="Black BodyTxt" Text="Order History" runat="server" OnCommand="PanelChoiceCommand_Click" CommandName="ShowOrderHistory" />
                    </div>
                    <asp:Panel ID="AssetMetadataPanel" runat="server">
                        <table width="330" border="0" cellspacing="0" cellpadding="2" class="TblOutline"
                            style="margin-top: 15px" id="metaTable">
                            <tr runat="server" id="TrAssetId" class="0">
                                <td class="TblCell2 BodyTxt Bold alignRight W125 Top">
                                    Reference Number
                                </td>
                                <td class="TblCell1 BodyTxt Top" id="AssetIdCell" runat="server">
                                    [AssetId]
                                </td>
                            </tr>
                            <app:MetadataVisibilityWrapper FieldName="PrimaryBrand" Section="AssetDetail" runat="server">
                                <tr runat="server" id="TrBrand">
                                    <td class="TblCell2 BodyTxt Bold alignRight Top">
                                        <app:BrandMetadataLabel FieldName="Brand" runat="server" />
                                    </td>
                                    <td class="TblCell1 BodyTxt Top" id="BrandCell" runat="server">
                                        [Brand]
                                    </td>
                                </tr>
                            </app:MetadataVisibilityWrapper>
                            <app:MetadataVisibilityWrapper FieldName="AssetType" Section="AssetDetail" runat="server">
                                <tr runat="server" id="TrAssetType">
                                    <td class="TblCell2 BodyTxt Bold alignRight W125 Top">
                                        <app:BrandMetadataLabel FieldName="AssetType" runat="server" />
                                    </td>
                                    <td class="TblCell1 BodyTxt Top" id="AssetTypeCell" runat="server">
                                        [AssetType]
                                    </td> 
                                </tr>
                            </app:MetadataVisibilityWrapper>
                            <app:MetadataVisibilityWrapper FieldName="Filename" Section="AssetDetail" runat="server">
                                <tr runat="server" id="TrFilename">
                                    <td class="TblCell2 BodyTxt Bold alignRight">
                                        <app:BrandMetadataLabel FieldName="Filename" runat="server" />
                                    </td>
                                    <td class="TblCell1 BodyTxt" id="FilenameCell" runat="server">
                                        [Filename]
                                    </td>
                                </tr>
                            </app:MetadataVisibilityWrapper>
                            <app:MetadataVisibilityWrapper FieldName="FileSize" Section="AssetDetail" runat="server">
                                <tr runat="server" id="TrFileSize">
                                    <td class="TblCell2 BodyTxt Bold alignRight Top">
                                        <app:BrandMetadataLabel FieldName="FileSize" runat="server" />
                                    </td>
                                    <td class="TblCell1 BodyTxt Top" id="FilesizeCell" runat="server">
                                        [Filesize]
                                    </td>
                                </tr>
                            </app:MetadataVisibilityWrapper>
                            <app:MetadataVisibilityWrapper FieldName="FileHash" Section="AssetDetail" runat="server">
                                <tr runat="server" id="TrFileHash">
                                    <td class="TblCell2 BodyTxt Bold alignRight Top">
                                        <app:BrandMetadataLabel FieldName="FileHash" runat="server" />
                                    </td>
                                    <td class="TblCell1 BodyTxt Top">
                                        <asp:Label ID="FileHashLabel" runat="server" />
                                    </td>
                                </tr>
                            </app:MetadataVisibilityWrapper>
                            <app:MetadataVisibilityWrapper FieldName="DateUploaded" Section="AssetDetail" runat="server">
                                <tr runat="server" id="TrDateUploaded">
                                    <td class="TblCell2 BodyTxt Bold alignRight Top">
                                        <app:BrandMetadataLabel FieldName="DateUploaded" runat="server" />
                                    </td>
                                    <td class="TblCell1 BodyTxt Top">
                                        <asp:Label ID="DateUploadedLabel" runat="server" />
                                    </td>
                                </tr>
                            </app:MetadataVisibilityWrapper>
                            <app:MetadataVisibilityWrapper FieldName="Title" Section="AssetDetail" runat="server">
                                <tr runat="server" id="TrTitle">
                                    <td class="TblCell2 BodyTxt Bold alignRight Top">
                                        <app:BrandMetadataLabel FieldName="Title" runat="server" />
                                    </td>
                                    <td class="TblCell1 BodyTxt Top" id="TitleCell" runat="server">
                                        [Title]
                                    </td>
                                </tr>
                            </app:MetadataVisibilityWrapper>
                            <app:MetadataVisibilityWrapper FieldName="ProjectCode" Section="AssetDetail" runat="server">
                                <tr runat="server" id="TrProjectCode"> 
                                    <td class="TblCell2 BodyTxt Bold alignRight Top">
                                        <app:BrandMetadataLabel FieldName="ProjectCode" runat="server" />
                                    </td>
                                    <td class="TblCell1 BodyTxt Top" id="ProjectCodeCell" runat="server">
                                        [Project Code]
                                    </td>
                                </tr>
                            </app:MetadataVisibilityWrapper>
                            <tr id="OrientationRow" runat="server">
                                <td class="TblCell2 BodyTxt Bold alignRight Top">
                                    Orientation
                                </td>
                                <td class="TblCell1 BodyTxt Top" id="OrientationCell" runat="server">
                                    [Orientation]
                                </td>
                            </tr>
                            <tr id="DurationRow" runat="server">
                                <td class="TblCell2 BodyTxt Bold alignRight Top">
                                    Duration
                                </td>
                                <td class="TblCell1 BodyTxt Top" id="DurationCell" runat="server">
                                    [Duration]
                                </td>
                            </tr>
                            <tr id="DimensionsRow" runat="server">
                                <td class="TblCell2 BodyTxt Bold alignRight Top">
                                    Dimensions
                                </td>
                                <td class="TblCell1 BodyTxt Top" id="DimensionsCell" runat="server">
                                    [Dimensions]
                                </td>
                            </tr>
                            <app:MetadataVisibilityWrapper FieldName="Originator" Section="AssetDetail" runat="server">
                                <tr runat="server" id="TrOriginator">
                                    <td class="TblCell2 BodyTxt Bold alignRight Top">
                                        <app:BrandMetadataLabel FieldName="Originator" runat="server" />
                                    </td>
                                    <td class="TblCell1 BodyTxt Top" id="OriginatorCell" runat="server">
                                        [Originator]
                                    </td>
                                </tr>
                            </app:MetadataVisibilityWrapper>
                            <app:MetadataVisibilityWrapper FieldName="DateProduced" Section="AssetDetail" runat="server">
                                <tr runat="server" id="TrDateProduced">
                                    <td class="TblCell2 BodyTxt Bold alignRight Top">
                                        <app:BrandMetadataLabel FieldName="DateProduced" runat="server" />
                                    </td>
                                    <td class="TblCell1 BodyTxt Top" id="DateProducedCell" runat="server">
                                        [Date Produced]
                                    </td>
                                </tr>
                            </app:MetadataVisibilityWrapper>
                            <app:MetadataVisibilityWrapper FieldName="ContactEmail" Section="AssetDetail" runat="server">
                                <tr runat="server" id="TrContactEmail">
                                    <td class="TblCell2 BodyTxt Bold alignRight Top">
                                        <app:BrandMetadataLabel FieldName="ContactEmail" runat="server" />
                                    </td>
                                    <td class="TblCell1 Top">
                                        <app:EmailHyperLink ID="ContactEmailHyperLink" CssClass="BodyTxt" runat="server" />
                                    </td>
                                </tr>
                            </app:MetadataVisibilityWrapper>
                            
                             <asp:Repeater ID="MetadataRepeater" OnItemDataBound="MetadataRepeater_ItemDataBound" runat="server" EnableViewState="true">
                            <ItemTemplate>
                                 <app:MetadataVisibilityWrapper ID="MetadataVisibilityWrapper" Section="AssetDetail" runat="server">
                                     <tr runat="server" id="TrRepeaterMeta">
                                         <td class="TblCell2 BodyTxt Bold alignRight Top">
                                             <app:BrandMetadataLabel ID="BrandMetadataLabel" runat="server" />
                                         </td>
                                         <td class="TblCell1 BodyTxt Top" id="Metadata_Cell" runat="server">
                                             [Metadata]
                                         </td>
                                     </tr>
                                 </app:MetadataVisibilityWrapper>
                                 </ItemTemplate>
                            </asp:Repeater>
                            <app:MetadataVisibilityWrapper FieldName="CopyrightOwner" Section="AssetDetail" runat="server">
                                <tr runat="server" id="TrCopyrightOwner">
                                    <td class="TblCell2 BodyTxt Bold alignRight Top">
                                        <app:BrandMetadataLabel FieldName="CopyrightOwner" runat="server" />
                                    </td>
                                    <td class="TblCell1 BodyTxt Top" id="CopyrightOwnerCell" runat="server">
                                        [Copyright Owner]
                                    </td>
                                </tr>
                            </app:MetadataVisibilityWrapper>
                            <tr runat="server" id="TrRestrictedDownload">
                                <td class="TblCell2 BodyTxt Bold alignRight Top">
                                    Restricted Download
                                </td>
                                <td class="TblCell1 BodyTxt Top" id="RestrictedDownloadCell" runat="server">
                                    [Restricted Download]
                                </td>
                            </tr>
                            <app:MetadataVisibilityWrapper FieldName="UsageRestrictions" Section="AssetDetail"
                                runat="server">
                                <tr runat="server" id="TrUsageRestrictions">
                                    <td class="TblCell2 BodyTxt Bold alignRight Top">
                                        <app:BrandMetadataLabel FieldName="UsageRestrictions" runat="server" />
                                    </td>
                                    <td class="TblCell1 BodyTxt Top" id="UsageRestrictionsCell" runat="server">
                                        [Usage Restrictions]
                                    </td>
                                </tr>
                            </app:MetadataVisibilityWrapper>
                            <app:MetadataVisibilityWrapper FieldName="LinkedAssets" Section="AssetDetail" runat="server">
                                <tr id="LinkedAssetsRow" runat="server">
                                    <td class="TblCell2 BodyTxt Bold alignRight Top">
                                        <app:BrandMetadataLabel FieldName="LinkedAssets" runat="server" />
                                    </td>
                                    <td class="TblCell1 BodyTxt Top">
                                        <asp:Repeater runat="server" ID="LinkedAssetsRepeater" OnItemDataBound="LinkedAssetsRepeater_ItemDataBound">
                                            <HeaderTemplate>
                                                <table>
                                            </HeaderTemplate>
                                            <FooterTemplate>
                                                </table></FooterTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton runat="server" ID="LinkedAssetTitleLinkButton" CssClass="BodyTxt" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </td>
                                </tr>
                            </app:MetadataVisibilityWrapper>
                            <app:MetadataVisibilityWrapper FieldName="AdditionalKeywords" Section="AssetDetail" runat="server">
                                <tr runat="server" id="TrAdditionalKeywords">
                                    <td class="TblCell2 BodyTxt Bold alignRight Top">
                                        <app:BrandMetadataLabel FieldName="AdditionalKeywords" runat="server" />
                                    </td>
                                    <td class="TblCell1 BodyTxt Top" id="AdditionalKeywordsCell" runat="server">
                                        [Additional Keywords]
                                    </td>
                                </tr>
                            </app:MetadataVisibilityWrapper>
                            <app:MetadataVisibilityWrapper FieldName="AttachedFiles" Section="AssetDetail" runat="server">
                                <tr id="AttachedFilesRow" runat="server">
                                    <td class="TblCell2 BodyTxt Bold alignRight Top">
                                        <app:BrandMetadataLabel FieldName="AttachedFiles" runat="server" />
                                    </td>
                                    <td class="Top">
                                        <asp:DataList ID="AttachedFilesDataList" OnItemDataBound="AttachedFilesDataList_ItemDataBound" runat="server">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="DownloadAttachedFileHyperLink" CssClass="BodyTxt" runat="server" />
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </td>
                                </tr>
                            </app:MetadataVisibilityWrapper>
                            <app:MetadataVisibilityWrapper FieldName="PublicationDate" Section="AssetDetail" runat="server">
                                <tr runat="server" id="TrPublicationDate">
                                    <td class="TblCell2 BodyTxt Bold alignRight Top">
                                        <app:BrandMetadataLabel FieldName="PublicationDate" runat="server" />
                                    </td>
                                    <td class="TblCell1 BodyTxt Top" id="PublicationDateCell" runat="server">
                                        [Publication Date]
                                    </td>
                                </tr>
                            </app:MetadataVisibilityWrapper>
                            <app:MetadataVisibilityWrapper FieldName="ExpiryDate" Section="AssetDetail" runat="server">
                                <tr runat="server" id="TrExpiryDate">
                                    <td class="TblCell2 BodyTxt Bold alignRight Top">
                                        <app:BrandMetadataLabel FieldName="ExpiryDate" runat="server" />
                                    </td>
                                    <td class="TblCell1 BodyTxt Top" id="ExpiryDateCell" runat="server">
                                        [Expiry Date]
                                    </td>
                                </tr>
                            </app:MetadataVisibilityWrapper>
                        </table>
                        
                    </asp:Panel>
                    <asp:Panel ID="AssetFileMetadataPanel" runat="server" Visible="false">
                        <asp:Repeater ID="AssetFileMetadataRepeater" runat="server" OnItemDataBound="AssetFileMetadataRepeater_ItemDataBound">
                            <HeaderTemplate>
                                <table width="330" border="0" cellspacing="0" cellpadding="2" class="TblOutline"
                                    style="margin-top: 15px">
                            </HeaderTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="TblCell2 BodyTxt Bold alignRight Top">
                                        <asp:Literal ID="MetadataNameLiteral" runat="server" />
                                    </td>
                                    <td class="TblCell1 BodyTxt Top">
                                        <asp:Literal ID="MetadataValueLiteral" runat="server" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </asp:Panel>
                    <asp:Panel ID="AssetOrderHistoryPanel" runat="server" Visible="false">
                        <asp:ListView ID="OrderHistoryListView" runat="server" OnItemDataBound="OrderHistoryListView_ItemDataBound">
                            <LayoutTemplate>
                                <table width="330" border="0" cellspacing="0" cellpadding="2" class="TblOutline" style="margin-top: 15px">
                                    <tr runat="server" id="itemPlaceholder"></tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="TblCell2 BodyTxt Bold alignRight W125 Top">
                                        Ordered By
                                    </td>
                                    <td class="TblCell1 BodyTxt Top">
                                        <asp:LinkButton ID="UserNameLinkButton" runat="server" CssClass="Black Bold BodyTxt" />
                                        <asp:Label ID="OrderByValueLabel" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TblCell2 BodyTxt Bold alignRight W125 Top">
                                        Date Ordered
                                    </td>
                                    <td class="TblCell1 BodyTxt Top">
                                        <asp:Label ID="OrderDateValueLabel" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TblCell2 BodyTxt Bold alignRight W125 Top">
                                    </td>
                                    <td class="TblCell1 BodyTxt Top">
                                        <asp:LinkButton ID="ToggleOrderDetailCommand" OnClick="ToggleOrderDetailButton_Click" CssClass="Black Bold BodyTxt" Text="[+] show order details" runat="server" />
                                    </td>
                                </tr>
                                <asp:Panel runat="server" ID="OrderHistoryDetailPanel" Visible="false">
                                    <tr>
                                        <td class="TblCell2 BodyTxt Bold alignRight W125 Top">
                                            Approval Deadline
                                        </td>
                                        <td class="TblCell1 BodyTxt Top">
                                            <asp:Label ID="DeadlineDateValueLabel" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TblCell2 BodyTxt Bold alignRight W125 Top">
                                            Reason for usage
                                        </td>
                                        <td class="TblCell1 BodyTxt Top">
                                            <asp:Label ID="NotesValueLabel" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TblCell2 BodyTxt Bold alignRight W125 Top">
                                            Approved
                                        </td>
                                        <td class="TblCell1 BodyTxt Top">
                                            <asp:Label ID="ApprovedYesNoValueLabel" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TblCell2 BodyTxt Bold alignRight W125 Top">
                                            Date
                                        </td>
                                        <td class="TblCell1 BodyTxt Top">
                                            <asp:Label ID="OrderItemStatusDateValueLabel" runat="server" />
                                        </td>
                                    </tr>
                                    <asp:Panel ID="ViewOrderPanel" runat="server">
                                        <tr>
                                            <td class="TblCell2 BodyTxt Bold alignRight W125 Top">
                                            </td>
                                            <td class="TblCell1 BodyTxt Top">
                                                <asp:LinkButton ID="ViewOrderLink" CssClass="Black Bold BodyTxt" Text="view order" runat="server" />
                                            </td>
                                        </tr>
                                    </asp:Panel>
                                </asp:Panel>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </asp:Panel>
                    
                    
                    
                </ContentTemplate>
            </aspajax:UpdatePanel>
        </div>
        <div>
            <div style="float: left; clear: both; padding: 10px;">
                <app:AttributionFooter ID="AttributionFooter" runat="server" />
            </div>
            <div style="float: right; text-align: right; padding: 10px; padding-top: 25px;">
                <a href="javascript:self.close();" class="BodyTxt Bold" title="click here to close this window">Close X</a>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
