<%@ Page Title="Delete Assets [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="AssetsBulkDelete.aspx.cs" Inherits="FocusOPEN.Website.Admin.Assets.AssetsBulkDelete" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="atkins" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg"> 
		<div class="Left AdmLeftPanel"> 
			<div class="AdmAppHdr">View Asset List</div>
		</div>
		<div class="Right AdmRightPanel"></div>
		<div class="AdmTblPanel Clear">
			<div class="Clear">
				<br />
				<div style="margin-left:10px;" class="BodyTxt">
					Please re-enter your password below and click 'confirm delete' to delete the assets below.  <strong>This action is permanent and cannot be undone.</strong>
					<br /><br />
					<app:FeedbackLabel ID="FeedbackLabel1" LineBreaks="1" runat="server" />
					<asp:TextBox ID="PasswordTextBox" TextMode="Password" CssClass="formInput W150" runat="server" />
					<app:PromptButton ID="DeleteButton" Prompt="Are you sure you want to delete the selected assets?" Text="delete" CssClass="button" OnClick="DeleteButton_Click" runat="server" />
					<app:CancelButton ID="CancelButton" Prompt="Are you sure? Assets will not be deleted" RedirectUrl="AssetList.aspx" Text="cancel" CssClass="button" runat="server" />
				</div>
				<br />
				<app:AssetList ID="AssetList1" AllowDelete="false" runat="server" />
				<br />
			</div>
		</div>
	</div>
	<div class="AdmAppFtr"></div>
</asp:Content>