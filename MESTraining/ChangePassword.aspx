<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/master/BMPExchangeUser.Master" CodeBehind="ChangePassword.aspx.vb" Inherits="MESTraining.ChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

        
<asp:Table ID="Table2" runat="server">
    <asp:TableRow><asp:TableCell HorizontalAlign="left" Wrap="false" CssClass="BMPXPageTitle"  ColumnSpan="2" >Change Password</asp:TableCell></asp:TableRow>
    <asp:TableRow>
        <asp:TableCell ColumnSpan="2">


        <asp:Table id="Table1" runat="server">
            <asp:TableRow><asp:TableCell colspan="3"><asp:Label ID="lblmessage" runat="server" Text=""></asp:Label></asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell colspan="3">&nbsp;</asp:TableCell></asp:TableRow>
            <asp:TableRow><asp:TableCell align="left"><asp:label ID="lblOldPW" runat="server">Old Password:</asp:label>&nbsp;</asp:TableCell>
                <asp:TableCell align="left" colspan="2"><asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password" Width="130"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell align="left" wrap="false" width="2%">New Password:&nbsp;</asp:TableCell>
                <asp:TableCell align="left" colspan="2"><asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" Width="130"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell align="left" wrap="false">Confirm:&nbsp;</asp:TableCell>
                <asp:TableCell align="left" colspan="2"><asp:TextBox ID="txtConfirm" runat="server" TextMode="Password" Width="130"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell colspan="1">&nbsp;</asp:TableCell>
                <asp:TableCell colspan="2" HorizontalAlign="left"><asp:Button ID="btSave" runat="server" Text="Save"  /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell colspan="3" align="left" ><asp:Label ID="lblError" Text="" runat="server" ForeColor="#FF3300"></asp:Label></asp:TableCell>
            </asp:TableRow>
        </asp:table>

        </asp:TableCell>
    </asp:TableRow>
</asp:Table>

</asp:Content>
