<%@ Page Title="Certification" Language="vb" AutoEventWireup="false" MasterPageFile="~/master/MESTraining.Master" CodeBehind="Certification.aspx.vb" Inherits="MESTraining.Certification" %>
<%@ Register Src="~/ExamRepeaterControl.ascx" TagName="ExamRepeaterControl" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="styles/BMPDMS.css" rel="stylesheet" />


    <asp:Table ID="Table1" runat="server" HorizontalAlign="Left">
        <asp:TableRow><asp:TableCell HorizontalAlign="left" Wrap="false" CssClass="BMPXPageTitleCenter" ColumnSpan="3">  <asp:Label ID="lblCertificationName" runat="server" Text="Label"></asp:Label></asp:TableCell></asp:TableRow>

        <asp:TableRow><asp:TableCell HorizontalAlign="Left" ColumnSpan="3">&nbsp;</asp:TableCell></asp:TableRow>    <%-- This is the blank line --%>

        <asp:TableRow>
            <asp:TableCell ColumnSpan="3" HorizontalAlign="Left">Please view training videos and complete practice review questions prior to taking the exam.</asp:TableCell>
        </asp:TableRow>

        <asp:TableRow><asp:TableCell HorizontalAlign="Left" ColumnSpan="3">&nbsp;</asp:TableCell></asp:TableRow>    <%-- This is the blank line --%>
<%--        
        <asp:TableRow>
            <asp:TableCell horizontalAlign ="Left">
                <a href="Default.aspx">Training Courses</a><br />
                <a href="LogOut.aspx">Log Out</a><br /><br />
            </asp:TableCell>
        </asp:TableRow>--%>

        <asp:TableRow>
            <asp:TableCell ColumnSpan="3" HorizontalAlign="Left">Select continue to take next course section.</asp:TableCell>
        </asp:TableRow>

        <asp:TableRow>
            <asp:TableCell ColumnSpan="3">
                <asp:Button ID="btContinue" runat="server" Text="Continue" />
            </asp:TableCell>
        </asp:TableRow>

        <asp:TableRow>
            <asp:TableCell ColumnSpan="3">
                <asp:Button ID="btTakeExam" runat="server" Text="Take Exam" />
            </asp:TableCell>
        </asp:TableRow>

        <asp:TableRow><asp:TableCell HorizontalAlign="Left" ColumnSpan="3">&nbsp;</asp:TableCell></asp:TableRow>    <%-- This is the blank line --%>

        <asp:TableRow>
            <asp:TableCell ColumnSpan="3">
                <asp:Label ID="lblMessage2" runat="server" Text=""></asp:Label>
            </asp:TableCell>
        </asp:TableRow>

        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <asp:TreeView ID="tvItems" runat="server" NodeWrap="true" Width="300px"></asp:TreeView>
            </asp:TableCell>
            <asp:TableCell>
                &nbsp;&nbsp;&nbsp;
            </asp:TableCell>

            <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" CssClass="inlineStyle">
                <asp:Panel ID="pnlMain" runat="server">
                    <asp:Table ID="Table2" runat="server" HorizontalAlign="Left" Width="100%">
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Left">
                                <asp:Label ID="lblHeading" Font-Bold="true" Font-Size="Large" runat="server" Text=""></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Left">
                                <asp:Label ID="lblItemHeading" Font-Bold="true" runat="server" Text=""></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Left">
                                <asp:Label ID="lblVerbiage" runat="server" Text=""></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Panel ID="pnlVideo" runat="server" width="512" height="384">
<%--                                    <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0" width="512" height="384">
                                        <param name="movie" value="<% =swfFileName%>" />
                                        <param name="quality" value="high" />
                                        <param name="play" value="false" />
                                        <param name="allowFullScreen" value="false" />
                                        <embed src="<% =swfFileName%>" quality="high" play="false" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" width="512" height="384" allowfullscreen="true"></embed>
                                    </object>

                                    xxxxxx

                                    <object classid="clsid:02BF25D5-8C17-4B23-BC80-D3488ABDDC6B" width="500" height="400"
                                    codebase="http://www.apple.com/qtactivex/qtplugin.cab">
                                    <param name="SRC" value=<% =swfFileName%> />
                                    <param name="AUTOPLAY" value="true" />
                                    <param name="CONTROLLER" value="false" />
                                    <embed src=<% =swfFileName%> autoplay="true" controller="false" width="500" height="400"
                                    pluginspage="http://www.apple.com/quicktime/download/">
                                    </embed>
                                    </object>


                                    yyyyyy--%>


    <video width="320" height="240" controls autoplay>
<%--  	<source src=<% =swfFileName%> type="video/mp4">--%>
    <source src=”<% = swfFileName%>” type=”video/mp4”> 

  	Your browser does not support the video tag.
	</video>
<%--	zzzzz--%>

                                </asp:Panel>

                                <asp:Panel ID="pnlCompressed" runat="server" width="512" height="384" visible="false">
                                    <video width="480" height="320" controls="controls">
                                        <source src="<% =swfFileName%>" type="video/mp4">
                                    </video>
                                </asp:Panel>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:label ID="lblVideoCaption" runat="server"></asp:label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Panel ID="pnlImage" runat="server">
                                    <asp:Image ID="iImage" runat="server" />
                                </asp:Panel>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:label ID="lblImageCaption" runat="server"></asp:label>
                            </asp:TableCell>
                        </asp:TableRow>

                        <asp:TableRow><asp:TableCell HorizontalAlign="Left" ColumnSpan="3">&nbsp;</asp:TableCell></asp:TableRow>    <%-- This is the blank line --%>

                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:HyperLink ID="hlStateManual" runat="server" Target="_blank"></asp:HyperLink>        
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:HyperLink ID="hlRPCpt1" runat="server" Target="_blank"></asp:HyperLink>        
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:HyperLink ID="hlRPCpt2" runat="server" Target="_blank"></asp:HyperLink>        
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:table>
                </asp:Panel>
                <asp:Panel ID="pnlFollowUpQuestions" runat="server" Visible="false">
                    <asp:Repeater runat="server" ID="rptFollowUpQuestions">
                        <ItemTemplate>
                            <uc1:ExamRepeaterControl id="ExamRepeaterControl1" runat="server" />
                        </ItemTemplate>
                    </asp:Repeater>
                </asp:Panel>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" ColumnSpan="3">
                <asp:Button ID="btSaveCont" runat="server" Text="Continue" />&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btTakeExam2" runat="server" Text="Take Exam" />
            </asp:TableCell>
        </asp:TableRow>

        <asp:TableRow>
            <asp:TableCell ColumnSpan="3">
                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
            </asp:TableCell>
        </asp:TableRow>



    </asp:Table>

    


</asp:Content>
