<%@ Page Title="Change asset permissions [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="AssetsBulkChangePermissions.aspx.cs" Inherits="FocusOPEN.Website.Admin.Assets.AssetsBulkChangePermissions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg"> 
		<div class="Left AdmLeftPanel"> 
			<div class="AdmAppHdr">Change Asset Permissions</div>
			<app:FeedbackLabel ID="MessageLabel1" UseContainer="true" runat="server" /><br />
		</div>
		<div class="Right AdmRightPanel"></div>
		<div class="AdmTblPanel Clear">
			<div class="Clear">
				<div style="margin-left:10px;" class="BodyTxt">
					Select the new permissions below.
					<br /><br />
					<table cellpadding="5" cellspacing="0" border="0">

					<%-------------------------------------------------------------------------------------%>
					<%-- Internal Users Restrictions ------------------------------------------------------%>
					<%-------------------------------------------------------------------------------------%>
					<tr>
						<td class="W150 alignRight Top">
							<a class="PanelTxt Bold" href="#">Internal Users</a>
							<br />
						</td>
						<td class="PanelTxt">
							<asp:RadioButtonList ID="InternalUsersRestrictionsRadioButtonList" CssClass="BodyTxt" RepeatDirection="Vertical" runat="server">
								<asp:ListItem Value="None" Text="No download restrictions" Selected="True" />
								<asp:ListItem Value="NotVisible" Text="Not Visible" />
								<asp:ListItem Value="ApprovalRequired" Text="Approval required to download this asset" />
							</asp:RadioButtonList>
						</td>
					</tr>

					<%-------------------------------------------------------------------------------------%>
					<%-- External Users Restrictions ------------------------------------------------------%>
					<%-------------------------------------------------------------------------------------%>
					<tr>
						<td class="W150 alignRight Top">
							<a class="PanelTxt Bold" href="#">External Users</a>
							<br />
						</td>
						<td class="PanelTxt">
							<asp:RadioButtonList ID="ExternalUsersRestrictionsRadioButtonList" CssClass="BodyTxt" RepeatDirection="Vertical" runat="server">
								<asp:ListItem Value="None" Text="No download restrictions" Selected="True" />
								<asp:ListItem Value="NotVisible" Text="Not Visible" />
								<asp:ListItem Value="ApprovalRequired" Text="Approval required to download this asset" />
							</asp:RadioButtonList>
						</td>
					</tr>
					</table>
                    <br /><br />
					<app:PromptButton ID="ReassignButton" Prompt="Are you sure you want to change the permissions of the selected assets?" Text="update assets" CssClass="button" OnClick="ReassignButton_Click" runat="server" />
					<app:CancelButton ID="CancelButton" Prompt="Are you sure? Assets permissions will not be changed" RedirectUrl="AssetList.aspx" Text="cancel" CssClass="button" runat="server" />
				</div>
				<br />
				<app:AssetList ID="AssetList1" AllowDelete="false" runat="server" />
				<br />
			</div>
		</div>
	</div>
	<div class="AdmAppFtr"></div>
</asp:Content>