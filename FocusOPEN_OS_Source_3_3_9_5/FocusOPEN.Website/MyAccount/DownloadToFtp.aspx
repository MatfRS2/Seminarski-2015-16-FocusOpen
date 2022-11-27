<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DownloadToFtp.aspx.cs" Inherits="FocusOPEN.Website.MyAccount.DownloadToFtp" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title>Download Manager [<%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().ApplicationName%>]</title>
	<app:BrandHeader ID="BrandHeader1" Css="Basic|Styles" runat="server" />
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
			<div class="DManAppPanel">
				<div class="DMHdr">
					<div class="Left">
						<br />
						<div class="OrdGrpHdr Left"></div>
						<br />
					</div>
				</div>
				<div class="Clear"></div>        
			</div>
			<div class="DMMrg BodyTxt"> 
				<asp:Panel ID="FormPanel" runat="server">
					<app:FeedbackLabel ID="FeedbackLabel1" UseContainer="true" LineBreaks="1" runat="server" />
					To download the selected assets via FTP, please enter the FTP details below.  If you have trouble
					downloading the assets, try enabling passive mode.
					<br /><br />
					<span class="Bold">FTP Host:</span><br />
					<asp:textbox id="FtpHostTextBox" CssClass="formInput W220" runat="server" />
					<br /><br />
					<span class="Bold">Port:</span><br />
					<asp:textbox id="FtpPortTextBox" CssClass="formInput W100" Text="21" runat="server" />
					<br /><br />
					<span class="Bold">Username:</span><br />
					<asp:textbox id="FtpUsernameTextBox" CssClass="formInput W150" runat="server" />
					<br /><br />
					<span class="Bold">Password:</span><br />
					<asp:textbox id="FtpPasswordTextBox" CssClass="formInput W150" TextMode="Password" runat="server" />
					<br /><br />
					<span class="Bold">Remote Folder:</span><br />
					<asp:textbox id="FtpRemoteFolderTextBox" CssClass="formInput W150" Text="/" runat="server" />
					<br /><br />
					<asp:CheckBox ID="FtpPassiveModeCheckBox" Text="Use passive mode" runat="server" />
					<br /><br />
					<app:CancelButton ID="CancelButton" CssClass="button" RedirectUrl="ViewOrders.aspx" Prompt="Are you sure you want to cancel? Assets will not be downloaded via FTP" Text="&laquo; cancel" runat="server" />
					<asp:Button ID="NextButton" CssClass="button" Text="continue &raquo;" OnClick="NextButton_Click" runat="server" />
					<br /><br />
				</asp:Panel>
				<asp:Panel ID="SuccessPanel" runat="server" Visible="false">
					The selected assets are being uploaded to the FTP server specified.  Depending on the size of the
					assets selected to transfer, this may take some time.  When the upload has completed, you will
					receive an email with the FTP log.
					<br /><br />
					<a href="ViewOrders.aspx" class="BodyTxt">&laquo; Back to download manager</a>
				</asp:Panel>
			</div>
			<div class="CatAppFtr"></div>
		</div>
		<app:PageFooter ID="PageFooter1" HelpPage="index.htm#catalogue" runat="server" />
	</div>
</form>
</body>
</html>