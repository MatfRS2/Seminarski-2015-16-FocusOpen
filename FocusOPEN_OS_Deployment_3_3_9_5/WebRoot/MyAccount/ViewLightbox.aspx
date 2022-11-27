<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="ViewLightbox.aspx.cs" Inherits="FocusOPEN.Website.MyAccount.ViewLightbox" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title>Lightbox [<%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().ApplicationName%>]</title>
	<app:BrandHeader ID="BrandHeader1" Css="Basic|Print|Styles|Other" runat="server" />
	<script src="../Includes/Javascript/jQuery/jquery-1.4.1.min.js" type="text/javascript"></script>
	<script src="../Includes/Javascript/jQuery/jquery-ui-1.7.2.custom.min.js" type="text/javascript"></script>
	<script src="../Includes/Javascript/Global.js" type="text/javascript"></script>
	<link rel="Stylesheet" type="text/css" href="../Includes/Css/jQuery-UI/smoothness/jquery-ui-1.7.2.custom.css" />
</head>
<body>
<form id="form1" runat="server">
	<aspajax:ScriptManager ID="ScriptManager1" EnablePageMethods="true" EnablePartialRendering="true" runat="server" />
	<script type="text/javascript">
		Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(pageLoaded);

		var notesSaved = false;
		var LIGHTBOX_ROW_HEIGHT = 253;
		var ANIMATION_SPEED_PARAM = 10;
		var SHOW_NOTES_SAVED_ALERT = false;

		function getCurrentLightboxId() {
		    return $("#<%=UserLightboxDropDownList1.ClientID %> > option:selected").val();
		}

		function getAssetId(checkbox) {
		    var scope = $(checkbox).parents(".lightbox-asset-row");
		    var test = jQuery("input", scope).val();
		    return test;
		}

		function animateScrollBy(y, yInc){
		    yPos = 0;
            var timerID = setInterval(function() {
            window.scrollBy(0, yInc); yPos = yPos + yInc;
                if(y>=0){ 
                    if( yPos >= y ) clearInterval(timerID);
                } else { 
                    if( yPos <= y ) clearInterval(timerID); 
                }
            }, 1);
        }

		function pageLoaded() {
        	var date = new Date();
        	date.setDate(date.getDate() + 1);
        	
        	$("#<%=SendLightboxExpiryDate.ClientID %>").datepicker({ showAnim: 'slideDown', dateFormat: 'dd MM yy', 'minDate': date });

        	$(".SendLightboxType").click(function(event) {
        		$("#<%=SendLightboxEnableLinkedEditCheckBox.ClientID %>").attr('checked', false);
        		
        		if (event.target.value == 'link') {
        			$("#<%=SendLightboxEnableLinkedEditCheckBox.ClientID %>").removeAttr('disabled');
        			$("#<%=SendLightboxEnableLinkedEditCheckBox.ClientID %>").closest('span').removeAttr('disabled');
        		}
        		else {
        			$("#<%=SendLightboxEnableLinkedEditCheckBox.ClientID %>").attr('disabled', 'disabled');
        			$("#<%=SendLightboxEnableLinkedEditCheckBox.ClientID %>").closest('span').attr('disabled', 'disabled');
        		}
        	});

            $(".notes-textbox").keyup(function () {
        	    notesSaved = false;
        	});

        	$(".save-notes-button").click(function (event) {

        	    $(this).val("saving...").attr("disabled", "true");

        	    event.preventDefault();

        	    var parentRow = $(this).parents(".lightbox-asset-row");
        	    var lbid = parentRow.find(".lightbox-asset-id > input").val();
        	    var notes = parentRow.find(".notes-textbox").val();

        	    PageMethods.SaveNotes(lbid, notes, function (result) {
        	        if (SHOW_NOTES_SAVED_ALERT) {
        	            if (result) {
        	                alert("Notes saved successfully");
        	            }
        	            else {
        	                alert("Notes not saved");
        	            }
        	        }
        	        $(".lbid-" + lbid).find(".save-notes-button").val("save notes").removeAttr("disabled");
        	    });
        	});

            $(".save-all-notes-button").click(function (event) {
                event.preventDefault();
                if (!notesSaved) {
                    $(".save-notes-button").val("saving...").attr("disabled", "true");
                    var overallResult = true;
                    $(".save-notes-button").each(function () {
                        var parentRow = $(this).parents(".lightbox-asset-row");
                        var lbAssetId = parentRow.find(".lightbox-asset-id > input").val();
                        var notes = parentRow.find(".notes-textbox").val();
                        PageMethods.SaveNotes(lbAssetId, notes, function (result) {
                            overallResult &= result;
                        });
                    });
                    if (overallResult) {
                        alert("All notes saved successfully");
                    }
                    else {
                        alert("All notes not saved");
                    }
                    $(".save-notes-button").val("save notes").removeAttr("disabled");
                    notesSaved = true;
                }
            });

            $("span.select-all-link input").click(function (event) {
                if (this.checked) {
                    PageMethods.SelectAllAssets(getCurrentLightboxId());
                } else {
                    PageMethods.DeselectAllAssets(getCurrentLightboxId());
                }
                $("span.select-asset-checkbox input").attr('checked', this.checked);
                $("span.select-all-link input").attr('checked', this.checked);
            });

            $("span.select-asset-checkbox input").click(function () {
                if (!this.checked) {
                    $("span.select-all-link input").attr('checked', false);
                }
                else if ($("span.select-asset-checkbox input").length == $("span.select-asset-checkbox input:checked").length) {
                    $("span.select-all-link input").attr('checked', true);
                }
                var assetId = getAssetId(this);
                var checked = this.checked;
                PageMethods.SetAssetSelected(assetId, checked);
            });

            $(".deselect-all-link").click(function (event) {
                PageMethods.DeselectAllAssets(getCurrentLightboxId());
                $("span.select-asset-checkbox input").attr('checked', false);
                $("span.select-all-link input").attr('checked', false);
                event.preventDefault();
            });

            var assetId = $('#<%=AssetIDPanelField.ClientID %>').val();
            if (assetId > 0) {
                var anchor = '#Asset' + assetId;
                var inARow = $('#<%=MovedInARow.ClientID %>').val();

                if (inARow=='down') {
                    animateScrollBy(LIGHTBOX_ROW_HEIGHT, ANIMATION_SPEED_PARAM);
                }
                else if (inARow == 'up') {
                    animateScrollBy(-1*LIGHTBOX_ROW_HEIGHT, -1*ANIMATION_SPEED_PARAM);
                }
                else {
                    window.location.hash = anchor;
                }
            }
        }


	</script>
	<div class="CatWrapper">
		<app:PageHeader ID="PageHeader1" runat="server" />
		<div class="CatBkg">
			<div class="LbxAppPanel">
				<div class="Left"> 
					<div class="AppHdr">
						<!--start text header//--><span class="CatAppHdr">LIGHTBOX</span>&nbsp;<!--end text header//-->
						<span class="PanelTxt BaseLine">
							selected lightbox 
							<app:UserLightboxDropDownList id="UserLightboxDropDownList1" ShowPublicLightboxes="true" OmitBlankItem="true" cssclass="formInput" AutoPostBack="true" OnSelectedIndexChanged="UserLightboxDropDownList1_SelectedIndexChanged" runat="server" />
						</span>
					</div>
                    <aspajax:UpdatePanel runat="server" ID="TotalCountUpdatePanel" >
                        <Triggers>
                            <aspajax:AsyncPostbackTrigger controlid="RemoveSelectedAssetsLinkButton1" eventname="Click" />
                            <aspajax:AsyncPostbackTrigger controlid="RemoveSelectedAssetsLinkButton2" eventname="Click" />
                        </Triggers> 
                        <ContentTemplate>
				            <div class="AppResults PanelTxt">total in lightbox: <span id="LightboxAssetCount1" class="Bold" runat="server"><%=AssetCount%></span> assets [displaying page <%=CurrentPage%> of <%=TotalPages%>]</div>
                        </ContentTemplate>
                    </aspajax:UpdatePanel>
				</div>
                <aspajax:UpdatePanel runat="server" ID="HeaderPagerUpdatePanel" >
                    <Triggers>
                        <aspajax:AsyncPostbackTrigger controlid="RemoveSelectedAssetsLinkButton1" eventname="Click" />
                        <aspajax:AsyncPostbackTrigger controlid="RemoveSelectedAssetsLinkButton2" eventname="Click" />
                    </Triggers> 
                    <ContentTemplate>
				<div class="Right">
					<div class="AppPageView">
						<span class="PanelTxt">show</span>
						<app:PageSizeDropDownList ID="PageSizeDropDownList1" OmitBlankItem="true" CssClass="formInput" AutoPostBack="true" OnSelectedIndexChanged="PageSize_SelectedIndexChanged" runat="server" />
						<div class="AppPaging">
							<app:SimplePager ID="TopPager" OnCommand="Page_IndexChanged" runat="server" />
						</div>
					</div>
				</div>
                    </ContentTemplate>
                </aspajax:UpdatePanel>
			</div>
			<div class="LbxContent">
				<br />
				<div class="LbxLeftPanel Left">
					<div class="LbxNotesPanel"> 
						<div class="NotesMrg">
							<aspajax:UpdatePanel ID="LightboxNotesUpdatePanel" ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server">
							<ContentTemplate>
								<asp:Panel ID="LightboxPanelWrapper" runat="server">
									<asp:Panel ID="LightboxTextPanel" runat="server">
										<span class="BodyTxt"> 
											<span class="Bold">Title: </span>
											<asp:Label ID="LightboxTitleLabel" Text="[Title]" runat="server" />
											<br />
											<span class="Bold">Summary: </span>
											<asp:Label ID="LightboxSummaryLabel" Text="[Summary]" runat="server" />
											<br />
											<span class="Bold">Status: </span>
											<asp:Label ID="LightboxStatusLabel" Text="[Status]" runat="server" />
											<br />
											<asp:Panel ID="NotesPanel" Visible="false" runat="server">
												<span class="Bold">Notes: </span>
												<asp:Label ID="LightboxNotesLabel" Text="[Notes]" runat="server" /><br />
											</asp:Panel>
											<br />
											<asp:HyperLink ID="LinkToLightboxHyperlink" Text="Link to this lightbox" CssClass="BodyTxt" runat="server" />
										</span>
										<br />
										<div class="alignRight"><asp:LinkButton ID="ToggleNotesLinkButton" CssClass="BodyTxt" OnClick="ToggleNotesLinkButton_Click" runat="server">show notes [+]</asp:LinkButton><asp:PlaceHolder ID="EditLightboxNotesButtonWrapper" runat="server"> <span class="BodyTxt">|</span> <asp:LinkButton ID="EditLightboxNotesLinkButton" CssClass="BodyTxt" Text="edit" OnClick="EditLightboxNotesLinkButton_Click" runat="server" /></asp:PlaceHolder></div>
									</asp:Panel>
									<asp:Panel ID="LightboxEditTextPanel" Visible="false" runat="server">
										<div class="EditMrg">
											<app:FeedbackLabel id="LightboxEditMessageLabel" runat="server" />
											<table border="0" cellspacing="0" cellpadding="2">
											<tr> 
												<td class="BodyTxt alignRight W125"><span class="Bold">Title</span><br />(max chars 25)</td>
												<td class="BodyTxt Top"><asp:TextBox ID="LightboxTitleTextBox" CssClass="formInput W100" MaxLength="25" runat="server" /></td>
											</tr>
											<tr> 
												<td class="BodyTxt alignRight W125"><span class="Bold">Summary</span><br />(max chars 160)</td>
												<td class="BodyTxt Top"><asp:TextBox ID="LightboxSummaryTextBox" CssClass="formInput W350" MaxLength="160" runat="server" /></td>
											</tr>
											<tr> 
												<td class="BodyTxt Bold alignRight W125">Status</td>
												<td class="BodyTxt Top">
													<asp:RadioButton ID="IsPublicRadioButton" GroupName="IsPublicRadioButtonGroup" OnCheckedChanged="IsPublicRadioButtonGroup_Click" AutoPostBack="true" runat="server" Text="public" />
													<asp:RadioButton ID="IsPrivateRadioButton" GroupName="IsPublicRadioButtonGroup" OnCheckedChanged="IsPublicRadioButtonGroup_Click" AutoPostBack="true" runat="server" Text="private" />
													<asp:Label ID="LightboxEditStatusLabel" Visible="false" runat="server" />
												</td>
											</tr>
											<tr id="BrandSelectorRow" visible="false" runat="server">
												<td class="BodyTxt Top Bold alignRight W125">Brands</td>
												<td class="BodyTxt Top">
													<app:BrandCheckBoxList ID="BrandCheckBoxList1" runat="server" />
												</td>
											</tr>
											<tr>
												<td class="BodyTxt alignRight Top W125"><span class="Bold">Notes</span><br />Are visible to you and users who share this lightbox</td>
												<td class="BodyTxt Top"><app:TextArea ID="LightboxNotesTextBox" Columns="25" Rows="4" Wrap="true" CssClass="formInput W350" MaxLength="500" runat="server" /></td>
											</tr>
											</table>
											<div class="alignRight">
												<br />
												<asp:Button ID="SaveLightboxNotesButton" Text="save lightbox notes" CssClass="button" OnClick="SaveLightboxNotesButton_Click" runat="server" />
												<asp:Button ID="CancelLightboxNotesButton" Text="cancel" CssClass="button" OnClick="CancelLightboxNotesButton_Click" runat="server" />
											</div>
										</div>
									</asp:Panel>
								</asp:Panel>
							</ContentTemplate>
							<Triggers>
								<aspajax:AsyncPostBackTrigger ControlID="ToggleNotesLinkButton" />
								<aspajax:AsyncPostBackTrigger ControlID="EditLightboxNotesLinkButton" />
								<aspajax:AsyncPostBackTrigger ControlID="IsPublicRadioButton" />
								<aspajax:AsyncPostBackTrigger ControlID="IsPrivateRadioButton" />
								<aspajax:PostBackTrigger ControlID="SaveLightboxNotesButton" />
								<aspajax:AsyncPostBackTrigger ControlID="CancelLightboxNotesButton" />
							</Triggers>
							</aspajax:UpdatePanel>
						</div>
					</div>
					<div class="LbxResultsPanel">
						<div class="LbxResultsHdr"></div>
						<app:FeedbackLabel id="FeedbackLabel1" runat="server" />

                            <aspajax:UpdatePanel runat="server" ID="AssetIDUpdatePanel">
                                <Triggers>
                                    <aspajax:AsyncPostbackTrigger controlid="LightboxAssetsRepeater" eventname="ItemCommand" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:HiddenField ID="AssetIDPanelField" runat="server" value="-1" EnableViewState="false"/>
                                    <asp:HiddenField ID="MovedInARow" runat="server" value="false" />
                                </ContentTemplate>
                            </aspajax:UpdatePanel>


                            <aspajax:UpdatePanel runat="server" ID="AssetListUpdatePanel">
                                <Triggers>
                                    <aspajax:AsyncPostbackTrigger controlid="RemoveSelectedAssetsLinkButton1" eventname="Click" />
                                    <aspajax:AsyncPostbackTrigger controlid="RemoveSelectedAssetsLinkButton2" eventname="Click" />
                                </Triggers> 
                                <ContentTemplate>
						<asp:Panel ID="LightboxAssetsPanel" runat="server">
                            <asp:Panel ID="LightboxAssetsPanelHeader" runat="server">
							    <div class="Left">
								    <app:BackToSearchHyperlink ID="BackToSearchHyperlink1" runat="server" />
							    </div>
							    <asp:Panel ID="LightboxAssetsOptions1" CssClass="Right BodyTxt" runat="server">
								    <span class="Bold">select all assets</span>
                                    <asp:CheckBox ID="SelectAllAssetsInLightbox1" 
                                        CssClass="select-all-link"
                                        runat="server" />
								    [<asp:LinkButton ID="DeselectAllLinkButton1" 
                                        CssClass="deselect-all-link BodyTxt" 
                                        Text="deselect all" 
                                        AutoPostback="false"
                                        runat="server" />
                                    <app:EnhancedLinkButton ID="RemoveSelectedAssetsLinkButton1" Prefix=" | " Suffix="" Text="remove selected" CssClass="BodyTxt" OnClick="RemoveSelectedAssetsLinkButton_Click" runat="server" />
                                    <asp:PlaceHolder ID="SaveAllNotesWrapper1" runat="server">&nbsp;|&nbsp;
                                        <asp:LinkButton ID="SaveAllNotesButton1" 
                                            CssClass="save-all-notes-button BodyTxt" 
                                            Text="save all notes" 
                                            AutoPostback="false"
                                            runat="server" />
                                    </asp:PlaceHolder>]
                                </asp:Panel>
							    <div class="Clear"></div>
                            </asp:Panel>
							<asp:Repeater ID="LightboxAssetsRepeater" OnItemDataBound="LightboxAssetsRepeater_OnItemDataBound" runat="server">
								<ItemTemplate>
									<div class="lightbox-asset-row lbid-<%# DataBinder.Eval (Container.DataItem, "LightboxAssetId") %>">
										
										<asp:HiddenField ID="AssetIdHiddenField" runat="server" />
                                            
										<span class="lightbox-asset-id"><asp:HiddenField ID="LightboxAssetIdHiddenField" runat="server" /></span>
										
										<div class="Left LbxThbMrg LbxThbPanel" >
											<a name="Asset<%# DataBinder.Eval (Container.DataItem, "Asset.AssetId") %>" />
                                            <app:AssetThumbnail ID="AssetThumbnail1" UseMarginCssClass="false" runat="server" /><br />
											<div class="ImgIcons"><app:AssetButtons ID="AssetButtons1" MessageLabelControlId="FeedbackLabel1" OnLightboxButtonClicked="AssetButtons1_LightboxButtonClicked"  runat="server" /></div>
										        
                                        </div>
										<div class="Right LbxThbMrg">
											<table width="350" border="0" cellspacing="0" cellpadding="2" class="TblOutline">
											<tr>
												<td class="TblCell2 BodyTxt Bold alignRight Top W125">Reference Number</td>
												<td class="TblCell1 BodyTxt Top"><%# DataBinder.Eval (Container.DataItem, "Asset.AssetId") %></td>
											</tr>
											<tr>
												<td class="TblCell2 BodyTxt Bold alignRight Top">Short Description</td>
												<td class="TblCell1 BodyTxt Top" id="DescriptionCell" runat="server">[Description]</td>
											</tr>
											<app:MultiBrandPlaceHolder runat="server">
												<td class="TblCell2 BodyTxt Bold alignRight">Brand</td>
												<td class="TblCell1 BodyTxt Top"><%# DataBinder.Eval (Container.DataItem, "Asset.BrandName") %></td>
											</app:MultiBrandPlaceHolder>
											<tr>
												<td class="TblCell2 BodyTxt Bold alignRight Top">Asset Type</td>
												<td class="TblCell1 BodyTxt Top"><asp:Label ID="AssetTypeLabel" runat="server" /></td>
											</tr>
											<tr>
												<td class="TblCell2 BodyTxt Bold alignRight Top">Usage Restrictions</td>
												<td class="TblCell1 BodyTxt Top"><%# DataBinder.Eval (Container.DataItem, "Asset.UsageRestrictions") %></td>
											</tr>
											<tr id="NotesRow" runat="server">
												<td class="TblCell2 BodyTxt alignRight Top W125"><span class="Bold">Notes</span><br />Are visible to you and users who share this lightbox</td>
												<td class="TblCell1 BodyTxt Top">
													<app:TextArea ID="NotesTextBox" Columns="25" Rows="4" Wrap="true" CssClass="notes-textbox formInput W225" MaxLength="500" runat="server"/>
												</td>
											</tr>
											</table>
                                        </div>
										<div class="Clear"></div>
										<div class="alignRight">
											<br />
											<asp:Panel ID="SaveNotesButtonPanel" CssClass="Right" runat="server">
												<asp:Button ID="SaveNotesButton" Text="save notes" CssClass="save-notes-button button" OnClick="SaveNotes_Click" runat="server" />
													    
											</asp:Panel> 
                                            <div class="Right">&nbsp;&nbsp;&nbsp;</div>
											<asp:Panel ID="OrderAssetButtonPanel" runat="server">
                                                <asp:Panel ID="OrderAssetButtonPanelUp" runat="server" CssClass="Right" >
												    <asp:Button ID="OrderAssetUpButton" Width="30" Text="up" CssClass="button" OnClick="OrderAssetUp_Click" runat="server" />
                                                </asp:Panel>
                                                <div class="Right">&nbsp;</div>
                                                <asp:Panel ID="OrderAssetButtonPanelDown" runat="server" CssClass="Right" >
												    <asp:Button ID="OrderAssetDownButton" Text="down" Width="40" CssClass="button" OnClick="OrderAssetDown_Click" runat="server" />
											    </asp:Panel>
                                            </asp:Panel>
											<br class="Clear"/>
											<span class="BodyTxt Bold">select this asset</span>
											<span><asp:CheckBox ID="SelectAssetCheckBox" CssClass="select-asset-checkbox" runat="server" /></span>
											<br />
											<br />
										</div>
										<div class="Clear"></div>
										<div class="LbxResultsFtr"></div>
									</div>
								</ItemTemplate>
							</asp:Repeater>
                                
                            <asp:Panel ID="LightboxAssetsPanelFooter" runat="server">
							    <br />
							    <div class="Left">
								    <app:BackToSearchHyperlink ID="BackToSearchHyperlink2" runat="server" />
							    </div>
							    <div class="Right BodyTxt">
								    <span class="Bold">select all assets</span>
                                    <asp:CheckBox ID="SelectAllAssetsInLightbox2" 
                                        CssClass="select-all-link"
                                        runat="server" />
								    [<asp:LinkButton ID="DeselectAllLinkButton2" 
                                        CssClass="deselect-all-link BodyTxt" 
                                        Text="deselect all" 
                                        AutoPostback="false"
                                        runat="server" />
                                    <app:EnhancedLinkButton ID="RemoveSelectedAssetsLinkButton2" Prefix=" | " Suffix="" Text="remove selected" CssClass="BodyTxt" OnClick="RemoveSelectedAssetsLinkButton_Click" runat="server" />
                                    <asp:PlaceHolder ID="SaveAllNotesWrapper2" runat="server">&nbsp;|&nbsp;
                                        <asp:LinkButton ID="SaveAllNotesButton2" 
                                            CssClass="save-all-notes-button BodyTxt" 
                                            Text="save all notes" 
                                            AutoPostback="false"
                                            runat="server" />
                                    </asp:PlaceHolder>]
							    </div>
							    <div class="Clear"></div>
                            </asp:Panel>
						</asp:Panel>
						
						<asp:Panel ID="EmptyLightboxPanel" Visible="false" runat="server">
							<div class="noResults">
								<app:BackToSearchHyperlink ID="BackToSearchHyperlink3" ShowRightDivider="true" runat="server" />
								<app:HelpHyperLink ID="HelpHyperLink3" HelpPage="3_Lightboxes.htm" CssClass="BodyTxt H20" Text="help" runat="server" />
							</div>
						</asp:Panel>
                                </ContentTemplate>
                            </aspajax:UpdatePanel>
					
					</div>
				</div>
				<div class="LbxRightPanel Right">
					<div class="LbxCtrlPanel">
						
						<aspajax:UpdatePanel ID="ControlAjaxPanel" RenderMode="Inline" ChildrenAsTriggers="true" runat="server">
						<ContentTemplate>
							<asp:Panel ID="AssetControlPanel" runat="server">
								<div class="LbxCtrlHdr">
									<div class="Left CtrlHdrMrg"><span class="CtrlHdrTxt"><span class="Bold">Assets</span> | </span><asp:LinkButton ID="ToggleLightboxesOn" Text="Lightboxes" CssClass="CtrlHdrTxt" OnCommand="ToggleControlPanelVisibility" CommandArgument="lightboxes" runat="server" /></div>
									<div class="Right CtrlHdrMrg"><app:HelpHyperLink ID="HelpHyperLink1" HelpPage="3_Lightboxes.htm#managing_assets" CssClass="CtrlHelpTxt" Text="help" runat="server" /></div>
								</div>
								<div class="LbxCtrlBkg">
									<div class="LbxCtrlMrg">
										<span class="BodyTxt H15">
											<br /><span class="BodyTxt H15">Select assets to REMOVE from this lightbox, MOVE &amp; COPY to another lightbox, or add them to your CART.</span>
											<br /><br />
											<app:EnhancedLinkButton ID="RemoveAssetsFromLightboxLinkButton" Text="Remove From This Lightbox" CssClass="PanelTxt" OnCommand="LightboxActionLinkButton_Command" CommandArgument="assets.remove" runat="server" />
											<app:EnhancedLinkButton ID="MoveCopyAssetsToAnotherLightbox" Text="Move Or Copy To Another Lightbox" CssClass="PanelTxt" OnCommand="LightboxActionLinkButton_Command" CommandArgument="assets.movecopylightbox" runat="server" />
											<app:EnhancedLinkButton ID="MoveCopyAssetsToCart" Text="Move Or Copy To Cart" CssClass="PanelTxt" OnCommand="LightboxActionLinkButton_Command" CommandArgument="assets.movecopycart" runat="server" />
											<br />
										</span>
										
										<asp:Panel ID="RemoveAssetsPanel" Visible="false" runat="server">
											<div class="LbxCtrlFtr">
												<br />
												<app:FeedbackLabel id="RemoveAssetsFeedbackLabel" LineBreaks="1" runat="server" />
												<span class="BodyTxt Bold">Remove Assets</span><br />
												<span class="BodyTxt">
													Remove selected assets from this lightbox<br />
													<br />
													<asp:Button ID="RemoveAssetsButton" Text="remove" CssClass="button" OnCommand="LightboxActionButton_Command" CommandArgument="assets.remove" runat="server" />
													<asp:Button ID="CancelRemoveAssetsButton" Text="cancel" CssClass="button" OnClick="CancelAction_Click" runat="server" />
												</span>
												<br />
											</div>
										</asp:Panel>
										
										<asp:Panel ID="MoveCopyAssetsPanel" Visible="false" runat="server">
											<div class="LbxCtrlFtr">
												<br />
												<app:FeedbackLabel id="MoveCopyAssetsFeedbackLabel" LineBreaks="1" runat="server" />
												<span class="BodyTxt Bold">Move/Copy Assets To Another Lightbox</span>
												<br />
												<span class="BodyTxt">
													Move or copy selected assets to lightbox:<br />
													<app:UserLightboxDropDownList ID="MoveCopyAssetsLightboxDropDownList" CssClass="formInput" OmitBlankItem="true" runat="server" />
													<br />
													<br />
													<asp:RadioButtonList ID="MoveCopyAssetsLightboxRadioButtonList" RepeatLayout="Flow" RepeatDirection="Horizontal" runat="server">
														<asp:ListItem Value="move">move</asp:ListItem>
														<asp:ListItem Value="copy" Selected="true">copy</asp:ListItem>
													</asp:RadioButtonList>
													<br />
													<br />
													<asp:Button ID="MoveCopyAssetsButton" Text="OK" CssClass="button" OnCommand="LightboxActionButton_Command" CommandArgument="assets.movecopylightbox" runat="server" />
													<asp:Button ID="CancelMoveopyAssetsButton" Text="cancel" CssClass="button" OnClick="CancelAction_Click" runat="server" />
												</span>
												<br />
											</div>
										</asp:Panel>
										
										<asp:Panel ID="MoveCopyAssetsToCartPanel" Visible="false" runat="server">
											<div class="LbxCtrlFtr">
												<br />
												<app:FeedbackLabel id="MoveCopyAssetsToCartFeedbackLabel" LineBreaks="1" runat="server" />
												<span class="BodyTxt Bold">Move/Copy Assets To Cart</span>
												<br />
												<span class="BodyTxt">
													Move or copy selected assets to the cart<br />
													<asp:RadioButtonList ID="MoveCopyAssetsCartRadioButtonList" RepeatLayout="Flow" RepeatDirection="Horizontal" runat="server">
														<asp:ListItem Value="move">move</asp:ListItem>
														<asp:ListItem Value="copy" Selected="True">copy</asp:ListItem>
													</asp:RadioButtonList>
													<br />
													<br />
													<asp:Button ID="MoveCopyAssetsCartButton" Text="OK" CssClass="button" OnCommand="LightboxActionButton_Command" CommandArgument="assets.movecopycart" runat="server" />
													<asp:Button ID="CancelMoveCopyAssetsToCartButton" Text="cancel" CssClass="button" OnClick="CancelAction_Click" runat="server" />
												</span>
												<br />
											</div>
										</asp:Panel>
										
									</div>
								</div>
							</asp:Panel>
							<asp:Panel ID="LightboxControlPanel" Visible="false" runat="server">
								<div class="LbxCtrlHdr">
									<div class="Left CtrlHdrMrg"><span class="CtrlHdrTxt"><span class="Bold">Lightboxes</span> | </span><asp:LinkButton ID="ToggleAssetsOn" Text="Assets" CssClass="CtrlHdrTxt" OnCommand="ToggleControlPanelVisibility" CommandArgument="assets" runat="server" /></div>
									<div class="Right CtrlHdrMrg"><app:HelpHyperLink ID="HelpHyperLink2" HelpPage="3_Lightboxes.htm#managing_lightboxes" CssClass="CtrlHelpTxt" Text="help" runat="server" /></div>
								</div>
								<div class="LbxCtrlBkg">
									<div class="LbxCtrlMrg">
										<span class="BodyTxt H15">
											<br />
											Add a NEW lightbox, set your default lightbox or REMOVE, SEND, DUPLICATE, MERGE or RENAME existing lightboxes.
											<br /><br />
											<app:EnhancedLinkButton ID="NewLightboxLinkButton" Text="New Lightbox" CssClass="PanelTxt" OnCommand="LightboxActionLinkButton_Command" CommandArgument="lightbox.new" runat="server" />
											<app:EnhancedLinkButton ID="RemoveLightboxLinkButton" Text="Remove Lightbox" CssClass="PanelTxt" OnCommand="LightboxActionLinkButton_Command" CommandArgument="lightbox.remove" runat="server" />
											<app:EnhancedLinkButton ID="MergeLightboxLinkButton" Text="Merge Lightboxes" CssClass="PanelTxt" OnCommand="LightboxActionLinkButton_Command" CommandArgument="lightbox.merge" runat="server" />
											<app:EnhancedLinkButton ID="DuplicateLightboxLinkButton" Text="Duplicate Lightbox" CssClass="PanelTxt" OnCommand="LightboxActionLinkButton_Command" CommandArgument="lightbox.duplicate" runat="server" />
											<app:EnhancedLinkButton ID="RenameLightboxLinkButton" Text="Rename Lightbox" CssClass="PanelTxt" OnCommand="LightboxActionLinkButton_Command" CommandArgument="lightbox.rename" runat="server" />
											<app:EnhancedLinkButton ID="SendLightboxLinkButton" Text="Send Lightbox" CssClass="PanelTxt" OnCommand="LightboxActionLinkButton_Command" CommandArgument="lightbox.send" runat="server" />
											<app:EnhancedLinkButton ID="SetDefaultLightboxLinkButton" Text="Set Default Lightbox" CssClass="PanelTxt" OnCommand="LightboxActionLinkButton_Command" CommandArgument="lightbox.makedefault" runat="server" />
											<br />
										</span>
										
										<asp:Panel ID="NewLightboxPanel" Visible="false" runat="server">
											<div class="LbxCtrlFtr">
												<br />
												<app:FeedbackLabel id="NewLightboxFeedbackLabel" LineBreaks="1" runat="server" />
												<span class="BodyTxt Bold">Add A New Lightbox</span><br />
												<span class="BodyTxt">
													Enter a name for new lightbox: <br />(max. 25 characters)<br />
													<asp:TextBox ID="NewLightboxNameTextBox" runat="server" CssClass="formInput W200" MaxLength="25" />
													<br />
													<asp:CheckBox ID="MakeNewLightboxDefaultCheckBox" runat="server" />make default lightbox<br />
													<br />
													<asp:Button ID="SaveNewLightboxButton" Text="save" CssClass="button" OnCommand="LightboxActionButton_Command" CommandArgument="lightbox.new" runat="server" />
													<asp:Button ID="CancelNewLightboxButton" Text="cancel" CssClass="button" OnClick="CancelAction_Click" runat="server" />
												</span>
												<br />
											</div>
										</asp:Panel>
										
										<asp:Panel ID="RemoveLightboxPanel" Visible="false" runat="server">
											<div class="LbxCtrlFtr">
												<br />
												<app:FeedbackLabel id="RemoveLightboxFeedbackLabel" LineBreaks="1" runat="server" />
												<span class="BodyTxt Bold">Remove Lightbox</span><br />
												<span class="BodyTxt">
													Select a lightbox to remove: <br />
													<app:UserLightboxDropDownList ID="RemoveLightboxDropDownList" CssClass="formInput" OmitBlankItem="true" runat="server" />
													<br />
													<br />
													<asp:Button ID="RemoveLightboxButton" Text="remove" CssClass="button" OnCommand="LightboxActionButton_Command" CommandArgument="lightbox.remove" runat="server" />
													<asp:Button ID="CancelRemoveLightboxButton" Text="cancel" CssClass="button" OnClick="CancelAction_Click" runat="server" />
												</span>
												<br />
											</div>
										</asp:Panel>
										
										<asp:Panel ID="MergeLightboxPanel" Visible="false" runat="server">
											<div class="LbxCtrlFtr">
												<br />
												<app:FeedbackLabel id="MergeLightboxFeedbackLabel" LineBreaks="1" runat="server" />
												<span class="BodyTxt Bold">Merge Lightboxes</span><br />
												<span class="BodyTxt">
													Select two lightboxes (source and target). Assets from the source will be copied to the target. If you have no need for the source afterwards, select 'remove source lightbox' and it will be deleted<br />
													<br />
													<span class="Bold">source</span><br />
													<app:UserLightboxDropDownList ID="MergeLightboxSourceDropDownList" CssClass="formInput" OmitBlankItem="true" HideLinkedLightboxes="true" runat="server" />
													<br />
													<br />
													<span class="Bold">target</span><br />
													<app:UserLightboxDropDownList ID="MergeLightboxTargetDropDownList" CssClass="formInput" OmitBlankItem="true" HideLinkedLightboxes="true" runat="server" />
													<br />
													<br />
													<asp:CheckBox ID="MergeLightboxRemoveSourceCheckBox" runat="server" /> remove source lightbox
													<br />
													<br />
													<asp:Button ID="MergeLightboxButton" Text="merge" CssClass="button" OnCommand="LightboxActionButton_Command" CommandArgument="lightbox.merge" runat="server" />
													<asp:Button ID="CancelMergeLightboxButton" Text="cancel" CssClass="button" OnClick="CancelAction_Click" runat="server" />
												</span>
												<br />
											</div>
										</asp:Panel>
										
										<asp:Panel ID="DuplicateLightboxPanel" Visible="false" runat="server">
											<div class="LbxCtrlFtr">
												<br />
												<app:FeedbackLabel id="DuplicateLightboxFeedbackLabel" LineBreaks="1" runat="server" />
												<span class="BodyTxt Bold">Duplicate Lightbox</span>
												<br />
												<span class="BodyTxt">
													Select a source lightbox to duplicate and enter a name for the duplicate:<br />
													<br />
													<span class="Bold">source</span><br />
													<app:UserLightboxDropDownList ID="DuplicateLightboxDropDownList" CssClass="formInput" OmitBlankItem="true" runat="server" />
													<br />
													<br />
													<span class="BodyTxt">
														Enter a name for duplicate lightbox:<br />
														(max. 25 characters)<br />
														<asp:TextBox ID="DuplicateLightboxNameTextBox" CssClass="formInput W200" MaxLength="25" runat="server" />
														<br />
													</span>
													<br />
													<asp:Button ID="DuplicateLightboxButton" Text="duplicate" CssClass="button" OnCommand="LightboxActionButton_Command" CommandArgument="lightbox.duplicate" runat="server" />
													<asp:Button ID="CancelDuplicateLightboxButton" Text="cancel" CssClass="button" OnClick="CancelAction_Click" runat="server" />
												</span>
												<br />
											</div>
										</asp:Panel>
										
										<asp:Panel ID="RenameLightboxPanel" Visible="false" runat="server">
											<div class="LbxCtrlFtr">
												<br />
												<app:FeedbackLabel id="RenameLightboxFeedbackLabel" LineBreaks="1" runat="server" />
												<span class="BodyTxt Bold">Rename Lightbox</span>
												<br />
												<span class="BodyTxt">
													Select a lightbox to rename and enter a new name for the lightbox:<br />
													<app:UserLightboxDropDownList ID="RenameLightboxDropDownList" CssClass="formInput" OmitBlankItem="true" runat="server" />
													<br />
													<br />
													<span class="BodyTxt">
														Enter a new name:<br />(max. 25 characters)<br />
														<asp:TextBox ID="RenameLightboxTextBox" CssClass="formInput W200" MaxLength="25" runat="server" />
														<br />
													</span>
													<br />
													<asp:Button ID="RenameLightboxButton" Text="rename" CssClass="button" OnCommand="LightboxActionButton_Command" CommandArgument="lightbox.rename" runat="server" />
													<asp:Button ID="CancelRenameLightboxButton" Text="cancel" CssClass="button" OnClick="CancelAction_Click" runat="server" />
												</span>
												<br />
											</div>
										</asp:Panel>
										
										<asp:Panel ID="SendLightboxPanel" Visible="false" runat="server">
											<div class="LbxCtrlFtr">
												<br />
												<app:FeedbackLabel ID="SendLightboxFeedbackLabel" LineBreaks="1" runat="server" />
												<span class="BodyTxt Bold">Send Lightbox</span>
												<br />
												<span class="BodyTxt">
													Select a lightbox to send and enter the email address of the recipient. You can leave a message, CC another person and set an expiry date on which the lightbox will no longer be available. (<span class="ReqField">*</span> are required)<br />
													<br />
													<app:FeedbackLabel ID="SendLightboxPanelMessageLabel" runat="server" />
													<br />
													<br />
													<span class="Bold">lightbox <span class="ReqField">*</span></span><br />
													<app:UserLightboxDropDownList ID="SendLightboxDropDownList" CssClass="formInput" OmitBlankItem="true" runat="server" />
													<br />
													<br />
													<span class="Bold">subject <span class="ReqField">*</span></span><br />
													<asp:TextBox ID="SendLightboxMessageSubject" CssClass="formInput W200" MaxLength="150" runat="server" /><br />
													<br />
													<span class="Bold">
														message<br />
														<app:TextArea ID="SendLightboxMessageBody" CssClass="formInput W200" Columns="25" Rows="4" MaxLength="400" runat="server" />
													</span>
													<br />
													<br />
													<span class="Bold">recipient email <span class="ReqField">*</span></span><br />
													Enter an email address below or use the shortcut menu to select an address you've used before:<br />
													<asp:TextBox ID="SendLightboxRecipientEmail" CssClass="formInput W200" MaxLength="2000" runat="server" /><br />
													<app:PreviousLightboxRecipientDropDownList ID="SendLightboxRecipientDropDownList" CssClass="formInput W200" BlankText="New recipient" BlankValue="" runat="server" />
													<br />
													<br />
													<span class="Bold">CC email</span><br />
                                                    Enter other addresses to CC (separate with semi-colons or commas) or use the shortcut menu to select addresses you've used before. Max <%=FocusOPEN.Business.LightboxManager.MaxNumberCCEmails%> addresses.
                                                    <app:TextArea ID="SendLightboxCCEmail" EmailsOnly="true" Rows="5" CssClass="formInput W200" Wrap="true"  runat="server" />																										
                                                    <br />
													<app:PreviousLightboxRecipientDropDownList ID="SendLightboxCCDropDownList" AutoPostBack="true" OnSelectedIndexChanged="SendLightboxCCDropDownList_SelectedIndexChanged" CssClass="formInput W200" BlankText="None or New recipient" BlankValue="" runat="server" />
													<br />
                                                    <br />
                                                    <asp:RadioButtonList ID="SendLightboxTypeRadioButtonList" CssClass="Bold SendLightboxType" RepeatLayout="Flow" RepeatDirection="Vertical" runat="server">
														<asp:ListItem Value="copy" Selected="true">copy lightbox</asp:ListItem>
														<asp:ListItem Value="link">link lightbox</asp:ListItem>
													</asp:RadioButtonList>
                                                    <br />
                                                    <asp:CheckBox ID="SendLightboxEnableLinkedEditCheckBox" ToolTip="linked lightboxes only" Checked="false" Enabled="false" CssClass="Bold" Text="enable recipients to edit lightbox" runat="server" />
                                                    <br />
                                                    <br />
                                                    <asp:PlaceHolder ID="SendLightboxDownloadLinksWrapper" runat="server" Visible="false">
                                                        <asp:CheckBox ID="SendLightboxDownloadLinksCheckBox" Checked="false" CssClass="Bold" Text="enable contact sheet download" runat="server" />
                                                        <br />
													    <br />
                                                    </asp:PlaceHolder>
                                                    <br />
													<span class="Bold">expiry date</span><br />
													<app:DatePicker ID="SendLightboxExpiryDate" AllowDateBeforeToday="false" runat="server" />
													<br />
													<br />
													<asp:Button ID="SendLightboxButton" Text="send" CssClass="button" OnCommand="LightboxActionButton_Command" CommandArgument="lightbox.send" runat="server" />
													<asp:Button ID="CancelSendLightboxButton" Text="cancel" CssClass="button" OnClick="CancelAction_Click" runat="server" />
												</span>
												<br />
											</div>
										</asp:Panel>
										
										<asp:Panel ID="SetDefaultLightboxPanel" Visible="false" runat="server">
											<div class="LbxCtrlFtr">
												<br />
												<app:FeedbackLabel id="SetDefaultLightboxFeedbackLabel" LineBreaks="1" runat="server" />
												<span class="BodyTxt Bold">Default Lightbox</span><br />
												<span class="BodyTxt">
													Select a lightbox to make the default: <br />
													<app:UserLightboxDropDownList ID="SetDefaultLightboxDropDownList" CssClass="formInput" OmitBlankItem="true" HideLinkedLightboxes="true" runat="server" />
													<br />
													<br />
													<asp:Button ID="MakeDefaultLightboxButton" Text="make default" CssClass="button" OnCommand="LightboxActionButton_Command" CommandArgument="lightbox.makedefault" runat="server" />
													<asp:Button ID="CancelMakeDefaultLightboxButton" Text="cancel" CssClass="button" OnClick="CancelAction_Click" runat="server" />
												</span>
												<br />
											</div>
										</asp:Panel>
										
									</div>
								</div>
							</asp:Panel>
						</ContentTemplate>
						<Triggers>
							<aspajax:PostBackTrigger ControlID="RemoveAssetsButton" />
							<aspajax:PostBackTrigger ControlID="MoveCopyAssetsButton" />
							<aspajax:PostBackTrigger ControlID="MoveCopyAssetsCartButton" />
							<aspajax:PostBackTrigger ControlID="SaveNewLightboxButton" />
							<aspajax:PostBackTrigger ControlID="RemoveLightboxButton" />
							<aspajax:PostBackTrigger ControlID="MergeLightboxButton" />
							<aspajax:PostBackTrigger ControlID="DuplicateLightboxButton" />
							<aspajax:PostBackTrigger ControlID="RenameLightboxButton" />
							<aspajax:PostBackTrigger ControlID="SendLightboxButton" />
							<aspajax:PostBackTrigger ControlID="MakeDefaultLightboxButton" />
							<aspajax:AsyncPostBackTrigger ControlID="SendLightboxCCDropDownList" />
						</Triggers>
						</aspajax:UpdatePanel>
					</div>
				</div>
				<div class="Clear"><br /></div>
			</div>
			<div class="CatAppFtr">
                <aspajax:UpdatePanel runat="server" ID="FooterPagerUpdatePanel">
                    <Triggers>
                        <aspajax:AsyncPostbackTrigger controlid="RemoveSelectedAssetsLinkButton1" eventname="Click" />
                        <aspajax:AsyncPostbackTrigger controlid="RemoveSelectedAssetsLinkButton2" eventname="Click" />
                    </Triggers> 
                    <ContentTemplate>
				<div class="Left">
					<div class="AppResults PanelTxt">total results: <span class="Bold"><%=AssetCount %></span> assets [displaying page <%=CurrentPage%> of <%=TotalPages %>]</div>
				</div>
				<div class="Right">
					<div class="AppPageView AppPaging">
						<app:SimplePager ID="BottomPager" OnCommand="Page_IndexChanged" runat="server" />
					</div>
				</div>
                    </ContentTemplate>
                </aspajax:UpdatePanel>
			</div>
		</div>
		<app:PageFooter ID="PageFooter1" HelpPage="index.htm#catalogue" runat="Server" />
	</div>
</form>
</body>
</html>