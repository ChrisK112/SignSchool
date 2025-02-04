<%@ Page Title="Sense Sign School" Language="C#" MasterPageFile="~/SingSchool.Master" AutoEventWireup="true" CodeBehind="DownloadLabels.aspx.cs" Inherits="SignSchool.WebForm3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="PageTitle">Download Labels</h1> 
    <br />
    
    <div ID="middle_div" Style="text-align: center" >
        <asp:Table runat="server" Style="text-align: center" CellPadding="5" GridLines="None" HorizontalAlign="Center">
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2">
                    <asp:Label ID="CountLabel" runat="server" Text="Placeholder" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell Width ="70%">
                    <asp:Button ID="DLLabels" runat="server" OnClick="DLLabels_Click" Text="Download Labels" Enabled="false" />
                </asp:TableCell>
                <asp:TableCell Width ="30%">
                        <asp:Button ID="RefreshButton" runat="server" OnClick="RefreshButton_Click" Text="Refresh" AutoPostBack="true"/>
                 </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
</asp:Content>

