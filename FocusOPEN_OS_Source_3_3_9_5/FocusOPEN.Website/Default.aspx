<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FocusOPEN.Website.Default" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<!--*******************************************************************************************************************
	
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011.  All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the
	software without specific, written prior permission. Title to copyright in this software and any associated
	documentation will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about
	this software.

********************************************************************************************************************-->


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title>Welcome [<%=FocusOPEN.Website.Components.WebsiteBrandManager.GetBrand().ApplicationName%>]</title>
	<app:BrandHeader ID="BrandHeader1" Css="Basic|Home|Styles" runat="server" />
	<script src="Includes/Javascript/jQuery/jquery-1.4.1.min.js" type="text/javascript"></script>
	<script src="Includes/Javascript/jQuery/jquery-ui-1.7.2.custom.min.js" type="text/javascript"></script>
	<script src="Includes/Javascript/jQuery/jquery.cookie.js" type="text/javascript"></script>
	<script src="Includes/Javascript/Global.js" type="text/javascript"></script>
	<link rel="Stylesheet" type="text/css" href="Includes/Css/jQuery-UI/smoothness/jquery-ui-1.7.2.custom.css" />
	<script type="text/javascript">ispreview = <%=(FocusOPEN.Shared.WebUtils.GetIntRequestParam("PreviewHomepageId", 0) > 0).ToString().ToLower()%>;</script>
</head>
<body>
	<form id="form1" runat="server">
	
		<asp:Panel ID="PreviewToolbarPanel" Visible="false" runat="server">
			<div style="width:100%;padding:10px;height:25px;background:#000;border-bottom:2px solid #666;text-align:center;color:#fff">
				<a style="color:#fff" href="Default.aspx?HideBumper=1&PreviewHomepageId=<%=FocusOPEN.Shared.WebUtils.GetIntRequestParam("PreviewHomepageId", 0)%>">Homepage</a>
				&nbsp;&nbsp;|&nbsp;&nbsp;
				<a style="color:#fff" href="Default.aspx?ShowBumper=1&PreviewHomepageId=<%=FocusOPEN.Shared.WebUtils.GetIntRequestParam("PreviewHomepageId", 0)%>">Bumper Page</a>
				&nbsp;&nbsp;|&nbsp;&nbsp;
				<a style="color:#fff" href="javascript:self.close()">Close [X]</a>
			</div>
		</asp:Panel>
	
		<aspajax:ScriptManager ID="ScriptManager1"  runat="server" />
		
		<asp:PlaceHolder ID="CustomHtmlPageContentWrapper_NoNav" runat="server">
			<asp:Literal ID="NoNav_HtmlContentLiteral" runat="server" />
		</asp:PlaceHolder>
		
		<asp:PlaceHolder ID="CustomHtmlPageContentWrapper_WithNav" runat="server">
			<div class="CatWrapper">
				<app:PageHeader ID="PageHeader1" runat="server" />
				<div class="HomeBkg">
					<asp:Literal ID="WithNav_HtmlContentLiteral" runat="server" />
				</div>
				<app:HomepageFooter ID="HomepageFooter1" runat="server" />
			</div>
		</asp:PlaceHolder>
		
		<asp:PlaceHolder ID="StandardTemplatePageContentWrapper" runat="server">
			<div class="CatWrapper">
				<app:PageHeader ID="PageHeader2" runat="server" />
				<div class="HomeBkg">
					<div class="HomePanel">
						<div class="HomeMrgLeft">
							<div class="HomeText">
								<asp:Literal ID="IntroTextLiteral" runat="server" />
								<asp:Repeater ID="FeaturedCategoriesRepeater" OnItemDataBound="FeaturedCategoriesRepeater_ItemDataBound" runat="server">
									<HeaderTemplate>
										<img src="images/home/headers/featured_categories.gif" width="120" height="38" alt="Featured Categories" />
										<ul>
									</HeaderTemplate>
									<FooterTemplate>
										</ul>
									</FooterTemplate>
									<ItemTemplate><li><asp:HyperLink ID="FeaturedCategoryHyperLink" CssClass="Black" runat="server" /> </li></ItemTemplate>
								</asp:Repeater>
							</div>
						</div>
						<div class="HomeMrgRight">
							<asp:HyperLink ID="QuickLink1HyperLink" runat="server"><app:HomepageImage ID="HomepageImage1" ImageNumber="1" runat="server"/></asp:HyperLink>
							<asp:HyperLink ID="QuickLink2HyperLink" runat="server"><app:HomepageImage ID="HomepageImage2" ImageNumber="2" runat="server"/></asp:HyperLink>
							<asp:HyperLink ID="QuickLink3HyperLink" runat="server"><app:HomepageImage ID="HomepageImage3" ImageNumber="3" runat="server"/></asp:HyperLink>
							<asp:HyperLink ID="QuickLink4HyperLink" runat="server"><app:HomepageImage ID="HomepageImage4" ImageNumber="4" runat="server"/></asp:HyperLink>
						</div>
					</div>
					<div class="Clear"></div>
				</div>
				<app:HomepageFooter ID="HomepageFooter2" runat="server" />
			</div>
		</asp:PlaceHolder>
	</form>
</body>
</html>