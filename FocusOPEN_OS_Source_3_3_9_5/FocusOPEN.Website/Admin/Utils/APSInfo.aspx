<%@ Page Async="true" AsyncTimeout="60" Title="Asset Processing Server Info [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="APSInfo.aspx.cs" Inherits="FocusOPEN.Website.Admin.APSInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg"> 
		<div class="Left AdmLeftPanel"> 
			<div class="AdmAppHdr">Asset Processing Server Info</div>
			<br />
			<div class="AppCtrlMrg">
				<span class="BodyTxt">
					Below is information about the Asset Processing Server
					|
					<app:HelpHyperLink ID="HelpHyperLink1" HelpPage="10_Utilities.htm#asset_processing_server_info" CssClass="BodyTxt" Text="help" runat="server" />
				</span>
			</div>
			<br />
		</div>
		<div class="Right AdmRightPanel"></div>
		<div class="AdmTblPanel Clear">
			<div class="Clear">
				<div class="AppCtrlMrg W700">
					<br />
					<app:FeedbackLabel ID="APSFeedbackLabel" Pinned="true"  runat="server" />
					<br />
					<asp:PlaceHolder ID="StatusWrapper" runat="server">
						<table border="0" cellpadding="5" cellspacing="0" runat="server">
						<tr>
							<td class="BodyTxt Bold">Server Version:</td>
							<td class="BodyTxt"><asp:Label ID="VersionLabel" runat="server" /></td>
						</tr>
						<tr>
							<td class="BodyTxt Bold">Server Build Date:</td>
							<td class="BodyTxt"><asp:Label ID="BuildDateLabel" runat="server" /></td>
						</tr>
						<tr>
							<td colspan="2">&nbsp;</td>
						</tr>
						<tr>
							<td class="BodyTxt Bold">Server started:</td>
							<td class="BodyTxt"><asp:Label ID="StartDateLabel" runat="server" /></td>
						</tr>
						<tr>
							<td class="BodyTxt Bold">Time service has been up:</td>
							<td class="BodyTxt"><asp:Label ID="ServiceUptimeLabel" runat="server" /></td>
						</tr>
						<tr>
							<td class="BodyTxt Bold">Job queue last checked:</td>
							<td class="BodyTxt"><asp:Label ID="LastCheckDate" runat="server" /></td>
						</tr>
						<tr>
							<td class="BodyTxt Bold">Number of jobs pending:</td>
							<td class="BodyTxt"><asp:Label ID="NumberOfJobsPendingCountLabel" runat="server" /> jobs</td>
						</tr>
						<tr>
							<td class="BodyTxt Bold">Number of jobs in progress:</td>
							<td class="BodyTxt"><asp:Label ID="NumberOfJobsInProgressCountLabel" runat="server" /> jobs</td>
						</tr>
						<tr>
							<td class="BodyTxt Bold">Number of jobs processed:</td>
							<td class="BodyTxt"><asp:Label ID="NumberOfJobsProcessedCountLabel" runat="server" /> jobs</td>
						</tr>
						</table>
						<div class="BodyTxt" style="padding:25px 5px 0px 5px;">
							<strong>Supported File Extensions:</strong><br />
							<asp:Literal ID="SupportedFileExtensionsLiteral" runat="server" />
						</div>
					</asp:PlaceHolder>
					<br />
				</div>
			</div>
		</div>
	</div>
	<br />
</asp:Content>
