<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssetWorkflowUserList.ascx.cs" Inherits="FocusOPEN.Website.Controls.AssetWorkflowUserList" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<asp:Repeater ID="WorkflowRepeater" OnItemDataBound="WorkflowRepeater_ItemDataBound" runat="server">
	<HeaderTemplate><span class="Bold">Workflow: </span> </HeaderTemplate>
	<FooterTemplate><br /></FooterTemplate>
	<SeparatorTemplate>&nbsp;&gt;&nbsp;</SeparatorTemplate>
	<ItemTemplate><app:EmailHyperLink ID="EmailHyperLink" runat="server" /></ItemTemplate>
</asp:Repeater>