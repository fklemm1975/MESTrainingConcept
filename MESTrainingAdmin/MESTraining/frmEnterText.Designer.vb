<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEnterText
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtBodyHeading = New McDull.Windows.Forms.HTMLTextBox()
        Me.btSave = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'txtBodyHeading
        '
        Me.txtBodyHeading.Location = New System.Drawing.Point(12, 12)
        Me.txtBodyHeading.Name = "txtBodyHeading"
        Me.txtBodyHeading.Size = New System.Drawing.Size(600, 450)
        Me.txtBodyHeading.TabIndex = 3
        '
        'btSave
        '
        Me.btSave.Location = New System.Drawing.Point(537, 468)
        Me.btSave.Name = "btSave"
        Me.btSave.Size = New System.Drawing.Size(75, 23)
        Me.btSave.TabIndex = 2
        Me.btSave.Text = "Save"
        Me.btSave.UseVisualStyleBackColor = True
        '
        'frmEnterText
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(628, 675)
        Me.Controls.Add(Me.txtBodyHeading)
        Me.Controls.Add(Me.btSave)
        Me.Name = "frmEnterText"
        Me.Text = "frmEnterText"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtBodyHeading As McDull.Windows.Forms.HTMLTextBox
    Friend WithEvents btSave As System.Windows.Forms.Button
End Class
