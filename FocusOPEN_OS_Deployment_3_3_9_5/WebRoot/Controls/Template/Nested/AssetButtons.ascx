<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssetButtons.ascx.cs" Inherits="FocusOPEN.Website.Controls.AssetButtons" %>

<asp:ImageButton ID="AddToLightboxImageButton" CommandName="Lightbox" OnCommand="AddLinkButton_Command" runat="server"/>
<asp:ImageButton ID="AddToCartImageButton" CommandName="Cart" OnCommand="AddLinkButton_Command" runat="server"/>
<asp:HyperLink ID="InfoHyperlink" ToolTip="More information" runat="server" />
<asp:HyperLink ID="DownloadHyperLink" ToolTip="Download" runat="server" />
<asp:Image ID="RestrictedImageIcon" ToolTip="Restricted" runat="server" />