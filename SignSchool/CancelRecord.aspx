<%@ Page Title="Sense Sign School" Language="C#" MasterPageFile="~/SingSchool.Master" AutoEventWireup="true" CodeBehind="CancelRecord.aspx.cs" Inherits="SignSchool.CancelRecord" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="PageTitle">Cancel Record</h1> 
    <br />
    <asp:Table id="DataEditTable" runat="server" CellPadding="5" GridLines="None" BackColor="#E3EAEB" BorderColor="#E6E2D8" BorderPadding="4" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#333333" TextLayout="TextOnTop" HorizontalAlign="Center">
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label runat="server" ID="ConsId_lbl" Text="Constituent Id:" ForeColor="Black"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                 <asp:TextBox ID="ConsId_txt" runat="server" Text="" ReadOnly="true"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label runat="server" ID="ad1_lbl" Text="Address Line 1:" ForeColor="Black"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                 <asp:TextBox ID="ad1_txt" runat="server" Text="" ReadOnly="true"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label runat="server" ID="Title_lbl" Text="Title:" ForeColor="Black"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                 <asp:TextBox ID="Title_txt" runat="server" Text="" ReadOnly="true"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label runat="server" ID="ad2_lbl" Text="Address Line 2:" ForeColor="Black"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                 <asp:TextBox ID="ad2_txt" runat="server" Text="" ReadOnly="true"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label runat="server" ID="Fname_lbl" Text="First Name:" ForeColor="Black"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                 <asp:TextBox ID="Fname_txt" runat="server" Text="" ReadOnly="true"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label runat="server" ID="ad3_lbl" Text="Address Line 3:" ForeColor="Black"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                 <asp:TextBox ID="ad3_txt" runat="server" Text="" ReadOnly="true"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label runat="server" ID="Sname_lbl" Text="Surname:" ForeColor="Black"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                 <asp:TextBox ID="Sname_txt" runat="server" Text="" ReadOnly="true"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label runat="server" ID="city_lbl" Text="City:" ForeColor="Black"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                 <asp:TextBox ID="city_txt" runat="server" Text="" ReadOnly="true"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>

        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label runat="server" ID="email_lbl" Text="Email:" ForeColor="Black"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                 <asp:TextBox ID="email_txt" runat="server" Text="" ReadOnly="true"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label runat="server" ID="county_lbl" Text="County:" ForeColor="Black"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                 <asp:TextBox ID="county_txt" runat="server" Text="" ReadOnly="true"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label runat="server" ID="regdate_lbl" Text="Registration Date:" ForeColor="Black"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                 <asp:TextBox ID="regdate_txt" runat="server" Text="" ReadOnly="true"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label runat="server" ID="pcode_lbl" Text="Post Code:" ForeColor="Black"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                 <asp:TextBox ID="pcode_txt" runat="server" Text="" ReadOnly="true"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label runat="server" ID="RecordId_lbl" Text="Record Id:" ForeColor="Black"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                 <asp:TextBox ID="RecordId_txt" runat="server" Text="" ReadOnly="true"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label runat="server" ID="country_lbl" Text="Country:" ForeColor="Black"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                 <asp:TextBox ID="country_txt" runat="server" Text="" ReadOnly="true"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            
        </asp:TableRow>
        <asp:TableFooterRow>
             <asp:TableCell ColumnSpan="4" HorizontalAlign="Center">
                <asp:Button ID="SaveButton" runat="server" OnClick="SaveButton_Click" Text="Cancel Record" />
             </asp:TableCell>
        </asp:TableFooterRow>
    </asp:Table>

</asp:Content>


