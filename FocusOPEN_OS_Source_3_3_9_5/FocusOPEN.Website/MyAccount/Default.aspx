<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FocusOPEN.Website.MyAccount.Default" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title>My Account [<%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().ApplicationName%>]</title>
	<app:BrandHeader ID="BrandHeader1" Css="Basic|Menus|Styles|Other" runat="server" />
	<script src="../Includes/Javascript/jQuery/jquery-1.4.1.min.js" type="text/javascript"></script>
	<script src="../Includes/Javascript/jQuery/jquery-ui-1.7.2.custom.min.js" type="text/javascript"></script>
	<script src="../Includes/Javascript/Global.js" type="text/javascript"></script>
	<link rel="Stylesheet" type="text/css" href="../Includes/Css/jQuery-UI/smoothness/jquery-ui-1.7.2.custom.css" />
</head>
<body>
<form id="form1" runat="server">
	<aspajax:ScriptManager ID="ScriptManager1" runat="server" />
	<div class="CatWrapper">
		<app:PageHeader ID="PageHeader1" runat="server" />
		<div class="CatBkg">
			<div class="CatAppPanel">
				<div class="Left"> 
					<div class="AppHdr"> 
						<!--start text header//--><span class="CatAppHdr">MY ACCOUNT</span>&nbsp;<!--end text header//-->
					</div>
					<div class="AppResults PanelTxt">welcome <span class="Bold" id="UserNameLabel" runat="server">[User Name]</span>. What would you like to do?</div>
				</div>
			</div>
			<div class="content">
				<div class="noResults">
					<span class="FtrHelpTxt">&bull; <app:HelpHyperLink ID="HelpHyperLink1" HelpPage="1_Getting_Started.htm" CssClass="BodyTxt" Text="Getting Started" runat="server" /><br /></span>
					&bull; <a href="EditProfile.aspx" class="BodyTxt">Edit My Profile</a><br />
					&bull; <a href="ViewOrders.aspx" class="BodyTxt">My Orders</a><br />
					&bull; <a href="../Login.aspx?action=logout" class="BodyTxt">Log Out</a><br />
					<span class="FtrHelpTxt">&bull; <app:HelpHyperLink ID="HelpHyperLink2" HelpPage="index.htm" CssClass="BodyTxt" Text="Help" runat="server" /></span>
					<p class="BodyTxt"><%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().MyAccountCopy%></p> 
				</div>
			</div>
		</div>
		<div class="CatAppFtr">
			<div class="Left">
				<div class="AppResults PanelTxt"></div>
			</div>
		</div>
		<app:PageFooter ID="PageFooter1" HelpPage="index.htm#catalogue" runat="server" />
	</div>
</form>
</body>
</html>