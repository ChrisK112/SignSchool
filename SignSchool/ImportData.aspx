<%@ Page Title="Sense Sign School" Language="C#" MasterPageFile="~/SingSchool.Master" AutoEventWireup="true" CodeBehind="ImportData.aspx.cs" Inherits="SignSchool.ImportData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="PageTitle">Import Data</h1> 
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
        <asp:Label runat="server" ID="DupLabel" Text="The following are duplicates found on the database. These will not be imported:" Visible="false"></asp:Label>
    </div>
    <div style="max-height:25vh;overflow-y:auto;overflow-x:auto;text-align:center;">
        <asp:GridView ID="GridView2" HorizontalAlign="Center" runat="server" CssClass="mydatagridDUPS"
            HeaderStyle-CssClass="header" RowStyle-CssClass="rows" AllowPaging="False">
        </asp:GridView>
    </div>

    <br />
    <br />
    <div style="text-align:center;">
        <asp:Label runat="server" ID="ImportLabel" Text="The following records will be imported:" Visible="false"></asp:Label><br />
        <asp:Label runat="server" ID="ImportDupLabel" Text="(Light yellow highlights are for duplicate URNs)" Visible="false"></asp:Label>
        <asp:Label runat="server" ID="ImportDupOnFileLabel" Text="(Orange highlights are for duplicate URNs + Reg Dates on file)" Visible="false"></asp:Label>
    </div>
    <div style="max-height:70vh;overflow-y:auto;overflow-x:auto;text-align: center;">
        <asp:GridView ID="GridView1" HorizontalAlign="Center" runat="server" CssClass="mydatagridIMPORT"
            HeaderStyle-CssClass="header" RowStyle-CssClass="rows" AllowPaging="False">
        </asp:GridView>
    </div>

</asp:Content>

