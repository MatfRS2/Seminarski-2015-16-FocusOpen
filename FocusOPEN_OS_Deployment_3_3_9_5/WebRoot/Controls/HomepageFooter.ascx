<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HomepageFooter.ascx.cs" Inherits="FocusOPEN.Website.Controls.HomepageFooter" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<div class="HomeFtrPanel" style="padding-top:10px">
	<div class="Left">
		<app:AttributionFooter ID="AttributionFooter" runat="server" />
	</div>
	<div class="Right FtrMrg">
		<a href="#" class="FooterTxt">Terms & Conditions</a> | 
		<span class="FtrHelpTxt"><a href="javascript:showHelp('index.htm')" class="FooterTxt">Help</a> |</span> 
		<span class="FooterTxt">&copy; <%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().OrganisationName%> <%=DateTime.Now.Year%></span>
	</div>
</div>

<app:VersionLabel ID="VersionInfoLabel" runat="server" />