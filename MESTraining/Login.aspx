<%@ Page Title="Login to MES Training" Language="vb" AutoEventWireup="false" MasterPageFile="~/master/MESTraining.Master" CodeBehind="Login.aspx.vb" Inherits="MESTraining.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <link href="styles/BMPDMS.css" rel="stylesheet" />
    <asp:Panel runat="server" DefaultButton="btLogin" CssClass="panel">
        <asp:Table ID="Table2" runat="server" HorizontalAlign="Center">
            <asp:TableRow>
<%--          Frank changed this cell      <asp:TableCell CssClass="BMPXPageTitle">
                    <br /><br />MES Training Center<br /><br />
                </asp:TableCell>--%>
                <asp:TableCell ColumnSpan ="2" HorizontalAlign="Center">
                    <h1 class="spec-text">MES Training Center</h1>
                </asp:TableCell>
            </asp:TableRow>
            <%-- Frank added row --%>
            <asp:TableRow>
                <asp:TableCell>&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    This online training is intended to help MES employees to complete training requirements.  
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell colspan="2">
<%--       Frank Changed             If it's your first time to the MES Training Center web site, please click register. &nbsp;&nbsp;
                    <asp:Button ID="btRegister" runat="server" Text="Register" />--%>
                    If this is your first time to the MES Training Center web site, please click <asp:LinkButton ID="lbRegister" runat="server" Text="Here" /> to register.
                </asp:TableCell></asp:TableRow></asp:Table><asp:Table ID="Table3" runat="server" HorizontalAlign="Center">
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle">
                    Email Address: &nbsp;&nbsp;
                </asp:TableCell><asp:TableCell>
                    <asp:TextBox ID="txtEmailAddress" runat="server"></asp:TextBox>
                </asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle">
                    Password: &nbsp;&nbsp;
                </asp:TableCell><asp:TableCell VerticalAlign="Bottom">
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox><br />
                    <br />
                </asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell ColumnSpan="2">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell>&nbsp;</asp:TableCell><asp:TableCell>
                    <%--                <asp:ImageButton ID="ImageButton1" runat="server" imageURL="../Images/login.png"/>--%>
                    <asp:Button ID="btLogin" runat="server" Text="Login" />
                </asp:TableCell></asp:TableRow></asp:Table></asp:Panel></asp:Content>