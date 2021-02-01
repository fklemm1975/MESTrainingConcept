Imports Microsoft.VisualBasic
Imports System.Data
Imports System

Public Class BMPXDto

#Region "BMPXUserDto"

    Public Class BMPXUserDto

#Region "Properties"

        Private objdb As BMPXDB

        Private iBMPXUserID As Int32 = 0
        Private sFirstName As String
        Private sLastName As String
        Private sEmailAddress As String
        Private sPassword As String = ""
        Private sAgency As String
        Private sAddress As String = ""
        Private sAddress2 As String = ""
        Private sCity As String = ""
        Private sState As String = ""
        Private sZip As String = ""
        Private bAdministrator As Boolean
        Private bActive As Boolean
        Private bAccountLocked As Boolean = False
        Private bPasswordResetRequired As Boolean = True
        Private iFailedLoginAttempts As Int32 = 0
        Private iCreatedByID As Int32
        Private dCreatedByDate As DateTime
        Private iUpdatedByID As Int32
        Private dUpdatedByDate As DateTime

        Public ValidationCode As Integer = -1


        Public Property BMPXUserID() As Int32
            Get
                BMPXUserID = iBMPXUserID
            End Get
            Set(ByVal value As Int32)
                iBMPXUserID = value
            End Set
        End Property

        Public Property FirstName() As String
            Get
                FirstName = sFirstName
            End Get
            Set(ByVal value As String)
                sFirstName = value
            End Set
        End Property

        Public Property LastName() As String
            Get
                LastName = sLastName
            End Get
            Set(ByVal value As String)
                sLastName = value
            End Set
        End Property

        Public Property EmailAddress() As String
            Get
                EmailAddress = sEmailAddress
            End Get
            Set(ByVal value As String)
                sEmailAddress = value
            End Set
        End Property

        Public Property Password() As String
            Get
                Password = sPassword
            End Get
            Set(ByVal value As String)
                sPassword = value
            End Set
        End Property

        Public Property Agency() As String
            Get
                Agency = sAgency
            End Get
            Set(ByVal value As String)
                sAgency = value
            End Set
        End Property

        Public Property Address() As String
            Get
                Address = sAddress
            End Get
            Set(ByVal value As String)
                sAddress = value
            End Set
        End Property

        Public Property Address2() As String
            Get
                Address2 = sAddress2
            End Get
            Set(ByVal value As String)
                sAddress2 = value
            End Set
        End Property

        Public Property City() As String
            Get
                City = sCity
            End Get
            Set(ByVal value As String)
                sCity = value
            End Set
        End Property

        Public Property State() As String
            Get
                State = sState
            End Get
            Set(ByVal value As String)
                sState = value
            End Set
        End Property

        Public Property Zip() As String
            Get
                Zip = sZip
            End Get
            Set(ByVal value As String)
                sZip = value
            End Set
        End Property

        Public Property Administrator() As Boolean
            Get
                Administrator = bAdministrator
            End Get
            Set(ByVal value As Boolean)
                bAdministrator = value
            End Set
        End Property

        Public Property Active() As Boolean
            Get
                Active = bActive
            End Get
            Set(ByVal value As Boolean)
                bActive = value
            End Set
        End Property

        Public Property AccountLocked() As Boolean
            Get
                AccountLocked = bAccountLocked
            End Get
            Set(ByVal value As Boolean)
                bAccountLocked = value
            End Set
        End Property

        Public Property PasswordResetRequired() As Boolean
            Get
                PasswordResetRequired = bPasswordResetRequired
            End Get
            Set(ByVal value As Boolean)
                bPasswordResetRequired = value
            End Set
        End Property

        Public Property FailedLoginAttempts() As Int32
            Get
                FailedLoginAttempts = iFailedLoginAttempts
            End Get
            Set(ByVal value As Int32)
                iFailedLoginAttempts = value
            End Set
        End Property

        Public Property CreatedByID() As Int32
            Get
                CreatedByID = iCreatedByID
            End Get
            Set(ByVal value As Int32)
                iCreatedByID = value
            End Set
        End Property

        Public Property CreatedByDate() As DateTime
            Get
                CreatedByDate = dCreatedByDate
            End Get
            Set(ByVal value As DateTime)
                dCreatedByDate = value
            End Set
        End Property

        Public Property UpdatedByID() As Int32
            Get
                UpdatedByID = iUpdatedByID
            End Get
            Set(ByVal value As Int32)
                iUpdatedByID = value
            End Set
        End Property

        Public Property UpdatedByDate() As DateTime
            Get
                UpdatedByDate = dUpdatedByDate
            End Get
            Set(ByVal value As DateTime)
                dUpdatedByDate = value
            End Set
        End Property

#End Region

        Public Sub Populate(ByRef ID As Integer, ByRef db As BMPXDB)

            Try

                objdb = db
                Populate(ID)
                objdb = Nothing

            Catch ex As Exception
                Throw New Exception("BMPXUserDto.Populate(ByVal ID As Integer, ByRef db As BMPXDB) Error:   ID = " & ID.ToString & vbCrLf & ex.Message)
            End Try
        End Sub

        Public Sub Populate(ByRef ID As Integer)

            Dim bDBIsNothing As Boolean = objdb Is Nothing
            If bDBIsNothing Then objdb = New BMPXDB

            Try
                Dim ds As DataSet = objdb.SelectByIDBMPXUser(ID)

                If ds.Tables(0).Rows.Count > 0 Then
                    Dim dr As DataRow = ds.Tables(0).Rows(0)
                    Me.BMPXUserID = dr("BMPXUserID")
                    Me.FirstName = dr("FirstName")
                    Me.LastName = dr("LastName")
                    Me.EmailAddress = dr("EmailAddress")
                    Me.Password = dr("Password")
                    Me.Agency = dr("Agency")
                    Me.Address = dr("Address")
                    Me.Address2 = dr("Address2")
                    Me.City = dr("City")
                    Me.State = dr("State")
                    Me.Zip = dr("Zip")
                    Me.Administrator = dr("Administrator")
                    Me.Active = dr("Active")
                    Me.AccountLocked = dr("AccountLocked")
                    Me.PasswordResetRequired = dr("PasswordResetRequired")
                    Me.FailedLoginAttempts = dr("FailedLoginAttempts")
                    Me.CreatedByID = dr("CreatedByID")
                    Me.CreatedByDate = dr("CreatedByDate")
                    Me.UpdatedByID = dr("UpdatedByID")
                    Me.UpdatedByDate = dr("UpdatedByDate")
                End If
            Catch ex As Exception
                Throw New Exception("Error BMPXUserDTO.Populate: " & vbCrLf & ex.Message)
            Finally
                If bDBIsNothing Then objdb = Nothing
            End Try
        End Sub

        Public Function Save(Optional ByVal SavePassword As Boolean = True) As Boolean

            Dim bDBIsNothing As Boolean = objdb Is Nothing
            If bDBIsNothing Then objdb = New BMPXDB
            Dim retval As Boolean = False

            Try

                'retval = db.SaveBMPXUser(Me)
                'Me.BMPXUserID = db.Identity
                If SavePassword Then
                    retval = objdb.SaveBMPXUser(Me)
                Else
                    retval = objdb.SaveBMPXUserNoPassword(Me)
                End If

                Me.BMPXUserID = objdb.Identity
                Me.ValidationCode = objdb.ValidationCode

                Select Case Me.ValidationCode
                    Case 100
                        retval = True
                    Case 101
                        Throw New Exception("This email address already exists.")
                    Case Else
                        Throw New Exception("Error saving user.")
                End Select


            Catch ex As Exception
                Throw New Exception("BMPXUserDto.Save: " & ex.Message & vbCrLf)

            Finally
                If bDBIsNothing Then objdb = Nothing
            End Try

            Return retval

        End Function

    End Class

#End Region

#Region "UploadedFilesDto"

    Public Class UploadedFilesDto

#Region "Properties"

        Private objdb As BMPXDB

        Private iUploadedFilesID As Int32 = 0
        Private iBMPXUserID As Int32
        Private iFileTypeID As Int32
        Private iFiscalYearID As Int32
        Private iSubmissionID As Int32
        Private sDescription As String
        Private sOriginalFilename As String
        Private sStoredFilename As String
        Private dUploadDate As DateTime
        Private bLocked As Boolean
        Private dLockedDate As DateTime
        Private bProcessed As Boolean
        Private dProcessedDate As DateTime
        Private bAccepted As Boolean
        Private iAcceptedByID As Int32
        Private dAcceptedDate As DateTime
        Private sBMPURL As String
        Private sBMPEventURL As String
        Private sManureTransportURL As String
        Private sManureTransportEventURL As String

        Public Property UploadedFilesID() As Int32
            Get
                UploadedFilesID = iUploadedFilesID
            End Get
            Set(ByVal value As Int32)
                iUploadedFilesID = value
            End Set
        End Property

        Public Property BMPXUserID() As Int32
            Get
                BMPXUserID = iBMPXUserID
            End Get
            Set(ByVal value As Int32)
                iBMPXUserID = value
            End Set
        End Property

        Public Property FileTypeID() As Int32
            Get
                FileTypeID = iFileTypeID
            End Get
            Set(ByVal value As Int32)
                iFileTypeID = value
            End Set
        End Property

        Public Property FiscalYearID() As Int32
            Get
                FiscalYearID = iFiscalYearID
            End Get
            Set(ByVal value As Int32)
                iFiscalYearID = value
            End Set
        End Property

        Public Property SubmissionID() As Int32
            Get
                SubmissionID = iSubmissionID
            End Get
            Set(ByVal value As Int32)
                iSubmissionID = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Description = sDescription
            End Get
            Set(ByVal value As String)
                sDescription = value
            End Set
        End Property

        Public Property OriginalFilename() As String
            Get
                OriginalFilename = sOriginalFilename
            End Get
            Set(ByVal value As String)
                sOriginalFilename = value
            End Set
        End Property

        Public Property StoredFilename() As String
            Get
                StoredFilename = sStoredFilename
            End Get
            Set(ByVal value As String)
                sStoredFilename = value
            End Set
        End Property

        Public Property UploadDate() As DateTime
            Get
                UploadDate = dUploadDate
            End Get
            Set(ByVal value As DateTime)
                dUploadDate = value
            End Set
        End Property

        Public Property Locked() As Boolean
            Get
                Locked = bLocked
            End Get
            Set(ByVal value As Boolean)
                bLocked = value
            End Set
        End Property

        Public Property LockedDate() As DateTime
            Get
                LockedDate = dLockedDate
            End Get
            Set(ByVal value As DateTime)
                dLockedDate = value
            End Set
        End Property

        Public Property Processed() As Boolean
            Get
                Processed = bProcessed
            End Get
            Set(ByVal value As Boolean)
                bProcessed = value
            End Set
        End Property

        Public Property ProcessedDate() As DateTime
            Get
                ProcessedDate = dProcessedDate
            End Get
            Set(ByVal value As DateTime)
                dProcessedDate = value
            End Set
        End Property

        Public Property Accepted() As Boolean
            Get
                Accepted = bAccepted
            End Get
            Set(ByVal value As Boolean)
                bAccepted = value
            End Set
        End Property

        Public Property AcceptedByID() As Int32
            Get
                AcceptedByID = iAcceptedByID
            End Get
            Set(ByVal value As Int32)
                iAcceptedByID = value
            End Set
        End Property

        Public Property AcceptedDate() As DateTime
            Get
                AcceptedDate = dAcceptedDate
            End Get
            Set(ByVal value As DateTime)
                dAcceptedDate = value
            End Set
        End Property

        Public Property BMPURL() As String
            Get
                BMPURL = sBMPURL
            End Get
            Set(ByVal value As String)
                sBMPURL = value
            End Set
        End Property

        Public Property BMPEventURL() As String
            Get
                BMPEventURL = sBMPEventURL
            End Get
            Set(ByVal value As String)
                sBMPEventURL = value
            End Set
        End Property

        Public Property ManureTransportURL() As String
            Get
                ManureTransportURL = sManureTransportURL
            End Get
            Set(ByVal value As String)
                sManureTransportURL = value
            End Set
        End Property

        Public Property ManureTransportEventURL() As String
            Get
                ManureTransportEventURL = sManureTransportEventURL
            End Get
            Set(ByVal value As String)
                sManureTransportEventURL = value
            End Set
        End Property
#End Region

        Public Sub Populate(ByRef ID As Integer, ByRef db As BMPXDB)

            Try

                objdb = db
                Populate(ID)
                objdb = Nothing

            Catch ex As Exception
                Throw New Exception("UploadedFilesDto.Populate(ByVal ID As Integer, ByRef db As BMPXDB) Error:   ID = " & ID.ToString & vbCrLf & ex.Message)
            End Try
        End Sub

        Public Sub Populate(ByRef ID As Integer)

            Dim bDBIsNothing As Boolean = objdb Is Nothing
            If bDBIsNothing Then objdb = New BMPXDB

            Try

                Dim ds As DataSet = objdb.SelectByIDUploadedFiles(ID)

                If ds.Tables(0).Rows.Count > 0 Then
                    Dim dr As DataRow = ds.Tables(0).Rows(0)
                    Me.UploadedFilesID = dr("UploadedFilesID")
                    Me.BMPXUserID = dr("BMPXUserID")
                    Me.FileTypeID = dr("FileTypeID")
                    Me.FiscalYearID = dr("FiscalYearID")
                    Me.SubmissionID = dr("SubmissionID")
                    Me.Description = dr("Description")
                    Me.OriginalFilename = dr("OriginalFilename")
                    Me.StoredFilename = dr("StoredFilename")
                    Me.UploadDate = dr("UploadDate")
                    Me.Locked = dr("Locked")
                    Me.LockedDate = dr("LockedDate")
                    Me.Processed = dr("Processed")
                    Me.ProcessedDate = dr("ProcessedDate")
                    Me.Accepted = dr("Accepted")
                    Me.AcceptedByID = dr("AcceptedByID")
                    Me.AcceptedDate = dr("AcceptedDate")
                    '10/17/2018 - GRP - These are in the submission table.  Not sure what I was thinking here.
                    'Me.BMPURL = dr("BMPURL")
                    'Me.BMPEventURL = dr("BMPEventURL")
                    'Me.ManureTransportURL = dr("ManureTransportURL")
                    'Me.ManureTransportEventURL = dr("ManureTransportEventURL")
                End If
            Catch ex As Exception
                Throw New Exception("Error UploadedFilesDTO.Populate: " & vbCrLf & ex.Message)
            Finally
                If bDBIsNothing Then objdb = Nothing
            End Try
        End Sub

        Public Function Save(Optional ByVal db As BMPXDB = Nothing) As Boolean

            Dim bDBIsNothing As Boolean = db Is Nothing
            If bDBIsNothing Then db = New BMPXDB
            Dim retval As Boolean = False

            Try

                retval = db.SaveUploadedFiles(Me)
                Me.UploadedFilesID = db.Identity

            Catch ex As Exception
                Throw New Exception("UploadedFilesDto.Save: " & ex.Message & vbCrLf)

            Finally
                If bDBIsNothing Then db = Nothing
            End Try

            Return retval

        End Function

        Public Function Delete(ByVal UploadedFilesID As Integer, Optional ByRef db As BMPXDB = Nothing) As Boolean

            Dim bDBIsNothing As Boolean = db Is Nothing
            If bDBIsNothing Then db = New BMPXDB
            Dim retval As Boolean = False

            Try
                iUploadedFilesID = UploadedFilesID

                retval = db.DeleteUploadedFilesByUploadedFilesID(UploadedFilesID)

                Return retval
            Catch ex As Exception
                Throw New Exception("Error UploadedFilesDto.Delete: " & vbCrLf & ex.Message)
            Finally
                If bDBIsNothing Then db = Nothing
            End Try

        End Function

    End Class

#End Region

#Region "SubmissionDto"

    Public Class SubmissionDto

#Region "Properties"

        Private objdb As BMPXDB

        Private iSubmissionID As Int32 = 0
        Private iFiscalYearID As Int32
        Private iBMPXUserID As Int32
        Private dSubmissionDate As DateTime
        Private bProcessed As Boolean
        Private dProcessedDate As DateTime
        Private bValidatedSuccessfully As Boolean
        Private sDataSourceCode As String = ""
        Private sFirstName As String = ""
        Private sLastName As String = ""
        Private sAgency As String = ""
        Private sTitle As String = ""
        Private sEmail As String = ""
        Private sPhone As String = ""
        Private sAddress As String = ""
        Private sAddress2 As String = ""
        Private sCity As String = ""
        Private sState As String = ""
        Private sZipCode As String = ""
        Private sBMPURL As String = ""
        Private sBMPEventURL As String = ""
        Private sManureTransportURL As String = ""
        Private sManureTransportEventURL As String = ""

        Public Property SubmissionID() As Int32
            Get
                SubmissionID = iSubmissionID
            End Get
            Set(ByVal value As Int32)
                iSubmissionID = value
            End Set
        End Property

        Public Property FiscalYearID() As Int32
            Get
                FiscalYearID = iFiscalYearID
            End Get
            Set(ByVal value As Int32)
                iFiscalYearID = value
            End Set
        End Property

        Public Property BMPXUserID() As Int32
            Get
                BMPXUserID = iBMPXUserID
            End Get
            Set(ByVal value As Int32)
                iBMPXUserID = value
            End Set
        End Property

        Public Property SubmissionDate() As DateTime
            Get
                SubmissionDate = dSubmissionDate
            End Get
            Set(ByVal value As DateTime)
                dSubmissionDate = value
            End Set
        End Property

        Public Property Processed() As Boolean
            Get
                Processed = bProcessed
            End Get
            Set(ByVal value As Boolean)
                bProcessed = value
            End Set
        End Property

        Public Property ProcessedDate() As DateTime
            Get
                ProcessedDate = dProcessedDate
            End Get
            Set(ByVal value As DateTime)
                dProcessedDate = value
            End Set
        End Property

        Public Property ValidatedSuccessfully() As Boolean
            Get
                ValidatedSuccessfully = bValidatedSuccessfully
            End Get
            Set(ByVal value As Boolean)
                bValidatedSuccessfully = value
            End Set
        End Property

        Public Property DataSourceCode() As String
            Get
                DataSourceCode = sDataSourceCode
            End Get
            Set(ByVal value As String)
                sDataSourceCode = value
            End Set
        End Property

        Public Property FirstName() As String
            Get
                FirstName = sFirstName
            End Get
            Set(ByVal value As String)
                sFirstName = value
            End Set
        End Property

        Public Property LastName() As String
            Get
                LastName = sLastName
            End Get
            Set(ByVal value As String)
                sLastName = value
            End Set
        End Property

        Public Property Agency() As String
            Get
                Agency = sAgency
            End Get
            Set(ByVal value As String)
                sAgency = value
            End Set
        End Property

        Public Property Title() As String
            Get
                Title = sTitle
            End Get
            Set(ByVal value As String)
                sTitle = value
            End Set
        End Property

        Public Property Email() As String
            Get
                Email = sEmail
            End Get
            Set(ByVal value As String)
                sEmail = value
            End Set
        End Property

        Public Property Phone() As String
            Get
                Phone = sPhone
            End Get
            Set(ByVal value As String)
                sPhone = value
            End Set
        End Property

        Public Property Address() As String
            Get
                Address = sAddress
            End Get
            Set(ByVal value As String)
                sAddress = value
            End Set
        End Property

        Public Property Address2() As String
            Get
                Address2 = sAddress2
            End Get
            Set(ByVal value As String)
                sAddress2 = value
            End Set
        End Property

        Public Property City() As String
            Get
                City = sCity
            End Get
            Set(ByVal value As String)
                sCity = value
            End Set
        End Property

        Public Property State() As String
            Get
                State = sState
            End Get
            Set(ByVal value As String)
                sState = value
            End Set
        End Property

        Public Property ZipCode() As String
            Get
                ZipCode = sZipCode
            End Get
            Set(ByVal value As String)
                sZipCode = value
            End Set
        End Property

        Public Property BMPURL() As String
            Get
                BMPURL = sBMPURL
            End Get
            Set(ByVal value As String)
                sBMPURL = value
            End Set
        End Property

        Public Property BMPEventURL() As String
            Get
                BMPEventURL = sBMPEventURL
            End Get
            Set(ByVal value As String)
                sBMPEventURL = value
            End Set
        End Property

        Public Property ManureTransportURL() As String
            Get
                ManureTransportURL = sManureTransportURL
            End Get
            Set(ByVal value As String)
                sManureTransportURL = value
            End Set
        End Property

        Public Property ManureTransportEventURL() As String
            Get
                ManureTransportEventURL = sManureTransportEventURL
            End Get
            Set(ByVal value As String)
                sManureTransportEventURL = value
            End Set
        End Property

#End Region

        Public Sub Populate(ByRef ID As Integer, ByRef db As BMPXDB)

            Try

                objdb = db
                Populate(ID)
                objdb = Nothing

            Catch ex As Exception
                Throw New Exception("SubmissionDto.Populate(ByVal ID As Integer, ByRef db As BMPXDB) Error:   ID = " & ID.ToString & vbCrLf & ex.Message)
            End Try
        End Sub

        Public Sub Populate(ByRef ID As Integer)

            Dim bDBIsNothing As Boolean = objdb Is Nothing
            If bDBIsNothing Then objdb = New BMPXDB

            Try

                Dim ds As DataSet = objdb.SelectByIDSubmission(ID)

                If ds.Tables(0).Rows.Count > 0 Then
                    Dim dr As DataRow = ds.Tables(0).Rows(0)
                    Me.SubmissionID = dr("SubmissionID")
                    Me.FiscalYearID = dr("FiscalYearID")
                    Me.BMPXUserID = dr("BMPXUserID")
                    Me.SubmissionDate = dr("SubmissionDate")
                    Me.Processed = dr("Processed")
                    Me.ProcessedDate = dr("ProcessedDate")
                    Me.ValidatedSuccessfully = dr("ValidatedSuccessfully")
                    Me.DataSourceCode = dr("DataSourceCode")
                    Me.FirstName = dr("FirstName")
                    Me.LastName = dr("LastName")
                    Me.Agency = dr("Agency")
                    Me.Title = dr("Title")
                    Me.Email = dr("Email")
                    Me.Phone = dr("Phone")
                    Me.Address = dr("Address")
                    Me.Address2 = dr("Address2")
                    Me.City = dr("City")
                    Me.State = dr("State")
                    Me.ZipCode = dr("ZipCode")
                    Me.BMPURL = dr("BMPURL")
                    Me.BMPEventURL = dr("BMPEventURL")
                    Me.ManureTransportURL = dr("ManureTransportURL")
                    Me.ManureTransportEventURL = dr("ManureTransportEventURL")
                End If
            Catch ex As Exception
                Throw New Exception("Error SubmissionDTO.Populate: " & vbCrLf & ex.Message)
            Finally
                If bDBIsNothing Then objdb = Nothing
            End Try
        End Sub

        Public Function Save(Optional ByVal db As BMPXDB = Nothing) As Boolean

            Dim bDBIsNothing As Boolean = db Is Nothing
            If bDBIsNothing Then db = New BMPXDB
            Dim retval As Boolean = False

            Try

                retval = db.SaveSubmission(Me)
                Me.SubmissionID = db.Identity

            Catch ex As Exception
                Throw New Exception("SubmissionDto.Save: " & ex.Message & vbCrLf)

            Finally
                If bDBIsNothing Then db = Nothing
            End Try

            Return retval

        End Function

        Public Function SaveSubmissionUploadedFiles(Optional ByVal db As BMPXDB = Nothing) As Boolean

            Dim bDBIsNothing As Boolean = db Is Nothing
            If bDBIsNothing Then db = New BMPXDB
            Dim retval As Boolean = False

            Try

                retval = db.SaveSubmissionUploadedFiles(Me)
                Me.SubmissionID = db.Identity

            Catch ex As Exception
                Throw New Exception("SubmissionDto.SaveSubmissionUploadedFiles: " & ex.Message & vbCrLf)

            Finally
                If bDBIsNothing Then db = Nothing
            End Try

            Return retval

        End Function


    End Class

#End Region




#Region "AgencyDto"
    Public Class AgencyDto

#Region "Properties"

        Private objdb As BMPXDB

        Private iAgencyID As Int32 = 0
        Private sOriginatingName As String
        Private sNEIENCode As String
        Private sDataSourceCode As String
        Private sSB_NAME As String
        Private iCreatedByID As Int32
        Private dCreatedByDate As DateTime
        Private iUpdatedByID As Int32
        Private dUpdatedByDate As DateTime

        Public Property AgencyID() As Int32
            Get
                AgencyID = iAgencyID
            End Get
            Set(ByVal value As Int32)
                iAgencyID = value
            End Set
        End Property

        Public Property OriginatingName() As String
            Get
                OriginatingName = sOriginatingName
            End Get
            Set(ByVal value As String)
                sOriginatingName = value
            End Set
        End Property

        Public Property NEIENCode() As String
            Get
                NEIENCode = sNEIENCode
            End Get
            Set(ByVal value As String)
                sNEIENCode = value
            End Set
        End Property

        Public Property DataSourceCode() As String
            Get
                DataSourceCode = sDataSourceCode
            End Get
            Set(ByVal value As String)
                sDataSourceCode = value
            End Set
        End Property

        Public Property SB_NAME() As String
            Get
                SB_NAME = sSB_NAME
            End Get
            Set(ByVal value As String)
                sSB_NAME = value
            End Set
        End Property

        Public Property CreatedByID() As Int32
            Get
                CreatedByID = iCreatedByID
            End Get
            Set(ByVal value As Int32)
                iCreatedByID = value
            End Set
        End Property

        Public Property CreatedByDate() As DateTime
            Get
                CreatedByDate = dCreatedByDate
            End Get
            Set(ByVal value As DateTime)
                dCreatedByDate = value
            End Set
        End Property

        Public Property UpdatedByID() As Int32
            Get
                UpdatedByID = iUpdatedByID
            End Get
            Set(ByVal value As Int32)
                iUpdatedByID = value
            End Set
        End Property

        Public Property UpdatedByDate() As DateTime
            Get
                UpdatedByDate = dUpdatedByDate
            End Get
            Set(ByVal value As DateTime)
                dUpdatedByDate = value
            End Set
        End Property

#End Region

        Public Sub Populate(ByRef ID As Integer, ByRef db As BMPXDB)

            Try

                objdb = db
                Populate(ID)
                objdb = Nothing

            Catch ex As Exception
                Throw New Exception("AgencyDto.Populate(ByVal ID As Integer, ByRef db As BMPXDB) Error:   ID = " & ID.ToString & vbCrLf & ex.Message)
            End Try
        End Sub
        Public Sub Populate(ByRef ID As Integer)

            Dim bDBIsNothing As Boolean = objdb Is Nothing
            If bDBIsNothing Then objdb = New BMPXDB

            Try

                Dim ds As DataSet = objdb.SelectByIDAgency(ID)

                If ds.Tables(0).Rows.Count > 0 Then
                    Dim dr As DataRow = ds.Tables(0).Rows(0)
                    Me.AgencyID = dr("AgencyID")
                    Me.OriginatingName = dr("OriginatingName")
                    Me.NEIENCode = dr("NEIENCode")
                    Me.DataSourceCode = dr("DataSourceCode")
                    Me.SB_NAME = dr("SB_NAME")
                    Me.CreatedByID = dr("CreatedByID")
                    Me.CreatedByDate = dr("CreatedByDate")
                    Me.UpdatedByID = dr("UpdatedByID")
                    Me.UpdatedByDate = dr("UpdatedByDate")
                End If
            Catch ex As Exception
                Throw New Exception("Error AgencyDTO.Populate: " & vbCrLf & ex.Message)
            Finally
                If bDBIsNothing Then objdb = Nothing
            End Try
        End Sub

        Public Function Save(Optional ByVal db As BMPXDB = Nothing) As Boolean

            Dim bDBIsNothing As Boolean = db Is Nothing
            If bDBIsNothing Then db = New BMPXDB
            Dim retval As Boolean = False

            Try

                retval = db.SaveAgency(Me)
                Me.AgencyID = db.Identity

            Catch ex As Exception
                Throw New Exception("AgencyDto.Save: " & ex.Message & vbCrLf)

            Finally
                If bDBIsNothing Then db = Nothing
            End Try

            Return retval

        End Function

    End Class

#End Region

#Region "NotificationDto"

    Public Class NotificationDto

#Region "Properties"

        Private objdb As BMPXDB

        Private iNotificationID As Int32 = 0
        Private sText As String
        Private bActive As Boolean
        Private iCreatedByID As Int32
        Private dCreatedByDate As DateTime
        Private iUpdatedByID As Int32
        Private dUpdatedByDate As DateTime

        Public Property NotificationID() As Int32
            Get
                NotificationID = iNotificationID
            End Get
            Set(ByVal value As Int32)
                iNotificationID = value
            End Set
        End Property

        Public Property Text() As String
            Get
                Text = sText
            End Get
            Set(ByVal value As String)
                sText = value
            End Set
        End Property

        Public Property Active() As Boolean
            Get
                Active = bActive
            End Get
            Set(ByVal value As Boolean)
                bActive = value
            End Set
        End Property

        Public Property CreatedByID() As Int32
            Get
                CreatedByID = iCreatedByID
            End Get
            Set(ByVal value As Int32)
                iCreatedByID = value
            End Set
        End Property

        Public Property CreatedByDate() As DateTime
            Get
                CreatedByDate = dCreatedByDate
            End Get
            Set(ByVal value As DateTime)
                dCreatedByDate = value
            End Set
        End Property

        Public Property UpdatedByID() As Int32
            Get
                UpdatedByID = iUpdatedByID
            End Get
            Set(ByVal value As Int32)
                iUpdatedByID = value
            End Set
        End Property

        Public Property UpdatedByDate() As DateTime
            Get
                UpdatedByDate = dUpdatedByDate
            End Get
            Set(ByVal value As DateTime)
                dUpdatedByDate = value
            End Set
        End Property

#End Region

        Public Sub Populate(ByRef db As BMPXDB)

            Try

                objdb = db
                Populate()
                objdb = Nothing

            Catch ex As Exception
                Throw New Exception("NotificationDto.Populate(ByVal ID As Integer, ByRef db As BMPXDB) Error:  " & vbCrLf & ex.Message)
            End Try
        End Sub
        Public Sub Populate()

            Dim bDBIsNothing As Boolean = objdb Is Nothing
            If bDBIsNothing Then objdb = New BMPXDB

            Try

                'Dim ds As DataSet = objdb.SelectByIDNotification(ID)
                Dim ds As DataSet = objdb.SelectActiveNotification()

                If ds.Tables(0).Rows.Count > 0 Then
                    Dim dr As DataRow = ds.Tables(0).Rows(0)
                    Me.NotificationID = dr("NotificationID")
                    Me.Text = dr("Text")
                    Me.CreatedByID = dr("CreatedByID")
                    Me.CreatedByDate = dr("CreatedByDate")
                    Me.UpdatedByID = dr("UpdatedByID")
                    Me.UpdatedByDate = dr("UpdatedByDate")
                End If
            Catch ex As Exception
                Throw New Exception("Error NotificationDTO.Populate: " & vbCrLf & ex.Message)
            Finally
                If bDBIsNothing Then objdb = Nothing
            End Try
        End Sub

        Public Function Save(Optional ByVal db As BMPXDB = Nothing) As Boolean

            Dim bDBIsNothing As Boolean = db Is Nothing
            If bDBIsNothing Then db = New BMPXDB
            Dim retval As Boolean = False

            Try

                retval = db.SaveNotification(Me)
                Me.NotificationID = db.Identity

            Catch ex As Exception
                Throw New Exception("NotificationDto.Save: " & ex.Message & vbCrLf)

            Finally
                If bDBIsNothing Then db = Nothing
            End Try

            Return retval

        End Function

    End Class

#End Region





End Class
