Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Configuration
Imports SimpleEncryption

Public Class CertDB

#Region "Variables and Properties"
    Public RowCount As Integer
    Public Identity As Integer

    Private Conn As SqlConnection
    Private Cmd As SqlCommand
    Private da As SqlDataAdapter
    Private l As CertDBLib = New CertDBLib

    Private eKey As String = "kej35(jJne6Hlnx9ie@Okd"
    Private ConnStr As String = Crypto.Decrypt(ConfigurationManager.AppSettings("my_connection"), eKey)

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

    Public Class SQLParamCollection
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

#Region "Select Methods"

    Public Function SelectByIDItems(ByVal ItemID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@ItemID", ItemID)
        Return SelectCommand("SelectByIDItems", Params)
    End Function

    Public Function SelectAllItems() As DataSet
        Return SelectCommand("SelectAllItems")
    End Function

    Public Function SelectAllActiveItems() As DataSet
        Return SelectCommand("SelectAllActiveItems")
    End Function

    Public Function SelectAllItemsByParentID(ByVal ParentID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@ParentID", ParentID)
        Return SelectCommand("SelectAllItemsByParentID", Params)
    End Function

    Public Function SelectAllActiveItemsByParentID(ByVal ParentID As Integer, ByVal CertificationTypeID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@ParentID", ParentID)
        Return SelectCommand("SelectAllActiveItemsByParentID", Params)
    End Function

    Public Function SelectAllActiveItemsByParentIDCertificationType(ByVal ParentID As Integer, ByVal CertificationTypeID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@ParentID", ParentID)
        Params.AddWithValue("@CertificationTypeID", CertificationTypeID)
        Return SelectCommand("SelectAllActiveItemsByParentIDCertificationType", Params)
    End Function

    Public Function SelectAllActiveCertificationType() As DataSet
        Return SelectCommand("SelectAllActiveCertificationType")
    End Function

    Public Function SelectActiveItemTypes() As DataSet
        Return SelectCommand("SelectActiveItemTypes")
    End Function

    Public Function SelectByIDQuestions(ByVal QuestionsID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@QuestionsID", QuestionsID)
        Return SelectCommand("SelectByIDQuestions", Params)
    End Function

    Public Function SelectAllQuestionsByCertificationTypeID(ByVal CertificationTypeID) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@CertificationTypeID", CertificationTypeID)
        Return SelectCommand("SelectAllQuestionsByCertificationTypeID", Params)
    End Function

    Public Function SelectAllQuestionItemNodes(ByVal CertificationTypeID) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@CertificationTypeID", CertificationTypeID)
        Return SelectCommand("SelectAllQuestionItemNodes", Params)
    End Function

    Public Function VerifyPublicUserRegistration(ByVal EmailAddress As String, ByVal Password As Byte()) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@EmailAddress", EmailAddress)
        Params.AddWithValue("@Password", Password)
        Return SelectCommand("VerifyPublicUserRegistration", Params)
    End Function

    Public Function VerifyPublicUserEmail(ByVal EmailAddress) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@EmailAddress", EmailAddress)
        Return SelectCommand("VerifyPublicUserEmail", Params)
    End Function

    Public Function SelectPublicUserRegistration() As DataSet
        Return SelectCommand("SelectPublicUserRegistration")
    End Function

    Public Function SelectByIDPublicUserRegistration(ByVal PublicUserRegistrationID) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@PublicUserRegistrationID", PublicUserRegistrationID)
        Return SelectCommand("SelectByIDPublicUserRegistration", Params)
    End Function

    Public Function SelectByDateRangePublicUserRegistration(ByVal BeginDate As DateTime, ByVal EndDate As DateTime) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@BeginDate", BeginDate)
        Params.AddWithValue("@EndDate", EndDate)
        Return SelectCommand("SelectByDateRangePublicUserRegistration", Params)
    End Function

    Public Function SelectByIDCertificationType(ByVal CertificationTypeID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@CertificationTypeID", CertificationTypeID)
        Return SelectCommand("SelectByIDCertificationType", Params)
    End Function

    Public Function SelectAllCertificationType() As DataSet
        Return SelectCommand("SelectAllCertificationType")
    End Function

    Public Function SelectMaxDisplaySequence(ByVal CertificationTypeID As Integer) As DataSet
        Dim params As SQLParamCollection = New SQLParamCollection("@CertificationTypeID", CertificationTypeID)
        Return SelectCommand("SelectMaxDisplaySequence", params)
    End Function

    Public Function SelectCertificationTypeAlphabetically() As DataSet
        Return SelectCommand("SelectCertificationTypeAlphabetically")
    End Function

   Public Function VerifyLogin(ByVal UserName As String, ByVal Password As Byte(), ByVal ApplicationID As Integer, Optional ByVal NetworkName As String = Nothing, Optional ByVal IPAddress As String = Nothing) As Integer
        Dim ds As DataSet
        Dim param As SQLParamCollection = New SQLParamCollection("@UserName", UserName)
        param.AddWithValue("@Password", Password)
        param.AddWithValue("@ApplicationID", ApplicationID)
        param.AddWithValue("@NetworkID", System.Environment.UserName)
        param.AddWithValue("@MachineName", System.Environment.MachineName)

        Dim HostIP As IPAddress
        HostIP = Dns.GetHostEntry(Dns.GetHostName).AddressList.GetValue(0)

        param.AddWithValue("@IPAddress", HostIP.ToString)
        ds = SelectCommand("VerifyLogin", param)
        Return Integer.Parse(ds.Tables(0).Rows(0)("UserID"))
    End Function

    Public Function SelectByIDUser(ByVal UserID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@UserID", UserID)
        Return SelectCommand("SelectByIDUser", Params)
    End Function

    Private Function SelectCommand(ByVal QueryName As String, Optional ByVal Params As SQLParamCollection = Nothing) As DataSet
        Dim ds As DataSet = New DataSet()
            Dim ErrMsg As String = ""

            Try
                Cmd = New SqlCommand(QueryName, Conn)
                Cmd.CommandType = CommandType.StoredProcedure

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
                    l.LogIt("SelectCommand: " & QueryName & vbCrLf & ex.Message, Params)
                    Throw New Exception("SelectCommand: " & QueryName & vbCrLf & ex.Message)
                Catch ex2 As Exception
                    Throw New Exception("SelectCommand and LogIt: " & QueryName & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex2.Message)
                End Try
            End Try

            Return ds
    End Function

#End Region

#Region "Save Methods"

    Public Function SaveItems(ByRef dto As CertDto.ItemsDto) As Boolean

        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@ItemID", dto.ItemID)
        Params.AddWithValue("@CertificationTypeID", dto.CertificationTypeID)
        Params.AddWithValue("@ItemTypeID", dto.ItemTypeID)
        Params.AddWithValue("@Name", dto.Name)
        Params.AddWithValue("@Heading", dto.Heading)
        Params.AddWithValue("@ItemHeading", dto.ItemHeading)
        Params.AddWithValue("@Verbiage", dto.Verbiage)
        Params.AddWithValue("@VideoFile", dto.VideoFile)
        Params.AddWithValue("@ImageFile", dto.ImageFile)
        Params.AddWithValue("@ParentID", dto.ParentID)
        Params.AddWithValue("@DisplaySequence", dto.DisplaySequence)
        Params.AddWithValue("@Active", dto.Active)
        Params.AddWithValue("@CreatedByID", dto.CreatedByID)
        Params.AddWithValue("@LastModifiedID", dto.LastModifiedID)
        Params.AddWithValue("@LastModifiedNetworkID", System.Environment.UserName)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

        Return SaveCommand("SaveItems", Params)

    End Function

    Public Function SaveQuestions(ByRef dto As CertDto.QuestionsDto) As Boolean

        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@QuestionsID", dto.QuestionsID)
        Params.AddWithValue("@CertificationTypeID", dto.CertificationTypeID)
        Params.AddWithValue("@ItemID", dto.ItemID)
        Params.AddWithValue("@Question", dto.Question)
        Params.AddWithValue("@NumberOfAnswers", dto.NumberOfAnswers)
        Params.AddWithValue("@Answer1", dto.Answer1)
        Params.AddWithValue("@Answer2", dto.Answer2)
        Params.AddWithValue("@Answer3", dto.Answer3)
        Params.AddWithValue("@Answer4", dto.Answer4)
        Params.AddWithValue("@Answer5", dto.Answer5)
        Params.AddWithValue("@CorrectAnswer", dto.CorrectAnswer)
        Params.AddWithValue("@QuestionWeight", dto.QuestionWeight)
        Params.AddWithValue("@Active", dto.Active)
        Params.AddWithValue("@DisplaySequence", dto.DisplaySequence)
        Params.AddWithValue("@CreatedByID", dto.CreatedByID)
        Params.AddWithValue("@LastModifiedID", dto.LastModifiedID)
        Params.AddWithValue("@LastModifiedNetworkID", System.Environment.UserName)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

        Return SaveCommand("SaveQuestions", Params)

    End Function

    Public Function SavePublicUserRegistration(ByRef dto As CertDto.PublicUserRegistrationDto) As Boolean

        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@PublicUserRegistrationID", dto.PublicUserRegistrationID)
        Params.AddWithValue("@FirstName", dto.FirstName)
        Params.AddWithValue("@MiddleName", dto.MiddleName)
        Params.AddWithValue("@LastName", dto.LastName)
        Params.AddWithValue("@EmailAddress", dto.EmailAddress)
        Params.AddWithValue("@Password", dto.Password)
        Params.AddWithValue("@TempPassword", dto.TempPassword)
        Params.AddWithValue("@WorkPhone", dto.WorkPhone)
        Params.AddWithValue("@MobilePhone", dto.MobilePhone)
        Params.AddWithValue("@HomePhone", dto.HomePhone)
        Params.AddWithValue("@Fax", dto.Fax)
        Params.AddWithValue("@CompanyName", dto.CompanyName)
        Params.AddWithValue("@JobResponsibility", dto.JobResponsibility)
        Params.AddWithValue("@DriverLicence", dto.DriverLicence)
        Params.AddWithValue("@Street", dto.Street)
        Params.AddWithValue("@City", dto.City)
        Params.AddWithValue("@State", dto.State)
        Params.AddWithValue("@Zip", dto.Zip)
        Params.AddWithValue("@County", dto.County)
        Params.AddWithValue("@RegisteredBy", dto.RegisteredBy)
        Params.AddWithValue("@RegisteredAt", dto.RegisteredAt)
        Params.AddWithValue("@RegisteredFrom", dto.RegisteredFrom)
        Params.AddWithValue("@UpdatedBy", dto.UpdatedBy)
        Params.AddWithValue("@UpdatedFrom", dto.UpdatedFrom)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

        Return SaveCommand("SavePublicUserRegistration", Params)

    End Function

    Public Function SaveCertificationType(ByRef dto As CertDto.CertificationTypeDto) As Boolean

        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@CertificationTypeID", dto.CertificationTypeID)
        Params.AddWithValue("@CertificationName", dto.CertificationName)
        Params.AddWithValue("@Active", dto.Active)
        Params.AddWithValue("@DisplaySequence", dto.DisplaySequence)
        Params.AddWithValue("@Publish", dto.Publish)
        Params.AddWithValue("@CreatedByID", dto.CreatedByID)
        Params.AddWithValue("@LastModifiedID", dto.LastModifiedID)
        Params.AddWithValue("@LastModifiedNetworkID", System.Environment.UserName)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

        Return SaveCommand("SaveCertificationType", Params)

    End Function

    'Public Function LogEventByUsername(ByVal EventTypeID As Integer, ByVal Username As String, Optional ByVal EventSubTypeID As Integer = Nothing, Optional ByVal EventDetails As String = Nothing)

    '    Dim Params As SQLParamCollection = New SQLParamCollection("@EventTypeID", EventTypeID)

    '    Params.AddWithValue("@EventSubTypeID", IIf(EventSubTypeID = Nothing, DBNull.Value, EventSubTypeID))
    '    Params.AddWithValue("@ApplicationID", Integer.Parse(ConfigurationManager.AppSettings("ApplicationID")))
    '    Params.AddWithValue("@Username", Username) 'get converted to an EmployeeID in the Stored Procedure
    '    Params.AddWithValue("@MachineName", System.Environment.MachineName)
    '    Params.AddWithValue("@NetworkID", System.Environment.UserName)
    '    Params.AddWithValue("@IPAddress", Dns.GetHostEntry(Dns.GetHostName).AddressList.GetValue(0).ToString)
    '    Params.AddWithValue("@EventDetails", IIf(EventDetails Is Nothing, DBNull.Value, EventDetails))
    '    Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

    '    Return SaveCommand("LogEventByUsername", Params)

    'End Function

    Public Function SaveUser(ByRef dto As CertDto.UserDto) As Boolean

        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@UserID", dto.UserID)
        Params.AddWithValue("@UserName", dto.UserName)
        Params.AddWithValue("@Password", dto.Password)
        Params.AddWithValue("@FName", dto.FName)
        Params.AddWithValue("@LName", dto.LName)
        Params.AddWithValue("@EmployeeNumber", dto.EmployeeNumber)
        Params.AddWithValue("@Active", dto.Active)
        Params.AddWithValue("@EmailAddress", dto.EmailAddress)
        Params.AddWithValue("@ManagerID", dto.ManagerID)
        Params.AddWithValue("@CreatedByID", dto.CreatedByID)
        Params.AddWithValue("@LastModifiedByID", dto.LastModifiedByID)
        Params.AddWithValue("@LastModifiedNetworkID", System.Environment.UserName)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

        Return SaveCommand("SaveUser", Params)

    End Function

    Private Function SaveCommand(ByRef QueryName As String, ByRef Params As SQLParamCollection) As Boolean
        Dim RetVal As Boolean = False
        Dim s As SqlParameter = New SqlParameter

        Try
            Identity = 0

            Cmd = New SqlCommand(QueryName, Conn)
            Dim pOut As SqlParameter = Cmd.CreateParameter()

            For Each p As SqlParameter In Params.ParamCollection
                If p.Direction = ParameterDirection.Output Then
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

            Identity = Integer.Parse(Cmd.Parameters("@IdentityVal").Value.ToString())
        Catch ex As Exception
            Dim bLogitFailed As Boolean = True
            Dim sMsg As String = "SaveDataCommand: " & QueryName & vbCrLf & ex.Message
            Try
                l.LogIt(sMsg, Params)
                bLogitFailed = False
            Catch ex2 As Exception
                sMsg = "SaveDataCommand and LogIt: " & QueryName & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex2.Message
            Finally
                Throw New Exception(sMsg)
            End Try
        End Try

        Return RetVal
    End Function

#End Region

#Region "Update Methods"

    Public Function ResetPublicUserPassword(ByVal EmailAddress As String, ByVal Password As Byte()) As Boolean
        Dim Params As SQLParamCollection = New SQLParamCollection("@EmailAddress", EmailAddress)
        Params.AddWithValue("@Password", Password)
        Return UpdateCommand("ResetPublicUserPassword", Params)
    End Function

    Public Function ResetAdminUserPassword(ByVal UserName As String, ByVal Password As Byte()) As Boolean
        Dim Params As SQLParamCollection = New SQLParamCollection("@UserName", UserName)
        Params.AddWithValue("@Password", Password)
        Return UpdateCommand("ResetAdminUserPassword", Params)
    End Function

    Private Function UpdateCommand(ByRef QueryName As String, ByRef Params As SQLParamCollection) As Boolean
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
                    pOut.DbType = p.SqlDbType
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

            Cmd.ExecuteNonQuery()
            RetVal = True

        Catch ex As Exception
            Dim bLogitFailed As Boolean = True
            Try
                l.LogIt("UpdateCommand: " & QueryName & vbCrLf & ex.Message, Params)
                bLogitFailed = False
            Catch ex2 As Exception
                Throw New Exception("UpdateCommand and LogIt: " & QueryName & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex2.Message)
            End Try
            If Not bLogitFailed Then Throw New Exception("UpdateCommand: " & QueryName & vbCrLf & ex.Message)
        End Try

        Return RetVal

    End Function

#End Region

#Region "Insert Methods"

    Private Function InsertCommand(ByRef QueryName As String, ByRef Params As SQLParamCollection) As Boolean
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
                    pOut.DbType = p.SqlDbType
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

            Cmd.ExecuteNonQuery()
            RetVal = True

            Identity = Integer.Parse(Cmd.Parameters("@IdentityVal").Value.ToString())

        Catch ex As Exception
            Dim bLogitFailed As Boolean = True
            Try
                l.LogIt("InsertCommand: " & QueryName & vbCrLf & ex.Message, Params)
                bLogitFailed = False
            Catch ex2 As Exception
                Throw New Exception("InsertCommand and LogIt: " & QueryName & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex2.Message)
            End Try
            If Not bLogitFailed Then Throw New Exception("InsertCommand: " & QueryName & vbCrLf & ex.Message)
        End Try

        Return RetVal

    End Function

#End Region

#Region "Delete Methods"

    Private Function DeleteCommand(ByRef QueryName As String, ByRef Params As SQLParamCollection, Optional ByVal CatchExceptions As Boolean = True) As Boolean

        Dim RetVal As Boolean = False
        Dim s As SqlParameter = New SqlParameter

        Try
            Identity = 0

            Cmd = New SqlCommand(QueryName, Conn)

            For Each p As SqlParameter In Params.ParamCollection
                Cmd.Parameters.AddWithValue(p.ParameterName, p.Value)
            Next

            Cmd.CommandType = CommandType.StoredProcedure
            Cmd.CommandTimeout = Timeout

            Cmd.ExecuteNonQuery()
            RetVal = True

        Catch ex As Exception

            Dim bLogitFailed As Boolean = True
            Try
                l.LogIt("DeleteCommand: " & QueryName & vbCrLf & ex.Message, Params)
                bLogitFailed = False
            Catch ex2 As Exception
                Throw New Exception("DeleteCommand and LogIt: " & QueryName & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex2.Message)
            End Try
            If Not bLogitFailed Then Throw New Exception("DeleteCommand: " & QueryName & vbCrLf & ex.Message)
        End Try

        Return RetVal

    End Function

#End Region

    Public Sub New()
        Try
            Me.Conn = New SqlConnection(ConnStr)
            Me.Conn.Open()
            Me.RowCount = 0
            Me.Identity = 0
        Catch ex As Exception
            l.LogIt("Create DB Object: " & vbCrLf & ex.Message)
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

End Class