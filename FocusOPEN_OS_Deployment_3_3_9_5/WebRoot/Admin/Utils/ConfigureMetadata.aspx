<%@Page Title="Configure Metadata [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="ConfigureMetadata.aspx.cs" Inherits="FocusOPEN.Website.Admin.Utils.ConfigureMetadata" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg"> 
		<div class="Left AdmLeftPanel"> 
			<div class="AdmAppHdr"><%= PageHeaderTitle %></div>
			<br />
		</div>

		<div class="Right AdmRightPanel"></div>
		<div class="AdmTblPanel Clear">
			<div class="Left">
				<div class="AppResults PanelTxt">total results: <span class="Bold"><%=RecordCount%></span> brands [displaying page <%=CurrentPage%> of <%=TotalPages%>]</div>
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
				<app:SortableRepeater ID="BrandsRepeater" DefaultSortExpression="Name" DefaultSortAscending="true" ColumnCount="1" OnItemDataBound="BrandsRepeater_ItemDataBound" runat="server">
					<HeaderTemplate>
						<table width="910" border="0" cellspacing="0" cellpadding="2" class="TblOutline AppTblMrg AutoStripe">
						<tr>
							<td id="HeaderCell1" runat="server" class="TblCell1 Bold"><asp:LinkButton ID="LinkButton1" CssClass="BodyTxt Bold" Text="Brand" OnCommand="SortListLinkButton_Command" CommandArgument="Name" runat="server" /></td>
							<td class="Bold">&nbsp;</td>
						</tr>
					</HeaderTemplate>
					<ItemTemplate>
						<tr> 
							<td><asp:HyperLink ID="BrandNameHyperLink" CssClass="BodyTxt" runat="server" /></td>
							<td class="W75 alignCentre"><asp:HyperLink ID="BrandEditHyperLink" CssClass="BodyTxt" Text="edit" runat="server" /></td>
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
			<div class="AppResults PanelTxt">total results: <span class="Bold"><%=RecordCount%></span> brands [displaying page <%=CurrentPage%> of <%=TotalPages%>]</div>
		</div>
		<div class="Right">
			<div class="AppPageView AppPaging">
				<app:SimplePager ID="BottomPager" OnCommand="SimplePager_OnCommand" runat="server" />
			</div>
		</div>
	</div>
</asp:Content>