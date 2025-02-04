<%@ Page Title="Sense Sign School" Language="C#" MasterPageFile="~/SingSchool.Master" AutoEventWireup="true" CodeBehind="CancelRecordSearch.aspx.cs" Inherits="SignSchool.CancelRecordSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="PageTitle">Search Records</h1> 
    <br />
    <asp:Table id="SearchBox" runat="server" CellPadding="5" GridLines="None" Font-Names="Verdana" Font-Size="0.8em" TextLayout="TextOnTop" HorizontalAlign="Center">
        <asp:TableRow>
            <asp:TableCell>
                <asp:DropDownList ID="SearchDropdownList" runat="server" AutoPostBack="true">
                    <asp:ListItem Value="Constituent ID" Text="Constituent ID"></asp:ListItem>
                    <asp:ListItem Value="Registration date" Text="Registration date"></asp:ListItem>
                    <asp:ListItem Value="Surname" Text="Surname"></asp:ListItem>
                    <asp:ListItem Value="Preferred Address Line 1" Text="Address Line 1"></asp:ListItem>
                    <asp:ListItem Value="Preferred Postcode" Text="Post Code"></asp:ListItem>
                    <asp:ListItem Value="Email Number" Text="Email"></asp:ListItem>   
                </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell>
                 <asp:TextBox ID="SearchTextBox" runat="server" Text=""></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Button ID="SearchButton" runat="server" OnClick="SearchButton_Click" Text="Search"/>
             </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:Label runat="server" ID="lblMessage" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
    <br /><br />
    <div style="max-height:70vh;overflow-y:scroll;overflow-x:auto;text-align: center;">
        <asp:GridView ID="GridView_EditRecord" HorizontalAlign="Center" runat="server" CssClass="mydatagrid"
            HeaderStyle-CssClass="header" RowStyle-CssClass="rows_editgrid" AllowPaging="False" AutoGenerateEditButton="true"
            OnRowEditing="GridView_EditRecord_RowEditing">
        </asp:GridView>
    </div>

</asp:Content>


