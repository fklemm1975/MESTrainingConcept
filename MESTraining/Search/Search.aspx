<%@ Page Language="vb" PageTitle = "Search" AutoEventWireup="false" CodeBehind="Search.aspx.vb" Inherits="BMPExchange.Search" MasterPageFile="~/master/BMPExchange.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit"%>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server"> 
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div>

Enter your search values in one or more of the text boxes below.</br>

  </br>
  </br>
        <table style="margin-left:auto; margin-right:auto;" width="900px">
            <tr>
                <td><asp:Label ID="lblFirstName" runat="server" Text="First Name:"></asp:Label></td>
                <td><asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox></td>
                <td><asp:Label ID="lblMiddleName" runat="server" Text="Middle Name:"></asp:Label></td>
                <td><asp:TextBox ID="txtMiddleName" runat="server"></asp:TextBox></td>
                <td><asp:Label ID="lblLastName" runat="server" Text="Last Name:"></asp:Label></td>
                <td><asp:TextBox ID="txtLastName" runat="server"></asp:TextBox></td>
            </tr>

<%--            <tr>
                <td><asp:Label ID="lblStreet" runat="server" Text="Street:"></asp:Label></td>
                <td><asp:TextBox ID="txtStreet" runat="server"></asp:TextBox></td>
                <td><asp:Label ID="lblCity" runat="server" Text="City:"></asp:Label></td>
                <td><asp:TextBox ID="txtCity" runat="server"></asp:TextBox></td>
                <td><asp:Label ID="lblCounty" runat="server" Text="County:"></asp:Label></td>
                <td><asp:TextBox ID="txtCounty" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblState" runat="server" Text="State:"></asp:Label></td>
                <td><asp:TextBox ID="txtState" runat="server"></asp:TextBox></td>
                <td><asp:Label ID="lblZipCode" runat="server" Text="Zip Code:"></asp:Label></td>
                <td><asp:TextBox ID="txtZipCode" runat="server"></asp:TextBox></td>
                <td><asp:Label ID="lblDriversLicense" runat="server" Text="Driver's License:"></asp:Label></td>
                <td><asp:TextBox ID="txtDriversLicense" runat="server"></asp:TextBox></td>
            </tr>
--%>
            <tr>
                <td><asp:Label ID="lblCompany" runat="server" Text="Company:"></asp:Label></td>
                <td><asp:TextBox ID="txtCompany" runat="server"></asp:TextBox></td>
                <td><asp:Label ID="lblIssueDate" runat="server" Text="Issue Date:"></asp:Label></td>
                <td><asp:TextBox ID="txtIssueDate" runat="server"></asp:TextBox></td>
                <ajaxToolkit:CalendarExtender ID="ceIssueDate" runat="server" format="d" TargetControlID="txtIssueDate"></ajaxToolkit:CalendarExtender>
                <td><asp:Label ID="lblCertificationNumber" runat="server" Text="Certification Number:"></asp:Label></td>
                <td><asp:TextBox ID="txtCertificationNumber" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="6">
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr><td colspan="6">&nbsp;</td></tr>
            <tr><td colspan="6" align="right"><asp:LinkButton ID="lbSearch" runat="server" Text="Search" OnClick="lbSearch_Click"></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td></tr>
            <tr><td colspan="6">&nbsp;</td></tr>
            <tr><td colspan="6">&nbsp;</td></tr>
        </table>
    </div>
    
    <br />

        <asp:GridView ID="gvSearch" runat="server" EmptyDataText="No Data Available" RowStyle-BackColor="#B0C4DE" AlternatingRowStyle-BackColor="White"
                      HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" Font-Size="8pt" AllowPaging="true" PageSize="20" 
                      AutoGenerateColumns="false" RowStyle-CssClass="GridView" RowStyle-Wrap="False" Width="900px">
            <Columns>
                <asp:BoundField DataField="CertificationName" HeaderText="Certification Name"/>
                <asp:BoundField DataField="FirstName" HeaderText="First Name"/>
                <asp:BoundField DataField="MiddleName" HeaderText="Middle Name"/>
                <asp:BoundField DataField="LastName" HeaderText="Last Name"/>
                <asp:BoundField DataField="EndDate" HeaderText="Date Issued"/>
                <asp:BoundField DataField="ExpirationDate" HeaderText="Date Expired"/>
                
<%--                <asp:ButtonField Text="View Certificate" CommandName="ViewCertificate"/>--%>
                
                <asp:BoundField DataField="UserSummaryID" ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone" />
            </Columns>
        </asp:GridView>
    <br />
    <br />

</asp:Content>
