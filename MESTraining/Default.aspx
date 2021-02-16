<%@ Page Title="Home Page" Language="vb" AutoEventWireup="false" MasterPageFile="~/master/MESTraining.Master" CodeBehind="Default.aspx.vb" Inherits="MESTraining._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<link href="styles/BMPDMS.css" rel="stylesheet" />

    <br />
<p class="BDHeader">
MES Training Center<br> <br>
</p>
<p>

<%--    <table width="800px">
        <tr>
            <td font-size:10pt" width="20%" nowrap="true">
                <a href="MESRegistration.aspx">Update Information</a><br />
                <a href="LogOut.aspx">Log Out</a><br />
                <br />
            </td>
        </tr>    
    </table>--%>

This online training is intended to help MES employees to complete training requirements.

<%--            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>--%>
</p>
    <table width="800px">
        <tr>
            <%--<td style="font-family:Cambria; font-size:14pt" width="20%" nowrap="true">--%>
            <td width="20%" nowrap="true">
<%--                <h1><a href="Search/Search.aspx">Search</a></h1>

                <h1><a href="CertificationsHome.aspx">Training Courses</a></h1>

                <h1><a href="Account/Registration.aspx">My Information</a></h1>

                <a href="Search/Search.aspx">Search for Certified Personnel</a><br />

--%>

                <a class="spec-text spec-text-mid" href="CertificationsHome.aspx">Training Courses</a><br />


            </td>
<%--            <td style="color:#04296e; font-family:Cambria; font-size:12pt" width="80%" nowrap align="center">&nbsp;&nbsp;Please select a task from the left menu to continue.</td>--%>
        </tr>    
    </table>

    <br />
    <br />
    <br />
    <br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
    <br />
    <br />


</asp:Content>
