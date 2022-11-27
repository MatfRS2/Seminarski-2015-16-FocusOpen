<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppInfo.aspx.cs" Inherits="FocusOPEN.Website.About.AppInfo" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Application Info</title>
    <style type="text/css">
		body
		{
			padding: 0px;
			margin: 0px;
			background: #eee;
		}
		h1
		{
			font-family: arial, verdana, sans-serif;
			padding: 15px 25px 10px 10px;
			border-bottom: 1px solid #999;
			margin: 0px;
			margin-bottom: 10px;
			background: #CEFFFF;
		}
		.txt
		{
			font-family: arial, verdana, sans-serif;
			font-size: 0.8em;
		}
		.label
		{
			font-family: arial, verdana, sans-serif;
			font-weight: bold;
			font-size: 0.8em;
			width: 130px;
			float: left;
			background: #eee;
			padding: 4px;
		}
		.info
		{
			font-family: arial, verdana, sans-serif;
			font-size: 0.8em;
			float: left;
			padding: 4px;
		}
		div .spacer
		{
			clear:both;
			height:5px;
		}
		#assemblyList
		{
			height:310px;
			overflow:auto;
			border:1px solid #ccc;
			padding: 0px 15px 0px 5px;
		}
		#assemblyList a
		{
			font-family: arial, sans-serif;
			font-size: 1em;
			display: block;
			padding: 5px;
			width: 100%;
		}
		#assemblyList a:hover
		{
			background: #ccc;
		}
		#footer
		{
			font-family: arial, sans-serif;
			font-size: 0.7em;
			font-weight: bold;
			margin: 0px;
			margin-top: 15px;
			padding: 8px;
			background: #CEFFFF;
		}
    </style>
</head>
<body>
<form id="form1" runat="server">
	<table cellpadding="0" cellspacing="0" width="100%" height="98%">
	<tr>
		<td align="center" valign="middle">
			<table border="0" cellpadding="10" cellspacing="0" bgcolor="#cccccc">
			<tr>
				<td align="center" valign="top">
					<table width="1000" height="400" bgcolor="#ffffff" style="border:1px solid #000; padding:5px;height:400px;">
					<tr>
						<td colspan="2"><h1>About - Application &amp; Dependency Information</h1></td>
					</tr>
					<tr>
						<td colspan="2" class="txt" valign="top">
							The application dependencies are listed below.  Click on a dependency filename to view more information.<br /><br />
						</td>
					</tr>
					<tr>
						<td width="270" valign="top">
							<div id="assemblyList">
								<asp:Repeater ID="repeater1" OnItemDataBound="repeater1_ItemDataBound" runat="server">
									<ItemTemplate>
										<asp:LinkButton ID="FileNameLinkButton" OnCommand="FileNameLinkButton_Command" runat="server" />
									</ItemTemplate>
								</asp:Repeater>
							</div>
						</td>
						<td valign="top">
							<div style="padding:0px;padding-left:10px;">
								<asp:Panel ID="AssemblyInfoPanel" runat="server">
									<span class="label">Filename:</span>
									<asp:Label ID="FilenameLabel" CssClass="info" runat="server" />
									<div class="spacer"></div>
									<span class="label">Title:</span>
									<asp:Label ID="TitleLabel" CssClass="info" runat="server" />
									<div class="spacer"></div>
									<span class="label">Description:</span>
									<asp:Label ID="DescriptionLabel" CssClass="info" runat="server" />
									<div class="spacer"></div>
									<span class="label">Product:</span>
									<asp:Label ID="ProductLabel" CssClass="info" runat="server" />
									<div class="spacer"></div>
									<span class="label">Company:</span>
									<asp:Label ID="CompanyLabel" CssClass="info" runat="server" />
									<div class="spacer"></div>
									<span class="label">Copyright:</span>
									<asp:Label ID="CopyrightLabel" CssClass="info" runat="server" />
									<div class="spacer"></div>
									<span class="label">Trademark:</span>
									<asp:Label ID="TrademarkLabel" CssClass="info" runat="server" />
									<div class="spacer"></div>
									<span class="label">Assembly Version:</span>
									<asp:Label ID="AssemblyVersionLabel" CssClass="info" runat="server" />
									<div class="spacer"></div>
									<span class="label">Created:</span>
									<asp:Label ID="CreatedLabel" CssClass="info" runat="server" />
									<div class="spacer"></div>
									<span class="label">Last Modified:</span>
									<asp:Label ID="LastModifiedLabel" CssClass="info" runat="server" />
									<div class="spacer"></div>
									<span class="label">File Size:</span>
									<asp:Label ID="FileSizeLabel" CssClass="info" runat="server" />
									<div class="spacer"></div>
								</asp:Panel>
							</div>
						</td>
					</tr>
					<tr>
						<td colspan="2">
							<div id="footer">
								<asp:Label ID="CurrentAssemblyNameLabel" runat="server" /> last modified on <asp:Label ID="CurrentAssemblyLastModifiedLabel" runat="server" />. Build Number: <asp:Label ID="CurrentAssemblyBuildNumberLabel" runat="server" />
							</div>
						</td>
					</tr>
					</table>
				</td>
			</tr>
			</table>
		</td>
	</tr>
	</table>
</form>
</body>
</html>