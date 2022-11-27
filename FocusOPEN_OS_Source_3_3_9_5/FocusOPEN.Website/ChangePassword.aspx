<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="FocusOPEN.Website.ChangePassword" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title>Password [<%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().ApplicationName%>]</title>
	<app:BrandHeader ID="BrandHeader1" Css="Basic|Styles|External" runat="server" />
	<script src="Includes/Javascript/jQuery/jquery-1.4.1.min.js" type="text/javascript"></script>
	<script src="Includes/Javascript/jQuery/jquery-ui-1.7.2.custom.min.js" type="text/javascript"></script>
	<script src="Includes/Javascript/Global.js" type="text/javascript"></script>
	<link rel="Stylesheet" type="text/css" href="Includes/Css/jQuery-UI/smoothness/jquery-ui-1.7.2.custom.css" />
</head>
<body>
	<div class="ExtWrapper">
		<div class="ExtHdrPanel"></div>
		<div class="ExtBkg">
			<div class="ExtMrg">
				<div class="RegFbk">
					<br />
					<app:FeedbackLabel ID="FeedbackLabel1" runat="server" />
					<span class="BodyTxt">Enter your old password, a new password and confirm.</span><br />
					<br />
					<div class="ExtTblMrg">
						<form id="form1" runat="server">
							<table width="420" border="0" cellspacing="0" cellpadding="2" class="TblOutline">
							<tr>
								<td class="TblCell2 W100 alignRight PanelTxt Bold">Old Password</td>
								<td class="TblCell1"><asp:TextBox ID="ExistingPasswordTextBox" TextMode="password" CssClass="formInput W300" runat="server" /></td>
							</tr>
							<tr>
								<td class="TblCell2 alignRight PanelTxt Bold">New Password</td>
								<td class="TblCell1"><asp:TextBox ID="NewPasswordTextBox" TextMode="password" CssClass="formInput W300" runat="server" /></td>
							</tr>
							<tr>
								<td class="TblCell2 alignRight PanelTxt Bold">Confirm</td>
								<td class="TblCell1"><asp:TextBox ID="ConfirmPasswordTextBox" TextMode="password" CssClass="formInput W300" runat="server" /></td>
							</tr>
							</table>
							<br />
							<div class="alignRight">
								<asp:Button ID="ChangePasswordButton" Text="activate new password" CssClass="button W140" runat="server" OnClick="ChangePasswordButton_Click" />
								<app:CancelButton ID="CancelButton1" Text="cancel" CssClass="button" RedirectUrl="Login.aspx" runat="server" />
							</div>
						</form>
					</div>
				</div>
				<br />
				<div class="ExtDiv">
					<br />
					<span class="BodyTxt">This facility is not available to the general public. It is for the sole use of <%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().OrganisationName%> employees and approved suppliers. Attempting unauthorised access to this facility is forbidden and may result in action being taken against you.<br /></span>
				</div>
			</div>
		</div>
		<app:Footer ID="Footer1" ShowLoginLink="true" runat="server" />
	</div>
</body>
</html>