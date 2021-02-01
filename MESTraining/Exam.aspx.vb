Imports System.Threading
Imports Microsoft.Reporting.WebForms


Public Class Exam
    Inherits System.Web.UI.Page


#Region "Properties"
    Private db As CertDB
    Private l As CertDBLib = New CertDBLib
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim bDBIsNothing = db Is Nothing
        Dim dto As CertDto.CertificationTypeDto


        Try
            If bDBIsNothing Then db = New CertDB

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

            If Not IsPostBack Then
                If Session("RegistrationID") Is Nothing Then
                    Response.Redirect("~/Login.aspx")
                End If

                Me.RegID.Text = Session("RegistrationID")    'Drop the reg id in a hidden field.

                If Session("CertificationTypeID") > 0 Then
                    BindExamData()
                    PopulateForm()

                    dto = New CertDto.CertificationTypeDto
                    dto.Populate(Session("CertificationTypeID"), db)
                    Me.lblCertificationName.Text = dto.CertificationName
                End If

                Dim dtoUS As CertDto.UserSummaryDto = New CertDto.UserSummaryDto
                dtoUS.Populate(Session("UserSummaryID"), db)

                If Not dtoUS.ExamEndDate Is DBNull.Value Then
                    SetAllControlsToReadOnly()

                    If dto Is Nothing Then
                        dto = New CertDto.CertificationTypeDto
                        dto.Populate(Session("CertificationTypeID"), db)
                    End If

                    If dto.PassingScore > dtoUS.ExamScore Then
                        'Me.lbViewCertificate.Visible = False
                        Me.btViewCertificate.Visible = False
                    End If

                Else
                    'Me.lbViewCertificate.Visible = False
                    Me.btViewCertificate.Visible = False
                End If


            End If

        Catch ex1 As ThreadAbortException
        Catch ex As Exception

            If Not db Is Nothing Then db.Dispose()
            db = Nothing


            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            l.LogIt("Exam PageLoad Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)
        Finally

            If Not db Is Nothing Then db.Dispose()
            db = Nothing


        End Try
    End Sub

    Private Sub BindExamData()
        Dim dsExam As DataSet
        Try
            dsExam = db.SelectAllUserQuestionsByUserIDCertificationTypeID(Session("RegistrationID"), Session("CertificationTypeID"), Session("UserSummaryID"))
            Me.rptExam.DataSource = dsExam
            Me.rptExam.DataBind()

        Catch ex As Exception

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            Throw New Exception("BuildExam Error: " & ex.Message & sInnerException)
        End Try
    End Sub

    Private Sub PopulateForm()
        Try

            Dim PreviousQuestionHeading As String = ""

            For Each item As RepeaterItem In rptExam.Items
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

            Throw New Exception("PopulateForm Error: " & ex.Message & sInnerException)
        End Try
    End Sub

    Private Sub btSaveExam_Click(sender As Object, e As EventArgs) Handles btSaveExam.Click
        Try
            SaveExam()
        Catch ex As Exception

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            l.LogIt("lbSaveExam_Click Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)
        End Try
    End Sub

    Private Sub btSaveComplete_Click(sender As Object, e As EventArgs) Handles btSaveComplete.Click
        Dim bDBIsNothing = db Is Nothing
        Dim ds As DataSet
        Dim sUnanswered As String = ""
        Dim sComma As String = ""

        Try
            If bDBIsNothing Then db = New CertDB

            SaveExam()

            'Loop through questions and verify they're all answered 
            Dim bAllAnswered As Boolean = True
            For Each item As RepeaterItem In rptExam.Items
                Dim UC As ExamRepeaterControl = New ExamRepeaterControl()
                UC = CType(item.FindControl("ExamRepeaterControl1"), ExamRepeaterControl)

                If UC.Answer.Trim.Length = 0 Then
                    bAllAnswered = False
                    sUnanswered += sComma & UC.Rank.Trim.Replace(".", "")
                    sComma = ", "
                End If
            Next


            If bAllAnswered Then    'If they're all answered, then grade it.
                ds = db.GradeExamUpdateUserSummaryExamScore(Session("RegistrationID"), Session("CertificationTypeID"), Session("UserSummaryID"))
                If ds.Tables(0).Rows.Count > 0 Then
                    Dim dr As DataRow = ds.Tables(0).Rows(0)
                    Response.Redirect("~/ExamResults.aspx?CertificationTypeID=" & Session("CertificationTypeID") & "&UserSummaryID=" & dr("UserSummaryID"))
                End If
            Else                    'Else
                Me.lblMessage.Text = "Please answer all Exam Questions. <br>The following questions were not answered: " & sUnanswered
                Me.lblMessage.ForeColor = Drawing.Color.Red

                Me.lblMessage2.Text = "Please answer all Exam Questions. <br>The following questions were not answered: " & sUnanswered
                Me.lblMessage2.ForeColor = Drawing.Color.Red
            End If

        Catch ex1 As ThreadAbortException

        Catch ex As Exception

            If Not db Is Nothing Then db.Dispose()
            db = Nothing


            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            l.LogIt("lbSaveComplete_Click Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)
        Finally

            If bDBIsNothing Then
                If Not db Is Nothing Then db.Dispose()
                db = Nothing
            End If

        End Try
    End Sub

    Public Function SaveExam() As Boolean

        Dim bDBIsNothing = db Is Nothing

        Dim dto As CertDto.UserQuestionsDto = New CertDto.UserQuestionsDto
        Dim bRetVal As Boolean = False

        Try
            If bDBIsNothing Then db = New CertDB

            Dim iRegistrationID As Integer



            'If Integer.TryParse(Session("RegistrationID"), iRegistrationID) Then           ### 5/11/2014 - we're going to store this in a hidden field to get around the session timeout from now on.

            If Integer.TryParse(Me.RegID.Text, iRegistrationID) Then

                Session("RegistrationID") = iRegistrationID

                For Each item As RepeaterItem In rptExam.Items
                    Dim UC As ExamRepeaterControl = New ExamRepeaterControl()
                    UC = CType(item.FindControl("ExamRepeaterControl1"), ExamRepeaterControl)


                    If Not dto.SaveAnswer(UC.QuestionID, UC.Answer, iRegistrationID, db) Then
                        Throw New Exception("Save Error in Save Answer." & vbTab & "RegistrationID = " & iRegistrationID.ToString & vbTab & "UC.QuestionID = " & UC.QuestionID & vbTab & "UC.Answer = " & UC.Answer)
                    End If


                Next

                bRetVal = True

                lblMessage.Text = "Exam Saved." & vbCrLf
                lblMessage.ForeColor = Drawing.Color.Green

                lblMessage2.Text = "Exam Saved." & vbCrLf
                lblMessage2.ForeColor = Drawing.Color.Green

            Else
                Throw New Exception("SaveExam: Invalid RegistrationID.")
            End If




        Catch ex As Exception

            If Not db Is Nothing Then db.Dispose()
            db = Nothing


            lblMessage.Text = "Save Error." & vbCrLf
            lblMessage.ForeColor = Drawing.Color.Red

            lblMessage2.Text = "Save Error." & vbCrLf
            lblMessage2.ForeColor = Drawing.Color.Red


            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If


            Throw New Exception("SaveExam Error: " & ex.Message & sInnerException)
        Finally

            If bDBIsNothing Then
                If Not db Is Nothing Then db.Dispose()
                db = Nothing
            End If
            SaveExam = bRetVal
        End Try
    End Function

    Private Sub btViewCertificate_Click(sender As Object, e As EventArgs) Handles btViewCertificate.Click
        Dim bDBIsNothing = db Is Nothing

        Try
            If bDBIsNothing Then db = New CertDB


            Dim ds As DataSet

            ds = db.ReportCertificate(Session("UserSummaryID"))

            Dim rv As ReportViewer = New ReportViewer
            rv.LocalReport.ReportPath = "Reports/MarineCertificate.rdlc"
            rv.LocalReport.EnableExternalImages = True
            Dim dr As DataRow = ds.Tables(0).Rows(0)

            If ds.Tables(0).Rows.Count > 0 Then
                rv.LocalReport.DataSources.Add(New ReportDataSource("DataSet1", ds.Tables(0)))
                rv.LocalReport.Refresh()
            Else
                Throw New Exception("No data available.  (lbViewCertificate_Click) - '" & Session("UserSummaryID") & "'")
            End If

            'server report as PDF
            Dim streamIDs() As String
            Dim warnings() As Warning
            Dim bytes() As Byte
            Dim mimeType As String
            Dim encoding As String
            Dim extension As String

            bytes = rv.LocalReport.Render("PDF", Nothing, mimeType, encoding, extension, streamIDs, warnings)

            Response.Buffer = True
            Response.Clear()
            Response.ContentType = mimeType
            Response.AddHeader("content-disposition", "attachment; filename=Certificate_" & Session("UserSummaryID").ToString & "." & extension)
            Response.BinaryWrite(bytes)
            Response.Flush()
            Response.End()


        Catch ex1 As ThreadAbortException
        Catch ex As Exception

            If Not db Is Nothing Then db.Dispose()
            db = Nothing

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If


            l.LogIt("lbViewCertificate_Click Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)
        Finally

            If bDBIsNothing Then
                If Not db Is Nothing Then db.Dispose()
                db = Nothing
            End If

        End Try
    End Sub


    Private Sub SetAllControlsToReadOnly()
        Try
            For Each item As RepeaterItem In rptExam.Items
                Dim UC As ExamRepeaterControl = New ExamRepeaterControl
                UC = CType(item.FindControl("ExamRepeaterControl1"), ExamRepeaterControl)

                UC.Enabled = False
            Next

            'Me.lbSaveExam.Visible = False
            'Me.lbSaveComplete.Visible = False
            Me.btSaveExam.Visible = False
            Me.btSaveComplete.Visible = False
        Catch ex As Exception

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            Throw New Exception("SetAllControlsToReadOnly Error: " & ex.Message & sInnerException)
        End Try
    End Sub


End Class