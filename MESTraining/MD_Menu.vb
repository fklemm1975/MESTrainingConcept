Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports System.Web.UI
Imports System.Xml

Namespace MDGOV
    ''' <summary>
    ''' Custom Navigation Class.  Inherits from the ASP:Menu control.
    ''' Used to mimic the HTML output of the MD.GOV Primary Navigation Template
    ''' </summary>
    <DefaultProperty("Text"), ToolboxData("<{0}:PrimaryNavMenu runat=""server"">")> _
    Public Class PrimaryNavMenu
        Inherits System.Web.UI.WebControls.Menu
        Protected Overrides Sub OnPreRender(e As EventArgs)
            'prevent scripts from loading
            'base.OnPreRender(e);
        End Sub

        Protected Overrides Sub Render(writer As System.Web.UI.HtmlTextWriter)
            'base.Render(writer);
            'DataBind our DataSource
            Me.PerformDataBinding()

            If StaticMenuStyle.CssClass = [String].Empty Then
                writer.Write("<nav><ul>")
            Else
                writer.Write("<nav><ul class=""" + StaticMenuStyle.CssClass + """>")
            End If

            'foreach (MenuItem item in Items)
            For a As Integer = 0 To Items.Count - 1
                'if it is the last child, put in last child class
                If a < (Items.Count - 1) Then
                    writer.Write("<li>")
                Else
                    writer.Write("<li class=""last-child"">")
                End If
                If StaticMenuItemStyle.CssClass <> [String].Empty Then
                    writer.Write("<a href=""" + Items(a).NavigateUrl + """ class=""" + StaticMenuItemStyle.CssClass + """>" + Items(a).Text + "</a>")
                Else
                    writer.Write("<a href=""" + Items(a).NavigateUrl + """>" + Items(a).Text + "</a>")
                End If
                writer.Write("</li>")
            Next
            writer.Write("</ul></nav>")

        End Sub
        'end Function: Render
    End Class
    'end Class

    ''' <summary>
    ''' Custom Navigation Class.  Inherits from the ASP:Menu control.
    ''' Used to mimic the HTML output of the MD.GOV Sub Navigation Template
    ''' </summary>
    <DefaultProperty("Text"), ToolboxData("<{0}:SubNavigation runat=""server"">")> _
    Public Class SubNavigation
        Inherits System.Web.UI.WebControls.Menu
        Protected Overrides Sub OnPreRender(e As EventArgs)
            'prevent scripts from loading
            'base.OnPreRender(e);
        End Sub

        Protected Overrides Sub Render(writer As System.Web.UI.HtmlTextWriter)
            'base.Render(writer);
            'DataBind our DataSource
            Me.PerformDataBinding()

            'Create a base section  for each Navigation Group
            For a As Integer = 0 To Items.Count - 1
                writer.Write("<section id=""subNav" + (a + 1).ToString() + """ class=""primary_left_col_list"">")
                writer.Write("<header>")
                writer.Write("<h2 data-target=""subNav" + (a + 1).ToString() + """>" + Items(a).Text + "</h2>")
                writer.Write("</header>")
                writer.Write("<div class=""subNavWrapper"">")
                writer.Write("<ul class=""cl_iconNav"">")

                'Loop through each child item in the node and print out the link.
                For c As Integer = 0 To Items(a).ChildItems.Count - 1
                    writer.Write("<li>")
                    'if Navigate URL Strings are empty, print out #/ links.
                    If Items(a).ChildItems(c).NavigateUrl <> String.Empty Then
                        writer.Write("<a href=""" + Items(a).ChildItems(c).NavigateUrl + """>" + Items(a).ChildItems(c).Text + "</a>")
                    Else
                        writer.Write("<a href=""#/"">" + Items(a).ChildItems(c).Text + "</a>")
                    End If
                    writer.Write("</li>")
                Next

                writer.Write("</ul>")
                writer.Write("</div>")
                writer.Write("</section>")
            Next
        End Sub
        'end Function: Render
    End Class

    ''' <summary>
    ''' Custom Navigation Class.  Inherits from the ASP:Menu control.
    ''' Used to mimic the HTML output of the MD.GOV Sub Navigation Template
    ''' </summary>
    <DefaultProperty("Text"), ToolboxData("<{0}:SocialMedia runat=""server"">")> _
    Public Class SocialMedia
        Inherits System.Web.UI.WebControls.Menu
        Protected Overrides Sub OnPreRender(e As EventArgs)
            'prevent scripts from loading
            'base.OnPreRender(e);
        End Sub

        Protected Overrides Sub Render(writer As System.Web.UI.HtmlTextWriter)
            'base.Render(writer);
            'DataBind our DataSource
            Try
                Me.PerformDataBinding()
            Catch
                writer.Write("Invalid or Missing Attribute.")
                Return
            End Try

            Dim icons As Integer = -1

            If Items.Count = 1 Then
                'Make Sure we have a Section for Social Media.  Should be only Child.
                If Items(0).ChildItems.Count = 2 Then
                    'Social Media Config should have two Children.  Icons and Directory
                    'Searches for the Icons Child.
                    If Items(0).ChildItems(0).Text = "Icons" Then
                        icons = 0
                    ElseIf Items(0).ChildItems(1).Text = "Icons" Then
                        icons = 1
                    Else
                        writer.Write("Icons Section is Missing.")
                        'No Icon Section Found
                        'exit gracefully.
                        Return
                    End If

                    Dim dir As Integer = 1 - icons
                    'Sets the Directory child based from the Icons child.
                    'If we have an Icons Child and and that child is Enabled, print out the Icons
                    If Items(0).ChildItems(icons).Enabled Then
                        writer.Write("<ul class=""cl_base_hNav cl_socialNav"">")
                        For Each item As MenuItem In Items(0).ChildItems(icons).ChildItems
                            If item.Enabled Then
                                writer.Write("<li><a class=""sm-" + item.Value.ToLower() + """ ")
                                If item.ToolTip <> [String].Empty Then
                                    writer.Write("title=""" + item.ToolTip + """ ")
                                End If
                                writer.Write("target=""_blank"" ")
                                writer.Write("href=""" + item.NavigateUrl + """>" + item.ToolTip + "</a></li>")
                            End If
                        Next
                        writer.Write("</ul>")
                        'If Icons are Disabled, print the Directory if it is found.
                    ElseIf Items(0).ChildItems(dir).Text = "Directory" Then
                        writer.Write("<ul class=""cl_base_hNav cl_socialNavDirectory"">")
                        writer.Write("<li><a class=""sm-directory"" ")
                        If Items(0).ChildItems(dir).ToolTip <> [String].Empty Then
                            writer.Write("title=""" + Items(0).ChildItems(dir).ToolTip + """ ")
                        End If
                        writer.Write("href=""" + Items(0).ChildItems(dir).NavigateUrl + """>" + Items(0).ChildItems(dir).ToolTip + "</a></li>")
                        writer.Write("</ul>")
                    Else
                        writer.Write("Directory Section is Missing.")
                        'No Directory Section Found.
                    End If
                Else
                    writer.Write("Social Media Section Config Error.")
                    'Social Media Section doesn't have exactly have two children.
                End If
            Else
                writer.Write("Social Media Section is Missing.")
            End If
            'Social Media section not found.

        End Sub
        'end Function: Render

    End Class
    'End Class
    ''' <summary>
    ''' Custom Navigation Class.  Inherits from the ASP:Menu control.
    ''' Used to mimic the HTML output of the MD.GOV Primary Navigation Template
    ''' </summary>
    <DefaultProperty("Text"), ToolboxData("<{0}:PrimaryNavMenu runat=""server"">")> _
    Public Class FooterMenu
        Inherits System.Web.UI.WebControls.Menu
        Protected Overrides Sub OnPreRender(e As EventArgs)
            'prevent scripts from loading
            'base.OnPreRender(e);
        End Sub

        Protected Overrides Sub Render(writer As System.Web.UI.HtmlTextWriter)
            'base.Render(writer);
            'DataBind our DataSource
            Me.PerformDataBinding()

            'Write out a basic Unordered List
            writer.Write("<ul>")

            'foreach (MenuItem item in Items)
            For a As Integer = 0 To Items.Count - 1
                writer.Write("<li>")
                writer.Write("<a href=""" + Items(a).NavigateUrl + """>" + Items(a).Text + "</a>")
                writer.Write("</li>")
            Next
            writer.Write("</ul>")

        End Sub
        'end Function: Render
    End Class
    'end Class
End Namespace
