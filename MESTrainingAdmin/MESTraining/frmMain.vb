Imports MESTrainingDB.CertDB
Imports System.IO
Imports System.Configuration
'Imports System.Net.FtpClient   'Removed and replaced with Upload.aspx in the website
Imports System.Text
Imports Design

'Imports ClosedXML.Excel
'Imports Microsoft.Office.Interop.Excel

Public Class frmMain

#Region "Properties, Variables, & Constants"

    Private dwsdb As daWSDB
    Private bIsTreeDirty As Boolean = False
    Private bIsQuestionsDirty As Boolean = False
    Private bFormLoading As Boolean = False
    Private bClearFields As Boolean = False
    Private bPopulateFields As Boolean = False
    Private bComboBoxLoading As Boolean = False
    Private bLoadingGrid As Boolean = False
    Private bQuestionsGridClick As Boolean = False
    Private bQuestionsItemComboBox As Boolean = False
    Private dto As daWSDTO.ItemsDto
    Private DisplaySequenceVar As Integer
    Private l As CertLib = New CertLib
    Private QuestionID As Integer = 0
    Private iPrevQuestionsID As Integer = 0
    Private bClearQuestionFields As Boolean = False
    Private bPopulateQuestionFields As Boolean = False
    Private iPrevNodeID As Integer = 0
    Private sPrevNodeName As String = ""
    Private iDgvRow As Integer = 0
    Public bIsAdministrator As Boolean = False

    Private bIsFollowUpQuestionsDirty As Boolean = False
    Private bFollowUpQuestionsItemComboBox As Boolean = False
    Private FollowUpQuestionID As Integer = 0
    Private bPopulateFollowUpQuestionFields As Boolean = False
    Private bClearFollowUpQuestionFields As Boolean = False
    Private iPrevFollowUpQuestionID As Integer = 0
    Private bFollowUpQuestionsGridClick As Boolean = False
    Private iFollowUpDgvRow As Integer = 0

    Private nPrevNode As LidorSystems.IntegralUI.Lists.TreeNode

    Protected LogonUserID As Integer = 0
    Private UserID As Integer = 0
    Private EmployeeName As String = ""
    Private EmployeeNumber As String = ""
    Private UserName As String = ""

    'properties to send to frmReport to capture what the last report run was.
    Private LastReportRun As String = ""
    Private LastReportParameter As String = ""
    Private LastReportStartDate As Object = DBNull.Value
    Private LastReportEndDate As Object = DBNull.Value

#Region "Constants"

    'questions datagrid constants re-use these for Follow-Up Questions datagrid
    Private Const kQuestionsID = 0
    Private Const kName = 1
    Private Const kQuestions = 2
    Private Const kAnswer1 = 3
    Private Const kAnswer2 = 4
    Private Const kAnswer3 = 5
    Private Const kAnswer4 = 6
    Private Const kAnswer5 = 7
    Private Const kCorrectAnswer = 8
    'Private Const kQuestionWeight = 8
    Private Const kActive = 9
    'Private Const kDisplaySequence = 9
    Private Const kItemID = 10
    Private Const kNumberOfAnswers = 11

    'Tab constants
    Private Const kTreeTab = 0
    Private Const kQuestionsTab = 1
    'Private Const kFollowUpQuestionsTab = 2    '04/09/2013 - GRP - Removing Review questions we're going to pull from the exam questions
    'Private Const kReportingTab = 3
    Private Const kReportingTab = 2

    'reporting datagrid constants
    Private Const kUserSummaryID = 0
    Private Const kCertificationTypeID = 1
    Private Const kCertificateNumber = 2
    Private Const kCertificationName = 3
    Private Const kFullName = 4
    Private Const kExamScore = 5
    Private Const kStartDate = 6
    Private Const kExamEndDate = 7
    Private Const kActiveExam = 8
    Private Const kReviewedMaterial = 9
    Private Const kExpirationDate = 10
    Private Const kPrintCertificate = 11

#End Region


    Public Property IsTreeDirty()
        Get
            IsTreeDirty = bIsTreeDirty
        End Get
        Set(ByVal value)
            bIsTreeDirty = value

            If Not bFormLoading And Not bPopulateFields Then
                Me.btnAddNode.Enabled = Not value
                Me.btnAddSubItem.Enabled = Not value
                Me.btnDelete.Enabled = Not value
                Me.btnUp.Enabled = Not value
                Me.btnDown.Enabled = Not value
                Me.btnLeft.Enabled = Not value
                Me.btnRight.Enabled = Not value
                Me.cbxCertificationTypes.Enabled = Not value

                Me.btnSave.Enabled = value
                Me.btnCancel.Enabled = value
            End If
        End Set
    End Property

    Public Property IsQuestionsDirty()
        Get
            IsQuestionsDirty = bIsQuestionsDirty
        End Get
        Set(ByVal value)
            bIsQuestionsDirty = value

            If Not bFormLoading And Not bPopulateFields And Not bQuestionsItemComboBox Then
                Me.btnNew.Enabled = Not value
                Me.cbxCertificationTypes.Enabled = Not value

                Me.btnSave.Enabled = value
                Me.btnCancel.Enabled = value
            End If
        End Set
    End Property

    Public Property IsFollowUpQuestionsDirty()
        Get
            IsFollowUpQuestionsDirty = bIsFollowUpQuestionsDirty
        End Get
        Set(ByVal value)
            bIsFollowUpQuestionsDirty = value

            If Not bFormLoading And Not bPopulateFields And Not bFollowUpQuestionsItemComboBox Then
                Me.btnNew.Enabled = Not value
                Me.cbxCertificationTypes.Enabled = Not value

                Me.btnSave.Enabled = value
                Me.btnCancel.Enabled = value
            End If
        End Set
    End Property

#End Region

#Region "Form Methods and events"

    Private Sub frmMain_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Try
            Me.Cursor = Cursors.WaitCursor

            If Me.TabControl1.SelectedIndex = kQuestionsTab Then
                'Questions tab
                If IsQuestionsDirty Then
                    If MsgBox("Do you want to save your changes?", MsgBoxStyle.YesNo, "Save?") = MsgBoxResult.Yes Then
                        SaveQuestions()
                    Else
                        IsQuestionsDirty = False
                    End If
                End If
            ElseIf Me.TabControl1.SelectedIndex = kTreeTab Then
                'Tree tab
                If IsTreeDirty Then
                    If MsgBox("Do you want to save your changes?", MsgBoxStyle.YesNo, "Save?") = MsgBoxResult.Yes Then
                        SaveTree()
                    Else
                        IsTreeDirty = False
                    End If
                End If
                'ElseIf Me.TabControl1.SelectedIndex = kFollowUpQuestionsTab Then
                '    If IsFollowUpQuestionsDirty Then
                '        If MsgBox("Do you want to save your changes?", MsgBoxStyle.YesNo, "Save?") = MsgBoxResult.Yes Then
                '            SaveFollowUpQuestions()
                '        Else
                '            IsFollowUpQuestionsDirty = False
                '        End If
                '    End If
            End If

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Form Closing Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        My.Settings.TrainingSiteName = My.Settings.DefaultTrainingSiteName
        My.Settings.EncryptedString = My.Settings.DefaultEncryptedString
        My.Settings.URL = My.Settings.DefaultURL
        My.Settings.MediaDir = My.Settings.DefaultMediaDir

        LoadForm()

    End Sub

    Private Sub LoadForm()
        Dim bDBIsNothing = dwsdb Is Nothing
        If bDBIsNothing Then dwsdb = New daWSDB
        Dim dto As daWSDTO.UserDto = New daWSDTO.UserDto

        Me.UseWaitCursor = True
        'Me.Cursor = Cursors.WaitCursor
        'Me.Refresh()

        Try

            If Not dwsdb Is Nothing Then
                'Dim frm As frmLogon = New frmLogon()
                'frm.ShowDialog()

                Me.LogonUserID = 4 'frm.UserID
                Me.UserID = 4 'frm.UserID
                Me.EmployeeName = "Greg" 'frm.EmployeeName
                Me.EmployeeNumber = 431 'frm.EmployeeNumber
                Me.UserName = "gpobs" 'frm.UserName


                'Me.LogonUserID = frm.UserID
                'Me.UserID = frm.UserID
                'Me.EmployeeName = frm.EmployeeName
                'Me.EmployeeNumber = frm.EmployeeNumber
                'Me.UserName = frm.UserName

                'frm.Dispose()

                If UserID = 0 Then
                    Me.Close()
                Else
                    'populate the user dto
                    dto.Populate(Me.UserID, dwsdb)

                    bFormLoading = True
                    Me.Cursor = Cursors.WaitCursor
                    Me.btnNew.Visible = False

                    BuildComboBox()
                    BuildQuestionsData()
                    BuildReportingTab()
                    'BuildFollowUpQuestionsData()      '04/09/2013 - GRP - Removing Followup questions we're going to pull from the exam questions
                    Me.cbxNumberOfAnswers.SelectedIndex = Me.cbxNumberOfAnswers.Items.Count - 1

                    If Me.TabControl1.TabPages.Count > 3 Then
                        Me.TabControl1.TabPages.RemoveAt(3) 'kFollowUpQuestionsTab
                    End If


                End If

                Me.cbxReports.SelectedIndex = -1

            Else
                Me.Close()
            End If

        Catch ex As Exception
            'Me.Cursor = Cursors.Default
            Me.UseWaitCursor = False

            l.LogIt("Form Load Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
            Me.Close()
        Finally
            'Me.Cursor = Cursors.Default
            Me.UseWaitCursor = False

            bFormLoading = False
            If bDBIsNothing Then dwsdb = Nothing
        End Try
    End Sub
#End Region

#Region "Build and Bind Controls"

    Public Sub BuildComboBox()
        Dim bDBIsNothing = dwsdb Is Nothing
        If bDBIsNothing Then dwsdb = New daWSDB

        Try
            bComboBoxLoading = True

            Me.cbxCertificationTypes.ValueMember = "CertificationTypeID"
            Me.cbxCertificationTypes.DisplayMember = "CertificationName"
            Me.cbxCertificationTypes.DataSource = dwsdb.SelectAllActiveCertificationType(Me.UserID).Tables(0)

            If cbxCertificationTypes.Items.Count > 0 Then
                Me.cbxCertificationTypes.SelectedIndex = 0
            End If

            Me.cbxItemTypes.ValueMember = "ItemTypeID"
            Me.cbxItemTypes.DisplayMember = "ItemDescription"
            Me.cbxItemTypes.DataSource = dwsdb.SelectActiveItemTypes.Tables(0)

            BuildQuestionItemNodeComboBox()
            BuildFollowUpQuestionItemNodeComboBox()

            If Me.cbxCertificationTypes.Items.Count < 1 Then
                SetButtons(False)
            Else
                SetButtons(True)
            End If

        Catch ex As Exception
            Throw New Exception("BuildComboBox Error" & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            bComboBoxLoading = False
            If bDBIsNothing Then dwsdb = Nothing
        End Try
    End Sub

    Private Sub BuildQuestionItemNodeComboBox()
        Dim bDBIsNothing = dwsdb Is Nothing
        If bDBIsNothing Then dwsdb = New daWSDB

        Try
            bQuestionsItemComboBox = True
            Dim dt As DataTable = New DataTable()

            If cbxCertificationTypes.Items.Count > 0 Then
                dt = dwsdb.SelectAllQuestionItemNodes(Me.cbxCertificationTypes.SelectedValue).Tables(0)
            Else
                Dim dc As DataColumn = New DataColumn()
                dc.ColumnName = "ItemID"
                dt.Columns.Add(dc)

                dc = New DataColumn()
                dc.ColumnName = "Name"
                dt.Columns.Add(dc)
            End If

            Dim dr As DataRow = dt.NewRow()
            dr("ItemID") = 0
            dr("Name") = "Un-Categorized"

            dt.Rows.InsertAt(dr, 0)

            With Me.cbxQuestionItemNode
                .BindingContext = Me.BindingContext
                .DisplayMember = "Name"
                .ValueMember = "ItemID"
                .DataSource = dt
                '.SelectedValue = 0
            End With

        Catch ex As Exception
            Throw New Exception("BuildQuestionItemNodeComboBox Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            If bDBIsNothing Then dwsdb = Nothing
            bQuestionsItemComboBox = False
        End Try
    End Sub

    Private Sub SetButtons(ByVal value As Boolean)
        Try
            Me.btnAddNode.Enabled = value
            Me.btnAddSubItem.Enabled = value
            Me.btnDelete.Enabled = value
            Me.btnUp.Enabled = value
            Me.btnDown.Enabled = value
            Me.btnLeft.Enabled = value
            Me.btnRight.Enabled = value
            Me.ViewTreeInIEToolStripMenuItem.Enabled = value
        Catch ex As Exception
            Throw New Exception("SetButtons Error: " & ex.Message)
        End Try
    End Sub

#End Region

#Region "Build Tree Methods"

    Private Sub BuildTree()
        Dim bDBIsNothing = dwsdb Is Nothing
        If bDBIsNothing Then dwsdb = New daWSDB

        Dim ds As DataSet
        Dim dtoParent As daWSDTO.ItemsDto
        Me.tvItems.SuspendUpdate()
        Me.tvItems.Nodes.Clear()

        Try
            ds = dwsdb.SelectAllActiveItemsByParentIDCertificationType(0, Me.cbxCertificationTypes.SelectedValue) 'get all root nodes
            For Each dr As DataRow In ds.Tables(0).Rows
                Dim tn As LidorSystems.IntegralUI.Lists.TreeNode = New LidorSystems.IntegralUI.Lists.TreeNode
                dtoParent = New daWSDTO.ItemsDto
                dtoParent.Populate(dr("ItemID"), dwsdb)
                tn.Tag = dtoParent

                tn.Text = dr("Name")
                BuildTreeChildren(tn, dr("ItemID"))

                'add node to tree
                Me.tvItems.Nodes.Add(tn)
            Next

            Me.tvItems.ResumeUpdate()
            Me.tvItems.ExpandAll()

            If Me.tvItems.Nodes.Count > 0 Then
                Me.tvItems.SelectedNode = Me.tvItems.Nodes(0)
                SetControls(True)

            Else
                SetControls(False)
            End If
        Catch ex As Exception
            Throw New Exception("BuildTree Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            If bDBIsNothing Then dwsdb = Nothing
        End Try
    End Sub

    Private Sub BuildTreeChildren(ByRef tnParent As LidorSystems.IntegralUI.Lists.TreeNode, ByVal ParentItemID As Integer)
        Try
            Dim tnChild As LidorSystems.IntegralUI.Lists.TreeNode
            Dim dtoChild As daWSDTO.ItemsDto
            Dim dsChild As DataSet

            dsChild = dwsdb.SelectAllActiveItemsByParentIDCertificationType(ParentItemID, Me.cbxCertificationTypes.SelectedValue)

            For Each dr As DataRow In dsChild.Tables(0).Rows
                dtoChild = New daWSDTO.ItemsDto
                dtoChild.Populate(dr("ItemID"), dwsdb)

                tnChild = New LidorSystems.IntegralUI.Lists.TreeNode(dtoChild.Name)
                tnChild.Tag = dtoChild

                'add node to tree
                tnParent.Nodes.Add(tnChild)

                'Now grab the childs children
                BuildTreeChildren(tnChild, dr("ItemID"))
            Next
        Catch ex As Exception
            Throw New Exception("BuildTreeChildren Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        End Try
    End Sub

#End Region

#Region "Save Methods"

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            Me.Cursor = Cursors.WaitCursor

            If Me.TabControl1.SelectedIndex = kQuestionsTab Then
                Dim iQID As Integer = 0
                iQID = SaveQuestions()
                SelectDGVRow(Me.dgvQuestions, iQID, kQuestionsID, kName)

            ElseIf Me.TabControl1.SelectedIndex = kTreeTab Then
                SaveTree()
                'ElseIf Me.TabControl1.SelectedIndex = kFollowUpQuestionsTab Then
                '    SaveFollowUpQuestions()
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Function SaveQuestions() As Integer
        Dim iQID As Integer = 0

        Try

            If ValidateQuestionFields() Then
                Dim QDto As daWSDTO.QuestionsDto = New daWSDTO.QuestionsDto

                QDto.QuestionsID = Me.QuestionID
                QDto.CertificationTypeID = Me.cbxCertificationTypes.SelectedValue
                QDto.Question = Me.txtQuestion.Text
                QDto.NumberOfAnswers = Me.cbxNumberOfAnswers.Text
                QDto.Answer1 = Me.txtAnswer1.Text
                QDto.Answer2 = Me.txtAnswer2.Text
                QDto.Answer3 = Me.txtAnswer3.Text
                QDto.Answer4 = Me.txtAnswer4.Text
                QDto.Answer5 = Me.txtAnswer5.Text
                QDto.QuestionWeight = Me.nudQuestionWeight.Text
                QDto.DisplaySequence = Me.nudDisplaySequence.Text

                If Me.rbCorrectAnswer1.Checked Then
                    QDto.CorrectAnswer = Me.txtAnswer1.Text
                ElseIf Me.rbCorrectAnswer2.Checked Then
                    QDto.CorrectAnswer = Me.txtAnswer2.Text
                ElseIf Me.rbCorrectAnswer3.Checked Then
                    QDto.CorrectAnswer = Me.txtAnswer3.Text
                ElseIf Me.rbCorrectAnswer4.Checked Then
                    QDto.CorrectAnswer = Me.txtAnswer4.Text
                ElseIf Me.rbCorrectAnswer5.Checked Then
                    QDto.CorrectAnswer = Me.txtAnswer5.Text
                Else
                    'shouldn't happen
                End If

                QDto.ItemID = Me.cbxQuestionItemNode.SelectedValue

                If Me.rbActiveTrue.Checked Then
                    QDto.Active = True
                Else
                    QDto.Active = False
                End If

                QDto.CreatedByID = Me.UserID
                QDto.LastModifiedID = Me.UserID

                If QDto.ItemID <> 0 Then

                    If QDto.Save() Then
                        iQID = QDto.QuestionsID
                        lblSave.Text = "Save Successful."
                        Me.lblSave.ForeColor = Color.Green
                        Me.lblSave.Visible = True
                        IsQuestionsDirty = False
                        BuildQuestionsData()
                    Else
                        Throw New Exception("Error Saving Question")
                    End If
                Else
                    Throw New Exception("Error Saving Question.  ItemID = 0.")
                End If
            End If

        Catch ex As Exception
            Throw New Exception("SaveQuestions Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            SaveQuestions = iQID
        End Try
    End Function

    Private Sub SaveFollowUpQuestions()
        Try
            If ValidateFollowUpQuestionFields() Then
                Dim QDto As daWSDTO.QuestionsDto = New daWSDTO.QuestionsDto

                QDto.QuestionsID = Me.FollowUpQuestionID
                QDto.CertificationTypeID = Me.cbxCertificationTypes.SelectedValue
                QDto.Question = Me.txtFollowUpQuestion.Text
                QDto.NumberOfAnswers = Me.cbxFollowUpNumberOfAnswers.Text
                QDto.Answer1 = Me.txtFollowUpAnswer1.Text
                QDto.Answer2 = Me.txtFollowUpAnswer2.Text
                QDto.Answer3 = Me.txtFollowUpAnswer3.Text
                QDto.Answer4 = Me.txtFollowUpAnswer4.Text
                QDto.Answer5 = Me.txtFollowUpAnswer5.Text
                QDto.QuestionWeight = Me.nudFollowUpQuestionWeight.Text
                QDto.DisplaySequence = Me.nudFollowUpDisplaySequence.Text

                If Me.rbFollowUpCorrectAnswer1.Checked Then
                    QDto.CorrectAnswer = Me.txtFollowUpAnswer1.Text
                ElseIf Me.rbFollowUpCorrectAnswer2.Checked Then
                    QDto.CorrectAnswer = Me.txtFollowUpAnswer2.Text
                ElseIf Me.rbFollowUpCorrectAnswer3.Checked Then
                    QDto.CorrectAnswer = Me.txtFollowUpAnswer3.Text
                ElseIf Me.rbFollowUpCorrectAnswer4.Checked Then
                    QDto.CorrectAnswer = Me.txtFollowUpAnswer4.Text
                ElseIf Me.rbFollowUpCorrectAnswer5.Checked Then
                    QDto.CorrectAnswer = Me.txtFollowUpAnswer5.Text
                Else
                    'shouldn't happen
                End If

                QDto.ItemID = Me.cbxFollowUpItemType.SelectedValue

                If Me.rbFollowUpActiveTrue.Checked Then
                    QDto.Active = True
                Else
                    QDto.Active = False
                End If

                QDto.CreatedByID = Me.UserID
                QDto.LastModifiedID = Me.UserID

                If QDto.Save() Then
                    lblSave.Text = "Save Successful."
                    Me.lblSave.ForeColor = Color.Green
                    Me.lblSave.Visible = True
                    IsFollowUpQuestionsDirty = False
                    BuildFollowUpQuestionsData()
                Else
                    Throw New Exception("Error Saving Follow-Up Question")
                End If
            End If

        Catch ex As Exception
            Throw New Exception("SaveFollowUpQuestions Error: " & ex.Message)
        End Try
    End Sub

    Private Function ValidateQuestionFields() As Boolean
        Try
            Dim ErrMsg As String = ""

            'must have at least 1 correct answer.
            If Not rbCorrectAnswer1.Checked Then
                If Not rbCorrectAnswer2.Checked Then
                    If Not rbCorrectAnswer3.Checked Then
                        If Not rbCorrectAnswer4.Checked Then
                            If Not rbCorrectAnswer5.Checked Then
                                ErrMsg += "Please select a correct answer." & vbCrLf
                            End If
                        End If
                    End If
                End If
            End If

            'Which ever radio button is checked as the correct answer must have text.
            If rbCorrectAnswer1.Checked And Not Me.txtAnswer1.Text.Trim.Length > 0 Then
                ErrMsg += "Answer 1 must have text." & vbCrLf
            ElseIf rbCorrectAnswer2.Checked And Not Me.txtAnswer2.Text.Trim.Length > 0 Then
                ErrMsg += "Answer 2 must have text." & vbCrLf
            ElseIf rbCorrectAnswer3.Checked And Not Me.txtAnswer3.Text.Trim.Length > 0 Then
                ErrMsg += "Answer 3 must have text." & vbCrLf
            ElseIf rbCorrectAnswer4.Checked And Not Me.txtAnswer4.Text.Trim.Length > 0 Then
                ErrMsg += "Answer 4 must have text." & vbCrLf
            ElseIf rbCorrectAnswer5.Checked And Not Me.txtAnswer5.Text.Trim.Length > 0 Then
                ErrMsg += "Answer 5 must have text." & vbCrLf
            End If

            If ErrMsg.Length > 0 Then
                MsgBox(ErrMsg, MsgBoxStyle.Exclamation, "Validation Error")
            End If

            Return ErrMsg.Length = 0
        Catch ex As Exception
            Throw New Exception("Validation Error")
        End Try
    End Function

    Private Sub SaveTree()
        Try
            Dim msg As String = ""

            If Not Me.tvItems.SelectedNode Is Nothing Then
                dto = Me.tvItems.SelectedNode.Tag

                dto.Name = Me.txtName.Text

                'dto.Heading = Me.txtHeading.Text
                dto.Heading = IIf(Me.eHeading.BodyHtml Is Nothing, "", Me.eHeading.BodyHtml)

                'dto.ItemHeading = Me.txtItemHeading.Text
                dto.ItemHeading = IIf(Me.eItemHeading.BodyHtml Is Nothing, "", Me.eItemHeading.BodyHtml)

                'dto.Verbiage = Me.txtVerbiage.Text
                dto.Verbiage = IIf(Me.eVerbiage.BodyHtml Is Nothing, "", Me.eVerbiage.BodyHtml)

                dto.ImageFile = Me.txtImage.Text
                dto.VideoFile = Me.txtVideoFile.Text

                dto.CertificationTypeID = Me.cbxCertificationTypes.SelectedValue
                dto.ItemTypeID = Me.cbxItemTypes.SelectedValue

                dto.ImageCaption = Me.txtImageCaption.Text
                dto.VideoCaption = Me.txtVideoCaption.Text
                dto.NumberOfQuestions = Me.nudNumberOfQuestions.Value

                If dto.Save() Then
                    msg = "Save Successful."
                    IsTreeDirty = False
                    BuildTree()

                    Me.lblSave.Text = msg
                    Me.lblSave.ForeColor = Color.Green
                    Me.lblSave.Visible = True

                    FindNodeByID(dto.ItemID)
                    Me.tvItems.Select()
                Else
                    Throw New Exception("Error Saving Item")
                End If
            Else
                Throw New Exception("No Item selected.")
            End If
        Catch ex As Exception
            Throw New Exception("SaveTree Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        End Try
    End Sub

#End Region

#Region "Tree Events"

    Private Sub tvItems_BeforeSelect(ByVal sender As Object, ByVal e As LidorSystems.IntegralUI.ObjectCancelEventArgs) Handles tvItems.BeforeSelect
        Try
            Me.Cursor = Cursors.WaitCursor

            Me.lblSave.ForeColor = Color.Black
            Me.lblSave.Visible = False

            If bIsTreeDirty Then
                SaveTree()
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("BeforeSelect Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub tvItems_AfterSelect(ByVal sender As Object, ByVal e As LidorSystems.IntegralUI.ObjectEventArgs) Handles tvItems.AfterSelect
        Try
            Me.Cursor = Cursors.WaitCursor

            iPrevNodeID = CType(Me.tvItems.SelectedNode.Tag, daWSDTO.ItemsDto).ItemID
            sPrevNodeName = CType(Me.tvItems.SelectedNode.Tag, daWSDTO.ItemsDto).Name

            PopulateFields()
            CheckDirectionalButtons()
            CheckItemNodeType()
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("AfterSelect Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not Me.tvItems.SelectedNode Is Nothing Then
                Dim SelectedNode As LidorSystems.IntegralUI.Lists.TreeNode = Me.tvItems.SelectedNode
                Dim SelectedNodeDto As daWSDTO.ItemsDto = New daWSDTO.ItemsDto
                SelectedNodeDto = CType(SelectedNode.Tag, daWSDTO.ItemsDto)

                If MsgBox("Are you sure you want to delete this tree item?", MsgBoxStyle.YesNo, "Delete Item") = DialogResult.Yes Then
                    If SelectedNode.Nodes.Count > 0 Then ' check if node has children
                        If MsgBox("This tree item has sub items.  If you delete this all sub items will be deleted also.  Are you sure you want to continue?", MsgBoxStyle.YesNo, "Delete Item") = DialogResult.Yes Then
                            For kappa As Integer = 0 To SelectedNode.Nodes.Count - 1 Step 1
                                DeleteChildNodes(SelectedNode)
                            Next
                        Else
                            Exit Try
                        End If
                    End If

                    SelectedNodeDto.Active = 0
                    SelectedNodeDto.LastModifiedID = Me.UserID
                    If SelectedNodeDto.Save() Then
                        IsTreeDirty = False
                        SelectedNode.Remove()
                        UpdateDisplaySequence()
                    Else
                        Throw New Exception("Error Saving Data")
                    End If
                End If
            Else
                MsgBox("Please select a tree item to delete.", MsgBoxStyle.OkOnly, "Select an item")
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Delete Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub DeleteChildNodes(ByRef Node As LidorSystems.IntegralUI.Lists.TreeNode)
        Try
            Dim NodeDto As daWSDTO.ItemsDto = New daWSDTO.ItemsDto
            NodeDto = CType(Node.Tag, daWSDTO.ItemsDto)

            NodeDto.Active = 0
            NodeDto.LastModifiedID = Me.UserID

            If Node.Nodes.Count > 0 Then
                For Each cn As LidorSystems.IntegralUI.Lists.TreeNode In Node.Nodes
                    DeleteChildNodes(cn)
                Next
            End If

            If NodeDto.Save() Then
                IsTreeDirty = False
                UpdateDisplaySequence()
            Else
                Throw New Exception("Error Saving Data")
            End If

            BuildTree()
        Catch ex As Exception
            Throw New Exception("Error Deleting Child Nodes: " & ex.Message)
        End Try
    End Sub

#End Region

    Private Sub CheckDirectionalButtons()
        Try
            Dim SelectedNode As LidorSystems.IntegralUI.Lists.TreeNode = Me.tvItems.SelectedNode
            Dim ParentNode As LidorSystems.IntegralUI.Lists.TreeNode
            Dim ParentNodeCount As Integer = 0

            If Not SelectedNode Is Nothing Then
                'up button
                If Not SelectedNode.Parent Is Nothing Then 'has a parent
                    ParentNode = SelectedNode.Parent
                    ParentNodeCount = ParentNode.Nodes.Count

                    If ParentNodeCount <= 1 Then 'if there is less then or equal to 1 child node
                        btnUp.Enabled = False
                    ElseIf SelectedNode.Equals(ParentNode.FirstNode) Then 'if selected node = first node of the parent
                        Me.btnUp.Enabled = False
                    Else
                        btnUp.Enabled = True
                    End If
                Else 'no parent
                    If SelectedNode.Equals(Me.tvItems.Nodes(0)) Then 'if first node
                        btnUp.Enabled = False
                    Else
                        btnUp.Enabled = True
                    End If
                End If

                'down button
                If Not SelectedNode.Parent Is Nothing Then 'has a parent
                    ParentNode = SelectedNode.Parent
                    ParentNodeCount = ParentNode.Nodes.Count

                    If ParentNodeCount <= 1 Then 'if there is less then or equal to 1 child node
                        btnDown.Enabled = False
                    ElseIf SelectedNode.Equals(ParentNode.Nodes(ParentNode.Nodes.Count - 1)) Then
                        btnDown.Enabled = False
                    Else
                        btnDown.Enabled = True
                    End If
                Else 'no parent
                    If SelectedNode.Equals(Me.tvItems.Nodes(Me.tvItems.Nodes.Count - 1)) Then 'if tree's first node
                        btnDown.Enabled = False
                    Else
                        btnDown.Enabled = True
                    End If
                End If

                'left button
                If Not SelectedNode.Parent Is Nothing Then 'has a parent
                    btnLeft.Enabled = True
                Else 'no parent
                    btnLeft.Enabled = False
                End If

                'right button
                If Not SelectedNode.Parent Is Nothing Then 'has a parent
                    ParentNode = SelectedNode.Parent

                    If SelectedNode.Equals(ParentNode.FirstNode) Then 'if parent's first node
                        btnRight.Enabled = False
                    Else
                        btnRight.Enabled = True
                    End If
                Else 'no parent
                    If SelectedNode.Equals(Me.tvItems.Nodes(0)) Then 'if tree's first node
                        btnRight.Enabled = False
                    Else
                        btnRight.Enabled = True
                    End If
                End If

            End If
        Catch ex As Exception
            Throw New Exception("CheckDirectionalButtons Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub PopulateFields()
        Try
            bPopulateFields = True

            Dim iNode As daWSDTO.ItemsDto = CType(Me.tvItems.SelectedNode.Tag, daWSDTO.ItemsDto)
            Me.txtName.Text = iNode.Name
            Me.cbxItemTypes.SelectedValue = iNode.ItemTypeID

            'GRP - 2019.11.05
            'Me.txtHeading.Document.OpenNew(False)
            'Me.txtHeading.Text = iNode.Heading
            Me.eHeading.BodyHtml = iNode.Heading

            'Me.txtItemHeading.Document.OpenNew(False)
            'Me.txtItemHeading.Text = iNode.ItemHeading
            Me.eItemHeading.BodyHtml = iNode.ItemHeading

            'Me.txtVerbiage2.Document.OpenNew(False)
            'Me.txtVerbiage.Text = iNode.Verbiage
            Me.eVerbiage.BodyHtml = iNode.Verbiage

            Me.txtImage.Text = iNode.ImageFile
            Me.txtVideoFile.Text = iNode.VideoFile

            If iNode.ImageFile.Trim.Length > 0 Then
                Me.txtImageURL.Text = My.Settings.URL & "Media/" & Me.cbxCertificationTypes.SelectedValue & "/Images/" & iNode.ImageFile
            Else
                Me.txtImageURL.Text = ""
            End If

            If iNode.VideoFile.Trim.Length > 0 Then
                Me.txtVideoURL.Text = My.Settings.URL & "Media/" & Me.cbxCertificationTypes.SelectedValue & "/Videos/" & iNode.VideoFile
            Else
                Me.txtVideoURL.Text = ""
            End If


            Me.txtImageCaption.Text = iNode.ImageCaption
            Me.txtVideoCaption.Text = iNode.VideoCaption
            Me.nudNumberOfQuestions.Value = iNode.NumberOfQuestions
        Catch ex As Exception
            Throw New Exception("PopulateFields Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            bPopulateFields = False
        End Try
    End Sub

    Private Sub ClearTextFields()
        Try
            bClearFields = True

            Me.txtName.Text = ""
            'Me.txtHeading.Document.OpenNew(False)
            'Me.txtHeading.Text = ""
            Me.eHeading.BodyHtml = ""

            'Me.txtItemHeading.Document.OpenNew(False)
            'Me.txtItemHeading.Text = ""
            Me.eItemHeading.BodyHtml = ""

            'Me.txtVerbiage2.Document.OpenNew(False)
            'Me.txtVerbiage.Text = ""
            Me.eVerbiage.BodyHtml = ""

            Me.txtVideoFile.Text = ""
            Me.txtImage.Text = ""
            Me.txtImageCaption.Text = ""
            Me.txtVideoCaption.Text = ""
            Me.txtImageURL.Text = ""
            Me.txtVideoURL.Text = ""

            Me.nudNumberOfQuestions.Value = 1
        Catch ex As Exception
            Throw New Exception("ClearTextFields Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            bClearFields = False
        End Try
    End Sub

    Private Sub CheckItemNodeType()
        Try
            If Me.cbxItemTypes.Text = "Question" Then
                Me.lblNumberOfQuestions.Visible = True
                Me.nudNumberOfQuestions.Visible = True
            Else
                Me.lblNumberOfQuestions.Visible = False
                Me.nudNumberOfQuestions.Visible = False
            End If
        Catch ex As Exception
            Throw New Exception("CheckItemNodeType Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        End Try
    End Sub

#Region "Add Item/SubItem Methods"

    Private Sub btnAddNode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNode.Click
        Dim bDBIsNothing = dwsdb Is Nothing
        If bDBIsNothing Then dwsdb = New daWSDB

        Try
            Me.Cursor = Cursors.WaitCursor
            Me.tvItems.SuspendUpdate()

            If Me.tvItems.Nodes.Count = 0 Then
                'code to add a tree node if there are no other tree nodes.
                Dim NewNode As LidorSystems.IntegralUI.Lists.TreeNode = New LidorSystems.IntegralUI.Lists.TreeNode
                dto = New daWSDTO.ItemsDto

                'set dummy data to be changed.
                NewNode.Text = "Item"

                dto.ItemID = 0
                dto.CertificationTypeID = Me.cbxCertificationTypes.SelectedValue
                dto.ItemTypeID = Me.cbxItemTypes.SelectedValue
                dto.Heading = ""
                dto.ItemHeading = ""
                dto.Verbiage = ""
                dto.VideoFile = ""
                dto.ImageFile = ""
                dto.Name = NewNode.Text
                dto.ParentID = 0
                dto.Active = 1
                dto.DisplaySequence = 10
                dto.ImageCaption = ""
                dto.VideoCaption = ""
                dto.CreatedByID = Me.UserID
                dto.LastModifiedID = Me.UserID

                If dto.Save() Then 'only add the item if the save to the db was successful
                    Me.tvItems.Nodes.Add(NewNode)
                    IsTreeDirty = False
                Else
                    Throw New Exception("Error Adding Item")
                End If

                BuildTree()
                FindNodeByID(dto.ItemID)
            ElseIf Not Me.tvItems.SelectedNode Is Nothing Then
                Dim SelectedNode As LidorSystems.IntegralUI.Lists.TreeNode = Me.tvItems.SelectedNode
                Dim NewNode As LidorSystems.IntegralUI.Lists.TreeNode = New LidorSystems.IntegralUI.Lists.TreeNode
                dto = New daWSDTO.ItemsDto
                Dim ParentNodeDto As daWSDTO.ItemsDto

                Dim SelectedNodeDto As daWSDTO.ItemsDto = New daWSDTO.ItemsDto
                SelectedNodeDto = CType(SelectedNode.Tag, daWSDTO.ItemsDto)

                ClearTextFields()

                'set dummy data to be changed.
                NewNode.Text = "Item"

                If Not SelectedNode.Parent Is Nothing Then
                    ParentNodeDto = CType(SelectedNode.Parent.Tag, daWSDTO.ItemsDto)
                End If

                dto.ItemID = 0
                dto.CertificationTypeID = Me.cbxCertificationTypes.SelectedValue
                dto.ItemTypeID = Me.cbxItemTypes.SelectedValue
                dto.Name = NewNode.Text
                dto.Heading = ""
                dto.ItemHeading = ""
                dto.Verbiage = ""
                dto.VideoFile = ""
                dto.ImageFile = ""

                If ParentNodeDto Is Nothing Then
                    dto.ParentID = 0
                Else
                    dto.ParentID = ParentNodeDto.ItemID
                End If

                dto.Active = 1
                dto.DisplaySequence = dwsdb.SelectMaxDisplaySequence(Me.cbxCertificationTypes.SelectedValue).Tables(0).Rows(0).Item("MaxDisplaySequence")
                dto.ImageCaption = ""
                dto.VideoCaption = ""

                dto.CreatedByID = Me.UserID
                dto.LastModifiedID = Me.UserID

                If dto.Save() Then 'only add the item if the save to the db was successful
                    SelectedNode.Nodes.Add(NewNode)
                    NewNode.Tag = dto
                    IsTreeDirty = False
                Else
                    Throw New Exception("Error Adding Item")
                End If

                bIsTreeDirty = False

                PopulateFields()
                BuildTree()
                FindNodeByID(dto.ItemID)
            Else
                MsgBox("Please select a tree item to add to.", MsgBoxStyle.OkOnly, "Select an item")
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Add Item Click Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
            Me.tvItems.Select()
            Me.tvItems.ResumeUpdate()
            If bDBIsNothing Then dwsdb = Nothing
        End Try
    End Sub

    Private Sub FindNodeByID(ByVal ID As Integer)
        Try
            Dim bNodeFound As Boolean = False

            For Each n As LidorSystems.IntegralUI.Lists.TreeNode In Me.tvItems.Nodes
                If Not bNodeFound Then
                    bNodeFound = FindChildNodeByID(n, ID)
                    If Not n.Parent Is Nothing Then n.Parent.Expand()
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Error Finding Node by ID: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Function FindChildNodeByID(ByRef n As LidorSystems.IntegralUI.Lists.TreeNode, ByVal ID As Integer) As Boolean
        Try
            Dim dto As daWSDTO.ItemsDto = CType(n.Tag, daWSDTO.ItemsDto)
            Dim retval As Boolean = False

            If dto.ItemID = ID Then
                retval = True
                If Not n.Parent Is Nothing Then n.Parent.Expand()
                n.Selected = True
            Else
                If n.Nodes.Count > 0 Then
                    For Each cn As LidorSystems.IntegralUI.Lists.TreeNode In n.Nodes
                        retval = FindChildNodeByID(cn, ID)
                        If Not cn.Parent Is Nothing Then cn.Parent.Expand()
                    Next
                End If
            End If

            Return retval
        Catch ex As Exception
            Throw New Exception("Error Finding Child Node by ID: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        End Try
    End Function

    Private Sub btnAddSubItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddSubItem.Click
        Dim bDBIsNothing = dwsdb Is Nothing
        If bDBIsNothing Then dwsdb = New daWSDB

        Try
            Me.Cursor = Cursors.WaitCursor
            Me.tvItems.SuspendUpdate()

            If Not Me.tvItems.SelectedNode Is Nothing Then
                Dim SelectedNode As LidorSystems.IntegralUI.Lists.TreeNode = Me.tvItems.SelectedNode
                Dim NewNode As LidorSystems.IntegralUI.Lists.TreeNode = New LidorSystems.IntegralUI.Lists.TreeNode
                dto = New daWSDTO.ItemsDto
                Dim SelectedNodeDto As daWSDTO.ItemsDto

                ClearTextFields()

                'set dummy data to be changed.
                NewNode.Text = "Sub Item"

                SelectedNodeDto = CType(SelectedNode.Tag, daWSDTO.ItemsDto)

                dto.ItemID = 0
                dto.CertificationTypeID = Me.cbxCertificationTypes.SelectedValue
                dto.ItemTypeID = Me.cbxItemTypes.SelectedValue
                dto.Name = NewNode.Text
                dto.Heading = ""
                dto.ItemHeading = ""
                dto.Verbiage = ""
                dto.VideoFile = ""
                dto.ImageFile = ""
                dto.ParentID = SelectedNodeDto.ItemID
                dto.Active = 1
                dto.DisplaySequence = dwsdb.SelectMaxDisplaySequence(Me.cbxCertificationTypes.SelectedValue).Tables(0).Rows(0).Item("MaxDisplaySequence")
                dto.ImageCaption = ""
                dto.VideoCaption = ""
                dto.CreatedByID = Me.UserID
                dto.LastModifiedID = Me.UserID

                If dto.Save() Then 'only add the item if the save to the db was successful
                    SelectedNode.Nodes.Add(NewNode)
                    IsTreeDirty = False
                Else
                    Throw New Exception("Error Adding Item")
                End If

                PopulateFields()
                BuildTree()
                FindNodeByID(dto.ItemID)
            Else
                MsgBox("Please select a tree item to add to.", MsgBoxStyle.OkOnly, "Select an item")
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Add Sub Item Click Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
            Me.tvItems.Select()
            Me.tvItems.ResumeUpdate()
            If bDBIsNothing Then dwsdb = Nothing
        End Try
    End Sub

#End Region

#Region "Tree IsDirty Events"

    Private Sub txtName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtName.TextChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading Then
                IsTreeDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("txtName_TextChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub cbxItemTypes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxItemTypes.SelectedIndexChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading Then
                IsTreeDirty = True
            End If

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("cbxItemTypes_SelectedIndexChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub txtImage_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtImage.TextChanged
        Try
            Me.Cursor = Cursors.WaitCursor
            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading Then
                IsTreeDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("txtImage_TextChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub txtVideoFile_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtVideoFile.TextChanged

        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading Then
                IsTreeDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("txtVideoFile_TextChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#Region "Editor isDirty Events"

    Private Sub txtHeading_ContentsChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bQuestionsGridClick And Not bClearQuestionFields And Not bPopulateQuestionFields Then
                IsTreeDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("txtHeading_ContentsChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub txtItemHeading_ContentsChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bQuestionsGridClick And Not bClearQuestionFields And Not bPopulateQuestionFields Then
                IsTreeDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("txtItemHeading_ContentsChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub txtVerbiage_ContentsChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bQuestionsGridClick And Not bClearQuestionFields And Not bPopulateQuestionFields Then
                IsTreeDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("txtVerbiage_ContentsChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub txtImageCaption_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtImageCaption.TextChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bQuestionsGridClick And Not bClearQuestionFields And Not bPopulateQuestionFields Then
                IsTreeDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("txtImageCaption_TextChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub txtVideoCaption_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtVideoCaption.TextChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bQuestionsGridClick And Not bClearQuestionFields And Not bPopulateQuestionFields Then
                IsTreeDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("txtVideoCaption_TextChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub nudNumberOfQuestions_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudNumberOfQuestions.ValueChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bQuestionsGridClick And Not bClearQuestionFields And Not bPopulateQuestionFields Then
                IsTreeDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("nudNumberOfQuestions_ValueChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#End Region

#Region "Questions IsDirty Events"

    Private Sub txtQuestion_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtQuestion.TextChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bQuestionsGridClick And Not bClearQuestionFields And Not bPopulateQuestionFields Then
                IsQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("txtQuestion_TextChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub cbxNumberOfAnswers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxNumberOfAnswers.SelectedIndexChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bQuestionsGridClick And Not bClearQuestionFields And Not bPopulateQuestionFields Then
                IsQuestionsDirty = True
            End If

            SetAnswerTextBoxes()
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("cbxNumberOfAnswers_SelectedIndexChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub txtAnswer1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAnswer1.TextChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bQuestionsGridClick And Not bClearQuestionFields And Not bPopulateQuestionFields Then
                IsQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("txtAnswer1_TextChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub txtAnswer2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAnswer2.TextChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bQuestionsGridClick And Not bClearQuestionFields And Not bPopulateQuestionFields Then
                IsQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("txtAnswer2_TextChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub txtAnswer3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAnswer3.TextChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bQuestionsGridClick And Not bClearQuestionFields And Not bPopulateQuestionFields Then
                IsQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("txtAnswer3_TextChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub txtAnswer4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAnswer4.TextChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bQuestionsGridClick And Not bClearQuestionFields And Not bPopulateQuestionFields Then
                IsQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("txtAnswer4_TextChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub txtAnswer5_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAnswer5.TextChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bQuestionsGridClick And Not bClearQuestionFields And Not bPopulateQuestionFields Then
                IsQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("txtAnswer5_TextChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub nudQuestionWeight_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudQuestionWeight.ValueChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bQuestionsGridClick And Not bClearQuestionFields And Not bPopulateQuestionFields Then
                IsQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("txtQuestionWeight_TextChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub txtQuestionDisplaySequence_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudDisplaySequence.ValueChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bQuestionsGridClick And Not bClearQuestionFields And Not bPopulateQuestionFields Then
                IsQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("txtQuestionDisplaySequence_TextChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub rbCorrectAnswer1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbCorrectAnswer1.CheckedChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bQuestionsGridClick And Not bClearQuestionFields And Not bPopulateQuestionFields Then
                IsQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("rbCorrectAnswer1_CheckedChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub rbCorrectAnswer2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbCorrectAnswer2.CheckedChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bQuestionsGridClick And Not bClearQuestionFields And Not bPopulateQuestionFields Then
                IsQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("rbCorrectAnswer2_CheckedChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub rbCorrectAnswer3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbCorrectAnswer3.CheckedChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bQuestionsGridClick And Not bClearQuestionFields And Not bPopulateQuestionFields Then
                IsQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("rbCorrectAnswer3_CheckedChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub rbCorrectAnswer4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbCorrectAnswer4.CheckedChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bQuestionsGridClick And Not bClearQuestionFields And Not bPopulateQuestionFields Then
                IsQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("rbCorrectAnswer4_CheckedChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub rbCorrectAnswer5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbCorrectAnswer5.CheckedChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bQuestionsGridClick And Not bClearQuestionFields And Not bPopulateQuestionFields Then
                IsQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("rbCorrectAnswer5_CheckedChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub cbxQuestionItemNode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxQuestionItemNode.SelectedIndexChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bQuestionsGridClick And Not bQuestionsItemComboBox And Not bClearQuestionFields And Not bPopulateQuestionFields Then
                IsQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("cbxQuestionItemNode_SelectedIndexChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub rbActiveTrue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbActiveTrue.CheckedChanged
        Try

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bQuestionsGridClick And Not bClearQuestionFields And Not bPopulateQuestionFields Then
                Me.Cursor = Cursors.WaitCursor
                IsQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("rbActiveTrue_CheckedChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bQuestionsGridClick And Not bClearQuestionFields And Not bPopulateQuestionFields Then
                Me.Cursor = Cursors.Default
            End If
        End Try
    End Sub

    Private Sub rbActiveFalse_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbActiveFalse.CheckedChanged
        Try

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bQuestionsGridClick And Not bClearQuestionFields And Not bPopulateQuestionFields Then
                Me.Cursor = Cursors.WaitCursor
                IsQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("rbActiveFalse_CheckedChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bQuestionsGridClick And Not bClearQuestionFields And Not bPopulateQuestionFields Then
                Me.Cursor = Cursors.Default
            End If
        End Try
    End Sub

#End Region

#Region "Directional Button Events"

    Private Sub btnUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUp.Click
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not Me.tvItems.SelectedNode Is Nothing Then
                Me.tvItems.SuspendUpdate()

                Me.tvItems.SelectedNode.Move(LidorSystems.IntegralUI.Lists.TreeNodeMoveDirection.Up)
                UpdateDisplaySequence()
                BuildTree()

                Me.tvItems.ResumeUpdate()
                Me.tvItems.ExpandAll()
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("btnUp_Click Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDown.Click
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not Me.tvItems.SelectedNode Is Nothing Then
                Me.tvItems.SuspendUpdate()

                Me.tvItems.SelectedNode.Move(LidorSystems.IntegralUI.Lists.TreeNodeMoveDirection.Down)
                UpdateDisplaySequence()
                BuildTree()

                Me.tvItems.ResumeUpdate()
                Me.tvItems.ExpandAll()
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("btnDown_Click Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLeft.Click
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not Me.tvItems.SelectedNode Is Nothing Then
                Me.tvItems.SuspendUpdate()

                Me.tvItems.SelectedNode.Move(LidorSystems.IntegralUI.Lists.TreeNodeMoveDirection.Left)
                UpdateParentID()
                BuildTree()

                Me.tvItems.ResumeUpdate()
                Me.tvItems.ExpandAll()
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("btnLeft_Click Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRight.Click
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not Me.tvItems.SelectedNode Is Nothing Then
                Me.tvItems.SuspendUpdate()

                Me.tvItems.SelectedNode.Move(LidorSystems.IntegralUI.Lists.TreeNodeMoveDirection.Right)
                UpdateParentID()
                BuildTree()

                Me.tvItems.ResumeUpdate()
                Me.tvItems.ExpandAll()
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("btnRight_Click Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region "Update DisplaySequence Methods"

    Private Sub UpdateDisplaySequence()
        Try
            DisplaySequenceVar = 10

            For Each Node As LidorSystems.IntegralUI.Lists.TreeNode In Me.tvItems.Nodes
                Dim NodeDto As daWSDTO.ItemsDto = New daWSDTO.ItemsDto
                NodeDto = CType(Node.Tag, daWSDTO.ItemsDto)

                NodeDto.DisplaySequence = DisplaySequenceVar
                NodeDto.LastModifiedID = Me.UserID

                NodeDto.Save()

                If Node.Nodes.Count > 0 Then
                    For alpha As Integer = 0 To Node.Nodes.Count - 1 Step 1
                        UpdateChildDisplaySequence(Node.Nodes(alpha))
                    Next
                End If

                DisplaySequenceVar += 10
            Next

        Catch ex As Exception
            Throw New Exception("UpdateDisplaySequence Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub UpdateChildDisplaySequence(ByRef ChildNode As LidorSystems.IntegralUI.Lists.TreeNode)
        Try
            Dim ChildNodeDto As daWSDTO.ItemsDto = New daWSDTO.ItemsDto
            ChildNodeDto = CType(ChildNode.Tag, daWSDTO.ItemsDto)
            DisplaySequenceVar += 10

            ChildNodeDto.DisplaySequence = DisplaySequenceVar
            ChildNodeDto.LastModifiedID = Me.UserID

            ChildNodeDto.Save()

            If ChildNode.Nodes.Count > 0 Then
                For beta As Integer = 0 To ChildNode.Nodes.Count - 1 Step 1
                    UpdateChildDisplaySequence(ChildNode.Nodes(beta))
                Next
            End If

        Catch ex As Exception
            Throw New Exception("UpdateChildDisplaySequence Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        End Try
    End Sub

#End Region

#Region "Update ParentID Methods"

    Private Sub UpdateParentID()
        Try
            Dim SelectedNode As LidorSystems.IntegralUI.Lists.TreeNode = New LidorSystems.IntegralUI.Lists.TreeNode
            SelectedNode = Me.tvItems.SelectedNode
            Dim SelectedNodeDto As daWSDTO.ItemsDto = New daWSDTO.ItemsDto
            SelectedNodeDto = CType(SelectedNode.Tag, daWSDTO.ItemsDto)

            Dim ParentNodeDto As daWSDTO.ItemsDto

            If Not SelectedNode.Parent Is Nothing Then
                ParentNodeDto = CType(SelectedNode.Parent.Tag, daWSDTO.ItemsDto)

                SelectedNodeDto.ParentID = ParentNodeDto.ItemID
                SelectedNodeDto.LastModifiedID = Me.UserID

                SelectedNodeDto.Save()
            Else
                SelectedNodeDto.ParentID = 0
                SelectedNodeDto.LastModifiedID = Me.UserID

                SelectedNodeDto.Save()
            End If
        Catch ex As Exception
            Throw New Exception("UpdateParentID Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        End Try
    End Sub

#End Region

#Region "Tool Strip Items"

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub NewCertificationTypeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewCertificationTypeToolStripMenuItem.Click
        Try
            Me.Cursor = Cursors.WaitCursor

            Dim frm As frmNewCertificationType = New frmNewCertificationType(Me.UserID)
            frm.ShowDialog()

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub UploadFilesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UploadFilesToolStripMenuItem.Click
        Try
            Me.Cursor = Cursors.WaitCursor

            Dim frm As frmFTP = New frmFTP(Me.cbxCertificationTypes.SelectedValue)
            frm.ShowDialog()

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("UploadFilesToolStripMenuItem_Click Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub ViewTreeInIEToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewTreeInIEToolStripMenuItem.Click
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not Me.cbxCertificationTypes.SelectedValue Is Nothing Then
                'System.Diagnostics.Process.Start("iexplore.exe", "http://localhost:53920/AdminViewTree.aspx?CertificationTypeID=" & Me.cbxCertificationTypes.SelectedValue)
                System.Diagnostics.Process.Start("iexplore.exe", "http://mesgis.com/MDESSDSCertification/AdminViewTree.aspx?CertificationTypeID=" & Me.cbxCertificationTypes.SelectedValue)
            Else
                MsgBox("Please add a certification to view.", MsgBoxStyle.Critical)
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("ViewTreeInIE Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region "Drag/Drop Events"

    Private Sub tvItems_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles tvItems.DragDrop
        Try
            Me.Cursor = Cursors.WaitCursor

            For Each tn As LidorSystems.IntegralUI.Lists.TreeNode In Me.tvItems.Nodes
                Dim NodeDto As daWSDTO.ItemsDto = New daWSDTO.ItemsDto
                NodeDto = CType(tn.Tag, daWSDTO.ItemsDto)

                If Not tn.Parent Is Nothing Then
                    Dim ParentDto As daWSDTO.ItemsDto = New daWSDTO.ItemsDto
                    ParentDto = CType(tn.Parent.Tag, daWSDTO.ItemsDto)

                    If NodeDto.ParentID <> ParentDto.ItemID Then
                        NodeDto.ParentID = ParentDto.ItemID
                        NodeDto.Save()
                    End If

                    If tn.Nodes.Count > 0 Then
                        For gamma As Integer = 0 To tn.Nodes.Count - 1 Step 1
                            CheckChildNodeParentID(tn.Nodes(gamma))
                        Next
                    End If
                Else
                    If NodeDto.ParentID <> 0 Then
                        NodeDto.ParentID = 0
                        NodeDto.Save()
                    End If

                    If tn.Nodes.Count > 0 Then
                        For gamma As Integer = 0 To tn.Nodes.Count - 1 Step 1
                            CheckChildNodeParentID(tn.Nodes(gamma))
                        Next
                    End If
                End If
            Next

            UpdateDisplaySequence()
            Me.tvItems.ExpandAll()

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("tvItems_DragDrop Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub CheckChildNodeParentID(ByRef ChildNode As LidorSystems.IntegralUI.Lists.TreeNode)
        Try
            Dim ChildNodeDto As daWSDTO.ItemsDto = New daWSDTO.ItemsDto
            ChildNodeDto = CType(ChildNode.Tag, daWSDTO.ItemsDto)

            If Not ChildNode.Parent Is Nothing Then
                Dim ParentDto As daWSDTO.ItemsDto = New daWSDTO.ItemsDto
                ParentDto = CType(ChildNode.Parent.Tag, daWSDTO.ItemsDto)

                If ChildNodeDto.ParentID <> ParentDto.ItemID Then
                    ChildNodeDto.ParentID = ParentDto.ItemID
                    ChildNodeDto.Save()
                End If

                If ChildNode.Nodes.Count > 0 Then
                    For delta As Integer = 0 To ChildNode.Nodes.Count - 1 Step 1
                        CheckChildNodeParentID(ChildNode.Nodes(delta))
                    Next
                End If

                'shouldn't have an else, b/c we were sent here by the parent.

            End If
        Catch ex As Exception
            Throw New Exception("CheckChildNodeParentID Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        End Try
    End Sub

#End Region

    Private Sub cbxCertificationTypes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxCertificationTypes.SelectedIndexChanged
        Try
            Me.UseWaitCursor = True

            'Me.Cursor = Cursors.WaitCursor
            'Me.Refresh()

            'questions tab changes
            ClearQuestionTextFields()
            iDgvRow = 0
            BuildQuestionsData()
            BuildQuestionItemNodeComboBox()
            'tree tab changes
            ClearTextFields()
            BuildTree()
            'follow-up questions tab changes
            ClearFollowUpQuestionTextFields()
            iFollowUpDgvRow = 0
            BuildFollowUpQuestionsData()
            BuildFollowUpQuestionItemNodeComboBox()

            'Reports tab changes
            Me.cbxReports.SelectedIndex = -1

        Catch ex As Exception
            Me.UseWaitCursor = False
            Me.Cursor = Cursors.Default
            l.LogIt("cbxCertificationTypes_SelectedIndexChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.UseWaitCursor = False
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnVideoBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVideoBrowse.Click
        Try
            Dim frm As frmAddFiles = New frmAddFiles(Me.cbxCertificationTypes.SelectedValue, "Videos")
            frm.ShowDialog()
            Me.txtVideoFile.Text = frm.File
            Me.txtVideoURL.Text = My.Settings.URL & "Media/" & Me.cbxCertificationTypes.SelectedValue & "/Videos/" & frm.File

        Catch ex As Exception
            l.LogIt("btnVideoBrowse_Click Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub btnImageBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImageBrowse.Click
        Try
            Dim frm As frmAddFiles = New frmAddFiles(Me.cbxCertificationTypes.SelectedValue, "Images")
            frm.ShowDialog()
            Me.txtImage.Text = frm.File
            Me.txtImageURL.Text = My.Settings.URL & "Media/" & Me.cbxCertificationTypes.SelectedValue & "/Images/" & frm.File

        Catch ex As Exception
            l.LogIt("btnImageBrowse_Click Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        End Try
    End Sub

#Region "Questions Tab Methods"

#Region "Questions Grid Methods"

    Private Sub BuildQuestionsData()
        Try
            l.ConfigureGrid(dgvQuestions, True)

            'grid columns
            Me.dgvQuestions.Columns.Add("QuestionsID", "QuestionsID")
            Me.dgvQuestions.Columns(kQuestionsID).DataPropertyName = "QuestionsID"
            Me.dgvQuestions.Columns(kQuestionsID).Visible = False

            Me.dgvQuestions.Columns.Add("Name", "Name")
            Me.dgvQuestions.Columns(kName).DataPropertyName = "Name"
            Me.dgvQuestions.Columns(kName).Visible = True

            Me.dgvQuestions.Columns.Add("Question", "Question")
            Me.dgvQuestions.Columns(kQuestions).DataPropertyName = "Question"
            Me.dgvQuestions.Columns(kQuestions).Visible = True

            Me.dgvQuestions.Columns.Add("Answer1", "Answer 1")
            Me.dgvQuestions.Columns(kAnswer1).DataPropertyName = "Answer1"
            Me.dgvQuestions.Columns(kAnswer1).Visible = True

            Me.dgvQuestions.Columns.Add("Answer2", "Answer 2")
            Me.dgvQuestions.Columns(kAnswer2).DataPropertyName = "Answer2"
            Me.dgvQuestions.Columns(kAnswer2).Visible = True

            Me.dgvQuestions.Columns.Add("Answer3", "Answer 3")
            Me.dgvQuestions.Columns(kAnswer3).DataPropertyName = "Answer3"
            Me.dgvQuestions.Columns(kAnswer3).Visible = True

            Me.dgvQuestions.Columns.Add("Answer4", "Answer 4")
            Me.dgvQuestions.Columns(kAnswer4).DataPropertyName = "Answer4"
            Me.dgvQuestions.Columns(kAnswer4).Visible = True

            Me.dgvQuestions.Columns.Add("Answer5", "Answer 5")
            Me.dgvQuestions.Columns(kAnswer5).DataPropertyName = "Answer5"
            Me.dgvQuestions.Columns(kAnswer5).Visible = True

            Me.dgvQuestions.Columns.Add("CorrectAnswer", "Correct Answer")
            Me.dgvQuestions.Columns(kCorrectAnswer).DataPropertyName = "CorrectAnswer"
            Me.dgvQuestions.Columns(kCorrectAnswer).Visible = True

            'Me.dgvQuestions.Columns.Add("QuestionWeight", "Question Weight")
            'Me.dgvQuestions.Columns(kQuestionWeight).DataPropertyName = "QuestionWeight"
            'Me.dgvQuestions.Columns(kQuestionWeight).Visible = True

            Me.dgvQuestions.Columns.Add("Active", "Active")
            Me.dgvQuestions.Columns(kActive).DataPropertyName = "Active"
            Me.dgvQuestions.Columns(kActive).Width = 55
            Me.dgvQuestions.Columns(kActive).Visible = True

            'Me.dgvQuestions.Columns.Add("DisplaySequence", "Display Sequence")
            'Me.dgvQuestions.Columns(kDisplaySequence).DataPropertyName = "DisplaySequence"
            'Me.dgvQuestions.Columns(kDisplaySequence).Visible = True

            Me.dgvQuestions.Columns.Add("ItemID", "ItemID")
            Me.dgvQuestions.Columns(kItemID).DataPropertyName = "ItemID"
            Me.dgvQuestions.Columns(kItemID).Visible = False

            Me.dgvQuestions.Columns.Add("NumberOfAnswers", "Number Of Answers")
            Me.dgvQuestions.Columns(kNumberOfAnswers).DataPropertyName = "NumberOfAnswers"
            Me.dgvQuestions.Columns(kNumberOfAnswers).Visible = True

            BindDataGrid()

        Catch ex As Exception
            Throw New Exception("BuildQuestionsData Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub BindDataGrid()
        Dim bDBIsNothing = dwsdb Is Nothing
        If bDBIsNothing Then dwsdb = New daWSDB

        Try
            bLoadingGrid = True

            If cbxCertificationTypes.Items.Count > 0 Then
                Dim ds As DataSet
                ds = dwsdb.SelectAllQuestionsByCertificationTypeID(Me.cbxCertificationTypes.SelectedValue)  '10/25/2013 - GRP  - Modified stored proc so it's only pulling active.  2020.08.13 - Going back to multiples, modified stored proc to use cert id

                Me.dgvQuestions.DataSource = ds.Tables(0)
                Me.dgvQuestions.Refresh()

                If Me.dgvQuestions.Rows.Count > 0 Then
                    PopulateQuestionFields(iDgvRow)
                    Me.dgvQuestions.Rows(iDgvRow).Selected = True
                End If
            End If
        Catch ex As Exception
            Throw New Exception("BindDataGrid Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            If bDBIsNothing Then dwsdb = Nothing
            bLoadingGrid = False
        End Try
    End Sub

    Public Sub SelectDGVRow(ByRef dgv As DataGridView, ByVal Value As Object, ByVal ColumnCompareIndex As Integer, ByVal ColumnSelectIndex As Integer)

        For Each dgvr As DataGridViewRow In dgv.Rows                        'Loop through the rows of the grid
            If dgvr.Cells(ColumnCompareIndex).Value.ToString = Value.ToString Then       'See if we can match the text with the column value
                dgvr.Selected = True                                        'If so, set it selected
                dgv.FirstDisplayedScrollingRowIndex = dgvr.Index            'This scrolls the grid to the row if necessary
                dgv.CurrentCell = dgv.Rows(dgvr.Index).Cells(ColumnSelectIndex)         'This will move row pointer on the grid 
                Exit For
            Else
                dgvr.Selected = False                                       'Else don't select anything
            End If
        Next
    End Sub

#End Region

    Private Sub ClearQuestionTextFields()
        Try
            bClearQuestionFields = True

            Me.txtQuestion.Text = ""
            Me.txtAnswer1.Text = ""
            Me.txtAnswer2.Text = ""
            Me.txtAnswer3.Text = ""
            Me.txtAnswer4.Text = ""
            Me.txtAnswer5.Text = ""
            Me.nudQuestionWeight.Text = 0
            Me.nudDisplaySequence.Text = 0
            Me.rbCorrectAnswer1.Checked = False
            Me.rbCorrectAnswer2.Checked = False
            Me.rbCorrectAnswer3.Checked = False
            Me.rbCorrectAnswer4.Checked = False
            Me.rbCorrectAnswer5.Checked = False
            Me.rbActiveTrue.Checked = True
            Me.rbActiveFalse.Checked = False
            Me.cbxQuestionItemNode.SelectedIndex = -1
            Me.cbxNumberOfAnswers.SelectedIndex = Me.cbxNumberOfAnswers.Items.Count - 1
        Catch ex As Exception
            Throw New Exception("ClearQuestionTextFields Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            bClearQuestionFields = False
        End Try
    End Sub

    Private Sub dgvQuestions_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvQuestions.CellClick
        Try
            If e.RowIndex > -1 Then  'skip the header row
                bQuestionsGridClick = True
                Me.Cursor = Cursors.WaitCursor

                iDgvRow = e.RowIndex

                If IsQuestionsDirty Then
                    If MsgBox("Do you want to save your changes?", MsgBoxStyle.YesNo, "Save?") = MsgBoxResult.Yes Then
                        SaveQuestions()
                    Else
                        IsQuestionsDirty = False
                    End If
                End If

                PopulateQuestionFields(iDgvRow)
                iPrevQuestionsID = Me.QuestionID
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
            bQuestionsGridClick = False
        End Try
    End Sub

    Private Sub PopulateQuestionFields(ByVal RowIndex As Integer)
        Try
            bPopulateQuestionFields = True

            If Me.dgvQuestions.Rows.Count > 0 Then
                Me.QuestionID = Me.dgvQuestions.Rows(RowIndex).Cells(kQuestionsID).Value

                Me.txtQuestion.Text = Me.dgvQuestions.Rows(RowIndex).Cells(kQuestions).Value.ToString
                Me.cbxNumberOfAnswers.Text = Me.dgvQuestions.Rows(RowIndex).Cells(kNumberOfAnswers).Value.ToString
                Me.txtAnswer1.Text = Me.dgvQuestions.Rows(RowIndex).Cells(kAnswer1).Value.ToString
                Me.txtAnswer2.Text = Me.dgvQuestions.Rows(RowIndex).Cells(kAnswer2).Value.ToString
                Me.txtAnswer3.Text = Me.dgvQuestions.Rows(RowIndex).Cells(kAnswer3).Value.ToString
                Me.txtAnswer4.Text = Me.dgvQuestions.Rows(RowIndex).Cells(kAnswer4).Value.ToString
                Me.txtAnswer5.Text = Me.dgvQuestions.Rows(RowIndex).Cells(kAnswer5).Value.ToString
                'Me.nudQuestionWeight.Text = Me.dgvQuestions.Rows(RowIndex).Cells(kQuestionWeight).Value.ToString
                'Me.nudDisplaySequence.Text = Me.dgvQuestions.Rows(RowIndex).Cells(kDisplaySequence).Value.ToString

                If Me.dgvQuestions.Rows(RowIndex).Cells(kCorrectAnswer).Value.ToString = Me.txtAnswer1.Text Then
                    Me.rbCorrectAnswer1.Checked = True
                ElseIf Me.dgvQuestions.Rows(RowIndex).Cells(kCorrectAnswer).Value.ToString = Me.txtAnswer2.Text Then
                    Me.rbCorrectAnswer2.Checked = True
                ElseIf Me.dgvQuestions.Rows(RowIndex).Cells(kCorrectAnswer).Value.ToString = Me.txtAnswer3.Text Then
                    Me.rbCorrectAnswer3.Checked = True
                ElseIf Me.dgvQuestions.Rows(RowIndex).Cells(kCorrectAnswer).Value.ToString = Me.txtAnswer4.Text Then
                    Me.rbCorrectAnswer4.Checked = True
                ElseIf Me.dgvQuestions.Rows(RowIndex).Cells(kCorrectAnswer).Value.ToString = Me.txtAnswer5.Text Then
                    Me.rbCorrectAnswer5.Checked = True
                Else
                    'shouldn't happen
                End If

                Me.cbxQuestionItemNode.SelectedValue = Me.dgvQuestions.Rows(RowIndex).Cells(kItemID).Value.ToString

                If Me.dgvQuestions.Rows(RowIndex).Cells(kActive).Value.ToString = "True" Then
                    Me.rbActiveTrue.Checked = True
                Else
                    Me.rbActiveFalse.Checked = True
                End If
            End If
        Catch ex As Exception
            Throw New Exception("PopulateQuestionFields Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            bPopulateQuestionFields = False
        End Try
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Try
            Me.Cursor = Cursors.WaitCursor

            If Me.TabControl1.SelectedIndex = kQuestionsTab Then
                'on questions tab
                IsQuestionsDirty = True
                Me.QuestionID = 0
                ClearQuestionTextFields()

                '    Me.txtQuestion.Focus()
                'ElseIf Me.TabControl1.SelectedIndex = kFollowUpQuestionsTab Then
                '    'on follow-up questions tab
                '    IsFollowUpQuestionsDirty = True
                '    Me.FollowUpQuestionID = 0
                '    ClearFollowUpQuestionTextFields()

                '    Me.txtFollowUpQuestion.Focus()
            End If

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            Me.Cursor = Cursors.WaitCursor

            If Me.TabControl1.SelectedIndex = kQuestionsTab Then
                'on questions tab
                Me.QuestionID = iPrevQuestionsID
                BindDataGrid()
                IsQuestionsDirty = False
                PopulateQuestionFields(iDgvRow)
            ElseIf Me.TabControl1.SelectedIndex = kTreeTab Then
                'on Tree tab
                IsTreeDirty = False
                Me.tvItems.SelectedNode = Me.tvItems.FindNode(sPrevNodeName)
                BuildTree()
                'ElseIf Me.TabControl1.SelectedIndex = kFollowUpQuestionsTab Then
                '    Me.FollowUpQuestionID = iPrevFollowUpQuestionID
                '    BindFollowUpQuestionsGrid()
                '    IsFollowUpQuestionsDirty = False
                '    PopulateFollowUpQuestionFields(iFollowUpDgvRow)
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#Region "Tab Control Methods"

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        Try
            If Me.TabControl1.SelectedIndex = kQuestionsTab Then
                'on questions tab
                Me.btnNew.Visible = True
                Me.btnSave.Visible = True
                Me.btnCancel.Visible = True
                BuildQuestionItemNodeComboBox()
                PopulateQuestionFields(iDgvRow)

                SetNewButtonBasedOnCertificationTypeID()
            ElseIf Me.TabControl1.SelectedIndex = kTreeTab Then
                'on Tree tab
                Me.btnNew.Visible = False
                Me.btnSave.Visible = True
                Me.btnCancel.Visible = True
            ElseIf Me.TabControl1.SelectedIndex = kReportingTab Then
                'on reporting tab
                Me.btnNew.Visible = False
                Me.btnSave.Visible = False
                Me.btnCancel.Visible = False
                'ElseIf Me.TabControl1.SelectedIndex = kFollowUpQuestionsTab Then
                '    'on Follow-Up Questions Tab
                '    Me.btnNew.Visible = True
                '    Me.btnSave.Visible = True
                '    Me.btnCancel.Visible = True

                '    BuildFollowUpQuestionItemNodeComboBox()
                '    PopulateFollowUpQuestionFields(iFollowUpDgvRow)

                '    SetNewButtonBasedOnCertificationTypeID()
            End If

            Me.lblSave.Text = ""
            Me.lblSave.Visible = False
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub SetNewButtonBasedOnCertificationTypeID()
        Try
            If Me.cbxCertificationTypes.Items.Count > 0 Then
                Me.btnNew.Enabled = True
            Else
                Me.btnNew.Enabled = False
            End If
        Catch ex As Exception
            Throw New Exception("SetNewButtonBasedOnCertificationTypeID Error: " & ex.Message)
        End Try
    End Sub

    Private Sub TabControl1_Selecting(ByVal sender As Object, ByVal e As System.Windows.Forms.TabControlCancelEventArgs) Handles TabControl1.Selecting
        Try
            If IsQuestionsDirty Then
                If MsgBox("Do you want to save your changes?", MsgBoxStyle.YesNo, "Save?") = MsgBoxResult.Yes Then
                    SaveQuestions()
                Else
                    IsQuestionsDirty = False
                End If
            ElseIf IsTreeDirty Then
                If MsgBox("Do you want to save your changes?", MsgBoxStyle.YesNo, "Save?") = MsgBoxResult.Yes Then
                    SaveTree()
                Else
                    IsTreeDirty = False
                End If
            ElseIf IsFollowUpQuestionsDirty Then
                If MsgBox("Do you want to save your changes?", MsgBoxStyle.YesNo, "Save?") = MsgBoxResult.Yes Then
                    SaveFollowUpQuestions()
                Else
                    IsFollowUpQuestionsDirty = False
                End If
            End If
        Catch ex As Exception
            l.LogIt("Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        End Try
    End Sub

#End Region

    Private Sub SetAnswerTextBoxes()
        Try

            Select Case CInt(Me.cbxNumberOfAnswers.Text)
                'Case 1
                'shouldn't happen b/c you should have more than 1 option for an answer - removed 1 from drop down.

                Case 2
                    Me.txtAnswer1.Enabled = True
                    Me.rbCorrectAnswer1.Enabled = True
                    Me.txtAnswer2.Enabled = True
                    Me.rbCorrectAnswer2.Enabled = True

                    If Me.txtAnswer3.Text.Trim.Length > 0 Then
                        Me.txtAnswer3.Text = ""
                    End If

                    If Me.rbCorrectAnswer3.Checked Then
                        Me.rbCorrectAnswer3.Checked = False
                    End If

                    Me.txtAnswer3.Enabled = False
                    Me.rbCorrectAnswer3.Enabled = False

                    If Me.txtAnswer4.Text.Trim.Length > 0 Then
                        Me.txtAnswer4.Text = ""
                    End If

                    If Me.rbCorrectAnswer4.Checked Then
                        Me.rbCorrectAnswer4.Checked = False
                    End If

                    Me.txtAnswer4.Enabled = False
                    Me.rbCorrectAnswer4.Enabled = False

                    If Me.txtAnswer5.Text.Trim.Length > 0 Then
                        Me.txtAnswer5.Text = ""
                    End If

                    If Me.rbCorrectAnswer5.Checked Then
                        Me.rbCorrectAnswer5.Checked = False
                    End If

                    Me.txtAnswer5.Enabled = False
                    Me.rbCorrectAnswer5.Enabled = False
                Case 3
                    Me.txtAnswer1.Enabled = True
                    Me.rbCorrectAnswer1.Enabled = True
                    Me.txtAnswer2.Enabled = True
                    Me.rbCorrectAnswer2.Enabled = True
                    Me.txtAnswer3.Enabled = True
                    Me.rbCorrectAnswer3.Enabled = True

                    If Me.txtAnswer4.Text.Trim.Length > 0 Then
                        Me.txtAnswer4.Text = ""
                    End If

                    Me.txtAnswer4.Enabled = False
                    Me.rbCorrectAnswer4.Enabled = False

                    Me.txtAnswer4.Enabled = False
                    Me.rbCorrectAnswer4.Enabled = False

                    If Me.txtAnswer5.Text.Trim.Length > 0 Then
                        Me.txtAnswer5.Text = ""
                    End If

                    If Me.rbCorrectAnswer5.Checked Then
                        Me.rbCorrectAnswer5.Checked = False
                    End If

                    Me.txtAnswer5.Enabled = False
                    Me.rbCorrectAnswer5.Enabled = False
                Case 4
                    Me.txtAnswer1.Enabled = True
                    Me.rbCorrectAnswer1.Enabled = True
                    Me.txtAnswer2.Enabled = True
                    Me.rbCorrectAnswer2.Enabled = True
                    Me.txtAnswer3.Enabled = True
                    Me.rbCorrectAnswer3.Enabled = True
                    Me.txtAnswer4.Enabled = True
                    Me.rbCorrectAnswer4.Enabled = True

                    If Me.txtAnswer5.Text.Trim.Length > 0 Then
                        Me.txtAnswer5.Text = ""
                    End If

                    If Me.rbCorrectAnswer5.Checked Then
                        Me.rbCorrectAnswer5.Checked = False
                    End If

                    Me.txtAnswer5.Enabled = False
                    Me.rbCorrectAnswer5.Enabled = False
                Case 5
                    Me.txtAnswer1.Enabled = True
                    Me.rbCorrectAnswer1.Enabled = True
                    Me.txtAnswer2.Enabled = True
                    Me.rbCorrectAnswer2.Enabled = True
                    Me.txtAnswer3.Enabled = True
                    Me.rbCorrectAnswer3.Enabled = True
                    Me.txtAnswer4.Enabled = True
                    Me.rbCorrectAnswer4.Enabled = True
                    Me.txtAnswer5.Enabled = True
                    Me.rbCorrectAnswer5.Enabled = True
                Case Else
                    'shouldn't happen
            End Select
        Catch ex As Exception
            Throw New Exception("Error in SetAnswerTextBoxes(): " & ex.Message)
        End Try
    End Sub

    Private Sub SetControls(ByVal value As Boolean)
        Try
            Me.txtName.Enabled = value
            Me.cbxItemTypes.Enabled = value

            'Me.txtHeading.Enabled = value
            Me.eHeading.Enabled = value
            Me.btEditHeading.Enabled = value

            'Me.txtItemHeading.Enabled = value
            Me.eItemHeading.Enabled = value
            Me.btEditItemHeading.Enabled = value

            'Me.txtVerbiage.Enabled = value
            Me.eVerbiage.Enabled = value
            Me.btEditVerbiage.Enabled = value

            Me.txtImage.Enabled = value
            Me.txtVideoFile.Enabled = value
            Me.btnImageBrowse.Enabled = value
            Me.btnVideoBrowse.Enabled = value

            Me.btnAddSubItem.Enabled = value
            Me.btnDelete.Enabled = value
            Me.btnUp.Enabled = value
            Me.btnDown.Enabled = value
            Me.btnLeft.Enabled = value
            Me.btnRight.Enabled = value
        Catch ex As Exception
            Throw New Exception("SetControlsDisabled Error: " & ex.Message)
        End Try
    End Sub

#Region "Reporting Tab Methods"

    Private Sub BuildReportingTab()
        Try
            BindReportComboBox()
            BindTotals()
            BuildReportingGrid()
        Catch ex As Exception
            Throw New Exception("BuildReportingTab Error: " & ex.Message)
        End Try
    End Sub

    Private Sub BindReportComboBox()
        Dim bDBIsNothing = dwsdb Is Nothing
        If bDBIsNothing Then dwsdb = New daWSDB

        Try
            Me.cbxReports.ValueMember = "ReportsID"
            Me.cbxReports.DisplayMember = "ReportName"

            Me.cbxReports.DataSource = dwsdb.SelectAllReports().Tables(0)

            If cbxReports.Items.Count > 0 Then
                Me.cbxReports.SelectedIndex = 0
            End If

        Catch ex As Exception
            Throw New Exception("BindReportComboBox Error: " & ex.Message)
        Finally
            If bDBIsNothing Then dwsdb = Nothing
        End Try
    End Sub

    Private Sub cbxReports_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxReports.SelectedIndexChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            ClearReportingFields()

            Select Case Me.cbxReports.Text
                Case "All", "Passed Only"
                    Me.txtFirstName.Enabled = False
                    Me.txtLastName.Enabled = False
                    Me.txtCertificationNumber.Enabled = False
                    Me.dtpStartDate.Enabled = False
                    Me.dtpEndDate.Enabled = False
                    Me.lblOrderBy.Visible = False
                    Me.rbLastName.Visible = False
                    Me.rbExpirationDate.Visible = False
                    Me.btnPrintGrid.Enabled = False
                    Me.btnToExcel.Enabled = False

                Case "First Name"
                    Me.txtFirstName.Enabled = True
                    Me.txtLastName.Enabled = False
                    Me.txtCertificationNumber.Enabled = False
                    Me.dtpStartDate.Enabled = False
                    Me.dtpEndDate.Enabled = False
                    Me.lblOrderBy.Visible = False
                    Me.rbLastName.Visible = False
                    Me.rbExpirationDate.Visible = False
                    Me.btnPrintGrid.Enabled = False
                    Me.btnToExcel.Enabled = False

                Case "Last Name"
                    Me.txtFirstName.Enabled = False
                    Me.txtLastName.Enabled = True
                    Me.txtCertificationNumber.Enabled = False
                    Me.dtpStartDate.Enabled = False
                    Me.dtpEndDate.Enabled = False
                    Me.lblOrderBy.Visible = False
                    Me.rbLastName.Visible = False
                    Me.rbExpirationDate.Visible = False
                    Me.btnPrintGrid.Enabled = False
                    Me.btnToExcel.Enabled = False

                Case "Certificate Number"
                    Me.txtFirstName.Enabled = False
                    Me.txtLastName.Enabled = False
                    Me.txtCertificationNumber.Enabled = True
                    Me.dtpStartDate.Enabled = False
                    Me.dtpEndDate.Enabled = False
                    Me.lblOrderBy.Visible = False
                    Me.rbLastName.Visible = False
                    Me.rbExpirationDate.Visible = False
                    Me.btnPrintGrid.Enabled = False
                    Me.btnToExcel.Enabled = False

                Case "Date Range"
                    Me.txtFirstName.Enabled = False
                    Me.txtLastName.Enabled = False
                    Me.txtCertificationNumber.Enabled = False
                    Me.dtpStartDate.Enabled = True
                    Me.dtpEndDate.Enabled = True
                    Me.lblOrderBy.Visible = False
                    Me.rbLastName.Visible = False
                    Me.rbExpirationDate.Visible = False
                    Me.btnPrintGrid.Enabled = False
                    Me.btnToExcel.Enabled = False

                Case "Expiring Certificates"
                    Me.txtFirstName.Enabled = False
                    Me.txtLastName.Enabled = False
                    Me.txtCertificationNumber.Enabled = False
                    Me.dtpStartDate.Enabled = True
                    Me.dtpEndDate.Enabled = True
                    Me.lblOrderBy.Visible = True
                    Me.rbLastName.Visible = True
                    Me.rbExpirationDate.Visible = True

                Case Else
                    Me.txtFirstName.Enabled = False
                    Me.txtLastName.Enabled = False
                    Me.txtCertificationNumber.Enabled = False
                    Me.dtpStartDate.Enabled = False
                    Me.dtpEndDate.Enabled = False
                    Me.lblOrderBy.Visible = False
                    Me.rbLastName.Visible = False
                    Me.rbExpirationDate.Visible = False
                    Me.btnRunReport.Enabled = False
                    Me.btnPrintGrid.Enabled = False
                    Me.btnToExcel.Enabled = False

            End Select

            If Me.cbxReports.SelectedIndex = -1 Then
                Me.btnRunReport.Enabled = False
                Me.lblReportToRun.Visible = True

            Else
                Me.btnRunReport.Enabled = True
                Me.lblReportToRun.Visible = False

            End If

            Me.btnPrintGrid.Enabled = False
            Me.btnToExcel.Enabled = False



        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub ClearReportingFields()
        Try
            Me.txtFirstName.Text = ""
            Me.txtLastName.Text = ""
            Me.txtCertificationNumber.Text = ""
        Catch ex As Exception
            Throw New Exception("ClearReportingFields Error: " & ex.Message)
        End Try
    End Sub

    Private Sub BindTotals()
        Dim bDBIsNothing = dwsdb Is Nothing
        If bDBIsNothing Then dwsdb = New daWSDB
        Dim ds As DataSet

        Try
            ds = dwsdb.SelectReportTotals()
            If ds.Tables(0).Rows.Count > 0 Then
                Me.txtTotalExamsTaken.Text = ds.Tables(0).Rows(0).Item("TotalExamCount")
            End If

            If ds.Tables(1).Rows.Count > 0 Then
                Me.txtTotalExamsPassed.Text = ds.Tables(1).Rows(0).Item("TotalExamsPassed")
            End If

            If ds.Tables(2).Rows.Count > 0 Then
                Me.txtTotalExamsFailed.Text = ds.Tables(2).Rows(0).Item("TotalExamsFailed")
            End If

            If ds.Tables(3).Rows.Count > 0 Then
                Me.txtTotalActiveExams.Text = ds.Tables(3).Rows(0).Item("TotalActiveExams")
            End If

        Catch ex As Exception
            Throw New Exception("BindTotals Error: " & ex.Message)
        Finally
            If bDBIsNothing Then dwsdb = Nothing
        End Try
    End Sub

    Private Sub BuildReportingGrid()
        Try
            l.ConfigureGrid(dgvCertResults, True)

            'grid columns
            Me.dgvCertResults.Columns.Add("UserSummaryID", "UserSummaryID")
            Me.dgvCertResults.Columns(kUserSummaryID).DataPropertyName = "UserSummaryID"
            Me.dgvCertResults.Columns(kUserSummaryID).Visible = False

            Me.dgvCertResults.Columns.Add("CertificationTypeID", "CertificationTypeID")
            Me.dgvCertResults.Columns(kCertificationTypeID).DataPropertyName = "CertificationTypeID"
            Me.dgvCertResults.Columns(kCertificationTypeID).Visible = False

            Me.dgvCertResults.Columns.Add("CertificateNumber", "Certificate Number")
            Me.dgvCertResults.Columns(kCertificateNumber).DataPropertyName = "CertificateNumber"
            Me.dgvCertResults.Columns(kCertificateNumber).Visible = True

            Me.dgvCertResults.Columns.Add("CertificationName", "Certification Name")
            Me.dgvCertResults.Columns(kCertificationName).DataPropertyName = "CertificationName"
            Me.dgvCertResults.Columns(kCertificationName).Visible = True

            Me.dgvCertResults.Columns.Add("FullName", "Full Name")
            Me.dgvCertResults.Columns(kFullName).DataPropertyName = "FullName"
            Me.dgvCertResults.Columns(kFullName).Visible = True

            Me.dgvCertResults.Columns.Add("ExamScore", "Exam Score")
            Me.dgvCertResults.Columns(kExamScore).DataPropertyName = "ExamScore"
            Me.dgvCertResults.Columns(kExamScore).Visible = False

            Me.dgvCertResults.Columns.Add("StartDate", "Start Date")
            Me.dgvCertResults.Columns(kStartDate).DataPropertyName = "StartDate"
            Me.dgvCertResults.Columns(kStartDate).Visible = False

            Me.dgvCertResults.Columns.Add("ExamEndDate", "Exam End Date")
            Me.dgvCertResults.Columns(kExamEndDate).DataPropertyName = "ExamEndDate"
            Me.dgvCertResults.Columns(kExamEndDate).Visible = False

            Me.dgvCertResults.Columns.Add("Active", "Active")
            Me.dgvCertResults.Columns(kActiveExam).DataPropertyName = "ActiveExam"
            Me.dgvCertResults.Columns(kActiveExam).Visible = True

            Me.dgvCertResults.Columns.Add("ReviewedMaterial", "Reviewed Material")
            Me.dgvCertResults.Columns(kReviewedMaterial).DataPropertyName = "ReviewedMaterial"
            Me.dgvCertResults.Columns(kReviewedMaterial).Visible = True

            Me.dgvCertResults.Columns.Add("ExpirationDate", "Expiration Date")
            Me.dgvCertResults.Columns(kExpirationDate).DataPropertyName = "ExpirationDate"
            Me.dgvCertResults.Columns(kExpirationDate).Visible = True

            Dim col As DataGridViewButtonColumn = New DataGridViewButtonColumn()
            col.Text = "Print Certificate"
            col.UseColumnTextForButtonValue = True
            col.CellTemplate.Style.BackColor = Me.dgvCertResults.RowHeadersDefaultCellStyle.BackColor
            col.CellTemplate.Style.SelectionBackColor = Me.dgvCertResults.RowHeadersDefaultCellStyle.BackColor
            col.CellTemplate.Style.SelectionForeColor = Me.dgvCertResults.RowHeadersDefaultCellStyle.ForeColor

            Me.dgvCertResults.Columns.Add(col)
        Catch ex As Exception
            Throw New Exception("BuildReportingGrid Error: " & ex.Message)
        End Try
    End Sub

    Private Sub BuildReportingGridExpCerts()
        Try
            Me.dgvCertResults.DataSource = Nothing

            l.ConfigureGrid(dgvCertResults, True)

            'grid columns
            Me.dgvCertResults.Columns.Add("FirstName", "First Name")
            Me.dgvCertResults.Columns(0).DataPropertyName = "FirstName"
            Me.dgvCertResults.Columns(0).Visible = True

            Me.dgvCertResults.Columns.Add("MiddleName", "Middle Name")
            Me.dgvCertResults.Columns(1).DataPropertyName = "MiddleName"
            Me.dgvCertResults.Columns(1).Visible = True

            Me.dgvCertResults.Columns.Add("LastName", "Last Name")
            Me.dgvCertResults.Columns(2).DataPropertyName = "LastName"
            Me.dgvCertResults.Columns(2).Visible = True

            Me.dgvCertResults.Columns.Add("EmailAddress", "Email Address")
            Me.dgvCertResults.Columns(3).DataPropertyName = "EmailAddress"
            Me.dgvCertResults.Columns(3).Visible = True

            Me.dgvCertResults.Columns.Add("CertificationNumber", "Certification Number")
            Me.dgvCertResults.Columns(4).DataPropertyName = "CertificationNumber"
            Me.dgvCertResults.Columns(4).Visible = True

            Me.dgvCertResults.Columns.Add("ExpirationDate", "Expiration Date")
            Me.dgvCertResults.Columns(5).DataPropertyName = "ExpirationDate"
            Me.dgvCertResults.Columns(5).Visible = True

        Catch ex As Exception
            Throw New Exception("BuildReportingGridExpCerts Error: " & ex.Message)
        End Try

    End Sub

    Private Sub btnRunReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRunReport.Click
        Dim bDBIsNothing = dwsdb Is Nothing
        If bDBIsNothing Then dwsdb = New daWSDB
        Dim ds As DataSet

        Try
            Me.Cursor = Cursors.WaitCursor

            Me.lblRowCount.Text = ""

            LastReportRun = Me.cbxReports.Text
            BuildReportingGrid()

            Select Case Me.cbxReports.Text
                Case "All"
                    ds = dwsdb.SelectAllExamsForReporting()
                Case "Passed Only"
                    ds = dwsdb.SelectAllPassedExamsForReporting()
                Case "Failed Only"
                    ds = dwsdb.SelectAllFailedExamsForReporting()
                Case "Active"
                    ds = dwsdb.SelectAllActiveExamsForReporting()
                Case "First Name"
                    If Me.txtFirstName.Text.Trim.Length > 0 Then
                        ds = dwsdb.SelectAllExamsByFirstName(Me.txtFirstName.Text)
                        LastReportParameter = Me.txtFirstName.Text
                    Else
                        MsgBox("You must enter a first name to run this report.")
                    End If
                Case "Last Name"
                    If Me.txtLastName.Text.Trim.Length > 0 Then
                        ds = dwsdb.SelectAllExamsByLastName(Me.txtLastName.Text)
                        LastReportParameter = Me.txtLastName.Text
                    Else
                        MsgBox("You must enter a last name to run this report.")
                    End If
                Case "Date Range"
                    ds = dwsdb.SelectAllExamsByDateRange(FormatDateTime(Me.dtpStartDate.Value, DateFormat.ShortDate), FormatDateTime(Me.dtpEndDate.Value, DateFormat.ShortDate))
                    LastReportStartDate = Me.dtpStartDate.Value
                    LastReportEndDate = Me.dtpEndDate.Value
                Case "Certificate Number"
                    If Me.txtCertificationNumber.Text.Trim.Length > 0 Then
                        If IsNumeric(Me.txtCertificationNumber.Text) Then
                            ds = dwsdb.SelectAllExamsByCertificateNumber(Me.txtCertificationNumber.Text)
                            LastReportParameter = Me.txtCertificationNumber.Text
                        Else
                            MsgBox("Certificate number must be numeric.")
                        End If
                    Else
                        MsgBox("You must enter a certificate number to run this report.")
                    End If
                Case "Expiring Certificates"
                    ' ''Dim frm As frmReport = New frmReport
                    ' ''frm.ReportType = "Expiring Certificates"
                    ' ''frm.CertificationTypeID = Me.cbxCertificationTypes.SelectedValue
                    ' ''frm.StartDate = Me.dtpStartDate.Value
                    ' ''frm.EndDate = Me.dtpEndDate.Value
                    ' ''frm.SortOrder = IIf(Me.rbLastName.Checked, 1, 2)
                    ' ''frm.CertificateName = Me.cbxCertificationTypes.Text

                    ' ''frm.Show()

                    ''LastReportStartDate = Me.dtpStartDate.Value
                    ''LastReportEndDate = Me.dtpEndDate.Value

                    ds = dwsdb.ReportExpiringCertificates(Me.cbxCertificationTypes.SelectedValue, Me.dtpStartDate.Value, Me.dtpEndDate.Value, IIf(Me.rbLastName.Checked, 1, 2))

                    Me.btnPrintGrid.Enabled = True
                    Me.btnToExcel.Enabled = True

                    'Because we don't use the same columns we're writing an exception for expired certificates.
                    BuildReportingGridExpCerts()

                Case Else
                    'shouldn't happen
            End Select

            If Not ds Is Nothing Then
                Me.dgvCertResults.DataSource = ds.Tables(0)
                Me.dgvCertResults.Refresh()
                Me.lblRowCount.Text = "Row Count: " & ds.Tables(0).Rows.Count.ToString
                If ds.Tables(0).Rows.Count > 0 Then
                    Me.btnToExcel.Enabled = True
                End If
            End If

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            If bDBIsNothing Then dwsdb = Nothing
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnPrintGrid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintGrid.Click
        Try
            Me.Cursor = Cursors.WaitCursor

            If LastReportRun <> "" Then

                Dim frm As frmReport = New frmReport

                If LastReportRun = "Expiring Certificates" Then

                    frm.ReportType = "Expiring Certificates"
                    frm.CertificationTypeID = Me.cbxCertificationTypes.SelectedValue
                    frm.StartDate = Me.dtpStartDate.Value
                    frm.EndDate = Me.dtpEndDate.Value
                    frm.SortOrder = IIf(Me.rbLastName.Checked, 1, 2)
                    frm.CertificateName = Me.cbxCertificationTypes.Text

                    frm.Show()

                Else

                    LastReportStartDate = Me.dtpStartDate.Value
                    LastReportEndDate = Me.dtpEndDate.Value
                    frm.ReportType = "Grid Results"
                    frm.LastReportRun = Me.LastReportRun
                    frm.LastReportParameter = Me.LastReportParameter
                    frm.LastReportStartDate = Me.LastReportStartDate
                    frm.LastReportEndDate = Me.LastReportEndDate
                    frm.Show()

                End If

            Else
                MsgBox("You must first run a report to print the grid.")
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnToExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnToExcel.Click

        Dim objReader As StreamWriter

        Try
            Me.Cursor = Cursors.WaitCursor

            If LastReportRun <> "" Then
                Dim sDatePart As String = Now.Year.ToString & MonthName(Now.Month, True) & Now.Day.ToString     'Returns 2013Aug26

                'Get our filename and path
                Dim sFileName As String = LastReportRun & " " & sDatePart & ".csv"
                Me.sfdMain.FileName = sFileName

                If Me.sfdMain.ShowDialog() = DialogResult.OK Then

                    'Dim sPath As String = "D:\bin\Test\"
                    Dim sPath As String = Me.sfdMain.FileName
                    Dim sb As StringBuilder = New StringBuilder
                    Dim sComma As String = ""

                    'Build out the column headers for our csv
                    Dim dt As DataTable = Me.dgvCertResults.DataSource

                    For Each c As DataColumn In dt.Columns
                        sb.Append(sComma & """" & c.ColumnName & """")
                        sComma = ","
                    Next

                    sb.Append(vbCrLf)   'End the line
                    sComma = ""         'Reset the comma

                    'Now grab the rows 
                    For Each dr As DataRow In dt.Rows
                        For i = 0 To dt.Columns.Count - 1                   'Loop through the columns and grab the values - a DataRow doesn't have columns though so we'll treat it as an array.
                            sb.Append(sComma & """" & dr(i).ToString & """")
                            sComma = ","
                        Next
                        sb.Append(vbCrLf)   'End the line
                        sComma = ""         'Reset the comma
                    Next

                    objReader = New StreamWriter(sPath, True)

                    objReader.Write(sb.ToString)
                    objReader.Close()

                End If

            Else
                MsgBox("You must first run a report to export data to Excel.")
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            objReader = Nothing
            Me.Cursor = Cursors.Default
        End Try


        'Try
        '    Me.Cursor = Cursors.WaitCursor

        '    If LastReportRun <> "" Then

        '        Me.sfdMain.FileName = LastReportRun & .xlsx

        '        Dim sPath As String = 

        '        Dim dt As DataTable = Me.dgvQuestions.DataSource

        '        'create the worksheets and save the file
        '        Dim wb As XLWorkbook = New XLWorkbook
        '        wb.Worksheets.Add(dt, LastReportRun)


        '        wb.SaveAs(Path)


        '    Else
        '        MsgBox("You must first run a report to print the grid.")
        '    End If
        'Catch ex As Exception
        '    Me.Cursor = Cursors.Default
        '    l.LogIt("Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        'Finally
        '    Me.Cursor = Cursors.Default
        'End Try
    End Sub

    Private Sub dgvCertResults_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvCertResults.CellContentClick
        Try
            Me.Cursor = Cursors.WaitCursor

            If e.RowIndex > -1 Then
                Select Case e.ColumnIndex
                    Case kPrintCertificate
                        Dim frm As frmReport = New frmReport
                        frm.ReportType = "Certificate"
                        frm.UserSummaryID = CInt(Me.dgvCertResults.Rows(e.RowIndex).Cells(kUserSummaryID).Value)
                        frm.Show()
                        'frm.Hide()
                    Case Else
                End Select
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, MsgBoxStyle.OkOnly, "Error")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region "Follow-Up Questions"

    Private Sub BuildFollowUpQuestionsData()
        Try
            l.ConfigureGrid(dgvFollowUpQuestions, True)

            'grid columns
            Me.dgvFollowUpQuestions.Columns.Add("QuestionsID", "QuestionsID")
            Me.dgvFollowUpQuestions.Columns(kQuestionsID).DataPropertyName = "QuestionsID"
            Me.dgvFollowUpQuestions.Columns(kQuestionsID).Visible = False

            Me.dgvFollowUpQuestions.Columns.Add("Name", "Name")
            Me.dgvFollowUpQuestions.Columns(kName).DataPropertyName = "Name"
            Me.dgvFollowUpQuestions.Columns(kName).Visible = True

            Me.dgvFollowUpQuestions.Columns.Add("Question", "Question")
            Me.dgvFollowUpQuestions.Columns(kQuestions).DataPropertyName = "Question"
            Me.dgvFollowUpQuestions.Columns(kQuestions).Visible = True

            Me.dgvFollowUpQuestions.Columns.Add("Answer1", "Answer 1")
            Me.dgvFollowUpQuestions.Columns(kAnswer1).DataPropertyName = "Answer1"
            Me.dgvFollowUpQuestions.Columns(kAnswer1).Visible = True

            Me.dgvFollowUpQuestions.Columns.Add("Answer2", "Answer 2")
            Me.dgvFollowUpQuestions.Columns(kAnswer2).DataPropertyName = "Answer2"
            Me.dgvFollowUpQuestions.Columns(kAnswer2).Visible = True

            Me.dgvFollowUpQuestions.Columns.Add("Answer3", "Answer 3")
            Me.dgvFollowUpQuestions.Columns(kAnswer3).DataPropertyName = "Answer3"
            Me.dgvFollowUpQuestions.Columns(kAnswer3).Visible = True

            Me.dgvFollowUpQuestions.Columns.Add("Answer4", "Answer 4")
            Me.dgvFollowUpQuestions.Columns(kAnswer4).DataPropertyName = "Answer4"
            Me.dgvFollowUpQuestions.Columns(kAnswer4).Visible = True

            Me.dgvFollowUpQuestions.Columns.Add("Answer5", "Answer 5")
            Me.dgvFollowUpQuestions.Columns(kAnswer5).DataPropertyName = "Answer5"
            Me.dgvFollowUpQuestions.Columns(kAnswer5).Visible = True

            Me.dgvFollowUpQuestions.Columns.Add("CorrectAnswer", "Correct Answer")
            Me.dgvFollowUpQuestions.Columns(kCorrectAnswer).DataPropertyName = "CorrectAnswer"
            Me.dgvFollowUpQuestions.Columns(kCorrectAnswer).Visible = True

            Me.dgvFollowUpQuestions.Columns.Add("Active", "Active")
            Me.dgvFollowUpQuestions.Columns(kActive).DataPropertyName = "Active"
            Me.dgvFollowUpQuestions.Columns(kActive).Width = 55
            Me.dgvFollowUpQuestions.Columns(kActive).Visible = True

            Me.dgvFollowUpQuestions.Columns.Add("ItemID", "ItemID")
            Me.dgvFollowUpQuestions.Columns(kItemID).DataPropertyName = "ItemID"
            Me.dgvFollowUpQuestions.Columns(kItemID).Visible = False

            Me.dgvFollowUpQuestions.Columns.Add("NumberOfAnswers", "Number Of Answers")
            Me.dgvFollowUpQuestions.Columns(kNumberOfAnswers).DataPropertyName = "NumberOfAnswers"
            Me.dgvFollowUpQuestions.Columns(kNumberOfAnswers).Visible = True

            BindFollowUpQuestionsGrid()
        Catch ex As Exception
            Throw New Exception("BuildFollowUpQuestionsData Error: " & ex.Message)
        End Try
    End Sub

    Private Sub BindFollowUpQuestionsGrid()
        Dim bDBIsNothing = dwsdb Is Nothing
        If bDBIsNothing Then dwsdb = New daWSDB

        Try
            If cbxCertificationTypes.Items.Count > 0 Then
                Dim ds As DataSet
                ds = dwsdb.SelectAllFollowUpQuestionsByCertificationTypeID(Me.cbxCertificationTypes.SelectedValue)

                Me.dgvFollowUpQuestions.DataSource = ds.Tables(0)
                Me.dgvFollowUpQuestions.Refresh()

                If Me.dgvFollowUpQuestions.Rows.Count > 0 Then
                    PopulateFollowUpQuestionFields(iFollowUpDgvRow)
                    Me.dgvFollowUpQuestions.Rows(iFollowUpDgvRow).Selected = True
                End If
            End If
        Catch ex As Exception
            Throw New Exception("BindFollowUpQuestionsGrid Error: " & ex.Message)
        Finally
            If bDBIsNothing Then dwsdb = Nothing
        End Try
    End Sub

    Private Sub PopulateFollowUpQuestionFields(ByVal RowIndex As Integer)
        Try
            bPopulateFollowUpQuestionFields = True

            If Me.dgvFollowUpQuestions.Rows.Count > 0 Then
                Me.FollowUpQuestionID = Me.dgvFollowUpQuestions.Rows(RowIndex).Cells(kQuestionsID).Value

                Me.txtFollowUpQuestion.Text = Me.dgvFollowUpQuestions.Rows(RowIndex).Cells(kQuestions).Value
                Me.cbxFollowUpNumberOfAnswers.Text = Me.dgvFollowUpQuestions.Rows(RowIndex).Cells(kNumberOfAnswers).Value.ToString
                Me.txtFollowUpAnswer1.Text = Me.dgvFollowUpQuestions.Rows(RowIndex).Cells(kAnswer1).Value.ToString
                Me.txtFollowUpAnswer2.Text = Me.dgvFollowUpQuestions.Rows(RowIndex).Cells(kAnswer2).Value.ToString
                Me.txtFollowUpAnswer3.Text = Me.dgvFollowUpQuestions.Rows(RowIndex).Cells(kAnswer3).Value.ToString
                Me.txtFollowUpAnswer4.Text = Me.dgvFollowUpQuestions.Rows(RowIndex).Cells(kAnswer4).Value.ToString
                Me.txtFollowUpAnswer5.Text = Me.dgvFollowUpQuestions.Rows(RowIndex).Cells(kAnswer5).Value.ToString

                If Me.dgvFollowUpQuestions.Rows(RowIndex).Cells(kCorrectAnswer).Value.ToString = Me.txtFollowUpAnswer1.Text Then
                    Me.rbFollowUpCorrectAnswer1.Checked = True
                ElseIf Me.dgvFollowUpQuestions.Rows(RowIndex).Cells(kCorrectAnswer).Value.ToString = Me.txtFollowUpAnswer2.Text Then
                    Me.rbFollowUpCorrectAnswer2.Checked = True
                ElseIf Me.dgvFollowUpQuestions.Rows(RowIndex).Cells(kCorrectAnswer).Value.ToString = Me.txtFollowUpAnswer3.Text Then
                    Me.rbFollowUpCorrectAnswer3.Checked = True
                ElseIf Me.dgvFollowUpQuestions.Rows(RowIndex).Cells(kCorrectAnswer).Value.ToString = Me.txtFollowUpAnswer4.Text Then
                    Me.rbFollowUpCorrectAnswer4.Checked = True
                ElseIf Me.dgvFollowUpQuestions.Rows(RowIndex).Cells(kCorrectAnswer).Value.ToString = Me.txtFollowUpAnswer5.Text Then
                    Me.rbFollowUpCorrectAnswer5.Checked = True
                Else
                    'shouldn't happen
                End If

                Me.cbxFollowUpItemType.SelectedValue = Me.dgvFollowUpQuestions.Rows(RowIndex).Cells(kItemID).Value.ToString

                If Me.dgvFollowUpQuestions.Rows(RowIndex).Cells(kActive).Value.ToString = "True" Then
                    Me.rbFollowUpActiveTrue.Checked = True
                Else
                    Me.rbFollowUpActiveFalse.Checked = True
                End If
            End If

        Catch ex As Exception
            Throw New Exception("PopulateFollowUpQuestionFields Error: " & ex.Message)
        Finally
            bPopulateFollowUpQuestionFields = False
        End Try
    End Sub

    Private Sub BuildFollowUpQuestionItemNodeComboBox()
        Dim bDBIsNothing = dwsdb Is Nothing
        If bDBIsNothing Then dwsdb = New daWSDB

        Try

            bFollowUpQuestionsItemComboBox = True
            Dim dt As DataTable = New DataTable()

            'If cbxCertificationTypes.Items.Count > 0 Then
            dt = dwsdb.SelectAllFollowUpQuestionItemNodes(Me.cbxCertificationTypes.SelectedValue).Tables(0)
            'Else
            '    Dim dc As DataColumn = New DataColumn()
            '    dc.ColumnName = "ItemID"
            '    dt.Columns.Add(dc)

            '    dc = New DataColumn()
            '    dc.ColumnName = "Name"
            '    dt.Columns.Add(dc)
            'End If

            'Dim dr As DataRow = dt.NewRow()
            'dr("ItemID") = 0
            'dr("Name") = "Un-Categorized"

            'dt.Rows.InsertAt(dr, 0)

            'With Me.cbxFollowUpItemType
            '    .BindingContext = Me.BindingContext
            '    .DisplayMember = "Name"
            '    .ValueMember = "ItemID"
            '    .DataSource = dt
            '    '.SelectedValue = 0
            'End With

            'bb - 5/11/2012 - removed un-categorized from this, because it was interferring with the query since we have to join to tbItems table which does not have a ItemID of 0
            Me.cbxFollowUpItemType.ValueMember = "ItemID"
            Me.cbxFollowUpItemType.DisplayMember = "Name"
            Me.cbxFollowUpItemType.DataSource = dt

        Catch ex As Exception
            Throw New Exception("BuildFollowUpQuestionItemNodeComboBox Error: " & ex.Message)
        Finally
            If bDBIsNothing Then dwsdb = Nothing
            bFollowUpQuestionsItemComboBox = False
        End Try
    End Sub

    Private Sub ClearFollowUpQuestionTextFields()
        Try
            bClearFollowUpQuestionFields = True

            Me.txtFollowUpQuestion.Text = ""
            Me.txtFollowUpAnswer1.Text = ""
            Me.txtFollowUpAnswer2.Text = ""
            Me.txtFollowUpAnswer3.Text = ""
            Me.txtFollowUpAnswer4.Text = ""
            Me.txtFollowUpAnswer5.Text = ""
            Me.nudFollowUpQuestionWeight.Text = 0
            Me.nudFollowUpDisplaySequence.Text = 0
            Me.rbFollowUpCorrectAnswer1.Checked = False
            Me.rbFollowUpCorrectAnswer2.Checked = False
            Me.rbFollowUpCorrectAnswer3.Checked = False
            Me.rbFollowUpCorrectAnswer4.Checked = False
            Me.rbFollowUpCorrectAnswer5.Checked = False
            Me.rbFollowUpActiveTrue.Checked = True
            Me.rbFollowUpActiveFalse.Checked = False
            Me.cbxFollowUpItemType.SelectedIndex = -1
            Me.cbxFollowUpNumberOfAnswers.SelectedIndex = Me.cbxFollowUpNumberOfAnswers.Items.Count - 1
        Catch ex As Exception
            Throw New Exception("ClearQuestionTextFields Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            bClearFollowUpQuestionFields = False
        End Try
    End Sub

    Private Function ValidateFollowUpQuestionFields() As Boolean
        Try
            Dim ErrMsg As String = ""

            'must have at least 1 correct answer.
            If Not rbFollowUpCorrectAnswer1.Checked Then
                If Not rbFollowUpCorrectAnswer2.Checked Then
                    If Not rbFollowUpCorrectAnswer3.Checked Then
                        If Not rbFollowUpCorrectAnswer4.Checked Then
                            If Not rbFollowUpCorrectAnswer5.Checked Then
                                ErrMsg += "Please select a correct answer." & vbCrLf
                            End If
                        End If
                    End If
                End If
            End If

            'Which ever radio button is checked as the correct answer must have text.
            If rbFollowUpCorrectAnswer1.Checked And Not Me.txtFollowUpAnswer1.Text.Trim.Length > 0 Then
                ErrMsg += "Answer 1 must have text." & vbCrLf
            ElseIf rbFollowUpCorrectAnswer2.Checked And Not Me.txtFollowUpAnswer2.Text.Trim.Length > 0 Then
                ErrMsg += "Answer 2 must have text." & vbCrLf
            ElseIf rbFollowUpCorrectAnswer3.Checked And Not Me.txtFollowUpAnswer3.Text.Trim.Length > 0 Then
                ErrMsg += "Answer 3 must have text." & vbCrLf
            ElseIf rbFollowUpCorrectAnswer4.Checked And Not Me.txtFollowUpAnswer4.Text.Trim.Length > 0 Then
                ErrMsg += "Answer 4 must have text." & vbCrLf
            ElseIf rbFollowUpCorrectAnswer5.Checked And Not Me.txtFollowUpAnswer5.Text.Trim.Length > 0 Then
                ErrMsg += "Answer 5 must have text." & vbCrLf
            End If

            If ErrMsg.Length > 0 Then
                MsgBox(ErrMsg, MsgBoxStyle.Exclamation, "Validation Error")
            End If

            Return ErrMsg.Length = 0
        Catch ex As Exception
            Throw New Exception("Validation Error: " & ex.Message)
        End Try
    End Function

    Private Sub dgvFollowUpQuestions_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvFollowUpQuestions.CellClick
        Try
            Me.Cursor = Cursors.WaitCursor

            If e.RowIndex > -1 Then  'skip the header row
                bFollowUpQuestionsGridClick = True
                Me.Cursor = Cursors.WaitCursor

                iFollowUpDgvRow = e.RowIndex

                If IsFollowUpQuestionsDirty Then
                    If MsgBox("Do you want to save your changes?", MsgBoxStyle.YesNo, "Save?") = MsgBoxResult.Yes Then
                        SaveFollowUpQuestions()
                    Else
                        IsFollowUpQuestionsDirty = False
                    End If
                End If

                PopulateFollowUpQuestionFields(iFollowUpDgvRow)
                iPrevFollowUpQuestionID = Me.FollowUpQuestionID
            End If

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("dgvFollowUpQuestions_CellClick Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            bFollowUpQuestionsGridClick = False
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub SetFollowUpAnswerTextBoxes()
        Try
            Select Case CInt(Me.cbxFollowUpNumberOfAnswers.Text)
                'Case 1
                'shouldn't happen b/c you should have more than 1 option for an answer - removed 1 from drop down.

                Case 2
                    Me.txtFollowUpAnswer1.Enabled = True
                    Me.rbFollowUpCorrectAnswer1.Enabled = True
                    Me.txtFollowUpAnswer2.Enabled = True
                    Me.rbFollowUpCorrectAnswer2.Enabled = True

                    If Me.txtFollowUpAnswer3.Text.Trim.Length > 0 Then
                        Me.txtFollowUpAnswer3.Text = ""
                    End If

                    If Me.rbFollowUpCorrectAnswer3.Checked Then
                        Me.rbFollowUpCorrectAnswer3.Checked = False
                    End If

                    Me.txtFollowUpAnswer3.Enabled = False
                    Me.rbFollowUpCorrectAnswer3.Enabled = False

                    If Me.txtFollowUpAnswer4.Text.Trim.Length > 0 Then
                        Me.txtFollowUpAnswer4.Text = ""
                    End If

                    If Me.rbFollowUpCorrectAnswer4.Checked Then
                        Me.rbFollowUpCorrectAnswer4.Checked = False
                    End If

                    Me.txtFollowUpAnswer4.Enabled = False
                    Me.rbFollowUpCorrectAnswer4.Enabled = False

                    If Me.txtFollowUpAnswer5.Text.Trim.Length > 0 Then
                        Me.txtFollowUpAnswer5.Text = ""
                    End If

                    If Me.rbFollowUpCorrectAnswer5.Checked Then
                        Me.rbFollowUpCorrectAnswer5.Checked = False
                    End If

                    Me.txtFollowUpAnswer5.Enabled = False
                    Me.rbFollowUpCorrectAnswer5.Enabled = False
                Case 3
                    Me.txtFollowUpAnswer1.Enabled = True
                    Me.rbFollowUpCorrectAnswer1.Enabled = True
                    Me.txtFollowUpAnswer2.Enabled = True
                    Me.rbFollowUpCorrectAnswer2.Enabled = True
                    Me.txtFollowUpAnswer3.Enabled = True
                    Me.rbFollowUpCorrectAnswer3.Enabled = True

                    If Me.txtFollowUpAnswer4.Text.Trim.Length > 0 Then
                        Me.txtFollowUpAnswer4.Text = ""
                    End If

                    Me.txtFollowUpAnswer4.Enabled = False
                    Me.rbFollowUpCorrectAnswer4.Enabled = False

                    Me.txtFollowUpAnswer4.Enabled = False
                    Me.rbFollowUpCorrectAnswer4.Enabled = False

                    If Me.txtFollowUpAnswer5.Text.Trim.Length > 0 Then
                        Me.txtFollowUpAnswer5.Text = ""
                    End If

                    If Me.rbFollowUpCorrectAnswer5.Checked Then
                        Me.rbFollowUpCorrectAnswer5.Checked = False
                    End If

                    Me.txtFollowUpAnswer5.Enabled = False
                    Me.rbFollowUpCorrectAnswer5.Enabled = False
                Case 4
                    Me.txtFollowUpAnswer1.Enabled = True
                    Me.rbFollowUpCorrectAnswer1.Enabled = True
                    Me.txtFollowUpAnswer2.Enabled = True
                    Me.rbFollowUpCorrectAnswer2.Enabled = True
                    Me.txtFollowUpAnswer3.Enabled = True
                    Me.rbFollowUpCorrectAnswer3.Enabled = True
                    Me.txtFollowUpAnswer4.Enabled = True
                    Me.rbFollowUpCorrectAnswer4.Enabled = True

                    If Me.txtFollowUpAnswer5.Text.Trim.Length > 0 Then
                        Me.txtFollowUpAnswer5.Text = ""
                    End If

                    If Me.rbFollowUpCorrectAnswer5.Checked Then
                        Me.rbFollowUpCorrectAnswer5.Checked = False
                    End If

                    Me.txtFollowUpAnswer5.Enabled = False
                    Me.rbFollowUpCorrectAnswer5.Enabled = False
                Case 5
                    Me.txtFollowUpAnswer1.Enabled = True
                    Me.rbFollowUpCorrectAnswer1.Enabled = True
                    Me.txtFollowUpAnswer2.Enabled = True
                    Me.rbFollowUpCorrectAnswer2.Enabled = True
                    Me.txtFollowUpAnswer3.Enabled = True
                    Me.rbFollowUpCorrectAnswer3.Enabled = True
                    Me.txtFollowUpAnswer4.Enabled = True
                    Me.rbFollowUpCorrectAnswer4.Enabled = True
                    Me.txtFollowUpAnswer5.Enabled = True
                    Me.rbFollowUpCorrectAnswer5.Enabled = True
                Case Else
                    'shouldn't happen
            End Select
        Catch ex As Exception
            Throw New Exception("SetFollowUpAnswerTextBoxes Error: " & ex.Message)
        End Try
    End Sub

#Region "Follow-Up Question IsDirty Events"

    Private Sub txtFollowUpQuestion_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFollowUpQuestion.TextChanged
        Try

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.WaitCursor
                IsFollowUpQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("txtFollowUpQuestion_TextChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.Default
            End If
        End Try
    End Sub

    Private Sub cbxFollowUpItemType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxFollowUpItemType.SelectedIndexChanged
        Try

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields And Not bFollowUpQuestionsItemComboBox Then
                Me.Cursor = Cursors.WaitCursor
                IsFollowUpQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("cbxFollowUpItemType_SelectedIndexChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields And Not bFollowUpQuestionsItemComboBox Then
                Me.Cursor = Cursors.Default
            End If
        End Try
    End Sub

    Private Sub cbxFollowUpNumberOfAnswers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxFollowUpNumberOfAnswers.SelectedIndexChanged
        Try

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.WaitCursor
                IsFollowUpQuestionsDirty = True
            End If

            SetFollowUpAnswerTextBoxes()
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("cbxFollowUpNumberOfAnswers_SelectedIndexChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.Default
            End If
        End Try
    End Sub

    Private Sub rbFollowUpActiveTrue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbFollowUpActiveTrue.CheckedChanged
        Try

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.WaitCursor
                IsFollowUpQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("rbFollowUpActiveTrue_CheckedChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.Default
            End If
        End Try
    End Sub

    Private Sub rbFollowUpActiveFalse_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbFollowUpActiveFalse.CheckedChanged
        Try

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.WaitCursor
                IsFollowUpQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("rbFollowUpActiveFalse_CheckedChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.Default
            End If
        End Try
    End Sub

    Private Sub txtFollowUpAnswer1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFollowUpAnswer1.TextChanged
        Try

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.WaitCursor
                IsFollowUpQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("txtFollowUpAnswer1_TextChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.Default
            End If
        End Try
    End Sub

    Private Sub txtFollowUpAnswer2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFollowUpAnswer2.TextChanged
        Try

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.WaitCursor
                IsFollowUpQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("txtFollowUpAnswer2_TextChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.Default
            End If
        End Try
    End Sub

    Private Sub txtFollowUpAnswer3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFollowUpAnswer3.TextChanged
        Try

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.WaitCursor
                IsFollowUpQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("txtFollowUpAnswer3_TextChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.Default
            End If
        End Try
    End Sub

    Private Sub txtFollowUpAnswer4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFollowUpAnswer4.TextChanged
        Try

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.WaitCursor
                IsFollowUpQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("txtFollowUpAnswer4_TextChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.Default
            End If
        End Try
    End Sub

    Private Sub txtFollowUpAnswer5_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFollowUpAnswer5.TextChanged
        Try

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.WaitCursor
                IsFollowUpQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("txtFollowUpAnswer5_TextChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.Default
            End If
        End Try
    End Sub

    Private Sub rbFollowUpCorrectAnswer1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbFollowUpCorrectAnswer1.CheckedChanged
        Try

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.WaitCursor
                IsFollowUpQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("rbFollowUpCorrectAnswer1_CheckedChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.Default
            End If
        End Try
    End Sub

    Private Sub rbFollowUpCorrectAnswer2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbFollowUpCorrectAnswer2.CheckedChanged
        Try

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.WaitCursor
                IsFollowUpQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("rbFollowUpCorrectAnswer2_CheckedChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.Default
            End If
        End Try
    End Sub

    Private Sub rbFollowUpCorrectAnswer3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbFollowUpCorrectAnswer3.CheckedChanged
        Try

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.WaitCursor
                IsFollowUpQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("rbFollowUpCorrectAnswer3_CheckedChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.Default
            End If
        End Try
    End Sub

    Private Sub rbFollowUpCorrectAnswer4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbFollowUpCorrectAnswer4.CheckedChanged
        Try

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.WaitCursor
                IsFollowUpQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("rbFollowUpCorrectAnswer4_CheckedChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.Default
            End If
        End Try
    End Sub

    Private Sub rbFollowUpCorrectAnswer5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbFollowUpCorrectAnswer5.CheckedChanged
        Try

            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.WaitCursor
                IsFollowUpQuestionsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("rbFollowUpCorrectAnswer5_CheckedChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Error")
        Finally
            If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading And Not bFollowUpQuestionsGridClick And Not bClearFollowUpQuestionFields And Not bPopulateFollowUpQuestionFields Then
                Me.Cursor = Cursors.Default
            End If
        End Try
    End Sub

#End Region

#End Region


    Private Sub btEditHeading_Click(sender As System.Object, e As System.EventArgs) Handles btEditHeading.Click

        Dim frme As frmEditText = New frmEditText
        frme.Text = "Edit Heading"
        frme.txtEditText.Text = eHeading.BodyHtml
        frme.ShowDialog()

        If frme.Save Then
            eHeading.BodyHtml = frme.txtEditText.Text
            IsTreeDirty = True
        End If


        'Dim frm As frmEnterText = New frmEnterText
        ''Dim f As TextInsertForm = New TextInsertForm(txtHeading.Text)

        ''f.ShowDialog()


        ''frm.sBodyText = txtItemHeading.BodyText
        ''frm.txtBodyHeading.Text = txtBodyHeading.BodyHtml
        ''frm.sDocumentText = Me.txtItemHeading.DocumentText
        'frm.Text = "Edit Heading"
        ''frm.txtBodyHeading.Text = txtHeading.Text.Replace("<br>", "")
        'frm.ShowDialog()

        'Dim s As String = frm.txtBodyHeading.Text
        's = s.Replace("EN""><br><HTML>", "EN""><HTML>")

        'txtHeading.Text = s

        'txtHeading.Text = txtHeading.Text.Replace("<br>", "")


        ''txtItemHeading.BodyHtml = frm.sBodyHTML
        ''Me.txtHeading.DocumentText = frm.sDocumentText



    End Sub

    Private Sub btEditItemHeading_Click(sender As System.Object, e As System.EventArgs) Handles btEditItemHeading.Click

        Dim frme As frmEditText = New frmEditText
        frme.Text = "Edit Item Heading"
        frme.txtEditText.Text = eItemHeading.BodyHtml
        frme.ShowDialog()

        If frme.Save Then
            eItemHeading.BodyHtml = frme.txtEditText.Text
            IsTreeDirty = True
        End If


        'Dim frm As frmEnterText = New frmEnterText
        ''frm.sBodyText = txtItemHeading.BodyText
        ''frm.txtBodyHeading.Text = txtBodyHeading.BodyHtml
        ''frm.sDocumentText = Me.txtItemHeading.DocumentText
        'frm.Text = "Edit Item Heading"
        'frm.ShowDialog()


        'txtItemHeading.Text = frm.txtBodyHeading.Text


        ''txtItemHeading.BodyHtml = frm.sBodyHTML
        ''Me.txtHeading.DocumentText = frm.sDocumentText
    End Sub

    Private Sub btEditVerbiage_Click(sender As System.Object, e As System.EventArgs) Handles btEditVerbiage.Click
        Dim frme As frmEditText = New frmEditText
        frme.Text = "Edit Verbiage"
        frme.txtEditText.Text = eVerbiage.BodyHtml
        frme.ShowDialog()

        If frme.Save Then
            eVerbiage.BodyHtml = frme.txtEditText.Text
            IsTreeDirty = True
        End If



        'Dim frm As frmEnterText = New frmEnterText
        ''frm.sBodyText = txtItemHeading.BodyText
        ''frm.txtBodyHeading.Text = txtBodyHeading.BodyHtml
        ''frm.sDocumentText = Me.txtItemHeading.DocumentText
        'frm.Text = "Edit Verbiage"
        'frm.ShowDialog()


        'txtVerbiage.Text = frm.txtBodyHeading.Text


        'txtItemHeading.BodyHtml = frm.sBodyHTML
        'Me.txtHeading.DocumentText = frm.sDocumentText
    End Sub

    Private Sub eHeading_BoldChanged(sender As Object, e As System.EventArgs) Handles eHeading.BoldChanged, eHeading.FontChanged, eHeading.FontSizeChanged, eHeading.ForeColorChanged
        If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading Then
            IsTreeDirty = True
        End If
    End Sub

    Private Sub eHeading_IndentChanged(sender As Object, e As System.EventArgs) Handles eHeading.IndentChanged, eHeading.ItalicChanged, eHeading.UnderlineChanged
        If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading Then
            IsTreeDirty = True
        End If
    End Sub

    Private Sub eHeading_JustifyLeftChanged(sender As Object, e As System.EventArgs) Handles eHeading.JustifyFullChanged, eHeading.JustifyLeftChanged, eHeading.JustifyRightChanged
        If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading Then
            IsTreeDirty = True
        End If

    End Sub


    Private Sub eItemHeading_BoldChanged(sender As Object, e As System.EventArgs) Handles eItemHeading.BoldChanged, eItemHeading.FontChanged, eItemHeading.FontSizeChanged, eItemHeading.ForeColorChanged
        If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading Then
            IsTreeDirty = True
        End If
    End Sub

    Private Sub eItemHeading_IndentChanged(sender As Object, e As System.EventArgs) Handles eItemHeading.IndentChanged, eItemHeading.ItalicChanged, eItemHeading.UnderlineChanged
        If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading Then
            IsTreeDirty = True
        End If
    End Sub

    Private Sub eItemHeading_JustifyLeftChanged(sender As Object, e As System.EventArgs) Handles eItemHeading.JustifyFullChanged, eItemHeading.JustifyLeftChanged, eItemHeading.JustifyRightChanged
        If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading Then
            IsTreeDirty = True
        End If

    End Sub

    Private Sub eVerbiage_BoldChanged(sender As Object, e As System.EventArgs) Handles eVerbiage.BoldChanged, eVerbiage.FontChanged, eVerbiage.FontSizeChanged, eVerbiage.ForeColorChanged
        If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading Then
            IsTreeDirty = True
        End If
    End Sub

    Private Sub eVerbiage_IndentChanged(sender As Object, e As System.EventArgs) Handles eVerbiage.IndentChanged, eVerbiage.ItalicChanged, eVerbiage.UnderlineChanged
        If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading Then
            IsTreeDirty = True
        End If
    End Sub

    Private Sub eVerbiage_JustifyLeftChanged(sender As Object, e As System.EventArgs) Handles eVerbiage.JustifyFullChanged, eVerbiage.JustifyLeftChanged, eVerbiage.JustifyRightChanged
        If Not bFormLoading And Not bClearFields And Not bPopulateFields And Not bComboBoxLoading Then
            IsTreeDirty = True
        End If

    End Sub

    Private Sub SwitchWebSiteToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SwitchWebSiteToolStripMenuItem.Click
        'MsgBox("Ability to switch between the Training Web Sites." & vbCrLf & "Coming Soon!", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "Switch Web Site")

        Dim frm As New frmChangeSite

        frm.ShowDialog()

        LoadForm()


    End Sub
End Class
