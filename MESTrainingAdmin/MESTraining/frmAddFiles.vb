Imports MESTrainingDB.CertDB
Imports System.Net
Imports System.IO

Public Class frmAddFiles

    Private dwsdb As daWSDB

#Region "Properties"
    Private l As CertLib = New CertLib
    Public File As String = ""
    Private Folder As String = ""
#End Region

    Private Sub frmAddFiles_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            BindListBox()
        Catch ex As Exception
            l.LogIt("frmAddFiles Load Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub BindListBox()
        Try
            '09/10/2020 - GRP - We're not using FTP anymore, use the web sevice
            'Dim ftp As Utilities.FTP.FTPclient = New Utilities.FTP.FTPclient(l.FTPSite + Folder, l.FTPUserName, l.FTPPassword)
            'Dim dirList As Utilities.FTP.FTPdirectory = ftp.ListDirectoryDetail("/")
            'Dim Files As Utilities.FTP.FTPdirectory = dirList.GetFiles()

            'For Each File As Utilities.FTP.FTPfileInfo In Files
            '    Me.lbFTPFiles.Items.Add(File.Filename)
            'Next


            Dim bDBIsNothing = dwsdb Is Nothing
            If bDBIsNothing Then dwsdb = New daWSDB

            Dim s As String = ""
            Dim sArr As String()
            Dim i As Integer

            s = dwsdb.GetFiles(My.Settings.MediaDir & Folder)
            's = "D:\Projects\MES\MESTraining\MESTraining\MESTraining\Media\" & Folder   'For debugging local

            sArr = s.Split(";")
            For i = 0 To sArr.GetUpperBound(0)
                If sArr(i).Trim.Length > 0 Then
                    Me.lbFTPFiles.Items.Add(sArr(i))
                End If
            Next


        Catch ex As Exception
            Throw New Exception("BindListBox Error: " & ex.Message)
        End Try
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Try
            File = lbFTPFiles.SelectedItem
            Me.Close()
        Catch ex As Exception
            l.LogIt("btnOk_Click Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

#Region "Constructors"

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Sub New(ByVal CertificationTypeID As Integer, ByVal f As String)
        InitializeComponent()

        Folder = CertificationTypeID & "\" & f & "\"
    End Sub

#End Region

End Class