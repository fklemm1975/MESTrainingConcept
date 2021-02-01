Imports System.Net

Public Class frmFTP

    Public Class ExtendedWebClient
        '09/03/2020 - GRP - From http://geekswithblogs.net/SoftwareDoneRight/archive/2010/09/14/supporting-request-timeout-with-a-webclient.aspx
        '                   Because the WebClient object doesn't allow you to extend the timeout value

        Inherits WebClient

        Public Property Timeout As Integer

        Protected Overrides Function GetWebRequest(ByVal address As Uri) As WebRequest
            Dim request As WebRequest = MyBase.GetWebRequest(address)
            If request IsNot Nothing Then request.Timeout = Timeout
            Return request
        End Function

        Public Sub New()
            Timeout = 1048576000
        End Sub
    End Class


#Region "Properties"
    Private l As CertLib = New CertLib
    Private CertificationTypeID As Integer = 0
    Private MediaType As String = ""

#End Region

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        Me.ofdFile.InitialDirectory = "c:\"
        Me.ofdFile.Title = "Select the file to upload"
        'Me.ofdFile.Filter = "Images | *jpg;*.png;*.gif; | Videos | *.swf;*.mp4;*.flv;*.js; | All Files | *.*;"
        Me.ofdFile.Filter = "Images | *jpg;*.png;*.gif; | Videos | *.swf;*.mp4;*.flv;*.js;" '9/3/2020 GRP - We only have 2 options, so no more All Files
        Me.ofdFile.FilterIndex = 1

        Dim ofdResult As DialogResult

        ofdResult = Me.ofdFile.ShowDialog()

        If ofdResult = Windows.Forms.DialogResult.OK Then
            MediaType = IIf(ofdFile.FilterIndex = 1, "Images", "Videos")
        End If

        If Me.ofdFile.FileName.Trim.Length > 0 Then
            Try
                For Each file As String In ofdFile.FileNames
                    Me.txtFile.Text += System.IO.Path.GetFileName(file) & "; "
                Next
            Catch ex As Exception
                Me.ofdFile.FileName = ""
                l.LogIt("btnImageBrowse_Click Error:" & vbCrLf & vbCrLf & ex.Message, True, "Error")
            End Try
        End If
    End Sub

    Private Sub btnUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        Try
            Me.Cursor = Cursors.WaitCursor

            If Me.ofdFile.FileName.Trim.Length > 0 Then
                For Each file As String In ofdFile.FileNames
                    'We're not using this anymore  FTP(file, IIf(ofdFile.FilterIndex = 1, Me.CertificationTypeID & "/Images", Me.CertificationTypeID & "/Videos/"))
                    UploadFile(file)
                Next
                MsgBox("Files uploaded successfully.", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "File Upload")
                'Me.Close()
            Else
                MsgBox("Please select a file to upload.", MsgBoxStyle.Critical, "Select a File")
                Me.Cursor = Cursors.Default
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("btnUpload_Click Error:" & vbCrLf & vbCrLf & ex.Message, True, "Error")

        Finally
            Me.Cursor = Cursors.Default

        End Try

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub UploadFile(ByVal FileName As String)

        Try

            'Dim fullUploadFilePath As String = "D:\bin\test.txt"
            'Dim uploadWebUrl As String = "https://gis.menv.com/MESTraining/upload.aspx?Test01=GRP"
            'client.UploadFile(uploadWebUrl, fullUploadFilePath)

            'Dim client As New WebClient
            Dim client As New ExtendedWebClient
            client.Timeout = 1048576000

            'client.UploadFile(My.Settings.UriString & "?CertificationTypeID=" & Me.CertificationTypeID, Folder & FileName)
            'client.UploadFile(My.Settings.UriString & "?CertificationTypeID=" & Me.CertificationTypeID & "&" & "MediaType=" & MediaType, "D:\bin\test.txt")

            client.UploadFile(My.Settings.URL & "upload.aspx?CertificationTypeID=" & Me.CertificationTypeID & "&MediaType=" & MediaType, FileName)

            'client.UploadFile(My.Settings.UriString & "?CertificationTypeID=" & Me.CertificationTypeID & "&MediaType=" & MediaType, FileName)
            'client.UploadFile("http://localhost:50867/upload.aspx?CertificationTypeID=" & Me.CertificationTypeID & "&MediaType=" & MediaType, FileName)

        Catch ex As Exception
            Throw New Exception("UploadFile Error: " & ex.Message)

        End Try

    End Sub
#Region "FTP Methods"
    '===== 
    '===== 09/01/2020 GRP We're no longer using FTP.  This can all be deleted...
    '===== 


    'SFTP login
    'MESTrainingAdmin2000
    'Dr9epper%@#



    Private Sub FTP(ByVal FileName As String, ByVal Folder As String)
        Try
            '09/01/2020 - GRP - Trying something different.
            '===== Begin Old Code
            '
            'Dim ftp As Utilities.FTP.FTPclient = New Utilities.FTP.FTPclient(l.FTPSite & Folder, l.FTPUserName, l.FTPPassword)

            'If ftp.FtpFileExists(System.IO.Path.GetFileName(FileName)) Then
            '    If MsgBox("This file already exists.  Are you sure you want to overwrite this file?", MsgBoxStyle.YesNo, "Overwrite File") = DialogResult.Yes Then
            '        ftp.Upload(FileName)
            '    Else
            '        Exit Sub
            '    End If
            'Else
            '    ftp.Upload(FileName)
            'End If
            '
            '===== End Old Code

            ''From https://stackoverflow.com/questions/22030617/i-want-to-upload-files-with-a-c-sharp-windows-forms-project-to-a-webserver
            ''string fullUploadFilePath = @"C:\Users\cc\Desktop\files\test.txt";
            ''string uploadWebUrl = "http://localhost:8080/upload.aspx";
            ''client.UploadFile(uploadWebUrl , fullUploadFilePath );


            'Dim openFileDialog = New OpenFileDialog()
            'Dim dialogResult As DialogResult = openFileDialog.ShowDialog()
            'If (dialogResult <> dialogResult.OK) Then
            '    '                UploadFile(openFileDialog.FileName)
            Dim client As New WebClient
            Dim fullUploadFilePath As String = "D:\bin\test.txt"
            Dim uploadWebUrl As String = "https://gis.menv.com/MESTraining/upload.aspx?Test01=GRP"
            client.UploadFile(uploadWebUrl, fullUploadFilePath)

            'Else
            ''???
            'End If


        Catch ex As Exception
            Throw New Exception("FTP Error: " & ex.Message)
        End Try
    End Sub

#End Region

#Region "Constructors"

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Sub New(ByVal CertTypeID As String)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.CertificationTypeID = CertTypeID
    End Sub

#End Region

End Class