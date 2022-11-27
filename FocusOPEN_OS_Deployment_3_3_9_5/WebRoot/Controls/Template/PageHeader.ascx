<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageHeader.ascx.cs" Inherits="FocusOPEN.Website.Controls.PageHeader" %>
<%@ Register Assembly="FocusOPEN.Website" Namespace="FocusOPEN.Website.Controls" TagPrefix="app" %>
<%@ Register Src="~/Controls/Template/MetadataFilters.ascx" TagName="MetaFilters" TagPrefix="metafilters" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="aspajax" %>

<div class="CatHdrPanel"></div>
<div class="CatNavPanel"><app:NavigationBar ID="NavigationBar1" runat="server" /></div>
<div class="Clear"></div>
<div class="CatSrcHdr Clear">
    <div class="Left SrcHdrMrg"><span class="SrcHdrTxt Bold">Search</span></div>
    <div class="Right SrcHdrMrg"><app:HelpHyperLink ID="HelpHyperLink1" HelpPage="2_Searching.htm#search_tips" CssClass="SrcHelpTxt" Text="search tips" ToolTip="Not getting the results you want? Click here to find out how." runat="server" /></div>
</div>
<div class="Clear"></div>
<aspajax:UpdatePanel ID="UpdatePanel1" ChildrenAsTriggers="true" UpdateMode="Conditional" runat="server">
    <ContentTemplate>                  
		<%--Search Header--%>	
		<asp:Panel ID="SearchPanel" runat="server">
		    <div class="CatSrcPanel">
		        <div class="SrcPanelControls Left" style="width:550px"> 
				    <asp:TextBox ID="KeywordsTextBox" CssClass="formInput W100" MaxLength="150" runat="server" />

					<app:MultiBrandPlaceHolder runat="server">
						<span class="PanelTxt Bold" style="margin-left:15px">brand</span>
						<app:BrandDropDownList ID="BrandDropDownList1" Restrict="true" BlankText="All" BlankValue="0" CssClass="formInput W125" AutoPostBack="true" OnSelectedIndexChanged="BrandDropDownList1_SelectedIndexChanged" runat="server" />
					</app:MultiBrandPlaceHolder>
					
					<span class="PanelTxt Bold" style="margin-left:15px">media</span>
					<app:AssetTypeDropDownList ID="AssetTypeDropDownList1" AutoPostBack="true" OnSelectedIndexChanged="AssetTypeDropDownList1_SelectedIndexChanged" CssClass="formInput" BlankText="All"  BlankValue="0" runat="server" />
				    
				    <asp:Button ID="SearchButton1" CssClass="button" Text="Go" runat="server" OnClick="SearchButton_Click" />
				    <asp:Button ID="ResetSearchButton1" CssClass="button" Text="Reset" OnClick="ResetSearchButton_Click" runat="server" />
				    
			    </div> 
				<div class="Right SrcPanelMrg">
					<asp:PlaceHolder ID="FilterSearchButtonWrapper" runat="server">
						<asp:LinkButton ID="FilterResultsLinkButton" CssClass="BodyTxt Bold" Text="show filters [+]" OnClick="FilterResultsLinkButton_Click" runat="server" />					
						<span class="BodyTxt">|</span>
					</asp:PlaceHolder>
					<asp:PlaceHolder ID="CategorySearchButtonWrapper" runat="server">
						<asp:LinkButton ID="CategoriesLinkButton" CssClass="BodyTxt Bold" Text="show categories [+]" OnClick="CategoriesLinkButton_Click" runat="server" />
					</asp:PlaceHolder>
				</div>
			</div>
		</asp:Panel>
        <asp:Panel ID="CategoryNavigationPanel" runat="server" Visible="false">
		    <div class="CatWrapper">
			    <div class="CatSrcAllPanel">			    
					<div class="Clear"></div> 
					<br />                        
                    <asp:Panel ID="CategorySearchPanel" runat="server">
                        
                        <asp:PlaceHolder ID="CategoryHeaderPlaceHolder" runat="server">
							<div style="margin-left:5px;">
								<asp:Repeater ID="BreadcrumbRepeater" OnPreRender="BreadcrumbRepeater_PreRender" OnItemCommand="BreadcrumbRepeater_ItemCommand" runat="server">
									<ItemTemplate>
										<asp:LinkButton runat="server" ID="BreadcrumbLinkButton" Text='<%# Eval("Name") %>' CommandArgument='<%# Eval("CategoryId") %>' CommandName="SelectCategory" CssClass="BodyTxt Bold"/>
									</ItemTemplate>
									<SeparatorTemplate>
										<span class="BodyTxt Bold" style="padding:0px 2px 0px 2px">&raquo;</span>
									</SeparatorTemplate>
								</asp:Repeater>
							</div>
							<div id="CategoryNavigationMessageDiv" runat="server" style="margin-left:5px">
								<br />
								<asp:Label runat="server" ID="CategoryNavigationMessage" CssClass="BodyTxt" />
								<br /><br />
							</div>
                        </asp:PlaceHolder>
                        
						<div id="CategoryNameDiv" runat="server" style="margin-left:5px;">
							<asp:Label runat="server" ID="CategoryNameLabel" CssClass="BodyTxt Bold" />
							<br /> 
						</div>
                        
						<div id="SubCategoryWrapper" style="margin-left: 23px;margin-top:6px;">
							<asp:DataList runat="server" ID="SubCategoriesRepeater" RepeatDirection="Vertical" RepeatLayout="table" CellSpacing="0" OnItemCommand="SubCategoriesRepeater_ItemCommand" OnItemDataBound="SubCategoriesRepeater_ItemDataBound">
								<ItemStyle VerticalAlign="Top" Width="154" />
								<ItemTemplate>
									<li style="list-style-position:outside;line-height:1.6em;font-size:10px;margin-bottom:4px;margin-left:20px">
										<asp:LinkButton runat="server" ID="SubCategoryLink" CommandArgument='<%# Eval("CategoryId") %>' CommandName="SelectCategory" CssClass="BodyTxt" />
										<asp:Label runat="server" ID="AssetsCountLabel" CssClass="BodyTxt" />
									</li>
								</ItemTemplate>
							</asp:DataList>
						</div>
						
						<br />
						
						<asp:Panel ID="ViewAllAssetsLinkButtonWrapper" style="padding:0px 0px 10px 10px" runat="server">
							<asp:LinkButton ID="ViewAllAssetsInCategoryLinkButton" OnClick="ViewAllAssetsInCategoryLinkButton_Click" Text="View all assets in this category &amp;raquo;" CssClass="BodyTxt" runat="server" />
                        </asp:Panel>
                        
                    </asp:Panel> 
		        </div>				
	        </div>
        </asp:Panel>   
		<%--Filters--%>
		<asp:Panel ID="FiltersPanel" Visible="false" runat="server">
			<div class="CatWrapper"> 
				<div class="CatSrcAllPanel"> 					
					<div class="Clear"></div> 
					<div class="FltrPanelControls PanelTxt Left">
						
						    <metafilters:MetaFilters ID="MetadataFilters" runat="server" />
						 
					</div> 
					<div class="Right"></div> 
					<div class="Clear"></div> 
				</div>
			</div>
		</asp:Panel>
	</ContentTemplate>
    <triggers>
		<aspajax:PostBackTrigger ControlID="SearchButton1" />
		<aspajax:PostBackTrigger ControlID="ResetSearchButton1" />
		<aspajax:PostBackTrigger ControlID="BreadcrumbRepeater" />
		<aspajax:PostBackTrigger ControlID="SubCategoriesRepeater"/>
	</triggers>
</aspajax:UpdatePanel>