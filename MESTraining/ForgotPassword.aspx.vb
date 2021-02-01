Imports System.Configuration
Imports System.Net.Mail
Imports SimpleEncryption
Imports System.Data

Public Class ForgotPassword
    Inherits System.Web.UI.Page

    Private sEKey As String = My.Settings.eKey


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub


    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim db As BMPXDB = New BMPXDB()
        Dim invalid As Boolean = False

        'Ensure an email address has been entered, mark as invalid if one hasn't
        If Me.txtEmailRecover.Text.Trim.Length() > 0 Then
            Dim ds As DataSet
            Dim lostID As Integer

            'Determine if the email address belongs to an user, mark as invalid if it doesn't
            ds = db.SelectIDByEmailAddress(Me.txtEmailRecover.Text)
            If ds.Tables.Count() > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    lostID = ds.Tables(0).Rows(0)("BMPXUserID")

                    Dim message As MailMessage = New MailMessage()
                    'message.From = New MailAddress("AccountInfo_DONOTREPLY@DNRGrantsOnline.com")
                    message.From = New MailAddress(My.Settings.FromEmailAddress)
                    message.To.Add(New MailAddress(Me.txtEmailRecover.Text))
                    message.Subject = ("BMP Exchange - Account Recovery")
                    message.Body = vbCrLf & "Please use the link below to complete your password recovery. You will be asked to enter a new password." & vbCrLf & vbCrLf & _
                        GetURL("ChangePassword.aspx?ID=" & ((Crypto.Encrypt(lostID.ToString & ";" & System.Environment.MachineName, sEKey)).ToString).Replace("+", "%2B") & "a") 'Add an 'a' to the end so that we know all of the characters are included in the url in Outlook

                    Dim client As SmtpClient = New SmtpClient(My.Settings.SmtpClient) 'MESGIS.COM

                    If My.Settings.PickupDirectoryFromIIS Then
                        client.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis
                    End If
                    If My.Settings.Environment = "Prod" Then
                        client.Send(message)
                    End If
                    Response.Redirect(GetURL("Login.aspx?MSG=An account recovery email was sent to your email address.  If you do not see it within 10 minutes please check your Junk Mail folder to ensure it wasn't filtered.&Color=Green"))
                Else
                    invalid = True
                End If
            Else
                invalid = True
            End If
        Else
            invalid = True
        End If
        If invalid = True Then
            lblWarning.Text = "This email address could not be found.  You can try again or contact an administrator."
            lblWarning.ForeColor = Drawing.Color.DarkRed
        End If
    End Sub

    Public Function GetURL(ByRef val As String) As String
        Dim retval As String = "Login.aspx"
        Dim appPath As String = Request.ApplicationPath.Trim

        If Right(appPath, 1) <> "/" Then appPath += "/"

        retval = ResolveUrl(Request.Url.GetLeftPart(UriPartial.Authority) & appPath & val)

        Return retval

    End Function

End Class