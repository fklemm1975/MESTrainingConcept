<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmNewCertificationType
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmNewCertificationType))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btEditFailingVerbiage = New System.Windows.Forms.Button()
        Me.btEditPassingVerbiage = New System.Windows.Forms.Button()
        Me.eFailingVerbiage = New Design.Editor()
        Me.ePassingVerbiage = New Design.Editor()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.nudDisplaySequence = New System.Windows.Forms.NumericUpDown()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.nudLengthOfCertification = New System.Windows.Forms.NumericUpDown()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.nudPassingScore = New System.Windows.Forms.NumericUpDown()
        Me.cbxPublish = New System.Windows.Forms.CheckBox()
        Me.lvCertificationType = New System.Windows.Forms.ListView()
        Me.txtCertificationName = New System.Windows.Forms.TextBox()
        Me.cbxActive = New System.Windows.Forms.CheckBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnSortAlphabetically = New System.Windows.Forms.Button()
        Me.ofdCertificateImage = New System.Windows.Forms.OpenFileDialog()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.nudDisplaySequence, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudLengthOfCertification, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudPassingScore, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 243)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(101, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Certification Names:"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel2, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(499, 682)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btEditFailingVerbiage)
        Me.Panel1.Controls.Add(Me.btEditPassingVerbiage)
        Me.Panel1.Controls.Add(Me.eFailingVerbiage)
        Me.Panel1.Controls.Add(Me.ePassingVerbiage)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.nudDisplaySequence)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.nudLengthOfCertification)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.nudPassingScore)
        Me.Panel1.Controls.Add(Me.cbxPublish)
        Me.Panel1.Controls.Add(Me.lvCertificationType)
        Me.Panel1.Controls.Add(Me.txtCertificationName)
        Me.Panel1.Controls.Add(Me.cbxActive)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(493, 641)
        Me.Panel1.TabIndex = 0
        '
        'btEditFailingVerbiage
        '
        Me.btEditFailingVerbiage.Location = New System.Drawing.Point(369, 456)
        Me.btEditFailingVerbiage.Name = "btEditFailingVerbiage"
        Me.btEditFailingVerbiage.Size = New System.Drawing.Size(118, 23)
        Me.btEditFailingVerbiage.TabIndex = 32
        Me.btEditFailingVerbiage.Text = "Edit Failing Verbiage"
        Me.btEditFailingVerbiage.UseVisualStyleBackColor = True
        '
        'btEditPassingVerbiage
        '
        Me.btEditPassingVerbiage.Location = New System.Drawing.Point(369, 306)
        Me.btEditPassingVerbiage.Name = "btEditPassingVerbiage"
        Me.btEditPassingVerbiage.Size = New System.Drawing.Size(118, 23)
        Me.btEditPassingVerbiage.TabIndex = 31
        Me.btEditPassingVerbiage.Text = "Edit Passing Verbiage"
        Me.btEditPassingVerbiage.UseVisualStyleBackColor = True
        '
        'eFailingVerbiage
        '
        Me.eFailingVerbiage.BodyHtml = Nothing
        Me.eFailingVerbiage.BodyText = Nothing
        Me.eFailingVerbiage.DocumentText = resources.GetString("eFailingVerbiage.DocumentText")
        Me.eFailingVerbiage.EditorBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.eFailingVerbiage.EditorForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.eFailingVerbiage.FontName = Nothing
        Me.eFailingVerbiage.FontSize = Design.FontSize.NA
        Me.eFailingVerbiage.Location = New System.Drawing.Point(6, 481)
        Me.eFailingVerbiage.Name = "eFailingVerbiage"
        Me.eFailingVerbiage.Size = New System.Drawing.Size(482, 150)
        Me.eFailingVerbiage.TabIndex = 16
        '
        'ePassingVerbiage
        '
        Me.ePassingVerbiage.BodyHtml = Nothing
        Me.ePassingVerbiage.BodyText = Nothing
        Me.ePassingVerbiage.DocumentText = resources.GetString("ePassingVerbiage.DocumentText")
        Me.ePassingVerbiage.EditorBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.ePassingVerbiage.EditorForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.ePassingVerbiage.FontName = Nothing
        Me.ePassingVerbiage.FontSize = Design.FontSize.NA
        Me.ePassingVerbiage.Location = New System.Drawing.Point(6, 332)
        Me.ePassingVerbiage.Name = "ePassingVerbiage"
        Me.ePassingVerbiage.Size = New System.Drawing.Size(481, 118)
        Me.ePassingVerbiage.TabIndex = 15
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(3, 465)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(85, 13)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "Failing Verbiage:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(3, 316)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(92, 13)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "Passing Verbiage:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(87, 296)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(96, 13)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Display Sequence:"
        '
        'nudDisplaySequence
        '
        Me.nudDisplaySequence.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        Me.nudDisplaySequence.Location = New System.Drawing.Point(189, 292)
        Me.nudDisplaySequence.Name = "nudDisplaySequence"
        Me.nudDisplaySequence.Size = New System.Drawing.Size(64, 20)
        Me.nudDisplaySequence.TabIndex = 10
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(271, 273)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(147, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Length of Certification (years):"
        '
        'nudLengthOfCertification
        '
        Me.nudLengthOfCertification.Location = New System.Drawing.Point(424, 270)
        Me.nudLengthOfCertification.Name = "nudLengthOfCertification"
        Me.nudLengthOfCertification.Size = New System.Drawing.Size(64, 20)
        Me.nudLengthOfCertification.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(87, 273)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(78, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Passing Score:"
        '
        'nudPassingScore
        '
        Me.nudPassingScore.Increment = New Decimal(New Integer() {5, 0, 0, 0})
        Me.nudPassingScore.Location = New System.Drawing.Point(189, 269)
        Me.nudPassingScore.Name = "nudPassingScore"
        Me.nudPassingScore.Size = New System.Drawing.Size(64, 20)
        Me.nudPassingScore.TabIndex = 5
        '
        'cbxPublish
        '
        Me.cbxPublish.AutoSize = True
        Me.cbxPublish.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cbxPublish.Location = New System.Drawing.Point(18, 293)
        Me.cbxPublish.Name = "cbxPublish"
        Me.cbxPublish.Size = New System.Drawing.Size(63, 17)
        Me.cbxPublish.TabIndex = 8
        Me.cbxPublish.Text = "Publish:"
        Me.cbxPublish.UseVisualStyleBackColor = True
        '
        'lvCertificationType
        '
        Me.lvCertificationType.Location = New System.Drawing.Point(6, 4)
        Me.lvCertificationType.Name = "lvCertificationType"
        Me.lvCertificationType.Size = New System.Drawing.Size(481, 229)
        Me.lvCertificationType.TabIndex = 0
        Me.lvCertificationType.UseCompatibleStateImageBehavior = False
        Me.lvCertificationType.View = System.Windows.Forms.View.Details
        '
        'txtCertificationName
        '
        Me.txtCertificationName.Location = New System.Drawing.Point(117, 239)
        Me.txtCertificationName.Name = "txtCertificationName"
        Me.txtCertificationName.Size = New System.Drawing.Size(370, 20)
        Me.txtCertificationName.TabIndex = 2
        '
        'cbxActive
        '
        Me.cbxActive.AutoSize = True
        Me.cbxActive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cbxActive.Location = New System.Drawing.Point(22, 270)
        Me.cbxActive.Name = "cbxActive"
        Me.cbxActive.Size = New System.Drawing.Size(59, 17)
        Me.cbxActive.TabIndex = 3
        Me.cbxActive.Text = "Active:"
        Me.cbxActive.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.btnCancel)
        Me.Panel2.Controls.Add(Me.btnSave)
        Me.Panel2.Controls.Add(Me.btnNew)
        Me.Panel2.Controls.Add(Me.btnSortAlphabetically)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(3, 650)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(493, 29)
        Me.Panel2.TabIndex = 1
        '
        'btnCancel
        '
        Me.btnCancel.Enabled = False
        Me.btnCancel.Location = New System.Drawing.Point(412, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Enabled = False
        Me.btnSave.Location = New System.Drawing.Point(331, 3)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 2
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(250, 3)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(75, 23)
        Me.btnNew.TabIndex = 1
        Me.btnNew.Text = "New"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnSortAlphabetically
        '
        Me.btnSortAlphabetically.Location = New System.Drawing.Point(3, 3)
        Me.btnSortAlphabetically.Name = "btnSortAlphabetically"
        Me.btnSortAlphabetically.Size = New System.Drawing.Size(75, 23)
        Me.btnSortAlphabetically.TabIndex = 0
        Me.btnSortAlphabetically.Text = "Alphabetize"
        Me.btnSortAlphabetically.UseVisualStyleBackColor = True
        '
        'ofdCertificateImage
        '
        Me.ofdCertificateImage.FileName = "OpenFileDialog1"
        '
        'frmNewCertificationType
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(499, 682)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmNewCertificationType"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Training Courses"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.nudDisplaySequence, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudLengthOfCertification, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudPassingScore, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents cbxActive As System.Windows.Forms.CheckBox
    Friend WithEvents txtCertificationName As System.Windows.Forms.TextBox
    Friend WithEvents lvCertificationType As System.Windows.Forms.ListView
    Friend WithEvents btnSortAlphabetically As System.Windows.Forms.Button
    Friend WithEvents cbxPublish As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents nudPassingScore As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents nudDisplaySequence As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents nudLengthOfCertification As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ofdCertificateImage As System.Windows.Forms.OpenFileDialog
    Friend WithEvents eFailingVerbiage As Design.Editor
    Friend WithEvents ePassingVerbiage As Design.Editor
    Friend WithEvents btEditFailingVerbiage As System.Windows.Forms.Button
    Friend WithEvents btEditPassingVerbiage As System.Windows.Forms.Button
End Class
