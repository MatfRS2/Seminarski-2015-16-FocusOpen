<%@ Page Title="Delegate Assets [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="AssetsBulkDelegation.aspx.cs" Inherits="FocusOPEN.Website.Admin.Assets.AssetsBulkDelegation" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="atkins" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg"> 
		<div class="Left AdmLeftPanel"> 
			<div class="AdmAppHdr">View Asset List</div>
			<br /><app:FeedbackLabel ID="MessageLabel1" UseContainer="true" runat="server" /><br />
		</div>
		<div class="Right AdmRightPanel"></div>
		<div class="AdmTblPanel Clear">
			<div class="Clear">
				<div style="margin-left:10px;" class="BodyTxt">
					This option will cause all selected assets to be transferred to the user whose email address is entered below.
					<br /><br />
					E-Mail Address:
					<asp:TextBox ID="EmailTextBox" CssClass="formInput" runat="server" />
					<app:PromptButton ID="DelegateButon" Prompt="Are you sure you want to transfer the selected assets?" Text="transfer" CssClass="button" OnClick="DelegateButton_Click" runat="server" />
					<app:CancelButton ID="CancelButton" Prompt="Are you sure? Assets will not be delegated" RedirectUrl="AssetList.aspx" Text="cancel" CssClass="button" runat="server" />
				</div>
				<br />
				<app:AssetList ID="AssetList1" AllowDelete="false" runat="server" />
				<br />
			</div>
		</div>
	</div>
	<div class="AdmAppFtr"></div>
</asp:Content>