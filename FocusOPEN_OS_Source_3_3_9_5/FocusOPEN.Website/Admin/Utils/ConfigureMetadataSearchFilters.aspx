<%@ Page Title="Configure Metadata [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="ConfigureMetadataSearchFilters.aspx.cs" Inherits="FocusOPEN.Website.Admin.ConfigureMetadataSearchFilters" %>
<%@ Register Src="~/Controls/Admin/BrandMetadataSettingDetails.ascx" TagName="MetaSettingDetails" TagPrefix="MetaSettingDetailsControl" %>
<%@ Register Src="~/Controls/Template/MetadataFilters.ascx" TagName="MetaFilters" TagPrefix="metafilters" %>

<asp:Content ID="Head1" ContentPlaceHolderID="HeadContent" runat="server">
    
    <script type="text/javascript">

            $(document).ready(function() {

                //collapsable details panel
                $('.expandCollapseDetails').click(function() {
                    //alert('d');
                    var anch = $(this);
                    $(this).nextAll('div').slideToggle(200, function() {

                        var labelText = $(anch).text();

                        if (labelText.indexOf('[+]') != -1)
                            $(anch).text(labelText.replace('[+]', '[-]'));
                        else
                            $(anch).text(labelText.replace('[-]', '[+]'));

                    });

                    return false;
                });

            });
			
	</script>
	
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg"> 
		<div class="Left AdmLeftPanel"> 
			<div id="PageHeaderDiv" runat="server" class="AdmAppHdr">Metadata Filters for focus open</div>
			<app:FeedbackLabel ID="GeneralFeedbackLabel" UseContainer="true" LineBreaks="0"  runat="server" />
			<br />
			
            <asp:MultiView ID="MainMultiView" runat="server" ActiveViewIndex="0">
                <asp:View ID="EditorView1" runat="server">
                
			            <div class="AppCtrlMrg PanelTxt">
            			
				            <app:MultiBrandPlaceHolder runat="server">
					            <a href="ConfigureMetadata.aspx?redirect=search-filters" class="PanelTxt Bold">&laquo; Back to configure metadata brand selection</a>
					            <br /><br />
				            </app:MultiBrandPlaceHolder>
            				
				            <strong> -  Pick required filters for each of the available 12 positions                        </strong><br />
                            <strong> -  Select a filter with the adaoent radio button and choose drop down or multi-select  </strong><br />
                            <strong> -  Use Filter Control Mark-up to modify Filter control HTML (if required)              </strong><br />
                            <strong> -  Test Changes to preview filter before saving or cancelling | <app:HelpHyperLink ID="HelpHyperLink1" HelpPage="10_Utilities.htm#configure_metadata" CssClass="BodyTxt" Text="help" runat="server" />                     </strong><br />
			            </div>
            			
			            <br />
            			
			            <div style="margin-left:12px">
            				
				            <table border="0" cellpadding="5" cellspacing="0">
				                <tr>
					                <td style="width:310px" class="PanelTxt"><asp:CheckBox ID="EnableFiltersCheckbox" runat="server" /> Enable filters</td>
				                </tr>
				            </table>
            						
			            </div>
            		
            		
		            <%--<div class="Left AdmLeftPanel"></div>
		            <div class="Right AdmRightPanel"></div>--%>
            		
		            <div class="AdmTblPanel Clear">
			            <div class="Clear">
				            <div class="AppCtrlMrg">
            					
            					
                                <asp:DataList ID="GroupsDataList" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                                 Width="500px" OnItemDataBound="GroupsDataList_ItemDataBound">
                                    <ItemTemplate>
                                        <table class="PanelTxt">
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>
                                                    <asp:Label ID="GroupLabel" runat="server" Text="Group "></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="GroupRadioButtonLabel" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="GroupDropDownList" 
                                                                      runat="server" 
                                                                      AppendDataBoundItems="true"
                                                                      DataValueField="BrandMetadataSelectableSettingId"
                                                                      DataTextField="FieldName"
                                                                      CssClass="formInput W125"
                                                                      AutoPostBack="true"
                                                                      ValidationGroup="Group@@@"
                                                                      OnSelectedIndexChanged="GroupDropDownList_SelectedIndexChanged"
                                                                      >
                                                        <asp:ListItem Text="None" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:DataList>
            					
					              <br />
            					    
					              <div class="AppCtrlMrg PanelTxt" style="text-align: left;">
                                    
                                    <asp:Panel ID="GroupDetailsPanel" runat="server" Visible="false">
                                        
                                        <hr style="width: 600px; margin-top: 0px; float: left;" />    
                                        
                                        <br />
                                        
                                        <div class="PanelTxt Bold">Change Selected Filter to:</div>
                                        
                                        <br />
                                        
                                        <table border="0" cellpadding="3" cellspacing="0">
				                            <tr>
					                            <td class="PanelTxt" style="vertical-align: top;">
            					                
                                                    <asp:RadioButton ID="Option1RadioButton" runat="server" GroupName="GroupDetails" OnCheckedChanged="OptionsRadioButton_OnCheckedChanged" AutoPostBack="true" />
                                                    
					                            </td>
					                            <td class="PanelTxt">
                        					    
                                                    <app:MetadataInputWrapper ID="PreviewMetaInputDropDown" runat="server" />
                                                    
					                            </td>
                        					    
				                            </tr>
				                            <tr>
					                            <td class="PanelTxt" style="vertical-align: top;">
            					                    <asp:RadioButton ID="Option2RadioButton" runat="server" GroupName="GroupDetails" OnCheckedChanged="OptionsRadioButton_OnCheckedChanged" AutoPostBack="true" />
					                            </td>
					                            <td class="PanelTxt">
                        					        
                                                    <%--<asp:ListBox ID="DemoListBox" runat="server" CssClass="formInput W110"></asp:ListBox>--%>
                                                    
                                                    <app:MetadataInputWrapper ID="PreviewMetaInputList" runat="server" />
                                                    
                                                    <br />
                                                    
                                                    <asp:DropDownList ID="RowsDropDownList" runat="server" CssClass="formInput W110"
                                                                      AutoPostBack="true"
                                                                      OnSelectedIndexChanged="RowsDropDownList_SelectedIndexChanged">
                                                        <asp:ListItem Text="2 rows" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="3 rows" Value="3" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="4 rows" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="5 rows" Value="5"></asp:ListItem>
                                                        <asp:ListItem Text="6 rows" Value="6"></asp:ListItem>
                                                        <asp:ListItem Text="7 rows" Value="7"></asp:ListItem>
                                                        <asp:ListItem Text="8 rows" Value="8"></asp:ListItem>
                                                        <asp:ListItem Text="9 rows" Value="9"></asp:ListItem>
                                                        <asp:ListItem Text="10 rows" Value="10"></asp:ListItem>
                                                    </asp:DropDownList> 
                                                    
                                                   <%-- <br />
                                                    
                                                    <asp:Button ID="ApplyButton" OnClick="ApplyButton_Click" CssClass="button" Text="Apply" runat="server" />
                                                    --%>
					                            </td>
				                            </tr>
				                        </table>
                                        
                                    
                                    </asp:Panel>
                        		    
            		                <br />
                        		    
            		                <a href="#" class="expandCollapseDetails PanelTxt Bold" id="ExpandCollapseDetailsAnchor" runat="server"><asp:Literal runat="server" ID="NameSignLiteral">[+]</asp:Literal> Modify Filter Control Mark-up</a> 
            								    
						            <div class="detailsContainer detailsContainer2" id="DetailsContainerDiv" runat="server">
            						        
                                        						        
                                        <asp:TextBox ID="MarkupTextBox" runat="server"
                                                             Rows="16"
                                                             Columns="100"
                                                             TextMode="MultiLine"
                                                            >
                                        </asp:TextBox>
                                        
                                        <br />
                                        <br />
                                        
                                        <asp:Button ID="LoadDefaultTemplateButton" OnClick="LoadDefaultTemplateButton_Click" CssClass="button" Text="Reset to Default" runat="server" />
                                        
						            </div>
                        		   
		                            </div>
            		                
		                            <br />
            									
					            <br />
            					  
            					
            					
			                    <div style="margin-left:12px">
                    			  
				                <asp:Button ID="TestChangesButton" OnClick="TestChangesButton_Click" CssClass="button" Text="Test Changes" runat="server" />
				                <app:CancelButton ID="CancelButton" CssClass="button" Text="Cancel Changes" Prompt="Are you sure? Changes will not be saved." RedirectUrl="ConfigureMetadata.aspx?redirect=search-filters" runat="server" />
                        			
			                    </div>
            			               
				               <br /><br />
            				   
				            </div>
			            </div>
		            </div>
		            
		        </asp:View>
                <asp:View ID="PreviewView2" runat="server">
                
                      <div class="AdmTblPanel Clear">
			                <div class="Clear">
				                <div class="AppCtrlMrg">
                                     
                                     <asp:LinkButton ID="BackToEditorLinkButton" runat="server" cssclass="PanelTxt Bold" OnClick="CancelPreviewButton_Click">Go back to filter editor</asp:LinkButton>
                                     
                                     <br />
                                     <br />
                                     
				                     <div class="CatSrcAllPanel" style="border: 0; border-collapse: collapse;"> 					
					                     <div class="Clear">
                                              
                                              <metafilters:MetaFilters ID="MetadataFilters" runat="server" />
                                              
                                              <br />
                                         </div> 
			                         </div>
			                         
			                         <br />
			                         
			                         <div style="margin-left:0px">
                                     	 
	                                     <asp:Button ID="MakeLiveButton" OnClick="MakeLiveButton_Click" CssClass="button" Text="Make Changes Live" runat="server" />
	                                     <asp:Button ID="CancelPreviewButton" OnClick="CancelPreviewButton_Click" CssClass="button" Text="Cancel & Return To Editor" runat="server" />
                                        	
                                     </div>
                                              
			                    </div>
		                    </div>
		                </div>
                </asp:View>
            </asp:MultiView>
		
		</div>
	</div>
	
	<div class="Clear">&nbsp;</div>
</asp:Content>
