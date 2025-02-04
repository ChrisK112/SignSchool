<%@ Page Title="Sense Sign School" Language="C#" MasterPageFile="~/SingSchool.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="SignSchool.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="PageTitle">Home</h1> 
    
    <br />

    <asp:Table id="NextPrintInfo" runat="server" CellPadding="5" GridLines="None" Font-Names="Verdana" Font-Size="0.8em" TextLayout="TextOnTop" >
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="DateLabel" runat="server" Text="Next Date To Print:"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label ID="DateLabelDATE" runat="server" Text=""></asp:Label>
             </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="TotalLabel" runat="server" Text="Total Labels:"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label ID="TotalLabelTOTAL" runat="server" Text=""></asp:Label>
             </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow><asp:TableCell></asp:TableCell></asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2">
                <asp:Label runat="server" ID="lblMessage" Text="Summary:" ForeColor="Black" Font-Bold="true"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    

    <div style="max-height:70vh;overflow-y:auto;overflow-x:auto;text-align: center;">
        <asp:GridView ID="GridView_NextPrint" runat="server"></asp:GridView>
    </div>


</asp:Content>

