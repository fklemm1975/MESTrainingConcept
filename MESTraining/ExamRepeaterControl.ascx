<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ExamRepeaterControl.ascx.vb" Inherits="MESTraining.ExamRepeaterControl" %>

<%--Frank added the exam-indent class and used this for questions below--%>
    <style>
        .exam-indent {
            margin-left: 25px;
        }
    </style>
    <h2 class="exam-indent">
        <asp:Label ID="lblQuestionHeading" runat="server" Text='<%#Eval("Name")%>' />
    </h2>
    <br />
    <asp:Label CssClass="exam-indent" ID="lblRank" runat="server" Text='<%#Eval("Rank")%>' />
    <asp:Label ID="lblQuestion" runat="server" Text='<%#Eval("Question")%>' />
    <br /><br />
    <asp:RadioButton ID="rbAnswer1" CssClass="exam-indent" runat="server" Text='<%#Eval("Answer1")%>' Selected="False" GroupName="Answers" /><br />
    <asp:RadioButton ID="rbAnswer2" CssClass="exam-indent" runat="server" Text='<%#Eval("Answer2")%>' Selected="False" GroupName="Answers" /><br />
    <asp:RadioButton ID="rbAnswer3" CssClass="exam-indent" runat="server" Text='<%#Eval("Answer3")%>' Selected="False" GroupName="Answers" /><br />
    <asp:RadioButton ID="rbAnswer4" CssClass="exam-indent" runat="server" Text='<%#Eval("Answer4")%>' Selected="False" GroupName="Answers" /><br />
    <asp:RadioButton ID="rbAnswer5" CssClass="exam-indent" runat="server" Text='<%#Eval("Answer5")%>' Selected="False" GroupName="Answers" /><br />
    <asp:HiddenField ID="hfQuestionID" runat="server" value='<%#Eval("UserQuestionsID")%>' />
    <asp:HiddenField ID="hfAnswer" runat="server" value='' />
    <asp:HiddenField ID="hfSelectedAnswer" runat="server" Value='<%#Eval("SelectedAnswer")%>' />
    <asp:Panel ID="Panel1" CssClass="exam-indent"  runat="server" Visible="false">
        <br />
        <asp:Label ID="lblCorrectIncorrect" runat="server" Text='<%#Eval("CorrectIncorrect") %>' Visible="false"/>
        <br />
        <asp:Label ID="lblCorrectAnswer" runat="server" Text='<%#Eval("FinalAnswer") %>' Visible="false"/>
    </asp:Panel>
    <hr />