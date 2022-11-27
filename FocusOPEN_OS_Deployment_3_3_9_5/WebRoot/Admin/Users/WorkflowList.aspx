<%@ Page Title="Manage Workflows [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="WorkflowList.aspx.cs" Inherits="FocusOPEN.Website.Admin.WorkflowList" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg"> 
		<div class="Left AdmLeftPanel"> 
			<div class="AdmAppHdr">Manage Workflows</div>
			<br />
			<app:FeedbackLabel ID="MessageLabel1" UseContainer="true" runat="server" />
			<br />
			<div class="AppCtrlMrg">
				<span class="Bold BodyTxt">Click on 'edit' to edit a workflow, or 'delete' to remove a workflow.<br />
				New workflows can be added <a href="WorkflowForm.aspx" class="Black">here</a> | </span>
				<app:HelpHyperLink HelpPage="6_User_Admin.htm#manage_workflows" CssClass="BodyTxt" Text="help" runat="server" /><br />
			</div>
		</div>
		
		<asp:PlaceHolder ID="BrandSelectorWrapper" runat="server">
			<div style="float:left;margin:10px 5px 10px 5px;padding:5px;width:100%;clear:both;">
				<span class="BodyTxt">Brand:</span>
				<app:BrandDropDownList ID="BrandDropDownList" CssClass="formInput W200" OmitBlankItem="false" BlankText="All" OnSelectedIndexChanged="BrandDropDownList_SelectedIndexChanged" AutoPostBack="true" runat="server" />
			</div>
		</asp:PlaceHolder>
		
		<div class="Right AdmRightPanel"></div>
		<div class="AdmTblPanel Clear">
			<div class="Left">
				<div class="AppResults PanelTxt">total results: <span class="Bold"><%=RecordCount%></span> workflows [displaying page <%=CurrentPage%> of <%=TotalPages%>]</div>
			</div>
			<div class="Right">
				<div class="AppPageView">
					<span class="PanelTxt">show</span>
					<app:PageSizeDropDownList ID="PageSizeDropDownList1" OmitBlankItem="true" AutoPostBack="true" OnSelectedIndexChanged="PageSizeDropDownList1_SelectedIndexChanged" Mode="Admin" CssClass="formInput" runat="server" />
					<div class="AppPaging">
						<app:SimplePager ID="TopPager" OnCommand="SimplePager_OnCommand" runat="server" />
					</div>
				</div>
			</div>
			<div class="Clear">
				<br />
				<app:SortableRepeater ID="WorkflowsRepeater" DefaultSortExpression="Name" DefaultSortAscending="true" ColumnCount="1" OnItemDataBound="WorkflowsRepeater_ItemDataBound" runat="server">
					<HeaderTemplate>
						<table width="910" border="0" cellspacing="0" cellpadding="2" class="TblOutline AppTblMrg AutoStripe">
						<tr>
							<td id="HeaderCell1" runat="server" class="Bold"><asp:LinkButton ID="LinkButton1" CssClass="BodyTxt Bold" Text="Workflow" OnCommand="SortListLinkButton_Command" CommandArgument="Name" runat="server" /></td>
							<td id="HeaderCell2" runat="server" class="Bold"><asp:LinkButton ID="LinkButton2" CssClass="BodyTxt Bold" Text="Brand" OnCommand="SortListLinkButton_Command" CommandArgument="BrandName" runat="server" /></td>
							<td class="Bold">&nbsp;</td>
						</tr>
					</HeaderTemplate>
					<ItemTemplate>
						<tr> 
							<td><asp:HyperLink ID="WorkflowNameHyperLink" CssClass="BodyTxt" runat="server" /></td>
							<td><asp:Label ID="BrandNameLabel" CssClass="BodyTxt" runat="server" /></td>
							<td class="W75 alignCentre">
								<asp:HyperLink ID="WorkflowEditHyperLink" CssClass="BodyTxt" Text="edit" runat="server" />
								<span class="BodyTxt">|</span>
								<app:PromptLinkButton ID="WorkflowDeleteLinkButton" Prompt="Are you sure you want to delete this workflow?" CssClass="BodyTxt" Text="delete" OnCommand="WorkflowDeleteLinkButton_Command" runat="server" />
							</td>
						</tr>
					</ItemTemplate>
					<FooterTemplate>
						</table>
					</FooterTemplate>
				</app:SortableRepeater>
				<asp:Panel ID="NoResultsPanel" Visible="false" runat="server">
					<div class="noResultsAdm AppCtrlMrg">
						<app:FeedbackLabel ID="NoResultsMessageLabel" MessageType="Negative" Header="no results found" Pinned="true" Visible="true" runat="server" />
					</div>
				</asp:Panel>
				<br />
			</div>
		</div>
	</div>
	<div class="AdmAppFtr">
		<div class="Left">
			<div class="AppResults PanelTxt">total results: <span class="Bold"><%=RecordCount%></span> workflows [displaying page <%=CurrentPage%> of <%=TotalPages%>]</div>
		</div>
		<div class="Right">
			<div class="AppPageView AppPaging">
				<app:SimplePager ID="BottomPager" OnCommand="SimplePager_OnCommand" runat="server" />
			</div>
		</div>
	</div>
</asp:Content>