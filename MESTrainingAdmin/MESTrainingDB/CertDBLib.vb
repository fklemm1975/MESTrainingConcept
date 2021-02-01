Imports System.IO
Imports System.Data.SqlClient

Public Class CertDBLib

    Public Sub LogIt(ByVal txt As String, Optional ByRef Params As CertDB.SQLParamCollection = Nothing)
        'Dim mbs As Integer = IIf(MessageBoxStyle = -1, MsgBoxStyle.Critical, MessageBoxStyle)
        Try
            '===== from - http://www.dotnet247.com/247reference/msgs/58/291207.aspx
            Dim s As String = System.Reflection.Assembly.GetExecutingAssembly().CodeBase

            '### We need to fix this path
            Dim uu As Uri = New Uri(s)
            Dim strFileName As String = uu.LocalPath
            '===== 

            Dim LogPath As String = strFileName & "_ErrorLog.txt"

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
                    ErrMsg += vbCrLf & vbTab & "Name: " & vbTab & p.ParameterName & vbTab & "Value: " & vbTab & p.Value
                Next
                objReader.Write(ErrMsg)
            End If

            objReader.Close()
        Catch ex As Exception
        End Try

    End Sub

End Class
