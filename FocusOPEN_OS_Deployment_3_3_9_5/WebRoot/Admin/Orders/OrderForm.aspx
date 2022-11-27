<%@ Page Title="View Order [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="OrderForm.aspx.cs" Inherits="FocusOPEN.Website.Admin.OrderForm" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg">    
		<div class="AdmTblPanel Clear">
			<div class="Left">
				<div class="AdmAppHdr">Order Details</div>
				<app:FeedbackLabel ID="MessageLabel1" UseContainer="true" runat="server" />
				<div class="AppResults PanelTxt">total in this order: <span class="Bold"><%=RecordCount%></span> assets [displaying page <%=CurrentPage%> of <%=TotalPages%>]</div>
			</div>
			<div class="Right">
				<br />
				<div class="AppPageView">
					<span class="PanelTxt">show</span>
					<app:PageSizeDropDownList ID="PageSizeDropDownList1" Mode="Admin" CssClass="formInput" OmitBlankItem="true" AutoPostBack="true" OnSelectedIndexChanged="PageSizeDropDownList1_SelectedIndexChanged" runat="server" />
					<div class="AppPaging">
						<app:SimplePager ID="TopPager" OnCommand="SimplePager_OnCommand" runat="server" />
					</div>
				</div>
			</div>
			<div class="Clear">
				<br />
				<div class="AdmOrdNotesPanel"> 
					<div class="NotesMrg"> 
						<span class="BodyTxt"> 
							<span class="Bold">Order Ref # </span><asp:Label ID="OrderRefLabel" runat="server" /> <br /> 
							<span class="Bold">Placed By: </span><asp:Label ID="PlacedByUserNameLabel" runat="server" /><br />
							<app:MultiBrandPlaceHolder runat="server">
								<span class="Bold">Brand: </span><asp:Label ID="PlacedByUserBrandName" runat="server" /><br />
							</app:MultiBrandPlaceHolder>
							<span class="Bold">Company: </span><asp:Label ID="PlacedByUserCompanyName" runat="server" />
						</span>
						<br />
						<div class="alignRight"><a href="OrderList.aspx" class="BodyTxt Bold">&laquo; View Orders List</a> <span class="BodyTxt">|</span> <app:HelpHyperLink ID="HelpHyperLink1" HelpPage="7_Manage_Orders.htm#order_approval" CssClass="BodyTxt" Text="help" runat="server" /></div>
					</div>
				</div>
			</div>
			<div class="AdmOrdBkg">
			
				<app:SortableRepeater ID="OrderItemsRepeater" OnItemDataBound="OrderItemsRepeater_ItemDataBound" runat="server">
					<ItemTemplate>
						<asp:HiddenField ID="OrderItemIdHiddenField" runat="server" />
						<div class="Left AdmOrdThbMrg">
							<div class="AdmOrdThbPanel"> 
								<app:AssetThumbnail ID="AssetThumbnail1" UseMarginCssClass="false" runat="server" />
								<br />
								<div class="ImgIcons">
									<app:AssetButtons ID="AssetButtons1" ShowButtons="Info" runat="server" />
								</div>
							</div>
						</div>
						<div class="Right LbxThbMrg">
							<table width="450" border="0" cellspacing="0" cellpadding="2" class="TblOutline">
							<tr> 
								<td class="TblCell2 BodyTxt Bold alignRight Top W125">Reference Number</td>
								<td class="TblCell1 BodyTxt Top"><asp:Label ID="AssetIdLabel" runat="server" /></td>
							</tr>
							<tr> 
								<td class="TblCell2 BodyTxt Bold alignRight Top">Short Description</td>
								<td class="TblCell1 BodyTxt Top"><asp:Label ID="AssetDescriptionLabel" runat="server" /></td>
							</tr>
							<tr> 
								<td class="TblCell2 BodyTxt Bold alignRight Top">Brand</td>
								<td class="TblCell1 BodyTxt Top"><asp:Label ID="BrandNameLabel" runat="server" /></td>
							</tr>
								<td class="TblCell2 BodyTxt Bold alignRight Top">Asset Type</td>
								<td class="TblCell1 BodyTxt Top"><asp:Label ID="AssetTypeLabel" runat="server" /></td>
							</tr>
							<tr> 
								<td class="TblCell2 BodyTxt Bold alignRight Top">Usage Restrictions</td>
								<td class="TblCell1 BodyTxt Top"><asp:Label ID="AssetUsageRestrictionsLabel" runat="server" /></td>
							</tr>
							<tr> 
								<td class="TblCell2 BodyTxt Bold alignRight Top">Deadline Date</td>
								<td class="TblCell1 BodyTxt Top"><asp:Label ID="DeadlineDateLabel" runat="server" /></td>
							</tr>
							<tr>
								<td class="TblCell2 BodyTxt Bold alignRight Top W125">Users Notes</td>
								<td class="TblCell1 BodyTxt Top"><asp:Label ID="UserNotesLabel" runat="server" /></td>
								</tr>
								<tr id="AssignedToRow" visible="false" runat="server">
									<td class="TblCell2 BodyTxt Bold alignRight Top W125">Assigned To</td>
									<td class="TblCell1 BodyTxt Top"><asp:Label ID="AssignedToLabel" runat="server" /></td>
								</tr>
							<tr>
								<td class="TblCell2 BodyTxt Bold alignRight Top W125">Conversation:</td>
								<td class="TblCell1 BodyTxt Top">
									<asp:Repeater ID="ConversationRepeater" OnItemDataBound="ConversationRepeater_ItemDataBound" runat="server">
										<ItemTemplate>
											<span class="PosTxt">
												<asp:Label ID="UserNameLabel" CssClass="BodyTxt PosTxt" runat="server" /> said on <asp:Label ID="DateLabel" CssClass="BodyTxt PosTxt" runat="server" />:<br />
												<asp:Label ID="CommentTextLabel" CssClass="BodyTxt PosTxt" runat="server" />
											</span>
										</ItemTemplate>
										<AlternatingItemTemplate>
											<asp:Label ID="UserNameLabel" CssClass="BodyTxt" runat="server" /> said on <asp:Label ID="DateLabel" CssClass="BodyTxt" runat="server" />:<br />
											<asp:Label ID="CommentTextLabel" CssClass="BodyTxt" runat="server" />
										</AlternatingItemTemplate>
										<SeparatorTemplate>
											<br />
											<br />
										</SeparatorTemplate>
									</asp:Repeater>
									<asp:Label ID="NoConversationLabel" Visible="false" CssClass="BodyTxt" Text="To start the conversation, enter a comment below, keeping the status as 'awaiting approval' and click 'process order'." runat="server" />
								</td>
							</tr>
							<tr id="AddCommentRow" runat="server"> 
								<td class="TblCell2 BodyTxt alignRight Top W125"><span class="Bold">Your Response</span><br />Is visible to you and the user who placed this order</td>
								<td class="TblCell1 BodyTxt Top">
									<app:TextArea ID="CommentsTextBox" Columns="25" Rows="5" CssClass="formInput W310" MaxLength="500" runat="server" />
								</td>
							</tr>
							</table>
							<div class="alignRight">
								<br />
								<span class="BodyTxt Bold">
									<asp:RadioButtonList ID="OrderItemStatusRadioButtonList" RepeatDirection="horizontal" RepeatLayout="flow" runat="server">
										<asp:ListItem Value="3" Text="Approved" />
										<asp:ListItem Value="4" Text="Rejected" />
										<asp:ListItem Value="2" Text="Awaiting Approval" />
									</asp:RadioButtonList>
									<app:FeedbackLabel ID="OrderItemStatusMessageLabel" Visible="false" Pinned="true" runat="server" />
								</span>
								<br />
								<br />
							</div>
						</div>
						<br />
						<div class="Clear"></div>
						<div class="AdmOrdResultsFtr"></div>
					</ItemTemplate>
				</app:SortableRepeater>
				
				<asp:Panel ID="NoResultsPanel" Visible="false" runat="server">
					<div class="noResultsAdm AppCtrlMrg">
						<app:FeedbackLabel ID="NoResultsMessageLabel" MessageType="Negative" Header="no results found" Pinned="true" Visible="true" runat="server" />
						<p class="BodyTxt">This order does not contain any assets requiring approval.</p> 
					</div>
				</asp:Panel>
				
				<div id="ButtonPanel" runat="server" class="alignRight">
					<br />
					<asp:Button ID="ProcessButton" Text="process order" CssClass="button" OnClick="ProcessButton_Click" runat="server" />
					<app:CancelButton ID="CancelButton1" CssClass="button" Text="cancel" Prompt="Are you sure you want to cancel?  Changes will not be saved" RedirectUrl="OrderList.aspx" runat="server" />
					<br />
					<br />
				</div>
			</div>
		</div>
	</div>
	<div class="AdmAppFtr">
		<div class="Left">
			<div class="AppResults PanelTxt">total in this order: <span class="Bold"><%=RecordCount%></span> assets [displaying page <%=CurrentPage %> of <%=TotalPages %>]</div>
		</div>
		<div class="Right">
			<div class="AppPageView AppPaging">
				<app:SimplePager ID="BottomPager" OnCommand="SimplePager_OnCommand" runat="server" />
			</div>
		</div>
	</div>
</asp:Content>