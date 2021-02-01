Imports System.Web
Imports System.Web.UI
Imports System.Threading


Public Class Certification
    Inherits System.Web.UI.Page

#Region "Variables"

    Private db As CertDB
    Private l As CertDBLib = New CertDBLib

    Public swfFileName As String = ""
    Private NextModuleItemID As Integer = 0
    Private bLastNode As Boolean = False

#End Region


    Private Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error
        ' 01/15/2014 - GRP - From this page: http://msdn.microsoft.com/en-us/library/ed577840(v=vs.100).aspx
        '                    I replaced the ExceptionUtility with our Logit method and a bunch of other stuff, see the link if you want the original

        ' Get last error from the server
        Dim ex As Exception = Server.GetLastError


        Dim sInnerException As String = ""
        If Not ex.InnerException Is Nothing Then
            sInnerException = sInnerException
        End If

        l.LogIt("Certification PageLoad Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)
        Server.ClearError()
        Response.Redirect("~/default.aspx?Msg=39")

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim bDBIsNothing As Boolean

        Try


            bDBIsNothing = db Is Nothing

            If bDBIsNothing Then db = New CertDB
            Dim dto As CertDto.CertificationTypeDto = New CertDto.CertificationTypeDto



            Dim ds As DataSet

            If Not Request.QueryString("CertificationTypeID") Is Nothing Then
                If Request.QueryString("CertificationTypeID").ToString.Trim.Length > 0 Then
                    Session("CertificationTypeID") = CInt(Request.QueryString("CertificationTypeID"))
                End If
            End If

            If Not Request.QueryString("UserSummaryID") Is Nothing Then
                If Request.QueryString("UserSummaryID").ToString.Trim.Length > 0 Then
                    Session("UserSummaryID") = CInt(Request.QueryString("UserSummaryID"))
                End If
            End If

            If Not Request.QueryString("LastModule") Is Nothing Then
                If Request.QueryString("LastModule").ToString.Trim.Length > 0 Then
                    Session("LastModule") = CInt(Request.QueryString("LastModule"))

                    ds = db.SelectLastNodeByUserIDCertificationTypeID(Session("RegistrationID"), Session("CertificationTypeID"), Session("UserSummaryID"))
                    If ds.Tables(0).Rows.Count > 0 Then
                        If CInt(Session("LastModule")) = CInt(ds.Tables(0).Rows(0).Item("ItemID")) Then
                            bLastNode = True
                        End If
                    End If
                End If
            End If

            If Not IsPostBack Then

                If Session("RegistrationID") Is Nothing Then
                    Response.Redirect("~/Login.aspx")
                End If

                If Session("CertificationTypeID") > 0 Then
                    BuildTree(db)
                End If

                If Session("CertificationTypeID") > 0 Then
                    dto.Populate(Session("CertificationTypeID"), db)
                    Me.lblCertificationName.Text = dto.CertificationName
                End If

                Dim dtoUS As CertDto.UserSummaryDto = New CertDto.UserSummaryDto
                dtoUS.Populate(Session("UserSummaryID"), db)

                If Not dtoUS.ExamEndDate Is DBNull.Value Then
                    Me.btSaveCont.Visible = False
                    Me.btTakeExam.Visible = False
                End If

                'testing these last lines in postback - bb - 5/10/2012
                'if there is a last module passed, open tree to that module
                TraverseTreeToFindLastModule()

                If Me.tvItems.SelectedNode Is Nothing Then
                    Me.tvItems.Nodes(0).Selected = True
                    DisplayData()
                    Me.tvItems.SelectedNode.ImageUrl = "~/Images/tvArrowRight.png"
                End If

                SaveTree()
            Else

            End If

        Catch ex1 As ThreadAbortException
            'Response.Redirect() will throw this error, we don't need to do anything with it, so we'll capture it here, instead of in the most general exception below
            'do nothing
        Catch ex As Exception

            If Not db Is Nothing Then db.Dispose()
            db = Nothing


            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            l.LogIt("PageLoad Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)
            Try
                Response.Redirect("~/default.aspx?Msg=38")

            Catch ex2 As ThreadAbortException
                'Response.Redirect() will throw this error, we don't need to do anything with it, so we'll capture it here, instead of in the most general exception below
                'do nothing
            End Try
        Finally

            If bDBIsNothing Then
                If Not db Is Nothing Then db.Dispose()
                db = Nothing
            End If

        End Try


    End Sub

#Region "Tree methods"

    Private Sub TraverseTreeToFindLastModule()
        For Each tn As TreeNode In Me.tvItems.Nodes
            If CInt(tn.Value) = Session("LastModule") Then
                tn.Selected = True
                DisplayData()
                Me.tvItems.SelectedNode.ImageUrl = "~/Images/tvArrowRight.png"
                Exit For
            Else
                TraverseChildNodes(tn)
            End If
        Next
    End Sub

    Private Sub TraverseChildNodes(ByRef tnChild As TreeNode)
        For Each tn As TreeNode In tnChild.ChildNodes
            If CInt(tn.Value) = Session("LastModule") Then
                tn.Selected = True
                DisplayData()
                Me.tvItems.SelectedNode.ImageUrl = "~/Images/tvArrowRight.png"
                Exit For
            Else
                TraverseChildNodes(tn)
            End If
        Next
    End Sub

    Private Sub BuildTree(ByRef db As CertDB)
        Dim bDBIsNothing = db Is Nothing

        Dim ds As DataSet

        Try
            'Let's at least try to catch any error instantiating the db object
            If bDBIsNothing Then db = New CertDB

            Me.tvItems.Nodes.Clear()

            ds = db.SelectAllUserItemsByParentIDCertificationType(0, Session("CertificationTypeID"), Session("RegistrationID"), Session("UserSummaryID")) 'get all root nodes
            For Each dr As DataRow In ds.Tables(0).Rows
                Dim tn As TreeNode = New TreeNode(dr("Name"), dr("ItemID"))
                tn.SelectAction = TreeNodeSelectAction.Select
                tn.NavigateUrl = "~/Certification.aspx?CertificationTypeID=" & Session("CertificationTypeID") & "&LastModule=" & tn.Value & "&UserSummaryID=" & Session("UserSummaryID")
                'If String.IsNullOrEmpty(dr("videoFile").ToString().Trim()) Then
                '    tn.NavigateUrl = "~/Certification.aspx?CertificationTypeID=" & Session("CertificationTypeID") &
                '                    "&LastModule=" & tn.Value & "&UserSummaryID=" & Session("UserSummaryID")
                'Else
                '    tn.NavigateUrl = "~/Media/" & dr("VideoFile").ToString()
                'End If



                SetNodeImage(tn, dr("ItemReviewed"))
                BuildTreeChildren(db, tn, dr("ItemID"))

                'add node to tree
                Me.tvItems.Nodes.Add(tn)
            Next

            Me.tvItems.ExpandAll()

        Catch ex As Exception

            If Not db Is Nothing Then db.Dispose()
            db = Nothing


            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            Throw New Exception("BuildTree Error: " & ex.Message & sInnerException)
        Finally

            If bDBIsNothing Then
                If Not db Is Nothing Then db.Dispose()
                db = Nothing
            End If

        End Try
    End Sub

    Private Sub BuildTreeChildren(ByRef db As CertDB, ByRef tnParent As TreeNode, ByVal ParentItemID As Integer)
        Try
            Dim tnChild As TreeNode
            Dim dsChild As DataSet

            dsChild = db.SelectAllUserItemsByParentIDCertificationType(ParentItemID, Session("CertificationTypeID"), Session("RegistrationID"), Session("UserSummaryID"))

            For Each dr As DataRow In dsChild.Tables(0).Rows
                tnChild = New TreeNode(dr("Name"), dr("ItemID"))
                tnChild.SelectAction = TreeNodeSelectAction.Select
                tnChild.NavigateUrl = "~/Certification.aspx?CertificationTypeID=" & Session("CertificationTypeID") & "&LastModule=" & tnChild.Value & "&UserSummaryID=" & Session("UserSummaryID")

                SetNodeImage(tnChild, dr("ItemReviewed"))
                'add node to tree
                tnParent.ChildNodes.Add(tnChild)

                'Now grab the childs children
                BuildTreeChildren(db, tnChild, dr("ItemID"))
            Next
        Catch ex As Exception

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            Throw New Exception("BuildTreeChildren Error: " & ex.Message & sInnerException)
        End Try
    End Sub

    Private Sub SetNodeImage(ByRef TreeNode As TreeNode, ByVal Value As Boolean)
        If Value Then
            TreeNode.ImageUrl = "~/Images/tvComplete.png"
        Else
            TreeNode.ImageUrl = "~/Images/tvIncomplete.png"
        End If
    End Sub
#End Region


    Private Sub DisplayData()
        Dim bDBIsNothing = db Is Nothing
        Dim ds As DataSet
        Dim bFollowUpQuestionsCompleted As Boolean = True

        Try


            db = New CertDB

            ds = db.SelectByItemsIDFromUserItems(Me.tvItems.SelectedValue, Session("UserSummaryID"))
            Dim dr As DataRow = ds.Tables(0).Rows(0)

            If dr("ItemTypeID") = CertDBLib.ItemTypeID.Question Then
                'show the followup questions panel
                Me.pnlFollowUpQuestions.Visible = True
                Me.pnlMain.Visible = False

                Dim dsFollowUpQuestions As DataSet
                dsFollowUpQuestions = db.SelectAllFollowUpQuestionsCertificationTypeID(Session("CertificationTypeID"), Me.tvItems.SelectedValue, Session("RegistrationID"), Session("UserSummaryID"))

                Me.rptFollowUpQuestions.DataSource = dsFollowUpQuestions
                Me.rptFollowUpQuestions.DataBind()

                Session("ItemTypeID") = CertDBLib.ItemTypeID.Question       'How is the Session("ItemTypeID") defaulted to 2???????  ###10/08/2013 - GRP


                If dsFollowUpQuestions.Tables(0).Rows.Count > 0 Then
                    'Loop through and see if they're all answered.

                    For i As Integer = 0 To dsFollowUpQuestions.Tables(0).Rows.Count - 1
                        If Not dsFollowUpQuestions.Tables(0).Rows(i).Item("FollowUpQuestionsCompleted") Then
                            bFollowUpQuestionsCompleted = False
                            Exit For
                        End If
                    Next
                    Session("FollowUpQuestionsCompleted") = bFollowUpQuestionsCompleted
                End If

                PopulateFollowUpQuestionsForm()
            Else

                Session("ItemTypeID") = dr("ItemTypeID")                    '

                Me.pnlMain.Visible = True
                Me.pnlFollowUpQuestions.Visible = False

                Me.lblHeading.Text = dr("Heading")
                Me.lblItemHeading.Text = dr("ItemHeading")
                Me.lblVerbiage.Text = dr("Verbiage")
                Dim sVideo As String = dr("VideoFile").ToString.Trim
                Dim sImage As String = dr("ImageFile").ToString.Trim

                If sVideo.Length > 0 Then
                    Me.pnlVideo.Visible = True
                    If Session("UseCompressedVideos") Then
                        pnlVideo.Visible = False
                        pnlCompressed.Visible = True
                        'Dim sMV4 As String = sVideo.Replace(".swf", ".m4v")
                        Dim sMV4 As String = sVideo.Replace(".swf", ".mp4")
                        'swfFileName = "../Media/" & Session("CertificationTypeID") & "/Videos/Compressed/" & sMV4
                        swfFileName = "./Media/" & Session("CertificationTypeID") & "/Videos/Compressed/" & sMV4
                    Else
                        'swfFileName = "../Media/" & Session("CertificationTypeID") & "/Videos/" & sVideo
                        swfFileName = "./Media/" & Session("CertificationTypeID") & "/Videos/" & sVideo
                    End If

                Else
                    Me.pnlVideo.Visible = False

                    Me.pnlVideo.Height = 1

                End If

                If sImage.Length > 0 Then
                    Me.pnlImage.Visible = True
                    'iImage.ImageUrl = "../Media/" & Session("CertificationTypeID") & "/Images/" & sImage
                    iImage.ImageUrl = "./Media/" & Session("CertificationTypeID") & "/Images/" & sImage
                Else
                    Me.pnlImage.Visible = False
                End If

                If dr("VideoCaption") Is DBNull.Value Then
                    Me.lblVideoCaption.Text = ""
                Else
                    Me.lblVideoCaption.Text = dr("VideoCaption")
                End If

                If dr("ImageCaption") Is DBNull.Value Then
                    Me.lblImageCaption.Text = ""
                Else
                    Me.lblImageCaption.Text = dr("ImageCaption")
                End If

                Me.hlStateManual.Text = My.Settings.StateManualVerbiage
                Me.hlStateManual.NavigateUrl = My.Settings.StateManualURL

                Me.hlRPCpt1.Text = My.Settings.RPCpt1Verbiage
                Me.hlRPCpt1.NavigateUrl = My.Settings.RPCpt1URL

                Me.hlRPCpt2.Text = My.Settings.RPCpt2Verbiage
                Me.hlRPCpt2.NavigateUrl = My.Settings.RPCpt2URL


            End If

        Catch ex As Exception

            If Not db Is Nothing Then db.Dispose()
            db = Nothing


            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            Throw New Exception("DisplayData Error: " & ex.Message & sInnerException)
        Finally

            If bDBIsNothing Then
                If Not db Is Nothing Then db.Dispose()
                db = Nothing
            End If

        End Try
    End Sub


    Private Sub ClearControls()
        Try
            Me.lblHeading.Text = ""
            Me.lblItemHeading.Text = ""
            Me.lblVerbiage.Text = ""
        Catch ex As Exception

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            Throw New Exception("ClearControls Error: " & ex.Message & sInnerException)
        End Try
    End Sub

    Protected Sub lbSaveCont_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            SaveTree()
            Session("LastModule") = Nothing
        Catch ex As Exception

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            l.LogIt("Save&Continue Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)
        Finally
            Response.Redirect("~/Certification.aspx?CertificationTypeID=" & Session("CertificationTypeID") & "&LastModule=" & NextModuleItemID & "&UserSummaryID=" & Session("UserSummaryID"))
        End Try
    End Sub

    Private Sub SaveTree()

        Dim bDBIsNothing As Boolean

        Try

            bDBIsNothing = db Is Nothing
            If bDBIsNothing Then db = New CertDB

            Dim dsUI As DataSet
            Dim dtoUI As CertDto.UserItemsDto = New CertDto.UserItemsDto

            Dim dsUS As DataSet
            Dim dtoUS As CertDto.UserSummaryDto = New CertDto.UserSummaryDto
            Dim bItemReviewed As Boolean = False
            Me.lblMessage.Text = "" 'Clear any previous error messages
            Me.lblMessage2.Text = "" 'Clear any previous error messages



            'update tbUserItems's ItemReviewed as True for the currently selected node
            dsUI = db.SelectUserItemsByUserIDItemID(Session("RegistrationID"), CInt(Me.tvItems.SelectedNode.Value), Session("UserSummaryID"))
            If dsUI.Tables(0).Rows.Count > 0 Then
                dtoUI.Populate(dsUI.Tables(0).Rows(0).Item("UserItemsID"), db)
                bItemReviewed = dtoUI.ItemReviewed     'Before we set it grab the original value so we know whether or not to show the Answer All Questions message.
                dtoUI.ItemReviewed = True
                dtoUI.LastModifiedID = Session("RegistrationID")
                dtoUI.Save(db)
            End If

            If Session("ItemTypeID") = CertDBLib.ItemTypeID.Question Then

                If Not Session("FollowUpQuestionsCompleted") Then

                    'Save the questions no matter what
                    SaveFollowUpQuestions()


                    If Not ValidateFollowUpQuestions() Then
                        If bItemReviewed Then
                            Me.lblMessage.Text = "Please answer all Review Questions."
                            Me.lblMessage.ForeColor = Drawing.Color.Red
                            Me.lblMessage2.Text = "Please answer all Review Questions."
                            Me.lblMessage2.ForeColor = Drawing.Color.Red

                            '### 02/14/2014 - GRP - They didn't answer all the questions so we want to stay on this page, use the same ItemID
                            'NextModuleItemID = Session("LastModule")

                        End If
                    End If

                    NextModuleItemID = Session("LastModule") '### 02/14/2014 - GRP - Comment this out and check for bItemReviewed to see what the next module should be

                Else
                    SetAllControlsToReadOnly()
                    SetNextModuleItemID()
                End If

            Else
                SetNextModuleItemID()
            End If

            dsUS = Nothing

            dtoUS.Populate(Session("UserSummaryID"), db)
            dtoUS.LastModifiedID = Session("RegistrationID")

            If Not bLastNode Then
                dtoUS.NextModuleItemID = NextModuleItemID
            Else
                dtoUS.EndDate = FormatDateTime(DateTime.Now, DateFormat.ShortDate)
            End If

            dtoUS.Save(db)

        Catch ex As Exception

            If Not db Is Nothing Then db.Dispose()
            db = Nothing


            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            l.LogIt("SaveTree Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)

            Throw New Exception("Save Error: " & ex.Message & sInnerException)
        Finally

            If bDBIsNothing Then
                If Not db Is Nothing Then db.Dispose()
                db = Nothing
            End If

        End Try
    End Sub

    Protected Sub lbTakeExam_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Response.Redirect("~/Exam.aspx?CertificationTypeID=" & Session("CertificationTypeID") & "&UserSummaryID=" & Session("UserSummaryID"))
        Catch ex1 As ThreadAbortException

        Catch ex As Exception

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            l.LogIt("TakeExam Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)
        End Try
    End Sub

    Private Function ValidateFollowUpQuestions() As Boolean
        Dim retval As Boolean = True

        Try
            For Each item As RepeaterItem In rptFollowUpQuestions.Items
                Dim UC As ExamRepeaterControl = New ExamRepeaterControl()
                UC = CType(item.FindControl("ExamRepeaterControl1"), ExamRepeaterControl)

                If UC.Answer = "" Then
                    retval = False
                    Exit For
                End If
            Next

            Return retval
        Catch ex As Exception

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            Throw New Exception("ValidateFollowUpQuestions Error: " & ex.Message & sInnerException)
        End Try
    End Function

    Private Sub SaveFollowUpQuestions()
        Dim bDBIsNothing As Boolean
        Dim dto As CertDto.UserQuestionsDto

        Try

            Dim iRegistrationID As Integer
            Dim bFollowUpQuestionsCompleted As Boolean

            If Integer.TryParse(Session("RegistrationID"), iRegistrationID) Then

                bDBIsNothing = db Is Nothing
                If bDBIsNothing Then db = New CertDB

                dto = New CertDto.UserQuestionsDto

                For Each item As RepeaterItem In rptFollowUpQuestions.Items
                    Dim UC As ExamRepeaterControl = New ExamRepeaterControl()
                    UC = CType(item.FindControl("ExamRepeaterControl1"), ExamRepeaterControl)

                    bFollowUpQuestionsCompleted = UC.Answer <> ""

                    If Not dto.SaveFollowUpUserQuestionsAnswer(UC.QuestionID, UC.Answer, bFollowUpQuestionsCompleted, iRegistrationID, db) Then
                        Throw New Exception("Save Error")
                    End If

                Next
            Else
                Throw New Exception("SaveFollowUpQuestions: Invalid RegistrationID.")
            End If

        Catch ex As Exception

            If Not db Is Nothing Then db.Dispose()
            db = Nothing


            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            Throw New Exception("SaveFollowUpQuestions Error: " & ex.Message & sInnerException)
        Finally

            If bDBIsNothing Then
                If Not db Is Nothing Then db.Dispose()
                db = Nothing
            End If
            dto = Nothing
        End Try
    End Sub

    Private Sub PopulateFollowUpQuestionsForm()
        Try

            Dim PreviousQuestionHeading As String = ""

            For Each item As RepeaterItem In rptFollowUpQuestions.Items
                Dim UC As ExamRepeaterControl = New ExamRepeaterControl
                UC = CType(item.FindControl("ExamRepeaterControl1"), ExamRepeaterControl)

                If UC.SelectedAnswer <> "" Then
                    Select Case UC.SelectedAnswer
                        Case UC.Answer1
                            UC.Answer = UC.Answer1
                        Case UC.Answer2
                            UC.Answer = UC.Answer2
                        Case UC.Answer3
                            UC.Answer = UC.Answer3
                        Case UC.Answer4
                            UC.Answer = UC.Answer4
                        Case UC.Answer5
                            UC.Answer = UC.Answer5
                    End Select
                End If

                If PreviousQuestionHeading = UC.QuestionHeading Then
                    UC.QuestionHeadingVisible = False
                Else
                    UC.QuestionHeadingVisible = True
                End If

                PreviousQuestionHeading = UC.QuestionHeading
            Next
        Catch ex As Exception

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            Throw New Exception("PopulateFollowUpQuestionsForm Error: " & ex.Message & sInnerException)
        End Try
    End Sub

    Private Sub SetAllControlsToReadOnly()
        Try
            For Each item As RepeaterItem In rptFollowUpQuestions.Items
                Dim UC As ExamRepeaterControl = New ExamRepeaterControl
                UC = CType(item.FindControl("ExamRepeaterControl1"), ExamRepeaterControl)

                UC.Enabled = False
            Next
        Catch ex As Exception

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            Throw New Exception("SetAllControlsToReadOnly Error: " & ex.Message & sInnerException)
        End Try
    End Sub

    Private Sub SetNextModuleItemID()
        Dim dsUS As DataSet
        Dim dtoUS As CertDto.UserSummaryDto = New CertDto.UserSummaryDto

        Try
            dsUS = db.SelectNextModuleByUserIDCertificationTypeID(Session("RegistrationID"), Session("CertificationTypeID"), Session("UserSummaryID"))
            If dsUS.Tables(0).Rows.Count > 0 Then
                NextModuleItemID = dsUS.Tables(0).Rows(0).Item("ItemID")
            Else
                dtoUS.Populate(Session("UserSummaryID"), db)
                NextModuleItemID = dtoUS.NextModuleItemID
            End If
        Catch ex As Exception

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            Throw New Exception("SetNextModuleItemID Error: " & ex.Message & sInnerException)
        End Try
    End Sub


    Private Sub btContinue_Click(sender As Object, e As EventArgs) Handles btContinue.Click
        Try
            SaveTree()
            Session("LastModule") = Nothing
        Catch ex As Exception

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            l.LogIt("Save&Continue Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)
        Finally
            Response.Redirect("~/Certification.aspx?CertificationTypeID=" & Session("CertificationTypeID") & "&LastModule=" & NextModuleItemID & "&UserSummaryID=" & Session("UserSummaryID"))
        End Try
    End Sub

    Private Sub btSaveCont_Click(sender As Object, e As EventArgs) Handles btSaveCont.Click
        Try
            SaveTree()
            Session("LastModule") = Nothing
        Catch ex As Exception

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            l.LogIt("Save&Continue Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)
        Finally
            Response.Redirect("~/Certification.aspx?CertificationTypeID=" & Session("CertificationTypeID") & "&LastModule=" & NextModuleItemID & "&UserSummaryID=" & Session("UserSummaryID"))
        End Try
    End Sub

    Private Sub btTakeExam_Click(sender As Object, e As EventArgs) Handles btTakeExam.Click
        Try
            Response.Redirect("~/Exam.aspx?CertificationTypeID=" & Session("CertificationTypeID") & "&UserSummaryID=" & Session("UserSummaryID"))
        Catch ex1 As ThreadAbortException

        Catch ex As Exception

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            l.LogIt("TakeExam Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)
        End Try
    End Sub

    Private Sub btTakeExam2_Click(sender As Object, e As EventArgs) Handles btTakeExam2.Click
        Try
            Response.Redirect("~/Exam.aspx?CertificationTypeID=" & Session("CertificationTypeID") & "&UserSummaryID=" & Session("UserSummaryID"))
        Catch ex1 As ThreadAbortException

        Catch ex As Exception

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            l.LogIt("TakeExam Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)
        End Try

    End Sub
End Class