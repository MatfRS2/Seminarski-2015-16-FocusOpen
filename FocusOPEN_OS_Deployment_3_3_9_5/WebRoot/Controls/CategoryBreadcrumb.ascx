<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CategoryBreadcrumb.ascx.cs" Inherits="FocusOPEN.Website.Controls.CategoryBreadcrumb" %>

<asp:Repeater runat="server" ID="BreadcrumbRepeater" OnItemCommand="BreadcrumbRepeater_ItemCommand">
    <ItemTemplate>
        <asp:LinkButton runat="server" ID="BreadcrumbLink" Text='<%# Eval("Name") %>' CommandArgument='<%# Eval("CategoryId") %>' CommandName="SelectCategory" CssClass="BodyTxt Bold"/>
        <asp:Label runat="server" ID="AssetsCountLabel" CssClass="BodyTxt Bold" Text='<%# " (" +  Eval("AssetsCount") + ")" %> '></asp:Label>
    </ItemTemplate>
    <SeparatorTemplate>
        <span class="BodyTxt Bold" style="padding:0px 2px 0px 2px">&raquo;</span>
    </SeparatorTemplate>
</asp:Repeater>
