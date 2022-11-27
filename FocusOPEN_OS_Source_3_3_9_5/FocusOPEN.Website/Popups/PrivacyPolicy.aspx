<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title>Privacy Policy [<%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().ApplicationName%>]</title>
	<app:BrandHeader ID="BrandHeader1" Css="Basic|Styles|External" runat="server" />
</head>
<body>
	<div class="ExtWrapper">
		<div class="ExtHdrPanel"></div>
		<div class="ExtBkg">
			<div class="ExtMrg"> 
				<div class="BodyTxt">
						<%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().PrivacyPolicyCopy%>
				</div>
			</div>
		</div>
		<div class="Right FtrMrg">
			<a href="TermsConditions.aspx" class="FooterTxt">Terms & Conditions</a> |  
			<span class="FooterTxt">&copy; <%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().OrganisationName%> <%=DateTime.Now.Year%></span>
		</div>
	</div>
</body>
</html>