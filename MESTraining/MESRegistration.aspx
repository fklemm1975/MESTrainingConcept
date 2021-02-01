<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/master/MESTraining.Master" CodeBehind="MESRegistration.aspx.vb" Inherits="MESTraining.MESRegistration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <link href="styles/BMPDMS.css" rel="stylesheet" />

        <asp:Table ID="Table1" runat="server">
        <asp:TableRow>
            <asp:TableCell CssClass="BMPXPageTitle" >
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

    <asp:Table ID="Table2" runat="server">
        <asp:TableRow>
            <asp:TableCell>First Name:*</asp:TableCell>
            <asp:Tablecell> 
                <asp:TextBox ID="txtFirstName" Width="250" runat="server"></asp:TextBox>&nbsp;&nbsp;<asp:Label ID="lblFirstNameMsg" runat="server" ForeColor="Red"></asp:Label>
<%--                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtFirstName" runat="server" ErrorMessage="First Name is required" ForeColor="Red"></asp:RequiredFieldValidator>--%>
            </asp:Tablecell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>Middle Name:</asp:TableCell>
            <asp:Tablecell> <asp:TextBox ID="txtMiddleName" Width="250" runat="server"></asp:TextBox>  </asp:Tablecell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>Last Name:*</asp:TableCell>
            <asp:Tablecell> 
                <asp:TextBox ID="txtLastName" Width="250" runat="server"></asp:TextBox>&nbsp;&nbsp;<asp:Label ID="lblLastNameMsg" runat="server" ForeColor="Red"></asp:Label>  
<%--                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Last Name is required" ControlToValidate="txtLastName" ForeColor="Red"></asp:RequiredFieldValidator>--%>
            </asp:Tablecell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>Email Address:*</asp:TableCell>
            <asp:Tablecell> 
                <asp:TextBox ID="txtEmailAddress" Width="250" runat="server"></asp:TextBox>&nbsp;&nbsp;<asp:Label ID="lblEmailAddressMsg" runat="server" ForeColor="Red"></asp:Label>
<%--                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Email Address is required" ControlToValidate="txtEmailAddress" ForeColor="Red"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmailAddress" ErrorMessage="&nbsp; Not a valid email address" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>--%>

            </asp:Tablecell>
        </asp:TableRow>
       <asp:TableRow>
            <asp:TableCell>Password:*</asp:TableCell>
            <asp:Tablecell> 
                <asp:TextBox ID="txtPassword" Width="250" runat="server" TextMode="Password"></asp:TextBox>&nbsp;&nbsp;&nbsp;<asp:Label ID="lblPasswordMsg" runat="server" ForeColor="Red"></asp:Label>
<%--                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Password is required" ControlToValidate="txtPassword" ForeColor="Red"></asp:RequiredFieldValidator>--%>
            </asp:Tablecell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>Confirm Password:*&nbsp;</asp:TableCell>
            <asp:Tablecell> 
                <asp:TextBox ID="txtConfirmPassword" Width="250" runat="server" TextMode="Password"></asp:TextBox>&nbsp;&nbsp;&nbsp;<asp:Label ID="lblConfirmPasswordMsg" runat="server" ForeColor="Red"></asp:Label>
<%--                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword" ErrorMessage="Password and Confirm Password do not match"></asp:CompareValidator>--%>
            </asp:Tablecell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>Street:</asp:TableCell>
            <asp:Tablecell> <asp:TextBox ID="txtStreet" Width="250" runat="server"></asp:TextBox>  </asp:Tablecell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>City:</asp:TableCell>
            <asp:Tablecell> <asp:TextBox ID="txtCity" Width="250" runat="server"></asp:TextBox>  </asp:Tablecell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>State:</asp:TableCell>
            <asp:Tablecell> <asp:TextBox ID="txtState" Width="250" runat="server"></asp:TextBox>  </asp:Tablecell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>Zip Code:</asp:TableCell>
            <asp:Tablecell> <asp:TextBox ID="txtZipCode" Width="250" runat="server"></asp:TextBox>  </asp:Tablecell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>Work Phone:</asp:TableCell>
            <asp:Tablecell> <asp:TextBox ID="txtWorkPhone" Width="250" runat="server"></asp:TextBox>  </asp:Tablecell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>Mobile Phone:</asp:TableCell>
            <asp:Tablecell> <asp:TextBox ID="txtMobilePhone" Width="250" runat="server"></asp:TextBox>  </asp:Tablecell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>Company Name:*</asp:TableCell>
            <asp:Tablecell> <asp:TextBox ID="txtCompany" Width="250" runat="server"></asp:TextBox>&nbsp;&nbsp;<asp:Label ID="lblCompanyNameMsg" runat="server" ForeColor="Red"></asp:Label>  </asp:Tablecell>
        </asp:TableRow>
        <asp:TableRow><asp:TableCell>&nbsp;</asp:TableCell></asp:TableRow>          <%--Blank row--%> 
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2">
                <asp:RadioButton id="Highspeed" Text="I have a high speed internet connection." Checked="True" GroupName="Compressed" runat="server"/>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2">
                <asp:RadioButton id="Compressed" Text="I don’t have a high speed internet connection and want to view the compressed video files." GroupName="Compressed" runat="server"/>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow><asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell></asp:TableRow>          <%--Blank row--%> 
        <asp:TableRow><asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell></asp:TableRow>          <%--Blank row--%> 

        <asp:TableRow>
            <asp:TableCell ColumnSpan="2">
               <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>

        <asp:TableRow><asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell></asp:TableRow>          <%--Blank row--%> 

        <asp:TableRow>
            <asp:TableCell ColumnSpan="2">
<%--               <asp:ImageButton ID="ibRegister" runat="server" imageURL="../Images/register.png"/>--%>
                <asp:Button ID="btRegister" runat="server" Text="Register" />
            </asp:TableCell>
        </asp:TableRow>



    </asp:Table>

</asp:Content>
