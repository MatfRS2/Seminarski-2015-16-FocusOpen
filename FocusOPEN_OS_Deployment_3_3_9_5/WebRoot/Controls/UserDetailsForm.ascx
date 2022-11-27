<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserDetailsForm.ascx.cs" Inherits="FocusOPEN.Website.Controls.UserDetailsForm" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<table width="420" border="0" cellspacing="0" cellpadding="2" class="TblOutline">
<tr> 
	<td class="TblCell2 W180 alignRight PanelTxt Bold">First Name <span class="ReqField">*</span></td>
	<td class="TblCell1"><asp:TextBox ID="FirstNameTextBox" CssClass="formInput W225" runat="Server" /></td>
</tr>
<tr> 
	<td class="TblCell2 alignRight PanelTxt Bold">Surname <span class="ReqField">*</span></td>
	<td class="TblCell1"><asp:TextBox ID="LastNameTextBox" CssClass="formInput W225" runat="Server" /></td>
</tr>
<tr> 
	<td class="TblCell2 alignRight PanelTxt Bold">Email Address <span class="ReqField">*</span></td>
	<td class="TblCell1"><asp:TextBox ID="EmailTextBox" CssClass="formInput W225" runat="Server" /></td>
</tr>
<tr> 
	<td class="TblCell2 alignRight PanelTxt Bold">Password <span class="ReqField">*</span></td>
	<td class="TblCell1"><asp:TextBox ID="PasswordTextBox" TextMode="Password" CssClass="formInput W225" runat="Server" /></td>
</tr>
<tr> 
	<td class="TblCell2 alignRight PanelTxt Bold">Confirm Password <span class="ReqField">*</span></td>
	<td class="TblCell1"><asp:TextBox ID="ConfirmPasswordTextBox" TextMode="Password" CssClass="formInput W225" runat="Server" /></td>
</tr>

<asp:PlaceHolder ID="RegistrationOnlyOptions" runat="server">
<tr>
	<td class="TblCell2 alignRight PanelTxt Bold" style="border-top:0px;">Are you a staff user? <span class="ReqField">*</span></td>
	<td class="TblCell1 BodyTxt" style="border-top: 0px;">
		<asp:RadioButtonList ID="IsStaffUserRadioButtonList" OnSelectedIndexChanged="IsStaffUserRadioButtonList_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Horizontal" runat="server">
			<asp:ListItem Value="1" Selected="true">Yes</asp:ListItem>
			<asp:ListItem Value="0">No</asp:ListItem>
		</asp:RadioButtonList>
	</td>
</tr>
<tr>
	<td class="TblCell2 alignRight PanelTxt">Company name <span class="ReqField">*</span></td>
	<td class="TblCell1">
	    <asp:DropDownList ID="CompanyDropDownList" CssClass="formInput W225 companyDropDown" runat="server" />
	    <asp:TextBox ID="CompanyNameTextBox" Enabled="False" CssClass="formInput W225" runat="Server" />
	</td>
</tr>
<tr id="BrandSelectorRow" runat="server">
	<td class="TblCell2 alignRight PanelTxt Bold">Brand</td>
	<td class="TblCell1"><app:BrandDropDownList ID="BrandDropDownList" CssClass="formInput W225" OmitBlankItem="true" runat="server" /></td>
</tr>
</asp:PlaceHolder>

<tr> 
	<td class="TblCell2 alignRight PanelTxt">Country</td>
	<td class="TblCell1"><app:CountryDropDownList ID="CountryDropDownList1" CssClass="formInput W225" runat="server" /></td>
</tr>
<tr> 
	<td class="TblCell2 alignRight PanelTxt">Contact Phone Number</td>
	<td class="TblCell1"><asp:TextBox ID="PhoneNumberTextBox" CssClass="formInput W225" runat="Server" /></td>
</tr>
<tr> 
	<td class="TblCell2 alignRight PanelTxt">Mobile</td>
	<td class="TblCell1"><asp:TextBox ID="MobileNumberTextBox" CssClass="formInput W225" runat="Server" /></td>
 </tr>
</table>