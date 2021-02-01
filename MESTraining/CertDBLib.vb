Imports System.IO
Imports System.Data.SqlClient

Public Class CertDBLib

    Enum ItemTypeID
        Instruction = 1
        Question = 2
        NA = 3
        FollowUpQuestions = 4
    End Enum

    Public Sub LogIt(ByVal txt As String, Optional ByRef Params As CertDB.SQLParamCollection = Nothing)
        Try
            '===== from - http://www.dotnet247.com/247reference/msgs/58/291207.aspx
            Dim s As String = System.Reflection.Assembly.GetExecutingAssembly().CodeBase

            '### We need to fix this path
            Dim uu As Uri = New Uri(s)
            Dim strFileName As String = uu.LocalPath
            '===== 

            Dim LogPath As String = HttpContext.Current.Server.MapPath("~/Logs") & "\MESTraining.txt"

            Dim objReader As StreamWriter = New StreamWriter(LogPath, True)
            Dim ErrMsg As String = ""

            objReader.Write(vbCrLf & vbCrLf & DateTime.Now.ToShortDateString & " " & DateTime.Now.ToShortTimeString & " - " & txt & vbCrLf)

            'Get the calling method's name
            Dim stackTrace As StackTrace = New StackTrace()
            Dim StackFrame As StackFrame = stackTrace.GetFrame(1)
            Dim methodBase As Reflection.MethodBase = StackFrame.GetMethod()
            objReader.Write("Method Name: " + methodBase.Name & vbCrLf)

            'Get the caller of the calling method
            If stackTrace.FrameCount > 2 Then
                StackFrame = stackTrace.GetFrame(2)
                methodBase = StackFrame.GetMethod()
                objReader.Write("Method Name: " + methodBase.Name & vbCrLf)
            End If

            If (Not Params Is Nothing) Then
                For Each p As SqlParameter In Params.ParamCollection
                    If Not p.ParameterName Is Nothing And Not p.Value Is Nothing Then
                        ErrMsg += vbCrLf & vbTab & "Name: " & vbTab & p.ParameterName.ToString & vbTab & "Value: " & vbTab & p.Value.ToString
                    End If
                Next
                objReader.Write(ErrMsg)
            End If

            objReader.Close()
        Catch ex As Exception
            Throw New Exception("LogIt Error: " & ex.Message & vbCrLf & ex.InnerException.ToString & vbCrLf & txt)
        End Try

    End Sub



End Class
