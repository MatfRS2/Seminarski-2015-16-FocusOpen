<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewContactSheet.aspx.cs" Inherits="FocusOPEN.Website.ViewContactSheet" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title>Contact Sheet [<%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().ApplicationName%>]</title>
	<app:BrandHeader ID="BrandHeader1" Css="Basic|Styles|Other" runat="server" />
	<script src="Includes/Javascript/jQuery/jquery-1.4.1.min.js" type="text/javascript"></script>
	<script src="Includes/Javascript/jQuery/jquery-ui-1.7.2.custom.min.js" type="text/javascript"></script>
	<script src="Includes/Javascript/Global.js" type="text/javascript"></script>
	<link rel="Stylesheet" type="text/css" href="Includes/Css/jQuery-UI/smoothness/jquery-ui-1.7.2.custom.css" />
</head>
<body>
	<form id="form1" runat="server">
		<aspajax:ScriptManager ID="ScriptManager1" runat="server" />
		<div class="CatWrapper">
			<div class="CatHdrPanel"></div>
			<div class="CntPanel alignRight">
				<br />
				<br />
			</div>
			<div class="CntBkg">
				<div class="CntHdr">
					<div class="CntMrg BodyTxt"> 
						<br />
						<div class="CatAppHdr">CONTACT SHEET</div>
						<br/>
						<span class="Bold">Title:</span>
						<asp:Label ID="LightboxTitleLabel" runat="server" />
						<br />
						<span class="Bold">Sent By:</span>
						<asp:Label ID="LightboxSenderName" runat="server" /> [<app:EmailHyperLink ID="LightboxSenderEmail" runat="server" />]
						<br />
						<span class="Bold">Sent On:</span>
						<asp:Label ID="LightboxDateSentLabel" runat="server" />
						<br />
						<span class="Bold">Expires On:</span>
						<asp:Label ID="LightboxDateExpiresLabel" runat="server" />
						<br />
						<span class="Bold">Notes:</span>
						<asp:Label ID="LightboxNotesLabel" runat="server" />
					</div>
				</div> 
				<div class="content">
					<asp:DataList ID="LightboxDataList" RepeatDirection="Horizontal" RepeatColumns="4" RepeatLayout="Table" Width="749" BorderWidth="0" CellSpacing="1" CellPadding="0" OnItemDataBound="LightboxDataList_ItemDataBound" runat="server">
						<ItemStyle CssClass="SrcGridBkg" />
						<ItemTemplate>
							<asp:Panel ID="LightboxAssetInfoPanel" runat="server">
								<app:AssetThumbnail ID="AssetThumbnail1" HighlightIfRestricted="false" ImageLinksToAssetInfo="false" runat="server" /><br />
								<span class="PanelTxt Bold" id="AssetIdLabel" runat="server">[AssetId]</span><br />
								<span class="PanelTxt" id="AssetTypeLabel" runat="server">[AssetType]</span><br />
								<br />
								<app:IconImage ID="NotesImage" Filename="notes.gif" AlternateText="notes" width="17" height="17" runat="server"/>
                                <asp:HyperLink ID="DownloadHyperLink" ToolTip="Download" runat="server" />
                            </asp:Panel>
							<br />
						</ItemTemplate>
					</asp:DataList>
				</div>
			</div>
			<div class="CntFtr">
				<br />
				<br />
			</div>
			<app:PageFooter ID="PageFooter1" HelpPage="0_Welcome.htm" runat="server" />
		</div>
	</form>
</body>
</html>