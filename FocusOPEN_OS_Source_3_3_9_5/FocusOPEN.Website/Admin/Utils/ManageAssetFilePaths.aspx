<%@ Page Title="Manage File Paths [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="ManageAssetFilePaths.aspx.cs" Inherits="FocusOPEN.Website.Admin.ManageAssetFilePaths" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	
	<script type="text/javascript">
	
		// Using radio buttons in a datagrid requires some trickery
		// Here, we're using javascript to deselect the other radio buttons when one is clicked
	
		function selectRadio(sel)
		{
			var e = document.getElementById("<%=AssetFilePathsDataGrid.ClientID%>");
			var radiobuttons = e.getElementsByTagName("input");
			
			for (var rbindex in radiobuttons)
			{
				var rb = radiobuttons[rbindex];
				if (rb.type == 'radio')
				{
					rb.checked = (rb.id == sel.id);
				}
			}
		}
	</script>
	
	<div class="AdmBkg"> 
		<div class="Left W700"> 
			<div class="AdmAppHdr">Manage File Paths</div>
			<br />
			<app:FeedbackLabel ID="MessageLabel1" UseContainer="true" runat="server" />
			<br />
			<div class="AppCtrlMrg">
				<span class="BodyTxt">
					<br />
					<span class="Bold">Below is a list of asset file paths in the system | <app:HelpHyperLink HelpPage="10_Utilities.htm#manage_file_paths" CssClass="BodyTxt" Text="help" runat="server" /></span><br />
					&bull; All paths must be writable.<br />
					&bull; If the default path is changed, new assets will be stored in the new path. <span class="Bold">Existing assets will not be changed.<br /></span>
					&bull; <span class="Bold">To add a new file path:</span> enter the path as it would be accessed from the server and press 'add'. The required subfolders will be created if they do not already exist.
					<br /><br />
				</span>
			</div>
		</div>
		<div class="Right W200"></div>
		<div class="AdmTblPanel Clear">
			<app:SortableDataGrid
				ID="AssetFilePathsDataGrid"
				OnItemDataBound="AssetFilePathsDataGrid_OnItemDataBound"
				OnItemCommand="AssetFilePathsDataGrid_OnItemCommand"
				AutoGenerateColumns="false"
				Width="100%"
				CellPadding="5"
				ShowFooter="true"
				AllowDelete="false"
				runat="server">
			
				<Columns>
					<asp:TemplateColumn HeaderText="File Path">
						<ItemTemplate>
							<asp:HiddenField ID="AssetFilePathIdHiddenField" runat="server" />
							<asp:Label ID="PathLabel" CssClass="BodyTxt" Text="[Path]" runat="server" />
						</ItemTemplate>
						<FooterTemplate>
							<span class="AdmHdrTxt Bold">Add File Path:</span>
							<asp:TextBox ID="NewFilePathTextBox" CssClass="formInput W225" runat="server" />
							<asp:Button ID="AddNewFilePathButton" CssClass="button W50" CommandName="add" text="add" runat="server" />
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="No. of Assets" ItemStyle-CssClass="BodyTxt W100 alignCentre">
						<HeaderStyle CssClass="BodyTxt W100 alignCentre" />
						<ItemTemplate>
							<asp:Label ID="AssetCountLabel" CssClass="BodyTxt" runat="server" />
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Default" ItemStyle-CssClass="BodyTxt W100 alignCentre">
						<HeaderStyle CssClass="BodyTxt W100 alignCentre" />
						<FooterStyle CssClass="BodyTxt W100 alignCentre" />
						<ItemTemplate>
							<asp:RadioButton ID="IsDefaultRadioButton" runat="server" />
						</ItemTemplate>
						<FooterTemplate>
							<asp:Button ID="UpdateDefaultButton" Text="set default" CssClass="button" OnClick="UpdateDefaultButton_Click" runat="server" />
						</FooterTemplate>
					</asp:TemplateColumn>
				</Columns>
			</app:SortableDataGrid>
			<br />
		</div>
    </div>
    <br />
</asp:Content>
