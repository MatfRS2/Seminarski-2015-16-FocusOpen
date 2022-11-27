<%@ Page Title="Edit User [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="UserForm.aspx.cs" Inherits="FocusOPEN.Website.Admin.UserForm" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript">
		$(function() {

			toggleCompanyTextboxVisiblity();

			$(".staffRadio > input").live('change', function() {
				setTimeout(toggleCompanyTextboxVisiblity, 500);
			});

			// Refresh UI when company dropdown changes
			$(".companyDropDown").live('change', function() {
				if ($(this).val() == "") {
					$(".companyTextBox").css("display", "");
				}
				else {
					$(".companyTextBox").css("display", "none");
				}
				var sv = $('option:selected', $(this)).val();

				if (sv.toLowerCase() != "other")
					$(".companyTextBox").val(sv);
			});

			// Refresh UI when company textbox changes
			$(".companyTextBox").live('blur', function() {
				if (companyNameIsInList()) {
					$(".companyDropDown").val($(".companyTextBox").val());
					$(this).hide();
				}
			});

			function toggleCompanyTextboxVisiblity() {
				// First hide the textbox
				$(".companyTextBox").hide();

				// Show it if the company name does not exist
				if (companyNameIsInList()) {
					var val = $(".companyTextBox").val();
					$(".companyDropDown").val(val);

					if (!val)
						$(".companyTextBox").show();
				}
				else {
					$(".companyDropDown").val("");
					$(".companyTextBox").show();
				}
			}

			function companyNameIsInList() {
				var name = $(".companyTextBox").val();
				return (name && $(".companyDropDown").find(":contains('" + name + "')").length > 0);
			}
		});
	</script>
	<div class="AdmBkg"> 
		<div class="Left AdmLeftPanel"> 
			<div class="AdmAppHdr">Edit User</div>
			<br />
			<asp:Panel ID="FormPanel" runat="server">
				<div class="AppCtrlMrg"><app:FeedbackLabel ID="MessageLabel1" runat="server" /></div>
				<br />
				<span class="AppCtrlMrg BodyTxt"><span class="Bold">Edit user details below.</span>  Note: fields marked <span class="ReqField">*</span> are required.</span>
				<span class="BodyTxt">|</span> <app:HelpHyperLink ID="HelpHyperLink1" HelpPage="6_User_Admin.htm#edit_users" CssClass="BodyTxt" Text="help" runat="server" /><br />
				<div class="AppCtrlMrg AppCtrlPanel W470">
					<table width="470" border="0" cellspacing="0" cellpadding="2">
					<tr>
						<td class="W130 alignRight PanelTxt">First Name <span class="ReqField">*</span></td>
						<td><asp:TextBox ID="FirstNameTextBox" CssClass="formInput W225" runat="server"/></td>
					</tr>
					<tr> 
						<td class="alignRight PanelTxt">Surname <span class="ReqField">*</span></td>
						<td><asp:TextBox ID="LastNameTextBox" CssClass="formInput W225" runat="server"/></td>
					</tr>
					<tr> 
						<td class="alignRight PanelTxt">Email <span class="ReqField">*</span></td>
						<td><asp:TextBox ID="EmailAddressTextBox" CssClass="formInput W225" runat="server"/></td>
					</tr>
					
					<asp:placeholder id="GeneratedPasswordPanel" runat="server">
					<tr>
						<td class="alignRight PanelTxt">Password <span class="ReqField">*</span></td>
						<td class="PanelTxt">(auto generated)</td>
					</tr>
					</asp:placeholder>
					
					<asp:PlaceHolder ID="EditPasswordPanel" runat="server">
					<tr> 
						<td class="alignRight PanelTxt" valign="top">Password&nbsp;&nbsp;&nbsp;</td>
						<td>
							<asp:TextBox ID="PasswordTextBox" TextMode="password" CssClass="formInput W225" runat="server" />
							<asp:Label ID="NewUserPasswordTooltip" style="font-size:10px" runat="server"><br />(Leave blank to generate random password automatically)</asp:Label>
						</td>
					</tr>
					<tr> 
						<td class="alignRight PanelTxt">Confirm Password&nbsp;&nbsp;&nbsp;</td>
						<td><asp:TextBox ID="ConfirmPasswordTextBox" TextMode="password" CssClass="formInput W225" runat="server" /></td>
					</tr>
					</asp:PlaceHolder>
					
					<tr id="WelcomeEmailCheckBoxRow" runat="server">
						<td class="alignRight PanelTxt" valign="top">No Welcome Email&nbsp;&nbsp;&nbsp;</td>
						<td>
							<asp:CheckBox ID="NoWelcomeEmailCheckBox" runat="server" /><br />
							<span style="font-size:10px">(If this option is checked, you must specify a password and manually notify the user about their login details)</span>
						</td>
					</tr>
					</table>
					
					<aspajax:UpdatePanel ID="UpdatePanel1" runat="server">
					<ContentTemplate>
						<table width="470" border="0" cellspacing="0" cellpadding="2">
						<tr id="IsStaffUserRow" runat="server"> 
							<td class="W130 alignRight PanelTxt">Staff User? <span class="ReqField">*</span></td>
							<td class="PanelTxt"> 
								<asp:RadioButtonList ID="IsStaffUserRadioButtonList" CssClass="staffRadio" OnSelectedIndexChanged="IsStaffUserRadioButtonList_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
									<asp:ListItem Value="1" Selected="true">Yes</asp:ListItem>
									<asp:ListItem Value="0">No</asp:ListItem>
								</asp:RadioButtonList>
							</td>
						</tr>
						<tr id="BrandSelectorRow" runat="server">
							<td class="W130 alignRight PanelTxt" valign="top">Chosen Brands <span class="ReqField">*</span></td>
							<td>
								<asp:Repeater ID="BrandSelectorRepeater" OnItemDataBound="BrandSelectorRepeater_ItemDataBound" runat="server">
								<HeaderTemplate>
									<table border="0" cellpadding="0" cellspacing="0">
									<tr>
										<td style="padding:4px 4px 6px 0px;" class="PanelTxt Bold">Brand</td>
										<td style="padding:4px 4px 6px 0px;" class="PanelTxt Bold alignCentre W50">Selected</td>
										<td style="padding:4px 4px 6px 0px;" class="PanelTxt Bold alignCentre W80">Main Brand</td>
									</tr>
								</HeaderTemplate>
								<FooterTemplate>
									</table>
								</FooterTemplate>
								<ItemTemplate>
									<asp:HiddenField ID="BrandIdHiddenField" runat="server" />
									<tr>
										<td><asp:Label ID="BrandNameLabel" CssClass="PanelTxt" runat="server" /></td>
										<td class="PanelTxt alignCentre"><asp:CheckBox ID="IsSelectedCheckBox" runat="server" /></td>
										<td class="PanelTxt alignCentre"><asp:RadioButton ID="IsMainBrandRadioButton" OnCheckedChanged="IsMainBrandRadioButton_CheckChanged" AutoPostBack="true" GroupName="IsMainBrand" runat="server" /></td>
									</tr>
								</ItemTemplate>
								</asp:Repeater>
							</td>
						</tr>
						<tr> 
							<td class="W130 alignRight PanelTxt" valign="top">Company <span class="ReqField">*</span></td>
							<td valign="top">
								<asp:DropDownList ID="CompanyDropDownList" CssClass="formInput W225 companyDropDown" runat="server" />
								<asp:TextBox ID="CompanyNameTextBox" CssClass="formInput W225 companyTextBox" style="margin:7px 0px 7px 0px" runat="server"/>
							</td>
						</tr>
						</table>
					</ContentTemplate>
					</aspajax:UpdatePanel>
					
					<table width="470" border="0" cellspacing="0" cellpadding="2">
					<tr> 
						<td class="W130 alignRight PanelTxt">Country&nbsp;&nbsp;&nbsp;</td>
						<td><app:CountryDropDownList ID="CountryDropDownList1" CssClass="formInput W225" runat="server" /></td>
					</tr>
					<tr>
						<td class="alignRight PanelTxt">Contact Phone Number</td>
						<td><asp:TextBox ID="PhoneNumberTextBox" CssClass="formInput W225" runat="server"/></td>
					</tr>
					<tr> 
						<td class="alignRight PanelTxt">Mobile&nbsp;&nbsp;&nbsp;</td>
						<td><asp:TextBox ID="MobileNumberTextBox" CssClass="formInput W225" runat="server"/></td>
					</tr>
					
					</table>
					
					<div class="HozLine"></div>
					
					<asp:PlaceHolder ID="DatesPanel" runat="server">
					<table width="470" border="0" cellspacing="0" cellpadding="2">
					<tr>
						<td class="W130 alignRight PanelTxt">Status:&nbsp;&nbsp;&nbsp;</td>
						<td class="PanelTxt Bold"><asp:Label ID="UserStatusLabel" runat="server" /></td>
					</tr>
					<tr>
						<td class="W130 alignRight PanelTxt">Date Registered:&nbsp;&nbsp;&nbsp;</td>
						<td class="PanelTxt Bold"><asp:Label ID="DateRegisteredLabel" runat="server" /></td>
					</tr>
					<tr>
						<td class="W130 alignRight PanelTxt">Last Logged In:&nbsp;&nbsp;&nbsp;</td>
						<td class="PanelTxt Bold"><asp:Label ID="DateLastLoginLabel" runat="server" /></td>
					</tr>
					<tr>
						<td class="alignRight PanelTxt">Password Expires:&nbsp;&nbsp;&nbsp;</td>
						<td class="PanelTxt">
							<asp:Label ID="DatePasswordExpiresLabel" style="float:left;display:block;width:140px" runat="server" />
							<app:EnhancedLinkButton ID="UpdatePasswordExpiryDateLinkButton" Prefix="&nbsp;&nbsp;&nbsp;" Suffix="" CssClass="PanelTxt" OnClick="UpdatePasswordExpiryDateLinkButton_Click" runat="server" />
						</td>
					</tr>
					<tr>
						<td class="W130 alignRight PanelTxt">Account Expires:&nbsp;&nbsp;&nbsp;</td>
						<td class="PanelTxt">
							<asp:Label ID="DateAccountExpiresLabel" style="float:left;display:block;width:140px" runat="server" />
							<app:EnhancedLinkButton ID="UpdateAccountExpiryDateLinkButton" Prefix="&nbsp;&nbsp;&nbsp;" Suffix="" CssClass="PanelTxt" OnClick="UpdateAccountExpiryDateLinkButton_Click" runat="server" />
						</td>
					</tr>
					<tr>
						<td class="W130 alignRight PanelTxt">API Token:&nbsp;&nbsp;&nbsp;</td>
						<td class="PanelTxt">
							<asp:Label ID="APITokenLabel" style="float:left;display:block;width:200px" runat="server" />&nbsp;&nbsp;&nbsp;
							<app:PromptLinkButton ID="RegenerateTokenLinkButton" Prompt="Regenerating the user token may break existing applications that depend on the current value. Are you sure you wish to continue?" Text="Regenerate Token" CssClass="PanelTxt" OnClick="RegenerateTokenLinkButton_Click" runat="server" />
						</td>
					</tr>
					</table>
					<div class="HozLine"></div>
					</asp:PlaceHolder>
					<table width="470" border="0" cellspacing="0" cellpadding="2">
					<tr>
						<td class="W130 alignRight PanelTxt">Permissions&nbsp;&nbsp;&nbsp;</td>
						<td><asp:DropDownList ID="UserRoleDropDownList" CssClass="formInput W225" runat="server"/></td>
					</tr>
					<tr>
						<td class="W130 alignRight PanelTxt" valign="top">Approver&nbsp;&nbsp;&nbsp;</td>
						<td class="PanelTxt">
							<app:UserDropDownList ID="ApproverUserDropDownList" CssClass="formInput W225" MinimumUserRoleId="3" OmitBlankItem="false" BlankValue="0" BlankText="User approves requests" runat="server"/>
							<br />
							<span style="font-size:10px">This is the user that will be responsible for approving order requests for assets uploaded by this user.</span>
						</td>
					</tr>
					<tr>
						<td class="W130 alignRight PanelTxt" valign="top">Workflow&nbsp;&nbsp;&nbsp;</td>
						<td>
							<asp:CheckBox ID="UseWorkflowCheckbox" CssClass="PanelTxt workflowCheckbox" Text="Uploaded assets require approval" runat="server" />
							<asp:DropDownList ID="WorkflowDropDownList" CssClass="formInput W225 workflowDropDown" style="margin-top:4px" runat="server" />
						</td>
					</tr>
					<tr>
						<td class="W130 alignRight PanelTxt">Enable Filepath Ingestion&nbsp;&nbsp;&nbsp;</td>
						<td class="PanelTxt"><asp:CheckBox ID="EnableFilePathIngestionCheckbox" runat="server" /></td>
					</tr>
					<tr>
						<td class="W130 alignRight PanelTxt">Account Suspended&nbsp;&nbsp;&nbsp;</td>
						<td class="PanelTxt"><asp:CheckBox ID="AccountSuspendedCheckbox" runat="server" /></td>
					</tr>
					<asp:PlaceHolder ID="AdvancedOptionsPanel" runat="server">
						<tr>
							<td class="W130 alignRight PanelTxt">Account Never Expires&nbsp;&nbsp;&nbsp;</td>
							<td class="PanelTxt"><asp:CheckBox ID="AccountNonExpiringCheckbox" runat="server" /></td>
						</tr>
						<tr>
							<td class="W130 alignRight PanelTxt">Password Never Expires&nbsp;&nbsp;&nbsp;</td>
							<td class="PanelTxt"><asp:CheckBox ID="PasswordNonExpiringCheckBox" runat="server" /></td>
						</tr>
						<tr>
							<td class="W130 alignRight PanelTxt">Home Use Permitted&nbsp;&nbsp;&nbsp;</td>
							<td class="PanelTxt"><asp:CheckBox ID="AccountExternalAccessAllowedCheckbox" runat="server" /></td>
						</tr>
					</asp:PlaceHolder>
					</table>
					<div class="alignRight" style="margin-top:10px;padding-top:10px;border-top:1px solid #ccc;">
						<asp:HyperLink ID="ViewAuditTrailHyperLink" CssClass="Left BodyTxt" style="margin-top:5px;" NavigateUrl="#" Text="view audit trail" runat="server" />
						<asp:Button ID="SaveButton" CssClass="button" Text="save user" OnClick="SaveButton_Click" runat="server" />
						<app:PromptButton ID="DeleteButton" CssClass="button" Text="delete user" OnClick="DeleteButton_Click" Prompt="Are you sure you want to delete this user?  This action cannot be undone!" runat="server" />
						<app:CancelButton ID="CancelButton" CssClass="button" Text="cancel" Prompt="Are you sure? User details will not be saved" RedirectUrl="UserList.aspx" runat="server" />
						&nbsp;&nbsp;&nbsp;
						<app:PromptButton ID="EmulateUserButton" Text="emulate user" Prompt="Are you sure? You will have to log out and back in as yourself afterwards" OnClick="EmulateUserButton_Click" CssClass="button" runat="server" />
					</div>
				</div>
			</asp:Panel>
			<asp:Panel ID="SuccessPanel" Visible="false" runat="server">
				<div class="AppCtrlMrg">
					<app:FeedbackLabel ID="FeedbackLabel2" LineBreaks="1" MessageType="Positive" Pinned="true" Header="User Updated" Text="The user details were updated successfully" runat="server" />
					<span class="BodyTxt Bold">What do you want to do now?</span>
					<ul>
						<li class="BodyTxt"><a class="Black" href="UserList.aspx">Return to the user list</a></li>
						<li class="BodyTxt"><a class="Black" href="UserForm.aspx?userId=<%=UserId%>">Edit the same user again</a></li>
					</ul>
				</div>
			</asp:Panel>
		</div>
		<div class="Right AdmRightPanel"></div>
		<div class="Clear"><br /></div>
	</div>
	<br />
</asp:Content>