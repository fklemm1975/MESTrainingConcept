Imports System.Drawing
Imports System.Data
Imports System.Security.Cryptography
Imports System.Net.Mail

Public Class ForgotPassword
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtEmailAddress.Focus()
        End If
    End Sub

    Private Sub btnResetMyPassword_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetMyPassword.Click
        If Page.IsValid Then

            Dim db As CertDB

            Try

                db = New CertDB

                Dim ds As New DataSet()

                ds = db.VerifyPublicUserEmail(txtEmailAddress.Text)

                If ds.Tables(0).Rows.Count > 0 Then
                    Dim rndNumber As Int32 = New Random().[Next](100000, 999999)

                    If db.ResetPublicUserPassword(txtEmailAddress.Text, SHA1Managed.Create().ComputeHash(ASCIIEncoding.ASCII.GetBytes(rndNumber.ToString()))) Then
                        Dim mmPasswordReset As New MailMessage()
                        mmPasswordReset.From = New MailAddress(My.Settings.AdminEmailAddress)
                        mmPasswordReset.[To].Add(New MailAddress(txtEmailAddress.Text))
                        mmPasswordReset.Subject = "MDE Certification - Account Recovery"
                        mmPasswordReset.Body = (vbLf & "A request was made to reset your MDE Certification password. Your logon email and password are as follows: " & vbLf & vbTab & "Logon Email: " & txtEmailAddress.Text & vbLf & vbTab & "Password: " & rndNumber.ToString & vbLf & vbLf & "Please change your password by clicking 'My Information' at the bottom of the MDE Certification main page after you have logged on." & vbLf)


                        Dim mailer As SmtpClient = New SmtpClient(My.Settings.SmtpClient)

                        If My.Settings.PickupDirectoryFromIIS Then
                            mailer.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis
                        End If

                        mailer.Send(mmPasswordReset)

                        lblPasswordReset.ForeColor = Color.Red
                        lblPasswordReset.Text = "Your password has been reset, you should receive it in an email shortly."
                    End If
                Else
                    lblPasswordReset.ForeColor = Color.Red
                    lblPasswordReset.Text = "The email address does not exist in the Certification database, please check."
                End If
            Catch ex As Exception

                Dim sInnerException As String = ""
                'If Not ex.InnerException Is Nothing Then
                '    sInnerException = sInnerException
                'End If

                Dim l As CertDBLib = New CertDBLib
                l.LogIt("btnResetMyPassword_Click Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)
                l = Nothing

                If Not db Is Nothing Then db.Dispose()
                db = Nothing

            Finally
                If Not db Is Nothing Then db.Dispose()
                db = Nothing
            End Try
        End If
    End Sub
End Class