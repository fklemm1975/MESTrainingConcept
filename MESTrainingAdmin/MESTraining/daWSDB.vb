Imports System.IO
Imports System.Configuration
Imports MESTrainingDB
Imports System.Net
Imports SimpleEncryption
Imports System.Data.SqlClient


Public Class daWSDB

    Private Conn As SqlConnection
    Private Cmd As SqlCommand
    Private da As SqlDataAdapter
    Private l As CertDBLib = New CertDBLib

    Public RowCount As Integer
    Public Identity As Integer
    Private Outputs As Collection = New Collection

    Private dws2 As daWebService.DataAccessWS


    'Private ekey As String = "kej35(jJne6Hlnx9ie@Okd"
    Dim eKey As String = "mc$47G0x\4-ZnpD6(Q1!j&Xd"

    Private CertConnStr As String '= Crypto.Decrypt(ConfigurationSettings.AppSettings("my_connection"), eKey)
    'Private ConnStr As String = Crypto.Decrypt(ConfigurationManager.AppSettings("my_connection"), eKey)

    Private skey As String = "bb*Kasd%@(89asdflkj1lsdfn"
    Private CertConnStrEncrypted As String '= SimpleEncryption.Crypto.Encrypt(CertConnStr, skey)

#Region "Param Collection"

    Private Class SQLParamCollection
        'You can't instantiate the SqlParameterCollection as a variable, you have to get it from the command object
        'which defeats the purpose of passing around a collection.  So we'll create this class and pass it.

        'GRP - 2019.11.04 replace the next 3 public declaarations to return to web service
        'Public ParamList As Generic.List(Of daWS.SqlParameter)
        Public ParamList As Generic.List(Of daWebService.SqlParameter)

        Public Sub New(Optional ByVal ParamName As String = "", Optional ByVal Value As Object = Nothing)
            ParamList = New Generic.List(Of daWebService.SqlParameter)
            If (ParamName.Length > 0) Then
                AddWithValue(ParamName, Value)
            End If
        End Sub

        Public Sub AddWithValue(ByVal ParamName As String, ByVal Value As Object, Optional ByVal SQLDBDataType As Object = Nothing, Optional ByVal ParamDirection As Object = Nothing)
            Dim p As daWebService.SqlParameter = New daWebService.SqlParameter
            p.ParameterName = ParamName
            If ParamDirection Is Nothing Then p.Value = Value
            If Not SQLDBDataType Is Nothing Then p.SqlDbType = SQLDBDataType
            If Not ParamDirection Is Nothing Then
                p.Direction = ParamDirection
            Else
                p.Direction = daWS.ParameterDirection.Input
            End If

            ParamList.Add(p)
        End Sub

        'And comment out these 3
        'Public ParamCollection As Collection

        'Public Sub New(Optional ByVal ParamName As String = "", Optional ByVal Value As Object = Nothing)
        '    ParamCollection = New Collection
        '    If (ParamName.Length > 0) Then
        '        AddWithValue(ParamName, Value)
        '    End If
        'End Sub

        'Public Sub AddWithValue(ByVal ParamName As String, ByVal Value As Object, Optional ByVal SQLDBDataType As Object = Nothing, Optional ByVal ParamDirection As Object = Nothing)
        '    Dim p As SqlParameter = New SqlParameter
        '    p.ParameterName = ParamName
        '    If ParamDirection Is Nothing Then p.Value = Value
        '    If Not SQLDBDataType Is Nothing Then p.SqlDbType = SQLDBDataType
        '    If Not ParamDirection Is Nothing Then
        '        p.Direction = ParamDirection
        '    Else
        '        p.Direction = ParameterDirection.Input
        '    End If

        '    ParamCollection.Add(p)
        'End Sub

    End Class

#End Region

    Private iTimeout As Integer = 30
    Public Property Timeout() As Integer
        Get
            Timeout = iTimeout
        End Get
        Set(ByVal value As Integer)
            iTimeout = value
        End Set
    End Property

#Region "Select Queries"
    'GRP - You needt to replace params to go back
    '        Return SelectCommand(CertConnStrEncrypted, "SelectAllActiveCertificationType", Params.ParamList)

    Public Function SelectAllLUTrainingSite() As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection()
        Return SelectCommand(CertConnStrEncrypted, "SelectAllLUTrainingSite", Params.ParamList)
    End Function

    Public Function SelectByIDTrainingSite(ByVal TrainingSiteID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@TrainingSiteID", TrainingSiteID)
        Return SelectCommand(CertConnStrEncrypted, "SelectByIDTrainingSite", Params.ParamList)
    End Function



    Public Function SelectAllActiveCertificationType(ByVal UserID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@UserID", UserID)
        Return SelectCommand(CertConnStrEncrypted, "SelectAllActiveCertificationType", Params.ParamList)
    End Function

    Public Function SelectActiveItemTypes() As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection()
        Return SelectCommand(CertConnStrEncrypted, "SelectActiveItemTypes", Params.ParamList)
    End Function

    Public Function SelectAllActiveItemsByParentIDCertificationType(ByVal ParentID As Integer, ByVal CertificationTypeID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@ParentID", ParentID)
        Params.AddWithValue("@CertificationTypeID", CertificationTypeID)
        Return SelectCommand(CertConnStrEncrypted, "SelectAllActiveItemsByParentIDCertificationType", Params.ParamList)
    End Function

    Public Function SelectAllQuestionItemNodes(ByVal CertificationTypeID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@CertificationTypeID", CertificationTypeID)
        Return SelectCommand(CertConnStrEncrypted, "SelectAllQuestionItemNodes", Params.ParamList)
    End Function

    Public Function SelectAllQuestionsByCertificationTypeID(ByVal CertificationTypeID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@CertificationTypeID", CertificationTypeID)
        Return SelectCommand(CertConnStrEncrypted, "SelectAllQuestionsByCertificationTypeID", Params.ParamList)
    End Function

    Public Function SelectMaxDisplaySequence(ByVal CertificationTypeID As Integer) As DataSet
        Dim params As SQLParamCollection = New SQLParamCollection("@CertificationTypeID", CertificationTypeID)
        Return SelectCommand(CertConnStrEncrypted, "SelectMaxDisplaySequence", params.ParamList)
    End Function

    Public Function SelectAllCertificationType(ByVal UserID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@UserID", UserID)
        Return SelectCommand(CertConnStrEncrypted, "SelectAllCertificationType", Params.ParamList)
    End Function

    Public Function SelectByIDItems(ByVal ItemID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@ItemID", ItemID)
        Return SelectCommand(CertConnStrEncrypted, "SelectByIDItems", Params.ParamList)
    End Function

    Public Function SelectByIDCertificationType(ByVal CertificationTypeID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@CertificationTypeID", CertificationTypeID)
        Return SelectCommand(CertConnStrEncrypted, "SelectByIDCertificationType", Params.ParamList)
    End Function

    Public Function SelectByIDPublicUserRegistration(ByVal PublicUserRegistrationID) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@PublicUserRegistrationID", PublicUserRegistrationID)
        Return SelectCommand(CertConnStrEncrypted, "SelectByIDPublicUserRegistration", Params.ParamList)
    End Function

    Public Function SelectByIDQuestions(ByVal QuestionsID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@QuestionsID", QuestionsID)
        Return SelectCommand(CertConnStrEncrypted, "SelectByIDQuestions", Params.ParamList)
    End Function

    Public Function SelectCertificationTypeAlphabetically() As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection()
        Return SelectCommand(CertConnStrEncrypted, "SelectCertificationTypeAlphabetically", Params.ParamList)
    End Function

    Public Function SelectCountOfItemsByCertificationTypeID(ByVal CertificationTypeID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@CertificationTypeID", CertificationTypeID)
        Return SelectCommand(CertConnStrEncrypted, "SelectCountOfItemsByCertificationTypeID", Params.ParamList)
    End Function

    Public Function VerifyLogin(ByVal UserName As String, ByVal Password As Byte(), Optional ByVal NetworkName As String = Nothing, Optional ByVal IPAddress As String = Nothing) As Integer
        Try
            Dim ds As DataSet
            Dim param As SQLParamCollection = New SQLParamCollection("@UserName", UserName)
            param.AddWithValue("@Password", Password)
            param.AddWithValue("@NetworkID", System.Environment.UserName)
            param.AddWithValue("@MachineName", System.Environment.MachineName)

            Dim HostIP As IPAddress
            HostIP = Dns.GetHostEntry(Dns.GetHostName).AddressList.GetValue(0)

            param.AddWithValue("@IPAddress", HostIP.ToString)
            ds = SelectCommand(CertConnStrEncrypted, "VerifyLogin", param.ParamList)  'GRP - 2019.11.04 replace =>  .ParamList
            Return Integer.Parse(ds.Tables(0).Rows(0)("UserID"))
        Catch ex As Exception
            LogIt("VerifyLogin: " & vbCrLf & ex.Message, False)
            Throw New Exception("VerifyLogin: " & vbCrLf & ex.Message)
        End Try
    End Function

    'Public Function VerifyLogin(ByVal UserName As String, ByVal Password As Byte(), Optional ByVal NetworkName As String = Nothing, Optional ByVal IPAddress As String = Nothing) As Integer
    '    Dim ds As DataSet
    '    Dim param As SQLParamCollection = New SQLParamCollection("@UserName", UserName)
    '    param.AddWithValue("@Password", Password)
    '    param.AddWithValue("@NetworkID", System.Environment.UserName)
    '    param.AddWithValue("@MachineName", System.Environment.MachineName)

    '    Dim HostIP As IPAddress
    '    HostIP = Dns.GetHostEntry(Dns.GetHostName).AddressList.GetValue(0)

    '    param.AddWithValue("@IPAddress", HostIP.ToString)
    '    ds = SelectCommand("", "VerifyLogin", param.ParamList)
    '    Return Integer.Parse(ds.Tables(0).Rows(0)("UserID"))
    'End Function

    Public Function SelectByIDUser(ByVal UserID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@UserID", UserID)
        Return SelectCommand(CertConnStrEncrypted, "SelectByIDUser", Params.ParamList)
    End Function

    Public Function SelectAppRightsByUserIDAppID(ByVal AppID As Integer, ByVal UserID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@AppID", AppID)
        Params.AddWithValue("@UserID", UserID)
        Return SelectCommand(CertConnStrEncrypted, "SelectAppRightsByUserIDAppID", Params.ParamList)
    End Function

    Public Function SelectAllReports() As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection()
        Return SelectCommand(CertConnStrEncrypted, "SelectAllReports", Params.ParamList)
    End Function

    Public Function SelectReportTotals() As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection()
        Return SelectCommand(CertConnStrEncrypted, "SelectReportTotals", Params.ParamList)
    End Function

    Public Function SelectAllExamsForReporting() As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection()
        Return SelectCommand(CertConnStrEncrypted, "SelectAllExamsForReporting", Params.ParamList)
    End Function

    Public Function SelectAllPassedExamsForReporting() As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection()
        Return SelectCommand(CertConnStrEncrypted, "SelectAllPassedExamsForReporting", Params.ParamList)
    End Function

    Public Function SelectAllFailedExamsForReporting() As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection()
        Return SelectCommand(CertConnStrEncrypted, "SelectAllFailedExamsForReporting", Params.ParamList)
    End Function

    Public Function SelectAllActiveExamsForReporting() As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection()
        Return SelectCommand(CertConnStrEncrypted, "SelectAllActiveExamsForReporting", Params.ParamList)
    End Function

    Public Function SelectAllTrainingSite() As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection()
        Return SelectCommand(CertConnStrEncrypted, "SelectAllTrainingSite", Params.ParamList)
    End Function

    Public Function SelectAllExamsByFirstName(ByVal FirstName As String) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@FirstName", FirstName)
        Return SelectCommand(CertConnStrEncrypted, "SelectAllExamsByFirstName", Params.ParamList)
    End Function

    Public Function SelectAllExamsByLastName(ByVal LastName As String) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@LastName", LastName)
        Return SelectCommand(CertConnStrEncrypted, "SelectAllExamsByLastName", Params.ParamList)
    End Function

    Public Function SelectAllExamsByDateRange(ByVal StartDate As DateTime, ByVal EndDate As DateTime) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@StartDate", StartDate)
        Params.AddWithValue("@EndDate", EndDate)
        Return SelectCommand(CertConnStrEncrypted, "SelectAllExamsByDateRange", Params.ParamList)
    End Function

    Public Function SelectAllExamsByCertificateNumber(ByVal CertificateNumber As String) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@CertificateNumber", CertificateNumber)
        Return SelectCommand(CertConnStrEncrypted, "SelectAllExamsByCertificateNumber", Params.ParamList)
    End Function

    Public Function ReportCertificate(ByVal UserSummaryID As Integer) As DataSet
        Dim params As SQLParamCollection = New SQLParamCollection("@UserSummaryID", UserSummaryID)
        Return SelectCommand(CertConnStrEncrypted, "ReportCertificate", params.ParamList)
    End Function

    Public Function ReportExpiringCertificates(ByVal CertificationTypeID As Integer, ByVal StartDate As DateTime, ByVal EndDate As DateTime, ByVal SortOrder As Integer) As DataSet
        Dim params As SQLParamCollection = New SQLParamCollection("@CertificationTypeID", CertificationTypeID)
        params.AddWithValue("@StartDate", Date.Parse(StartDate.ToShortDateString))
        params.AddWithValue("@EndDate", Date.Parse(EndDate.ToShortDateString))
        params.AddWithValue("@SortOrder", SortOrder)
        Return SelectCommand(CertConnStrEncrypted, "ReportExpiringCertificates", params.ParamList)
    End Function

    Public Function SelectAllFollowUpQuestionsByCertificationTypeID(ByVal CertificationTypeID As Integer) As DataSet
        Dim params As SQLParamCollection = New SQLParamCollection("@CertificationTypeID", CertificationTypeID)
        Return SelectCommand(CertConnStrEncrypted, "SelectAllFollowUpQuestionsByCertificationTypeID", params.ParamList)
    End Function

    Public Function SelectAllFollowUpQuestionItemNodes(ByVal CertificationTypeID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@CertificationTypeID", CertificationTypeID)
        Return SelectCommand(CertConnStrEncrypted, "SelectAllFollowUpQuestionItemNodes", Params.ParamList)
    End Function

    ';GRP - 2019.11.04
    Private Function SelectCommand(ByVal ConnStrEncrypted As String, ByVal QueryName As String, ByVal Params As Generic.List(Of daWebService.SqlParameter)) As DataSet
        Try
            'dws = New daWS.DataAccessWSSoapClient
            Dim dws2 = New daWebService.DataAccessWS

            dws2.Url = My.Settings.WebServiceURL

            Return dws2.SelectCommandWS(ConnStrEncrypted, QueryName, Params.ToArray)

        Catch ex As Exception
            LogIt("SelectCommand: " & QueryName & vbCrLf & ex.Message, False, "WS Error", Params)
            Throw New Exception("Select Command: " & QueryName & vbCrLf & ex.Message)
        End Try
    End Function

    Public Function GetFiles(ByVal DirectoryPath As String) As String
        Try
            dws2 = New daWebService.DataAccessWS
            Return dws2.GetFileList(CertConnStrEncrypted, DirectoryPath)
        Catch ex As Exception
            LogIt("GetFiles: " & ex.Message, False)
            Throw New Exception("GetFiles: " & ex.Message)
        End Try

    End Function



    'Private Function SelectCommand(ByVal ConnStrEncrypted As String, ByVal QueryName As String, Optional ByVal Params As SQLParamCollection = Nothing) As DataSet

    '    Dim ds As DataSet = New DataSet()
    '    Dim ErrMsg As String = ""

    '    Try
    '        Cmd = New SqlCommand(QueryName, Conn)
    '        Cmd.CommandType = CommandType.StoredProcedure

    '        da = New SqlDataAdapter(Cmd)

    '        If (Not Params Is Nothing) Then
    '            For Each p As SqlParameter In Params.ParamCollection
    '                da.SelectCommand.Parameters.AddWithValue(p.ParameterName, p.Value)
    '            Next
    '        End If

    '        RowCount = da.Fill(ds)

    '        Return ds

    '    Catch ex As Exception
    '        Try

    '            Dim sInnerException As String = ""

    '            If Not ex.InnerException Is Nothing Then
    '                sInnerException = sInnerException
    '            End If

    '            'GRP - 2019.11.04   l.LogIt("SelectCommand: " & QueryName & vbCrLf & ex.Message & sInnerException, Params)
    '            Throw New Exception("SelectCommand: " & QueryName & vbCrLf & ex.Message & sInnerException)
    '        Catch ex2 As Exception
    '            Throw New Exception("SelectCommand and LogIt: " & QueryName & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex2.Message)
    '        End Try
    '    End Try

    '    Return ds

    'End Function


#End Region

#Region "Save Queries"

    Public Function SaveCertificationType(ByRef dto As daWSDTO.CertificationTypeDto) As Boolean

        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@CertificationTypeID", dto.CertificationTypeID)
        Params.AddWithValue("@CertificationName", dto.CertificationName)
        Params.AddWithValue("@Active", dto.Active)
        Params.AddWithValue("@DisplaySequence", dto.DisplaySequence)
        Params.AddWithValue("@Publish", dto.Publish)
        Params.AddWithValue("@PassingScore", dto.PassingScore)
        Params.AddWithValue("@LengthOfCertification", dto.LengthOfCertification)
        Params.AddWithValue("@PassingVerbiage", dto.PassingVerbiage)
        Params.AddWithValue("@FailingVerbiage", dto.FailingVerbiage)
        Params.AddWithValue("@CreatedByID", dto.CreatedByID)
        Params.AddWithValue("@LastModifiedID", dto.LastModifiedID)
        Params.AddWithValue("@LastModifiedNetworkID", System.Environment.UserName)
        Params.AddWithValue("@UserID", dto.UserID)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, daWS.ParameterDirection.Output)
        'Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, 2)

        Return SaveCommand(CertConnStrEncrypted, "SaveCertificationType", Params.ParamList)

    End Function

    Public Function SaveItems(ByRef dto As daWSDTO.ItemsDto) As Boolean

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
        Params.AddWithValue("@ImageCaption", dto.ImageCaption)
        Params.AddWithValue("@VideoCaption", dto.VideoCaption)
        Params.AddWithValue("@NumberOfQuestions", dto.NumberOfQuestions)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, daWS.ParameterDirection.Output)
        'Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, 2)

        Return SaveCommand(CertConnStrEncrypted, "SaveItems", Params.ParamList)

    End Function

    Public Function SavePublicUserRegistration(ByRef dto As daWSDTO.PublicUserRegistrationDto) As Boolean

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
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, daWS.ParameterDirection.Output)
        'Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, 2)

        Return SaveCommand(CertConnStrEncrypted, "SavePublicUserRegistration", Params.ParamList)

    End Function

    Public Function SaveQuestions(ByRef dto As daWSDTO.QuestionsDto) As Boolean

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
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, daWS.ParameterDirection.Output)
        'Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, 2)

        Return SaveCommand(CertConnStrEncrypted, "SaveQuestions", Params.ParamList)

    End Function

    Public Function SaveUser(ByRef dto As daWSDTO.UserDto) As Boolean

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
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, daWS.ParameterDirection.Output)
        'Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, 2)

        Return SaveCommand(CertConnStrEncrypted, "SaveUser", Params.ParamList)

    End Function

    'GRP 2019.11.04 - replace the following
    Private Function SaveCommand(ByVal ConnStrEncrypted As String, ByRef QueryName As String, ByRef Params As Generic.List(Of daWebService.SqlParameter)) As Boolean
        Try
            dws2 = New daWebService.DataAccessWS
            Identity = dws2.SaveCommandWS(ConnStrEncrypted, QueryName, Params.ToArray)
            Return True
        Catch ex As Exception
            MsgBox("SaveCommand: " & QueryName & vbCrLf & ex.Message, MsgBoxStyle.Critical, "WS Error")
            LogIt("SaveCommand: " & QueryName & vbCrLf & ex.Message, True, "WS Error", Params)
            Return False
        End Try
    End Function

    'GRP2020.08.12- going back to the web service
    'Private Function SaveCommand(ByVal ConnStrEncrypted As String, ByRef QueryName As String, ByRef Params As SQLParamCollection) As Boolean
    '    Dim RetVal As Boolean = False
    '    Dim s As SqlParameter = New SqlParameter

    '    Try
    '        Identity = 0

    '        Cmd = New SqlCommand(QueryName, Conn)
    '        Dim pOut As SqlParameter = Cmd.CreateParameter()

    '        For Each p As SqlParameter In Params.ParamCollection
    '            If p.Direction = ParameterDirection.Output Then
    '                pOut.ParameterName = p.ParameterName
    '                pOut.Direction = ParameterDirection.Output
    '                pOut.SqlDbType = p.SqlDbType
    '                Cmd.Parameters.Add(pOut)

    '            Else
    '                If p.SqlDbType = SqlDbType.NVarChar Then
    '                    Cmd.Parameters.AddWithValue(p.ParameterName, p.Value)
    '                Else
    '                    Dim sp As SqlParameter = New SqlParameter()
    '                    sp.ParameterName = p.ParameterName
    '                    sp.Value = p.Value
    '                    sp.SqlDbType = p.SqlDbType
    '                    Cmd.Parameters.Add(sp)
    '                End If
    '            End If
    '        Next

    '        Cmd.CommandType = CommandType.StoredProcedure
    '        Cmd.CommandTimeout = Timeout

    '        Cmd.ExecuteNonQuery()
    '        RetVal = True

    '        If Not Integer.TryParse(Cmd.Parameters("@IdentityVal").Value.ToString(), Identity) Then
    '            Throw New Exception("SaveCommand unable to parse @IdentityVal to an integer.  @IdentityVal = '" & Cmd.Parameters("@IdentityVal").Value.ToString() & "'")
    '        End If


    '    Catch ex As Exception
    '        Dim bLogitFailed As Boolean = True
    '        Dim sMsg As String = "SaveDataCommand: " & QueryName & vbCrLf & ex.Message
    '        Try
    '            GRP - 2019.11.04 - l.LogIt(sMsg, Params)
    '            bLogitFailed = False
    '        Catch ex2 As Exception
    '            sMsg = "SaveDataCommand and LogIt: " & QueryName & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex2.Message
    '        Finally
    '            Throw New Exception(sMsg)
    '        End Try
    '    End Try

    '    Return RetVal

    'End Function


#End Region

#Region "Delete Queries"

    Private Function DeleteCommand(ByVal ConnStrEncrypted As String, ByRef QueryName As String, ByRef Params As Generic.List(Of daWebService.SqlParameter)) As Boolean
        Try
            dws2 = New daWebService.DataAccessWS
            Return dws2.DeleteCommandWS(ConnStrEncrypted, QueryName, Params.ToArray)
        Catch ex As Exception
            MsgBox("DeleteCommand: " & QueryName & vbCrLf & ex.Message, MsgBoxStyle.Critical, "WS Error")
            LogIt("DeleteCommand: " & QueryName & vbCrLf & ex.Message, True, "WS Error", Params)
        End Try
    End Function

#End Region

    Private Sub LogIt(ByVal txt As String, Optional ByVal ShowMsgBox As Boolean = False, Optional ByVal MsgTitle As String = "", Optional ByVal Params As Generic.List(Of daWebService.SqlParameter) = Nothing)
        Try

            '===== from - http://www.dotnet247.com/247reference/msgs/58/291207.aspx
            Dim s As String = System.Reflection.Assembly.GetExecutingAssembly().CodeBase
            s += ".config"

            Dim uu As Uri = New Uri(s)
            Dim strFileName As String = uu.LocalPath
            '===== 

            Dim LogPath As String = strFileName & "_cbDB.txt"
            Dim objReader As StreamWriter = New StreamWriter(LogPath, True)
            Dim ErrMsg As String = ""

            objReader.Write(vbCrLf & vbCrLf & DateTime.Now.ToShortDateString & " " & DateTime.Now.ToShortTimeString & " - " & txt & vbCrLf)

            If (Not Params Is Nothing) Then
                For Each p As daWebService.SqlParameter In Params
                    Try
                        ErrMsg += vbCrLf & vbTab & "Name: " & vbTab & p.ParameterName & vbTab & "Value: " & p.Value.ToString
                    Catch ex As Exception
                    End Try
                Next
                objReader.Write(ErrMsg)
            End If

            objReader.Close()

        Catch ex As Exception
            'WriteEvent()
        End Try

        If (ShowMsgBox) Then
        End If
        MsgBox(txt, MsgBoxStyle.Critical, MsgTitle)

    End Sub

    Public Sub New(Optional ByVal EncryptedString As String = "")
        Try
            dws2 = New daWebService.DataAccessWS
            Me.RowCount = 0
            Me.Identity = 0

            'Private ekey As String = "kej35(jJne6Hlnx9ie@Okd"
            'Dim eKey As String = "mc$47G0x\4-ZnpD6(Q1!j&Xd"

            If EncryptedString = "" Then
                CertConnStr = Crypto.Decrypt(My.Settings.EncryptedString, eKey)
            Else
                CertConnStr = Crypto.Decrypt(ConfigurationManager.AppSettings("my_connection"), eKey)
            End If

            'Private ConnStr As String = Crypto.Decrypt(ConfigurationManager.AppSettings("my_connection"), eKey)

            'Private skey As String = "bb*Kasd%@(89asdflkj1lsdfn"
            CertConnStrEncrypted = SimpleEncryption.Crypto.Encrypt(CertConnStr, skey)

        Catch ex As Exception
            LogIt("Create Webservice Object: " & vbCrLf & ex.Message, False, "Webservice Error" & vbCrLf)
            Throw (New Exception("Unable to connect to the webservice.  Please check your configuration settings or contact your adminstrator." & vbCrLf))
        End Try
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        Try
            'dws2.Close()
            dws2 = Nothing
        Catch ex As Exception

        End Try
    End Sub

    'Public Sub New()
    '    Try
    '        Me.Conn = New SqlConnection(ConnStr)
    '        Me.Conn.Open()
    '        Me.RowCount = 0
    '        Me.Identity = 0
    '    Catch ex As Exception
    '        l.LogIt("Create DB Object: " & vbCrLf & ex.Message)
    '        Throw (New Exception("Unable to connect to the database.  Please check your configuration settings or contact your adminstrator." & vbCrLf))
    '    End Try
    'End Sub

    'Protected Overrides Sub Finalize()
    '    MyBase.Finalize()
    '    Try
    '        Me.Conn.Close()
    '        Me.Conn.Dispose()
    '        Me.Conn = Nothing

    '        Me.Cmd.Dispose()
    '        Me.Cmd = Nothing

    '        Me.da.Dispose()
    '        Me.da = Nothing

    '    Catch ex As Exception

    '    End Try
    'End Sub

End Class
