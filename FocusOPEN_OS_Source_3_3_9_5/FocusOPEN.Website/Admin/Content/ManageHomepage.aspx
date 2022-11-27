<%@ Page Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="ManageHomepage.aspx.cs" Inherits="FocusOPEN.Website.Admin.ManageHomepage" Title="Manage Homepage [ADMIN]" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MainContent">
    <script language="javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(setupTreeViewPostBack);
        function setupTreeViewPostBack()
        {
        	// We have to use this trick to do the postback because ASP.NET's 
        	// treeview control doesn't do a Postback when a checkbox is checked.
        	$("div.TreeViewContainer input:checkbox").click(function()
            {
                var href = $(this).next("a").attr("href");
                eval(href);
            });
        }
	</script>
	<style type="text/css">
		div.SectionHeader
		{
			font-size: 11px;
			font-weight: bold;
			padding: 9px 9px 9px 0px;
			border-bottom:1px solid #ccc;
			margin-bottom: 5px;
		}
	</style>
	<div class="AdmBkg"> 
		<div class="Left"> 
			<div class="AdmAppHdr">Home Page Management</div>
			
			<app:FeedbackLabel ID="MessageLabel1" UseContainer="true" runat="server" />
			<br />
			
			<asp:PlaceHolder ID="BrandSelectorPanel" runat="server">
				<div style="margin:5px 5px 5px 15px;">
					<span class="PanelTxt Bold" style="margin-right:6px;display:block;width:60px;float:left;">Brand: </span>
					<app:BrandDropDownList ID="BrandDropDownList1" CssClass="formInput" OmitBlankItem="true" AutoPostBack="true" OnSelectedIndexChanged="BrandDropDownList1_SelectedIndexChanged" runat="server" />
				</div>
			</asp:PlaceHolder>
			
			<asp:PlaceHolder ID="TemplateSelectorPanel" runat="server">
				<div style="margin:0px 0px 0px 15px;">
					<span class="PanelTxt Bold" style="margin-right:6px;display:block;width:60px;float:left;">Template: </span>
					<app:HomepageTypeDropDownList ID="HomepageTemplateTypeDropDownList" AutoPostBack="true" OnSelectedIndexChanged="HomepageTemplateTypeDropDownList_SelectedIndexChanged" CssClass="formInput" OmitBlankItem="true" runat="server"/>
				</div>
			</asp:PlaceHolder>
			
			<div class="AppCtrlMrg" style="width:100%;">
				<br />
				
				<asp:PlaceHolder ID="StandardTemplateEditorPanel" runat="server">
					<span class="BodyTxt">
						<span class="Bold">
							- Upload new images and select new categories and links for the homepage below.<br />
							&nbsp;&nbsp;&nbsp;Note: Image files must be in PNG, GIF or JPG format.<br />
							- Press 'preview' to view your changes as they will appear homepage (changes will not be saved)<br />
							- Press 'publish' to save your changes and update the homepage
						</span>
						<span class="BodyTxt">|</span>
						<app:HelpHyperLink ID="HelpHyperLink1" CssClass="BodyTxt" HelpPage="9_Manage_Content.htm#manage_homepage" Text="help" runat="server" />
						<br />
					</span>
					<br />
					<app:RichTextBox Id="IntroTextBox" RemovePlugins="save,preview,newpage,templates,forms" runat="server" />
					<br />
					<div class="SectionHeader">Featured Categories</div>
					<aspajax:UpdatePanel ID="CategoryUpdatePanel" ChildrenAsTriggers="true" runat="server">
						<ContentTemplate>
							<div class="TreeViewContainer">
								<app:CategoriesTreeView ID="CategoriesTreeView" BindingMode="Manual" ShowCheckBoxes="All" NodeStyle-CssClass="PanelTxt" OnTreeNodeCheckChanged="CategoriesTreeView_TreeNodeCheckChanged" runat="server"/>
							</div>
							<br />
							<asp:ListBox ID="SelectedCategoriesListBox" CssClass="formInput" Width="200px" Height="100px" runat="server" />
							<br />
							<asp:Button ID="MoveUpCategoryButton" OnClick="MoveUpCategoryButton_Click" Text="up" CssClass="button" runat="server" />
							<asp:Button ID="MoveDownCategoryButton" OnClick="MoveDownCategoryButton_Click" Text="down" CssClass="button" runat="server" />
							<asp:Button ID="RemoveCategoryButton" OnClick="RemoveCategoryButton_Click" Text="remove" CssClass="button" runat="server" />
						</ContentTemplate>
					</aspajax:UpdatePanel>
					<br />
					<br />
					<div class="SectionHeader">Edit the first featured quicklink below</div>
					<app:HomepageImageEditor ID="HomepageImageEditor1" ImageNumber="1" runat="server" />
					<br />
					<div class="SectionHeader">Edit the second featured quicklink below</div>
					<app:HomepageImageEditor ID="HomepageImageEditor2" ImageNumber="2" runat="server" />
					<br />
					<div class="SectionHeader">Edit the third featured quicklink below</div>
					<app:HomepageImageEditor ID="HomepageImageEditor3" ImageNumber="3" runat="server" />
					<br />
					<div class="SectionHeader">Edit the fourth featured quicklink below</div>
					<app:HomepageImageEditor ID="HomepageImageEditor4" ImageNumber="4" runat="server" />
				</asp:PlaceHolder>
				
				<asp:PlaceHolder ID="CustomHtmlTemplateEditorPanel" runat="server">
					<span class="BodyTxt">
						<span class="Bold">
							- This template is a full page: no navigation will be shown to users.  You must provide a link to the catalogue for users to access it<br />
							- Press 'preview' to view your changes as they will appear homepage (changes will not be saved)<br />
							- Press 'publish' to save your changes and update the homepage
						</span>
						<br />
					</span>
					<br />
					<app:FeedbackLabel ID="MessageLabel2" LineBreaks="1" runat="server" />
					<br />
					<div class="SectionHeader" style="border-bottom:0px">Enter custom HTML below</div>
					<app:RichTextBox ID="CustomHtmlTextBox" Height="350px" runat="server"/>
				</asp:PlaceHolder>
				
				<aspajax:UpdatePanel ID="BumperPageUpdatePanel" ChildrenAsTriggers="true" runat="server">
					<ContentTemplate>
						<br />
						<asp:CheckBox ID="IncludeBumperPageCheckBox" Text="Include Bumper Page" CssClass="BodyTxt" AutoPostBack="true" OnCheckedChanged="IncludeBumperPage_CheckChanged" runat="server" />
						<br />
						<asp:CheckBox ID="SkipBumperPageCheckBox" Text="Skip bumper page after initial view" CssClass="BodyTxt" runat="server" />
						<asp:Panel ID="BumperPageHtmlWrapper" runat="server">
							<br />
							<div class="SectionHeader" style="border-bottom:0px">Enter bumper page HTML below</div>
							<app:TextArea ID="BumperPageTextBox" Height="350px" Width="500px" runat="server"/>
						</asp:Panel>					
					</ContentTemplate>
				</aspajax:UpdatePanel>
				
				<br />
				<br />
				
				<div style="border-top:1px solid #ccc;padding:5px;">
					<div class="Left BodyTxt">
						<asp:Label ID="LastModifiedLabel" runat="server" />
					</div>
					<div class="Right alignRight">
						<app:CancelButton ID="CancelButton" Text="Cancel" CssClass="button" RedirectUrl="ManageHomepage.aspx" Visible="false" runat="server" />
						<asp:Button ID="PreviewButton" Text="Preview" CssClass="button" OnClick="PreviewButton_Click" runat="server" />
						<asp:Button ID="PublishButton" Text="Publish" CssClass="button" OnClick="PublishButton_Click" runat="server" />
					</div>
				</div>
			</div>
		</div>
		<div class="Clear"><br /></div>
	</div>
	<br />
</asp:Content>