<%@ Page Title="View Logs [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="LogViewer.aspx.cs" Inherits="FocusOPEN.Website.Admin.LogViewer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg"> 
		<div class="Left W700"> 
			<div class="AdmAppHdr">View Logs</div>
			<br />
			<div class="AppCtrlMrg">
				<span class="BodyTxt">
					Below is a list of log files.  Click on a filename to download
					|
					<app:HelpHyperLink ID="HelpHyperLink1" HelpPage="10_Utilities.htm#view_logs" CssClass="BodyTxt" Text="help" runat="server" />
				</span>
			</div>
			<br />
			<div class="AppCtrlMrg">
				<asp:DataList ID="LogFilesDataList" OnItemDataBound="LogFilesDataList_ItemDataBound" runat="server">
					<ItemTemplate>
						&middot; <asp:LinkButton ID="DownloadLogLinkButton" CssClass="BodyTxt" OnCommand="DownloadLogLinkButton_Command" runat="server" />
						<br />
					</ItemTemplate>
				</asp:DataList>
			</div>
		</div>
		<div class="Right AdmRightPanel"></div>
		<div class="Clear"><br /></div>
	</div>
	<div class="AdmAppFtr"></div>
</asp:Content>