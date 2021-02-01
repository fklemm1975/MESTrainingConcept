Public Class frmChangeSite

    Private dwsdb As daWSDB

    'Private sTrainingSiteName As String = ""
    'Private sEncryptedString As String = ""
    'Private sURL As String = ""

    'Public Property TrainingSiteName() As String
    '    Get
    '        TrainingSiteName = sTrainingSiteName
    '    End Get
    '    Set(ByVal value As String)
    '        sTrainingSiteName = value
    '    End Set
    'End Property

    'Public Property EncryptedString() As String
    '    Get
    '        EncryptedString = sEncryptedString
    '    End Get
    '    Set(ByVal value As String)
    '        sEncryptedString = value
    '    End Set
    'End Property

    'Public Property URL() As String
    '    Get
    '        URL = sURL
    '    End Get
    '    Set(ByVal value As String)
    '        sURL = value
    '    End Set
    'End Property


    Private Sub frmChangeSite_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        BuildComboBox()
    End Sub


    Public Sub BuildComboBox()
        Dim bDBIsNothing = dwsdb Is Nothing
        If bDBIsNothing Then dwsdb = New daWSDB("False")

        Try

            Me.cboSelectWebSite.ValueMember = "TrainingSiteID"
            Me.cboSelectWebSite.DisplayMember = "TrainingSiteName"
            Me.cboSelectWebSite.DataSource = dwsdb.SelectAllLUTrainingSite.Tables(0)

            If cboSelectWebSite.Items.Count > 0 Then
                Me.cboSelectWebSite.SelectedIndex = 0
            End If

        Catch ex As Exception
            Throw New Exception("BuildComboBox Error" & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            If bDBIsNothing Then dwsdb = Nothing
        End Try
    End Sub

    Private Sub btOK_Click(sender As System.Object, e As System.EventArgs) Handles btOK.Click

        Dim bDBIsNothing = dwsdb Is Nothing
        If bDBIsNothing Then dwsdb = New daWSDB("False")

        'Me.Cursor = Cursors.WaitCursor
        Me.UseWaitCursor = True

        Try

            Dim dt As DataTable = New DataTable()

            If cboSelectWebSite.Items.Count > 0 Then
                dt = dwsdb.SelectByIDTrainingSite(Me.cboSelectWebSite.SelectedValue).Tables(0)

                Dim dr As DataRow

                If dt.Rows.Count > 0 Then
                    dr = dt.Rows(0)

                    My.Settings.TrainingSiteName = dr("TrainingSiteName")
                    My.Settings.EncryptedString = dr("EncryptedString")
                    My.Settings.URL = dr("URL")
                    My.Settings.MediaDir = dr("MediaDir")
                    My.Settings.WebServiceURL = dr("WebServiceURL")
                End If

            End If


        Catch ex As Exception
            Throw New Exception("Button OK Error" & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            If bDBIsNothing Then dwsdb = Nothing
            'Me.Cursor = Cursors.Default
            Me.UseWaitCursor = False
            Me.Close()
        End Try


    End Sub

    Private Sub btCancel_Click(sender As System.Object, e As System.EventArgs) Handles btCancel.Click
        Me.Close()
    End Sub

End Class