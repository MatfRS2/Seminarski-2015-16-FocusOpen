<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BrandMetadataSettingDetails.ascx.cs" Inherits="FocusOPEN.Website.Controls.Admin.BrandMetadataSettingDetails" %>

<div>

Name<br />
<asp:TextBox ID="FieldNameTextBox" CssClass="formInput W100" MaxLength="100" runat="server" />
<br />
<br />

Additional copy<br />
<app:TextArea ID="AdditionalCopyTextBox" Width="150px" Height="40px" MaxLength="250" runat="server" />

<br />


Tooltip<br />
<app:TextArea ID="ToolTipTextBox" Width="150px" Height="40px" MaxLength="250" runat="server" />
								

</div>