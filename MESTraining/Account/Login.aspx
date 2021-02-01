<%@ Page Language="vb" Debug="true" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="MESTraining.MESLogin" MasterPageFile="~/master/BMPExchange.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server" >
    <br />
<p class="BDHeader">
    <asp:table
    <table width="900px" style="margin-left:auto; margin-right:auto; margin-top:0; margin-bottom:0;height:"100%">
        <tr align="center">
            <td colspan="2" style="color:#04296e; font-weight:bold; font-family:Cambria; font-size:22pt">Marine Contractors Licensing Board Training Center
        </td>
        </tr>
        <tr align="center">
            <td colspan="2" style="color:#04296e; font-weight:bold; font-family:Cambria; font-size:16pt";  >This online trainins is intended to help contractors fulfull the traiing requirement.  Once all modules save and complete this traiing will award 6 hours of Continuing Education.<br< /<br /><br />
Please keep in mind: To renew a marine contractor's license a licensee needs to submit at the time of license renewal to the MCLB satisfactory proof of completeion of 12 hours of approved Continuing
Education.  Approved continuing education needs to be completed by the representative named on the license issued to the cmopany/entity.  The Continuing Education should be performed during the current license period.
</td>
        </tr>
        <tr align="center">
            <td colspan="2" style="color:#04296e; font-family:Cambria; font-size:12pt">&nbsp;</td>
        </tr>
        <tr align="center">
            <td colspan="2" style="color:#04296e; font-family:Cambria; font-size:12pt">&nbsp;</td>
        </tr>

        
        <tr>
            <td style="width:15px">&nbsp;</td>
            <td align="center" colspan="2" > 
                <br /> 
                <br />
                    <table style="text-align:left; width:115%;">
                        <tr align="left" valign="middle">
                            <td align="center">
                                <table id="Table1" runat="server" style="color:#eaeefa; text-align:center; width:366px;">
                                    <tr>
                                        <td align="right" nowrap="nowrap">Email Address:&nbsp;</td>
                                        <td align="left">
                                            <asp:TextBox ID="txtEmailAddress" runat="server" Width="160px" Wrap="False"></asp:TextBox>
                                        </td>
                                        <td align="left">&nbsp;</td>
                                        <td align="left" nowrap="nowrap">
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Not a valid email address" ControlToValidate="txtEmailAddress" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">Password:&nbsp;</td>
                                        <td align="left">
                                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="164px" Wrap="False"></asp:TextBox>
                                        </td>
                                        <td align="left">&nbsp;</td>
                                        <td align="left" nowrap="nowrap"></td>
                                    </tr>
                                    <tr>
                                        <td align="right">Remember Me?&nbsp;</td>
                                        <td align="left">
                                            <asp:CheckBox ID="cbxRememberMe" runat="server" text=""/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">&nbsp;</td>
                                        <td align="left">
                                            <asp:ImageButton ID="lbLogin2" runat="server" OnClick="lbLogin_Click" ImageUrl="../Images/login.png">

                                            </asp:ImageButton>

<%--                                                <asp:LinkButton ID="lbLogin" runat="server" Text="" OnClick="lbLogin_Click">
                                                    <asp:Image ID="imgLogin" runat="server" ImageUrl="../Images/login.png" />
                                                </asp:LinkButton>
--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr><td colspan="2">&nbsp;</td></tr>
                                    <tr><td align="left">&nbsp;</td><td align="left"></td></tr>         
                                    <tr><td colspan="2"><a href="ForgotPassword.aspx">I forgot my password</a></td></tr>
                                    <tr><td colspan="2">&nbsp;</td></tr>
                                    <tr><td align="left">&nbsp;</td><td align="left"></td></tr>         
                                    <tr><td colspan="2" nowrap="nowrap">If it's your first time to the MDE Certification web site, please</td></tr>
                                    <tr>
                                        <td align="left">&nbsp;</td>
                                        <td align="left">
                                            <asp:LinkButton ID="lbRegister" runat="server" Text="" OnClick="lbRegister_Click">
                                                <asp:Image ID="imgRegister" runat="server" ImageUrl="../Images/register.png" />
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                     <tr><td colspan="2">&nbsp;</td></tr>
                                     <tr><td colspan="2">&nbsp;</td></tr>
                               </table>                 
                            </td>
                        </tr>
                    </table>    
                <br />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" style="width:691px"></td>
        </tr>
    </table>

    <script type="text/javascript" language="JavaScript">
        window.history.forward(1);
    </script>
</asp:Content>
