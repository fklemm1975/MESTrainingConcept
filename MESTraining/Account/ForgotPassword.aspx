<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ForgotPassword.aspx.vb" Inherits="SSDSCertification.ForgotPassword" MasterPageFile="~/MasterPage/MDE.Master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <table width="100%" style="margin-left:auto; margin-right:auto; margin-top:0; margin-bottom:0;height:"100%">
                <tr align="center">
                    <td colspan="2" style="color:#04296e; font-weight:bold; font-family:Cambria; font-size:24pt">Forgot Password
                    </td>
                </tr>
    </table>

     <table  align="center" width="743px">
            <tr>
                <td colspan="2">Your password together with your email addrss allows you to login onto MDE Certification Website<br /><br /></td>
            </tr>
            <tr>
                <td colspan="2">Please enter your email address below, and your password will be reset to a randow number and sent to you through email<br /><br /></asp:Label></td>
            </tr>
             <tr>
                <td colspan="2"></td>
            </tr>  
            <tr>
                <td align="right" style="width:213px"><asp:Label ID="lblYourEmail" runat="server" Text="Your Email:"></asp:Label></td>
                <td align="left"><asp:TextBox ID="txtEmailAddress"  runat="server" Width="175px"></asp:TextBox>
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ErrorMessage="Please enter your password" ControlToValidate="txtEmailAddress"></asp:RequiredFieldValidator>
                </td>
             </tr>
             <tr><td  colspan="2" align="center">
                  <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                        ErrorMessage="Not a valid email address" 
                        ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                        ControlToValidate="txtEmailAddress"></asp:RegularExpressionValidator>                  
             </td></tr>   
           <tr>
                <td colspan="2"></td>
            </tr>  
             <tr>
                <td colspan="2"></td>
            </tr>  
             <tr>
                <td colspan="2" align="right"><asp:Button ID="btnResetMyPassword" runat="server" Text="Reset My Password" /></td>
            </tr>  
            <tr>
                <td colspan="2"></td>
            </tr>  
            <tr>
                <td colspan="2"></td>
            </tr>  
            <tr>
                <td colspan="2"><asp:Label ID="lblPasswordReset" runat="server"></asp:Label></td>
            </tr>     
    </table> 

</asp:Content>
