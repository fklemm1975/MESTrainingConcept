<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/master/MESTraining.Master" CodeBehind="MESRegistration.aspx.vb" Inherits="MESTraining.MESRegistration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="styles/BMPDMS.css" rel="stylesheet" />
<%--    <style>
        table.radioWithProperWrap input{
            float:left
        }
        table.radioWithProperWrap label{
            display:block;
        }
    </style>--%>

    <asp:Table ID="Table1" runat="server">
        <asp:TableRow>
            <asp:TableCell CssClass="BMPXPageTitle">
                <br /><br /><B>Registration</b><br /><br />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                To use this website we recommend using a high speed internet connection using Internet Explorer 7.0 or 8.0 or Google Chrome.
When using higher versions of Internet Explorer, choose to view the website in compatibility mode in the "tools" dropdown menu.<br /><br />
 
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                If you do not have a high speed internet connection, please select the option below to view compressed video files.
Please note, compressed video files must be viewed in either Google Chrome or Safari.<br /><br />
 
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>

    <%--Frank - aligned the table to center added a border to this customized it too, probably should be in style sheet--%>
    <%--Frank - added margin left of 15px to labels of textboxes--%>
    <asp:Table ID="Table2" runat="server" HorizontalAlign="Center" Style="border-style:solid; border-width:thin; border-radius: 5px; border-collapse: separate;">
        <%--Frank - added empty row--%>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Style="padding-left:15px;">First Name:*</asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txtFirstName" Width="250" runat="server"></asp:TextBox>&nbsp;&nbsp;<asp:Label ID="lblFirstNameMsg" runat="server" ForeColor="Red"></asp:Label>
                <%--                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtFirstName" runat="server" ErrorMessage="First Name is required" ForeColor="Red"></asp:RequiredFieldValidator>--%>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Style="padding-left:15px;">Middle Name:</asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txtMiddleName" Width="250" runat="server"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Style="padding-left:15px;">Last Name:*</asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txtLastName" Width="250" runat="server"></asp:TextBox>&nbsp;&nbsp;<asp:Label ID="lblLastNameMsg" runat="server" ForeColor="Red"></asp:Label>
                <%--                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Last Name is required" ControlToValidate="txtLastName" ForeColor="Red"></asp:RequiredFieldValidator>--%>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Style="padding-left:15px;">Email Address:*</asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txtEmailAddress" Width="250" runat="server"></asp:TextBox>&nbsp;&nbsp;<asp:Label ID="lblEmailAddressMsg" runat="server" ForeColor="Red"></asp:Label>
                <%--                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Email Address is required" ControlToValidate="txtEmailAddress" ForeColor="Red"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmailAddress" ErrorMessage="&nbsp; Not a valid email address" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>--%>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Style="padding-left:15px;">Password:*</asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txtPassword" Width="250" runat="server" TextMode="Password"></asp:TextBox>&nbsp;&nbsp;&nbsp;<asp:Label ID="lblPasswordMsg" runat="server" ForeColor="Red"></asp:Label>
                <%--                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Password is required" ControlToValidate="txtPassword" ForeColor="Red"></asp:RequiredFieldValidator>--%>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Style="padding-left:15px;">Confirm Password:*&nbsp;</asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txtConfirmPassword" Width="250" runat="server" TextMode="Password"></asp:TextBox>&nbsp;&nbsp;&nbsp;<asp:Label ID="lblConfirmPasswordMsg" runat="server" ForeColor="Red"></asp:Label>
                <%--                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword" ErrorMessage="Password and Confirm Password do not match"></asp:CompareValidator>--%>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Style="padding-left:15px;">Street:</asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txtStreet" Width="250" runat="server"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Style="padding-left:15px;">City:</asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txtCity" Width="250" runat="server"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Style="padding-left:15px;">State:</asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txtState" Width="250" runat="server"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Style="padding-left:15px;">Zip Code:</asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txtZipCode" Width="250" runat="server"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Style="padding-left:15px;">Work Phone:</asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txtWorkPhone" Width="250" runat="server"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Style="padding-left:15px;">Mobile Phone:</asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txtMobilePhone" Width="250" runat="server"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Style="padding-left:15px;">Company Name:*</asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txtCompany" Width="250" runat="server"></asp:TextBox>&nbsp;&nbsp;<asp:Label ID="lblCompanyNameMsg" runat="server" ForeColor="Red"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell>
        </asp:TableRow>
        <%--Blank row--%>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2" Style="padding-left:15px;">
                <asp:RadioButton ID="Highspeed" Text="&nbsp;I have a high speed internet connection." Checked="True" GroupName="Compressed" runat="server" />
            </asp:TableCell>
        </asp:TableRow>

        <asp:TableRow>
            <asp:TableCell ColumnSpan="2" Style="padding-left:15px;" CssClass="radioWithProperWrap">
                <asp:RadioButton ID="Compressed" Text="&nbsp;I don’t have a high speed internet connection and want to view the compressed video files." GroupName="Compressed" runat="server" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell></asp:TableRow>
        <%--Blank row--%>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell></asp:TableRow>
        <%--Blank row--%>

        <asp:TableRow>
            <asp:TableCell ColumnSpan="2">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>

        <asp:TableRow>
            <asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell></asp:TableRow>
        <%--Blank row--%>

        <%--Frank - added padding to the button, so it would not be on the border--%>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2" style="padding: 0 0 10px 10px">
                <%--               <asp:ImageButton ID="ibRegister" runat="server" imageURL="../Images/register.png"/>--%>
                <asp:Button ID="btRegister" runat="server" Text="Register" />
            </asp:TableCell>
        </asp:TableRow>



    </asp:Table>

</asp:Content>
