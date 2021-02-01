Public Class CertDto

#Region "ItemsDto"

    Public Class ItemsDto

#Region "Properties"

        Private objdb As CertDB

        Private iItemID As Int32 = 0
        Private iCertificationTypeID As Int32
        Private iItemTypeID As Int32
        Private sName As String
        Private sHeading As String
        Private sItemHeading As String
        Private sVerbiage As String
        Private sVideoFile As String
        Private sImageFile As String
        Private iParentID As Int32
        Private iDisplaySequence As Int32
        Private bActive As Boolean
        Private iCreatedByID As Int32
        Private dCreatedByDate As DateTime
        Private iLastModifiedID As Int32
        Private dLastModifiedDate As DateTime
        Private sLastModifiedNetworkID As String
        Private sImageCaption As Object
        Private sVideoCaption As Object
        Private iNumberOfQuestions As Int32

        Public Property ItemID() As Int32
            Get
                ItemID = iItemID
            End Get
            Set(ByVal value As Int32)
                iItemID = value
            End Set
        End Property

        Public Property CertificationTypeID() As Int32
            Get
                CertificationTypeID = iCertificationTypeID
            End Get
            Set(ByVal value As Int32)
                iCertificationTypeID = value
            End Set
        End Property

        Public Property ItemTypeID() As Int32
            Get
                ItemTypeID = iItemTypeID
            End Get
            Set(ByVal value As Int32)
                iItemTypeID = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Name = sName
            End Get
            Set(ByVal value As String)
                sName = value
            End Set
        End Property

        Public Property Heading() As String
            Get
                Heading = sHeading
            End Get
            Set(ByVal value As String)
                sHeading = value
            End Set
        End Property

        Public Property ItemHeading() As String
            Get
                ItemHeading = sItemHeading
            End Get
            Set(ByVal value As String)
                sItemHeading = value
            End Set
        End Property

        Public Property Verbiage() As String
            Get
                Verbiage = sVerbiage
            End Get
            Set(ByVal value As String)
                sVerbiage = value
            End Set
        End Property

        Public Property VideoFile() As String
            Get
                VideoFile = sVideoFile
            End Get
            Set(ByVal value As String)
                sVideoFile = value
            End Set
        End Property

        Public Property ImageFile() As String
            Get
                ImageFile = sImageFile
            End Get
            Set(ByVal value As String)
                sImageFile = value
            End Set
        End Property

        Public Property ParentID() As Int32
            Get
                ParentID = iParentID
            End Get
            Set(ByVal value As Int32)
                iParentID = value
            End Set
        End Property

        Public Property DisplaySequence() As Int32
            Get
                DisplaySequence = iDisplaySequence
            End Get
            Set(ByVal value As Int32)
                iDisplaySequence = value
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

        Public Property LastModifiedID() As Int32
            Get
                LastModifiedID = iLastModifiedID
            End Get
            Set(ByVal value As Int32)
                iLastModifiedID = value
            End Set
        End Property

        Public Property LastModifiedDate() As DateTime
            Get
                LastModifiedDate = dLastModifiedDate
            End Get
            Set(ByVal value As DateTime)
                dLastModifiedDate = value
            End Set
        End Property

        Public Property LastModifiedNetworkID() As String
            Get
                LastModifiedNetworkID = sLastModifiedNetworkID
            End Get
            Set(ByVal value As String)
                sLastModifiedNetworkID = value
            End Set
        End Property

        Public Property ImageCaption() As Object
            Get
                If sImageCaption Is Nothing Then
                    ImageCaption = DBNull.Value
                Else
                    ImageCaption = sImageCaption
                End If
            End Get
            Set(ByVal value As Object)
                sImageCaption = value.ToString
            End Set
        End Property

        Public Property VideoCaption() As Object
            Get
                If sVideoCaption Is Nothing Then
                    VideoCaption = DBNull.Value
                Else
                    VideoCaption = sVideoCaption
                End If
            End Get
            Set(ByVal value As Object)
                sVideoCaption = value.ToString
            End Set
        End Property

        Public Property NumberOfQuestions() As Int32
            Get
                NumberOfQuestions = iNumberOfQuestions
            End Get
            Set(ByVal value As Int32)
                iNumberOfQuestions = value
            End Set
        End Property

#End Region

        Public Sub Populate(ByRef ID As Integer, Optional ByRef db As CertDB = Nothing)

            Dim bDBIsNothing As Boolean = db Is Nothing

            Try
                If bDBIsNothing Then db = New CertDB

                Dim ds As DataSet = db.SelectByIDItems(ID)

                If ds.Tables(0).Rows.Count > 0 Then
                    Dim dr As DataRow = ds.Tables(0).Rows(0)
                    Me.ItemID = dr("ItemID")
                    Me.CertificationTypeID = dr("CertificationTypeID")
                    Me.ItemTypeID = dr("ItemTypeID")
                    Me.Name = dr("Name")
                    Me.Heading = dr("Heading")
                    Me.ItemHeading = dr("ItemHeading")
                    Me.Verbiage = dr("Verbiage")
                    Me.VideoFile = dr("VideoFile")
                    Me.ImageFile = dr("ImageFile")
                    Me.ParentID = dr("ParentID")
                    Me.DisplaySequence = dr("DisplaySequence")
                    Me.Active = dr("Active")
                    Me.CreatedByID = dr("CreatedByID")
                    Me.CreatedByDate = dr("CreatedByDate")
                    Me.LastModifiedID = dr("LastModifiedID")
                    Me.LastModifiedDate = dr("LastModifiedDate")
                    Me.LastModifiedNetworkID = dr("LastModifiedNetworkID")
                    Me.ImageCaption = dr("ImageCaption")
                    Me.VideoCaption = dr("VideoCaption")
                    Me.NumberOfQuestions = dr("NumberOfQuestions")
                End If
            Catch ex As Exception

                If Not db Is Nothing Then db.Dispose()
                db = Nothing


                Dim sInnerException As String = ""
                If Not ex.InnerException Is Nothing Then
                    sInnerException = sInnerException
                End If

                Throw New Exception("Error ItemsDTO.Populate: " & vbCrLf & ex.Message & sInnerException)
            Finally

                If bDBIsNothing Then
                    If Not db Is Nothing Then db.Dispose()
                    db = Nothing
                End If

            End Try
        End Sub

        Public Function Save(Optional ByVal db As CertDB = Nothing) As Boolean

            Dim bDBIsNothing As Boolean = db Is Nothing
            Dim retval As Boolean = False

            Try
                If bDBIsNothing Then db = New CertDB

                retval = db.SaveItems(Me)
                Me.ItemID = db.Identity

            Catch ex As Exception

                If Not db Is Nothing Then db.Dispose()
                db = Nothing

                Throw New Exception("ItemsDto.Save: " & ex.Message & vbCrLf)

            Finally

                If bDBIsNothing Then
                    If Not db Is Nothing Then db.Dispose()
                    db = Nothing
                End If

            End Try

            Return retval

        End Function

    End Class

#End Region

#Region "QuestionsDto"

    Public Class QuestionsDto

#Region "Properties"
        Private iQuestionsID As Integer = 0
        Private iCertificationTypeID As Integer
        Private iItemID As Integer
        Private sQuestion As String
        Private sAnswer1 As String
        Private sAnswer2 As String
        Private sAnswer3 As String
        Private sAnswer4 As String
        Private sAnswer5 As String
        Private sCorrectAnswer As String
        Private iQuestionWeight As Integer
        Private bActive As Boolean
        Private iDisplaySequence As Integer
        Private iCreatedByID As Integer
        Private dCreatedByDate As DateTime
        Private iLastModifiedID As Integer
        Private dLastModifiedDate As DateTime
        Private sLastModifiedNetworkID As String

        Public Property QuestionsID() As Integer
            Get
                QuestionsID = iQuestionsID
            End Get
            Set(ByVal value As Integer)
                iQuestionsID = value
            End Set
        End Property

        Public Property CertificationTypeID() As Integer
            Get
                CertificationTypeID = iCertificationTypeID
            End Get
            Set(ByVal value As Integer)
                iCertificationTypeID = value
            End Set
        End Property

        Public Property ItemID() As Integer
            Get
                ItemID = iItemID
            End Get
            Set(ByVal value As Integer)
                iItemID = value
            End Set
        End Property

        Public Property Question() As String
            Get
                Question = sQuestion
            End Get
            Set(ByVal value As String)
                sQuestion = value
            End Set
        End Property

        Public Property Answer1() As String
            Get
                Answer1 = sAnswer1
            End Get
            Set(ByVal value As String)
                sAnswer1 = value
            End Set
        End Property

        Public Property Answer2() As String
            Get
                Answer2 = sAnswer2
            End Get
            Set(ByVal value As String)
                sAnswer2 = value
            End Set
        End Property

        Public Property Answer3() As String
            Get
                Answer3 = sAnswer3
            End Get
            Set(ByVal value As String)
                sAnswer3 = value
            End Set
        End Property

        Public Property Answer4() As String
            Get
                Answer4 = sAnswer4
            End Get
            Set(ByVal value As String)
                sAnswer4 = value
            End Set
        End Property

        Public Property Answer5() As String
            Get
                Answer5 = sAnswer5
            End Get
            Set(ByVal value As String)
                sAnswer5 = value
            End Set
        End Property

        Public Property CorrectAnswer() As String
            Get
                CorrectAnswer = sCorrectAnswer
            End Get
            Set(ByVal value As String)
                sCorrectAnswer = value
            End Set
        End Property

        Public Property QuestionWeight() As Integer
            Get
                QuestionWeight = iQuestionWeight
            End Get
            Set(ByVal value As Integer)
                iQuestionWeight = value
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

        Public Property DisplaySequence() As Integer
            Get
                DisplaySequence = iDisplaySequence
            End Get
            Set(ByVal value As Integer)
                iDisplaySequence = value
            End Set
        End Property

        Public Property CreatedByID() As Integer
            Get
                CreatedByID = iCreatedByID
            End Get
            Set(ByVal value As Integer)
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

        Public Property LastModifiedID() As Integer
            Get
                LastModifiedID = iLastModifiedID
            End Get
            Set(ByVal value As Integer)
                iLastModifiedID = value
            End Set
        End Property

        Public Property LastModifiedDate() As DateTime
            Get
                LastModifiedDate = dLastModifiedDate
            End Get
            Set(ByVal value As DateTime)
                dLastModifiedDate = value
            End Set
        End Property

        Public Property LastModifiedNetworkID() As String
            Get
                LastModifiedNetworkID = sLastModifiedNetworkID
            End Get
            Set(ByVal value As String)
                sLastModifiedNetworkID = value
            End Set
        End Property

#End Region

        Public Sub Populate(ByVal ID As Integer, Optional ByVal db As CertDB = Nothing)

            Dim bDBIsNothing As Boolean = db Is Nothing

            Try

                If bDBIsNothing Then db = New CertDB

                Dim ds As DataSet = db.SelectByIDQuestions(ID)

                If ds.Tables(0).Rows.Count > 0 Then
                    Dim dr As DataRow = ds.Tables(0).Rows(0)
                    Me.QuestionsID = dr("QuestionsID")
                    Me.CertificationTypeID = dr("CertificationTypeID")
                    Me.ItemID = dr("ItemID")
                    Me.Question = dr("Question")
                    Me.Answer1 = dr("Answer1")
                    Me.Answer2 = dr("Answer2")
                    Me.Answer3 = dr("Answer3")
                    Me.Answer4 = dr("Answer4")
                    Me.Answer5 = dr("Answer5")
                    Me.CorrectAnswer = dr("CorrectAnswer")
                    Me.QuestionWeight = dr("QuestionWeight")
                    Me.Active = dr("Active")
                    Me.DisplaySequence = dr("DisplaySequence")
                    Me.CreatedByID = dr("CreatedByID")
                    Me.CreatedByDate = dr("CreatedByDate")
                    Me.LastModifiedID = dr("LastModifiedID")
                    Me.LastModifiedDate = dr("LastModifiedDate")
                    Me.LastModifiedNetworkID = dr("LastModifiedNetworkID")

                End If
            Catch ex As Exception

                If Not db Is Nothing Then db.Dispose()
                db = Nothing


                Dim sInnerException As String = ""
                If Not ex.InnerException Is Nothing Then
                    sInnerException = sInnerException
                End If

                Throw New Exception("Error QuestionsDTO.Populate: " & vbCrLf & ex.Message & sInnerException)
            Finally

                If bDBIsNothing Then
                    If Not db Is Nothing Then db.Dispose()
                    db = Nothing
                End If

            End Try
        End Sub

        Public Function Save(Optional ByVal db As CertDB = Nothing) As Boolean

            Dim bDBIsNothing As Boolean = db Is Nothing
            Dim retval As Boolean = False

            Try

                If bDBIsNothing Then db = New CertDB

                retval = db.SaveQuestions(Me)
                Me.QuestionsID = db.Identity

            Catch ex As Exception

                If Not db Is Nothing Then db.Dispose()
                db = Nothing

                Throw New Exception("QuestionsDto.Save: " & ex.Message & vbCrLf)

            Finally

                If bDBIsNothing Then
                    If Not db Is Nothing Then db.Dispose()
                    db = Nothing
                End If

            End Try

            Return retval

        End Function

    End Class

#End Region

#Region "CertificationTypeDto"

    Public Class CertificationTypeDto

#Region "Properties"

        Private objdb As CertDB

        Private iCertificationTypeID As Int32 = 0
        Private sCertificationName As String
        Private bActive As Boolean
        Private iDisplaySequence As Int32
        Private bPublish As Boolean
        Private iPassingScore As Int32
        Private iLengthOfCertification As Int32
        Private sPassingVerbiage As String
        Private sFailingVerbiage As String
        Private iCreatedByID As Int32
        Private dCreatedByDate As DateTime
        Private iLastModifiedID As Int32
        Private dLastModifiedDate As DateTime
        Private sLastModifiedNetworkID As String

        Public Property CertificationTypeID() As Int32
            Get
                CertificationTypeID = iCertificationTypeID
            End Get
            Set(ByVal value As Int32)
                iCertificationTypeID = value
            End Set
        End Property

        Public Property CertificationName() As String
            Get
                CertificationName = sCertificationName
            End Get
            Set(ByVal value As String)
                sCertificationName = value
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

        Public Property DisplaySequence() As Int32
            Get
                DisplaySequence = iDisplaySequence
            End Get
            Set(ByVal value As Int32)
                iDisplaySequence = value
            End Set
        End Property

        Public Property Publish() As Boolean
            Get
                Publish = bPublish
            End Get
            Set(ByVal value As Boolean)
                bPublish = value
            End Set
        End Property

        Public Property PassingScore() As Int32
            Get
                PassingScore = iPassingScore
            End Get
            Set(ByVal value As Int32)
                iPassingScore = value
            End Set
        End Property

        Public Property LengthOfCertification() As Int32
            Get
                LengthOfCertification = iLengthOfCertification
            End Get
            Set(ByVal value As Int32)
                iLengthOfCertification = value
            End Set
        End Property

        Public Property PassingVerbiage() As String
            Get
                PassingVerbiage = sPassingVerbiage
            End Get
            Set(ByVal value As String)
                sPassingVerbiage = value
            End Set
        End Property

        Public Property FailingVerbiage() As String
            Get
                FailingVerbiage = sFailingVerbiage
            End Get
            Set(ByVal value As String)
                sFailingVerbiage = value
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

        Public Property LastModifiedID() As Int32
            Get
                LastModifiedID = iLastModifiedID
            End Get
            Set(ByVal value As Int32)
                iLastModifiedID = value
            End Set
        End Property

        Public Property LastModifiedDate() As DateTime
            Get
                LastModifiedDate = dLastModifiedDate
            End Get
            Set(ByVal value As DateTime)
                dLastModifiedDate = value
            End Set
        End Property

        Public Property LastModifiedNetworkID() As String
            Get
                LastModifiedNetworkID = sLastModifiedNetworkID
            End Get
            Set(ByVal value As String)
                sLastModifiedNetworkID = value
            End Set
        End Property

#End Region

        Public Sub Populate(ByRef ID As Integer, Optional ByRef db As CertDB = Nothing)

            Dim bDBIsNothing As Boolean = db Is Nothing

            Try

                If bDBIsNothing Then db = New CertDB

                Dim ds As DataSet = db.SelectByIDCertificationType(ID)

                If ds.Tables(0).Rows.Count > 0 Then
                    Dim dr As DataRow = ds.Tables(0).Rows(0)
                    Me.CertificationTypeID = dr("CertificationTypeID")
                    Me.CertificationName = dr("CertificationName")
                    Me.Active = dr("Active")
                    Me.DisplaySequence = dr("DisplaySequence")
                    Me.Publish = dr("Publish")
                    Me.PassingScore = dr("PassingScore")
                    Me.LengthOfCertification = dr("LengthOfCertification")
                    Me.PassingVerbiage = dr("PassingVerbiage")
                    Me.FailingVerbiage = dr("FailingVerbiage")
                    Me.CreatedByID = dr("CreatedByID")
                    Me.CreatedByDate = dr("CreatedByDate")
                    Me.LastModifiedID = dr("LastModifiedID")
                    Me.LastModifiedDate = dr("LastModifiedDate")
                    Me.LastModifiedNetworkID = dr("LastModifiedNetworkID")
                End If
            Catch ex As Exception

                If Not db Is Nothing Then db.Dispose()
                db = Nothing


                Dim sInnerException As String = ""
                If Not ex.InnerException Is Nothing Then
                    sInnerException = sInnerException
                End If

                Throw New Exception("Error CertificationTypeDTO.Populate: " & vbCrLf & ex.Message & sInnerException)
            Finally

                If bDBIsNothing Then
                    If Not db Is Nothing Then db.Dispose()
                    db = Nothing
                End If

            End Try
        End Sub

        Public Function Save(Optional ByVal db As CertDB = Nothing) As Boolean

            Dim bDBIsNothing As Boolean = db Is Nothing
            Dim retval As Boolean = False

            Try
                If bDBIsNothing Then db = New CertDB

                retval = db.SaveCertificationType(Me)
                Me.CertificationTypeID = db.Identity

            Catch ex As Exception

                If Not db Is Nothing Then db.Dispose()
                db = Nothing

                Throw New Exception("CertificationTypeDto.Save: " & ex.Message & vbCrLf)

            Finally

                If bDBIsNothing Then
                    If Not db Is Nothing Then db.Dispose()
                    db = Nothing
                End If

            End Try

            Return retval

        End Function

    End Class

#End Region

    '#Region "PublicUserRegistrationDto"

    '    Public Class PublicUserRegistrationDto

    '#Region "Properties"

    '        Private objdb As CertDB

    '        Private iPublicUserRegistrationID As Int32 = 0
    '        Private sFirstName As String
    '        Private sMiddleName As String
    '        Private sLastName As String
    '        Private sEmailAddress As String
    '        Private bPassword As Byte()
    '        Private bTempPassword As Byte()
    '        Private sWorkPhone As String
    '        Private sMobilePhone As String
    '        Private sHomePhone As String
    '        Private sFax As String
    '        Private sCompanyName As String
    '        Private sJobResponsibility As String
    '        Private sDriverLicence As String
    '        Private sStreet As String
    '        Private sCity As String
    '        Private sState As String
    '        Private sZip As String
    '        Private sCounty As String
    '        Private bUseCompressedVideos As Boolean = False
    '        Private sRegisteredBy As String
    '        Private dRegisteredAt As DateTime
    '        Private sRegisteredFrom As String
    '        Private sUpdatedBy As String
    '        Private dUpdatedAt As DateTime
    '        Private sUpdatedFrom As String

    '        Public Property PublicUserRegistrationID() As Int32
    '            Get
    '                PublicUserRegistrationID = iPublicUserRegistrationID
    '            End Get
    '            Set(ByVal value As Int32)
    '                iPublicUserRegistrationID = value
    '            End Set
    '        End Property

    '        Public Property FirstName() As String
    '            Get
    '                FirstName = sFirstName
    '            End Get
    '            Set(ByVal value As String)
    '                sFirstName = value
    '            End Set
    '        End Property

    '        Public Property MiddleName() As String
    '            Get
    '                MiddleName = sMiddleName
    '            End Get
    '            Set(ByVal value As String)
    '                sMiddleName = value
    '            End Set
    '        End Property

    '        Public Property LastName() As String
    '            Get
    '                LastName = sLastName
    '            End Get
    '            Set(ByVal value As String)
    '                sLastName = value
    '            End Set
    '        End Property

    '        Public Property EmailAddress() As String
    '            Get
    '                EmailAddress = sEmailAddress
    '            End Get
    '            Set(ByVal value As String)
    '                sEmailAddress = value
    '            End Set
    '        End Property

    '        Public Property Password() As Byte()
    '            Get
    '                Password = bPassword
    '            End Get
    '            Set(ByVal value As Byte())
    '                bPassword = value
    '            End Set
    '        End Property

    '        Public Property TempPassword() As Byte()
    '            Get
    '                TempPassword = bTempPassword
    '            End Get
    '            Set(ByVal value As Byte())
    '                bTempPassword = value
    '            End Set
    '        End Property

    '        Public Property WorkPhone() As String
    '            Get
    '                WorkPhone = sWorkPhone
    '            End Get
    '            Set(ByVal value As String)
    '                sWorkPhone = value
    '            End Set
    '        End Property

    '        Public Property MobilePhone() As String
    '            Get
    '                MobilePhone = sMobilePhone
    '            End Get
    '            Set(ByVal value As String)
    '                sMobilePhone = value
    '            End Set
    '        End Property

    '        Public Property HomePhone() As String
    '            Get
    '                HomePhone = sHomePhone
    '            End Get
    '            Set(ByVal value As String)
    '                sHomePhone = value
    '            End Set
    '        End Property

    '        Public Property Fax() As String
    '            Get
    '                Fax = sFax
    '            End Get
    '            Set(ByVal value As String)
    '                sFax = value
    '            End Set
    '        End Property

    '        Public Property CompanyName() As String
    '            Get
    '                CompanyName = sCompanyName
    '            End Get
    '            Set(ByVal value As String)
    '                sCompanyName = value
    '            End Set
    '        End Property

    '        Public Property JobResponsibility() As String
    '            Get
    '                JobResponsibility = sJobResponsibility
    '            End Get
    '            Set(ByVal value As String)
    '                sJobResponsibility = value
    '            End Set
    '        End Property

    '        Public Property DriverLicence() As String
    '            Get
    '                DriverLicence = sDriverLicence
    '            End Get
    '            Set(ByVal value As String)
    '                sDriverLicence = value
    '            End Set
    '        End Property

    '        Public Property Street() As String
    '            Get
    '                Street = sStreet
    '            End Get
    '            Set(ByVal value As String)
    '                sStreet = value
    '            End Set
    '        End Property

    '        Public Property City() As String
    '            Get
    '                City = sCity
    '            End Get
    '            Set(ByVal value As String)
    '                sCity = value
    '            End Set
    '        End Property

    '        Public Property State() As String
    '            Get
    '                State = sState
    '            End Get
    '            Set(ByVal value As String)
    '                sState = value
    '            End Set
    '        End Property

    '        Public Property Zip() As String
    '            Get
    '                Zip = sZip
    '            End Get
    '            Set(ByVal value As String)
    '                sZip = value
    '            End Set
    '        End Property

    '        Public Property County() As String
    '            Get
    '                County = sCounty
    '            End Get
    '            Set(ByVal value As String)
    '                sCounty = value
    '            End Set
    '        End Property

    '        Public Property UseCompressedVideos() As Boolean
    '            Get
    '                UseCompressedVideos = bUseCompressedVideos
    '            End Get
    '            Set(ByVal value As Boolean)
    '                bUseCompressedVideos = value
    '            End Set
    '        End Property

    '        Public Property RegisteredBy() As String
    '            Get
    '                RegisteredBy = sRegisteredBy
    '            End Get
    '            Set(ByVal value As String)
    '                sRegisteredBy = value
    '            End Set
    '        End Property

    '        Public Property RegisteredAt() As DateTime
    '            Get
    '                RegisteredAt = dRegisteredAt
    '            End Get
    '            Set(ByVal value As DateTime)
    '                dRegisteredAt = value
    '            End Set
    '        End Property

    '        Public Property RegisteredFrom() As String
    '            Get
    '                RegisteredFrom = sRegisteredFrom
    '            End Get
    '            Set(ByVal value As String)
    '                sRegisteredFrom = value
    '            End Set
    '        End Property

    '        Public Property UpdatedBy() As String
    '            Get
    '                UpdatedBy = sUpdatedBy
    '            End Get
    '            Set(ByVal value As String)
    '                sUpdatedBy = value
    '            End Set
    '        End Property

    '        Public Property UpdatedAt() As DateTime
    '            Get
    '                UpdatedAt = dUpdatedAt
    '            End Get
    '            Set(ByVal value As DateTime)
    '                dUpdatedAt = value
    '            End Set
    '        End Property

    '        Public Property UpdatedFrom() As String
    '            Get
    '                UpdatedFrom = sUpdatedFrom
    '            End Get
    '            Set(ByVal value As String)
    '                sUpdatedFrom = value
    '            End Set
    '        End Property

    '#End Region

    '        Public Sub Populate(ByRef ID As Integer, ByRef db As CertDB)

    '            Try

    '                objdb = db
    '                Populate(ID)
    '                objdb = Nothing

    '            Catch ex As Exception

    '                Dim sInnerException As String = ""
    '                If Not ex.InnerException Is Nothing Then
    '                    sInnerException = sInnerException
    '                End If

    '                Throw New Exception("PublicUserRegistrationDto.Populate(ByVal ID As Integer, ByRef db As CertDB) Error:   ID = " & ID.ToString & vbCrLf & ex.Message & sInnerException)
    '            End Try
    '        End Sub

    '        Public Sub Populate(ByRef ID As Integer)

    '            Dim bDBIsNothing As Boolean = objdb Is Nothing

    '            Try
    '                If bDBIsNothing Then objdb = New CertDB

    '                Dim ds As DataSet = objdb.SelectByIDPublicUserRegistration(ID)

    '                If ds.Tables(0).Rows.Count > 0 Then
    '                    Dim dr As DataRow = ds.Tables(0).Rows(0)
    '                    Me.PublicUserRegistrationID = dr("PublicUserRegistrationID")
    '                    Me.FirstName = dr("FirstName")
    '                    Me.MiddleName = dr("MiddleName")
    '                    Me.LastName = dr("LastName")
    '                    Me.EmailAddress = dr("EmailAddress")
    '                    Me.Password = dr("Password")
    '                    Me.TempPassword = dr("TempPassword")
    '                    Me.WorkPhone = dr("WorkPhone")
    '                    Me.MobilePhone = dr("MobilePhone")
    '                    Me.HomePhone = dr("HomePhone")
    '                    Me.Fax = dr("Fax")
    '                    Me.CompanyName = dr("CompanyName")
    '                    Me.JobResponsibility = dr("JobResponsibility")
    '                    Me.DriverLicence = dr("DriverLicence")
    '                    Me.Street = dr("Street")
    '                    Me.City = dr("City")
    '                    Me.State = dr("State")
    '                    Me.Zip = dr("Zip")
    '                    Me.County = dr("County")
    '                    Me.UseCompressedVideos = dr("UseCompressedVideos")
    '                    Me.RegisteredBy = dr("RegisteredBy")
    '                    Me.RegisteredAt = dr("RegisteredAt")
    '                    Me.RegisteredFrom = dr("RegisteredFrom")
    '                    Me.UpdatedBy = dr("UpdatedBy")
    '                    Me.UpdatedAt = dr("UpdatedAt")
    '                    Me.UpdatedFrom = dr("UpdatedFrom")
    '                End If
    '            Catch ex As Exception

    '                If Not objdb Is Nothing Then objdb.Dispose()
    '                objdb = Nothing


    '                Dim sInnerException As String = ""
    '                If Not ex.InnerException Is Nothing Then
    '                    sInnerException = sInnerException
    '                End If

    '                Throw New Exception("Error PublicUserRegistrationDTO.Populate: " & vbCrLf & ex.Message & sInnerException)
    '            Finally
    '                If bDBIsNothing Then
    '                    objdb.Dispose()
    '                    objdb = Nothing
    '                End If
    '            End Try
    '        End Sub

    '        Public Function Save(Optional ByVal db As CertDB = Nothing) As Boolean

    '            Dim bDBIsNothing As Boolean = db Is Nothing
    '            Dim retval As Boolean = False

    '            Try
    '                If bDBIsNothing Then db = New CertDB


    '                retval = db.SavePublicUserRegistration(Me)
    '                Me.PublicUserRegistrationID = db.Identity

    '            Catch ex As Exception

    '                If Not db Is Nothing Then db.Dispose()
    '                db = Nothing

    '                Throw New Exception("PublicUserRegistrationDto.Save: " & ex.Message & vbCrLf)

    '            Finally

    '                If bDBIsNothing Then
    '                    If Not db Is Nothing Then db.Dispose()
    '                    db = Nothing
    '                End If

    '            End Try

    '            Return retval

    '        End Function

    '    End Class
    '#End Region

#Region "UserItemsDto"

    Public Class UserItemsDto

#Region "Properties"

        Private objdb As CertDB

        Private iUserItemsID As Int32 = 0
        Private iUserID As Int32
        Private iItemID As Int32
        Private iCertificationTypeID As Int32
        Private iItemTypeID As Int32
        Private sName As String
        Private sHeading As String
        Private sItemHeading As String
        Private sVerbiage As String
        Private sVideoFile As String
        Private sImageFile As String
        Private iParentID As Int32
        Private iDisplaySequence As Int32
        Private bItemReviewed As Boolean
        Private bActive As Boolean
        Private iUserSummaryID As Int32
        Private iCreatedByID As Int32
        Private dCreatedByDate As DateTime
        Private iLastModifiedID As Int32
        Private dLastModifiedDate As DateTime
        Private sLastModifiedNetworkID As String
        Private sImageCaption As String
        Private sVideoCaption As String
        Private iNumberOfQuestions As Int32

        Public Property UserItemsID() As Int32
            Get
                UserItemsID = iUserItemsID
            End Get
            Set(ByVal value As Int32)
                iUserItemsID = value
            End Set
        End Property

        Public Property UserID() As Int32
            Get
                UserID = iUserID
            End Get
            Set(ByVal value As Int32)
                iUserID = value
            End Set
        End Property

        Public Property ItemID() As Int32
            Get
                ItemID = iItemID
            End Get
            Set(ByVal value As Int32)
                iItemID = value
            End Set
        End Property

        Public Property CertificationTypeID() As Int32
            Get
                CertificationTypeID = iCertificationTypeID
            End Get
            Set(ByVal value As Int32)
                iCertificationTypeID = value
            End Set
        End Property

        Public Property ItemTypeID() As Int32
            Get
                ItemTypeID = iItemTypeID
            End Get
            Set(ByVal value As Int32)
                iItemTypeID = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Name = sName
            End Get
            Set(ByVal value As String)
                sName = value
            End Set
        End Property

        Public Property Heading() As String
            Get
                Heading = sHeading
            End Get
            Set(ByVal value As String)
                sHeading = value
            End Set
        End Property

        Public Property ItemHeading() As String
            Get
                ItemHeading = sItemHeading
            End Get
            Set(ByVal value As String)
                sItemHeading = value
            End Set
        End Property

        Public Property Verbiage() As String
            Get
                Verbiage = sVerbiage
            End Get
            Set(ByVal value As String)
                sVerbiage = value
            End Set
        End Property

        Public Property VideoFile() As String
            Get
                VideoFile = sVideoFile
            End Get
            Set(ByVal value As String)
                sVideoFile = value
            End Set
        End Property

        Public Property ImageFile() As String
            Get
                ImageFile = sImageFile
            End Get
            Set(ByVal value As String)
                sImageFile = value
            End Set
        End Property

        Public Property ParentID() As Int32
            Get
                ParentID = iParentID
            End Get
            Set(ByVal value As Int32)
                iParentID = value
            End Set
        End Property

        Public Property DisplaySequence() As Int32
            Get
                DisplaySequence = iDisplaySequence
            End Get
            Set(ByVal value As Int32)
                iDisplaySequence = value
            End Set
        End Property

        Public Property ItemReviewed() As Boolean
            Get
                ItemReviewed = bItemReviewed
            End Get
            Set(ByVal value As Boolean)
                bItemReviewed = value
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

        Public Property UserSummaryID() As Int32
            Get
                UserSummaryID = iUserSummaryID
            End Get
            Set(ByVal value As Int32)
                iUserSummaryID = value
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

        Public Property LastModifiedID() As Int32
            Get
                LastModifiedID = iLastModifiedID
            End Get
            Set(ByVal value As Int32)
                iLastModifiedID = value
            End Set
        End Property

        Public Property LastModifiedDate() As DateTime
            Get
                LastModifiedDate = dLastModifiedDate
            End Get
            Set(ByVal value As DateTime)
                dLastModifiedDate = value
            End Set
        End Property

        Public Property LastModifiedNetworkID() As String
            Get
                LastModifiedNetworkID = sLastModifiedNetworkID
            End Get
            Set(ByVal value As String)
                sLastModifiedNetworkID = value
            End Set
        End Property

        Public Property ImageCaption() As Object
            Get
                If sImageCaption Is Nothing Then
                    ImageCaption = DBNull.Value
                Else
                    ImageCaption = sImageCaption
                End If
            End Get
            Set(ByVal value As Object)
                sImageCaption = value.ToString
            End Set
        End Property

        Public Property VideoCaption() As Object
            Get
                If sVideoCaption Is Nothing Then
                    VideoCaption = DBNull.Value
                Else
                    VideoCaption = sVideoCaption
                End If
            End Get
            Set(ByVal value As Object)
                sVideoCaption = value.ToString
            End Set
        End Property

        Public Property NumberOfQuestions() As Int32
            Get
                NumberOfQuestions = iNumberOfQuestions
            End Get
            Set(ByVal value As Int32)
                iNumberOfQuestions = value
            End Set
        End Property

#End Region

        Public Sub Populate(ByRef ID As Integer, ByRef db As CertDB)
            Try
                objdb = db
                Populate(ID)
                objdb = Nothing
            Catch ex As Exception

                Dim sInnerException As String = ""
                If Not ex.InnerException Is Nothing Then
                    sInnerException = sInnerException
                End If

                Throw New Exception("UserItemsDto.Populate(ByVal ID As Integer, ByRef db As certDB) Error:   ID = " & ID.ToString & vbCrLf & ex.Message & sInnerException)
            End Try
        End Sub

        Public Sub Populate(ByRef ID As Integer)

            Dim bDBIsNothing As Boolean = objdb Is Nothing

            Try
                If bDBIsNothing Then objdb = New CertDB

                Dim ds As DataSet = objdb.SelectByIDUserItems(ID)

                If ds.Tables(0).Rows.Count > 0 Then
                    Dim dr As DataRow = ds.Tables(0).Rows(0)
                    Me.UserItemsID = dr("UserItemsID")
                    Me.UserID = dr("UserID")
                    Me.ItemID = dr("ItemID")
                    Me.CertificationTypeID = dr("CertificationTypeID")
                    Me.ItemTypeID = dr("ItemTypeID")
                    Me.Name = dr("Name")
                    Me.Heading = dr("Heading")
                    Me.ItemHeading = dr("ItemHeading")
                    Me.Verbiage = dr("Verbiage")
                    Me.VideoFile = dr("VideoFile")
                    Me.ImageFile = dr("ImageFile")
                    Me.ParentID = dr("ParentID")
                    Me.DisplaySequence = dr("DisplaySequence")
                    Me.ItemReviewed = dr("ItemReviewed")
                    Me.Active = dr("Active")
                    Me.UserSummaryID = dr("UserSummaryID")
                    Me.CreatedByID = dr("CreatedByID")
                    Me.CreatedByDate = dr("CreatedByDate")
                    Me.LastModifiedID = dr("LastModifiedID")
                    Me.LastModifiedDate = dr("LastModifiedDate")
                    Me.LastModifiedNetworkID = dr("LastModifiedNetworkID")
                    Me.ImageCaption = dr("ImageCaption")
                    Me.VideoCaption = dr("VideoCaption")
                    Me.NumberOfQuestions = dr("NumberOfQuestions")
                End If
            Catch ex As Exception

                If Not objdb Is Nothing Then objdb.Dispose()
                objdb = Nothing


                Dim sInnerException As String = ""
                If Not ex.InnerException Is Nothing Then
                    sInnerException = sInnerException
                End If

                Throw New Exception("Error UserItemsDTO.Populate: " & vbCrLf & ex.Message & sInnerException)
            Finally

                If bDBIsNothing Then
                    If Not objdb Is Nothing Then objdb.Dispose()
                    objdb = Nothing
                End If

            End Try
        End Sub

        Public Function Save(Optional ByVal db As CertDB = Nothing) As Boolean

            Dim bDBIsNothing As Boolean = db Is Nothing
            Dim retval As Boolean = False

            Try
                If bDBIsNothing Then db = New CertDB

                retval = db.SaveUserItems(Me)
                Me.UserItemsID = db.Identity

            Catch ex As Exception

                If Not db Is Nothing Then db.Dispose()
                db = Nothing

                Throw New Exception("UserItemsDto.Save: " & ex.Message & vbCrLf)

            Finally

                If bDBIsNothing Then
                    If Not db Is Nothing Then db.Dispose()
                    db = Nothing
                End If

            End Try

            Return retval

        End Function


        Protected Overrides Sub Finalize()
            MyBase.Finalize()

            If Not objdb Is Nothing Then objdb.Dispose()

        End Sub
    End Class

#End Region

#Region "UserSummaryDto"

    Public Class UserSummaryDto

#Region "Properties"

        Private objdb As CertDB

        Private iUserSummaryID As Int32 = 0
        Private iCertificationTypeID As Int32
        Private iUserID As Int32
        Private dExamScore As Object
        Private dStartDate As Object
        Private dEndDate As Object
        Private iNextModuleItemID As Int32
        Private bActive As Boolean
        Private dExamEndDate As Object
        Private iCreatedByID As Int32
        Private dCreatedByDate As DateTime
        Private iLastModifiedID As Int32
        Private dLastModifiedDate As DateTime
        Private sLastModifiedNetworkID As String

        Public Property UserSummaryID() As Int32
            Get
                UserSummaryID = iUserSummaryID
            End Get
            Set(ByVal value As Int32)
                iUserSummaryID = value
            End Set
        End Property

        Public Property CertificationTypeID() As Int32
            Get
                CertificationTypeID = iCertificationTypeID
            End Get
            Set(ByVal value As Int32)
                iCertificationTypeID = value
            End Set
        End Property

        Public Property UserID() As Int32
            Get
                UserID = iUserID
            End Get
            Set(ByVal value As Int32)
                iUserID = value
            End Set
        End Property

        Public Property ExamScore() As Object
            Get
                If dExamScore Is Nothing Then
                    ExamScore = DBNull.Value
                Else
                    ExamScore = dExamScore
                End If
            End Get
            Set(ByVal value As Object)
                Dim x As Double
                If Not value Is DBNull.Value And Not value Is Nothing Then
                    If Double.TryParse(value.ToString, x) Then
                        dExamScore = x
                    Else
                        Throw New Exception("ExamScore - Invalid data type.  '" & value.ToString & "' is not of type Double.")
                    End If
                Else
                    dExamScore = Nothing
                End If
            End Set
        End Property

        Public Property StartDate() As Object
            Get
                If dStartDate Is Nothing Then
                    StartDate = DBNull.Value
                Else
                    StartDate = dStartDate
                End If
            End Get
            Set(ByVal value As Object)
                Dim x As DateTime
                If Not value Is DBNull.Value And Not value Is Nothing Then
                    If DateTime.TryParse(value.ToString, x) Then
                        dStartDate = x
                    Else
                        Throw New Exception("StartDate - Invalid data type.  '" & value.ToString & "' is not of type DateTime.")
                    End If
                Else
                    dStartDate = Nothing
                End If
            End Set
        End Property

        Public Property EndDate() As Object
            Get
                If dEndDate Is Nothing Then
                    EndDate = DBNull.Value
                Else
                    EndDate = dEndDate
                End If
            End Get
            Set(ByVal value As Object)
                Dim x As DateTime
                If Not value Is DBNull.Value And Not value Is Nothing Then
                    If DateTime.TryParse(value.ToString, x) Then
                        dEndDate = x
                    Else
                        Throw New Exception("EndDate - Invalid data type.  '" & value.ToString & "' is not of type DateTime.")
                    End If
                Else
                    dEndDate = Nothing
                End If
            End Set
        End Property

        Public Property NextModuleItemID() As Int32
            Get
                NextModuleItemID = iNextModuleItemID
            End Get
            Set(ByVal value As Int32)
                iNextModuleItemID = value
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

        Public Property ExamEndDate() As Object
            Get
                If dExamEndDate Is Nothing Then
                    ExamEndDate = DBNull.Value
                Else
                    ExamEndDate = dExamEndDate
                End If
            End Get
            Set(ByVal value As Object)
                Dim x As DateTime
                If Not value Is DBNull.Value And Not value Is Nothing Then
                    If DateTime.TryParse(value.ToString, x) Then
                        dExamEndDate = x
                    Else
                        Throw New Exception("ExamEndDate - Invalid data type.  '" & value.ToString & "' is not of type DateTime.")
                    End If
                Else
                    dExamEndDate = Nothing
                End If
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

        Public Property LastModifiedID() As Int32
            Get
                LastModifiedID = iLastModifiedID
            End Get
            Set(ByVal value As Int32)
                iLastModifiedID = value
            End Set
        End Property

        Public Property LastModifiedDate() As DateTime
            Get
                LastModifiedDate = dLastModifiedDate
            End Get
            Set(ByVal value As DateTime)
                dLastModifiedDate = value
            End Set
        End Property

        Public Property LastModifiedNetworkID() As String
            Get
                LastModifiedNetworkID = sLastModifiedNetworkID
            End Get
            Set(ByVal value As String)
                sLastModifiedNetworkID = value
            End Set
        End Property

#End Region

        Public Sub Populate(ByRef ID As Integer, ByRef db As CertDB)
            Try
                objdb = db
                Populate(ID)
                objdb = Nothing
            Catch ex As Exception

                Dim sInnerException As String = ""
                If Not ex.InnerException Is Nothing Then
                    sInnerException = sInnerException
                End If

                Throw New Exception("UserSummaryDto.Populate(ByVal ID As Integer, ByRef db As certDB) Error:   ID = " & ID.ToString & vbCrLf & ex.Message & sInnerException)
            End Try
        End Sub

        Public Sub Populate(ByRef ID As Integer)
            Dim bDBIsNothing As Boolean = objdb Is Nothing

            Try
                If bDBIsNothing Then objdb = New CertDB

                Dim ds As DataSet = objdb.SelectByIDUserSummary(ID)

                If ds.Tables(0).Rows.Count > 0 Then
                    Dim dr As DataRow = ds.Tables(0).Rows(0)
                    Me.UserSummaryID = dr("UserSummaryID")
                    Me.CertificationTypeID = dr("CertificationTypeID")
                    Me.UserID = dr("UserID")
                    Me.ExamScore = dr("ExamScore")
                    Me.StartDate = dr("StartDate")
                    Me.EndDate = dr("EndDate")
                    Me.NextModuleItemID = dr("NextModuleItemID")
                    Me.Active = dr("Active")
                    Me.ExamEndDate = dr("ExamEndDate")
                    Me.CreatedByID = dr("CreatedByID")
                    Me.CreatedByDate = dr("CreatedByDate")
                    Me.LastModifiedID = dr("LastModifiedID")
                    Me.LastModifiedDate = dr("LastModifiedDate")
                    Me.LastModifiedNetworkID = dr("LastModifiedNetworkID")
                End If
            Catch ex As Exception

                If Not objdb Is Nothing Then objdb.Dispose()
                objdb = Nothing


                Dim sInnerException As String = ""
                If Not ex.InnerException Is Nothing Then
                    sInnerException = sInnerException
                End If

                Throw New Exception("Error UserSummaryDTO.Populate: " & vbCrLf & ex.Message & sInnerException)
            Finally


                If bDBIsNothing Then
                    If Not objdb Is Nothing Then objdb.Dispose()
                    objdb = Nothing
                End If

            End Try
        End Sub

        Public Function Save(Optional ByVal db As CertDB = Nothing) As Boolean

            Dim bDBIsNothing As Boolean = db Is Nothing
            Dim retval As Boolean = False

            Try
                If bDBIsNothing Then db = New CertDB

                retval = db.SaveUserSummary(Me)
                Me.UserSummaryID = db.Identity

            Catch ex As Exception

                If Not db Is Nothing Then db.Dispose()
                db = Nothing

                Throw New Exception("UserSummaryDto.Save: " & ex.Message & vbCrLf)

            Finally

                If bDBIsNothing Then
                    If Not db Is Nothing Then db.Dispose()
                    db = Nothing
                End If

            End Try

            Return retval

        End Function

    End Class

#End Region

#Region "UserQuestionsDto"

    Public Class UserQuestionsDto

#Region "Properties"

        Private objdb As CertDB

        Private iUserQuestionsID As Int32 = 0
        Private iUserID As Int32
        Private iQuestionsID As Int32
        Private iCertificationTypeID As Int32
        Private iItemID As Int32
        Private sQuestion As String
        Private sAnswer1 As String
        Private sAnswer2 As String
        Private sAnswer3 As String
        Private sAnswer4 As String
        Private sAnswer5 As String
        Private sCorrectAnswer As String
        Private iQuestionWeight As Int32
        Private iDisplaySequence As Int32
        Private sSelectedAnswer As String
        Private bAnswerScore As Boolean
        Private iNumberOfAnswers As Int32
        Private bActive As Boolean
        Private iUserSummaryID As Int32
        Private bFollowUpQuestionsCompleted As Boolean
        Private bIsFollowUpQuestion As Boolean
        Private iCreatedByID As Int32
        Private dCreatedByDate As DateTime
        Private iLastModifiedID As Int32
        Private dLastModifiedDate As DateTime
        Private sLastModifiedNetworkID As String

        Public Property UserQuestionsID() As Int32
            Get
                UserQuestionsID = iUserQuestionsID
            End Get
            Set(ByVal value As Int32)
                iUserQuestionsID = value
            End Set
        End Property

        Public Property UserID() As Int32
            Get
                UserID = iUserID
            End Get
            Set(ByVal value As Int32)
                iUserID = value
            End Set
        End Property

        Public Property QuestionsID() As Int32
            Get
                QuestionsID = iQuestionsID
            End Get
            Set(ByVal value As Int32)
                iQuestionsID = value
            End Set
        End Property

        Public Property CertificationTypeID() As Int32
            Get
                CertificationTypeID = iCertificationTypeID
            End Get
            Set(ByVal value As Int32)
                iCertificationTypeID = value
            End Set
        End Property

        Public Property ItemID() As Int32
            Get
                ItemID = iItemID
            End Get
            Set(ByVal value As Int32)
                iItemID = value
            End Set
        End Property

        Public Property Question() As String
            Get
                Question = sQuestion
            End Get
            Set(ByVal value As String)
                sQuestion = value
            End Set
        End Property

        Public Property Answer1() As String
            Get
                Answer1 = sAnswer1
            End Get
            Set(ByVal value As String)
                sAnswer1 = value
            End Set
        End Property

        Public Property Answer2() As String
            Get
                Answer2 = sAnswer2
            End Get
            Set(ByVal value As String)
                sAnswer2 = value
            End Set
        End Property

        Public Property Answer3() As String
            Get
                Answer3 = sAnswer3
            End Get
            Set(ByVal value As String)
                sAnswer3 = value
            End Set
        End Property

        Public Property Answer4() As String
            Get
                Answer4 = sAnswer4
            End Get
            Set(ByVal value As String)
                sAnswer4 = value
            End Set
        End Property

        Public Property Answer5() As String
            Get
                Answer5 = sAnswer5
            End Get
            Set(ByVal value As String)
                sAnswer5 = value
            End Set
        End Property

        Public Property CorrectAnswer() As String
            Get
                CorrectAnswer = sCorrectAnswer
            End Get
            Set(ByVal value As String)
                sCorrectAnswer = value
            End Set
        End Property

        Public Property QuestionWeight() As Int32
            Get
                QuestionWeight = iQuestionWeight
            End Get
            Set(ByVal value As Int32)
                iQuestionWeight = value
            End Set
        End Property

        Public Property DisplaySequence() As Int32
            Get
                DisplaySequence = iDisplaySequence
            End Get
            Set(ByVal value As Int32)
                iDisplaySequence = value
            End Set
        End Property

        Public Property SelectedAnswer() As String
            Get
                SelectedAnswer = sSelectedAnswer
            End Get
            Set(ByVal value As String)
                sSelectedAnswer = value
            End Set
        End Property

        Public Property AnswerScore() As Boolean
            Get
                AnswerScore = bAnswerScore
            End Get
            Set(ByVal value As Boolean)
                bAnswerScore = value
            End Set
        End Property

        Public Property NumberOfAnswers() As Int32
            Get
                NumberOfAnswers = iNumberOfAnswers
            End Get
            Set(ByVal value As Int32)
                iNumberOfAnswers = value
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

        Public Property UserSummaryID() As Int32
            Get
                UserSummaryID = iUserSummaryID
            End Get
            Set(ByVal value As Int32)
                iUserSummaryID = value
            End Set
        End Property

        Public Property FollowUpQuestionsCompleted() As Boolean
            Get
                FollowUpQuestionsCompleted = bFollowUpQuestionsCompleted
            End Get
            Set(ByVal value As Boolean)
                bFollowUpQuestionsCompleted = value
            End Set
        End Property

        Public Property IsFollowUpQuestion() As Boolean
            Get
                IsFollowUpQuestion = bIsFollowUpQuestion
            End Get
            Set(ByVal value As Boolean)
                bIsFollowUpQuestion = value
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

        Public Property LastModifiedID() As Int32
            Get
                LastModifiedID = iLastModifiedID
            End Get
            Set(ByVal value As Int32)
                iLastModifiedID = value
            End Set
        End Property

        Public Property LastModifiedDate() As DateTime
            Get
                LastModifiedDate = dLastModifiedDate
            End Get
            Set(ByVal value As DateTime)
                dLastModifiedDate = value
            End Set
        End Property

        Public Property LastModifiedNetworkID() As String
            Get
                LastModifiedNetworkID = sLastModifiedNetworkID
            End Get
            Set(ByVal value As String)
                sLastModifiedNetworkID = value
            End Set
        End Property

#End Region

        Public Sub Populate(ByRef ID As Integer, ByRef db As CertDB)
            Try
                objdb = db
                Populate(ID)
                objdb = Nothing
            Catch ex As Exception

                Dim sInnerException As String = ""
                If Not ex.InnerException Is Nothing Then
                    sInnerException = sInnerException
                End If

                Throw New Exception("UserQuestionsDto.Populate(ByVal ID As Integer, ByRef db As certDB) Error:   ID = " & ID.ToString & vbCrLf & ex.Message & sInnerException)
            End Try
        End Sub

        Public Sub Populate(ByRef ID As Integer)

            Dim bDBIsNothing As Boolean = objdb Is Nothing

            Try
                If bDBIsNothing Then objdb = New CertDB

                Dim ds As DataSet = objdb.SelectByIDUserQuestions(ID)

                If ds.Tables(0).Rows.Count > 0 Then
                    Dim dr As DataRow = ds.Tables(0).Rows(0)
                    Me.UserQuestionsID = dr("UserQuestionsID")
                    Me.UserID = dr("UserID")
                    Me.QuestionsID = dr("QuestionsID")
                    Me.CertificationTypeID = dr("CertificationTypeID")
                    Me.ItemID = dr("ItemID")
                    Me.Question = dr("Question")
                    Me.Answer1 = dr("Answer1")
                    Me.Answer2 = dr("Answer2")
                    Me.Answer3 = dr("Answer3")
                    Me.Answer4 = dr("Answer4")
                    Me.Answer5 = dr("Answer5")
                    Me.CorrectAnswer = dr("CorrectAnswer")
                    Me.QuestionWeight = dr("QuestionWeight")
                    Me.DisplaySequence = dr("DisplaySequence")
                    Me.SelectedAnswer = dr("SelectedAnswer")
                    Me.AnswerScore = dr("AnswerScore")
                    Me.NumberOfAnswers = dr("NumberOfAnswers")
                    Me.Active = dr("Active")
                    Me.UserSummaryID = dr("UserSummaryID")
                    Me.FollowUpQuestionsCompleted = dr("FollowUpQuestionsCompleted")
                    Me.IsFollowUpQuestion = dr("IsFollowUpQuestion")
                    Me.CreatedByID = dr("CreatedByID")
                    Me.CreatedByDate = dr("CreatedByDate")
                    Me.LastModifiedID = dr("LastModifiedID")
                    Me.LastModifiedDate = dr("LastModifiedDate")
                    Me.LastModifiedNetworkID = dr("LastModifiedNetworkID")
                End If
            Catch ex As Exception

                If Not objdb Is Nothing Then objdb.Dispose()
                objdb = Nothing


                Dim sInnerException As String = ""
                If Not ex.InnerException Is Nothing Then
                    sInnerException = sInnerException
                End If

                Throw New Exception("Error UserQuestionsDTO.Populate: " & vbCrLf & ex.Message & sInnerException)
            Finally

                If bDBIsNothing Then
                    If Not objdb Is Nothing Then objdb.Dispose()
                    objdb = Nothing
                End If

            End Try
        End Sub

        Public Function Save(Optional ByVal db As CertDB = Nothing) As Boolean

            Dim bDBIsNothing As Boolean = db Is Nothing
            Dim retval As Boolean = False

            Try
                If bDBIsNothing Then db = New CertDB

                retval = db.SaveUserQuestions(Me)
                Me.UserQuestionsID = db.Identity

            Catch ex As Exception

                If Not db Is Nothing Then db.Dispose()
                db = Nothing

                Throw New Exception("UserQuestionsDto.Save: " & ex.Message & vbCrLf)

            Finally

                If bDBIsNothing Then
                    If Not db Is Nothing Then db.Dispose()
                    db = Nothing
                End If

            End Try

            Return retval

        End Function

        Public Function SaveAnswer(ByVal UserQuestionsID As Integer, ByVal SelectedAnswer As String, ByVal LastModifiedID As Integer, ByRef db As CertDB) As Boolean
            'Force the caller to pass through an existing CertDB so we're not creating db connections over and over.

            Dim retval As Boolean = False

            Try

                retval = db.SaveUserQuestionsAnswer(UserQuestionsID, SelectedAnswer, LastModifiedID)

            Catch ex As Exception
                Throw New Exception("UserQuestionsDto.SaveAnswer: " & ex.Message & vbCrLf)
            End Try

            Return retval

        End Function

        Public Function SaveFollowUpUserQuestionsAnswer(ByVal UserQuestionsID As Integer, ByVal SelectedAnswer As String, ByVal FollowUpQuestionsCompleted As Boolean, ByVal LastModifiedID As Integer, ByRef db As CertDB) As Boolean
            'Force the caller to pass through an existing CertDB so we're not creating db connections over and over.

            Dim retval As Boolean = False

            Try

                retval = db.SaveFollowUpUserQuestionsAnswer(UserQuestionsID, SelectedAnswer, FollowUpQuestionsCompleted, LastModifiedID)

            Catch ex As Exception
                Throw New Exception("UserQuestionsDto.SaveAnswer: " & ex.Message & vbCrLf)
            End Try

            Return retval

        End Function

    End Class

#End Region

#Region "SearchDTO"

    Public Class SearchDto

#Region "Properties"
        Private db As CertDB

        Private sFirstName As String
        Private sMiddleName As String
        Private sLastName As String
        Private sStreet As String
        Private sCity As String
        Private sCounty As String
        Private sState As String
        Private sZipCode As String
        Private sCompany As String
        Private objIssueDate As Object
        Private sDriversLicense As String
        Private objCertificationNumber As Object

        Public Property FirstName() As String
            Get
                FirstName = sFirstName
            End Get
            Set(ByVal value As String)
                sFirstName = value
            End Set
        End Property

        Public Property MiddleName() As String
            Get
                MiddleName = sMiddleName
            End Get
            Set(ByVal value As String)
                sMiddleName = value
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

        Public Property Street() As String
            Get
                Street = sStreet
            End Get
            Set(ByVal value As String)
                sStreet = value
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

        Public Property County() As String
            Get
                County = sCounty
            End Get
            Set(ByVal value As String)
                sCounty = value
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

        Public Property Company() As String
            Get
                Company = sCompany
            End Get
            Set(ByVal value As String)
                sCompany = value
            End Set
        End Property

        Public Property IssueDate() As Object
            Get
                If objIssueDate Is Nothing Then
                    IssueDate = DBNull.Value
                Else
                    IssueDate = objIssueDate
                End If
            End Get
            Set(ByVal value As Object)
                Dim x As DateTime
                If Not value Is DBNull.Value And Not value Is Nothing Then
                    If DateTime.TryParse(value.ToString, x) Then
                        objIssueDate = x
                    Else
                        Throw New Exception("Issue Date - Invalid data type.  '" & value.ToString & "' is not of type DateTime.")
                    End If
                Else
                    objIssueDate = Nothing
                End If
            End Set
        End Property

        Public Property DriversLicense() As String
            Get
                DriversLicense = sDriversLicense
            End Get
            Set(ByVal value As String)
                sDriversLicense = value
            End Set
        End Property

        Public Property CertificationNumber() As Object
            Get
                If objCertificationNumber Is Nothing Then
                    CertificationNumber = DBNull.Value
                Else
                    CertificationNumber = objCertificationNumber
                End If
            End Get
            Set(ByVal value As Object)
                objCertificationNumber = value
            End Set
        End Property

#End Region

        Public Function SelectCertifiedUsersByOptionalColumns(Optional ByVal db As CertDB = Nothing) As DataSet
            Dim bDBIsNothing As Boolean = db Is Nothing
            Dim ds As DataSet

            Try
                If bDBIsNothing Then db = New CertDB

                ds = db.SelectCertifiedUsersByOptionalColumns(Me)
            Catch ex As Exception

                If Not db Is Nothing Then db.Dispose()
                db = Nothing

                Throw New Exception("UserQuestionsDto.Save: " & ex.Message & vbCrLf)
            Finally

                If bDBIsNothing Then
                    If Not db Is Nothing Then db.Dispose()
                    db = Nothing
                End If

            End Try

            Return ds
        End Function

    End Class

#End Region

#Region "UserDto"

    Public Class UserDto

#Region "Properties"

        Private objdb As CertDB

        Private iUserID As Int32 = 0
        Private sEmailAddress As String
        Private bPassword As Byte()
        Private sFName As String
        Private sMName As String
        Private sLName As String
        Private sStreet As String
        Private sCity As String
        Private sState As String
        Private sZipCode As String
        Private sWorkPhone As String
        Private sMobilePhone As String
        Private sCompanyName As String
        Private bUseCompressedVideos As Boolean
        Private iCreatedByID As Int32
        Private dCreatedByDate As DateTime
        Private iLastModifiedByID As Int32
        Private dLastModifiedDate As DateTime
        Private sLastModifiedNetworkID As String

        Public Property UserID() As Int32
            Get
                UserID = iUserID
            End Get
            Set(ByVal value As Int32)
                iUserID = value
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

        Public Property Password() As Byte()
            Get
                Password = bPassword
            End Get
            Set(ByVal value As Byte())
                bPassword = value
            End Set
        End Property

        Public Property FName() As String
            Get
                FName = sFName
            End Get
            Set(ByVal value As String)
                sFName = value
            End Set
        End Property

        Public Property MName() As String
            Get
                MName = sMName
            End Get
            Set(ByVal value As String)
                sMName = value
            End Set
        End Property

        Public Property LName() As String
            Get
                LName = sLName
            End Get
            Set(ByVal value As String)
                sLName = value
            End Set
        End Property

        Public Property Street() As String
            Get
                Street = sStreet
            End Get
            Set(ByVal value As String)
                sStreet = value
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

        Public Property WorkPhone() As String
            Get
                WorkPhone = sWorkPhone
            End Get
            Set(ByVal value As String)
                sWorkPhone = value
            End Set
        End Property

        Public Property MobilePhone() As String
            Get
                MobilePhone = sMobilePhone
            End Get
            Set(ByVal value As String)
                sMobilePhone = value
            End Set
        End Property

        Public Property CompanyName() As String
            Get
                CompanyName = sCompanyName
            End Get
            Set(ByVal value As String)
                sCompanyName = value
            End Set
        End Property

        Public Property UseCompressedVideos() As Boolean
            Get
                UseCompressedVideos = bUseCompressedVideos
            End Get
            Set(ByVal value As Boolean)
                bUseCompressedVideos = value
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

        Public Property LastModifiedByID() As Int32
            Get
                LastModifiedByID = iLastModifiedByID
            End Get
            Set(ByVal value As Int32)
                iLastModifiedByID = value
            End Set
        End Property

        Public Property LastModifiedDate() As DateTime
            Get
                LastModifiedDate = dLastModifiedDate
            End Get
            Set(ByVal value As DateTime)
                dLastModifiedDate = value
            End Set
        End Property

        Public Property LastModifiedNetworkID() As String
            Get
                LastModifiedNetworkID = sLastModifiedNetworkID
            End Get
            Set(ByVal value As String)
                sLastModifiedNetworkID = value
            End Set
        End Property

#End Region

        Public Sub Populate(ByRef ID As Integer, ByRef db As CertDB)

            Try

                objdb = db
                Populate(ID)
                objdb = Nothing

            Catch ex As Exception
                Throw New Exception("UserDto.Populate(ByVal ID As Integer, ByRef db As MCDB) Error:   ID = " & ID.ToString & vbCrLf & ex.Message)
            End Try
        End Sub
        Public Sub Populate(ByRef ID As Integer)

            Dim bDBIsNothing As Boolean = objdb Is Nothing
            If bDBIsNothing Then objdb = New CertDB

            Try

                Dim ds As DataSet = objdb.SelectByIDUser(ID)

                If ds.Tables(0).Rows.Count > 0 Then
                    Dim dr As DataRow = ds.Tables(0).Rows(0)
                    Me.UserID = dr("UserID")
                    Me.EmailAddress = dr("EmailAddress")
                    Me.Password = dr("Password")
                    Me.FName = dr("FName")
                    Me.MName = dr("MName")
                    Me.LName = dr("LName")
                    Me.Street = dr("Street")
                    Me.City = dr("City")
                    Me.State = dr("State")
                    Me.ZipCode = dr("ZipCode")
                    Me.WorkPhone = dr("WorkPhone")
                    Me.MobilePhone = dr("MobilePhone")
                    Me.CompanyName = dr("CompanyName")
                    Me.UseCompressedVideos = dr("UseCompressedVideos")
                    Me.CreatedByID = dr("CreatedByID")
                    Me.CreatedByDate = dr("CreatedByDate")
                    Me.LastModifiedByID = dr("LastModifiedByID")
                    Me.LastModifiedDate = dr("LastModifiedDate")
                    Me.LastModifiedNetworkID = dr("LastModifiedNetworkID")
                End If
            Catch ex As Exception
                Throw New Exception("Error UserDTO.Populate: " & vbCrLf & ex.Message)
            Finally
                If bDBIsNothing Then objdb = Nothing
            End Try
        End Sub

        Public Function Save(Optional ByVal db As CertDB = Nothing) As Boolean

            Dim bDBIsNothing As Boolean = db Is Nothing
            If bDBIsNothing Then db = New CertDB
            Dim retval As Boolean = False

            Try

                retval = db.SaveUser(Me)
                Me.UserID = db.Identity

            Catch ex As Exception
                Throw New Exception("UserDto.Save: " & ex.Message & vbCrLf)

            Finally
                If bDBIsNothing Then db = Nothing
            End Try

            Return retval

        End Function

    End Class

#End Region




End Class