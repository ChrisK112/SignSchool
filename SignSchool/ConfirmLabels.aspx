<%@ Page Title="Sense Sign School" Language="C#" MasterPageFile="~/SingSchool.Master" AutoEventWireup="true" CodeBehind="ConfirmLabels.aspx.cs" Inherits="SignSchool.WebForm4" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="PageTitle">Confirm Labels</h1> 
    <br />

    <div ID="middle_div" Style="text-align: center" >

        <asp:Table runat="server" Style="text-align: center" CellPadding="5" GridLines="None" HorizontalAlign="Center">
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2">
                    <asp:Label runat="server" Text="Lables merged and checked?"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2">
                    <asp:Label ID="CountLabel" runat="server" Text="" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2">
                    <asp:CheckBox ID="LabelCheckbox" runat="server" OnCheckedChanged="LabelCheckbox_CheckedChanged" AutoPostBack="true" Checked="false"/>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell Width ="50%">
                     <asp:Button ID="ConfirmLabelsButton" runat="server" OnClick="ConfirmLabelsButton_Click" Text="Confirm Labels" Style="display:none"/>
                </asp:TableCell>
                <asp:TableCell Width ="50%">
                     <asp:Button ID="UndoLabelButton" runat="server" OnClick="UndoLabelButton_Click" Text="Undo Confirmation" AutoPostBack="true" Enabled="false" Style="display:none"/>
                 </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2">
                    <asp:Label ID="ProgressLabel" runat="server" Text="" Style="display:none"/>
                </asp:TableCell></asp:TableRow>

        </asp:Table><br />

        <asp:GridView ID="GridView1" HorizontalAlign="Center" runat="server" CssClass="mydatagrid" PagerStyle-CssClass="pager"
            HeaderStyle-CssClass="header" RowStyle-CssClass="rows" AllowPaging="True" PageSize=15 OnPageIndexChanging="datagrid_PageIndexChanging">
        </asp:GridView>


  
    </div>
    
</asp:Content>

