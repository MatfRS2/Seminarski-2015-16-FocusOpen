<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NavigationBar.ascx.cs" Inherits="FocusOPEN.Website.Controls.NavigationBar" %>
<div class="Left">
	<a href="<%=ResolveUrl("~/Default.aspx")%>?nav=1" class="NavTxt Bold" title="Home">Home</a> | 
	<a href="<%=ResolveUrl("~/SearchResults.aspx")%>?nav=1" class="NavTxt Bold" title="Browse">Browse</a> | 
	<a href="<%=ResolveUrl("~/MyAccount/Default.aspx")%>?nav=1" class="NavTxt Bold" title="My Account">My Account</a> | 
	<a href="<%=ResolveUrl("~/MyAccount/ViewLightbox.aspx")%>?nav=1" class="NavTxt Bold" title="Lightbox">Lightbox</a> | 
	<a href="<%=ResolveUrl("~/MyAccount/ViewCart.aspx")%>?nav=1" class="NavTxt Bold" title="Cart">View Cart</a> | 
	<a href="<%=ResolveUrl("~/MyAccount/ViewOrders.aspx")%>?nav=1" class="NavTxt Bold" title="My Orders">My Orders</a>
	<span id="UploadLinkWrapper" visible="false" runat="server"> | <a href="<%=ResolveUrl("~/Admin/Assets/AssetUpload.aspx")%>" class="NavTxt Bold" title="Upload">Upload</a></span>
	<span id="AdminLinkWrapper" visible="false" runat="server"> | <a href="<%=ResolveUrl("~/Admin/Default.aspx")%>" class="NavTxt Bold" title="Admin">Admin</a></span>
</div>   
<div class="Right">
	<a id="UserNameHref" href="~/MyAccount/EditProfile.aspx" class="LoginTxt" runat="server">[Name]</a> <span class="LoginTxt">logged in</span> | <a id="LogoutLink" href="~/Login.aspx?action=logout" class="LoginTxt" runat="server">Log Out</a>
</div>