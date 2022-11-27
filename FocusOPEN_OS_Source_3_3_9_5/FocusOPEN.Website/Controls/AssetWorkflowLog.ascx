<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssetWorkflowLog.ascx.cs" Inherits="FocusOPEN.Website.Controls.AssetWorkflowLog" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>

<script type="text/javascript">
	function toggleComments(linkId, commentsId)
	{
		var link = document.getElementById(linkId);
		var comment = document.getElementById(commentsId);
		
		if (comment.style.display)
		{
			link.innerHTML = 'hide comments [-]';
			comment.style.display = '';
		}
		else
		{
			link.innerHTML = 'show comments [+]';
			comment.style.display = 'none';
		}
	}
</script>
<asp:Repeater ID="WorkflowInfoRepeater" OnItemDataBound="WorkflowInfoRepeater_ItemDataBound" runat="server">
	<ItemTemplate>
		
		<asp:PlaceHolder ID="PendingPlaceHolder" Visible="false" runat="server">
			<app:IconImage ID="PendingIconImage" Filename="fbkPending.gif" AlternateText="[pending]" runat="server"/>
			<span class="FbkNullTxt">Pending approval from <app:EmailHyperLink ID="PendingUserHyperLink" CssClass="Black" runat="server" /></span>
			<span class="BodyTxt">[stage <asp:Literal ID="PendingCurrentPositionLiteral" Text="X" runat="server" /> of <asp:Literal ID="PendingTotalPositionsCountLiteral" Text="Y" runat="server" />]</span>
			<span class="FbkNullTxt"><app:EnhancedHyperLink ID="PendingShowCommentsHyperLink" CssClass="Black" Prefix="&nbsp;" runat="server">show comments [+]</app:EnhancedHyperLink></span>
			<br />
			<div id="PendingCommentText" runat="server" style="display:none;" class="W410 alignLeft BodyTxt">[Comments]</div>
		</asp:PlaceHolder>
		
		<asp:PlaceHolder ID="WaitingPlaceHolder" Visible="false" runat="server">
			<app:IconImage ID="WaitingIconImage" Filename="fbkNext.gif" AlternateText="[waiting]" runat="server"/>
			<span class="NullTxt">Next workflow user <app:EmailHyperLink ID="WaitingUserHyperLink" CssClass="Black" runat="server" /></span>
			<span class="BodyTxt">[stage <asp:Literal ID="WaitingCurrentPositionLiteral" Text="X" runat="server" /> of <asp:Literal ID="WaitingTotalPositionsCountLiteral" Text="Y" runat="server" />]</span>
			<br />
		</asp:PlaceHolder>
		
		<asp:PlaceHolder ID="ApprovedPlaceHolder" Visible="false" runat="server">
			<app:IconImage ID="ApprovedIconImage" Filename="fbkPositive.gif" AlternateText="[positive]" runat="server"/>
			<span class="FbkPosTxt">
				Approved by <app:EmailHyperLink ID="ApprovedUserHyperLink" CssClass="Green" runat="server" /> on <asp:Literal ID="ApprovedDateLiteral" runat="server" />
				<app:EnhancedHyperLink ID="ApprovedShowCommentsHyperLink" CssClass="Black" Prefix="&nbsp;" runat="server">show comments [+]</app:EnhancedHyperLink>
				<br />
			</span>
			<div id="ApprovedCommentText" runat="server" style="display:none;" class="W410 alignLeft BodyTxt">[Comments]</div>
		</asp:PlaceHolder>
		
		<asp:PlaceHolder ID="RejectedPlaceHolder" Visible="false" runat="server">
			<app:IconImage ID="RejectedIconImage" Filename="fbkNegative.gif" AlternateText="[rejected]" runat="server"/>
			<span class="FbkNegTxt">
				Rejected by <app:EmailHyperLink ID="RejectedUserHyperLink" CssClass="Red" runat="server" /> on <asp:Literal ID="RejectedDateLiteral" runat="server" />
				<app:EnhancedHyperLink ID="RejectedShowCommentsHyperLink" CssClass="Black" Prefix="&nbsp;" runat="server">show comments [+]</app:EnhancedHyperLink>
				<br />
			</span>
			<div id="RejectedCommentText" runat="server" style="display:none;" class="W410 alignLeft BodyTxt">[Comments]</div>
		</asp:PlaceHolder>
		
		<asp:PlaceHolder ID="EmptyCommentsPlaceHolder" Visible="false" runat="server">
			<app:IconImage ID="EmptyCommentsIconImage" Filename="fbkComments.gif" AlternateText="" runat="server"/>
			<span class="Bold BodyTxt">
				<app:EmailHyperLink ID="EmptyCommentsInvitedUserHyperLink" CssClass="Black" runat="server" /> invited by <app:EmailHyperLink ID="EmptyCommentsInvitingUserHyperLink" CssClass="Black" runat="server" /> on <asp:Literal ID="EmptyCommentsDateLiteral" runat="server" />
				<app:EnhancedHyperLink ID="EmptyCommentsShowCommentsHyperLink" CssClass="Black" Prefix="&nbsp;" runat="server">show message [+]</app:EnhancedHyperLink>
				<br />
			</span>
			<div id="EmptyCommentsCommentText" runat="server" style="display:none;" class="W410 alignLeft BodyTxt">[Message]</div>
		</asp:PlaceHolder>
		
		<asp:PlaceHolder ID="CommentsPlaceHolder" Visible="false" runat="server">
			<app:IconImage ID="CommentsIconImage" Filename="fbkComments.gif" AlternateText="" runat="server"/>
			<span class="Bold BodyTxt">
				Comments by <app:EmailHyperLink ID="CommentsUserHyperLink" CssClass="Black" runat="server" /> on <asp:Literal ID="CommentsDateLiteral" runat="server" />
				<app:EnhancedHyperLink ID="CommentsShowCommentsHyperLink" CssClass="Black" Prefix="&nbsp;" runat="server">show comments [+]</app:EnhancedHyperLink>
				<br />
			</span>
			<div id="CommentsCommentText" runat="server" style="display:none;" class="W410 alignLeft BodyTxt">
				Message from: <asp:Literal ID="CommentsInviterNameLiteral" runat="server" />:<br />
				<asp:Literal ID="CommentsInviterMessageLiteral" runat="server" />
				<br /><br />
				Response from <asp:Literal ID="CommentsInvitedUserNameLiteral" runat="server" />:<br />
				<asp:Literal ID="CommentsCommentTextLiteral" runat="server">[Comments]</asp:Literal>
			</div>
		</asp:PlaceHolder>
		
	</ItemTemplate>
</asp:Repeater>