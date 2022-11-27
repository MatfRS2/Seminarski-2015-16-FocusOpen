<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="FocusOPEN.Website.Register" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title>Register [<%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().ApplicationName%>]</title>
	<app:BrandHeader ID="BrandHeader1" Css="Basic|Styles|External" runat="server" />
	<script src="Includes/Javascript/Global.js" type="text/javascript"></script>
	<script src="Includes/Javascript/jQuery/jquery-1.4.1.min.js" type="text/javascript"></script>
	<script src="Includes/Javascript/jQuery/jquery-ui-1.7.2.custom.min.js" type="text/javascript"></script>
	<script src="Includes/Javascript/Global.js" type="text/javascript"></script>
	<link rel="Stylesheet" type="text/css" href="Includes/Css/jQuery-UI/smoothness/jquery-ui-1.7.2.custom.css" />
</head>
<body>
	<div class="RegWrapper">
		<div class="ExtHdrPanel"></div>
		<div class="ExtBkg">
			<div class="ExtMrg">
				
				<asp:panel ID="FeedbackPanel" visible="False" runat="Server">
					<div class="RegFbk">
						<br />
						<app:IconImage ID="PositiveIconImage" Filename="fbkPositive.gif" AlternateText="[tick]" runat="server"/>
						<span class="FbkPosTxt">Registration process started.<br /></span>
						<span id="PendingEmailMessage" class="PosTxt" visible="false" runat="server">Please complete your registration by clicking the link in the confirmation email which has been sent to your inbox.<br /></span>
						<span id="AdminApprovalMessage" class="PosTxt" visible="false" runat="server">Your application has been sent to an Administrator for approval.  You will be notified of a decision via email shortly.<br /></span>
					</div>
				</asp:panel>
				
				<asp:panel ID="FormPanel" runat="server">
					<span class="BodyTxt">To register, please enter your details below. Note: fields marked <span class="ReqField">*</span> are required.<br /></span>
					<br />
					<div class="ExtTblMrg">
						<form id="form1" runat="server">
							<aspajax:ScriptManager ID="ScriptManager1" runat="server" />
							<app:FeedbackLabel ID="FeedbackLabel1" runat="server" />
							<br />
							<app:UserDetailsForm ID="UserDetailsForm1" Mode="Registration" runat="server" />
							<br />
							<div class="alignRight">
								<asp:Button ID="RegisterButton" CssClass="button" OnClick="RegisterButton_Click" Text="register" runat="server" />
								<app:CancelButton ID="CancelButton" CssClass="button" Prompt="Are you sure you want to cancel registration?" RedirectUrl="Login.aspx" Text="cancel" runat="server" />
							</div>
						</form>
					</div>
				</asp:panel>
				<br />
				<div class="ExtDiv">
					<br />
					<span class="BodyTxt">This facility is not available to the general public. It is for the sole use of <%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().OrganisationName%> employees and approved suppliers. Attempting unauthorised access to this facility is forbidden and may result in action being taken against you.<br /></span>
				</div>
			</div>
		</div>
		<app:Footer ID="Footer1" ShowLoginLink="true" ShowRegistrationLink="false" runat="server" />
	</div>
</body>
</html>