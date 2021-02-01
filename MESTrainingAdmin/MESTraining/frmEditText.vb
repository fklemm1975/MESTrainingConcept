Public Class frmEditText
    Public Property Save As Boolean = False

    Private Sub btSave_Click(sender As System.Object, e As System.EventArgs) Handles btSave.Click
        Save = True
        Me.Close()

    End Sub

    Private Sub btCancel_Click(sender As System.Object, e As System.EventArgs) Handles btCancel.Click
        Me.Close()

    End Sub
End Class