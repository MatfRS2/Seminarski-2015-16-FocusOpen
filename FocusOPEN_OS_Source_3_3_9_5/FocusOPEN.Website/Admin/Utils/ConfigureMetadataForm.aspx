<%@ Page Title="Configure Metadata [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="ConfigureMetadataForm.aspx.cs" Inherits="FocusOPEN.Website.Admin.ConfigureMetadataForm" %>
<%@ Register Src="~/Controls/Admin/BrandMetadataSettingDetails.ascx" TagName="MetaSettingDetails" TagPrefix="MetaSettingDetailsControl" %>

<asp:Content ID="Head1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="<%=ResolveUrl("~/Includes/Javascript/jQuery/jquery.tablednd_0_5.js")%>"></script>
    <script type="text/javascript">

        var mode = '<%= (int)Mode %>';
        
        function InitControls() {

            //============= expandCollapse clicked
            $('.expandCollapseDetails').unbind('click');
            $('.expandCollapseDetails').click(function() {
                var anch = $(this);
                $(this).nextAll('.detailsContainer, .detailsContainer2').slideToggle(200, function() {

                    var labelText = $(anch).text();

                    if (labelText.indexOf('[+]') != -1)
                        $(anch).text(labelText.replace('[+]', '[-]'));
                    else
                        $(anch).text(labelText.replace('[-]', '[+]'));

                });

                return false;
            });

            $('.updownimagebuttons').click(function() {

                var butt = $(this);
                var row = $(butt).parents('.existingMetaRow:first');
                var nxt = $(row).nextAll('.existingMetaRow:first');
                var prv = $(row).prevAll('.existingMetaRow:first');
                //var nxt = $(butt).parents('.existingMetaRow:first').nextAll('.existingMetaRow:first');
                //var prv = $(butt).parents('.existingMetaRow:first').prevAll('.existingMetaRow:first');
                var swappedA = $(row).find('input[type="hidden"]:first').val();
                var swappedB = '-1';

                if (butt.attr('id').toLowerCase().indexOf("uporderimagebutton") != -1) {
                    $(row).insertBefore($(prv));
                    swappedB = $(prv).find('input[type="hidden"]:first').val();
                }
                else {
                    $(row).insertAfter($(nxt));
                    swappedB = $(nxt).find('input[type="hidden"]:first').val();
                }

                var options = {
                    type: "POST",
                    url: "ConfigureMetadataForm.aspx/MoveUpDown",
                    data: "{'swappedA' : '" + swappedA + "', 'swappedB' : '" + swappedB + "', 'mode' : '" + mode + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(response) {
                        
                        $('.rowLabels').hide();
                        $(row).find('.rowLabels').toggle();

                    }
                };

                //fire
                $.ajax(options);


                AlternateRowStyles();

                return false;
            });

            $('#SettingsTable').tableDnD({
                onDrop: function(table, row) {
                    alert($(this).tableDnDSerialize());
                },
                dragHandle: "draggableCell",
                onDragStart: function() {
                    
                    $('.rowLabels').hide();
                    
                    }, 
                onDrop: function(table, row) {

                    AlternateRowStyles();

                    var rows = table.tBodies[0].rows;
                    var idsStr = "Row dropped was " + row + ". New order: ";

                    for (var i = 0; i < rows.length; i++) {
                        idsStr += $(rows[i]).find('input[type="hidden"]:first').val();
                        idsStr += ',';
                    }

                    var options = {
                        type: "POST",
                        url: "ConfigureMetadataForm.aspx/DragMeta",
                        data: "{'ids' : '" + idsStr + "', 'mode' : '" + mode + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function(response) {

                            $('.rowLabels').hide();
                            $(row).find('.rowLabels').toggle();

                        }
                    };

                    //fire
                    $.ajax(options);
                    
                    $(row).find('.rowLabels').toggle();
                }
            });
            

            //============ autostripe logic from global so that the repeater doesn't get stripped out of the alternating pattern upon ajax postback
            AlternateRowStyles();

        }
        function AlternateRowStyles() {
            $("table.AutoStripe tr td").css("height", "25px");
            $("table.AutoStripe tr:even").find("td").removeClass("TblCell2").addClass("TblCell1");
            $("table.AutoStripe tr:odd").find("td").removeClass("TblCell1").addClass("TblCell2");
        }
        
        function HideRow(divId) {

            $('#' + divId).parents('.existingMetaRow').hide();
        
        }
        function OpenDetails(divId) {

            $('#' + divId).show();
            
            var labelText = $('#' + divId).prevAll('.expandCollapseDetails:first').text();

            alert(divId + ' ' + labelText);
            
            $('#' + divId).prevAll('.expandCollapseDetails:first').text(labelText.replace('[+]', '[-]'));
        }
        
        $(document).ready(function() {

            setupDatePicker();

            InitControls();

        });
			
		</script>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg"> 
		<div class="Left AdmLeftPanel"> 
			<div id="PageHeaderDiv" runat="server" class="AdmAppHdr">Configure Metadata</div>
			<app:FeedbackLabel ID="GeneralFeedbackLabel" UseContainer="true" LineBreaks="1" runat="server" />
			<br />
			<div class="AppCtrlMrg PanelTxt">
			
				<app:MultiBrandPlaceHolder runat="server">
					<a href="ConfigureMetadata.aspx" class="PanelTxt Bold">&laquo; Back to configure metadata brand selection</a>
					<br /><br />
				</app:MultiBrandPlaceHolder>
				
				<strong> - Click Catalogue to change the order of fields when cataloguing</strong><br />
				<strong> - Click Asset Detail to change the order and visibility of fields end users see in asset detail</strong><br />
				<strong> - Renamed fields and ordering changes take immediately</strong><br />
				<strong> - Click on 'Save Metadata Scheme' below to save other changes</strong>
				<strong> | <app:HelpHyperLink HelpPage="10_Utilities.htm#configure_metadata" CssClass="BodyTxt" Text="help" runat="server" /></strong>
				
			</div>
			<br />
		</div>
		<div class="Left AdmLeftPanel"> 
		    
		    <div class="AppCtrlMrg PanelTxt">
                <asp:RadioButtonList ID="ConfigurationModeRadios" 
                            runat="server" 
                            CssClass="PanelTxt" 
                            RepeatDirection="Horizontal"
                            OnSelectedIndexChanged="ConfigurationModeRadios_SelectedIndexChanged"
                            AutoPostBack="true">
                    <asp:ListItem Text="Catalogue" Value="Catalogue" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Asset Detail" Value="AssetDetail"></asp:ListItem>
                </asp:RadioButtonList>
		    </div>
		    <br />
		</div>

		<div class="Right AdmRightPanel"></div>
		<div class="AdmTblPanel Clear">
			<div class="Clear">
				<div class="AppCtrlMrg">
					
					<asp:Repeater ID="MetadataRepeater" OnItemDataBound="MetadataRepeater_ItemDataBound" runat="server">
						<HeaderTemplate>
							<table ID="SettingsTable" border="0" cellpadding="5" cellspacing="0" class="AutoStripe TblOutline AppTblMrg">
							<tr>
								<td style="width:200px" class="PanelTxt alignTop"><strong>Field</strong></td>
								<td style="width:70px" class="PanelTxt"><strong>Type</strong></td>
								<td style="width:75px" class="PanelTxt alignCentre" runat="server" id="CatalogueHeaderTd"><strong>Catalogue</strong></td>
								<td style="width:75px" class="PanelTxt alignCentre" runat="server" id="RequiredHeaderTd"><strong>Required</strong></td>
								<td style="width:75px" class="PanelTxt alignCentre" runat="server" id="AssetDetailHeaderTd"><strong>Asset Detail</strong></td>
								<td style="width:30px" class="PanelTxt alignCentre borderless">&nbsp;</td>
								<td style="width:30px" class="PanelTxt alignCentre borderless">&nbsp;</td>
								<td style="width:50px" class="PanelTxt alignCentre borderless">&nbsp;</td>
							</tr>
						</HeaderTemplate>
						<FooterTemplate>
						    <tr id="FooterRow" runat="server">
								<td class="PanelTxt Bold" style="height:30px">
								
								    <asp:HiddenField ID="BrandMetadataSettingIdHiddenField" runat="server" />
								    
								    <a href="#" class="expandCollapseDetails BodyTxt" id="ExpandCollapseDetailsAnchor" runat="server"><asp:Literal runat="server" ID="NameSignLiteral">[+]</asp:Literal> Add New Field</a> 
								    
								    <div class="detailsContainer detailsContainer2" id="DetailsContainerDiv" runat="server">
									
									<br />
									
									<MetaSettingDetailsControl:MetaSettingDetails ID="ucEditDetails" runat="server" />
									
									<br />
									
									<span style="font-size:0.9em;">
										&nbsp;&nbsp;
										<asp:LinkButton ID="SaveFieldNameLinkButton" OnClick="SaveFieldNameLinkButton_Click" CssClass="black" Text="save" runat="server" />
										&nbsp;&nbsp;
										<app:PromptLinkButton ID="CancelFieldNameLinkButton" OnClick="CancelFieldNameLinkButton_Click" Prompt="Are you sure? Changes will not be saved." CssClass="black" Text="cancel" runat="server" />
									</span>
									<br />
									<br />
											
									</div>
								    
								</td>
								<td class="PanelTxt alignTop">
									
									<asp:DropDownList ID="UiControlTypeDropDown" runat="server" DataTextField="Value" DataValueField="Key" />
									
								</td>
								<td class="PanelTxt alignCentreTop">
									<asp:Label ID="CatalogueLabel" runat="server" />
									<asp:CheckBox ID="CatalogueCheckBox" runat="server" />
								</td>
								<td class="PanelTxt alignCentreTop">
									<asp:Label ID="RequiredLabel" runat="server" />
									<asp:CheckBox ID="RequiredCheckBox" runat="server" />
								</td>
								<%--<td class="PanelTxt alignCentreTop">
									<asp:Label ID="AssetDetailLabel" runat="server" />
									<asp:CheckBox ID="AssetDetailCheckBox" runat="server" />
								</td>--%>
								<td class="borderless" colspan="3">
								    &nbsp;
								</td>
						    </tr>
							</table>
						</FooterTemplate>
						<ItemTemplate>
						    <tr class="existingMetaRow">
								<td class="PanelTxt Bold" style="height:30px">
								
									<asp:HiddenField ID="BrandMetadataSettingIdHiddenField" runat="server" />
									
									<aspajax:UpdatePanel ID="UpdatePanel2" ChildrenAsTriggers="true" runat="server">
					                <ContentTemplate>
					
									<a href="#" class="expandCollapseDetails BodyTxt" id="ExpandCollapseDetailsAnchor" runat="server"><asp:Literal runat="server" ID="NameSignLiteral">[+]</asp:Literal> <asp:Label ID="FieldNameLabel" runat="server" /></a>
									
									<asp:Label ID="FeedbackLabel1" runat="server" ForeColor="Gray" CssClass="FbkPosTxt" style="margin: 0 0 5px 30px; padding: 0 0 5px 0;" />
									
									<div class="detailsContainer detailsContainer2" id="DetailsContainerDiv" runat="server">
                                    
                                    <br />
                                    
                                    <MetaSettingDetailsControl:MetaSettingDetails ID="ucEditDetails" runat="server" />
                                    
									<br />
							
										
										<asp:Panel ID="EditPanel" runat="server">
											<span style="font-size:0.9em;">
												&nbsp;&nbsp;
												<asp:LinkButton ID="SaveFieldNameLinkButton" OnClick="SaveFieldNameLinkButton_Click" CssClass="black" Text="save" runat="server" />
												&nbsp;&nbsp;
												<app:PromptLinkButton ID="DeletePromptLinkButton" OnClick="DeletePromptLinkButton_Click" Prompt="Are you sure? Metadata field and all existing data will be deleted." CssClass="black" Text="delete" runat="server" />
												&nbsp;&nbsp;
												<app:PromptLinkButton ID="CancelFieldNameLinkButton" OnClick="CancelFieldNameLinkButton_Click" Prompt="Are you sure? Changes will not be saved." CssClass="black" Text="cancel" runat="server" />
											</span>
											<br />
											<br />
										</asp:Panel>
									
									</div>
									
									</ContentTemplate>
					                </aspajax:UpdatePanel>
						    
								</td>
								<td class="PanelTxt alignTop">
									<asp:Label ID="DescriptionLabel" runat="server" />
								</td>
								<td class="PanelTxt alignCentreTop" runat="server" id="CatalogueTd">
									<asp:Label ID="CatalogueLabel" runat="server" />
									<asp:CheckBox ID="CatalogueCheckBox" runat="server" />
								</td>
								<td class="PanelTxt alignCentreTop" runat="server" id="RequiredTd">
									<asp:Label ID="RequiredLabel" runat="server" />
									<asp:CheckBox ID="RequiredCheckBox" runat="server" />
								</td>
								<td class="PanelTxt alignCentreTop" runat="server" id="AssetDetailTd">
									<asp:Label ID="AssetDetailLabel" runat="server" />
									<asp:CheckBox ID="AssetDetailCheckBox" runat="server" />
								</td>
								<td class="PanelTxt alignCentreTop borderless" style="">
									
                                    <asp:ImageButton ID="UpOrderImageButton" 
                                                        runat="server" 
                                                        CssClass="updownimagebuttons"
                                                        ImageUrl="~/Images/icons/bullet_arrow_up.png" 
                                                        AlternateText="move order up" 
                                                        ToolTip="move order up" 
                                                        OnClientClick="javascript:return false;"
                                                        OnClick="UpDownImageButton_Command" />
                                    <asp:ImageButton ID="DownOrderImageButton" 
                                                        runat="server" 
                                                        CssClass="updownimagebuttons"
                                                        ImageUrl="~/Images/icons/bullet_arrow_down.png" 
                                                        AlternateText="move order down" 
                                                        ToolTip="move order down" 
                                                        OnClientClick="javascript:return false;"
                                                        OnClick="UpDownImageButton_Command" />
                               	</td>
								<td class="draggableCell alignCentreTop borderless" style="width: 30px;">
								    <asp:ImageButton ID="DragImageButton" 
                                                        runat="server" 
                                                        ImageUrl="~/Images/icons/drag_handle.png" 
                                                        AlternateText="drag to change order quickly" 
                                                        ToolTip="drag to change order quickly" 
                                                        OnClientClick="javascript:return false;"
                                                         />  
								</td>
								<td class="PanelTxt alignCentreTop borderless">
								    <span class="FbkPosTxt rowLabels" style="display: none;">saved</span>
								</td>
							</tr>
							
						</ItemTemplate>
					</asp:Repeater>

					
					<br />
					<div style="margin-left:12px">
						<table border="0" cellpadding="5" cellspacing="0">
						<tr>
							<td style="width:310px" class="PanelTxt">Hide filter search for this brand</td>
							<td><asp:CheckBox ID="HideFilterSearchCheckBox" runat="server" /></td>
						</tr>
						<tr>
							<td class="PanelTxt">Hide category search for this brand</td>
							<td><asp:CheckBox ID="HideCategorySearchCheckBox" runat="server" /></td>
						</tr>
						</table>
						
						<br />
						
						<asp:Button ID="SaveButton" OnClick="SaveButton_Click" CssClass="button" Text="Save Metadata Scheme" runat="server" />
						<app:CancelButton ID="CancelButton" CssClass="button" Text="Cancel Changes" Prompt="Are you sure? Metadata settings will not be saved." RedirectUrl="ConfigureMetadata.aspx" runat="server" />
						
					</div>
					<br /><br />
				</div>
			</div>
		</div>
	</div>
	<br />
</asp:Content>
