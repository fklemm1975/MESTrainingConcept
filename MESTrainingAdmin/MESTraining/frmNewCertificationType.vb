Imports CertDB

Public Class frmNewCertificationType

#Region "Properties"

    Private dwsdb As daWSDB
    Private bIsDirty As Boolean = False
    Private bFormLoading As Boolean = False
    Private bPopulateFields As Boolean = False
    Private iPrevCertificationID As Integer = 0
    Private CertificationTypeID As Integer = 0
    Private l As CertLib = New CertLib
    Private UserID As Integer = 0
    Private CertificationUserID As Integer = 0

    Public Property IsDirty()
        Get
            IsDirty = bIsDirty
        End Get
        Set(ByVal value)
            bIsDirty = value

            If Not bFormLoading And Not bPopulateFields Then
                Me.btnNew.Enabled = Not value
                Me.lvCertificationType.Enabled = Not value
                Me.btnSortAlphabetically.Enabled = Not value

                Me.btnSave.Enabled = value
                Me.btnCancel.Enabled = value
            End If
        End Set
    End Property

    Private Const kCertificationName As Integer = 0
    Private Const kPassingScore As Integer = 1
    Private Const kDisplaySequence As Integer = 2
    Private Const kCertificationTypeID As Integer = 3
    Private Const kActive As Integer = 4
    Private Const kCertificationUserID As Integer = 5
    Private Const kLengthOfCertification As Integer = 6

#End Region

#Region "Form Events"

    Private Sub frmNewCertificationType_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            Me.Cursor = Cursors.WaitCursor
            'add these 2 lines to stop Design.cs from erroring out.
            'Me.txtPassingVerbiage.Dispose()
            'Me.txtFailingVerbiage.Dispose()
            ePassingVerbiage.Dispose()
            eFailingVerbiage.Dispose()


            'rebuild main forms combobox, to update any items that may have been added or changed.
            frmMain.BuildComboBox()
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Closing Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub frmNewCertificationType_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim bDBIsNothing = dwsdb Is Nothing
        If bDBIsNothing Then dwsdb = New daWSDB

        Try
            bFormLoading = True
            Me.Cursor = Cursors.WaitCursor

            BindListView()

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Load Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            Me.Cursor = Cursors.Default
            If bDBIsNothing Then dwsdb = Nothing
            bFormLoading = False
        End Try
    End Sub

#End Region

    Private Sub BindListView()
        Dim bDBIsNothing = dwsdb Is Nothing
        If bDBIsNothing Then dwsdb = New daWSDB

        Try
            Dim ds As DataSet
            Dim lvi As ListViewItem
            ds = dwsdb.SelectAllCertificationType(Me.UserID)

            Me.lvCertificationType.Items.Clear()

            Me.lvCertificationType.Columns.Add("Certification Type", 240)
            Me.lvCertificationType.View = View.Details
            Me.lvCertificationType.LabelEdit = False
            Me.lvCertificationType.CheckBoxes = False
            Me.lvCertificationType.FullRowSelect = True
            Me.lvCertificationType.GridLines = True
            Me.lvCertificationType.MultiSelect = False

            Me.lvCertificationType.Columns.Add("Passing Score", 100)
            Me.lvCertificationType.View = View.Details
            Me.lvCertificationType.LabelEdit = False
            Me.lvCertificationType.CheckBoxes = False
            Me.lvCertificationType.FullRowSelect = True
            Me.lvCertificationType.GridLines = True
            Me.lvCertificationType.MultiSelect = False

            Me.lvCertificationType.Columns.Add("Display Sequence", 100)
            Me.lvCertificationType.View = View.Details
            Me.lvCertificationType.LabelEdit = False
            Me.lvCertificationType.CheckBoxes = False
            Me.lvCertificationType.FullRowSelect = True
            Me.lvCertificationType.GridLines = True
            Me.lvCertificationType.MultiSelect = False

            For Each dr As DataRow In ds.Tables(0).Rows
                lvi = New ListViewItem(dr("CertificationName").ToString)
                lvi.SubItems.Add(dr("PassingScore").ToString)
                lvi.SubItems.Add(dr("DisplaySequence").ToString)
                lvi.SubItems.Add(dr("CertificationTypeID").ToString)
                lvi.SubItems.Add(dr("Active").ToString)
                lvi.SubItems.Add(dr("CertificationUserID").ToString)
                lvi.SubItems.Add(dr("LengthOfCertification").ToString)

                Me.lvCertificationType.Items.Add(lvi)
            Next

            If Me.lvCertificationType.Items.Count > 0 Then
                Me.lvCertificationType.Items(0).Selected = True
            End If
        Catch ex As Exception
            Throw New Exception("Bind List View Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            If bDBIsNothing Then dwsdb = Nothing
        End Try
    End Sub

#Region "Button Events"

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Try
            Me.Cursor = Cursors.WaitCursor

            ClearFields()
            Me.CertificationTypeID = 0
            Me.CertificationUserID = 0
            Me.cbxPublish.Enabled = False
            Me.txtCertificationName.Focus()
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim bDBIsNothing = dwsdb Is Nothing
        If bDBIsNothing Then dwsdb = New daWSDB

        Try
            Me.Cursor = Cursors.WaitCursor

            Dim dto As daWSDTO.CertificationTypeDto = New daWSDTO.CertificationTypeDto
            dto.CertificationTypeID = Me.CertificationTypeID
            dto.CertificationName = Me.txtCertificationName.Text
            If Me.cbxActive.Checked Then
                dto.Active = True
            Else
                dto.Active = False
            End If
            dto.DisplaySequence = Me.nudDisplaySequence.Value
            If Me.cbxPublish.Checked Then
                'check if the certification has items and questions.
                If SelectTotalItemsCountAndQuestionsCount() Then
                    dto.Publish = True
                Else
                    dto.Publish = False
                End If
            Else
                dto.Publish = False
            End If
            dto.PassingScore = Me.nudPassingScore.Value
            dto.LengthOfCertification = Me.nudLengthOfCertification.Value

            'dto.PassingVerbiage = Me.txtPassingVerbiage.DocumentText
            dto.PassingVerbiage = ePassingVerbiage.BodyHtml

            'dto.FailingVerbiage = Me.txtFailingVerbiage.DocumentText
            dto.FailingVerbiage = eFailingVerbiage.BodyHtml

            dto.CreatedByID = Me.UserID
            dto.LastModifiedID = Me.UserID
            dto.UserID = Me.UserID

            If dto.Save() Then
                IsDirty = False
                'only create the ftp directories when it's a new certification
                If Me.CertificationTypeID = 0 Then
                    CreateFTPDirectory(dto.CertificationTypeID)
                End If

                Me.Close()
            Else
                Throw New Exception("Error Saving CertificationType.")
            End If

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message, True, "Save Command Failed")

        Finally
            Me.Cursor = Cursors.Default
            If bDBIsNothing Then dwsdb = Nothing
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            Me.Cursor = Cursors.WaitCursor

            PopulateFields(iPrevCertificationID)
            Me.cbxPublish.Enabled = True
            IsDirty = False
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnSortAlphabetically_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSortAlphabetically.Click
        Dim bDBIsNothing = dwsdb Is Nothing
        If bDBIsNothing Then dwsdb = New daWSDB

        Try
            Me.Cursor = Cursors.WaitCursor
            Dim ds As DataSet = dwsdb.SelectCertificationTypeAlphabetically()

            Me.lvCertificationType.Clear()

            BindListView()
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            If bDBIsNothing Then dwsdb = Nothing
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

    Private Sub lvCertificationType_ItemSelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs) Handles lvCertificationType.ItemSelectionChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            PopulateFields()
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub PopulateFields(Optional ByVal i As Integer = 0)
        Try
            bPopulateFields = True

            Dim dto As daWSDTO.CertificationTypeDto = New daWSDTO.CertificationTypeDto

            If i = 0 Then
                Me.CertificationTypeID = GetSelectedItem()
            Else
                Me.CertificationTypeID = i
            End If

            iPrevCertificationID = Me.CertificationTypeID
            dto.Populate(Me.CertificationTypeID)

            Me.txtCertificationName.Text = dto.CertificationName
            If dto.Active Then
                Me.cbxActive.Checked = True
            Else
                Me.cbxActive.Checked = False
            End If
            Me.nudDisplaySequence.Value = dto.DisplaySequence
            If dto.Publish Then
                Me.cbxPublish.Checked = True
            Else
                Me.cbxPublish.Checked = False
            End If
            Me.nudPassingScore.Value = dto.PassingScore
            Me.nudLengthOfCertification.Value = dto.LengthOfCertification

            'Me.txtPassingVerbiage.Document.OpenNew(False)
            'Me.txtPassingVerbiage.DocumentText = dto.PassingVerbiage
            Me.ePassingVerbiage.BodyHtml = dto.PassingVerbiage

            'Me.txtFailingVerbiage.Document.OpenNew(False)
            'Me.txtFailingVerbiage.DocumentText = dto.FailingVerbiage
            Me.eFailingVerbiage.BodyHtml = dto.FailingVerbiage

        Catch ex As Exception
            Throw New Exception("Error Populating Fields.")
        Finally
            bPopulateFields = False
        End Try
    End Sub

    Private Function GetSelectedItem() As Integer
        Dim retval As Integer

        For Each item As ListViewItem In Me.lvCertificationType.SelectedItems
            retval = item.SubItems(kCertificationTypeID).Text
        Next

        Return retval
    End Function

    Private Sub ClearFields()
        Try
            Me.txtCertificationName.Text = ""
            Me.cbxActive.Checked = False
            Me.nudDisplaySequence.Value = GetMaxDisplaySequence() + 10
            Me.cbxPublish.Checked = False
            Me.nudPassingScore.Value = 0
            Me.nudLengthOfCertification.Value = 0

            'Me.txtPassingVerbiage.Document.OpenNew(False)
            'Me.txtPassingVerbiage.DocumentText = ""
            ePassingVerbiage.BodyHtml = ""

            'Me.txtFailingVerbiage.Document.OpenNew(False)
            'Me.txtFailingVerbiage.DocumentText = ""
            Me.eFailingVerbiage.BodyHtml = ""

        Catch ex As Exception
            Throw New Exception("Error Clearing Fields:" & ex.Message)
        End Try
    End Sub

    Private Function GetMaxDisplaySequence() As Integer
        Dim retval As Integer

        For Each item As ListViewItem In Me.lvCertificationType.Items
            retval = CInt(item.SubItems(kDisplaySequence).Text)
        Next

        Return retval
    End Function

#Region "IsDirty Events"

    Private Sub txtCertificationName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCertificationName.TextChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bPopulateFields Then
                IsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub cbxActive_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxActive.CheckedChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bPopulateFields Then
                IsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub nudDisplaySequence_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bPopulateFields Then
                IsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub cbxPublish_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxPublish.CheckedChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bPopulateFields Then
                IsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub nudPassingScore_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudPassingScore.ValueChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bPopulateFields Then
                IsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub nudLengthOfCertification_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudLengthOfCertification.ValueChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bPopulateFields Then
                IsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub txtFailingVerbiage_ContentsChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bPopulateFields Then
                IsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub txtPassingVerbiage_ContentsChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Me.Cursor = Cursors.WaitCursor

            If Not bFormLoading And Not bPopulateFields Then
                IsDirty = True
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region "Constructors"

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Sub New(ByVal UID As Integer)
        InitializeComponent()
        Me.UserID = UID
    End Sub

#End Region

    Private Sub CreateFTPDirectory(ByVal CertificationTypeID As String)
        Try
            Dim ftp As Utilities.FTP.FTPclient = New Utilities.FTP.FTPclient(l.FTPSite, l.FTPUserName, l.FTPPassword)

            ftp.FtpCreateDirectory(CertificationTypeID) 'certification root folder
            ftp.FtpCreateDirectory(CertificationTypeID & "/Images") 'certification images folder
            ftp.FtpCreateDirectory(CertificationTypeID & "/Videos") 'certification videos folder
        Catch ex As Exception
            Throw New Exception("Error Creating FTP Directory: " & ex.Message)
        End Try
    End Sub

    Private Function SelectTotalItemsCountAndQuestionsCount() As Boolean
        Dim bDBIsNothing = dwsdb Is Nothing
        If bDBIsNothing Then dwsdb = New daWSDB
        Dim ds As DataSet
        Dim retval As Boolean = True

        Try
            ds = dwsdb.SelectCountOfItemsByCertificationTypeID(Me.CertificationTypeID)

            If ds.Tables(0).Rows.Count > 0 And ds.Tables(1).Rows.Count > 0 And ds.Tables(2).Rows.Count > 0 Then
                If ds.Tables(0).Rows(0).Item("TotalItemsCount") <= 0 _
                Or ds.Tables(1).Rows(0).Item("TotalQuestionsCount") <= 0 Then
                    'Or ds.Tables(2).Rows(0).Item("TotalFollowUpQuestionsCount") <= 0   'Per meeting with Ray in March 2013 they want Questions and Follou Up to be the same questions.

                    'MsgBox("This certification cannot be published because it doesn't have enough content, questions or follow-up questions.", MsgBoxStyle.Critical, "Publish Error")
                    MsgBox("This certification cannot be published because it doesn't have enough content or questions.", MsgBoxStyle.Critical, "Publish Error")
                    retval = False
                End If
            End If

            Return retval
        Catch ex As Exception
            Throw New Exception("SelectTotalItemsCountAndQuestionsCount Error: " & ex.Message)
        Finally
            If bDBIsNothing Then dwsdb = Nothing
        End Try
    End Function

    Private Sub btEditPassingVerbiage_Click(sender As System.Object, e As System.EventArgs) Handles btEditPassingVerbiage.Click
        Dim frme As frmEditText = New frmEditText
        frme.Text = "Edit Passing Verbiage"
        frme.txtEditText.Text = ePassingVerbiage.BodyHtml
        frme.ShowDialog()

        If frme.Save Then
            ePassingVerbiage.BodyHtml = frme.txtEditText.Text
            IsDirty = True
        End If
    End Sub

    Private Sub btEditFailingVerbiage_Click(sender As System.Object, e As System.EventArgs) Handles btEditFailingVerbiage.Click
        Dim frme As frmEditText = New frmEditText
        frme.Text = "Edit Passing Verbiage"
        frme.txtEditText.Text = eFailingVerbiage.BodyHtml
        frme.ShowDialog()

        If frme.Save Then
            eFailingVerbiage.BodyHtml = frme.txtEditText.Text
            IsDirty = True
        End If

    End Sub


    Private Sub ePassingVerbiage_BoldChanged(sender As Object, e As System.EventArgs) Handles ePassingVerbiage.BoldChanged, ePassingVerbiage.FontChanged, ePassingVerbiage.FontSizeChanged, ePassingVerbiage.ForeColorChanged
        If Not bFormLoading Then
            bIsDirty = True
        End If
    End Sub

    Private Sub ePassingVerbiage_IndentChanged(sender As Object, e As System.EventArgs) Handles ePassingVerbiage.IndentChanged, ePassingVerbiage.ItalicChanged, ePassingVerbiage.UnderlineChanged
        If Not bFormLoading Then
            bIsDirty = True
        End If
    End Sub

    Private Sub ePassingVerbiage_JustifyLeftChanged(sender As Object, e As System.EventArgs) Handles ePassingVerbiage.JustifyFullChanged, ePassingVerbiage.JustifyLeftChanged, ePassingVerbiage.JustifyRightChanged
        If Not bFormLoading Then
            bIsDirty = True
        End If
    End Sub


    Private Sub eFailingVerbiage_BoldChanged(sender As Object, e As System.EventArgs) Handles eFailingVerbiage.BoldChanged, eFailingVerbiage.FontChanged, eFailingVerbiage.FontSizeChanged, eFailingVerbiage.ForeColorChanged
        If Not bFormLoading Then
            bIsDirty = True
        End If
    End Sub

    Private Sub eFailingVerbiage_IndentChanged(sender As Object, e As System.EventArgs) Handles eFailingVerbiage.IndentChanged, eFailingVerbiage.ItalicChanged, eFailingVerbiage.UnderlineChanged
        If Not bFormLoading Then
            bIsDirty = True
        End If
    End Sub

    Private Sub eFailingVerbiage_JustifyLeftChanged(sender As Object, e As System.EventArgs) Handles eFailingVerbiage.JustifyFullChanged, eFailingVerbiage.JustifyLeftChanged, eFailingVerbiage.JustifyRightChanged
        If Not bFormLoading Then
            bIsDirty = True
        End If
    End Sub


End Class