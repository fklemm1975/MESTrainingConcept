Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Configuration
Imports System.Threading
Imports SimpleEncryption

Public Class CertDB

#Region "Variables and Properties"

    Public RowCount As Integer
    Public Identity As Integer
    Dim eKey As String = "mc$47G0x\4-ZnpD6(Q1!j&Xd"

    Private Conn As SqlConnection
    Private Cmd As SqlCommand
    Private da As SqlDataAdapter
    Private l As CertDBLib = New CertDBLib

    'Private ConnStr As String = "Server=CNU92806XX;Database=SSDSCertification;User=rpcDBuser;Pwd=t^ssdsuser933;"
    'Private ConnStr As String = "Server=giswebp2\sqlexpress;Database=SSDSCertification;User=rpcDBuser;Pwd=t^ssdsuser933;"
    'Private ConnStr As String = ConfigurationManager.AppSettings("my_connection") & "Connection Timeout=10"
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


    Public Function SelectByIDUser(ByVal UserID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@UserID", UserID)
        Return SelectCommand("SelectByIDUser", Params)
    End Function


    Public Function SelectCertificationsFirstModuleByUserSummaryID(ByVal UserSummaryID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@UserSummaryID", UserSummaryID)
        Return SelectCommand("SelectCertificationsFirstModuleByUserSummaryID", Params)
    End Function

    Public Function SelectAppConfigByConfigKey(ByVal ConfigKey As String) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@ConfigKey", ConfigKey)
        Return SelectCommand("SelectAppConfigByConfigKey", Params)
    End Function

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

    Public Function SelectAllActiveCertificationType(ByVal UserID As Integer) As DataSet
        Dim params As SQLParamCollection = New SQLParamCollection("@UserID", UserID)
        Return SelectCommand("SelectAllActiveCertificationType", params)
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

    Public Function SelectMaxDisplaySequence(ByVal CertificationTypeID) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@CertificationTypeID", CertificationTypeID)
        Return SelectCommand("SelectMaxDisplaySequence", Params)
    End Function

    Public Function SelectByIDCertificationType(ByVal CertificationTypeID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@CertificationTypeID", CertificationTypeID)
        Return SelectCommand("SelectByIDCertificationType", Params)
    End Function

    Public Function SelectAllCertificationType() As DataSet
        Return SelectCommand("SelectAllCertificationType")
    End Function

    Public Function VerifyPublicUserRegistration(ByVal EmailAddress As String, ByVal Password As Byte()) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@EmailAddress", EmailAddress)
        Params.AddWithValue("@Password", Password)
        Return SelectCommand("VerifyPublicUserRegistration", Params)
    End Function

    Public Function VerifyAdminUserPermission(ByVal UserName As String, ByVal Password As Byte()) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@UserName", UserName)
        Params.AddWithValue("@Password", Password)
        Return SelectCommand("VerifyAdminUserPermission", Params)
    End Function

    Public Function VerifyPublicUserEmail(ByVal EmailAddress) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@EmailAddress", EmailAddress)
        Return SelectCommand("VerifyPublicUserEmail", Params)
    End Function

    Public Function SelecttblAdminUserPermission() As DataSet
        Return SelectCommand("SelecttblAdminUserPermission")
    End Function

    Public Function SelectByIDAdminUserPermission(ByVal AdminUserPermissionID) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@AdminUserPermissionID", AdminUserPermissionID)
        Return SelectCommand("SelectByIDAdminUserPermission", Params)
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

    Public Function SelectAllCertificationsByUserID(ByVal UserID As Integer) As DataSet
        Dim params As SQLParamCollection = New SQLParamCollection("@UserID", UserID)
        Return SelectCommand("SelectAllCertificationsByUserID", params)
    End Function

    Public Function SelectByIDUserItems(ByVal UserItemsID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@UserItemsID", UserItemsID)
        Return SelectCommand("SelectByIDUserItems", Params)
    End Function

    Public Function SelectAllUserItemsByParentIDCertificationType(ByVal ParentID As Integer, ByVal CertificationTypeID As Integer, ByVal UserID As Integer, ByVal UserSummaryID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@ParentID", ParentID)
        Params.AddWithValue("@CertificationTypeID", CertificationTypeID)
        Params.AddWithValue("@UserID", UserID)
        Params.AddWithValue("@UserSummaryID", UserSummaryID)

        Return SelectCommand("SelectAllUserItemsByParentIDCertificationType", Params)
    End Function

    Public Function SelectByIDUserSummary(ByVal UserSummaryID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@UserSummaryID", UserSummaryID)
        Return SelectCommand("SelectByIDUserSummary", Params)
    End Function

    Public Function SelectUserSummaryByUserIDCertificationTypeID(ByVal UserID As Integer, ByVal CertificationTypeID As Integer) As DataSet
        Dim params As SQLParamCollection = New SQLParamCollection("@UserID", UserID)
        params.AddWithValue("@CertificationTypeID", CertificationTypeID)
        Return SelectCommand("SelectUserSummaryByUserIDCertificationTypeID", params)
    End Function

    Public Function SelectUserItemsByUserIDItemID(ByVal UserID As Integer, ByVal ItemID As Integer, ByVal UserSummaryID As Integer) As DataSet
        Dim params As SQLParamCollection = New SQLParamCollection("@UserID", UserID)
        params.AddWithValue("@ItemID", ItemID)
        params.AddWithValue("@UserSummaryID", UserSummaryID)

        Return SelectCommand("SelectUserItemsByUserIDItemID", params)
    End Function

    Public Function SelectNextModuleByUserIDCertificationTypeID(ByVal UserID As Integer, ByVal CertificationTypeID As Integer, ByVal UserSummaryID As Integer) As DataSet
        Dim params As SQLParamCollection = New SQLParamCollection("@UserID", UserID)
        params.AddWithValue("@CertificationTypeID", CertificationTypeID)
        params.AddWithValue("@UserSummaryID", UserSummaryID)

        Return SelectCommand("SelectNextModuleByUserIDCertificationTypeID", params)
    End Function

    Public Function SelectLastNodeByUserIDCertificationTypeID(ByVal UserID As Integer, ByVal CertificationTypeID As Integer, ByVal UserSummaryID As Integer) As DataSet
        Dim params As SQLParamCollection = New SQLParamCollection("@UserID", UserID)
        params.AddWithValue("@CertificationTypeID", CertificationTypeID)
        params.AddWithValue("@UserSummaryID", UserSummaryID)
        Return SelectCommand("SelectLastNodeByUserIDCertificationTypeID", params)
    End Function

    Public Function SelectCountOfTotalItemsNotReviewed(ByVal UserID As Integer, ByVal CertificationTypeID As Integer, ByVal UserSummaryID As Integer) As DataSet
        Dim params As SQLParamCollection = New SQLParamCollection("@UserID", UserID)
        params.AddWithValue("@CertificationTypeID", CertificationTypeID)
        params.AddWithValue("@UserSummaryID", UserSummaryID)
        Return SelectCommand("SelectCountOfTotalItemsNotReviewed", params)
    End Function

    Public Function SelectAllUserQuestionsByUserIDCertificationTypeID(ByVal UserID As Integer, ByVal CertificationTypeID As Integer, ByVal UserSummaryID As Integer) As DataSet
        Dim params As SQLParamCollection = New SQLParamCollection("@UserID", UserID)
        params.AddWithValue("@CertificationTypeID", CertificationTypeID)
        params.AddWithValue("@UserSummaryID", UserSummaryID)
        Return SelectCommand("SelectAllUserQuestionsByUserIDCertificationTypeID", params)
    End Function

    Public Function SelectAllActiveCertificationTypeByUserID() As DataSet
        Return SelectCommand("SelectAllActiveCertificationTypeByUserID")
    End Function

    Public Function SelectByIDUserQuestions(ByVal UserQuestionsID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@UserQuestionsID", UserQuestionsID)
        Return SelectCommand("SelectByIDUserQuestions", Params)
    End Function

    Public Function SelectAllItemsByParentIDCertificationType(ByVal ParentID As Integer, ByVal CertificationTypeID As Integer) As DataSet
        Dim params As SQLParamCollection = New SQLParamCollection("@ParentID", ParentID)
        params.AddWithValue("@CertificationTypeID", CertificationTypeID)
        Return SelectCommand("SelectAllItemsByParentIDCertificationType", params)
    End Function

    Public Function SelectLastNodeByCertificationTypeID(ByVal CertificationTypeID As Integer) As DataSet
        Dim params As SQLParamCollection = New SQLParamCollection("@CertificationTypeID", CertificationTypeID)
        Return SelectCommand("SelectLastNodeByCertificationTypeID", params)
    End Function

    Public Function SelectNextModuleByCertificationTypeID(ByVal CertificationTypeID As Integer, ByVal ItemID As Integer) As DataSet
        Dim params As SQLParamCollection = New SQLParamCollection("@CertificationTypeID", CertificationTypeID)
        params.AddWithValue("@ItemID", ItemID)
        Return SelectCommand("SelectNextModuleByCertificationTypeID", params)
    End Function

    Public Function SelectFirstTreeNodeByCertificationTypeID(ByVal CertificationTypeID As Integer) As DataSet
        Dim params As SQLParamCollection = New SQLParamCollection("@CertificationTypeID", CertificationTypeID)
        Return SelectCommand("SelectFirstTreeNodeByCertificationTypeID", params)
    End Function

    Public Function SelectCountOfItemsByCertificationTypeID(ByVal CertificationTypeID As Integer) As DataSet
        Dim params As SQLParamCollection = New SQLParamCollection("@CertificationTypeID", CertificationTypeID)
        Return SelectCommand("SelectCountOfItemsByCertificationTypeID", params)
    End Function

    Public Function ReportCertificate(ByVal UserSummaryID As Integer) As DataSet
        Dim params As SQLParamCollection = New SQLParamCollection("@UserSummaryID", UserSummaryID)
        Return SelectCommand("ReportCertificate", params)
    End Function

    Public Function SelectCertifiedUsersByOptionalColumns(ByRef dto As CertDto.SearchDto) As DataSet
        Dim params As SQLParamCollection = New SQLParamCollection("@FirstName", dto.FirstName)
        params.AddWithValue("@MiddleName", dto.MiddleName)
        params.AddWithValue("@LastName", dto.LastName)
        params.AddWithValue("@Street", dto.Street)
        params.AddWithValue("@City", dto.City)
        params.AddWithValue("@County", dto.County)
        params.AddWithValue("@State", dto.State)
        params.AddWithValue("@ZipCode", dto.ZipCode)
        params.AddWithValue("@Company", dto.Company)
        params.AddWithValue("@IssueDate", dto.IssueDate)
        params.AddWithValue("@DriversLicense", dto.DriversLicense)
        params.AddWithValue("@CertificationNumber", dto.CertificationNumber)
        Return SelectCommand("SelectCertifiedUsersByOptionalColumns", params)
    End Function

    Public Function GradeExamUpdateUserSummaryExamScore(ByVal UserID As Integer, ByVal CertificationTypeID As Integer, ByVal UserSummaryID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@UserID", UserID)
        Params.AddWithValue("@CertificationTypeID", CertificationTypeID)
        Params.AddWithValue("@LastModifiedID", UserID)
        Params.AddWithValue("@LastModifiedNetworkID", System.Environment.UserName)
        Params.AddWithValue("@UserSummaryID", UserSummaryID)

        Return SelectCommand("GradeExamUpdateUserSummaryExamScore", Params)
    End Function

    Public Function SelectByItemsIDFromUserItems(ByVal ItemID As Integer, ByVal UserSummaryID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection("@ItemID", ItemID)
        Params.AddWithValue("@UserSummaryID", UserSummaryID)

        Return SelectCommand("SelectByItemsIDFromUserItems", Params)
    End Function

    Public Function SelectAllItemsOfTypeQuestionByCertificationTypeID(ByVal CertificationTypeID As Integer) As DataSet
        Dim params As SQLParamCollection = New SQLParamCollection("@CertificationTypeID", CertificationTypeID)
        Return SelectCommand("SelectAllItemsOfTypeQuestionByCertificationTypeID", params)
    End Function

    Public Function SelectAllFollowUpQuestionsCertificationTypeID(ByVal CertificationTypeID As Integer, ByVal ItemID As Integer, ByVal UserID As Integer, ByVal UserSummaryID As Integer) As DataSet
        Dim params As SQLParamCollection = New SQLParamCollection("@CertificationTypeID", CertificationTypeID)
        params.AddWithValue("@ItemID", ItemID)
        params.AddWithValue("@UserID", UserID)
        params.AddWithValue("@UserSummaryID", UserSummaryID)

        Return SelectCommand("SelectAllFollowUpQuestionsCertificationTypeID", params)
    End Function

    Public Function GradeFollowUpQuestions(ByVal UserID As Integer, ByVal CertificationTypeID As Integer) As DataSet
        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@UserID", UserID)
        Params.AddWithValue("@CertificationTypeID", CertificationTypeID)
        Params.AddWithValue("@LastModifiedID", UserID)
        Params.AddWithValue("@LastModifiedNetworkID", System.Environment.UserName)

        Return SelectCommand("GradeFollowUpQuestions", Params)
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

                Dim sInnerException As String = ""

                If Not ex.InnerException Is Nothing Then
                    sInnerException = sInnerException
                End If

                l.LogIt("SelectCommand: " & QueryName & vbCrLf & ex.Message & sInnerException, Params)
                Throw New Exception("SelectCommand: " & QueryName & vbCrLf & ex.Message & sInnerException)
            Catch ex2 As Exception
                Throw New Exception("SelectCommand and LogIt: " & QueryName & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex2.Message)
            End Try
        End Try

        Return ds

    End Function

#End Region

#Region "Save Methods"

    Public Function SaveUser(ByRef dto As CertDto.UserDto) As Boolean

        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@UserID", dto.UserID)
        Params.AddWithValue("@EmailAddress", dto.EmailAddress)
        Params.AddWithValue("@Password", dto.Password)
        Params.AddWithValue("@FName", dto.FName)
        Params.AddWithValue("@MName", dto.MName)
        Params.AddWithValue("@LName", dto.LName)
        Params.AddWithValue("@Street", dto.Street)
        Params.AddWithValue("@City", dto.City)
        Params.AddWithValue("@State", dto.State)
        Params.AddWithValue("@ZipCode", dto.ZipCode)
        Params.AddWithValue("@WorkPhone", dto.WorkPhone)
        Params.AddWithValue("@MobilePhone", dto.MobilePhone)
        Params.AddWithValue("@CompanyName", dto.CompanyName)
        Params.AddWithValue("@UseCompressedVideos", dto.UseCompressedVideos)
        Params.AddWithValue("@CreatedByID", dto.CreatedByID)
        Params.AddWithValue("@LastModifiedByID", dto.LastModifiedByID)
        Params.AddWithValue("@LastModifiedNetworkID", System.Environment.UserName)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

        Return SaveCommand("SaveUser", Params)

    End Function







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
        Params.AddWithValue("@ImageCaption", dto.ImageCaption)
        Params.AddWithValue("@VideoCaption", dto.VideoCaption)
        Params.AddWithValue("@NumberOfQuestions", dto.NumberOfQuestions)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

        Return SaveCommand("SaveItems", Params)

    End Function

    Public Function SaveQuestions(ByRef dto As CertDto.QuestionsDto) As Boolean

        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@QuestionsID", dto.QuestionsID)
        Params.AddWithValue("@CertificationTypeID", dto.CertificationTypeID)
        Params.AddWithValue("@ItemID", dto.ItemID)
        Params.AddWithValue("@Question", dto.Question)
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

    Public Function SaveCertificationType(ByRef dto As CertDto.CertificationTypeDto) As Boolean

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
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

        Return SaveCommand("SaveCertificationType", Params)

    End Function

    'Public Function SavePublicUserRegistration(ByRef dto As CertDto.PublicUserRegistrationDto) As Boolean

    '    Dim Params As SQLParamCollection = New SQLParamCollection()

    '    Params.AddWithValue("@PublicUserRegistrationID", dto.PublicUserRegistrationID)
    '    Params.AddWithValue("@FirstName", dto.FirstName)
    '    Params.AddWithValue("@MiddleName", dto.MiddleName)
    '    Params.AddWithValue("@LastName", dto.LastName)
    '    Params.AddWithValue("@EmailAddress", dto.EmailAddress)
    '    Params.AddWithValue("@Password", dto.Password)
    '    Params.AddWithValue("@TempPassword", dto.TempPassword)
    '    Params.AddWithValue("@WorkPhone", dto.WorkPhone)
    '    Params.AddWithValue("@MobilePhone", dto.MobilePhone)
    '    Params.AddWithValue("@HomePhone", dto.HomePhone)
    '    Params.AddWithValue("@Fax", dto.Fax)
    '    Params.AddWithValue("@CompanyName", dto.CompanyName)
    '    Params.AddWithValue("@JobResponsibility", dto.JobResponsibility)
    '    Params.AddWithValue("@DriverLicence", dto.DriverLicence)
    '    Params.AddWithValue("@Street", dto.Street)
    '    Params.AddWithValue("@City", dto.City)
    '    Params.AddWithValue("@State", dto.State)
    '    Params.AddWithValue("@Zip", dto.Zip)
    '    Params.AddWithValue("@County", dto.County)
    '    Params.AddWithValue("@UseCompressedVideos", dto.UseCompressedVideos)
    '    Params.AddWithValue("@RegisteredBy", dto.RegisteredBy)
    '    Params.AddWithValue("@RegisteredAt", dto.RegisteredAt)
    '    Params.AddWithValue("@RegisteredFrom", dto.RegisteredFrom)
    '    Params.AddWithValue("@UpdatedBy", dto.UpdatedBy)
    '    Params.AddWithValue("@UpdatedFrom", dto.UpdatedFrom)
    '    Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

    '    Return SaveCommand("SavePublicUserRegistration", Params)

    'End Function

    Public Function SaveUserItems(ByRef dto As CertDto.UserItemsDto) As Boolean

        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@UserItemsID", dto.UserItemsID)
        Params.AddWithValue("@UserID", dto.UserID)
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
        Params.AddWithValue("@UserSummaryID", dto.UserSummaryID)
        Params.AddWithValue("@ItemReviewed", dto.ItemReviewed)
        Params.AddWithValue("@Active", dto.Active)
        Params.AddWithValue("@CreatedByID", dto.CreatedByID)
        Params.AddWithValue("@LastModifiedID", dto.LastModifiedID)
        Params.AddWithValue("@LastModifiedNetworkID", System.Environment.UserName)
        Params.AddWithValue("@ImageCaption", dto.ImageCaption)
        Params.AddWithValue("@VideoCaption", dto.VideoCaption)
        Params.AddWithValue("@NumberOfQuestions", dto.NumberOfQuestions)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

        Return SaveCommand("SaveUserItems", Params)

    End Function

    Public Function CopyCurrentItemsForUser(ByVal UserID As Integer, ByVal CertificationTypeID As Integer, ByVal UserSummaryID As Integer) As Boolean
        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@UserID", UserID)
        Params.AddWithValue("@CertificationTypeID", CertificationTypeID)
        Params.AddWithValue("@UserSummaryID", UserSummaryID)
        Params.AddWithValue("@CreatedByID", UserID)
        Params.AddWithValue("@LastModifiedID", UserID)
        Params.AddWithValue("@LastModifiedNetworkID", System.Environment.UserName)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

        Return SaveCommand("CopyCurrentItemsForUser", Params)
    End Function

    Public Function CopyCurrentQuestionsForUser(ByVal UserID As Integer, ByVal CertificationTypeID As Integer, ByVal UserSummaryID As Integer) As Boolean
        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@UserID", UserID)
        Params.AddWithValue("@CertificationTypeID", CertificationTypeID)
        Params.AddWithValue("@UserSummaryID", UserSummaryID)
        Params.AddWithValue("@CreatedByID", UserID)
        Params.AddWithValue("@LastModifiedID", UserID)
        Params.AddWithValue("@LastModifiedNetworkID", System.Environment.UserName)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

        Return SaveCommand("CopyCurrentQuestionsForUser", Params)
    End Function

    Public Function UpdateAllUserItemsWithUserSummaryID(ByVal UserID As Integer, ByVal CertificationTypeID As Integer, ByVal UserSummaryID As Integer) As Boolean
        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@UserID", UserID)
        Params.AddWithValue("@CertificationTypeID", CertificationTypeID)
        Params.AddWithValue("@UserSummaryID", UserSummaryID)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

        Return SaveCommand("UpdateAllUserItemsWithUserSummaryID", Params)
    End Function

    Public Function UpdateAllUserQuestionsWithUserSummaryID(ByVal UserID As Integer, ByVal CertificationTypeID As Integer, ByVal UserSummaryID As Integer) As Boolean
        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@UserID", UserID)
        Params.AddWithValue("@CertificationTypeID", CertificationTypeID)
        Params.AddWithValue("@UserSummaryID", UserSummaryID)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

        Return SaveCommand("UpdateAllUserQuestionsWithUserSummaryID", Params)
    End Function

    Public Function SaveUserSummary(ByRef dto As CertDto.UserSummaryDto) As Boolean

        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@UserSummaryID", dto.UserSummaryID)
        Params.AddWithValue("@CertificationTypeID", dto.CertificationTypeID)
        Params.AddWithValue("@UserID", dto.UserID)
        Params.AddWithValue("@ExamScore", dto.ExamScore)
        Params.AddWithValue("@StartDate", dto.StartDate)
        Params.AddWithValue("@EndDate", dto.EndDate)
        Params.AddWithValue("@NextModuleItemID", dto.NextModuleItemID)
        Params.AddWithValue("@Active", dto.Active)
        Params.AddWithValue("@ExamEndDate", dto.ExamEndDate)
        Params.AddWithValue("@CreatedByID", dto.CreatedByID)
        Params.AddWithValue("@LastModifiedID", dto.LastModifiedID)
        Params.AddWithValue("@LastModifiedNetworkID", System.Environment.UserName)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

        Return SaveCommand("SaveUserSummary", Params)

    End Function

    Public Function SaveUserQuestions(ByRef dto As CertDto.UserQuestionsDto) As Boolean

        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@UserQuestionsID", dto.UserQuestionsID)
        Params.AddWithValue("@UserID", dto.UserID)
        Params.AddWithValue("@QuestionsID", dto.QuestionsID)
        Params.AddWithValue("@CertificationTypeID", dto.CertificationTypeID)
        Params.AddWithValue("@ItemID", dto.ItemID)
        Params.AddWithValue("@Question", dto.Question)
        Params.AddWithValue("@Answer1", dto.Answer1)
        Params.AddWithValue("@Answer2", dto.Answer2)
        Params.AddWithValue("@Answer3", dto.Answer3)
        Params.AddWithValue("@Answer4", dto.Answer4)
        Params.AddWithValue("@Answer5", dto.Answer5)
        Params.AddWithValue("@CorrectAnswer", dto.CorrectAnswer)
        Params.AddWithValue("@QuestionWeight", dto.QuestionWeight)
        Params.AddWithValue("@DisplaySequence", dto.DisplaySequence)
        Params.AddWithValue("@SelectedAnswer", dto.SelectedAnswer)
        Params.AddWithValue("@AnswerScore", dto.AnswerScore)
        Params.AddWithValue("@NumberOfAnswers", dto.NumberOfAnswers)
        Params.AddWithValue("@Active", dto.Active)
        Params.AddWithValue("@UserSummaryID", dto.UserSummaryID)
        Params.AddWithValue("@FollowUpQuestionsCompleted", dto.FollowUpQuestionsCompleted)
        Params.AddWithValue("@IsFollowUpQuestion", dto.IsFollowUpQuestion)
        Params.AddWithValue("@CreatedByID", dto.CreatedByID)
        Params.AddWithValue("@LastModifiedID", dto.LastModifiedID)
        Params.AddWithValue("@LastModifiedNetworkID", System.Environment.UserName)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

        Return SaveCommand("SaveUserQuestions", Params)

    End Function

    Public Function SaveUserQuestionsAnswer(ByVal UserQuestionsID As Integer, ByVal SelectedAnswer As String, ByVal LastModifiedID As Integer) As Boolean

        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@UserQuestionsID", UserQuestionsID)
        Params.AddWithValue("@SelectedAnswer", SelectedAnswer)
        Params.AddWithValue("@LastModifiedID", LastModifiedID)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

        Return SaveCommand("SaveUserQuestionsAnswer", Params)

    End Function

    Public Function SaveFollowUpUserQuestionsAnswer(ByVal UserQuestionsID As Integer, ByVal SelectedAnswer As String, ByVal FollowUpQuestionsCompleted As Boolean, ByVal LastModifiedID As Integer) As Boolean

        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@UserQuestionsID", UserQuestionsID)
        Params.AddWithValue("@SelectedAnswer", SelectedAnswer)
        Params.AddWithValue("@FollowUpQuestionsCompleted", FollowUpQuestionsCompleted)
        Params.AddWithValue("@LastModifiedID", LastModifiedID)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

        Return SaveCommand("SaveFollowUpUserQuestionsAnswer", Params)

    End Function

    Public Function CopyRandomQuestionsForUser(ByVal UserID As Integer, ByVal CertificationTypeID As Integer, ByVal UserSummaryID As Integer, ByVal NumberOfQuestions As Integer, ByVal ItemID As Integer) As Boolean
        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@UserID", UserID)
        Params.AddWithValue("@CertificationTypeID", CertificationTypeID)
        Params.AddWithValue("@UserSummaryID", UserSummaryID)
        Params.AddWithValue("@NumberOfQuestions", NumberOfQuestions)
        Params.AddWithValue("@ItemID", ItemID)
        Params.AddWithValue("@CreatedByID", UserID)
        Params.AddWithValue("@LastModifiedID", UserID)
        Params.AddWithValue("@LastModifiedNetworkID", System.Environment.UserName)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

        Return SaveCommand("CopyRandomQuestionsForUser", Params)
    End Function

    Public Function CopyFollowUpQuestionsForUser(ByVal UserID As Integer, ByVal CertificationTypeID As Integer) As Boolean
        Dim Params As SQLParamCollection = New SQLParamCollection()

        Params.AddWithValue("@UserID", UserID)
        Params.AddWithValue("@CertificationTypeID", CertificationTypeID)
        Params.AddWithValue("@CreatedByID", UserID)
        Params.AddWithValue("@LastModifiedID", UserID)
        Params.AddWithValue("@LastModifiedNetworkID", System.Environment.UserName)
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)

        Return SaveCommand("CopyFollowUpQuestionsForUser", Params)
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

            If Not Integer.TryParse(Cmd.Parameters("@IdentityVal").Value.ToString(), Identity) Then
                Throw New Exception("SaveCommand unable to parse @IdentityVal to an integer.  @IdentityVal = '" & Cmd.Parameters("@IdentityVal").Value.ToString() & "'")
            End If


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
        Params.AddWithValue("@IdentityVal", 0, System.Data.SqlDbType.Int, ParameterDirection.Output)
        Return UpdateCommand("ResetPublicUserPassword", Params)
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

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            If Not bLogitFailed Then Throw New Exception("UpdateCommand: " & QueryName & vbCrLf & ex.Message & sInnerException)
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

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            If Not bLogitFailed Then Throw New Exception("InsertCommand: " & QueryName & vbCrLf & ex.Message & sInnerException)
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

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            If Not bLogitFailed Then Throw New Exception("DeleteCommand: " & QueryName & vbCrLf & ex.Message & sInnerException)
        End Try

        Return RetVal

    End Function

#End Region

    Public Sub New()


        Try
            ConnectToDB()
        Catch ex As Exception
            Try
                'If it doesn't connect, let's pause for 1/5 second
                Thread.Sleep(500)
                ConnectToDB()
            Catch ex2 As Exception
                Try
                    'If it doesn't connect, let's pause for 1/5 second
                    Thread.Sleep(500)
                    ConnectToDB()
                Catch ex3 As Exception

                    Dim sInnerException As String = ""
                    If Not ex.InnerException Is Nothing Then
                        sInnerException = sInnerException
                    End If

                    l.LogIt("Create DB Object: " & vbCrLf & ex3.Message & ex3.InnerException.Message)
                    Throw (New Exception("Unable to connect to the database.  Please check your configuration settings or contact your adminstrator." & vbCrLf))
                End Try
            End Try
        End Try
    End Sub

    Private Sub ConnectToDB()
        Me.Conn = New SqlConnection(ConnStr)
        Me.Conn.Open()
        Me.RowCount = 0
        Me.Identity = 0
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

    Public Sub Dispose()
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