<%@ Page Title="Sense Sign School" Language="C#" MasterPageFile="~/SingSchool.Master" AutoEventWireup="true" CodeBehind="UserConfiguration.aspx.cs" Inherits="SignSchool.UserConfiguration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            font-weight: bold;
            color: white;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="PageTitle">User Configuration</h1> 


    <asp:DropDownList ID="DropDownList1" runat="server" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack = "true">
        <asp:ListItem Value="0" Text="Select..."></asp:ListItem>
        <asp:ListItem Value="1" Text="Add New User"></asp:ListItem>
        <asp:ListItem Value="2" Text="Edit User"></asp:ListItem>
    </asp:DropDownList>

    <asp:Table id="DataEditTable" runat="server" CellPadding="5" GridLines="None" BackColor="#E3EAEB" BorderColor="#E6E2D8" BorderPadding="4" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#333333" TextLayout="TextOnTop" HorizontalAlign="Center" Visible="false" Enabled="false">
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" >
                <asp:Label runat="server" ID="UserId_lbl" Text="User Id:" ForeColor="Black"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                 <asp:TextBox ID="UserId_txt" runat="server" Text="" Enabled="false" AutoCompleteType="Disabled"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label runat="server" ID="Username_lbl" Text="Username:" ForeColor="Black"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                 <asp:TextBox ID="Username_txt" runat="server" Text="" Enabled="false"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label runat="server" ID="Email_lbl" Text="Email:" ForeColor="Black"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                 <asp:TextBox ID="Email_txt" runat="server" Text="" AutoCompleteType="Disabled"></asp:TextBox>
            </asp:TableCell>
            
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label runat="server" ID="SendEmailOption_lbl" Text="Send Emails?" ForeColor="Black"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="SendEmailOption_drp" runat="server">
                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                    <asp:ListItem Value="To" Text="To"></asp:ListItem>
                    <asp:ListItem Value="CC" Text="CC"></asp:ListItem>
                 </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label runat="server" ID="NewPassword_lbl" Text="New Password:" ForeColor="Black"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                 <asp:TextBox ID="NewPassword_txt" runat="server" Text="" Value="" TextMode="Password" AutoCompleteType="Disabled"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Center">
                <asp:CheckBox runat="server" ID="Admin_cbx" Text="Admin User?" ForeColor="Black"/>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Center">
                 <asp:CheckBox runat="server" ID="Live_cbx" Text="Live User?" ForeColor="Black"/>
            </asp:TableCell>
        </asp:TableRow>   
        
        <asp:TableFooterRow>
             <asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
                <asp:Button ID="SaveButton" runat="server" OnClick="SaveButton_Click" Text="Save Changes" />
             </asp:TableCell>
        </asp:TableFooterRow>
    </asp:Table>

    <div style="max-height:70vh;overflow-y:auto;overflow-x:auto;text-align: center;">
        <asp:GridView ID="GridView_ViewUsers" HorizontalAlign="Center" runat="server" CssClass="mydatagridUsers"
            HeaderStyle-CssClass="header" RowStyle-CssClass="rows_editgrid" AllowPaging="False" AutoGenerateEditButton="true"
            OnRowEditing="GridView_ViewUsers_EditUser"
            Visible="false" Enabled="false">
        </asp:GridView>
    </div>

    <asp:Table id="NewUserCreation" runat="server" CellPadding="5" GridLines="None" BackColor="#E3EAEB" BorderColor="#E6E2D8" BorderPadding="4" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#333333" TextLayout="TextOnTop" HorizontalAlign="Center" Visible="false" Enabled="false">
        <asp:TableHeaderRow>
            <asp:TableHeaderCell ColumnSpan="2" VerticalAlign="Top" HorizontalAlign="Center" >Create New User</asp:TableHeaderCell>
        </asp:TableHeaderRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Right">
                    <asp:Label runat="server" Text="User Name:"/>
                </asp:TableCell><asp:TableCell>
                    <asp:TextBox id="UsernameTextBox" runat="server" Value="" AutoCompleteType="Disabled"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Right">
                    <asp:Label runat="server" Text="Password:"/>
                </asp:TableCell><asp:TableCell>
                    <asp:TextBox id="Password1TextBox" runat="server" Value="" TextMode="Password" AutoCompleteType="Disabled"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Right">
                    <asp:Label runat="server" Text="Confirm Password:"/>
                </asp:TableCell><asp:TableCell>
                    <asp:TextBox id="Password2TextBox" runat="server" Value="" TextMode="Password" AutoCompleteType="Disabled"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Right">
                    <asp:Label runat="server" Text="E-Mail:"/>
                </asp:TableCell><asp:TableCell>
                    <asp:TextBox id="EmailTextBox" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Right">
                    <asp:Label runat="server" Text="Admin User?:"/>
                </asp:TableCell><asp:TableCell>
                 <asp:CheckBox ID="AdminRightsCheckbox" runat="server" Checked="false"/>
                 </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow HorizontalAlign="Center">
                    <asp:TableCell ColumnSpan="2">
                    <asp:Label id="ProgressLabel" runat="server" ForeColor="#fc0303" Text=""/>
                    </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow HorizontalAlign="Right">
                    <asp:TableCell ColumnSpan="2">
                    <asp:Button ID="CreateNewUserButton" runat="server" OnClick="CreateNewUserButton_Click" Text="Create User" BackColor="White" BorderColor="#C5BBAF" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em"/>
                    </asp:TableCell>
            </asp:TableRow>

    </asp:Table>


</asp:Content>
