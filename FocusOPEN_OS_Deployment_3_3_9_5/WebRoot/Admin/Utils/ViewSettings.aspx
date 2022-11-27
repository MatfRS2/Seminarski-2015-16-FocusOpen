<%@ Page Title="View Settings [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="ViewSettings.aspx.cs" Inherits="FocusOPEN.Website.Admin.ViewSettings" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg"> 
		<div class="Left"> 
			<div class="AdmAppHdr">View Settings</div>
			<br />
			<div class="AppCtrlMrg">
				<span class="BodyTxt">
					Below is a list of configuration settings
					|
					<app:HelpHyperLink HelpPage="10_Utilities.htm#view_settings" CssClass="BodyTxt" Text="help" runat="server" />
				</span>
			</div>
			<br />
		</div>
		<div class="Right AdmRightPanel"></div>
		<div class="AdmTblPanel Clear">
			<div class="Clear">
				<app:FeedbackLabel ID="GeneralFeedbackLabel" UseContainer="true" LineBreaks="1" runat="server" />
				<div class="AppCtrlMrg">
					<span class="BodyTxt Bold">Server Name:</span>
					<asp:Label ID="ServerNameLabel" CssClass="BodyTxt" runat="server" />
					<br />
					<span class="BodyTxt Bold">Base URL:</span>
					<span class="BodyTxt"><%=FocusOPEN.Shared.WebUtils.GetBaseUrl()%></span>
				</div>
				<br />
				<app:SortableDataGrid ID="SettingsDataGrid" ShowFooter="false" AllowSorting="false" runat="server">
					<HeaderStyle Height="30px" />
					<ItemStyle Height="25px" VerticalAlign="Middle" />
					<Columns>
						<asp:BoundColumn DataField="Key" ItemStyle-CssClass="BodyTxt" HeaderText="Setting Name" />
						<asp:BoundColumn DataField="Value" ItemStyle-CssClass="BodyTxt" HeaderText="Value" />
					</Columns>
				</app:SortableDataGrid>
				<br />
				<div class="BodyTxt" style="margin-left:10px">
					<strong>Version Info:</strong><br />
					<app:VersionLabel ID="VersionLabel" DisplayMode="Html" runat="server" />
				</div>
				<br />
				<div style="margin:10px">
					<asp:Button ID="RestartApplicationButton" Text="Restart Application" OnClick="RestartApplicationButton_Click" CssClass="button" runat="server" />
					<asp:Button ID="ReloadSettingsButton" Text="Reload Settings" OnClick="ReloadSettingsButton_Click" CssClass="button" runat="server" />
				</div>
				<br />
			</div>
		</div>
	</div>
	<br />
</asp:Content>
