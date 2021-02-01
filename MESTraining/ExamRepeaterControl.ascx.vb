Public Class ExamRepeaterControl
    Inherits System.Web.UI.UserControl

#Region "properties"
    Private l As CertDBLib = New CertDBLib

    Private sQuestionHeading As String = ""
    Private sQuestion As String = ""
    Private sAnswer1 As String = ""
    Private sAnswer2 As String = ""
    Private sAnswer3 As String = ""
    Private sAnswer4 As String = ""
    Private sAnswer5 As String = ""
    Private iQuestionID As Integer = 0
    Private sAnswer As String = ""
    Private sSelectedAnswer As String = ""
    Private bEnabled As Boolean = False
    Private bQuestionHeadingVisible As Boolean = False
    Private sRank As String = ""

    Public Property QuestionHeading() As String
        Get
            QuestionHeading = Me.lblQuestionHeading.Text
        End Get
        Set(ByVal value As String)
            Me.lblQuestionHeading.Text = value
        End Set
    End Property

    Public Property Question() As String
        Get
            Question = Me.lblQuestion.Text
        End Get
        Set(ByVal value As String)
            Me.lblQuestion.Text = value
        End Set
    End Property

    Public Property Answer1() As String
        Get
            Answer1 = Me.rbAnswer1.Text
        End Get
        Set(ByVal value As String)
            Me.rbAnswer1.Text = value
        End Set
    End Property

    Public Property Answer2() As String
        Get
            Answer2 = Me.rbAnswer2.Text
        End Get
        Set(ByVal value As String)
            Me.rbAnswer2.Text = value
        End Set
    End Property

    Public Property Answer3() As String
        Get
            Answer3 = Me.rbAnswer3.Text
        End Get
        Set(ByVal value As String)
            Me.rbAnswer3.Text = value
        End Set
    End Property

    Public Property Answer4() As String
        Get
            Answer4 = Me.rbAnswer4.Text
        End Get
        Set(ByVal value As String)
            Me.rbAnswer4.Text = value
        End Set
    End Property

    Public Property Answer5() As String
        Get
            Answer5 = Me.rbAnswer5.Text
        End Get
        Set(ByVal value As String)
            Me.rbAnswer5.Text = value
        End Set
    End Property

    Public Property QuestionID() As Integer
        Get
            QuestionID = Me.hfQuestionID.Value
        End Get
        Set(ByVal value As Integer)
            Me.hfQuestionID.Value = value
        End Set
    End Property

    Public Property Answer() As String
        Get
            Answer = Me.hfAnswer.Value
        End Get
        Set(ByVal value As String)
            Me.hfAnswer.Value = value
        End Set
    End Property

    Public Property SelectedAnswer() As String
        Get
            SelectedAnswer = Me.hfSelectedAnswer.Value
        End Get
        Set(ByVal value As String)
            Me.hfSelectedAnswer.Value = value
        End Set
    End Property

    Public Property Enabled() As Boolean
        Get
            Enabled = bEnabled
        End Get
        Set(ByVal value As Boolean)
            Me.bEnabled = value

            Me.rbAnswer1.Enabled = value
            Me.rbAnswer2.Enabled = value
            Me.rbAnswer3.Enabled = value
            Me.rbAnswer4.Enabled = value
            Me.rbAnswer5.Enabled = value

            Me.Panel1.Visible = Not value
            Me.lblCorrectIncorrect.Visible = Not value
            Me.lblCorrectIncorrect.Font.Bold = True
            If Me.lblCorrectIncorrect.Text = "Correct" Then
                Me.lblCorrectIncorrect.ForeColor = Drawing.Color.Green
            Else
                Me.lblCorrectIncorrect.ForeColor = Drawing.Color.Red
            End If

            If Me.lblCorrectIncorrect.Text.Trim.Length > 0 Then
                Me.lblCorrectAnswer.Visible = Not value
            End If
        End Set
    End Property

    Public Property QuestionHeadingVisible() As Boolean
        Get
            QuestionHeadingVisible = bQuestionHeadingVisible
        End Get
        Set(ByVal value As Boolean)
            Me.lblQuestionHeading.Visible = value
        End Set
    End Property

    Public ReadOnly Property Rank() As String
        Get
            Rank = Me.lblRank.Text
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If rbAnswer1.Text = "" Then
                rbAnswer1.Visible = False
            End If

            If rbAnswer2.Text = "" Then
                rbAnswer2.Visible = False
            End If

            If rbAnswer3.Text = "" Then
                rbAnswer3.Visible = False
            End If

            If rbAnswer4.Text = "" Then
                rbAnswer4.Visible = False
            End If

            If rbAnswer5.Text = "" Then
                rbAnswer5.Visible = False
            End If

            If Not IsPostBack Then
                PopulateForm()
            End If
        Catch ex As Exception

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            l.LogIt("UserControl PageLoad Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)
        End Try
    End Sub

    Private Sub PopulateForm()
        Try
            If Me.hfAnswer.Value <> "" Then
                Select Case Me.hfSelectedAnswer.Value
                    Case Me.Answer1
                        Me.rbAnswer1.Checked = True
                    Case Me.Answer2
                        Me.rbAnswer2.Checked = True
                    Case Me.Answer3
                        Me.rbAnswer3.Checked = True
                    Case Me.Answer4
                        Me.rbAnswer4.Checked = True
                    Case Me.Answer5
                        Me.rbAnswer5.Checked = True
                End Select
            End If
        Catch ex As Exception

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            Throw New Exception("PopulateForm Error: " & ex.Message & sInnerException)
        End Try
    End Sub

    Protected Sub rbAnswer1_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rbAnswer1.CheckedChanged, rbAnswer2.CheckedChanged, rbAnswer3.CheckedChanged, rbAnswer4.CheckedChanged, rbAnswer5.CheckedChanged
        Try
            If rbAnswer1.Checked Then
                Me.hfAnswer.Value = Me.rbAnswer1.Text
            ElseIf rbAnswer2.Checked Then
                Me.hfAnswer.Value = Me.rbAnswer2.Text
            ElseIf rbAnswer3.Checked Then
                Me.hfAnswer.Value = Me.rbAnswer3.Text
            ElseIf rbAnswer4.Checked Then
                Me.hfAnswer.Value = Me.rbAnswer4.Text
            ElseIf rbAnswer5.Checked Then
                Me.hfAnswer.Value = Me.rbAnswer5.Text
            Else
                'shouldn't happen
            End If
        Catch ex As Exception

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            l.LogIt("rbAnswer_CheckedChanged Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)
        End Try
    End Sub

End Class