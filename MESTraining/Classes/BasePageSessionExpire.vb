Imports Microsoft.VisualBasic

Public Class BasePageSessionExpire
    Inherits System.Web.UI.Page

    Public Sub New()
    End Sub



    Protected Overloads Overrides Sub OnInit(ByVal e As EventArgs)
        MyBase.OnInit(e)


        'It appears from testing that the Request and Response both share the 
        ' same cookie collection.  If I set a cookie myself in the Reponse, it is 
        ' also immediately visible to the Request collection.  This just means that 
        ' since the ASP.Net_SessionID is set in the Session HTTPModule (which 
        ' has already run), thatwe can't use our own code to see if the cookie was 
        ' actually sent by the agent with the request using the collection. Check if 
        ' the given page supports session or not (this tested as reliable indicator 
        ' if EnableSessionState is true), should not care about a page that does 
        ' not need session
        If Context.Session IsNot Nothing Then
            'Tested and the IsNewSession is more advanced then simply checking if 
            ' a cookie is present, it does take into account a session timeout, because 
            ' I tested a timeout and it did show as a new session
            If Session.IsNewSession Then
                ' If it says it is a new session, but an existing cookie exists, then it must 
                ' have timed out (can't use the cookie collection because even on first 
                ' request it already contains the cookie (request and response
                ' seem to share the collection)
                Dim szCookieHeader As String = Request.Headers("Cookie")
                If (szCookieHeader IsNot Nothing) AndAlso (szCookieHeader.IndexOf("ASP.NET_SessionId") >= 0) And GetPageName() <> "Error.aspx Then" Then
                    Response.Redirect("~/Login.aspx?Msg=Session expired, please log into the BMP Exchange web site.")
                End If
            End If
        End If
    End Sub

    Private Function GetPageName() As String
        Dim sPath As String = System.Web.HttpContext.Current.Request.Url.AbsolutePath
        Dim oInfo As New System.IO.FileInfo(sPath)
        Dim sFName As String = oInfo.Name

        Return sFName

    End Function

    Public Function GetURL(ByRef val As String) As String
        Dim retval As String = "Default.aspx"
        Dim appPath As String = Request.ApplicationPath.Trim

        If Right(appPath, 1) <> "/" Then appPath += "/"

        retval = ResolveUrl(Request.Url.GetLeftPart(UriPartial.Authority) & appPath & val)

        Return retval

    End Function
End Class
