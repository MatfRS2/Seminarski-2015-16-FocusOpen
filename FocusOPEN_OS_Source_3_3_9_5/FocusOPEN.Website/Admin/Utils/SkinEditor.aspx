<%@ Page Title="Edit UI [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="SkinEditor.aspx.cs" Inherits="FocusOPEN.Website.Admin.Utils.SkinEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg"> 
		<div class="Left"> 
			<div id="PageHeaderDiv" runat="server" class="AdmAppHdr"></div>
			<app:FeedbackLabel ID="FeedbackLabel" UseContainer="true" runat="server" />
			<br />
			<div class="AppCtrlMrg">
				<asp:Panel ID="FileEditorPanel" runat="server">
					<asp:TextBox ID="FileContentTextBox" Width="700px" Height="450px" TextMode="MultiLine" style="border:1px solid #666" CssClass="sizable" runat="server" />
					<br />
					<asp:Button ID="SaveButton" CssClass="button" OnClick="SaveButton_Click" Text="Save changes" runat="server" />
					<app:PromptButton ID="CancelButton" CssClass="button" OnClick="CancelButton_Click" Prompt="Are you sure? Changes will not be saved." Text="Cancel without saving" runat="server" />
				</asp:Panel>
				<asp:Panel ID="NoFileSelectedPanel" runat="server">
					<span class="BodyTxt Bold">Please select a file to edit from the list on the right.</span>
					<br /><br />
					<a href="../Content/ManageBrandsForm.aspx?BrandId=<%=FocusOPEN.Shared.WebUtils.GetIntRequestParam("BrandId", 0) %>" class="BodyTxt Bold">&laquo; Back to brand</a>
				</asp:Panel>
			</div>
		</div>
		<div class="Right AdmRightPanel" style="width:200px;margin-top:55px">
			<asp:Repeater ID="FileRepeater" OnItemDataBound="FileRepeater_ItemDataBound" runat="server">
				<ItemTemplate>
					<asp:LinkButton ID="FileLinkButton" CssClass="Black BodyTxt" OnCommand="FileLinkButton_Command" runat="server" />
					<br />
				</ItemTemplate>
			</asp:Repeater>
		</div>
		<div class="Clear"><br /></div>
	</div>
	<div class="AdmAppFtr"></div>
</asp:Content>