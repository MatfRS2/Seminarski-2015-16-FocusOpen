<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectableMetadataSettingDetails.ascx.cs" Inherits="FocusOPEN.Website.Controls.Admin.SelectableMetadataSettingDetails" %>

<a href="#" class="BodyTxt Bold" id="ExpandCollapseDetailsAnchor" style="margin-left: 30px;"><asp:Literal runat="server" ID="SignLiteral">[+]</asp:Literal> Show metadata catalogue selection options</a>
<br />
				
<div id="SelectableDetailsDiv" runat="server" style="display: none;" class="selectableLogicContainer">
    <br />
    <div class="PanelTxt" style="margin-left: 30px;">
        
        <app:FeedbackLabel ID="FeedbackLabel" UseContainer="false" Pinned="false" LineBreaks="1" runat="server" />
        
        <span class="Bold">Type</span>
        <br />
        <asp:DropDownList ID="TypeDropDownList" CssClass="formInput W75" runat="server" 
                          DataTextField="Value" DataValueField="Key" AutoPostBack="true" 
                          OnSelectedIndexChanged="TypeDropDownList_SelectedIndexChanged" 
                          Width="120px"
                        />
        
        <br />
        <br />
        
        <asp:Panel runat="server" ID="DepthPanel">   
        <span class="Bold">Selection depth</span>
        <br />
        <asp:DropDownList ID="DepthDropDownList" CssClass="formInput W75" runat="server" Width="45px" />
        
        <br />
        <br />
        </asp:Panel>
        
        <asp:CheckBox ID="LinearCheckBox" runat="server" 
                          Text="Linear presentation (non-hierarchical)" 
                          AutoPostBack="true" 
                          OnCheckedChanged="LinearCheckBox_OnCheckedChanged" /> 
        <br />
        <asp:RadioButtonList id="SortTypeRadioButtonList" runat="server" DataTextField="Value" CssClass="PanelTxt" DataValueField="Key" RepeatDirection="Horizontal" />
        
        <asp:CheckBox ID="AllowMultipleCheckbox" runat="server" Text="Allow multiple selections" />
        <br />
        <br />
        
        <asp:Panel runat="server" ID="LayoutOptionsPanel">
        <span class="Bold">Layout options</span><br />
        (applies to checkboxes and radio buttons only)  
        <br />
        <br />
        <asp:RadioButtonList id="OrderTypeRadioButtonList" runat="server" DataTextField="Value" CssClass="PanelTxt" DataValueField="Key" RepeatDirection="Horizontal" />
        <br />
        <asp:DropDownList ID="ColumnsDropDownList" CssClass="formInput W75" runat="server" Width="45px" /> columns/rows
        </asp:Panel>
        
    </div>
				
</div>