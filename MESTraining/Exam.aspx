<%@ Page Title="Marine Contractors Exam" Language="vb" AutoEventWireup="false" MasterPageFile="~/master/MESTraining.Master" CodeBehind="Exam.aspx.vb" Inherits="MESTraining.Exam" %>
<%@ Register Src="~/ExamRepeaterControl.ascx" TagName="ExamRepeaterControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="styles/BMPDMS.css" rel="stylesheet" />

    <asp:Table ID="Table2" runat="server" HorizontalAlign="Left" Width="100%">
        <asp:TableRow><asp:TableCell HorizontalAlign="left" Wrap="false" CssClass="BMPXPageTitleCenter" ColumnSpan="3">  <asp:Label ID="lblCertificationName" runat="server" Text="Label"></asp:Label></asp:TableCell></asp:TableRow>

        <asp:TableRow><asp:TableCell HorizontalAlign="Left" ColumnSpan="3">&nbsp;</asp:TableCell></asp:TableRow>    <%-- This is the blank line --%>
        
        <asp:TableRow><asp:TableCell HorizontalAlign="Left" Wrap="false" CssClass="BMPXPageTitle" ColumnSpan="3"><asp:Label  runat="server">Exam</asp:Label></asp:TableCell></asp:TableRow>
        <asp:TableRow><asp:TableCell HorizontalAlign="Left" Wrap="false" ColumnSpan="3"><asp:Label  runat="server">Exam may be saved for later or submitted at the bottom of the page.</asp:Label></asp:TableCell></asp:TableRow>

        <asp:TableRow>
            <asp:TableCell CssClass="inlineStyle">
     

                <asp:Label ID="lblMessage2" runat="server" /> <br />

                <asp:Repeater runat="server" ID="rptExam">
                    <ItemTemplate>
                        <uc1:ExamRepeaterControl id="ExamRepeaterControl1" runat="server" />
                    </ItemTemplate>
                </asp:Repeater>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>

<%--    <asp:LinkButton ID="lbSaveExam" runat="server" Text="" OnClick="lbSaveExam_Click"><asp:Image ID="imgSaveExam" runat="server" ImageUrl="../Images/saveandcompletelater.png" /></asp:LinkButton>
    <asp:LinkButton ID="lbSaveComplete" runat="server" Text="" OnClick="lbSaveComplete_Click"><asp:Image ID="imgSaveComplete" runat="server" ImageUrl="../Images/submitforgrading.png" /></asp:LinkButton>

    <br />
    <asp:LinkButton ID="lbViewCertificate" runat="server" OnClick="lbViewCertificate_Click">View Certificate</asp:LinkButton>--%>

    <asp:Button ID="btSaveExam" runat="server" Text="Save Exam" />
    <asp:Button ID="btSaveComplete" runat="server" Text="Submit Exam for Grading" />
    <br />
    <asp:Button ID="btViewCertificate" runat="server" Text="View Certificate" />


    <br />
    <asp:Label ID="lblMessage" runat="server" />

    <asp:Label ID="RegID" runat="server" visible="False" />

    <br />
    <br />
    <br />
    <br />

</asp:Content>
