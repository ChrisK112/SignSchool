<%@ Page Title="Sense Sign School" Language="C#" MasterPageFile="~/SingSchool.Master" AutoEventWireup="true" CodeBehind="EditHoliday.aspx.cs" Inherits="SignSchool.EditHoliday" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="PageTitle">Edit Holiday</h1> 
    <br />



    <asp:Table id="DateControl" runat="server" CellPadding="5" GridLines="None" Font-Names="Verdana" Font-Size="0.8em" TextLayout="TextOnTop" HorizontalAlign="Center">
        <asp:TableRow HorizontalAlign="Right">
            <asp:TableCell ColumnSpan="2">
                <asp:Calendar ID="Calendar" runat="server" OnDayRender="Calendar_DayRender" Style="text-align: center" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="15pt" ForeColor="Black" Height="500px" Width="540px">
                    <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="15pt" />
                    <NextPrevStyle VerticalAlign="Bottom" />
                    <OtherMonthDayStyle ForeColor="#808080" />
                    <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                    <SelectorStyle BackColor="#CCCCCC" />
                    <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
                    <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <WeekendDayStyle BackColor="#FFFFCC" />
                </asp:Calendar>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow HorizontalAlign="Right">
            <asp:TableCell>
                <asp:Button ID="RemoveDay" runat="server" OnClick="RemoveDayButton_Click" Text="Remove Day" CSSClass="Date_Button"/>
                <asp:Button ID="AddDay" runat="server" OnClick="AddDayButton_Click" Text="Add Day" CSSClass="Date_Button"/>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>

