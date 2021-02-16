<%@ Page Title="Continuing Education Instruction" Language="vb" AutoEventWireup="false" MasterPageFile="~/master/MESTraining.Master" CodeBehind="CertificationsHome.aspx.vb" Inherits="MESTraining.CertificationHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="styles/BMPDMS.css" rel="stylesheet" />

    <asp:Table ID="Table1" runat="server" Width="800px">
        <asp:TableRow>
            <asp:TableCell CssClass="BMPXHeader">
                <br /><br /><b>Continuing Education Instruction</b><br /><br />
            </asp:TableCell>
        </asp:TableRow>

        <asp:TableRow>
            <asp:TableCell>

                <asp:Panel ID="ddlPanel1" runat="server" Visible="False">
                    Start a new
                    <asp:DropDownList ID="ddlCertification" runat="server"></asp:DropDownList>
                    certification.
                    <br />
                    <br />
                    <asp:Button ID="btStartNewCertification" runat="server" Text="Start New Certification" />
                    <br />
                    <br />
                    <br />
                </asp:Panel>

                <asp:Panel ID="ddlPanel2" runat="server" Visible="False">
                    No certifications to start.
                    <br />
                    <br />
                    <br />
                </asp:Panel>

            </asp:TableCell>
        </asp:TableRow>

        <%--        <asp:TableRow>
            <asp:TableCell horizontalAlign ="Left">
                <a href="Default.aspx">Training Courses</a><br />
                <a href="LogOut.aspx">Log Out</a><br /><br />
            </asp:TableCell>
        </asp:TableRow>--%>
        <%--        Frank moved My Training Modules to second table--%>
    </asp:Table>


<%--    Frank set the width of the "grand table" to 100%, so it took up the whole screen
    Then I created three columns for each row. I placed the relevant text in the center column
    I also matched the top main cell width to match the set width of the grid table--%>
    <asp:Table ID="Table2" runat="server" Width="100%">
        <asp:TableRow>
            <asp:TableCell>&nbsp;</asp:TableCell>
            <asp:TableCell HorizontalAlign="left" Wrap="false" CssClass="BMPXHeader" Width="800px">
                My Training Modules
            </asp:TableCell>
            <asp:TableCell>&nbsp;</asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>&nbsp;</asp:TableCell>
            <asp:TableCell HorizontalAlign="Center">
                <asp:GridView ID="gvMyCertifications" runat="server"
                    EmptyDataText="No Certifications" RowStyle-BackColor="#B0C4DE" AlternatingRowStyle-BackColor="white"
                    HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false"
                    AllowPaging="true" PageSize="20" AutoGenerateColumns="false"
                    RowStyle-CssClass="GridView" RowStyle-Wrap="False" Width="800px">
                    <Columns>
                        <asp:BoundField DataField="UserSummaryID" ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone" Visible="True" />
                        <asp:BoundField DataField="CertificationTypeID" ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone" Visible="false" />
                        <asp:BoundField DataField="CertificationName" HeaderText="Name" />
                        <asp:BoundField DataField="StartDate" HeaderText="Start Date" />
                        <asp:BoundField DataField="ExamComplete" HeaderText="Course Status" />
                        <asp:HyperLinkField DataTextField="LastModuleText" HeaderText="Next Module" DataNavigateUrlFields="LastModule" />
                        <asp:BoundField DataField="ExamScore" HeaderText="Score" />
                        <asp:BoundField DataField="PassFail" HeaderText="Pass/Fail" />
                        <asp:HyperLinkField DataTextField="TakeExamText" DataNavigateUrlFields="TakeExamLink" />
                        <asp:ButtonField DataTextField="ViewCertificate" CommandName="ViewCertificate" />
                    </Columns>
                </asp:GridView>
            </asp:TableCell>
            <asp:TableCell>&nbsp;</asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>&nbsp;</asp:TableCell>
            <asp:TableCell>&nbsp;Please press continue to advance to the next training module.</asp:TableCell>
            <asp:TableCell>
<%--                <asp:ImageButton ID="ibStart" runat="server" imageURL="../Images/Start.png"/>--%>
<%--                <asp:Button ID="btStart" runat="server" Text="Start" />--%>
            </asp:TableCell>
        </asp:TableRow>

    </asp:Table>



</asp:Content>
