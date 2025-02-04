<%@ Page Title="Sense Sign School" Language="C#" MasterPageFile="~/SingSchool.Master" AutoEventWireup="true" CodeBehind="ImportAddressData.aspx.cs" Inherits="SignSchool.ImportAddressData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="PageTitle">Import Address Data</h1> 
    <br />

    <asp:Table runat="server" Style="text-align: center" CellPadding="5" GridLines="None" HorizontalAlign="Center">
            <asp:TableRow>
                <asp:TableCell>
                   <asp:FileUpload ID="DataUpload" runat="server" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Button  runat="server" ID="btnUpload" Text="Upload" onclick="BtnUpload_Click" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2">
                    <asp:Label runat="server" ID="lblMessage" Text=""></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        <asp:TableRow>
                <asp:TableCell ColumnSpan="2">
                    <asp:Button ID="DownloadReportButton" runat="server" OnClick="DownloadReportButton_Click" Text="Download Report" Style="display:none"/>
                </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2">
                <asp:Button ID="ImportDataButton" runat="server" OnClick="ImportDataButton_Click" Text="Import Data" Style="display:none"/>
            </asp:TableCell>
         </asp:TableRow>
           

        </asp:Table>
    <br />
    <div style="text-align:center;">
        <asp:Label runat="server" ID="AddImportLabel" Text="Red: These records could not be matched to any database." Visible="false"></asp:Label>
    </div>

    <div style="max-height:70vh;overflow-y:scroll;overflow-x:auto;text-align:center;">
        <asp:GridView ID="GridView1" HorizontalAlign="Center" runat="server" CssClass="mydatagrid"
            HeaderStyle-CssClass="header" RowStyle-CssClass="rows" AllowPaging="False">
        </asp:GridView>
    </div>

</asp:Content>

