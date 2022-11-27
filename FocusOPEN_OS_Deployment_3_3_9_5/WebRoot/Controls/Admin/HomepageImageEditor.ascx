<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HomepageImageEditor.ascx.cs" Inherits="FocusOPEN.Website.Controls.HomepageImageEditor" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<table border="0" cellpadding="5" cellspacing="0">
<tr>
	<td class="BodyTxt Bold alignTop">URL:</td>
	<td valign="top"><asp:TextBox ID="UrlTextBox" CssClass="formInput W225" runat="server" /></td>
</tr>
<tr>
	<td class="BodyTxt Bold" valign="top">Image:</td>
	<td valign="top">
		<app:HomepageImage ID="HomepageImage" runat="server" />
		<div style="font-size:11px;padding:10px 0px 10px 0px;">Note: uploaded images will automatically be resized to 168px (w) x 124px (h).</div>
		<asp:FileUpload ID="ImageFileUpload" CssClass="formInput" runat="server" />
	</td>
</tr>
</table>
<br />