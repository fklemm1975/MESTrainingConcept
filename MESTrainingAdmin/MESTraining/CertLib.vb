Imports MESTrainingDB
Imports SimpleEncryption

Public Class CertLib
    Private l As MESTrainingDB.CertDBLib = New MESTrainingDB.CertDBLib
    Private FTPKey As String = "Qi0&(Tj24p@W<3ZrN_z7>8P"
    Public FTPUserName As String = Crypto.Decrypt(My.Settings.FTPEncryptedUserName, FTPKey)
    Public FTPPassword As String = Crypto.Decrypt(My.Settings.FTPEncryptedPassword, FTPKey)
    Public FTPSite As String = "ftp://63.139.120.111:22/"

    Public Sub LogIt(ByVal txt As String, Optional ByVal ShowMsgBox As Boolean = False, Optional ByVal MsgTitle As String = "", Optional ByRef Params As MESTrainingDB.CertDB.SQLParamCollection = Nothing, Optional ByVal MessageBoxStyle As MsgBoxStyle = MsgBoxStyle.Critical)
        l.LogIt(txt, Params)

        If (ShowMsgBox) Then
            MsgBox(txt, MessageBoxStyle, MsgTitle)
        End If
    End Sub

    Public Enum ItemType
        Instruction = 1
        Question = 2
        NA = 3
    End Enum

    Public Sub ConfigureGrid(ByRef dgv As DataGridView, Optional ByVal ClearColumns As Boolean = True)
        If ClearColumns Then dgv.Columns.Clear()

        dgv.Font = New Font("Microsoft Sans Serif", 8.25)
        dgv.BorderStyle = BorderStyle.FixedSingle
        dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single

        dgv.AutoGenerateColumns = False
        dgv.AllowUserToAddRows = False
        dgv.AllowUserToDeleteRows = False
        dgv.AllowUserToOrderColumns = False
        dgv.AllowUserToResizeRows = False

        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.LightSteelBlue
        'dgv.RowsDefaultCellStyle.SelectionBackColor = Color.LightGoldenrodYellow
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.MultiSelect = False
        dgv.RowHeadersVisible = False
        dgv.ReadOnly = True
    End Sub

End Class
