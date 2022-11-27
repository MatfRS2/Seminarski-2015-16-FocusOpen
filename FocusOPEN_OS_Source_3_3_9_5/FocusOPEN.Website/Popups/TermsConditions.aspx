<%@ Page  AutoEventWireup="true"  %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title>Terms &amp; Conditions [<%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().ApplicationName%>]</title>
	<app:BrandHeader ID="BrandHeader1" Css="Basic|Styles|External" runat="server" />
</head>
<body>
	<div class="ExtWrapper">
		<div class="ExtHdrPanel"></div>
		<div class="ExtBkg">
			<div class="ExtMrg"> 
				<div class="BodyTxt">
					<%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().TermsConditionsCopy%>
				</div>
			</div>
		</div>
		<div class="Right FtrMrg">
			<a href="PrivacyPolicy.aspx" class="FooterTxt">Privacy Policy</a> |  
			<span class="FooterTxt">&copy; <%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().OrganisationName%> <%=DateTime.Now.Year%></span>
		</div>
	</div>
</body>
</html>