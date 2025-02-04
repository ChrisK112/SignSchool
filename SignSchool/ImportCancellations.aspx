<%@ Page Title="Sense Sign School" Language="C#" MasterPageFile="~/SingSchool.Master" AutoEventWireup="true" CodeBehind="ImportCancellations.aspx.cs" Inherits="SignSchool.ImportCancellations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="PageTitle">Import Cancellation Data</h1> 
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
        <asp:Label runat="server" ID="CancLabelRED" Text="Red: These records could not be matched to the live or 12 pack database." Visible="false"></asp:Label>
    </div>
    <div style="text-align:center;">
        <asp:Label runat="server" ID="CancLabelLY" Text="Light Yellow: These records are on the 12 pack database." Visible="false"></asp:Label>
    </div>
    <div style="text-align:center;">
        <asp:Label runat="server" ID="CancLabelGR" Text="Light Green: These records were already cancelled." Visible="false"></asp:Label>
    </div>
    <div style="text-align:center;">
        <asp:Label runat="server" ID="CancLabelLB" Text="Light Blue: These records match through URN, but not registration date." Visible="false"></asp:Label>
    </div>
        
        <asp:GridView ID="GridView1" HorizontalAlign="Center" runat="server" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="Large" RowStyle-Font-Size="Medium">
        </asp:GridView>


</asp:Content>

