<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Registration.aspx.vb" Inherits="MESTraining.Registration" MasterPageFile="~/master/BMPExchange.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server"> 

    <table width="100%" style="margin-left:auto; margin-right:auto; margin-top:0; margin-bottom:0;height:"100%">
        <tr align="center">
            <td colspan="2" style="color:#04296e; font-weight:bold; font-family:Cambria; font-size:24pt">Registration</td>
        </tr>
        <tr align="center"><td colspan="2" >&nbsp;</td></tr>
        <tr align="center">
            <td colspan="2" style="font-weight:bold; font-family:Cambria; font-size:10pt">To use this website we recommend using a high speed internet connection using Internet Explorer 7.0 or 8.0 or Google Chrome.</br>When using higher versions of Internet Explorer, choose to view the website in compatibility mode in the “tools” dropdown menu.</td>
        </tr>
        <tr align="center"><td colspan="2" >&nbsp;</td></tr>
        <tr align="center">
            <td colspan="2" style="font-weight:bold; font-family:Cambria; font-size:10pt">If you do not have a high speed internet connection, please select the option below to view compressed video files.</br>Please note, compressed video files must be viewed in either Google Chrome or Safari. </td>
        </tr>
        <tr align="center"><td colspan="2" >&nbsp;</td></tr>
    </table>

    
     <table align="center" width="743px">
        <tr>
            <td colspan="2"><asp:Label ID="lblFirstName" runat="server" Text="First Name*:"></asp:Label></td>
            <td colspan="2">
                <asp:TextBox ID="txtFirstName" Width="250" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtFIRSTNAME" runat="server" ErrorMessage="First Name is required" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2"><asp:Label ID="lblMiddleName" runat="server" Text="Middle Name:"></asp:Label></td>
            <td colspan="2"><asp:TextBox ID="txtMiddleName" Width="250" runat="server"></asp:TextBox></td>  
        </tr>
        <tr>
            <td colspan="2"><asp:Label ID="lblLastName" runat="server" Text="Last Name*:"></asp:Label></td>
            <td colspan="2">
                <asp:TextBox ID="txtLastName" Width="250" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Last Name is required" ControlToValidate="txtLASTNAME" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2"><asp:Label ID="Label3" runat="server" Text="Email Address*:"></asp:Label></td>
            <td colspan="2">
                <asp:TextBox ID="txtEmailAddress"  Width="250" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Email Address is required" ControlToValidate="txtEmailAddress" ForeColor="Red"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmailAddress" ErrorMessage="Not a valid email address" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
            </td>
        </tr> 
        <tr>
            <td colspan="2"><asp:Label ID="lblPassword" runat="server" Text="Password*:"></asp:Label></td>
            <td colspan="2">
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"  Width="250"  Wrap="False"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Password is required" ControlToValidate="txtPassword" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>           
        <tr>
            <td colspan="2"><asp:Label ID="Label17" runat="server" Text="Confirm Password*:"></asp:Label></td>
            <td colspan="2">
                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password"  Width="250"  Wrap="False"></asp:TextBox>
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword" ErrorMessage="Password and Confirm Password do not match"></asp:CompareValidator>
            </td>
        </tr>      
        <tr>
            <td colspan="2"><asp:Label ID="lblStreet" runat="server" Text="Street:"></asp:Label></td>
            <td colspan="2"><asp:TextBox ID="txtStreet" Width="400" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td colspan="2"><asp:Label ID="lblCity" runat="server" Text="City:"></asp:Label></td>
            <td colspan="2"><asp:TextBox ID="txtCity"  Width="250" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td colspan="2"><asp:Label ID="lblState" runat="server" Text="State:" Width="45"></asp:Label></td>
            <td colspan="2"><asp:TextBox ID="txtState"  Width="250" runat="server"></asp:TextBox></td>
        </tr> 
        <tr>
            <td colspan="2"><asp:Label ID="Label4" runat="server" Text="Zip Code:" Width="85"></asp:Label></td>                
            <td colspan="2"><asp:TextBox ID="txtZipCode" runat="server"  Width="250" ></asp:TextBox></td>
        </tr> 
        <tr>
            <td colspan="2"><asp:Label ID="lblWorkPhone" runat="server" Text="Work Phone:"></asp:Label></td>
            <td colspan="2"><asp:TextBox ID="txtWorkPhone" runat="server"  Width="250" ></asp:TextBox></td>
        </tr>  
        <tr>
            <td colspan="2"><asp:Label ID="lblMobilePhone" runat="server" Text="Mobile Phone:"></asp:Label></td>
            <td colspan="2"><asp:TextBox ID="txtMobilePhone" runat="server"  Width="250" ></asp:TextBox></td>
        </tr>   
        <tr> 
            <td colspan="2"><asp:Label ID="lblCompany" runat="server" Text="Company Name:"></asp:Label></td>
            <td colspan="2">
                <asp:TextBox ID="txtCompany"  Width="400" runat="server"></asp:TextBox>
<%--                <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ErrorMessage="Company Name is required." ControlToValidate="txtCompany" ForeColor="Red"></asp:RequiredFieldValidator>
--%>            
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
            <td colspan="2"><asp:RadioButton id="Highspeed" Text="I have a high speed internet connection." Checked="True" GroupName="Compressed" runat="server"/></td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
            <td colspan="2"><asp:RadioButton id="Compressed" Text="I don’t have a high speed internet connection and want to view the compressed video files." GroupName="Compressed" runat="server"/></td>
        </tr>


<%--        Align Full Left

        <tr>
            <td colspan="4"><asp:RadioButton id="Highspeed" Text="I have a high speed internet connection." Checked="True" GroupName="Compressed" runat="server"/></td>
        </tr>
        <tr>
            <td colspan="4"><asp:RadioButton id="Compressed" Text="I don’t have a high speed internet connection and want to view the compressed video files." Checked="True" GroupName="Compressed" runat="server"/></td>
        </tr>
--%>
        <tr>
            <td colspan="4"><asp:Label ID="lblMessage" runat="server"></asp:Label></td>
        </tr>  
        <tr>
            <td colspan="4" align="right">
                <asp:LinkButton ID="lbRegister" runat="server" Text="" OnClick="lbRegister_Click">
                    <asp:Image ID="imgRegister" runat="server" ImageUrl="../Images/register.png" />
                </asp:LinkButton>
            </td>
        </tr>
    </table> 
</asp:Content>
