Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Data.SqlClient
Imports System.Data

Public Class BMPXLib

    Public Sub LogIt(ByVal txt As String, ByVal Path As String, Optional ByVal Params As Collection = Nothing)
        Dim ErrMsg As String = ""
        Dim LogPath As String = Path

        If LogPath.Substring(LogPath.Length - 1) <> "\" Then LogPath += "\"
        LogPath += "BMPExchangeErrorLog.txt"

        Try

            Dim objReader As StreamWriter = New StreamWriter(LogPath, True)

            objReader.Write(vbCrLf & vbCrLf & DateTime.Now.ToShortDateString & " " & DateTime.Now.ToShortTimeString & " - " & txt & vbCrLf)

            If (Not Params Is Nothing) Then
                For Each p As SqlParameter In Params
                    Try
                        ErrMsg += vbCrLf & vbTab & "Name: " & vbTab & p.ParameterName & vbTab & "Value: " & p.Value.ToString
                    Catch ex As Exception
                        Try
                            ErrMsg += vbCrLf & vbTab & "Name: " & vbTab & p.ParameterName
                        Catch ex2 As Exception
                            ErrMsg += vbCrLf & vbTab & "errror with parameter"
                        End Try
                    End Try
                Next
                objReader.Write(ErrMsg)
            End If

            objReader.Close()


            'Dim EventLog1 As New System.Diagnostics.EventLog()

            'If Not EventLog.SourceExists("RegisterTreesDB") Then
            '    EventLog.CreateEventSource("RegisterTreesDB", "Application")
            'End If
            'EventLog1.Source = "RegisterTreesDB"
            'EventLog1.WriteEvent(New EventInstance(1, 1, EventLogEntryType.Error), New String() {ErrMsg})


        Catch ex As Exception
            'WriteEvent()
            'Dim EventLog1 As New System.Diagnostics.EventLog()

            'If Not EventLog.SourceExists("RegisterTreesDB") Then
            '    EventLog.CreateEventSource("RegisterTreesDB", "Application")
            'End If
            'EventLog1.Source = "RegisterTreesDB"
            'EventLog1.WriteEvent(New EventInstance(1, 1, EventLogEntryType.Error), New String() {ErrMsg & vbCrLf & vbCrLf & ex.Message})

        End Try

    End Sub

    Public Shared Function ConvertDirectionToSql(ByVal sortExpr As String, ByVal sortDir As SortDirection, ByVal LastSortExpr As String, ByVal LastSortDir As String) As String
        Dim strNewSortDirection As String = String.Empty

        Dim strExpression As String = sortExpr
        Dim strDirection As String = IIf(sortDir = SortDirection.Ascending, "ASC", "DESC")
        Dim strLastExpression As String = LastSortExpr
        Dim strLastDirection As String = LastSortDir

        Select Case strDirection
            Case "ASC"
                If strExpression = strLastExpression Then
                    If strLastDirection = "Asc" Then
                        strNewSortDirection = "Desc"
                    Else
                        strNewSortDirection = "Asc"
                    End If
                Else
                    strNewSortDirection = "Asc"
                End If
            Case "DESC"
                If strExpression = strLastExpression Then
                    If strLastDirection = "Desc" Then
                        strNewSortDirection = "Asc"
                    Else
                        strNewSortDirection = "Desc"
                    End If
                Else
                    strNewSortDirection = "Desc"
                End If
        End Select

        Return strNewSortDirection
    End Function

    Public Sub New()
        Dim i As Integer


        i = 1


    End Sub
End Class
