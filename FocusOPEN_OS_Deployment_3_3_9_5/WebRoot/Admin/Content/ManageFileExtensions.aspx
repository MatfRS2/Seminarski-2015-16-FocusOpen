<%@ Page Title="Manage File Extensions [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="ManageFileExtensions.aspx.cs" Inherits="FocusOPEN.Website.Admin.Content.ManageFileExtensions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg"> 
		<div class="Left"> 
			<div class="AdmAppHdr">Manage File Extensions </div>
			<app:FeedbackLabel ID="FeedbackLabel" UseContainer="true" runat="server" />
			<br />
			<div class="AppCtrlMrg AppCtrlPanel W470">
				<span class="BodyTxt">Choose an asset type to filter by and pick an extension, or <a href="ManageFileExtensionsForm.aspx" class="Black">add a new extension here</a>
				|
				<app:HelpHyperLink HelpPage="9_Manage_Content.htm#manage_fileextensions" CssClass="BodyTxt" Text="help" runat="server" />
				</span>
				<br /><br />
				<app:AssetTypeDropDownList ID="AssetTypeDropDownList1" CssClass="formInput W200" BlankText="All" BlankValue="0" AutoPostBack="true" OnSelectedIndexChanged="AssetTypeDropDownList1_SelectedIndexChanged" runat="server" />
			</div>
		</div>
		<div class="Right AdmRightPanel"></div>
		<div class="AdmTblPanel Clear">
			<div class="Left">
				<br /><br />
				<div class="AppResults PanelTxt">total results: <span class="Bold"><%=RecordCount%></span> file extensions [displaying page <%=CurrentPage%> of <%=TotalPages%>]</div>
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
				<app:SortableRepeater ID="AssetTypeFileExtensionsRepeater" DefaultSortExpression="Extension" DefaultSortAscending="true" ColumnCount="3" OnItemDataBound="AssetTypeFileExtensionsRepeater_ItemDataBound" runat="server">
					<HeaderTemplate>
						<table border="0" cellspacing="0" cellpadding="2" class="TblOutline AppTblMrg AutoStripe">
						<tr>
							<td class="TblCell1 Bold" width="20">&nbsp;</td>
							<td width="90" id="HeaderCell1" runat="server" class="TblCell1 Bold"><asp:LinkButton ID="LinkButton1" CssClass="BodyTxt Bold" Text="Extension" OnCommand="SortListLinkButton_Command" CommandArgument="Extension" runat="server" /></td>
							<td width="120" id="HeaderCell2" runat="server" class="TblCell1 Bold"><asp:LinkButton ID="LinkButton2" CssClass="BodyTxt Bold" Text="Name" OnCommand="SortListLinkButton_Command" CommandArgument="Name" runat="server" /></td>
							<td width="120" id="HeaderCell3" runat="server" class="TblCell1 Bold"><asp:LinkButton ID="LinkButton3" CssClass="BodyTxt Bold" Text="Asset Type" OnCommand="SortListLinkButton_Command" CommandArgument="AssetTypeName" runat="server" /></td>
							<td width="50" class="TblCell1 BodyTxt Bold">APS</td>
							<td width="50" class="TblCell1 Bold">&nbsp;</td>
						</tr>
					</HeaderTemplate>
					<ItemTemplate>
						<tr style="height:30px">
							<td><asp:Image ID="IconImage" runat="server" /></td>
							<td><a href="ManageFileExtensionsForm.aspx?AssetTypeFileExtensionId=<%# DataBinder.Eval (Container.DataItem, "AssetTypeFileExtensionId") %>" class="BodyTxt"><%# DataBinder.Eval (Container.DataItem, "Extension") %></a></td>
							<td><a href="ManageFileExtensionsForm.aspx?AssetTypeFileExtensionId=<%# DataBinder.Eval (Container.DataItem, "AssetTypeFileExtensionId") %>" class="BodyTxt"><%# DataBinder.Eval (Container.DataItem, "Name") %></a></td>
							<td class="BodyTxt"><%# DataBinder.Eval (Container.DataItem, "AssetTypeName") %></td>
							<td class="BodyTxt"><asp:Literal ID="APSSupportedLiteral" runat="server" /></td>
							<td class="alignCentre"><a href="ManageFileExtensionsForm.aspx?AssetTypeFileExtensionId=<%# DataBinder.Eval (Container.DataItem, "AssetTypeFileExtensionId") %>" class="BodyTxt">edit</a></td>
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
			<div class="AppResults PanelTxt">total results: <span class="Bold"><%=RecordCount%></span> file extensions [displaying page <%=CurrentPage%> of <%=TotalPages%>]</div>
		</div>
		<div class="Right">
			<div class="AppPageView AppPaging">
				<app:SimplePager ID="BottomPager" OnCommand="SimplePager_OnCommand" runat="server" />
			</div>
		</div>
	</div>
</asp:Content>