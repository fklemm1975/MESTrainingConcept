Imports CertDB
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Configuration

Public Class frmLogon

#Region "Properties, Variables and Constants"

    Private dwsdb As daWSDB
    Private iLogonResult As Integer = -99

    Private iUserID As Integer = 0
    Private sEmployeeName As String = ""
    Private sEmployeeNumber As String = ""
    Private sUserName As String = ""
    Private sPassword As String = ""

    Private ttMsg As ToolTip = New ToolTip

#Region "Properties"
    Public ReadOnly Property UserID() As Integer
        Get
            UserID = iUserID
        End Get
    End Property

    Public ReadOnly Property EmployeeName() As String
        Get
            EmployeeName = sEmployeeName
        End Get
    End Property

    Public ReadOnly Property EmployeeNumber() As String
        Get
            EmployeeNumber = sEmployeeNumber
        End Get
    End Property

    Public Property UserName() As String
        Get
            UserName = sUserName
        End Get
        Set(ByVal value As String)
            sUserName = value
        End Set
    End Property

    Public Property Password() As String
        Get
            Password = sPassword
        End Get
        Set(ByVal value As String)
            sPassword = value
        End Set
    End Property

#End Region

#End Region

    Private Sub frmLogon_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.txtUserName.Text = My.Settings.UserName.ToString

        If Me.txtUserName.Text.Trim.Length > 0 Then
            Me.txtUserName.TabIndex = 7
            Me.txtPassword.Focus()
        Else
            Me.txtUserName.Focus()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub txtUserName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUserName.GotFocus
        Me.txtUserName.SelectAll()
    End Sub

    Private Sub txtPassword_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPassword.GotFocus
        Me.txtPassword.SelectAll()
    End Sub

    Private Sub btnLogon_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogon.Click
        ' iUserID = 4

        Me.pgbLogon.Visible = True
        Me.Refresh()

        Me.pgbLogon.Style = ProgressBarStyle.Marquee

        Me.pgbLogon.Refresh()
        Me.Refresh()

        Me.Cursor = Cursors.WaitCursor
        Me.bgwLogon.RunWorkerAsync()
    End Sub

    Private Sub bgwLogon_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgwLogon.RunWorkerCompleted
        pgbLogon.Style = ProgressBarStyle.Blocks
        Me.Cursor = Cursors.Default
        Me.pgbLogon.Visible = False


        Me.ttMsg = New ToolTip
        Me.ttMsg.IsBalloon = True
        Me.ttMsg.ShowAlways = True
        Me.ttMsg.UseFading = True
        Me.ttMsg.UseAnimation = True
        Me.ttMsg.ToolTipTitle = "Login Failed"
        Me.ttMsg.ToolTipIcon = ToolTipIcon.Warning

        Select Case iLogonResult
            'Case -1
            '    Me.ttMsg.Show("User does not have permission to access this application.  Please contact your administrator.", Me.txtUserName, -15, -70, 4000)
            '    'MsgBox("User does not have permission to access this application.  Please contact your administrator.", MsgBoxStyle.Exclamation, "Login Failed")
            '    Me.txtPassword.Focus()
            '    Me.txtPassword.SelectAll()
            Case 0
                Me.ttMsg.Show("Invalid User Name or Password.  Please contact your administrator.", Me.txtUserName, -15, -70, 4000)
                'MsgBox("Invalid User Name or Password.  Please try again or contact your administrator.", MsgBoxStyle.Exclamation, "Login Failed")
                Me.txtPassword.Focus()
                Me.txtPassword.SelectAll()
            Case Else
                Try
                    Me.Cursor = Cursors.WaitCursor
                    dwsdb = New daWSDB

                    Dim ds As DataSet = dwsdb.SelectByIDUser(iLogonResult)
                    Dim dr As DataRow = ds.Tables(0).Rows(0)
                    sEmployeeName = dr("FName") & " " & dr("LName")
                    sEmployeeNumber = dr("EmployeeNumber")
                    sUserName = dr("UserName")

                    My.Settings.UserName = Me.txtUserName.Text
                    Me.Close()
                Catch ex As Exception
                    Me.Cursor = Cursors.Default
                    MsgBox(ex.Message, MsgBoxStyle.Critical, "Login Error")
                Finally
                    Me.Cursor = Cursors.Default
                End Try
        End Select
    End Sub

    Private Sub bgwLogon_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bgwLogon.DoWork
        Logon()
    End Sub

    Private Function Logon() As Integer
        Try
            dwsdb = New daWSDB

            Dim s As String = Me.txtPassword.Text

            Dim password As Byte() = SHA1Managed.Create().ComputeHash(ASCIIEncoding.ASCII.GetBytes(Me.txtPassword.Text))

            iUserID = dwsdb.VerifyLogin(Me.txtUserName.Text, password)

            If UserID > 0 Then
                iLogonResult = UserID
            Else
                iLogonResult = 0
            End If

            'iLogonResult = 10

            dwsdb = Nothing
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox("Login Error", MsgBoxStyle.Critical, "Login Error")
        Finally
            dwsdb = Nothing
        End Try
    End Function

End Class