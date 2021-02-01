Imports System.Web.Security
Imports System.Security.Cryptography
Imports System.Data
Imports SimpleEncryption
Imports MESTraining.BMPXDto

Public Class ChangePassword
    Inherits System.Web.UI.Page

    Dim db As BMPXDB = New BMPXDB()
    Private l As BMPXLib = New BMPXLib

    Private iBMPXUserID As Integer

    Private sEKey As String = My.Settings.eKey

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim bDBIsNothing As Boolean = db Is Nothing
        If bDBIsNothing Then db = New BMPXDB

        Dim i As Integer

        If Not IsNothing(Session("BMPXUserID")) Then
            If Session("BMPXUserID").ToString.Trim.Length > 0 Then
                If Integer.TryParse(Session("BMPXUserID"), i) Then

                    iBMPXUserID = i
                Else
                    Response.Redirect(GetURL("Login.aspx?MSG=Access Denied&Color=Red"))
                End If
            Else
                Response.Redirect(GetURL("Login.aspx?MSG=Access Denied&Color=Red"))
            End If
        Else
            Response.Redirect(GetURL("Login.aspx?MSG=Access Denied&Color=Red"))
        End If


    End Sub

    Private Sub btSave_Click(sender As Object, e As EventArgs) Handles btSave.Click

        Dim ds As DataSet

        Dim sPassword As String = BitConverter.ToString(SHA512Managed.Create().ComputeHash(ASCIIEncoding.ASCII.GetBytes(Me.txtOldPassword.Text)))
        sPassword = sPassword.Replace("-", "")
        sPassword = sPassword.Insert(0, "0x")

        ds = db.VerifyLogin(Session("EmailAddress"), sPassword)

        If ds.Tables(0).Rows.Count > 0 Then
            If Me.txtNewPassword.Text = Me.txtConfirm.Text Then
                Dim dto As BMPXUserDto = New BMPXUserDto()

                Dim sNewPassword As String = BitConverter.ToString(SHA512Managed.Create().ComputeHash(ASCIIEncoding.ASCII.GetBytes(Me.txtNewPassword.Text)))
                sNewPassword = sNewPassword.Replace("-", "")
                sNewPassword = sNewPassword.Insert(0, "0x")

                dto.Populate(iBMPXUserID)
                dto.Password = sNewPassword
                dto.UpdatedByID = Me.Session("BMPXUserID")
                dto.Save()

                db = Nothing

                Me.lblError.Text = "Your password was changed successfully."
                Me.lblError.ForeColor = Drawing.Color.Green

            Else
                Me.lblError.Text = "The password you entered to confirm does not match your new password."
            End If

        Else
            Me.lblError.Text = "The old password you entered is incorrect."
        End If

    End Sub

    Public Function GetURL(ByRef val As String) As String
        Dim retval As String = "Default.aspx"
        Dim appPath As String = Request.ApplicationPath.Trim

        If Right(appPath, 1) <> "/" Then appPath += "/"

        retval = ResolveUrl(Request.Url.GetLeftPart(UriPartial.Authority) & appPath & val)

        Return retval

    End Function

End Class