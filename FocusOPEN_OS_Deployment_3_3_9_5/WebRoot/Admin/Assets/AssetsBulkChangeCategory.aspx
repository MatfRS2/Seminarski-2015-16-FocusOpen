<%@ Page Title="Change asset category [ADMIN]" Language="C#" MasterPageFile="~/Templates/Website/AdminTemplate.Master" AutoEventWireup="true" CodeBehind="AssetsBulkChangeCategory.aspx.cs" Inherits="FocusOPEN.Website.Admin.Assets.AssetsBulkChangeCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="AdmBkg"> 
		<div class="Left AdmLeftPanel"> 
			<div class="AdmAppHdr">View Asset List</div>
			<app:FeedbackLabel ID="MessageLabel1" UseContainer="true" runat="server" /><br />
		</div>
		<div class="Right AdmRightPanel"></div>
		<div class="AdmTblPanel Clear">
			<div class="Clear">
				<div style="margin-left:10px;" class="BodyTxt">
					Select the source category below, and the target category with which it should be replaced.
					<br /><br />
					<table>
					<tr>
						<td class="BodyTxt Bold alignRight">Brand:</td>
						<td><app:BrandDropDownList ID="BrandDropDownList1" CssClass="formInput W180" AutoPostBack="true" OnSelectedIndexChanged="BrandDropDownList1_SelectedIndexChanged" runat="server" /></td>
					</tr>
					<tr>
						<td class="BodyTxt Bold alignRight">Source Category:</td>
						<td><app:NestedDataCategoryDropDownList ID="SourceCategoryDropDownList" CssClass="formInput W180" OnSelectedIndexChanged="SourceCategoryDropDownList_SelectedIndexChanged" AutoPostBack="true" runat="server" /></td>
					</tr>
					<tr>
						<td class="BodyTxt Bold alignRight">Target Category:</td>
						<td><app:NestedDataCategoryDropDownList ID="TargetCategoryDropDownList" CssClass="formInput W180" runat="server" /></td>
					</tr>
					<tr>
						<td>&nbsp;</td>
						<td><asp:CheckBox ID="ClearExistingCategoriesCheckBox" CssClass="clearcat formInput" Text="Clear existing categories that assets are assigned to" runat="server" /></td>
					</tr>
					<tr>
						<td>&nbsp;</td>
						<td><asp:CheckBox ID="DeleteSourceCategoryCheckBox" CssClass="deletesrc formInput" Text="Delete source category after reassignment (if empty)" runat="server" /></td>
					</tr>
					<tr>
						<td>&nbsp;</td>
						<td>
							<app:PromptButton ID="ReassignButton" Prompt="Are you sure you want to reassign the selected assets to this category?" Text="reassign assets" CssClass="button" OnClick="ReassignButton_Click" runat="server" />
							<app:CancelButton ID="CancelButton" Prompt="Are you sure? Assets will not be reassigned" RedirectUrl="AssetList.aspx" Text="cancel" CssClass="button" runat="server" />
						</td>
					</tr>
					</table>
				</div>
				<br />
				<app:AssetList ID="AssetList1" AllowDelete="false" runat="server" />
				<br />
			</div>
		</div>
	</div>
	<div class="AdmAppFtr"></div>
</asp:Content>