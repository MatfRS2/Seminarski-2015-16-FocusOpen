<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssetList.ascx.cs" Inherits="FocusOPEN.Website.Controls.Admin.AssetList" %>

<script type="text/javascript">
	$(function() {

		var imageWidth = 75;

		$(".assetThumbnail").each(function() {
			$(this).load(function() {
			if ($(this).width() > imageWidth)
				$(this).attr("style", "width:" + imageWidth + "px");
			});
		});
	});
</script>

<app:SortableRepeater ID="AssetsRepeater" OnItemDataBound="AssetsRepeater_OnItemDataBound" DefaultSortExpression="AssetId" DefaultSortAscending="false" ColumnCount="9" runat="server">
	<HeaderTemplate>
		<table width="910" border="0" cellspacing="0" cellpadding="2" class="TblOutline AppTblMrg AutoStripe">
		<tr>
			<td id="CheckBoxHeaderCell" runat="server" class="TblCell1 Bold">&nbsp;</td>
			<td id="ThumbnailHeaderCell" runat="server" class="TblCell1 Bold">&nbsp;</td>
			<td id="HeaderCell1" runat="server" nowrap="nowrap" class="Bold"><asp:LinkButton ID="LinkButton1" CssClass="BodyTxt Bold" Text="Ref #" OnCommand="SortListLinkButton_Command" CommandArgument="AssetId" runat="server" /></td>
			<td id="HeaderCell2" runat="server" nowrap="nowrap" class="Bold alignCentre"><asp:LinkButton ID="LinkButton2" CssClass="BodyTxt Bold" Text="Type" OnCommand="SortListLinkButton_Command" CommandArgument="AssetTypeName" runat="server" /></td>
			<td id="HeaderCell3" runat="server" nowrap="nowrap" class="Bold"><asp:LinkButton ID="LinkButton3" CssClass="BodyTxt Bold" Text="Upload User" OnCommand="SortListLinkButton_Command" CommandArgument="UploadedByUserName" runat="server" /></td>
			<td id="HeaderCell4" runat="server" nowrap="nowrap" class="Bold"><asp:LinkButton ID="LinkButton4" CssClass="BodyTxt Bold" Text="Title" OnCommand="SortListLinkButton_Command" CommandArgument="Title" runat="server" /></td>
			<td id="HeaderCell5" runat="server" nowrap="nowrap" class="Bold alignCentre"><asp:LinkButton ID="LinkButton5" CssClass="BodyTxt Bold" Text="File Size" OnCommand="SortListLinkButton_Command" CommandArgument="FileSize" runat="server" /></td>
			<td id="HeaderCell6" runat="server" nowrap="nowrap" class="Bold alignCentre"><asp:LinkButton ID="LinkButton6" CssClass="BodyTxt Bold" Text="Produced" OnCommand="SortListLinkButton_Command" CommandArgument="BASE.ShadowDate" runat="server" /></td>
			<td id="HeaderCell7" runat="server" nowrap="nowrap" class="Bold alignCentre"><asp:LinkButton ID="LinkButton7" CssClass="BodyTxt Bold" Text="Expired" OnCommand="SortListLinkButton_Command" CommandArgument="dbo.IsExpired(BASE.ExpiryDate)" runat="server" /></td>
			<td id="HeaderCell8" runat="server" nowrap="nowrap" class="Bold alignCentre"><asp:LinkButton ID="LinkButton8" CssClass="BodyTxt Bold" Text="Expiry Date" OnCommand="SortListLinkButton_Command" CommandArgument="ExpiryDate" runat="server" /></td>
			<td id="HeaderCell9" runat="server" nowrap="nowrap" class="Bold alignCentre"><asp:LinkButton ID="LinkButton9" CssClass="BodyTxt Bold" Text="Published" OnCommand="SortListLinkButton_Command" CommandArgument="AssetPublishStatusName" runat="server" /></td>
			<td class="Bold">&nbsp;</td>
		</tr>
	</HeaderTemplate>
	<ItemTemplate>
		<asp:HiddenField ID="AssetIdHiddenField" runat="server" />
		<tr>
			<td id="CheckBoxCell" runat="server" nowrap="nowrap" class="TblCell2"><asp:CheckBox ID="AssetCheckBox" runat="server" /></td>
			<td id="ThumbnailCell" runat="server" nowrap="nowrap" class="TblCell2 alignCentre"><app:AssetThumbnail ID="AssetThumbnail" CssClass="assetThumbnail" HighlightIfRestricted="false" ShowInfoOnHover="false" runat="server" /></td>
			<td><asp:HyperLink ID="AssetIdHyperLink" CssClass="BodyTxt" runat="server" /></td>
			<td class="BodyTxt alignCentre"><asp:Label ID="AssetTypeLabel" runat="server" /></td>
			<td nowrap="nowrap" class="BodyTxt"><asp:Label ID="UploadedByUserNameLabel" runat="server" /></td>
			<td class="BodyTxt"><asp:Label ID="TitleLabel" runat="server" /></td>
			<td class="alignCentre BodyTxt"><asp:Label ID="FileSizeLabel" runat="server" /></td>
			<td class="BodyTxt alignCentre"><asp:Label ID="ProductionDateLabel" runat="server" /></td>
			<td class="alignCentre BodyTxt"><asp:Label ID="IsExpiredLabel" runat="server" /></td>
			<td nowrap="nowrap" class="alignCentre BodyTxt"><asp:Label ID="ExpiryDateLabel" runat="server" /></td>
			<td class="BodyTxt alignCentre"><asp:HyperLink ID="IsPublishedHyperLink" runat="server" /></td>
			<td nowrap="nowrap">
				<a href="../Reports/AssetStats.aspx?assetId=<%# DataBinder.Eval(Container.DataItem, "AssetId") %>" class="BodyTxt">stats</a>
				<span class="BodyTxt">|</span>
				<a href="AssetForm.aspx?assetId=<%# DataBinder.Eval(Container.DataItem, "AssetId") %>" class="BodyTxt">edit</a>
				<span class="BodyTxt">|</span>
				<app:PromptLinkButton ID="DeleteLinkButton" class="BodyTxt" Prompt="Are you sure you want to delete this asset? This cannot be undone" Text="delete" OnCommand="DeleteLinkButton_OnCommand" runat="server" />
				<app:EnhancedHyperLink ID="AuditLogHyperLink" CssClass="BodyTxt" ShowLeftDivider="true" Text="log" runat="server" />
			</td>
		</tr>
	</ItemTemplate>
	<FooterTemplate>
		</table>
	</FooterTemplate>
</app:SortableRepeater>