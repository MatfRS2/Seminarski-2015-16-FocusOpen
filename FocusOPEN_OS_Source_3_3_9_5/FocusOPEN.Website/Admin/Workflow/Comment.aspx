<%@ Page Title="Comment [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="Comment.aspx.cs" Inherits="FocusOPEN.Website.Admin.WorkflowComment" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg">    
		<div class="AdmTblPanel Clear">
			<div class="Left">
				<div class="AdmAppHdr">Comments</div>
				<br />
			</div>
			<div class="Right"></div>
			<div class="Clear">
				<br />
				<div class="AdmOrdNotesPanel"> 
					<div class="NotesMrg H15"> 
						<span class="BodyTxt"> 
							<span class="Bold">Asset Ref # </span><asp:Literal ID="AssetReferenceLiteral" runat="server" /><br /> 
							<span class="Bold">Upload Administrator: </span><app:EmailHyperLink ID="UploadUserEmailHyperLink" runat="server" /><br />
							
							<!--workflow-->
							<app:AssetWorkflowUserList ID="AssetWorkflowUserList" runat="server" />
							
							<!--publication date-->
							<span class="Bold">Publication Date: </span><asp:Literal ID="PublicationDateLiteral" runat="server" /><br />
						</span>
						<br />
						
						<!--start notes nav-->
						<div class="alignRight">
							<asp:HyperLink ID="DownloadAssetHyperLink" CssClass="BodyTxt Bold" Text="download this asset" runat="server" />
							<span class="BodyTxt">|</span>
							<app:EnhancedHyperLink ID="EditAssetHyperLink" Text="edit asset" NavigateUrl="#" CssClass="BodyTxt" ShowRightDivider="true" runat="server" />
							<a href="../Assets/AssetList.aspx" class="BodyTxt">view asset list</a>
							<span class="BodyTxt">|</span>
							<a href="#" class="BodyTxt">help</a>
						</div>
					</div>
				</div>
			</div>
			<app:FeedbackLabel ID="MessageLabel" UseContainer="true" runat="server" />
			<div class="AdmOrdBkg">
				<div class="Left AdmOrdThbMrg">
					<div class="AdmOrdThbPanel">
						<app:AssetThumbnail ID="AssetThumbnail" HighlightIfRestricted="false" runat="server" />
						<br />
						<div class="ImgIcons">
							<app:AssetButtons ID="AssetButtons1" ShowButtons="Info" runat="server" />
						</div>
					</div>
				</div>
				<div class="Right LbxThbMrg">
					<table width="450" border="0" cellspacing="0" cellpadding="2" class="TblOutline">
					<tr> 
						<td class="TblCell2 BodyTxt Bold alignRight Top">Asset Type</td>
						<td class="TblCell1 BodyTxt Top" ID="AssetTypeCell" runat="server">[Asset Type]</td>
					</tr>
					
					<app:MultiBrandPlaceHolder runat="server">
					<tr> 
						<td class="TblCell2 BodyTxt Bold alignRight Top">Brand</td>
						<td class="TblCell1 BodyTxt Top" ID="BrandCell" runat="server">[Brand]</td>
					</tr>
					</app:MultiBrandPlaceHolder>
					
					<tr> 
						<td class="TblCell2 BodyTxt Bold alignRight Top">Short Description</td>
						<td class="TblCell1 BodyTxt Top" ID="ShortDescriptionCell" runat="server">[Short Description]</td>
					</tr>
					<tr> 
						<td class="TblCell2 BodyTxt Bold alignRight Top">Usage Restrictions</td>
						<td class="TblCell1 BodyTxt Top" ID="UsageRestrictionsCell" runat="server">[Usage Restrictions]</td>
					</tr>
					<tr> 
						<td class="TblCell2 BodyTxt Bold alignRight Top">Message from<br /><asp:Literal ID="InvitingUserName" runat="server" /></td>
						<td class="TblCell1 BodyTxt Top" ID="InvitingUserMessageCell" runat="server">[InvitingUserMessage]</td>
					</tr>
					<tr>
						<td class="TblCell2 BodyTxt alignRight Top W125"><span class="Bold">Your Response</span><br />Is visible to you<br>and other users in<br>the workflow</td>
						<td class="TblCell1 BodyTxt Top"><app:TextArea ID="ResponseTextArea" Rows="4" Columns="25" CssClass="formInput W310" Wrap="true" runat="server" /></td>
					</tr>
					</table>
					<div class="alignRight">
						<br />
						<br />
					</div>
				</div>
				<br />
				<div class="Clear"></div>
				<div class="AdmOrdResultsFtr"></div>
				<div class="alignRight">
					<br />
					<asp:Button ID="ProcessButton" CssClass="button" Text="submit" OnClick="ProcessButton_Click" runat="server" />
					<app:CancelButton ID="CancelButton" CssClass="button" Text="cancel" RedirectUrl="~/Admin/" runat="server" />
					<br />
					<br />
				</div>
				
				<br />
				
				<div class="alignCentre">
					<app:AssetWorkflowLog ID="AssetWorkflowLog" runat="server" />
				</div>
				
				<br />
				
			</div>
		</div>
	</div>
</asp:Content>