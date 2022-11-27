<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewOrdersEmpty.aspx.cs" Inherits="FocusOPEN.Website.ViewOrdersEmpty" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title>Download Manager [<%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().ApplicationName%>]</title>
	<app:BrandHeader ID="BrandHeader1" Css="Basic|Styles" runat="server" />
	<script src="../Includes/Javascript/jQuery/jquery-1.4.1.min.js" type="text/javascript"></script>
	<script src="../Includes/Javascript/jQuery/jquery-ui-1.7.2.custom.min.js" type="text/javascript"></script>
	<script src="../Includes/Javascript/Global.js" type="text/javascript"></script>
	<link rel="Stylesheet" type="text/css" href="../Includes/Css/jQuery-UI/smoothness/jquery-ui-1.7.2.custom.css" />
</head>
<body>
<form id="form1" runat="server">
	<aspajax:ScriptManager ID="ScriptManager1" runat="server" />
	<div class="CatWrapper">
		<app:PageHeader ID="PageHeader1" runat="server" />
		<div class="CatBkg">
			<div class="DManAppPanel">
				<div class="DMHdr">
					<div class="Left">
						<br />
						<!--start text header//--><span class="CatAppHdr">MY ORDERS</span>&nbsp;<!--end text header//-->
						<span class="PanelTxt BaseLine">current order: <span class="Bold">no orders placed</span></span><br />
					</div>
					<div class="Right">
						<br />
						<span class="PanelTxt">show</span>
						<app:PageSizeDropDownList ID="PageSizeDropDownList1" CssClass="formInput" runat="server" />
					</div>
				</div>
				<div class="Clear"><br /></div>
				<div class="DMHdrSelector">
					<br />
				</div>
				<div class="Clear"></div> 
				<div class="DMHdrFtr">
					<div class="Left"> <span class="PanelTxt H20">total in this order: <span class="Bold">0</span> assets [displaying page 0 of 0]</span></div>
					<div class="Right">
						<!--Empty Pager-->
					</div>
					<div class="Clear"></div>
				</div>
			</div>
			<div class="noResults DMMrg"> 
				<app:FeedbackLabel ID="FeedbackLabel1" MessageType="Negative" Header="no orders have been made" LineBreaks="1" Pinned="true" runat="server" />
				<app:BackToSearchHyperlink ID="BackToSearchHyperLink1" ShowRightDivider="true" runat="server" />
				<app:HelpHyperLink ID="HelpHyperLink1" HelpPage="5_Download_Manager.htm" CssClass="BodyTxt H20" Text="help" runat="server" />
			</div>
			<div class="CatAppFtr">
				<div class="Left">
					<div class="AppResults PanelTxt">total in this order: <span class="Bold">0</span> assets [displaying page 0 of 0]</div>
				</div>
				<div class="Right">
					<!-- Empty Pager-->
				</div>
			</div>
		</div>
		<app:PageFooter ID="PageFooter1" HelpPage="index.htm#catalogue" runat="server" />
	</div>
</form>
</body>
</html>