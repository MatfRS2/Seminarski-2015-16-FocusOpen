<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="FocusOPEN.Website.Login" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<!--*******************************************************************************************************************
	
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011.  All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the
	software without specific, written prior permission. Title to copyright in this software and any associated
	documentation will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about
	this software.

********************************************************************************************************************-->

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title>Login [<%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().ApplicationName%>]</title>
	<app:BrandHeader ID="BrandHeader1" Css="Basic|Styles|External" runat="server" />
	<script src="Includes/Javascript/jQuery/jquery-1.4.1.min.js" type="text/javascript"></script>
	<script src="Includes/Javascript/jQuery/jquery-ui-1.7.2.custom.min.js" type="text/javascript"></script>
	<script src="Includes/Javascript/Global.js" type="text/javascript"></script>
</head>
<body>
	<div class="ExtWrapper">
		<div class="ExtHdrPanel"></div>
		<div class="ExtBkg">
			<div class="ExtMrg">
				<span class="BodyTxt"><%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().LoginPageUpperCopy%><br /></span>
				<br />
				<br />
				<div class="ExtTblMrg">
					<form id="form1" runat="server">
						<app:FeedbackLabel ID="MessageLabel1" Visible="false" Pinned="true" LineBreaks="1" runat="server" />
						<table width="420" border="0" cellspacing="0" cellpadding="2" class="TblOutline">
						<tr>
							<td class="TblCell2 W100 alignRight PanelTxt Bold">Email</td>
							<td id="EmailCell" runat="server" class="TblCell1"><asp:TextBox ID="EmailTextBox" CssClass="formInput W300" runat="server" /></td>
						</tr>
						<tr>
							<td class="TblCell2 alignRight PanelTxt Bold">Password</td>
							<td class="TblCell1"><asp:TextBox ID="PasswordTextBox" CssClass="formInput W300" TextMode="Password" runat="server" /></td>
						</tr>
						</table>
						<div class="BodyTxt ExtSubCkMrg"><asp:CheckBox ID="RememberMeCheckBox" runat="server" /><span class="BaseLine">remember my email next time</span></div>
						<div class="alignRight">
							<asp:Button ID="LoginButton" CssClass="button" runat="server" OnClick="LoginButton_Click" Text="login" />
							<app:PromptButton ID="PasswordReminderButton" CssClass="button W100" Prompt="Are you sure you want to reset your password?\n\nPlease ensure you have access to the specified email account before using this option." runat="server" OnClick="PasswordReminderButton_Click" Text="forgot password?" />
						</div>
					</form>
				</div>
				<app:RegistrationInfoWrapper ID="RegInfoWrapper" runat="server">
					<span class="BodyTxt"><span class="Bold">Not registered?</span> <%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().OrganisationName%> employees and suppliers can apply for access by </span><a href="Register.aspx" class="BodyTxt">registering here.</a><br />
				</app:RegistrationInfoWrapper>
				<br />
				<div class="ExtDiv">
					<br />
					<span class="BodyTxt"><%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().LoginPageLowerCopy%><br /></span>
				</div>
			</div>
		</div>
		<app:Footer ID="Footer1" ShowLoginLink="false" ShowRegistrationLink="true" runat="server" />
	</div>
</body>
</html>