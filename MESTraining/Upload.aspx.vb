Imports System
Imports System.IO
Imports System.Net
Imports System.Web

Public Class Upload
    Inherits System.Web.UI.Page


    Private l As CertDBLib = New CertDBLib

    Private Sub Upload_Error(sender As Object, e As EventArgs) Handles Me.Error

        Dim exp As Exception = Server.GetLastError

        If Not exp Is Nothing Then

            Response.Write(exp.Message)
            Dim s As String = ""
            If Not exp.InnerException Is Nothing Then
                s = exp.InnerException.ToString
            End If

            l.LogIt("Page Error: '" & exp.Message & vbCrLf & s)

        End If


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        'TextBox1.Text = "Upload"

        'Dim LogPath As String = HttpContext.Current.Server.MapPath("~/Logs") & "\MESTraining.txt"

        'Throw New Exception("Log Path = '" & LogPath & "'")




        Dim sCertificationTypeID As String = "Null"
        Dim sMediaType As String = "Null"
        Dim sPath As String = ""
        Try

            l.LogIt("Testing 9/8")

            Dim UploadedFilesPath As String = My.Settings.UploadedFilesPath

            'UploadedFilesPath = "D:\Projects\MES\MESTraining\MESTraining\MESTraining\Media\"     '###### Comment this line out for production



            If Request.QueryString("CertificationTypeID") IsNot Nothing Then
                sCertificationTypeID = Request.QueryString("CertificationTypeID")
                sMediaType = Request.QueryString("MediaType")

                If Directory.Exists(UploadedFilesPath & sCertificationTypeID) Then
                    'We're not creating this directory

                    l.LogIt("We're not creating this directory: '" & UploadedFilesPath & sCertificationTypeID & "'")

                Else

                    l.LogIt("Creating Directory: '" & UploadedFilesPath & sCertificationTypeID & "'")


                    Directory.CreateDirectory(UploadedFilesPath & sCertificationTypeID)
                    Directory.CreateDirectory(UploadedFilesPath & sCertificationTypeID & "\Images")
                    Directory.CreateDirectory(UploadedFilesPath & sCertificationTypeID & "\Videos")

                End If


                For Each f As String In Request.Files.AllKeys

                    Dim d As New DateTime

                    l.LogIt(d.ToLongDateString & " -  Request.Files(f): '" & f & "'")    '#######  Stopping here, publish in the morning


                    Dim file As HttpPostedFile = Request.Files(f)

                    Dim d2 As New DateTime

                    l.LogIt(d2.ToLongDateString & " -  Save this file: '" & UploadedFilesPath & sCertificationTypeID & "\" & sMediaType & "\" & file.FileName & "'")
                    '        l.LogIt(d2.ToLongDateString & " -  Save this file: '" & UploadedFilesPath & sCertificationTypeID & "\" & sMediaType & "\" & "'")

                    '        'file.SaveAs(Server.MapPath("~/Uploads/" + file.FileName))
                    '        'file.SaveAs("D:\inetpub\wwwroot\MES\MESTraining\MESTraining\Uploads\" + file.FileName)
                    file.SaveAs(UploadedFilesPath & sCertificationTypeID & "\" & sMediaType & "\" & file.FileName)

                    Dim d3 As New DateTime

                    l.LogIt(d3.ToLongDateString & " -  Finisheed Saving this file: '" & UploadedFilesPath & sCertificationTypeID & "\" & sMediaType & "\" & file.FileName & "'")
                    '        l.LogIt(d3.ToLongDateString & " -  Finisheed Saving this file: '" & UploadedFilesPath & sCertificationTypeID & "\" & sMediaType & "\" & "'")

                Next
            End If

        Catch ex As Exception
            Dim d3 As New DateTime

            l.LogIt(d3.ToLongDateString & " -  PageLoad Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & vbCrLf & "sCertificationTypeID = '" & sCertificationTypeID & "'" & vbCrLf & "sMediaType = '" & sMediaType & "'")
        End Try



    End Sub

End Class