<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ExamRepeaterControl.ascx.vb" Inherits="MESTraining.ExamRepeaterControl" %>

    <h2>
        <asp:Label ID="lblQuestionHeading" runat="server" Text='<%#Eval("Name")%>' />
    </h2>
    <br />
    <asp:Label ID="lblRank" runat="server" Text='<%#Eval("Rank")%>' />
    <asp:Label ID="lblQuestion" runat="server" Text='<%#Eval("Question")%>' />
    <br /><br />
    <asp:RadioButton ID="rbAnswer1" runat="server" Text='<%#Eval("Answer1")%>' Selected="False" GroupName="Answers" /><br />
    <asp:RadioButton ID="rbAnswer2" runat="server" Text='<%#Eval("Answer2")%>' Selected="False" GroupName="Answers" /><br />
    <asp:RadioButton ID="rbAnswer3" runat="server" Text='<%#Eval("Answer3")%>' Selected="False" GroupName="Answers" /><br />
    <asp:RadioButton ID="rbAnswer4" runat="server" Text='<%#Eval("Answer4")%>' Selected="False" GroupName="Answers" /><br />
    <asp:RadioButton ID="rbAnswer5" runat="server" Text='<%#Eval("Answer5")%>' Selected="False" GroupName="Answers" /><br />
    <asp:HiddenField ID="hfQuestionID" runat="server" value='<%#Eval("UserQuestionsID")%>' />
    <asp:HiddenField ID="hfAnswer" runat="server" value='' />
    <asp:HiddenField ID="hfSelectedAnswer" runat="server" Value='<%#Eval("SelectedAnswer")%>' />
    <asp:Panel ID="Panel1" runat="server" Visible="false">
        <br />
        <asp:Label ID="lblCorrectIncorrect" runat="server" Text='<%#Eval("CorrectIncorrect") %>' Visible="false"/>
        <br />
        <asp:Label ID="lblCorrectAnswer" runat="server" Text='<%#Eval("FinalAnswer") %>' Visible="false"/>
    </asp:Panel>
    <hr />