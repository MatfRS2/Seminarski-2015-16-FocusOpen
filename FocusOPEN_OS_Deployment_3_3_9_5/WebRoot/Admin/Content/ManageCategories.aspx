<%@ Page Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="ManageCategories.aspx.cs" Inherits="FocusOPEN.Website.Admin.ManageCategories" Title="Manage Categories [ADMIN]" %>

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
                $.tree_reference("categoryTree").remove();
            });
            
            EnableButton("<%=UpdateButton.ClientID %>", false);
            EnableButton("AddButton", false);
            
            CategoriesFetched();
        });
        
		function CategoriesFetched(jsonResponse)
		{
			$("#categoryTree").tree({
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
					json: <%=GetAllCategories(BrandId)%>
				},
				callback:
				{
					beforedelete: BeforeDelete,
					onselect: OnSelect,
					onrename: OnRename,
					onmove: OnMove,
					beforemove: BeforeMove,
					beforechange: BeforeChange
				},
				cookies:
				{
					prefix: "categoryTree",
					opts: { path: '/'}
				},
				ui:
				{
					context: false
				}
			});
			
			$("li[assetcount] > a").each(function() {
				var count = $(this).parent().attr("assetcount");
				$(this).append(" (" + count + ")");
			});

			var selectedCategoryId = $("#<%=CategoryIdTextBox.ClientID %>").val();
			
			if (selectedCategoryId != "" && selectedCategoryId > 0)
			{
				$.tree_reference("categoryTree").select_branch($("#" + selectedCategoryId));
			}
			else
			{
				$("#categoryTree").height($("#categoryTree").parent("td").height());
			}
		}

		function BeforeMove(item, refnode)
		{
			if (refnode.id <= 0 || !ChangeAllowed)
			{
				alert("This category cannot be moved");
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
			var tree = $.tree_reference("categoryTree");
			var n = tree.get_node(node);
			
			if (node.id > 0)
			{
				if (!tree.parent(node))
				{
					alert("This is the root category and cannot be deleted.");
					return false;
				}
				
				if (tree.children(node).length > 0)
				{
					alert("This category has subcategories and cannot be deleted.");
					return false;
				}
				
				var msg = "Are you sure you want to delete the following category:\n\n\"" + $("#<%=NameTextBox.ClientID%>").val() + "\"\n\nThis operation cannot be undone.";
				
				if (confirm(msg))
				{
					PageMethods.DeleteCategory(node.id, function(result) {
						if (result.Success)
						{
							var n = $("#categoryTree").find("li#" + result.CategoryId);
							n.remove();
						}
						else if (result.ErrorCode = "has-assets")
						{
							var conf = confirm("This category cannot be deleted as there are assets assigned to it.\n\nWould you like to move the existing assets to another category and delete this category?");

							if (conf)
							{
								PageMethods.SetupCategoryDelete(node.id, function() {
									window.location = '<%=ResolveUrl("~/Admin/Assets/AssetsBulkChangeCategory.aspx") %>';
								});
							}
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
				if (node.id == "NewCategory")
					return true;
			};
			return false;
		}

		function OnRename(node)
		{
			PageMethods.RenameCategory(node.id, GetNodeText(node));
			return true;
		}
	    
		function OnMove(node, treeobj, RB)
		{
			var tree = $.tree_reference("categoryTree");
			var parent = tree.parent(node);
			var parent_id = (parent != null && parent) ? parent.attr("id") : 0;
			var position = GetNodeOrder(node);
			
			PageMethods.MoveCategory(node.id, parent_id, position);
		}

		function OnSelect(node, treeobj)
		{
			if (node.id != "NewCategory")
			{
				PageMethods.GetCategory(node.id, CategoryInfoFetched);

				EnableButton("AddButton", true);
				EnableButton("<%=UpdateButton.ClientID %>", true);
			}
		}

		function CategoryInfoFetched(categoryJson)
		{
			var category = eval(' (' + categoryJson + ') ');
			var synonyms = ProcessSynonyms(category.synonyms);
			var trimmed = synonyms.replace(/^\s+|\s+$/g, '') ;

			$("#<%=CategoryIdTextBox.ClientID %>").val(category.CategoryId);
			$("#<%=NameTextBox.ClientID %>").val(category.Name);
			$("#<%=ExternalRefTextBox.ClientID %>").val(category.ExternalRef);
			$("#<%=MessageTextBox.ClientID %>").val(category.Message);
			$("#<%=SynonymsTextBox.ClientID %>").val(trimmed);
		}
	    
		function GetNodeOrder(node)
		{
			var tree = $.tree_reference("categoryTree");
			
			if (node.previousSibling != null)
			{
				var parentNode = tree.get_node(node.previousSibling);
				var previousOrder = parseInt(parentNode.attr("categoryOrder"));
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

		function AddCategory()
		{
			ClearFields();
	        
			var parentId = $.tree_reference("categoryTree").selected.attr("id");
			$("#<%= ParentIdTextBox.ClientID %>").val(parentId);
	        
			var tree = $.tree_reference("categoryTree");
			var defaultName = 'New category';
			tree.create({ attributes: { id: "NewCategory" }, data: defaultName });
			tree.select_branch($("#NewCategory"));

			var nameTextBox = $("#<%= NameTextBox.ClientID %>");
			nameTextBox.val(defaultName);
			nameTextBox.get(0).focus();
			nameTextBox.get(0).select();

			EnableAddNewCategoryMode(true);
		}

		function EnableAddNewCategoryMode(value)
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
			var tree = $.tree_reference("categoryTree");
			ClearFields();
			EnableAddNewCategoryMode(false);
			tree.remove();
		}

		function ClearFields()
		{
			$("#<%= CategoryIdTextBox.ClientID %>").val("");
			$("#<%= NameTextBox.ClientID %>").val("");
			$("#<%= MessageTextBox.ClientID %>").val("");
			$("#<%= SynonymsTextBox.ClientID %>").val("");
		}

		function MoveUp()
		{
			var tree = $.tree_reference("categoryTree");
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
			var tree = $.tree_reference("categoryTree");
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
			var tree = $.tree_reference("categoryTree");
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
			var tree = $.tree_reference("categoryTree");
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
			var tree = $.tree_reference("categoryTree");
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
    
    <div class="AdmBkg">
        <div class="Left W700">
            <div class="AdmAppHdr">Manage Categories</div>
            <app:FeedbackLabel ID="FeedbackLabel1" UseContainer="true" runat="server" />
            <br />
            <div class="AppCtrlMrg">
                <span class="Bold BodyTxt">
					- Drag & drop categories to reorder them, or use the buttons.<br />
                    - New categories can be added at any level.<br />
                    - Only categories with no children can be deleted. |
				</span>
				<app:HelpHyperLink ID="HelpHyperLink2" HelpPage="9_Manage_Content.htm#manage_categories" CssClass="BodyTxt" Text="help" runat="server" /><br />
			</div>
        </div>
        <div class="Right W200">
        </div>
        <div class="AdmTblPanel Clear">
            <div style="clear: both; height: 15px">&nbsp;</div>
			
			<asp:PlaceHolder ID="BrandSelector" runat="server">
				<div style="margin:15px 7px 25px 7px;padding:5px;background:#eee;">
					<span class="PanelTxt Bold" style="margin-right:6px;">Brand: </span>
					<app:BrandDropDownList ID="BrandDropDownList1" class="formInput" OmitBlankItem="true" AutoPostBack="true" runat="server" />
				</div>
			</asp:PlaceHolder>
			
            <table style="width: 100%">
                <tr>
                    <td valign="top">
                        <table>
                            <tr>
                                <td>
                                    <div class="Clear">
                                        <span class="BodyTxt Bold alignRight Top W125">Reorder category</span>
                                        <input id="LeftButton" type="button" value="Left" class="button" onclick="MoveLeft();" />&nbsp;
                                        <input id="RightButton" type="button" value="Right" class="button" onclick="MoveRight();" />&nbsp;
                                        <input id="UpButton" type="button" value="Up" class="button" onclick="MoveUp();" />&nbsp;
                                        <input id="DownButton" type="button" value="Down" class="button" onclick="MoveDown();" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div id="categoryTree" style="overflow: visible;" class="BodyTxt"><!-- Category Tree -->&nbsp;</div>
                    </td>
                    <td valign="top">
                        <table width="450" border="0" cellspacing="0" cellpadding="2" class="TblOutline">
                            <tr>
                                <td class="TblCell2 BodyTxt Bold alignRight Top W125">Category Name <span class="ReqField">*</span></td>
                                <td class="TblCell1 BodyTxt Top" colspan="2"><asp:TextBox ID="NameTextBox" CssClass="formInput W225" runat="Server" MaxLength="150" /></td>
                            </tr>
                            <tr>
                                <td class="TblCell2 BodyTxt Bold alignRight Top W125">External Reference</td>
                                <td class="TblCell1 BodyTxt Top" colspan="2"><asp:TextBox ID="ExternalRefTextBox" CssClass="formInput W50" runat="Server" MaxLength="50" /></td>
                            </tr>
                            <tr>
                                <td class="TblCell2 BodyTxt alignRight Top W125"><span class="Bold">Category Message</span><br />(Enter a message that users will see when browsing this Category)</td>
                                <td class="TblCell1 BodyTxt Top" colspan="2"><app:TextArea ID="MessageTextBox" Columns="25" Rows="5" CssClass="formInput W310" MaxLength="500" runat="server" /></td>
                            </tr>
                            <tr>
                                <td class="TblCell2 BodyTxt alignRight Top W125"><span class="Bold">Synonyms</span><br />(Enter alternative terms for selected Category; one per line)</td>
                                <td class="TblCell1 BodyTxt Top" colspan="2"><app:TextArea ID="SynonymsTextBox" Columns="25" Rows="5" CssClass="formInput W310" MaxLength="250" runat="server" /></td>
                            </tr>
                            <tr>
                                <td style="text-align: left" nowrap="nowrap">
                                    <input id="AddButton" type="button" value="Add New" class="button" onclick="AddCategory();" />
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
            <br />
            <br />
            <asp:HiddenField ID="CategoryIdTextBox" runat="server" />
            <asp:HiddenField ID="ParentIdTextBox" runat="server" />
        </div>
    </div>
</asp:Content>