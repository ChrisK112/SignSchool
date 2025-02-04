<%@ Page Title="Sense Sign School" Language="C#" MasterPageFile="~/SingSchool.Master" AutoEventWireup="true" CodeBehind="BrowseLiveData.aspx.cs" Inherits="SignSchool.BrowseLiveData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="PageTitle">Browse Live Data</h1> 
    <br />
    <asp:Table id="SearchBox" runat="server" CellPadding="5" GridLines="None" Font-Names="Verdana" Font-Size="0.8em" TextLayout="TextOnTop" HorizontalAlign="Center">
        <asp:TableRow>
            <asp:TableHeaderCell></asp:TableHeaderCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell>
                <asp:Label ID="DateLabel1" runat="server" Text="between" Visible="false"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell><asp:Label runat="server" Text="Search By:"></asp:Label></asp:TableHeaderCell>
            <asp:TableCell>
                <asp:DropDownList ID="SearchDropdownList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="SearchDropdownList_SelectedIndexChanged">
                        <asp:ListItem Value="All" Text="All"></asp:ListItem>
                        <asp:ListItem Value="Constituent ID" Text="Constituent ID"></asp:ListItem>
                        <asp:ListItem Value="Surname" Text="Surname"></asp:ListItem>
                        <asp:ListItem Value="Preferred Postcode" Text="Post Code"></asp:ListItem>
                        <asp:ListItem Value="Pack Sent Previously" Text="Packs Sent"></asp:ListItem>
                        <asp:ListItem Value="Registration date" Text="Registration Date"></asp:ListItem>
                        <asp:ListItem Value="First fulfilment date" Text="First Fulfilment Date"></asp:ListItem>
                        <asp:ListItem Value="Date Imported" Text="Import Date"></asp:ListItem>
                        <asp:ListItem Value="Last Date Pack Changed" Text="Last Pack Sent Date"></asp:ListItem>
                    </asp:DropDownList>      
            </asp:TableCell>
            <asp:TableCell>
                 <asp:TextBox ID="SearchTextBox" runat="server" Text="" Enabled="false"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Button ID="SearchButton" runat="server" OnClick="SearchButton_Click" Text="Search"/>
             </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell></asp:TableHeaderCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell>
                <asp:Label ID="DateLabel2" runat="server" Text="and" Visible="false"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell></asp:TableHeaderCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="SearchTextBox2" runat="server" Text="" Visible="false" TextMode="Date"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:Label runat="server" ID="lblMessage" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
    <br />
    <br />
    <div style="max-height:65vh;overflow-y:scroll;overflow-x:auto;text-align: center;">
        <asp:GridView ID="GridView1" HorizontalAlign="Center" runat="server" CssClass="mydatagrid"
            HeaderStyle-CssClass="header" RowStyle-CssClass="rows" AllowPaging="False" >
        </asp:GridView>
    </div>
        
</asp:Content>

