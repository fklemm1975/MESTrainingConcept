<%@ Page Title="Exam Results" Language="vb" AutoEventWireup="false" MasterPageFile="~/master/MESTraining.Master" CodeBehind="ExamResults.aspx.vb" Inherits="MESTraining.ExamResults" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%-- Frank set width to 100% for main table, this centers the main table--%>
    <asp:Table ID="Table1" runat="server" HorizontalAlign="Left" Width="100%">
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="left" Wrap="false" CssClass="BMPXPageTitle" ColumnSpan="3">
                <h2>
                    <asp:Label ID="lblCertificationName" runat="server" Text="Label"></asp:Label></h2>
                <%--Frank made this to h2--%>
            </asp:TableCell>
        </asp:TableRow>

        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Left" ColumnSpan="3">&nbsp;</asp:TableCell>
        </asp:TableRow>
        <%-- This is the blank line --%>
        <%--Frank got rid of row below, options exist above--%>
        <%--        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Left">
                <a href="Default.aspx">Training Courses</a><br />
                <a href="LogOut.aspx">Log Out</a><br /><br />
            </asp:TableCell>
        </asp:TableRow>--%>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Center" ColumnSpan="3">
                <h3>
                    <asp:Label ID="lblExamResults" runat="server" /></h3>
                <br />
            </asp:TableCell>
        </asp:TableRow>

        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Center">
                <h3><b>Score: &nbsp;</b><asp:Label ID="lblExamScore" runat="server" /></h3>
                <br />
                <br />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Center">
                <asp:Panel ID="pnlPass" runat="server" Width="900px">
                    <asp:LinkButton ID="lbCertificate" runat="server" Text="Click here" OnClick="lbCertificate_Click"></asp:LinkButton>
                    to view your certificate.
                    <br />
                    <br />
                    <asp:LinkButton ID="lbTestResultsP" runat="server" Text="Click here" OnClick="lbTestResultsP_Click"></asp:LinkButton>
                    to view the test results.
                </asp:Panel>
                <asp:Panel ID="pnlFail" runat="server" Width="900px">
                    <asp:LinkButton ID="lbReview" runat="server" Text="Click here" OnClick="lbReview_Click"></asp:LinkButton>
                    to review the course material.
                    <br />
                    <br />
                    <asp:LinkButton ID="lbTestResultsF" runat="server" Text="Click here" OnClick="lbTestResultsF_Click"></asp:LinkButton>
                    to view the test results.
                </asp:Panel>
            </asp:TableCell>
        </asp:TableRow>

    </asp:Table>






    <br />
    <br />
    <br />

</asp:Content>
