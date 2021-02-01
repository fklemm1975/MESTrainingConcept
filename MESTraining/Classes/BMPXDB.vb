Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports System.Configuration
Imports SimpleEncryption
Imports System.IO
Imports System.Net

Public Class BMPXDB

#Region "Variables and Properties"
    Dim isDisposed As Boolean = False
    Dim eKey As String = "gP/7G0x\4-ZnpD6(Q1!j&Xd"

    Public RowCount As Integer = 0
    Public Identity As Integer = 0
    Public ErrorCode As Integer = 0
    Public ValidationCode As Integer = -1   '100 = Ok, 101 = Email Address Exists

    Private Conn As SqlConnection
    Private Cmd As SqlCommand
    Private da As SqlDataAdapter

    Private ConnStr As String = Crypto.Decrypt(ConfigurationManager.ConnectionStrings("my_connection").ToString(), eKey)

#Region "Properties"

    Private iTimeout As Integer = 30
    Private Outputs As Collection = New Collection

    Public Property Timeout() As Integer
        Get
            Timeout = iTimeout
        End Get
        Set(ByVal value As Integer)
            iTimeout = value
        End Set
    End Property
#End Region

#End Region


#Region "SQL Param Collection"

    Private Class SQLParamCollection
        'You can't instantiate the SqlParameterCollection as a variable, you have to get it from the command object
        'which defeats the purpose of passing around a collection.  So we'll create this class and pass it.

        Public ParamCollection As Collection

        Public Sub New(Optional ByVal ParamName As String = "", Optional ByVal Value As Object = Nothing)
            ParamCollection = New Collection
            If (ParamName.Length > 0) Then
                AddWithValue(ParamName, Value)
            End If
        End Sub

        Public Sub AddWithValue(ByVal ParamName As String, ByVal Value As Object, Optional ByVal SQLDBDataType As Object = Nothing, Optional ByVal ParamDirection As Object = Nothing)
            Dim p As SqlParameter = New SqlParameter
            p.ParameterName = ParamName
            If ParamDirection Is Nothing Then p.Value = Value
            If Not SQLDBDataType Is Nothing Then p.SqlDbType = SQLDBDataType
            If Not ParamDirection Is Nothing Then
                p.Direction = ParamDirection
            Else
                p.Direction = ParameterDirection.Input
            End If

            ParamCollection.Add(p)
        End Sub
    End Class

#End Region

#Region "New, Finalize and Dispose"

    Public Sub New()
        'Try
        '    strConn = Crypto.Decrypt(ConfigurationManager.AppSettings("my_Connection"), eKey)

        '    Conn = New SqlConnection(strConn)
        '    Conn.Open()
        'Catch excep As Exception
        '    Throw New ApplicationException(excep.Message)
        'End Try

        Dim stackTrace As StackFrame
        Dim sMethodName As String = ""

        Try
            Me.Conn = New SqlConnection(ConnStr)
            Me.Conn.Open()
            Me.RowCount = 0
            Me.Identity = 0
        Catch ex As Exception
            Try
                stackTrace = New StackFrame(1)
                sMethodName = stackTrace.GetMethod.Name
            Catch ex2 As Exception
                sMethodName = "<unknown>"
            End Try
            LogIt("Create DB Object failed: Caller = " & sMethodName & vbCrLf & ex.Message)
            Throw (New Exception("Unable to connect to the database.  Please check your configuration settings or contact your adminstrator." & vbCrLf))
        End Try



    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        Try
            Me.Conn.Close()
            Me.Conn.Dispose()
            Me.Conn = Nothing

            Me.Cmd.Dispose()
            Me.Cmd = Nothing

            Me.da.Dispose()
            Me.da = Nothing

        Catch ex As Exception

        End Try
    End Sub

    Protected Overloads Sub Dispose(ByVal disposing As Boolean)
        If Not isDisposed Then
            If disposing Then
                If (Not (Conn) Is Nothing) Then
                    Conn.Close()
                    Conn.Dispose()
                End If

            End If

        End If

        isDisposed = True
    End Sub

    Public Overloads Sub Dispose()
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region


#Region "Select Methods"

    Public Function SelectByIDBMPXUser(ByVal BMPXUserID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@BMPXUserID", BMPXUserID)
        Return SelectCommand("SelectByIDBMPXUser", Params)
    End Function

    Public Function SelectIDByEmailAddress(ByVal EmailAddress As String) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@EmailAddress", EmailAddress)
        Return SelectCommand("SelectIDByEmailAddress", Params)
    End Function

    Public Function SelectByIDAgency(ByVal AgencyID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@AgencyID", AgencyID)
        Return SelectCommand("SelectByIDAgency", Params)
    End Function

    Public Function SelectByIDSubmission(ByVal SubmissionID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@SubmissionID", SubmissionID)
        Return SelectCommand("SelectByIDSubmission", Params)
    End Function

    Public Function SelectByIDUploadedFiles(ByVal UploadedFilesID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@UploadedFilesID", UploadedFilesID)
        Return SelectCommand("SelectByIDUploadedFiles", Params)
    End Function

    Public Function SelectByBMPXUserIDUploadedFiles(ByVal BMPXUserID As Integer, ByVal FiscalYearID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@BMPXUserID", BMPXUserID)
        Params.AddWithValue("FiscalYearID", FiscalYearID)

        Return SelectCommand("SelectByBMPXUserIDUploadedFiles", Params)
    End Function

    Public Function SelectByIDNotification(ByVal NotificationID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@NotificationID", NotificationID)
        Return SelectCommand("SelectByIDNotification", Params)
    End Function

    Public Function SelectSubmittedDataByFiscalYearID(ByVal FiscalYearID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@FiscalYearID", FiscalYearID)

        Return SelectCommand("SelectSubmittedDataByFiscalYearID", Params)
    End Function

    Public Function SelectFiscalYear(ByVal FiscalYearID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@FiscalYearID", FiscalYearID)

        Return SelectCommand("SelectFiscalYear", Params)
    End Function

    Public Function SelectAllUser() As DataSet
        Return SelectCommand("SelectAllUser")
    End Function

    Public Function SelectAllAgency() As DataSet
        Return SelectCommand("SelectAllAgency")
    End Function

    Public Function SelectAllFiscalYear() As DataSet
        Return SelectCommand("SelectAllFiscalYear")
    End Function

    Public Function SelectAllNotification() As DataSet
        Return SelectCommand("SelectAllNotification")
    End Function

    Public Function SelectActiveNotification() As DataSet
        Return SelectCommand("SelectActiveNotification")
    End Function

    Public Function SelectBySubmissionIDStagingBMP(ByVal SubmissionID As Integer) As DataSet

        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@SubmissionID", SubmissionID)

        Return SelectCommand("SelectBySubmissionIDStagingBMP", Params)

    End Function

    Public Function SelectBySubmissionIDStagingEvent(ByVal SubmissionID As Integer, ByVal Type As Integer) As DataSet

        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@SubmissionID", SubmissionID)
        Params.AddWithValue("@Type", Type)

        Return SelectCommand("SelectBySubmissionIDStagingEvent", Params)

    End Function

    Public Function SelectBySubmissionIDStagingManureTransport(ByVal SubmissionID As Integer) As DataSet

        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@SubmissionID", SubmissionID)

        Return SelectCommand("SelectBySubmissionIDStagingManureTransport", Params)

    End Function










    Public Function VerifyLogin(ByVal EmailAddress As String, ByVal Password As String) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@EmailAddress", EmailAddress)
        Params.AddWithValue("@Password", Password)
        Params.AddWithValue("@NetworkID", System.Environment.UserName)
        Params.AddWithValue("@MachineName", System.Environment.MachineName)

        Dim HostIP As IPAddress
        HostIP = Dns.GetHostEntry(Dns.GetHostName).AddressList.GetValue(0)
        Params.AddWithValue("@IPAddress", HostIP.ToString)

        Return SelectCommand("VerifyLogin", Params)

    End Function

    Public Function LogEvent(ByVal EventTypeID As Integer, ByVal EventSubTypeID As Object, ByVal BMPXUserID As Integer, ByVal EmailAddress As String, ByVal MachineName As String, ByVal NetworkID As String, ByVal IPAddress As String) As Boolean

        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@EventTypeID", EventTypeID)
        Params.AddWithValue("@EventSubTypeID", EventSubTypeID)
        Params.AddWithValue("@BMPXUserID", BMPXUserID)
        Params.AddWithValue("@EmailAddress", EmailAddress)
        Params.AddWithValue("@MachineName", MachineName)
        Params.AddWithValue("@NetworkID", NetworkID)
        Params.AddWithValue("@IPAddress", IPAddress)

        LogEvent = SaveCommand("LogEvent", Params, False)

    End Function

    Public Function SelectAllEventLog() As DataSet
        Return SelectCommand("SelectAllEventLog")
    End Function

    Private Function SelectCommand(ByVal QueryName As String, Optional ByVal Params As SQLParamCollection = Nothing) As DataSet

        Dim ds As DataSet = New DataSet()
        Dim ErrMsg As String = ""

        Try
            Cmd = New SqlCommand(QueryName, Conn)
            Cmd.CommandType = CommandType.StoredProcedure
            Cmd.CommandTimeout = Timeout

            da = New SqlDataAdapter(Cmd)

            If (Not Params Is Nothing) Then
                For Each p As SqlParameter In Params.ParamCollection
                    da.SelectCommand.Parameters.AddWithValue(p.ParameterName, p.Value)
                Next
            End If

            RowCount = da.Fill(ds)

            Return ds

        Catch ex As Exception
            Try
                LogIt("SelectCommand: " & QueryName & vbCrLf & ex.Message, Params)
                Throw New Exception("SelectCommand: " & QueryName & vbCrLf & ex.Message)
            Catch ex2 As Exception
                Throw New Exception("SelectCommand and LogIt: " & QueryName & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex2.Message)
            End Try
        End Try

        Return ds
    End Function

#End Region

#Region "Save Methods"

    Public Function SaveBMPXUser(ByRef dto As BMPXDto.BMPXUserDto) As Boolean

        Dim Params As SQLParamCollection = New SQLParamCollection()
        Dim retval As Boolean = False

        Params.AddWithValue("@BMPXUserID", dto.BMPXUserID)
        Params.AddWithValue("@FirstName", dto.FirstName)
        Params.AddWithValue("@LastName", dto.LastName)
        Params.AddWithValue("@EmailAddress", dto.EmailAddress)
        Params.AddWithValue("@Password", dto.Password)
        Params.AddWithValue("@Agency", dto.Agency)
        Params.AddWithValue("@Address", dto.Address)
        Params.AddWithValue("@Address2", dto.Address2)
        Params.AddWithValue("@City", dto.City)
        Params.AddWithValue("@State", dto.State)
        Params.AddWithValue("@Zip", dto.Zip)
        Params.AddWithValue("@Administrator", dto.Administrator)
        Params.AddWithValue("@Active", dto.Active)
        Params.AddWithValue("@AccountLocked", dto.AccountLocked)
        Params.AddWithValue("@PasswordResetRequired", dto.PasswordResetRequired)
        Params.AddWithValue("@FailedLoginAttempts", dto.FailedLoginAttempts)
        Params.AddWithValue("@CreatedByID", dto.CreatedByID)
        Params.AddWithValue("@UpdatedByID", dto.UpdatedByID)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)
        Params.AddWithValue("@ValidationCode", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

        retval = SaveCommand("SaveBMPXUser", Params)

        Return retval And (ValidationCode = 100)

    End Function

    Public Function SaveBMPXUserNoPassword(ByRef dto As BMPXDto.BMPXUserDto) As Boolean

        Dim Params As SQLParamCollection = New SQLParamCollection()
        Dim retval As Boolean = False

        Params.AddWithValue("@BMPXUserID", dto.BMPXUserID)
        Params.AddWithValue("@FirstName", dto.FirstName)
        Params.AddWithValue("@LastName", dto.LastName)
        Params.AddWithValue("@EmailAddress", dto.EmailAddress)
        Params.AddWithValue("@Agency", dto.Agency)
        Params.AddWithValue("@Address", dto.Address)
        Params.AddWithValue("@Address2", dto.Address2)
        Params.AddWithValue("@City", dto.City)
        Params.AddWithValue("@State", dto.State)
        Params.AddWithValue("@Zip", dto.Zip)
        Params.AddWithValue("@Administrator", dto.Administrator)
        Params.AddWithValue("@Active", dto.Active)
        Params.AddWithValue("@AccountLocked", dto.AccountLocked)
        Params.AddWithValue("@PasswordResetRequired", dto.PasswordResetRequired)
        Params.AddWithValue("@FailedLoginAttempts", dto.FailedLoginAttempts)
        Params.AddWithValue("@CreatedByID", dto.CreatedByID)
        Params.AddWithValue("@UpdatedByID", dto.UpdatedByID)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)
        Params.AddWithValue("@ValidationCode", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

        retval = SaveCommand("SaveBMPXUserNoPassword", Params)

        Return retval And (ValidationCode = 100)

    End Function

    Public Function SaveUploadedFiles(ByRef dto As BMPXDto.UploadedFilesDto) As Boolean

        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@UploadedFilesID", dto.UploadedFilesID)
        Params.AddWithValue("@BMPXUserID", dto.BMPXUserID)
        Params.AddWithValue("@FileTypeID", dto.FileTypeID)
        Params.AddWithValue("@FiscalYearID", dto.FiscalYearID)
        Params.AddWithValue("@SubmissionID", dto.SubmissionID)
        Params.AddWithValue("@Description", dto.Description)
        Params.AddWithValue("@OriginalFilename", dto.OriginalFilename)
        Params.AddWithValue("@StoredFilename", dto.StoredFilename)
        Params.AddWithValue("@UploadDate", dto.UploadDate)
        Params.AddWithValue("@Locked", dto.Locked)
        Params.AddWithValue("@LockedDate", dto.LockedDate)
        Params.AddWithValue("@Processed", dto.Processed)
        Params.AddWithValue("@ProcessedDate", dto.ProcessedDate)
        Params.AddWithValue("@Accepted", dto.Accepted)
        Params.AddWithValue("@AcceptedByID", dto.AcceptedByID)
        Params.AddWithValue("@AcceptedDate", dto.AcceptedDate)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

        Return SaveCommand("SaveUploadedFiles", Params)

    End Function

    Public Function SaveSubmissionUploadedFiles(ByRef dto As BMPXDto.SubmissionDto) As Boolean

        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@SubmissionID", dto.SubmissionID)
        Params.AddWithValue("@FiscalYearID", dto.FiscalYearID)
        Params.AddWithValue("@BMPXUserID", dto.BMPXUserID)
        Params.AddWithValue("@SubmissionDate", dto.SubmissionDate)
        Params.AddWithValue("@Processed", dto.Processed)
        Params.AddWithValue("@ProcessedDate", dto.ProcessedDate)
        Params.AddWithValue("@ValidatedSuccessfully", dto.ValidatedSuccessfully)
        Params.AddWithValue("@DataSourceCode", dto.DataSourceCode)
        Params.AddWithValue("@FirstName", dto.FirstName)
        Params.AddWithValue("@LastName", dto.LastName)
        Params.AddWithValue("@Agency", dto.Agency)
        Params.AddWithValue("@Title", dto.Title)
        Params.AddWithValue("@Email", dto.Email)
        Params.AddWithValue("@Phone", dto.Phone)
        Params.AddWithValue("@Address", dto.Address)
        Params.AddWithValue("@Address2", dto.Address2)
        Params.AddWithValue("@City", dto.City)
        Params.AddWithValue("@State", dto.State)
        Params.AddWithValue("@ZipCode", dto.ZipCode)
        Params.AddWithValue("@BMPURL", dto.BMPURL)
        Params.AddWithValue("@BMPEventURL", dto.BMPEventURL)
        Params.AddWithValue("@ManureTransportURL", dto.ManureTransportURL)
        Params.AddWithValue("@ManureTransportEventURL", dto.ManureTransportEventURL)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

        Return SaveCommand("SaveSubmissionUploadedFiles", Params)

    End Function

    Public Function SaveSubmission(ByRef dto As BMPXDto.SubmissionDto) As Boolean

        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@SubmissionID", dto.SubmissionID)
        Params.AddWithValue("@FiscalYearID", dto.FiscalYearID)
        Params.AddWithValue("@BMPXUserID", dto.BMPXUserID)
        Params.AddWithValue("@SubmissionDate", dto.SubmissionDate)
        Params.AddWithValue("@Processed", dto.Processed)
        Params.AddWithValue("@ProcessedDate", dto.ProcessedDate)
        Params.AddWithValue("@ValidatedSuccessfully", dto.ValidatedSuccessfully)
        Params.AddWithValue("@DataSourceCode", dto.DataSourceCode)
        Params.AddWithValue("@FirstName", dto.FirstName)
        Params.AddWithValue("@LastName", dto.LastName)
        Params.AddWithValue("@Agency", dto.Agency)
        Params.AddWithValue("@Title", dto.Title)
        Params.AddWithValue("@Email", dto.Email)
        Params.AddWithValue("@Phone", dto.Phone)
        Params.AddWithValue("@Address", dto.Address)
        Params.AddWithValue("@Address2", dto.Address2)
        Params.AddWithValue("@City", dto.City)
        Params.AddWithValue("@State", dto.State)
        Params.AddWithValue("@ZipCode", dto.ZipCode)
        Params.AddWithValue("@BMPURL", dto.BMPURL)
        Params.AddWithValue("@BMPEventURL", dto.BMPEventURL)
        Params.AddWithValue("@ManureTransportURL", dto.ManureTransportURL)
        Params.AddWithValue("@ManureTransportEventURL", dto.ManureTransportEventURL)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

        Return SaveCommand("SaveSubmission", Params)

    End Function

    Public Function ResubmitByBMPXUserID(ByVal BMPXUserID As Integer, ByVal FiscalYearID As Integer) As Boolean

        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@BMPXUserID", BMPXUserID)
        Params.AddWithValue("@FiscalYearID", FiscalYearID)

        Return SaveCommand("ResubmitByBMPXUserID", Params, False)

    End Function

    Public Function SaveAgency(ByRef dto As BMPXDto.AgencyDto) As Boolean

        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@AgencyID", dto.AgencyID)
        Params.AddWithValue("@OriginatingName", dto.OriginatingName)
        Params.AddWithValue("@NEIENCode", dto.NEIENCode)
        Params.AddWithValue("@DataSourceCode", dto.DataSourceCode)
        Params.AddWithValue("@SB_NAME", dto.SB_NAME)
        Params.AddWithValue("@CreatedByID", dto.CreatedByID)
        Params.AddWithValue("@UpdatedByID", dto.UpdatedByID)
        Params.AddWithValue("@UpdatedByDate", dto.UpdatedByDate)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

        Return SaveCommand("SaveAgency", Params)

    End Function

    Public Function SaveNotification(ByRef dto As BMPXDto.NotificationDto) As Boolean

        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@NotificationID", dto.NotificationID)
        Params.AddWithValue("@Text", dto.Text)
        Params.AddWithValue("@Active", dto.Active)
        Params.AddWithValue("@CreatedByID", dto.CreatedByID)
        Params.AddWithValue("@UpdatedByID", dto.UpdatedByID)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

        Return SaveCommand("SaveNotification", Params)

    End Function

    Public Function SetFiscalYearLock(ByVal FiscalYearID As Integer, ByVal Locked As Boolean) As Boolean

        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@FiscalYearID", FiscalYearID)
        Params.AddWithValue("@Locked", Locked)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

        Return SaveCommand("SetFiscalYearLock", Params)

    End Function







    Private Function SaveCommand(ByRef QueryName As String, ByRef Params As SQLParamCollection, Optional ByVal RetreiveIdentity As Boolean = True) As Boolean

        Dim RetVal As Boolean = False
        Dim s As SqlParameter = New SqlParameter


        Try
            Identity = 0

            Cmd = New SqlCommand(QueryName, Conn)

            For Each p As SqlParameter In Params.ParamCollection
                If p.Direction = ParameterDirection.Output Then
                    Dim pOut As SqlParameter = Cmd.CreateParameter()

                    pOut.ParameterName = p.ParameterName
                    pOut.Direction = ParameterDirection.Output
                    pOut.SqlDbType = p.SqlDbType
                    Cmd.Parameters.Add(pOut)

                Else
                    If p.SqlDbType = SqlDbType.NVarChar Then
                        Cmd.Parameters.AddWithValue(p.ParameterName, p.Value)
                    Else
                        Dim sp As SqlParameter = New SqlParameter()
                        sp.ParameterName = p.ParameterName
                        sp.Value = p.Value
                        sp.SqlDbType = p.SqlDbType
                        Cmd.Parameters.Add(sp)
                    End If
                End If
            Next

            Cmd.CommandType = CommandType.StoredProcedure
            Cmd.CommandTimeout = Timeout

            Cmd.ExecuteNonQuery()
            RetVal = True

            If RetreiveIdentity Then    'Some of our Save don't do inserts and don't have this param, they can just pass false
                Identity = Int32.Parse(Cmd.Parameters("@IdentityVal").Value.ToString())
            End If

            Select Case QueryName    'Handle queries with additional outputs
                Case "SaveBMPXUser", "SaveBMPXUserNoPassword"
                    ValidationCode = Int32.Parse(Cmd.Parameters("@ValidationCode").Value.ToString())

                Case Else
                    'Do Nothing
            End Select

        Catch ex As Exception
            Try
                LogIt("SaveDataCommand: " & QueryName & vbCrLf & ex.Message, Params)
                Throw New Exception("SaveDataCommand: " & QueryName & vbCrLf & ex.Message)
            Catch ex2 As Exception
                Throw New Exception("SaveDataCommand and LogIt: " & QueryName & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex2.Message)
            End Try
        End Try

        Return RetVal

    End Function

#End Region

#Region "Delete Methods"

    Public Function DeleteUploadedFilesByUploadedFilesID(ByVal UploadedFilesID As Integer) As Boolean
        Dim Params As SQLParamCollection = New SQLParamCollection()
        Params.AddWithValue("@UploadedFilesID", UploadedFilesID)
        Return DeleteCommand("DeleteUploadedFilesByUploadedFilesID", Params, False)
    End Function







    Private Function DeleteCommand(ByRef QueryName As String, ByRef Params As SQLParamCollection, Optional ByVal RetreiveErrorCode As Boolean = True) As Boolean

        Dim RetVal As Boolean = False
        Dim s As SqlParameter = New SqlParameter

        Try
            Identity = 0
            ErrorCode = 0

            Cmd = New SqlCommand(QueryName, Conn)

            For Each p As SqlParameter In Params.ParamCollection
                If p.Direction = ParameterDirection.Output Then
                    Dim pOut As SqlParameter = Cmd.CreateParameter()

                    pOut.ParameterName = p.ParameterName
                    pOut.Direction = ParameterDirection.Output
                    pOut.SqlDbType = p.SqlDbType
                    Cmd.Parameters.Add(pOut)

                Else
                    If p.SqlDbType = SqlDbType.NVarChar Then
                        Cmd.Parameters.AddWithValue(p.ParameterName, p.Value)
                    Else
                        Dim sp As SqlParameter = New SqlParameter()
                        sp.ParameterName = p.ParameterName
                        sp.Value = p.Value
                        sp.SqlDbType = p.SqlDbType
                        Cmd.Parameters.Add(sp)
                    End If
                End If
            Next

            Cmd.CommandType = CommandType.StoredProcedure
            Cmd.CommandTimeout = Timeout

            Cmd.ExecuteNonQuery()
            RetVal = True

            If RetreiveErrorCode Then    'Some of our Save don't do inserts and don't have this param, they can just pass false
                ErrorCode = Int32.Parse(Cmd.Parameters("@ErrorCode").Value.ToString())
            End If



        Catch ex As Exception
            Try
                LogIt("DeleteCommand: " & QueryName & vbCrLf & ex.Message, Params)
                Throw New Exception("DeleteCommand: " & QueryName & vbCrLf & ex.Message)
            Catch ex2 As Exception
                Throw New Exception("DeleteCommand and LogIt: " & QueryName & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex2.Message)
            End Try

        Finally
            Select Case ErrorCode
                Case 547                'The DELETE statement conflicted with the REFERENCE constraint 
                    Throw New Exception("The record can not be deleted because of a Foreign Key constraint.")
                Case Else
                    'Do nothing for now
            End Select

        End Try

        Return RetVal

    End Function


#End Region


    Private Sub LogIt(ByVal txt As String, Optional ByVal Params As SQLParamCollection = Nothing)
        Try

            '===== from - http://www.dotnet247.com/247reference/msgs/58/291207.aspx
            Dim s As String = System.Reflection.Assembly.GetExecutingAssembly().CodeBase
            s += ".config"

            Dim uu As Uri = New Uri(s)
            Dim strFileName As String = uu.LocalPath
            '===== 

            Dim LogPath As String = strFileName & "_DNRDB.txt"
            Dim objReader As StreamWriter = New StreamWriter(LogPath, True)
            Dim ErrMsg As String = ""

            objReader.Write(vbCrLf & vbCrLf & DateTime.Now.ToShortDateString & " " & DateTime.Now.ToShortTimeString & " - " & txt & vbCrLf)

            If (Not Params Is Nothing) Then
                For Each p As SqlParameter In Params.ParamCollection
                    Try
                        ErrMsg += vbCrLf & vbTab & "Name: " & vbTab & p.ParameterName & vbTab & "Value: " & p.Value.ToString
                    Catch ex As Exception
                    End Try
                Next
                objReader.Write(ErrMsg)
            End If

            objReader.Close()



        Catch ex As Exception
            Throw New Exception("LogIt Error: " & vbCrLf & ex.Message)
        End Try

    End Sub


End Class
