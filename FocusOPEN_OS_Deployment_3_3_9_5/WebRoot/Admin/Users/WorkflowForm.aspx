<%@ Page Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="WorkflowForm.aspx.cs" Inherits="FocusOPEN.Website.Admin.WorkflowForm" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg"> 
		<div class="Left AdmLeftPanel"> 
			<div id="HeaderTextDiv" runat="server" class="AdmAppHdr">Add Workflow</div>
			<br />
			<br />
			<div class="AppCtrlMrg"><asp:Panel ID="MessageLabelPanel" runat="server"><app:FeedbackLabel ID="MessageLabel1" runat="server" /></asp:Panel></div>
			<br />

			<asp:PlaceHolder ID="ExistingWorkflowInfoPlaceHolder1" Visible="false" runat="server">
				<div class="AppCtrlMrg">
					<span class="BodyTxt">Edit this workflow below | </span><app:HelpHyperLink HelpPage="6_User_Admin.htm#manage_workflows" CssClass="BodyTxt" Text="help" runat="server" /><br /><br />
				</div>
			</asp:PlaceHolder>
			
			<asp:PlaceHolder ID="NewWorkflowInfoPlaceHolder1" Visible="false" runat="server">
				<div class="AppCtrlMrg">
					<span class="BodyTxt">Select a brand to create a workflow from or create your own below | </span><a href="#" class="BodyTxt">help</a><br /><br />
				</div>
			</asp:PlaceHolder>
			
			<div class="AppCtrlMrg AppCtrlPanel W550">
				
				<asp:PlaceHolder ID="NewWorkflowInfoPlaceHolder2" Visible="false" runat="server">
					<table width="550" border="0" cellspacing="0" cellpadding="2">
					<tr> 
						<td class="W130 alignRight PanelTxt Bold">Select Brand</td>
						<td>
							<app:BrandDropDownList ID="BrandDropDownList" CssClass="formInput W150" runat="server" />
							<asp:Button ID="CreateWorkflowFromBrandButton" CssClass="button W100" Text="create workflow" OnClick="CreateWorkflowFromBrandButton_Click" runat="server" />
						</td>
					</tr>
					</table>
					<div class="CatHozLine"></div>
				</asp:PlaceHolder>
				
				<table width="550" border="0" cellspacing="0" cellpadding="2">
				<asp:PlaceHolder ID="WorkflowBrandSelector" runat="server">
				<tr>
					<td class="W130 alignRight PanelTxt Bold">Brand<span class="ReqField">*</span></td>
					<td colspan="2">
						<app:BrandDropdownList ID="WorkflowBrandDropDownList" CssClass="formInput W150" OmitBlankItem="true" OnSelectedIndexChanged="WorkflowBrandDropDownList_SelectedIndexChanged" AutoPostBack="true" runat="server"/>
					</td>
				</tr>
				</asp:PlaceHolder>
				<tr>
					<td class="W130 alignRight PanelTxt Bold">Workflow Name<span class="ReqField">*</span></td>
					<td colspan="2">
						<asp:TextBox ID="WorkflowNameTextBox" CssClass="formInput W150" runat="server" />
					</td>
				</tr>
				<tr>
					<td class="W130 alignRight PanelTxt Bold Top">Select Users <span class="ReqField">*</span></td>
					<td colspan="2">
						<asp:ListBox ID="AllUsersListBox" Rows="10" SelectionMode="multiple" CssClass="BodyTxt W150" runat="server" />
						<br />
						<br />
						<asp:Button ID="AddToWorkflowButton" OnClick="AddToWorkflowButton_Click" Text="add to workflow" CssClass="button W150" runat="server" />
						<br /><br />
					</td>
				</tr>
				<tr> 
					<td class="alignRight PanelTxt Top W130"><span class="Bold">In This Workflow</span><br />(first to last in position)</td>
					<td class="PanelTxt Top W150">
						<asp:ListBox ID="SelectedUsersListBox" Rows="10" SelectionMode="multiple" CssClass="BodyTxt W150" runat="server" />
						<br />
						<br />
						<asp:Button ID="RemoveFromWorkflowButton" Text="remove from workflow" CssClass="button W150" OnClick="RemoveFromWorkflowButton_Click" runat="server" />
					</td>
					<td>
						<asp:Button ID="MoveUpButton" Text="move up" CssClass="button W75" OnClick="MoveUpButton_Click" runat="server" />
						<br /><br />
						<asp:Button ID="MoveDownButton" Text="move down" CssClass="button W75" OnClick="MoveDownButton_Click" runat="server" />
						<br />
					</td>
				</tr>
				</table>
				<div class="CatHozLine"></div>
				<div class="alignRight">
					<asp:Button ID="SaveButton" OnClick="SaveButton_Click" CssClass="button" Text="save" runat="server" />
					<app:CancelButton ID="CancelButton" Prompt="Are you sure you want to cancel?  Changes will not be saved" RedirectUrl="WorkflowList.aspx" CssClass="button" Text="cancel" runat="server" />
				</div>
			</div>
		</div>
		<div class="Right AdmRightPanel"></div>
		<div class="AdmTblPanel Clear">
			<div class="Clear">
				<br />
				<br />
			</div>
		</div>
	</div>
</asp:Content>