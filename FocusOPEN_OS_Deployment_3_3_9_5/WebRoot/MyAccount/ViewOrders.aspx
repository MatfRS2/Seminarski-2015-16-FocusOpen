<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewOrders.aspx.cs" Inherits="FocusOPEN.Website.MyAccount.ViewOrders" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title>View Orders [<%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().ApplicationName%>]</title>
	<app:BrandHeader ID="BrandHeader1" Css="Basic|Styles|Other" runat="server" />
	<style type="text/css">
		a.button { padding:2px 7px 1px; text-decoration:none; }
	</style>
	<script src="../Includes/Javascript/jQuery/jquery-1.4.1.min.js" type="text/javascript"></script>
	<script src="../Includes/Javascript/jQuery/jquery-ui-1.7.2.custom.min.js" type="text/javascript"></script>
	<script src="../Includes/Javascript/Global.js" type="text/javascript"></script>
	<link rel="Stylesheet" type="text/css" href="../Includes/Css/jQuery-UI/smoothness/jquery-ui-1.7.2.custom.css" />
</head>
<body>
<form id="form1" runat="server">
	<aspajax:ScriptManager ID="ScriptManager1" runat="server" />
	<script type="text/javascript">
		function downloadAsset(assetId, orderId, orderItemId, formatDropDownId, imageSizeDropDownId)
		{
			var dd1 = $get(formatDropDownId);
			var dd2 = $get(imageSizeDropDownId);
			
			var url = "../Assets/AssetFile." + assetId + ".ashx?OrderId=" + orderId + "&OrderItemId=" + orderItemId;
			
			if (dd1 && dd2)
			{
				var formatId = dd1.options[dd1.options.selectedIndex].value;
				var imageSizeId = dd2.options[dd2.options.selectedIndex].value;
				url += "&AssetImageFormat=" + formatId + "&AssetImageSizeId=" + imageSizeId
			}
			
			window.location.href = url;
		}
	</script>
	
	<div class="CatWrapper">
		<app:PageHeader ID="PageHeader1" runat="server" />
		<div class="CatBkg">
			<div class="DManAppPanel">
				<div class="DMHdr">
					<div class="Left">
						<br />
						<!--start text header//--><span class="CatAppHdr">MY ORDERS</span>&nbsp;<!--end text header//-->
						<span class="PanelTxt BaseLine">current order: <span class="Bold">Ref <asp:Label ID="OrderIdLabel" runat="server" /></span> date: <span class="Bold"><asp:Label ID="OrderDateLabel" runat="server" /></span></span><br />
					</div>
					<div class="Right">
						<br />
						<span class="PanelTxt">show</span>
						<app:PageSizeDropDownList ID="PageSizeDropDownList1" OmitBlankItem="true" AutoPostBack="true" OnSelectedIndexChanged="PageSizeDropDownList1_SelectedIndexChanged" CssClass="formInput" runat="server" />
					</div>
				</div>
				<div class="Clear"><br /></div>
				<div class="DMHdrSelector">
					<asp:Panel ID="OrderPanelsWrapperPanel" runat="server">
						<asp:Panel ID="LimitedOrdersPanel" runat="server">
							<span class="PanelTxt Bold">view last 10 orders</span> 
							<app:UserOrderDropDownList ID="UserOrderDropDownList1" AutoPostBack="true" OnSelectedIndexChanged="UserOrder_SelectedIndexChanged" OmitBlankItem="true" CssClass="formInput" runat="server" />
							<asp:LinkButton ID="ViewAllOrdersLinkButton" CssClass="BodyTxt" Text="view all orders" OnClick="ViewAllOrdersLinkButton_Click" runat="server" />
							<span class="InlineHelpTxt">
								<span class="PanelTxt">|</span>
								<app:HelpHyperLink ID="HelpHyperLink1" HelpPage="5_Download_Manager.htm#view_orders" CssClass="BodyTxt" Text="help" runat="server" />
							</span>
						</asp:Panel>
						<asp:Panel ID="AllOrdersPanel" Visible="false" runat="server">
							<span class="PanelTxt Bold Top">view all orders</span>
							<app:UserOrderListBox ID="UserOrderListBox1" AutoPostBack="true" OnSelectedIndexChanged="UserOrder_SelectedIndexChanged" CssClass="formInput" Rows="8" SelectionMode="single" runat="server" />
							<asp:LinkButton ID="ViewLimitedOrdersLinkButton" CssClass="BodyTxt" Text="view last 10 orders" OnClick="ViewLimitedOrdersLinkButton_Click" runat="server" />
							<span class="InlineHelpTxt">
								<span class="PanelTxt">|</span>
								<app:HelpHyperLink ID="HelpHyperLink2" HelpPage="5_Download_Manager.htm#view_orders" CssClass="BodyTxt" Text="help" runat="server" />
							</span>
						</asp:Panel>
					</asp:Panel>
					<br />
				</div>
				<div class="Clear"></div> 
				<div class="DMHdrFtr">
					<div class="Left"> <span class="PanelTxt H20">total in this order: <span class="Bold"><%=AssetCount%></span> assets [displaying page <%=CurrentPage%> of <%=TotalPages%>]</span></div>
					<div class="Right"><app:SimplePager ID="TopPager" OnCommand="Page_IndexChanged" runat="server" /></div>
					<div class="Clear"></div>
				</div>
			</div>
			<asp:Panel ID="OrderDetailsPanel" runat="server">
				<div class="OrdHdr">
					<div class="Left">
						<br />
						<app:BackToSearchHyperlink ID="BackToSearchHyperLink1" runat="server" />
					</div>
					<div class="Right"> 
						<br />
						<span class="BodyTxt Bold">select all assets</span>
						<asp:CheckBox ID="SelectAllAssets1" OnCheckedChanged="SelectAllAssets_CheckChanged" AutoPostBack="true" runat="server" />	
						<span class="BodyTxt">[</span><asp:LinkButton ID="DeselectAllLinkButton1" CssClass="BodyTxt" Text="deselect all" OnClick="DeselectAllLinkButton_Click" runat="server" />
						<span class="BodyTxt">|</span>
						<asp:LinkButton ID="DownloadAsFtpLinkButton1" OnClick="DownloadAsFtpLinkButton_Click" CssClass="BodyTxt" runat="server"><app:IconImage ID="FtpIconImage1" Filename="ftp.gif" AlternateText="[ftp]" runat="server"/>&nbsp;transfer to ftp</asp:LinkButton>
						<span class="BodyTxt">|</span>
						<asp:LinkButton ID="DownloadAsZipLinkButton1" OnClick="DownloadAsZipLinkButton_Click" CssClass="BodyTxt" runat="server"><app:IconImage ID="ZipIconImage1" Filename="zip.gif" AlternateText="[zip]" runat="server"/>&nbsp;download as zip</asp:LinkButton><span class="BodyTxt">]</span>
					</div>
				</div>
				<div class="Clear"></div>
				<app:FeedbackLabel ID="FeedbackLabel1" runat="server" />
				<br />
				<asp:Repeater ID="OrderItemsRepeater" OnItemDataBound="OrderItemsRepeater_ItemDataBound" runat="server">
					<ItemTemplate>
						
						<asp:HiddenField ID="OrderItemIdHiddenField" runat="server" />
						<asp:HiddenField ID="AssetIdHiddenField" runat="server" />
						
						<div class="OrdPanel">
							<div class="Left OrdThbPanel">
								<app:AssetThumbnail ID="AssetThumbnail1" UseMarginCssClass="false" runat="server" />
								<br />
								<div class="ImgIcons">
									<app:AssetButtons ID="AssetButtons1" ShowButtons="Lightbox,Cart,Info" runat="server" />
								</div>
							</div>
							<div class="Left OrdTblPanel OrdThbMrg">
								<table width="430" border="0" cellspacing="0" cellpadding="2" class="TblOutline">
								<tr> 
									<td class="TblCell2 BodyTxt Bold alignRight Top W125">Reference Number</td>
									<td class="TblCell1 BodyTxt Top" id="AssetReferenceCell" runat="server">[AssetId]</td>
								</tr>
								<tr> 
									<td class="TblCell2 BodyTxt Bold alignRight Top">Short Description</td>
									<td class="TblCell1 BodyTxt Top" id="DescriptionCell" runat="server">[Description]</td>
								</tr>
								<app:MultiBrandPlaceHolder runat="server">
								<tr>
									<td class="TblCell2 BodyTxt Bold alignRight">Brand</td>
									<td class="TblCell1 BodyTxt Top" id="BrandCell" runat="server">[Brand]</td>
								</tr>
								</app:MultiBrandPlaceHolder>
								<tr> 
									<td class="TblCell2 BodyTxt Bold alignRight Top">Asset Type</td>
									<td class="TblCell1 BodyTxt Top" id="AssetTypeCell" runat="server">[AssetType]</td>
								</tr>
								<tr> 
									<td class="TblCell2 BodyTxt Bold alignRight Top">Usage Restrictions</td>
									<td class="TblCell1 BodyTxt Top" id="UsageRestrictionsCell" runat="server">[Usage restrictions]</td>
								</tr>
								
								<tr id="FileSizeRow" visible="false" runat="server"> 
									<td class="TblCell2 BodyTxt Bold alignRight Top">File Size</td>
									<td class="TblCell1 BodyTxt Top"><app:FileSizeMessageLabel ID="FileSizeLabel" ShowIcon="false" TextPrefix="" runat="server" /></td>
								</tr>
								
								<asp:PlaceHolder ID="ImageOptionsPlaceHolder" Visible="false" runat="server">
									<tr>
										<td class="TblCell2 BodyTxt Bold alignRight Top">Download Format</td>
										<td class="TblCell1 BodyTxt Top">
											<app:DownloadFormatDropDownList ID="DownloadFormatDropDownList1" CssClass="formInput W150" AutoPostBack="true" OnSelectedIndexChanged="DownloadFormatDropDownList1_SelectedIndexChanged" runat="server"/>
										</td>
									</tr>
									<tr> 
										<td class="TblCell2 BodyTxt Bold alignRight Top">Download Resolution</td>
										<td class="TblCell1 BodyTxt Top">
											<app:AssetImageSizeDropDownList ID="AssetImageSizeDropDownList1" AutoPostBack="true" OnSelectedIndexChanged="AssetImageSizeDropDownList1_SelectedIndexChanged" OmitBlankItem="true" CssClass="formInput" runat="server" />
											<app:FileSizeMessageLabel ID="ImageFileSizeMessageLabel" Visible="false" runat="server" />
										</td>
									</tr>
								</asp:PlaceHolder>
								
								</table>
								
								<aspajax:UpdatePanel ID="UpdatePanel2" ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server">
									<ContentTemplate>
										<div id="LinkButtonWrapper" runat="server" class="DMViewSelector">
											<asp:LinkButton ID="ToggleApprovalDetailsLinkButton" OnClick="ToggleApprovalDetailsLinkButton_Click" CssClass="BodyTxt" Text="show approval details [+]" runat="server" />
											<span class="BodyTxt"> |</span>
											<app:HelpHyperLink ID="HelpHyperLink3" HelpPage="5_Download_Manager.htm#asset_status" CssClass="BodyTxt" Text="help" runat="server" />
										</div>
										<asp:Panel ID="ApprovalDetailsPanel" Visible="false" runat="server">
											<table width="430" border="0" cellspacing="0" cellpadding="2" class="TblOutline">
											<tr> 
												<td class="TblCell2 BodyTxt Bold alignRight Top W125">Date Requested By </td>
												<td class="TblCell1 BodyTxt Top" id="DateRequestedByCell" runat="server">[Date requested by]</td>
											</tr>
											<tr> 
												<td class="TblCell2 BodyTxt Bold alignRight Top">Date Of Decision</td>
												<td class="TblCell1 BodyTxt Top" id="DateOfDecisionCell" runat="server">[Date of decision]</td>
											</tr>
											<tr> 
												<td class="TblCell2 BodyTxt Bold alignRight Top">Your Notes</td>
												<td class="TblCell1 BodyTxt Top" id="NotesCell" runat="server">[Notes]</td>
											</tr>
											<tr> 
												<td class="TblCell2 BodyTxt Bold alignRight Top">Asset Contact</td>
												<td class="TblCell1 Top"><app:EmailHyperLink ID="AssetContactHyperlink" CssClass="BodyTxt" runat="server" /></td>
											</tr>
											<tr id="ConversationRow" runat="server"> 
												<td class="TblCell2 BodyTxt Bold alignRight Top">Conversation</td>
												<td class="TblCell1 BodyTxt Top">
													<asp:Repeater ID="ConversationRepeater" OnItemDataBound="ConversationRepeater_ItemDataBound" runat="server">
														<ItemTemplate>
															<span class="PosTxt">
																<asp:Label ID="UserNameLabel" CssClass="BodyTxt PosTxt" runat="server" /> said on <asp:Label ID="DateLabel" CssClass="BodyTxt PosTxt" runat="server" />:<br />
																<asp:Label ID="CommentTextLabel" CssClass="BodyTxt PosTxt" runat="server" />
															</span>
														</ItemTemplate>
														<AlternatingItemTemplate>
															<asp:Label ID="UserNameLabel" CssClass="BodyTxt" runat="server" /> said on <asp:Label ID="DateLabel" CssClass="BodyTxt" runat="server" />:<br />
															<asp:Label ID="CommentTextLabel" CssClass="BodyTxt" runat="server" />
														</AlternatingItemTemplate>
														<SeparatorTemplate>
															<br />
															<br />
														</SeparatorTemplate>
													</asp:Repeater>
												</td>
											</tr>
											<tr id="AddNotesRow" runat="server"> 
												<td class="TblCell2 BodyTxt alignRight Top"><span class="Bold">Add comment</span><br />Is visible to you and the administrator assigned to approve this request</td>
												<td class="TblCell1 BodyTxt Top">
													<app:TextArea ID="CommentsTextBox" Columns="25" Rows="5" CssClass="formInput W300" MaxLength="500" runat="server" />
													<div class="alignRight">
														<asp:Button ID="SaveNotesButton" Text="Save" CssClass="button" OnClick="SaveNotesButton_Click" runat="server" />
													</div>
												</td>
											</tr>
											</table>
											<br />
										</asp:Panel>
									</ContentTemplate>
									<Triggers>
										<aspajax:AsyncPostBackTrigger ControlID="ToggleApprovalDetailsLinkButton" />
										<aspajax:AsyncPostBackTrigger ControlID="SaveNotesButton" />
									</Triggers>
								</aspajax:UpdatePanel>
								
								<div class="alignRight" id="SelectorContainer" runat="server">
									<br />
									<app:PromptButton ID="CopyDownloadSettingsToAllButton" OnClick="CopyDownloadSettingsToAllButton_Click" Prompt="This will copy the selected download settings to all other assets of the same type in this order.\n\nAre you sure you want to continue?" CssClass="button" Text="copy download settings" runat="server" />
									&nbsp;
									<asp:HyperLink ID="DownloadHyperlink" CssClass="button" Text="download" runat="server" /><br />
									<br />
									<span class="BodyTxt Bold">select this asset</span>
									<span><asp:CheckBox ID="SelectAssetCheckBox" OnCheckedChanged="SelectAssetCheckBox_CheckChanged" AutoPostBack="true" runat="server" /></span>
									<br />
									<br />
								</div>
								
							</div>
							<div class="Left OrdAppPanel">
								<div class="OrdFbkPanel"> 
									<app:FeedbackLabel ID="OrderItemMessageLabel" TextPrefix="" Pinned="true" runat="server" />
								</div>
							</div>
						</div>
						<div class="Clear"></div>
						<div class="OrdResultsFtr"></div>
						<br />
					</ItemTemplate>
				</asp:Repeater>
				<div class="OrdFtr">
					<div class="Left">
						<app:BackToSearchHyperlink ID="BackToSearchHyperlink2" runat="server" />
					</div>
					<div class="Right"> 
						<span class="BodyTxt Bold">select all assets</span>
						<asp:CheckBox ID="SelectAllAssets2" OnCheckedChanged="SelectAllAssets_CheckChanged" AutoPostBack="true" runat="server" />
						<span class="BodyTxt">[</span><asp:LinkButton ID="DeselectAllLinkButton2" CssClass="BodyTxt" Text="deselect all" OnClick="DeselectAllLinkButton_Click" runat="server" />
						<span class="BodyTxt">|</span>
						<asp:LinkButton ID="DownloadAsFtpLinkButton2" OnClick="DownloadAsFtpLinkButton_Click" CssClass="BodyTxt" runat="server"><app:IconImage ID="FtpIconImage2" Filename="ftp.gif" AlternateText="[ftp]" runat="server"/>&nbsp;transfer to ftp</asp:LinkButton>
						<span class="BodyTxt">|</span>
						<asp:LinkButton ID="DownloadAsZipLinkButton2" OnClick="DownloadAsZipLinkButton_Click" CssClass="BodyTxt" runat="server"><app:IconImage ID="ZipIconImage2" Filename="zip.gif" AlternateText="[zip]" runat="server"/>&nbsp;download as zip</asp:LinkButton><span class="BodyTxt">]</span>
					</div>
				</div>
			</asp:Panel>
			<asp:Panel ID="EmptyOrderPanel" Visible="false" runat="server">
				<div style="padding:10px">
					<app:FeedbackLabel ID="EmptyOrderFeedbackLabel" Pinned="true" Header="All assets in this order have been deleted" MessageType="Negative" runat="server" />
				</div>
			</asp:Panel>
			<div class="Clear"></div>
			<br />
			<div class="CatAppFtr">
				<div class="Left">
					<div class="AppResults PanelTxt">total in this order: <span class="Bold"><%=AssetCount%></span> assets [displaying page <%=CurrentPage%> of <%=TotalPages%>]</div>
				</div>
				<div class="Right">
					<div class="AppPageView AppPaging">
						<app:SimplePager ID="BottomPager" OnCommand="Page_IndexChanged" runat="server" />
					</div>
				</div>
			</div>
		</div>
		<app:PageFooter ID="PageFooter1" HelpPage="index.htm#catalogue" runat="server" />
	</div>
</form>
</body>
</html>