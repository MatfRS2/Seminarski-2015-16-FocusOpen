<%@ Page Title="Admin Home [ADMIN]" MasterPageFile="~/Templates/Website/AdminTemplate.Master" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FocusOPEN.Website.Admin.AdminDefault" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<asp:Content ID="MainContentText" runat="server" ContentPlaceHolderID="MainContent">
	<div class="AdmBkg"> 
		<div class="Left AdmLeftPanel"> 
			<div class="AdmAppHdr">ADMIN</div>
			<br />
			<div class="AppCtrlMrg">
				<span class="BodyTxt">Welcome <asp:Label ID="UserNameLabel" CssClass="Bold" Text="[User Name]" runat="server" />.   You are a <asp:Label ID="UserRoleNameLabel" Text="[UserRole]" runat="server" /> | </span><app:HelpHyperLink ID="HelpHyperLink1" HelpPage="6_User_Admin.htm#user_roles" CssClass="BodyTxt" Text="help" runat="server" /><br />
			</div>
		</div>
		<div class="Right AdmRightPanel"></div>
		<div class="AdmTblPanel Clear"> 
			<br />
			<div class="noResults AdmHomeMrg">
				<span class="BodyTxt"><%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().AdminCopy%></span> 
			</div>
		</div>
	</div>
	<div class="AdmAppFtr"></div>
</asp:Content>