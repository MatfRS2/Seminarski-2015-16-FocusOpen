<%@ Page Title="Renew Assets [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="AssetsBulkRenewal.aspx.cs" Inherits="FocusOPEN.Website.Admin.Assets.AssetsBulkRenewal" %>
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
					This option will cause all selected assets to have their expiry date extended by the date specified.
					<br /><br />
					Extend expiry by:
					<asp:DropDownList ID="ExtendByMonthsDropDownList" CssClass="formInput" runat="server">
						<asp:ListItem Value="1">1 month</asp:ListItem>
						<asp:ListItem Value="6">6 months</asp:ListItem>
						<asp:ListItem Value="12">1 year</asp:ListItem>
						<asp:ListItem Value="24" Selected="True">2 years</asp:ListItem>
						<asp:ListItem Value="36">3 years</asp:ListItem>
					</asp:DropDownList>
					<app:PromptButton ID="RenewButton" Prompt="Are you sure you want to update the expiry date for the selected assets?" Text="renew" CssClass="button" OnClick="RenewButton_Click" runat="server" />
					<app:CancelButton ID="CancelButton" Prompt="Are you sure? Assets will not be renewed" RedirectUrl="AssetList.aspx" Text="cancel" CssClass="button" runat="server" />
				</div>
				<br />
				<app:AssetList ID="AssetList1" AllowDelete="false" runat="server" />
				<br />
			</div>
		</div>
	</div>
	<div class="AdmAppFtr"></div>
</asp:Content>