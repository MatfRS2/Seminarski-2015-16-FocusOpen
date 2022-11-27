<%@ Page Title="Approve User [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="UserApproval.aspx.cs" Inherits="FocusOPEN.Website.Admin.UserApproval" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg"> 
		<div class="Left AdmLeftPanel"> 
			<div class="AdmAppHdr">User Approval</div>
			<br />
			<app:FeedbackLabel ID="MessageLabel1" UseContainer="true" runat="server" />
			<br />
			<div class="AppCtrlMrg">
				<span class="BodyTxt"><span class="Bold">An application has been made for external access.</span> 
				<br />
				Review the application below </span> <span class="BodyTxt">|</span> 
				<app:HelpHyperLink ID="HelpHyperLink1" HelpPage="6_User_Admin.htm" CssClass="BodyTxt" Text="help" runat="server" />
			</div>
			<br />
			<div class="AppCtrlMrg AppCtrlPanel W470">
				<table width="470" border="0" cellspacing="0" cellpadding="2">
				<tr> 
					<td class="W130 alignRight PanelTxt Bold">First Name:</td>
					<td class="PanelTxt"><asp:Label ID="FirstNameLabel" runat="server" /></td>
				</tr>
				<tr> 
					<td class="alignRight PanelTxt Bold">Surname:</td>
					<td class="PanelTxt"><asp:Label ID="LastNameLabel" runat="server" /></td>
				</tr>
				<tr> 
					<td class="alignRight PanelTxt Bold">Email:</td>
					<td><app:EmailHyperLink ID="EmailAddressHyperLink" CssClass="PanelTxt" runat="server" /></td>
				</tr>
				<tr> 
					<td class="alignRight PanelTxt Bold">Company: </td>
					<td class="PanelTxt"><asp:Label ID="CompanyLabel" runat="server" /></td>
				</tr>
				<tr> 
					<td class="alignRight PanelTxt Bold">Country:</td>
					<td class="PanelTxt"><asp:Label ID="CountryLabel" runat="server" /></td>
				</tr>
				<tr> 
					<td class="alignRight PanelTxt Bold">Contact Phone No:</td>
					<td class="PanelTxt"><asp:Label ID="PhoneNumberLabel" runat="server" /></td>
				</tr>
				<tr> 
					<td class="alignRight PanelTxt Bold">Mobile:</td>
					<td class="PanelTxt"><asp:Label ID="MobileNumberLabel" runat="server" /></td>
				</tr>
				</table>
				<div class="HozLine"></div>
				<asp:PlaceHolder ID="ActionPanel" runat="server">
					<table width="470" border="0" cellspacing="0" cellpadding="2">
					<tr>
						<td class="alignRight PanelTxt Bold W130">Your Decision</td>
						<td class="PanelTxt">
							<asp:RadioButtonList ID="ApprovedRadioButtonList" RepeatDirection="Horizontal" RepeatLayout="flow" runat="server">
								<asp:ListItem Value="1" Selected="True">Approve</asp:ListItem>
								<asp:ListItem Value="0">Reject</asp:ListItem>
							</asp:RadioButtonList>
						</td>
					</tr>
					<tr>
						<td class="PanelTxt alignRight Top W130">
							<span class="Bold">Your Response</span><br />
							If application is rejected specify your reasons
						</td>
						<td class="PanelTxt"><app:TextArea ID="NotesTextBox" Columns="25" Rows="4" CssClass="formInput W310" MaxLength="1000" runat="server" /></td>
					</tr>
					</table>
					<br />
				</asp:PlaceHolder>
				<div class="alignRight">
					<asp:Button ID="SubmitButton" Text="save" CssClass="button W50" runat="server" OnClick="SubmitButton_Click" />
					<app:CancelButton ID="CancelButton1" Text="cancel" CssClass="button" Prompt="Are you sure?  Changes will not be saved." RedirectUrl="UserList.aspx" runat="Server" />
				</div>
			</div>
		</div>
		<div class="Right AdmRightPanel"></div>
		<div class="Clear"><br /></div>
	</div>
</asp:Content>