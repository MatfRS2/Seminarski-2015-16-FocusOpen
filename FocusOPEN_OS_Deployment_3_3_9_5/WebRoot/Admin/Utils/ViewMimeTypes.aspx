<%@ Page Title="View Mime Types [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="ViewMimeTypes.aspx.cs" Inherits="FocusOPEN.Website.Admin.ViewMimeTypes" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript">
		$(function() {
			$("#AddMimeTypeFromFileLink").click(function() {
				if ($("#UploadControls").is(":visible"))
					return;
				$(this).attr("style", "padding-bottom:10px");
				$("#UploadButton").hide();
				$("#UploadControls").show();
			});
		});
	</script>
	<div class="AdmBkg"> 
		<div class="Left AdmLeftPanel"> 
			<div class="AdmAppHdr">View Mime Types</div>
			<br />
			<br />
			<div class="AppCtrlMrg"><span class="BodyTxt">Below is a list of configured mime types and the file extensions mapped to each.   | </span><app:HelpHyperLink HelpPage="10_Utilities.htm#view_mime_types" CssClass="BodyTxt" Text="help" runat="server" /><br /></div>
			<br />
		</div>
		<div class="Right AdmRightPanel"></div>
		<div class="AdmTblPanel Clear">
			<div class="Clear">
				<app:SortableDataGrid ID="MimeTypesDataGrid" ShowFooter="false" AllowSorting="false" OnItemDataBound="MimeTypesDataGrid_ItemDataBound" AllowDelete="false" runat="server">
					<Columns>
						<asp:TemplateColumn HeaderText="Mime Type Name">
							<ItemStyle VerticalAlign="top" Width="50%" />
							<ItemTemplate>
								<asp:Label ID="MimeTypeNameLabel" CssClass="BodyTxt" runat="server" />
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="File Extensions">
							<ItemTemplate>
								<asp:Label ID="FileExtensionsLabel" CssClass="BodyTxt" runat="server" />
							</ItemTemplate>
						</asp:TemplateColumn>
					</Columns>
				</app:SortableDataGrid>
				<br />
				<div class="AppCtrlMrg">
					<a id="AddMimeTypeFromFileLink" href="#" class="BodyTxt Bold">Add mime type from file</a><br />
					<div id="UploadControls" style="padding-top:10px;display:none">
						<asp:FileUpload ID="FileUpload1" runat="server" />
						<asp:Button ID="UploadButton" Text="Upload" OnClick="UploadButton_Click" runat="server" />
					</div>
				</div>
				<br />
			</div>
		</div>
	</div>
	<br />
</asp:Content>
