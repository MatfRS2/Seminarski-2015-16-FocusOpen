<%@ Page Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="ManageMetadataValuesDetail.aspx.cs" Inherits="FocusOPEN.Website.Admin.ManageMetadataValuesDetail" %>
<%@ Register Src="~/Controls/Admin/SelectableMetadataSettingDetails.ascx" TagName="SelectableMetaSettingDetails" TagPrefix="SelectableMetaSettingDetailsControl" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<asp:Content ID="PageHeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="../../Includes/Javascript/jsTree/tree.lib.js"></script>
    <script type="text/javascript" src="../../Includes/Javascript/jsTree/tree_component.min.js"></script>
    <link rel="stylesheet" type="text/css" href="../../Includes/Javascript/jsTree/tree_component.css" />
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

	<script type="text/javascript">
        var ChangeAllowed = true;
        
        $(document).ready(function()
        {
            $("#CancelButton").click(function()
            {
                var result = confirm("Are you sure you want to cancel the update?");
                if (result)
                {
                    OnCancelButtonClicked();
                }
            }).hide();

            $("#DeleteButton").click(function()
            {
                $.tree_reference("metadataTree").remove();
            });
            
            EnableButton("<%=UpdateButton.ClientID %>", false);
            EnableButton("AddButton", false);
            
            MetadataFetched();
            
            //collapsable details panel
            $('#ExpandCollapseDetailsAnchor').click(function() {
                
                var anch = $(this);
                $('.selectableLogicContainer').slideToggle(200, function() {
                //$(this).nextAll('div').slideToggle(200, function() {

                    var labelText = $(anch).text();

                    if (labelText.indexOf('[+] Show') != -1)
                        $(anch).text(labelText.replace('[+] Show', '[-] Hide'));
                    else
                        $(anch).text(labelText.replace('[-] Hide', '[+] Show'));

                });

                return false;
            });
            
        });
        
		function MetadataFetched(jsonResponse)
		{
			$("#metadataTree").tree({
				rules:
				{
					draggable: "all",
					renameable: "none",
					deletable: "all",
					creatable: "all"
				},
				data:
				{
					type: "json",
					async: false,
					json: <%=GetAllMetadata(BrandId)%>
				},
				callback:
				{
					beforedelete: BeforeDelete,
					onselect: OnSelect,
					onmove: OnMove,
					beforemove: BeforeMove,
					beforechange: BeforeChange
				},
				cookies:
				{
					prefix: "metadataTree",
					opts: { path: '/'}
				},
				ui:
				{
					context: false
				}
			});
			
			var selectedMetadataId = $("#<%=MetadataIdTextBox.ClientID %>").val();

			if (selectedMetadataId != "" && selectedMetadataId > 0)
			{
				$.tree_reference("metadataTree").select_branch($("#" + selectedMetadataId));
			}
			else
			{
				$("#metadataTree").height($("#metadataTree").parent("td").height());
			}
		}

		function BeforeMove(item, refnode)
		{
			if (!ChangeAllowed)
			{
				alert("This metadata item cannot be moved");
				return false;
			}
			
			return true;
		}

		function BeforeChange()
		{
			return ChangeAllowed;
		}

		function BeforeDelete(node, treeobj)
		{
			var tree = $.tree_reference("metadataTree");
			var n = tree.get_node(node);
			
			if (node.id > 0)
			{
				if (!tree.parent(node) && n.siblings().length == 0)
				{
					alert("This is the only root metadata item and cannot be deleted.");
					return false;
				}
				
				if (tree.children(node).length > 0)
				{
					alert("This metadata item has children and cannot be deleted.");
					return false;
				}
				
				var msg = "Are you sure you want to delete the metadata item:\n\n\"" + $("#<%=NameTextBox.ClientID%>").val() + "\"\n\nThis operation cannot be undone.";
				
				if (confirm(msg))
				{
					PageMethods.DeleteMetadata(node.id, function(result){
						var n = $("#metadataTree").find("li#" + result.MetadataId);
						
						if (result.Success)
						{
							n.remove();
						}
						else
						{
							alert(result.Message);
						}
					});
				}
			}
			else
			{
				if (node.id == "NewMetadata")
					return true;
			};
			return false;
		}

		function OnMove(node, treeobj, RB)
		{
			var tree = $.tree_reference("metadataTree");
			var parent = tree.parent(node);
			var parent_id = (parent != null && parent) ? parent.attr("id") : 0;
			var position = GetNodeOrder(node);
			
			PageMethods.MoveMetadata(node.id, parent_id, position)
		}

		function OnSelect(node, treeobj)
		{
			if (node.id != "NewMetadata")
			{
				PageMethods.GetMetadata(node.id, MetadataInfoFetched);

				EnableButton("AddButton", true);
				EnableButton("<%=UpdateButton.ClientID %>", true);
			}
		}

		function MetadataInfoFetched(metadataJson)
		{
			var metadata = eval(' (' + metadataJson + ') ');
			var synonyms = ProcessSynonyms(metadata.synonyms);
			var trimmed = synonyms.replace(/^\s+|\s+$/g, '') ;

			$("#<%=MetadataIdTextBox.ClientID %>").val(metadata.MetadataId);
			$("#<%=NameTextBox.ClientID %>").val(metadata.Name);
			$("#<%=ExternalRefTextBox.ClientID %>").val(metadata.ExternalRef);
			$("#<%=SynonymsTextBox.ClientID %>").val(trimmed);
		}
	    
	    function GetNodeOrder(node)
		{
			var tree = $.tree_reference("metadataTree");
			
			if (node.previousSibling != null)
			{
				var parentNode = tree.get_node(node.previousSibling);
				var previousOrder = parseInt(parentNode.attr("metadataOrder"));
				return previousOrder + 1;
			}
			return 1;
		}
		
		function GetNodeText(node)
		{
			if (node != null)
			{
				var visibleChild = $(node).children("a:visible");

				if (visibleChild != null)
				{
					return visibleChild.text();
				}
				else
				{
					return node.children[0].innerText;
				}
			}

			return null;
		}

		function AddMetadata()
		{
			ClearFields();
	        
			var parentId = $.tree_reference("metadataTree").selected.attr("id");
			$("#<%= ParentIdTextBox.ClientID %>").val(parentId);
	        
			var tree = $.tree_reference("metadataTree");
			var defaultName = 'New Metadata';
			tree.create({ attributes: { id: "NewMetadata" }, data: defaultName });
			tree.select_branch($("#NewMetadata"));

			var nameTextBox = $("#<%= NameTextBox.ClientID %>");
			nameTextBox.val(defaultName);
			nameTextBox.get(0).focus();
			nameTextBox.get(0).select();

			EnableAddNewMetadataMode(true);
		}

		function EnableAddNewMetadataMode(value)
		{
			EnableButton("AddButton", !value);
			EnableButton("<%=UpdateButton.ClientID %>", value);
			ChangeAllowed = !value;
	    
			if (value)
			{
				$("#CancelButton").show();
				$("#DeleteButton").hide();
			}
			else
			{
				$("#CancelButton").hide();
				$("#DeleteButton").show();
			}
		}
	    
		function EnableButton(buttonId, value)
		{
			var color = (value) ? "#000000" : "#c0c0c0";
			$("#" + buttonId).css("color", color).attr("disabled", !value);
		}

		function OnCancelButtonClicked()
		{
			var tree = $.tree_reference("metadataTree");
			ClearFields();
			EnableAddNewMetadataMode(false);
			tree.remove();
		}

		function ClearFields()
		{
			$("#<%= MetadataIdTextBox.ClientID %>").val("");
			$("#<%= NameTextBox.ClientID %>").val("");
			$("#<%= SynonymsTextBox.ClientID %>").val("");
		}

		function MoveUp()
		{
			var tree = $.tree_reference("metadataTree");
			var selectedNode = tree.selected;
			var parentNode = tree.parent(selectedNode);
			var nodeIndex = GetNodeIndex(selectedNode);

			if (parentNode != null && parentNode != false && nodeIndex > 0)
			{
				tree.cut();
				tree.paste(parentNode, nodeIndex - 1);
			}  
		}

		function MoveDown()
		{
			var tree = $.tree_reference("metadataTree");
			var selectedNode = tree.selected;
			var parentNode = tree.parent(selectedNode);
			var nodeIndex = GetNodeIndex(selectedNode);
			var isLast = tree.next(selectedNode, true) == false;
			
			if (parentNode != null && parentNode != false && !isLast)
			{
				tree.cut();
				tree.paste(parentNode, nodeIndex + 2);
			}  
		}

		function MoveLeft()
		{
			var tree = $.tree_reference("metadataTree");
			var selectedNode = tree.selected;
			var parentNode = tree.parent(selectedNode);
			
			if (parentNode != null)
			{
				var grandParent = tree.parent(parentNode);
				
				if (grandParent != null)
				{
					tree.cut();
					tree.paste(grandParent);
				}
			}
		}

		function GetNodeIndex(node)
		{
			var tree = $.tree_reference("metadataTree");
			var prevNode = tree.prev(node, true);
			var index = 0;
			
			while (prevNode != false)
			{
				index++;
				prevNode = tree.prev(prevNode, true);
			}
			
			return index;
		}

		function MoveRight()
		{
			var tree = $.tree_reference("metadataTree");
			var selectedNode = tree.selected;
			var prevNode = tree.prev(selectedNode, true);
			var parentNode = tree.parent(selectedNode);
	        
			if (prevNode != null)
			{
				tree.cut();
				tree.paste(prevNode);
			}
		}

		function ProcessSynonyms(synonyms)
		{
			var result = '';
			var splitResult = synonyms.split(';');
			
			for (i = 0; i < splitResult.length; i++)
			{
				result += splitResult[i] + '\n';
			}              
			
			return result;
		}
    
    </script>
    
	<asp:HiddenField ID="MetadataIdTextBox" runat="server" />
	<asp:HiddenField ID="ParentIdTextBox" runat="server" />

	<div class="AdmBkg"> 
		<div class="Left W400">
			<div class="AdmAppHdr" id="PageHeader" runat="server">Manage Metadata</div><br />
			<app:FeedbackLabel ID="MessageLabel1" UseContainer="true" runat="server" />
			<div class="AppCtrlMrg">
				<asp:HyperLink ID="ManageMetadataHyperlink1" CssClass="PanelTxt Bold" Text="&laquo; Back to metadata list" runat="server" />
				<br /><br />
				<span class="Bold BodyTxt">
					- Drag & drop metadata items to move them, or use the buttons.<br />
					&nbsp;&nbsp;(Metadata items are always order alphabetically)<br />
                    - New metadata items can be added at any level.<br />
                    - Only metadata items with no children can be deleted
                    |
					<app:HelpHyperLink HelpPage="9_Manage_Content.htm#manage_metadata" CssClass="BodyTxt" Text="help" runat="server" /><br />
				</span>
				
				<br />
				
				<div>
				
				    <SelectableMetaSettingDetailsControl:SelectableMetaSettingDetails ID="ucEditDetails" runat="server" />
				    
		    	</div>
				
				
				<br />
				
			</div>
		</div>
		<div class="Right" style="width: 410px; margin: 177px 152px 0 0;">
		    <div style="" class="selectableLogicContainer" id="PreviewContainerDiv" runat="server" visible="false">
				    <div class="PanelTxt">
				    Preview<br />
				    
				    The metadata control will look like *:<br />
				    
				    <app:MetadataInputWrapper ID="PreviewMetadataInputWrapper" runat="server" />
				      
				    <br />
				    <span style="font-size: 11px; font-style: italic; width: 300px;">* demonstrates the control with the currently entered settings - click save to persist these to the database</span>
				    </div>
		    </div>
		</div>
        <div style="margin: 30px 0 5px 38px; clear: both;">
                <asp:LinkButton ID="PreviewControlHyperlink" CssClass="PanelTxt" Text="Preview control" runat="server" OnClick="PreviewControlHyperlink_Click" />
            </div>
            
			<div style="margin-top:10px;margin-bottom:0px;margin-left:38px">
				
				<asp:Button ID="SaveSettingsButton" Text="Save" CssClass="button" OnClick="SaveSettingsButton_Click" runat="server" />
				
	    </div>
			
		<div class="AdmTblPanel Clear">
			<table style="width: 100%">
                <tr>
                    <td valign="top">
                        <table>
                            <tr>
                                <td>
                                    <div class="Clear">
                                        <span class="BodyTxt Bold alignRight Top W125">Reorder metadata</span>
                                        <input id="LeftButton" type="button" value="Left" class="button" onclick="MoveLeft();" />&nbsp;
                                        <input id="RightButton" type="button" value="Right" class="button" onclick="MoveRight();" />&nbsp;
                                        <input id="UpButton" type="button" value="Up" class="button" onclick="MoveUp();" />&nbsp;
                                        <input id="DownButton" type="button" value="Down" class="button" onclick="MoveDown();" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div id="metadataTree" style="overflow: visible;" class="BodyTxt"><!-- Category Tree -->&nbsp;</div>
                    </td>
                    <td valign="top">
                        <table width="450" border="0" cellspacing="0" cellpadding="2" class="TblOutline">
                            <tr>
                                <td class="TblCell2 BodyTxt Bold alignRight Top W125">Metadata Value <span class="ReqField">*</span></td>
                                <td class="TblCell1 BodyTxt Top" colspan="2"><asp:TextBox ID="NameTextBox" CssClass="formInput W225" runat="Server" MaxLength="100" /></td>
                            </tr>
                            <tr>
                                <td class="TblCell2 BodyTxt Bold alignRight Top W125">External Reference</td>
                                <td class="TblCell1 BodyTxt Top" colspan="2"><asp:TextBox ID="ExternalRefTextBox" CssClass="formInput W50" runat="Server" MaxLength="50" /></td>
                            </tr>
                            <tr>
                                <td class="TblCell2 BodyTxt alignRight Top W125"><span class="Bold">Synonyms</span><br />(Enter alternative terms for selected metadata; one per line)</td>
                                <td class="TblCell1 BodyTxt Top" colspan="2"><app:TextArea ID="SynonymsTextBox" Columns="25" Rows="5" CssClass="formInput W310" MaxLength="500" runat="server" /></td>
                            </tr>
                            <tr>
                                <td style="text-align: left" nowrap="nowrap">
                                    <input id="AddButton" type="button" value="Add New" class="button" onclick="AddMetadata();" />
                                </td>
                                <td style="text-align: left">
									<input id="CancelButton" class="button" type="button" value="Cancel" />
                                    <input id="DeleteButton" class="button" type="button" value="Delete" />
                                </td>
                                <td style="text-align: right">
                                    <asp:Button ID="UpdateButton" CssClass="button" Text="update" runat="server" OnClick="UpdateButton_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            
            
            
			<div class="AppCtrlMrg" style="margin:20px 0px 20px 15px">
				<asp:HyperLink ID="ManageMetadataHyperlink2" CssClass="PanelTxt Bold" Text="&laquo; Back to metadata list" runat="server" />
			</div>
		</div>
	</div>
</asp:Content>