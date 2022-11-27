<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PersistentCartLightbox.ascx.cs" Inherits="FocusOPEN.Website.Controls.PersistentCartLightbox" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>


<!--[if lte IE 6]>
	<style type="text/css">
   /*<![CDATA[*/ 
		html
		{
			overflow-x:auto;
			overflow-y:hidden;
		}
		body
		{
			margin:0;
			padding:0 8px 0 8px;
			border:0;
			height:100%;
			overflow-y:auto;
		}
   /*]]>*/
   </style>
   <p>&nbsp;</p>
<![endif]-->

<asp:PlaceHolder ID="CartOpen" Visible="false" runat="server">
	<style type="text/css">
		body
		{
			margin-bottom: 196px;
		}
	</style>
	<!--[if lte IE 6]>
	   <p>&nbsp;</p>
	   <p>&nbsp;</p>
	<![endif]-->
	<div id="fixedUp"> 
		<div class="CartMenuBkgUp"> 
			<div class="MenuWrapper"> 
				<!--col 1-->
				<div class="Left MenuPanelOpt">
					<asp:LinkButton ID="CloseCartLinkButton1" CommandName="close" CommandArgument="cart" OnCommand="TogglePanelLinkButton_Click" CssClass="MenuHdrTxt" Text="hide [-]" runat="server" />
					<br />
					<!--options menu-->
					<br />
					<span class="MenuTxt Bold">Options:</span><br />
					<span class="MenuTxt">&bull;</span> <a id="A1" href="~/MyAccount/ViewCart.aspx" runat="server" class="MenuTxt">Place Order</a><br />
					<span class="MenuTxt">&bull;</span> <app:PromptLinkButton ID="EmptyCartLinkButton" OnClick="EmptyCartLinkButton_Click" CssClass="MenuTxt" Text="Remove All" Prompt="Are you sure you want to remove all assets from your cart?" runat="server" /> <br />
					<span class="InlineHelpTxt"><span class="MenuTxt">&bull;</span> <app:HelpHyperLink ID="HelpHyperLink1" HelpPage="4_Cart.htm#persistent_cart" CssClass="MenuTxt" Text="Help" runat="server" /><br /></span>
				</div>
				<!--col 2-->
				<div class="Left MenuPanelThb">
					<!--header-->
					<span class="MenuHdrTxt Bold">CART &nbsp;</span>
					<!--view selector-->
					<span class="MenuHdrTxt">current view</span> 
					<app:PersistentCartLightboxModeDropDownList ID="CartOpen_ModeDropDownList" OnSelectedIndexChanged="ModeDropDownList_SelectedIndexChanged" runat="server" />
					<!--total-->
					<span class="MenuTxt">&nbsp; total assets: <span class="Bold"><asp:Label ID="CartItemCount1" Text="[CartItemCount1]" runat="server" /></span> in cart<br /></span>
					<!--thumbnail display-->
					<asp:Panel ID="CartRepeaterPanel" runat="server">
						<asp:Repeater ID="CartRepeater" OnItemDataBound="AssetRepeater_ItemDataBound"  runat="server">
							<HeaderTemplate>
								<table border="0" cellspacing="0" cellpadding="0">
								<tr>
									<td class="CartEndL">
										<div class="ArrowL">
											<asp:ImageButton ID="CartLeftArrowImageButton" CommandName="Cart" ImageUrl="~/images/icons/arrowL.gif" Width="8" Height="20" BorderWidth="0" AlternateText="[left]" OnCommand="ArrowImageButton_Command" runat="server" />
										</div>
									</td>
							</HeaderTemplate>
							<ItemTemplate>
									<td id="AssetCell" runat="server" class="MenuGridBkg">
										<asp:PlaceHolder ID="AssetContainer" runat="server">
											<app:AssetThumbnail ID="AssetThumbnail1" UseMarginCssClass="false" OtherCssClass="MenuThbMrg" runat="server" />
											<br /> 
											<div class="MenuIcons">
												<app:AssetButtons ID="AssetButtons1" ShowButtons="Cart,Lightbox" runat="server" />
											</div>
										</asp:PlaceHolder>
										<asp:PlaceHolder ID="EmptyAssetContainer" runat="server">
											<img id="Img1" src="~/images/no-assets-available.gif" runat="server" alt="no assets available" width="90" height="120" class="MenuThbMrg"/><br />
										</asp:PlaceHolder>
									</td>
							</ItemTemplate>
							<FooterTemplate>
									<td class="CartEndR">
										<div class="ArrowR">
											<asp:ImageButton ID="CartRightArrowImageButton" CommandName="Cart" ImageUrl="~/images/icons/arrowR.gif" Width="8" Height="20" BorderWidth="0" AlternateText="[right]" OnCommand="ArrowImageButton_Command" runat="server" />
										</div>
									</td>
								</tr>
								</table>
							</FooterTemplate>
						</asp:Repeater>
					</asp:Panel>
				</div>
				<!--col 3-->
				<div class="Left MenuPanelEnd">
					<asp:LinkButton ID="CloseCartLinkButton2" CommandName="close" CommandArgument="cart" OnCommand="TogglePanelLinkButton_Click" CssClass="MenuHdrTxt" Text="hide [-]" runat="server" />
				</div>
				<div class="Clear"></div>
			</div>
		</div>
	</div>
</asp:PlaceHolder>


<asp:PlaceHolder ID="CartClosed" Visible="false" runat="server">
	<style type="text/css">
		body
		{
			margin-bottom: 25px;
		}
	</style>
	<div id="fixedDown">
		<div class="CartMenuBkgDown">
			<div class="MenuWrapper"> 
				<!--col 1-->
				<div class="Left MenuPanelOpt">
					<asp:LinkButton ID="OpenCartLinkButton1" CommandName="open" CommandArgument="cart" OnCommand="TogglePanelLinkButton_Click" CssClass="MenuHdrTxt" Text="show [+]" runat="server" />
				</div>
				<!-- col 2-->
				<div class="Left MenuPanelThb"> 
					<!--Hdr-->
			        <span class="MenuHdrTxt Bold">CART &nbsp;</span> 
					<!--View selector-->
					<span class="MenuTxt">current view</span> 
					<app:PersistentCartLightboxModeDropDownList ID="CartClosed_ModeDropDownList" CssClass="formInput" OnSelectedIndexChanged="ModeDropDownList_SelectedIndexChanged" runat="server" />
					<!--Total-->
					<span class="MenuTxt"> 
						&nbsp; total assets: <span class="Bold"><asp:Label ID="CartItemCount2" Text="[CartItemCount2]" runat="server" /></span> in cart
						<br />
					</span>
				</div>
				<!--col 3-->
				<div class="Left MenuPanelEnd">
					<asp:LinkButton ID="OpenCartLinkButton2" CommandName="open" CommandArgument="cart" OnCommand="TogglePanelLinkButton_Click" CssClass="MenuHdrTxt" Text="show [+]" runat="server" />
				</div>
				<div class="Clear"></div>
			</div>
		</div>
	</div>
</asp:PlaceHolder>


<asp:PlaceHolder ID="LightboxOpen" Visible="false" runat="server">
	<style type="text/css">
		body
		{
			margin-bottom: 196px;
		}
	</style>
	<!--[if lte IE 6]>
	   <p>&nbsp;</p>
	   <p>&nbsp;</p>
	<![endif]-->
	<div id="fixedUp"> 
		<div class="LbxMenuBkgUp"> 
			<div class="MenuWrapper"> 
				<!--col 1-->
				<div class="Left MenuPanelOpt">
					<!--show/hide//--> 
					<asp:LinkButton ID="CloseLightboxLinkButton1" CommandName="close" CommandArgument="lightbox" OnCommand="TogglePanelLinkButton_Click" CssClass="MenuHdrTxt" Text="hide [-]" runat="server" />
					<br />
					<!--options menu-->
					<br />
					<span class="MenuTxt Bold">Options:</span><br />
					<span class="MenuTxt">&bull;</span> <asp:HyperLink ID="NewLightboxHyperLink" CssClass="MenuTxt" Text="New Lightbox" runat="server" /><br />
					<span class="MenuTxt">&bull;</span> <asp:HyperLink ID="SendLightboxHyperLink" CssClass="MenuTxt" Text="Send Lightbox" runat="server" /><br />
					<span class="MenuTxt">&bull;</span> <asp:HyperLink ID="ViewLightboxHyperLink" CssClass="MenuTxt" Text="View Lightbox" runat="server" /><br />
					<span class="MenuTxt">&bull;</span> <asp:LinkButton ID="CopyAllToCartLinkButton" CssClass="MenuTxt" Text="Add All To Cart" OnClick="CopyAllToCartLinkButton_Click" runat="server" /><br />
					<span class="InlineHelpTxt"><span class="MenuTxt">&bull;</span> <app:HelpHyperLink ID="HelpHyperLink2" HelpPage="3_Lightboxes.htm#persistent_lightbox" CssClass="MenuTxt" Text="Help" runat="server" /><br /></span>
				</div>
				<!--col 2-->
				<div class="Left MenuPanelThb">
					<!--Header-->
					<span class="MenuHdrTxt Bold">LIGHTBOX &nbsp;</span> 
					<!--View Selector-->
					<span class="MenuHdrTxt">current view</span> 
					<app:PersistentCartLightboxModeDropDownList ID="LightboxOpen_ModeDropDownList" OnSelectedIndexChanged="ModeDropDownList_SelectedIndexChanged" runat="server" />
					<!--Lightbox Selector-->
					<span class="MenuTxt">
						selected lightbox 
						<app:UserLightboxDropDownList ID="LightboxDropDownList1" AutoPostBack="true" OnSelectedIndexChanged="LightboxDropDownList_SelectedIndexChanged" CssClass="formInput" OmitBlankItem="true" runat="server" />
						<!--Total-->
						&nbsp; total assets: <span class="Bold"><asp:Label ID="LightboxItemCount1" Text="[LightboxItemCount1]" runat="server" /></span> in this lightbox
						<br />
					</span>
					<asp:Panel ID="LightboxRepeaterPanel" runat="server">
						<asp:Repeater ID="LightboxRepeater" OnItemDataBound="AssetRepeater_ItemDataBound"  runat="server">
							<HeaderTemplate>
								<table border="0" cellspacing="0" cellpadding="0">
								<tr>
									<td class="LbxEndL">
										<div class="ArrowL">
											<asp:ImageButton ID="LightboxLeftArrowImageButton" CommandName="Lightbox" ImageUrl="~/images/icons/arrowL.gif" Width="8" Height="20" BorderWidth="0" AlternateText="[left]" OnCommand="ArrowImageButton_Command" runat="server" />
										</div>
									</td>
							</HeaderTemplate>
							<ItemTemplate>
									<td id="AssetCell" runat="server" class="MenuGridBkg">
										<asp:PlaceHolder ID="AssetContainer" runat="server">
											<app:AssetThumbnail ID="AssetThumbnail1" UseMarginCssClass="false" OtherCssClass="MenuThbMrg" runat="server" />
											<br /> 
											<div class="MenuIcons">
												<app:AssetButtons ID="AssetButtons1" ShowButtons="Cart,Lightbox" runat="server" />
											</div>
										</asp:PlaceHolder>
										<asp:PlaceHolder ID="EmptyAssetContainer" runat="server">
											<img src="~/images/no-assets-available.gif" runat="server" alt="no assets available" width="90" height="120" class="MenuThbMrg"/><br />
										</asp:PlaceHolder>
									</td>
							</ItemTemplate>
							<FooterTemplate>
									<td class="LbxEndR">
										<div class="ArrowR">
											<asp:ImageButton ID="LightboxRightArrowImageButton" CommandName="Lightbox" ImageUrl="~/images/icons/arrowR.gif" Width="8" Height="20" BorderWidth="0" AlternateText="[right]" OnCommand="ArrowImageButton_Command" runat="server" />
										</div>
									</td>
								</tr>
								</table>
							</FooterTemplate>
						</asp:Repeater>
					</asp:Panel>
				</div>
				<div class="Left MenuPanelEnd">
					<asp:LinkButton ID="CloseLightboxLinkButton2" CommandName="close" CommandArgument="lightbox" OnCommand="TogglePanelLinkButton_Click" CssClass="MenuHdrTxt" Text="hide [-]" runat="server" />
				</div>
				<div class="Clear"></div>
			</div>
		</div>
	</div>
</asp:PlaceHolder>


<asp:PlaceHolder ID="LightboxClosed" Visible="false" runat="server">
	<style type="text/css">
		body
		{
			margin-bottom: 25px;
		}
	</style>
	<div id="fixedDown">
		<div class="LbxMenuBkgDown">
			<div class="MenuWrapper"> 
				<!--col 1-->
				<div class="Left MenuPanelOpt">
					<asp:LinkButton ID="OpenLightboxLinkButton1" CommandName="open" CommandArgument="lightbox" OnCommand="TogglePanelLinkButton_Click" CssClass="MenuHdrTxt" Text="show [+]" runat="server" />
				</div>
				<!--col 2-->
				<div class="Left MenuPanelThb">
					<!--header-->
					<span class="MenuHdrTxt Bold">LIGHTBOX &nbsp;</span> 
					<!--view selector-->
					<span class="MenuTxt">current view</span> 
					<app:PersistentCartLightboxModeDropDownList ID="LightboxClosed_ModeDropDownList" OnSelectedIndexChanged="ModeDropDownList_SelectedIndexChanged" runat="server" />
					<!--lightbox selector-->
					<span class="MenuTxt">selected lightbox 
						<app:UserLightboxDropDownList ID="LightboxDropDownList2" AutoPostBack="true" OnSelectedIndexChanged="LightboxDropDownList_SelectedIndexChanged" CssClass="formInput" OmitBlankItem="true" runat="server" />
						<!--total-->
						&nbsp; total assets: <span class="Bold"><asp:Label ID="LightboxItemCount2" Text="[LightboxItemCount2]" runat="server" /></span> in this lightbox <br />
					</span>
				</div>
				<!--col 3-->
				<div class="Left MenuPanelEnd">
					<asp:LinkButton ID="OpenLightboxLinkButton2" CommandName="open" CommandArgument="lightbox" OnCommand="TogglePanelLinkButton_Click" CssClass="MenuHdrTxt" Text="show [+]" runat="server" />
				</div>
				<div class="Clear"></div>
			</div>
		</div>
	</div>
</asp:PlaceHolder>