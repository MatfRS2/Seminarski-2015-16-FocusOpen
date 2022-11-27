<%@ Page Title="Manage Preview Plug-ins [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="ManagePlugins.aspx.cs" Inherits="FocusOPEN.Website.Admin.Content.ManagePlugins" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

	<div class="AdmBkg"> 
		<div class="Left"> 
			<div class="AdmAppHdr">Manage Preview Plug-ins </div>
			<app:FeedbackLabel ID="FeedbackLabel" UseContainer="true" runat="server" />
			<br />
			<div class="AppCtrlMrg AppCtrlPanel W470">
				<span class="BodyTxt">Use this to register, unregister and troubleshoot any preview plug-ins 				|
				<app:HelpHyperLink ID="HelpHyperLink1" HelpPage="9_Manage_Content.htm#manage_plugins" CssClass="BodyTxt" Text="help" runat="server" />
				</span>
				<br /><br />
                <asp:LinkButton ID="AddPluginButton" CssClass="BodyTxt Bold" OnClick="AddPluginButton_Click" runat="server">Add a Preview Plug-in</asp:LinkButton>
                <div><app:FeedbackLabel runat="server" ID="AddPluginFeedbackLabel" Visible="false" /></div>
                <asp:Panel ID="AddPluginPanel" Visible="false" runat="server">                 
                    <br />
	                <asp:FileUpload ID="PluginFileUpload" CssClass="formInput W450" runat="server" />
                    <br />
                    <app:PromptButton ID="AddPluginSaveButton" Prompt="This may overwrite an existing plug-in if the plug-in folder already exists. Do you wish to continue?" CssClass="button" Text="save" OnClick="AddPluginSaveButton_Click" runat="server" />
                    <asp:Button ID="AddPluginCancelButton" CssClass="button" Text="cancel" OnClick="AddPluginCancelButton_Click" runat="server" />
                </asp:Panel>
			</div>
		</div>
		<div class="Right AdmRightPanel"></div>
		<div class="AdmTblPanel Clear">
            <br />
            <div class="Left AppCtrlMrg">
                 <span class="BodyTxt W80">Registered</span>
                 <asp:DropDownList ID="PluginRegisteredDropDownList" CssClass="formInput W60" AutoPostBack="true" OnSelectedIndexChanged="PluginFilters_SelectedIndexChanged" runat="server" >
                      <asp:ListItem Text="All" Value="-1" Selected="True" />
                      <asp:ListItem Text="Yes" Value="1" />
                      <asp:ListItem Text="No" Value="0" />
                </asp:DropDownList> &nbsp;
                 <span class="BodyTxt W80">Active</span>
                 <asp:DropDownList ID="PluginActiveDropDownList" CssClass="formInput W60" AutoPostBack="true" OnSelectedIndexChanged="PluginFilters_SelectedIndexChanged" runat="server" >
                      <asp:ListItem Text="All" Value="-1" Selected="True" />
                      <asp:ListItem Text="Yes" Value="1" />
                      <asp:ListItem Text="No" Value="0" />
                </asp:DropDownList>&nbsp;
                <span class="BodyTxt W80">Status</span>
				<asp:DropDownList ID="PluginStatusDropDownList" CssClass="formInput W200" AutoPostBack="true" OnSelectedIndexChanged="PluginFilters_SelectedIndexChanged" runat="server" >
                </asp:DropDownList>         
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
				<app:SortableRepeater ID="PluginsRepeater" DefaultSortExpression="Name" DefaultSortAscending="true" ColumnCount="7" OnItemDataBound="PluginsRepeater_ItemDataBound" runat="server">
					<HeaderTemplate>
						<table border="0" cellspacing="0" cellpadding="2" class="TblOutline AppTblMrg AutoStripe">
						<tr>
							<td width="120" id="HeaderCell1" runat="server" class="TblCell1 Bold"><asp:LinkButton ID="LinkButton1" CssClass="BodyTxt Bold" Text="Plug-in Name" OnCommand="SortListLinkButton_Command" CommandArgument="Name" runat="server" /></td>
							<td width="120" id="HeaderCell2" runat="server" class="TblCell1 Bold"><asp:LinkButton ID="LinkButton2" CssClass="BodyTxt Bold" Text="Folder" OnCommand="SortListLinkButton_Command" CommandArgument="Folder" runat="server" /></td>
							<td width="80" id="HeaderCell3" runat="server" class="TblCell1 Bold"><asp:LinkButton ID="LinkButton3" CssClass="BodyTxt Bold" Text="No. Contexts" OnCommand="SortListLinkButton_Command" CommandArgument="Contexts" runat="server" /></td>
                            <td width="60" id="HeaderCell4" runat="server" class="TblCell1 Bold"><asp:LinkButton ID="LinkButton4" CssClass="BodyTxt Bold" Text="Registered" OnCommand="SortListLinkButton_Command" CommandArgument="Registered" runat="server" /></td>
							<td width="40" id="HeaderCell5" runat="server" class="TblCell1 Bold"><asp:LinkButton ID="LinkButton5" CssClass="BodyTxt Bold" Text="Active" OnCommand="SortListLinkButton_Command" CommandArgument="Active" runat="server" /></td>
                            <td width="80" id="HeaderCell6" runat="server" class="TblCell1 Bold"><asp:LinkButton ID="LinkButton6" CssClass="BodyTxt Bold" Text="Status" OnCommand="SortListLinkButton_Command" CommandArgument="Status" runat="server" /></td>
                            <td width="60" id="HeaderCell7" runat="server" class="TblCell1 Bold"><asp:LinkButton ID="LinkButton7" CssClass="BodyTxt Bold" Text="Version" OnCommand="SortListLinkButton_Command" CommandArgument="Version" runat="server" /></td>
                            <td width="120" id="HeaderCell8" runat="server" class="TblCell1 Bold"><asp:LinkButton ID="LinkButton8" CssClass="BodyTxt Bold" Text="GUID" OnCommand="SortListLinkButton_Command" CommandArgument="Guid" runat="server" /></td>
							<td width="50" class="TblCell1 Bold">&nbsp;</td>
						</tr>
					</HeaderTemplate>
					<ItemTemplate>
						<tr style="height:30px;font-size:11px;" runat="server" id="itemRow">
							<td><asp:Literal ID="PluginName" runat="server" /></td>
                            <td><asp:Literal ID="Folder" runat="server" /></td>
                            <td class="alignCentre"><asp:Literal ID="Contexts" runat="server" /></td>
                            <td class="alignCentre"><asp:Literal ID="Registered" runat="server" /></td>
                            <td class="alignCentre"><asp:Literal ID="Active" runat="server" /></td>
                            <td><asp:Label ID="Status" runat="server" /></td>
                            <td class="alignCentre"><asp:Literal ID="Version" runat="server" /></td>
                            <td><asp:Literal ID="Guid" runat="server" /></td>
							<td class="alignCentre"><a href="ManagePluginsForm.aspx?PluginId=<%# DataBinder.Eval (Container.DataItem, "PluginId") %>" class="BodyTxt">edit</a></td>
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
			<div class="AppResults PanelTxt">total results: <span class="Bold"><%=RecordCount%></span> preview plug-ins [displaying page <%=CurrentPage%> of <%=TotalPages%>]</div>
		</div>
		<div class="Right">
			<div class="AppPageView AppPaging">
				<app:SimplePager ID="BottomPager" OnCommand="SimplePager_OnCommand" runat="server" />
			</div>
		</div>
	</div>



</asp:Content>
