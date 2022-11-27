<%@ Page Title="Registration &amp; Login Report [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="RegistrationLoginStats.aspx.cs" Inherits="FocusOPEN.Website.Admin.RegistrationLoginStats" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>
<%@ Register Src="~/Controls/Admin/LineChart.ascx" TagPrefix="app" TagName="LineChart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg">
		<div class="Left AdmLeftPanel"> 
			<div class="AdmAppHdr">Registration &amp; Login Report</div>
			<br />
			<br />
			<span class="AppCtrlMrg BodyTxt Bold">Select the company, brand and timeframe for this report | </span>
			<app:HelpHyperLink ID="HelpHyperLink1" HelpPage="11_Reports.htm#registration_login_stats" Text="help" CssClass="BodyTxt" runat="server" /><br />
			<div class="AppCtrlMrg AppCtrlPanel W500">
				<table border="0" cellspacing="0" cellpadding="2" class="W500">
				
				<app:SecuredPlaceHolder ID="RestrictedFilterOptionsPanel" AllowedRoles="SuperAdministrator" runat="server">
				<tr id="BrandSelectorWrapper" runat="server"> 
					<td class="alignRight PanelTxt Bold">Brand</td>
					<td><app:BrandDropDownList ID="BrandDropDownList1" BlankText="All" CssClass="formInput W180" runat="server" /></td>
				</tr>
				<tr> 
					<td class="alignRight PanelTxt Bold">Company</td>
					<td><app:CompanyDropDownList ID="CompanyDropDownList1" Restriction="None" ShowDomain="true" BlankText="All" CssClass="formInput W180" runat="server"/></td>
				</tr>
				</app:SecuredPlaceHolder>
				
				<tr> 
					<td class="alignRight PanelTxt Bold">Timeframe</td>
					<td><app:TimeFrameDropDownList ID="TimeFrameDropDownList1" FirstTimeframeName="Since System Release" OmitBlankItem="true" CssClass="formInput W180" runat="server" /></td>
				</tr>
				<tr>
					<td class="alignRight PanelTxt Bold">Accrue Figures</td>
					<td class="PanelTxt"><asp:CheckBox ID="AccrueCheckBox" Text="Add previous month totals for graphs" runat="server" /></td>
				</tr>
				</table>
				<br />
				<div class="alignRight Clear W500">
					<app:CancelButton ID="ResetButton" Text="reset" CssClass="button" Prompt="" RedirectUrl="RegistrationLoginStats.aspx" runat="server" />
					<asp:Button ID="GenerateReportButton" Text="generate report" CssClass="button" OnClick="GenerateReportButton_Click" runat="server" />
				</div>
			</div>
		</div>
		<div class="Right AdmRightPanel"></div>
		<div class="AdmTblPanel Clear">
			
			<asp:Panel ID="InitialPanel" runat="server">
				<div class="Clear noResultsAdm"><br /></div>
			</asp:Panel>
			
			<asp:Panel ID="ResultsPanel" Visible="false" runat="server">
				<div class="Left">
					<div class="AdmAppHdr">Registration &amp; Login Report</div>
					<br />
					<div class="AppResults PanelTxt">
						total results:
						<asp:Label ID="RegistrationInfoTopLabel" CssClass="PanelTxt" Text="10% increase in registrations" runat="server" />
						|
						<asp:Label ID="LoginInfoTopLabel" CssClass="PanelTxt" Text="15% increase in logins" runat="server" />
						|
						<app:HelpHyperLink ID="HelpHyperLink2" CssClass="PanelTxt" HelpPage="11_Reports.htm#registration_login_stats" Text="help" runat="server" />
					</div>
				</div>
				<div class="Clear">
					<div class="AppCtrlMrg AppCtrlPanel W500">
						<br />
						<div class="BodyTxt Bold alignCentre W500">Registration Report For<br /><%=GetChartHeader()%></div>
						<div class="alignCentre W500"><app:LineChart ID="RegistrationChart" runat="server" /></div>
					</div>
					<br />
					<table class="W520 TblOutline AppTblMrg" border="0" cellspacing="0" cellpadding="2">
					<tr>
						<td class="alignRight TblCell2 Bold PanelTxt W260">No. Of Registrations At Timeframe Start </td>
						<td class="TblCell1 PanelTxt"><%=RegistrationCountAtStart%></td>
					</tr>
					<tr>
						<td class="alignRight TblCell2 Bold PanelTxt W260">No. Of Registrations At Timeframe End</td>
						<td class="TblCell1 PanelTxt"><%=RegistrationCountAtEnd%></td>
					</tr>
					<tr> 
						<td class="alignRight TblCell2 Bold PanelTxt">Increase Over Timeframe</td>
						<td class="TblCell1 PanelTxt"><%=RegistrationCountAtEnd-RegistrationCountAtStart%></td>
					</tr>
					<tr> 
						<td class="alignRight TblCell2 Bold PanelTxt">Percentage Rise</td>
						<td class="TblCell1 PanelTxt"><asp:Label ID="RegistrationCountPercentageRiseLabel" Text="[RegistrationCountPercentageRiseLabel]" runat="server" /></td>
					</tr>
					</table>
					<br />
					<div class="AppCtrlMrg AppCtrlPanel W500">
						<br />
						<div class="BodyTxt Bold alignCentre W500">Login Report For<br /><%=GetChartHeader()%></div>
						<div class="alignCentre W500"><app:LineChart ID="LoginChart" runat="server" /></div>
					</div>
					<br />
					<table class="W520 TblOutline AppTblMrg" border="0" cellspacing="0" cellpadding="2">
					<tr>   
						<td class="alignRight TblCell2 Bold PanelTxt W260">No. Of Logins At Timeframe Start</td>
						<td class="TblCell1 PanelTxt"><%=LoginCountAtStart%></td>
					</tr>
					<tr>   
						<td class="alignRight TblCell2 Bold PanelTxt W260">No. Of Logins At Timeframe End</td>
						<td class="TblCell1 PanelTxt"><%=LoginCountAtEnd%></td>
					</tr>
					<tr> 
						<td class="alignRight TblCell2 Bold PanelTxt">Increase Over Timeframe</td>
						<td class="TblCell1 PanelTxt"><%=LoginCountAtEnd-LoginCountAtStart%></td>
					</tr>
					<tr> 
						<td class="alignRight TblCell2 Bold PanelTxt">Percentage Rise</td>
						<td class="TblCell1 PanelTxt"><asp:Label ID="LoginCountPercentageRiseLabel" Text="[LoginCountPercentageRiseLabel]" runat="server" /></td>
					</tr>
					</table>
				</div>
			</asp:Panel>
			
			<br /> 
		</div>
	</div>
	<div class="AdmAppFtr">
		<asp:Panel ID="InitialFooterPanel" runat="server">
			&nbsp;
		</asp:Panel>
		<asp:Panel ID="ResultsFooterPanel" Visible="false" runat="server">
			<div class="Left">
				<div class="AppResults PanelTxt">
					total results:
					<asp:Label ID="RegistrationInfoBottomLabel" CssClass="PanelTxt" Text="10% increase in registrations" runat="server" />
					|
					<asp:Label ID="LoginInfoBottomLabel" CssClass="PanelTxt" Text="15% increase in logins" runat="server" />
					|
					<app:HelpHyperLink ID="HelpHyperLink3" CssClass="PanelTxt" HelpPage="11_Reports.htm#registration_login_stats" Text="help" runat="server" />
				</div>
			</div>
		</asp:Panel>
	</div>
</asp:Content>