<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditProfile.aspx.cs" Inherits="FocusOPEN.Website.MyAccount.EditProfile" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title>My Profile [<%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().ApplicationName%>]</title>
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
						 <!--start text header//--><span class="CatAppHdr">MY PROFILE</span>&nbsp;<!--end text header//-->
					</div>
					<div class="AppResults PanelTxt">Edit your profile below. Note: fields marked <span class="ReqField">*</span> are required.</div>
				</div>
			</div>
			<div class="content">
				<app:FeedbackLabel ID="FeedbackLabel1" runat="server" />
				<br />
				<app:UserDetailsForm ID="UserDetailsForm1" Mode="EditProfile" runat="server" />
				<br />
				<div class="alignRight W420">
					<asp:Button ID="SaveChangesButton" CssClass="button" OnClick="SaveChangesButton_Click" Text="save changes" runat="server" />
					<app:CancelButton ID="CancelButton" CssClass="button" Prompt="Are you sure you want to cancel changes?" RedirectUrl="Default.aspx" Text="cancel" runat="server" />
				</div>
			</div>
			<div class="CatAppFtr"></div>
		</div>
		<app:PageFooter ID="PageFooter1" HelpPage="index.htm#catalogue" runat="server" />
	</div>
</form>
</body>
</html>