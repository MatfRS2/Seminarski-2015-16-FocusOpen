<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MetadataFilters.ascx.cs" Inherits="FocusOPEN.Website.Controls.MetadataFilters" %>

<table width="760" border="0" cellspacing="0" cellpadding="3">
    <tr>
        <td class="Top" style="width: 405px">
            
            
            <asp:Literal ID="TemplateHtmlLiteral" runat="server" Text=""></asp:Literal>
            
            <asp:PlaceHolder ID="TemporaryMetaControlsPlaceHolder" runat="server"></asp:PlaceHolder>
                        
        </td>
        <td class="Top SrcFltrBkg2">

				<span class="PanelTxt Bold">file size</span><br /> 
				<asp:DropDownList ID="FileSizeDropDownList" CssClass="formInput W75" runat="server">
					<asp:ListItem Value="any">All</asp:ListItem>
					<asp:ListItem Value="LessThan">Less than</asp:ListItem>
					<asp:ListItem Value="MoreThan">Greater than</asp:ListItem>
				</asp:DropDownList>
				<asp:TextBox ID="FileSizeTextBox" CssClass="formInput W75" MaxLength="8" runat="server" />
				<span class="PanelTxt">(MB)</span>
				
				<br /><br />
				
				<span class="PanelTxt"><span class="Bold">orientation:</span> image</span><br />
				<asp:DropDownList ID="OrientationDropDownList" CssClass="formInput W75" runat="server">
					<asp:ListItem Value="all">All</asp:ListItem>
					<asp:ListItem Value="landscape">Landscape</asp:ListItem>
					<asp:ListItem Value="portrait">Portrait</asp:ListItem>
					<asp:ListItem Value="square">Square</asp:ListItem>
				</asp:DropDownList>

			</td> 
			<td class="Top">
				
					<app:MetadataVisibilityWrapper ID="MetadataVisibilityWrapper1" BrandDropDownListId="BrandDropDownList1" FieldName="PublicationDate" Section="AssetForm" runat="server">
						<span class="PanelTxt"><span class="Bold">date:</span> from</span><br />
						<app:DayDropDownList ID="FromDayDropDownList" CssClass="formInput" BlankText="DD" runat="server" />
						<app:MonthDropDownList ID="FromMonthDropDownList" Abbreviated="true" BlankText="MM" CssClass="formInput" runat="server" />
						<app:YearDropDownList ID="FromYearDropDownList" CssClass="formInput" BlankText="YYYY" runat="server" />
						<br /> 
					</app:MetadataVisibilityWrapper>
					
					<app:MetadataVisibilityWrapper ID="MetadataVisibilityWrapper2" BrandDropDownListId="BrandDropDownList1" FieldName="ExpiryDate" Section="AssetForm" runat="server">
						<span class="PanelTxt"><span class="Bold">date:</span> to</span><br /> 
						<app:DayDropDownList ID="ToDayDropDownList" CssClass="formInput" BlankText="DD" runat="server" />
						<app:MonthDropDownList ID="ToMonthDropDownList" Abbreviated="true" BlankText="MM" CssClass="formInput" runat="server" />
						<app:YearDropDownList ID="ToYearDropDownList" CssClass="formInput" BlankText="YYYY" runat="server" />
						<br /> 
					</app:MetadataVisibilityWrapper>

			</td> 
    </tr>
    <tr>
        <td colspan="2">
            <asp:CheckBox ID="IncludeUnpublishedExpiredAssets" Text="Include unpublished &amp; expired assets"
                CssClass="BodyTxt" runat="server" />
        </td>
        <td align="right">
        
            <asp:Button ID="SearchButton2" CssClass="button" Text="search" OnClick="SearchButton_Click"
                runat="server" />
                
            <asp:Button ID="ResetSearchButton2" CssClass="button" Text="reset" OnClick="ResetSearchButton_Click"
                runat="server" />
                
        </td>
    </tr>
</table>