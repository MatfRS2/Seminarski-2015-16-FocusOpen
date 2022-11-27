<%@Page Title="Drive Information [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="DriveInfo.aspx.cs" Inherits="FocusOPEN.Website.Admin.DriveInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg"> 
		<div class="Left" style="width:900px"> 
			<div class="AdmAppHdr">Drive Information</div>
			<br />
			<div class="AppCtrlMrg">
				<span class="BodyTxt">
					Below is a list of drives and the space used
					|
					<app:HelpHyperLink HelpPage="10_Utilities.htm#drive_info" CssClass="BodyTxt" Text="help" runat="server" />
				</span>
			</div>
			<br />
			<div class="AppCtrlMrg" style="width:100%">
				<asp:Repeater ID="DriveRepeater" OnItemDataBound="DriveRepeater_ItemDataBound" runat="server">
					<ItemTemplate>
						<table border="0" cellpadding="5" cellspacing="0" width="100%">
						<tr>
							<td valign="top" width="50%">
								<h2><asp:Literal ID="DriveLiteral" runat="server" /></h2>
								
								<asp:Label ID="ErrorLabel" ForeColor="Red" Font-Bold="true" Visible="false" runat="server" />
								
								<asp:Panel ID="DriveInfoPanel" runat="server">
									<table border="0" cellpadding="3" cellspacing="0" width="100%">
									<tr>
										<td>Drive Name:</td>
										<td align="right"><asp:Label ID="DriveNameLabel" runat="server" /></td>
									</tr>
									<tr>
										<td>Drive Type:</td>
										<td align="right"><asp:Label ID="DriveTypeLabel" runat="server" /></td>
									</tr>
									<tr>
										<td>Drive Volume Label:</td>
										<td align="right"><asp:Label ID="DriveVolumeLabel" runat="server" /></td>
									</tr>
									<tr>
										<td>Drive Format:</td>
										<td align="right"><asp:Label ID="DriveFormatLabel" runat="server" /></td>
									</tr>
									<tr>
										<td>Ready:</td>
										<td align="right"><asp:Label ID="DriveReadyLabel" runat="server" /></td>
									</tr>
									<tr>
										<td>Used Space:</td>
										<td align="right"><asp:Label ID="DriveUsedSpaceLabel" runat="server" /></td>
									</tr>
									<tr>
										<td>Available Free Space:</td>
										<td align="right"><asp:Label ID="DriveAvailableFreeSpaceLabel" runat="server" /></td>
									</tr>
									<tr>
										<td>Total Free Space:</td>
										<td align="right"><asp:Label ID="DriveTotalFreeSpaceLabel" runat="server" /></td>
									</tr>
									<tr>
										<td>Total Space:</td>
										<td align="right"><asp:Label ID="DriveTotalSpaceLabel" runat="server" /></td>
									</tr>
									</table>
									</asp:Panel>
								</td>
								<td valign="top" align="right" width="50%">
									<asp:Literal ID="ChartLiteral" runat="server" />
								</td>
							</tr>
							</table>
						
					</ItemTemplate>
					<SeparatorTemplate>
					<tr>
						<td colspan="2"><hr /></td>
					</tr>
					</SeparatorTemplate>
				</asp:Repeater>
			</div>
		</div>
		<div class="Right AdmRightPanel"></div>
		<div class="Clear"><br /></div>
	</div>
	<div class="AdmAppFtr"></div>
</asp:Content>