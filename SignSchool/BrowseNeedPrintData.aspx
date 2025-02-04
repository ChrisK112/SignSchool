<%@ Page Title="Sense Sign School" Language="C#" MasterPageFile="~/SingSchool.Master" AutoEventWireup="true" CodeBehind="BrowseNeedPrintData.aspx.cs" Inherits="SignSchool.BrowseNeedPrintData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="PageTitle">Browse Records for Printing</h1> 
    <br />


    <div style="max-height:70vh;overflow-y:scroll;overflow-x:auto;text-align: center;">
        <asp:GridView ID="GridView1" HorizontalAlign="Center" runat="server" CssClass="mydatagrid"
            HeaderStyle-CssClass="header" RowStyle-CssClass="rows" AllowPaging="False" >
        </asp:GridView>
    </div>
        
</asp:Content>

