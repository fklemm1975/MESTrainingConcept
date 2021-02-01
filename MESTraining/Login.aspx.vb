Imports System.Security.Cryptography
Imports SimpleEncryption
Imports System.Threading


Public Class Login
    Inherits System.Web.UI.Page

    Private sEkey As String = "mc$47G0x\4-ZnpD6(Q1!j&Xd"
    Private l As CertDBLib = New CertDBLib

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session.Clear()
        Me.txtEmailAddress.Focus()

        If Not IsPostBack Then
            If Session("RegistrationID") IsNot Nothing Then
                Response.Redirect("Logout.aspx")
            Else
                txtEmailAddress.Focus()
            End If

            'Check if the browser support cookies
            If Request.Browser.Cookies Then
                'Check if the cookies with name RSLOGIN exist on user's machine
                If Request.Cookies("SSDSLOGIN") IsNot Nothing Then
                    'Pass the user name and password to the VerifyLogin method
                    Me.ProcessCookieInfo((Crypto.Decrypt(Request.Cookies("SSDSLOGIN")("UID").ToString(), sEkey)))
                End If
            End If
        End If

    End Sub





    Protected Sub VerifyLogin(ByVal UserName As String, ByVal TextPassword As String)
        Dim Password As Byte() = SHA1Managed.Create().ComputeHash(ASCIIEncoding.ASCII.GetBytes(TextPassword))


        Dim db As CertDB
        Dim ds As DataSet
        Dim iUserID As Integer

        Try
            db = New CertDB

            If txtEmailAddress.Text <> "" Then
                ds = db.VerifyPublicUserRegistration(Me.txtEmailAddress.Text, Password)

                If ds.Tables(0).Rows.Count > 0 Then
                    iUserID = Int32.Parse(ds.Tables(0).Rows(0)("UserID").ToString())

                    'If Me.cbxRememberMe.Checked Then
                    '    If Request.Browser.Cookies Then
                    '        If Request.Cookies("SSDSLOGIN") Is Nothing Then
                    '            Response.Cookies("SSDSLOGIN").Expires = DateTime.Now.AddDays(30)
                    '            Response.Cookies("SSDSLOGIN").Item("UID") = Crypto.Encrypt(iUserID.ToString() & "," & System.Environment.MachineName, sEkey)
                    '        Else
                    '            Response.Cookies("SSDSLOGIN").Item("UID") = Crypto.Encrypt(iUserID.ToString() & "," & System.Environment.MachineName, sEkey)
                    '        End If
                    '    End If
                    'End If

                    LoginByID(iUserID)
                Else
                    lblMessage.Text = "Login Failed: Invalid Email/Password"
                End If
            Else
                lblMessage.Text = "Please enter your password"
            End If
        Catch ex As Exception

            If Not db Is Nothing Then db.Dispose()
            db = Nothing

            lblMessage.Text = ex.Message
        Finally
            If Not db Is Nothing Then db.Dispose()
            db = Nothing
        End Try
    End Sub

    Protected Sub ProcessCookieInfo(ByVal infoString As String)
        Dim passedComma As Boolean = False
        Dim IDString As String = ""
        Dim MName As String = ""

        'Parse the cookie string in the form of: "UserID,MachineName"
        For Each c As Char In infoString
            If Not passedComma Then

                If Not c = "," Then
                    IDString += c
                Else
                    passedComma = True
                End If
            Else
                MName += c
            End If
        Next

        If MName = System.Environment.MachineName Then
            LoginByID(Integer.Parse(IDString))
        Else
            If (Request.Cookies("SSDSLOGIN") IsNot Nothing) Then
                'Expire the cookie
                Response.Cookies("SSDSLOGIN").Expires = DateTime.Now.AddDays(-30)
            End If
            Response.Redirect("~/Login.aspx")
        End If
    End Sub

    Protected Sub LoginByID(ByVal UserID As Integer)
        Dim db As CertDB
        Dim bRedirect As Boolean = False
        Dim sRedirectPath As String = ""

        Try
            db = New CertDB

            ' Dim ds As DataSet = db.SelectByIDPublicUserRegistration(UserID)
            Dim dto As New CertDto.UserDto()
            'Dim dr As DataRow

            ' 10/29/19 - GRP - Just populate the dto object

            'If ds.Tables(0).Rows.Count > 0 Then
            '    dr = ds.Tables(0).Rows(0)

            '    Session("RegistrationID") = UserID.ToString

            '    Session("LastName") = dr("LastName").ToString
            '    Session("FirstName") = dr("FirstName").ToString
            '    Session("FullName") = Session("FirstName") + " " + Session("LastName")
            '    Session("UseCompressedVideos") = dr("UseCompressedVideos")

            'End If

            dto.Populate(UserID)

            Session("RegistrationID") = UserID.ToString

            Session("LastName") = dto.LName
            Session("FirstName") = dto.FName
            Session("FullName") = Session("FirstName") + " " + Session("LastName")
            Session("UseCompressedVideos") = dto.UseCompressedVideos

        Catch ex As Exception

            If Not db Is Nothing Then db.Dispose()
            db = Nothing

            Dim sInnerException As String = ""
            'If Not ex.InnerException Is Nothing Then
            '    sInnerException = sInnerException
            'End If


            Throw New Exception("LoginByID Error: " & ex.Message & sInnerException)
        Finally
            If Not db Is Nothing Then db.Dispose()
            db = Nothing
            Response.Redirect("~/Default.aspx")
        End Try
    End Sub

    'Private Sub ImageButton1_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton1.Click       'Login buton
    '    Me.VerifyLogin(Me.txtMarineEmailAddress.Text, Me.txtMarinePassword.Text)

    'End Sub
    Private Sub btLogin_Click(sender As Object, e As EventArgs) Handles btLogin.Click
        Me.VerifyLogin(Me.txtEmailAddress.Text, Me.txtPassword.Text)
    End Sub

    'Private Sub ImageButton2_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton2.Click       'Register button
    'Try
    '    Response.Redirect("~/MarineRegistration.aspx")

    'Catch ex1 As ThreadAbortException
    '    'Response.Redirect() will throw this error, we don't need to do anything with it, so we'll capture it here, instead of in the most general exception below
    '    'do nothing

    'Catch ex As Exception

    '    Dim sInnerException As String = ""
    '    'If Not ex.InnerException Is Nothing Then
    '    '    sInnerException = sInnerException
    '    'End If

    '    l.LogIt("lbRegister_Click Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)
    'End Try


    'End Sub

    Private Sub btRegister_Click(sender As Object, e As EventArgs) Handles btRegister.Click
        Try
            Response.Redirect("~/MESRegistrationNew.aspx")

        Catch ex1 As ThreadAbortException
            'Response.Redirect() will throw this error, we don't need to do anything with it, so we'll capture it here, instead of in the most general exception below
            'do nothing

        Catch ex As Exception

            Dim sInnerException As String = ""
            'If Not ex.InnerException Is Nothing Then
            '    sInnerException = sInnerException
            'End If

            l.LogIt("lbRegister_Click Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)
        End Try
    End Sub

End Class