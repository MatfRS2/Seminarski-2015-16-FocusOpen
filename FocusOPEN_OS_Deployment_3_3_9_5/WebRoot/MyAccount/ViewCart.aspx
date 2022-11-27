<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="ViewCart.aspx.cs" Inherits="FocusOPEN.Website.MyAccount.ViewCart" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title>Cart [<%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().ApplicationName%>]</title>
	<app:BrandHeader ID="BrandHeader1" Css="Basic|Styles|Other" runat="server" />
	<script src="../Includes/Javascript/jQuery/jquery-1.4.1.min.js" type="text/javascript"></script>
	<script src="../Includes/Javascript/jQuery/jquery-ui-1.7.2.custom.min.js" type="text/javascript"></script>
	<script src="../Includes/Javascript/Global.js" type="text/javascript"></script>
	<link rel="Stylesheet" type="text/css" href="../Includes/Css/jQuery-UI/smoothness/jquery-ui-1.7.2.custom.css" />
</head>
<body>
	<form id="form1" runat="server">
		<aspajax:ScriptManager ID="ScriptManager1" runat="server" />
		<script type="text/javascript">
			Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(pageLoaded);
			function pageLoaded() {
        		setupDatePicker();
			}
		</script>
		<div class="CatWrapper">
			<app:PageHeader ID="PageHeader1" runat="server" />
			<div class="CatBkg">
				<div class="CatAppPanel">
					<div class="Left"> 
						<div class="AppHdr">
							<!--start text header//--><span class="CatAppHdr">VIEW CART</span>&nbsp;<!--end text header//-->
						</div>
						<div class="AppResults PanelTxt">total in this order: <span class="Bold"><%=AssetCount%></span> assets [displaying page <%=CurrentPage%> of <%=TotalPages%>]</div>
					</div>
					<div class="Right">
						<div class="AppPageView">
							<span class="PanelTxt">show</span>
							<app:PageSizeDropDownList ID="PageSizeDropDownList1" OmitBlankItem="true" CssClass="formInput" AutoPostBack="true" OnSelectedIndexChanged="PageSize_SelectedIndexChanged" runat="server" />
							<div class="AppPaging">
								<app:SimplePager ID="TopPager" OnCommand="Page_IndexChanged" runat="server" />
							</div>
						</div>
					</div>
				</div>
				
				<asp:PlaceHolder ID="CartResultsPanel" runat="server">
					<div class="OrdHdr">
						<div class="Left">
							<br />
							<app:BackToSearchHyperlink ID="BackToSearchHyperlink1" runat="server" />
						</div>
						<div class="Right"> 
							<br />
							<span class="BodyTxt Bold">select all assets</span><asp:CheckBox ID="SelectAllAssetsInCart1" OnCheckedChanged="SelectAllAssetsInCart_CheckChanged" AutoPostBack="true" runat="server" />
							<span class="BodyTxt">[</span><asp:LinkButton ID="DeselectAllLinkButton1" CssClass="BodyTxt" Text="deselect all" OnClick="DeselectAllLinkButton_Click" runat="server" />
							<span class="BodyTxt"> |</span>
							<asp:LinkButton ID="RemoveSelectedAssetsLinkButton1" Text="remove selected" CssClass="BodyTxt" OnClick="RemoveSelectedAssetsLinkButton_Click" runat="server" /><span class="BodyTxt">]</span>
						</div>
					</div>
					<div class="Clear"></div>
					<div class="OrdHdr">
						<app:FeedbackLabel ID="FeedbackLabel1" runat="server" />
					</div>
					<br />
					<asp:Repeater ID="CartRepeater" OnItemDataBound="CartRepeater_ItemDataBound" runat="server">
						<ItemTemplate>
						
							<asp:HiddenField ID="AssetIdHiddenField" runat="server" />
							<asp:HiddenField ID="CartIdHiddenField" runat="server" />
						
							<div class="OrdPanel">
								<div class="Left OrdThbPanel">
									<app:AssetThumbnail ID="AssetThumbnail1" UseMarginCssClass="false" runat="server" /><br />
									<div class="ImgIcons"><app:AssetButtons ID="AssetButtons1" OnCartButtonClicked="AssetButtons1_CartButtonClicked" MessageLabelControlId="FeedbackLabel1" ShowButtons="Cart,Info,Lightbox" runat="server" /></div>
								</div>
								<div class="Left OrdTblPanel OrdThbMrg">
									<table width="430" border="0" cellspacing="0" cellpadding="2" class="TblOutline">
									<tr> 
										<td class="TblCell2 BodyTxt Bold alignRight Top W125">Reference Number</td>
										<td class="TblCell1 BodyTxt Top" id="AssetIdCell" runat="server">[AssetId]</td>
									</tr>
									<tr>
										<td class="TblCell2 BodyTxt Bold alignRight Top">Short Description</td>
										<td class="TblCell1 BodyTxt Top" id="AssetShortDescriptionCell" runat="server">[ShortDescription]</td>
									</tr>
									<app:MultiBrandPlaceHolder runat="server">
									<tr>
										<td class="TblCell2 BodyTxt Bold alignRight">Brand</td>
										<td class="TblCell1 BodyTxt Top" id="AssetBrandNameCell" runat="server">[Brand]</td>
									</tr>
									</app:MultiBrandPlaceHolder>
									<tr> 
										<td class="TblCell2 BodyTxt Bold alignRight Top">Asset Type</td>
										<td class="TblCell1 BodyTxt Top" id="AssetTypeCell" runat="server">[AssetType]</td>
									</tr>
									<tr> 
										<td class="TblCell2 BodyTxt Bold alignRight Top">Usage Restrictions</td>
										<td class="TblCell1 BodyTxt Top" id="UsageRestrictionsCell" runat="server">[UsageRestrictionsCell]</td>
									</tr>
									<tr>
										<td class="TblCell2 BodyTxt Bold alignRight Top">Asset Contact</td>
										<td class="TblCell1  Top"><app:EmailHyperLink ID="AssetContactEmailHyperLink" CssClass="BodyTxt" runat="server" /></td>
									</tr>
									<tr id="DateRequiredRow" runat="server">
										<td class="TblCell2 BodyTxt Bold alignRight Top">Date Required By</td>
										<td class="TblCell1 BodyTxt Top"><app:DatePicker ID="DateRequiredPicker" AllowDateBeforeToday="false" runat="server" /></td>
									</tr>
									<tr id="IntendedUseRow" runat="server">
										<td class="TblCell2 BodyTxt alignRight Top W125"><span class="Bold">Intended Use</span><br />Enter details on the<br />intended use of this<br />asset | <app:HelpHyperLink ID="HelpHyperLink1" HelpPage="4_Cart.htm#intended_use_notes" CssClass="BodyTxt" Text="help" runat="server" /></td>
										<td class="TblCell1 BodyTxt Top"><app:TextArea ID="NotesTextBox" Columns="25" Rows="4" CssClass="formInput W300" MaxLength="500" runat="server" /></td>
									</tr>
									</table>
									<div class="alignRight">
										<asp:PlaceHolder ID="SaveNotesButtonPlaceHolder" runat="server">
											<br />
											<asp:Button ID="CopyNotesToAllButton" Text="copy to all" CssClass="button W80" OnClick="CopyNotesToAllButton_Click" runat="server" />
											&nbsp;
											<asp:Button ID="SaveNotesButton" Text="save" CssClass="button W50" OnClick="SaveNotesButton_Click" runat="server" /><br />
										</asp:PlaceHolder>
										<br />
										<asp:PlaceHolder ID="SelectAssetCheckBoxWrapperPlaceHolder" runat="server">
											<span class="BodyTxt Bold">select this asset</span>
											<span><asp:CheckBox ID="SelectAssetCheckBox" OnCheckedChanged="SelectAssetCheckBox_CheckChanged" AutoPostBack="true" runat="server" /></span><br />
											<br />
										</asp:PlaceHolder>
									</div>
								</div>
								<div class="Left OrdAppPanel">
									<div class="OrdFbkPanel"><app:FeedbackLabel ID="CartItemMessageLabel" Pinned="true" TextPrefix="" runat="server" /></div>
								</div>
							</div>
							<div class="Clear"></div>
							<div class="OrdResultsFtr"></div>
						</ItemTemplate>
						<SeparatorTemplate>
							<br />
						</SeparatorTemplate>
					</asp:Repeater>
					<div class="OrdFtr">
						<div class="Left">
							<app:BackToSearchHyperlink ID="BackToSearchHyperlink2" runat="server" />
						</div>
						<div class="Right"> 
							<span class="BodyTxt Bold">select all assets</span><asp:CheckBox ID="SelectAllAssetsInCart2" OnCheckedChanged="SelectAllAssetsInCart_CheckChanged" AutoPostBack="true" runat="server" />
							<span class="BodyTxt">[</span><asp:LinkButton ID="DeselectAllLinkButton2" CssClass="BodyTxt" Text="deselect all" OnClick="DeselectAllLinkButton_Click" runat="server" />
							<span class="BodyTxt"> |</span>
							<asp:LinkButton ID="RemoveSelectedAssetsLinkButton2" Text="remove selected" CssClass="BodyTxt" OnClick="RemoveSelectedAssetsLinkButton_Click" runat="server" /><span class="BodyTxt">]</span>
						</div>
					</div>
					<div class="Clear"></div>
					<br />
					<div class="OrdProcess">
						<app:FeedbackLabel ID="FeedbackLabel2" LineBreaks="1" runat="server" />
						<span class="BodyTxt Bold">I agree to the </span><a href="javascript:showTermsConditions()" class="BodyTxt Bold">Terms &amp; Conditions of Use</a> <span class="BodyTxt Bold">of <%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().ApplicationName%> and confirm that I am authorised to use this system <asp:CheckBox ID="AgreeTermsConditionsCheckBox" runat="server" /><br /></span>
						<br />
						<asp:Button ID="ProcessCartButton" Text="process cart" CssClass="button" OnClick="ProcessCartButton_Click" runat="server" /><br />
					</div>
					<br />
				</asp:PlaceHolder>
				
				<asp:PlaceHolder ID="CartNoResultsPanel" Visible="false" runat="server">
					<div class="noResults OrdMrg">
						<app:FeedbackLabel ID="NoCartResultsMessageLabel" MessageType="Negative" Header="no assets have been added to the cart" Pinned="true" LineBreaks="1" runat="server" />
						<app:BackToSearchHyperlink ID="BackToSearchHyperlink3" ShowRightDivider="true" runat="server" />
						<app:HelpHyperLink ID="HelpHyperLink1" HelpPage="4_Cart.htm" CssClass="BodyTxt H20" Text="help" runat="server" />
					</div>
				</asp:PlaceHolder>
				
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