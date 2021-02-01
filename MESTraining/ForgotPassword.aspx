<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/master/BMPExchange.Master" CodeBehind="ForgotPassword.aspx.vb" Inherits="MESTraining.ForgotPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<%--
  <h2>
        Forgotten Password
    </h2>
    <p>
        Please enter your username and password.
        <asp:HyperLink ID="RegisterHyperLink" runat="server" EnableViewState="false">Register</asp:HyperLink> if you don't have an account.
    </p>
    <p>
        &nbsp;
    </p>
   
--%>  
    
    <p>
        <asp:Label ID="lblMessage" runat="server" Text=""> </asp:Label>
        <br />
    </p>

        Forgot your password? 
        <p>
            Don't worry.  Just submit the email address associated with your account and you will<br />receive an email with further instructions regarding account recovery.
        </p>
        <p>
            Email Address:&nbsp;<asp:TextBox ID="txtEmailRecover" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;<asp:Button ID="btnSubmit" runat="server" Text="Submit" />
        </p>
        <p>
            <asp:Label ID="lblWarning" runat="server" Text=""></asp:Label>
        </p>
                
    <br />
    <br />

</asp:Content>
