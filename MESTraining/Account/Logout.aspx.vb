Public Class Logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Request.Cookies("SSDSLOGIN") IsNot Nothing) Then
            'Expire the cookie
            Response.Cookies("SSDSLOGIN").Expires = DateTime.Now.AddDays(-30)
        End If

        Session.Clear()
        Session("RegistrationID") = Nothing


    End Sub

End Class