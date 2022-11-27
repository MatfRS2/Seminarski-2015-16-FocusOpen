<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Footer.ascx.cs" Inherits="FocusOPEN.Website.Controls.Footer" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<div class="FtrMrg" style="float:left;width:100%;padding-top:10px;padding-bottom:225px">
	<div class="Left">
		<app:AttributionFooter ID="AttributionFooter" runat="server" />
	</div>
	<div class="Right">
		<a href="javascript:showTermsConditions();" class="FooterTxt">Terms &amp; Conditions</a> | 
		<% if (ShowLoginLink) { %><a id="link2" runat="server" href="~/Login.aspx" class="FooterTxt">Login</a> |<% }%>
		<% if (ShowRegistrationLink && FocusOPEN.Website.Global.AllowPublicRegistration) {%><a id="link3" runat="server" href="~/Register.aspx" class="FooterTxt">Register</a> | <% } %>
		<app:HelpHyperLink ID="FooterHelpLink" HelpPage="0_Welcome.htm" CssClass="FooterTxt" Text="Help" runat="server" /> | 
		<span class="FooterTxt">&copy; <%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().OrganisationName%> <%=DateTime.Now.Year%></span>
	</div>
</div>

<app:VersionLabel ID="VersionInfoLabel" runat="server" />