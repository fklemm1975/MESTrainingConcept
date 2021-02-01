<%@ Page Title="Exam Results" Language="vb" AutoEventWireup="false" MasterPageFile="~/master/MESTraining.Master" CodeBehind="ExamResults.aspx.vb" Inherits="MESTraining.ExamResults" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Table ID="Table1" runat="server" HorizontalAlign="Left">
        <asp:TableRow><asp:TableCell HorizontalAlign="left" Wrap="false" CssClass="BMPXPageTitle" ColumnSpan="3">  <asp:Label ID="lblCertificationName" runat="server" Text="Label"></asp:Label></asp:TableCell></asp:TableRow>

        <asp:TableRow><asp:TableCell HorizontalAlign="Left" ColumnSpan="3">&nbsp;</asp:TableCell></asp:TableRow>    <%-- This is the blank line --%>
        <asp:TableRow>
            <asp:TableCell horizontalAlign ="Left">
                <a href="Default.aspx">Training Courses</a><br />
                <a href="LogOut.aspx">Log Out</a><br /><br />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell horizontalAlign ="Center">
                <asp:Label id="lblExamResults" runat="server" /> <br />
            </asp:TableCell>
        </asp:TableRow>

        <asp:TableRow>
            <asp:TableCell horizontalAlign ="Center">
                <b>Score: &nbsp;</b><asp:Label id="lblExamScore" runat="server" />
                <br />
                <br />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell horizontalAlign ="Center">
                <asp:Panel id="pnlPass" runat="server" width="900px">
                    <asp:LinkButton ID="lbCertificate" runat="server" Text="Click here" OnClick="lbCertificate_Click"></asp:LinkButton> to view your certificate.
                    <br />
                    <br />
                    <asp:LinkButton ID="lbTestResultsP" runat="server" Text="Click here" OnClick="lbTestResultsP_Click"></asp:LinkButton> to view the test results.
                </asp:Panel>
                <asp:Panel id="pnlFail" runat="server" width="900px">
                    <asp:LinkButton ID="lbReview" runat="server" Text="Click here" OnClick="lbReview_Click"></asp:LinkButton> to review the course material.
                    <br />
                    <br />
                    <asp:LinkButton ID="lbTestResultsF" runat="server" Text="Click here" OnClick="lbTestResultsF_Click"></asp:LinkButton> to view the test results.
                </asp:Panel>
            </asp:TableCell>
        </asp:TableRow>

    </asp:Table>

    
    



    <br />
    <br />
    <br />

</asp:Content>
