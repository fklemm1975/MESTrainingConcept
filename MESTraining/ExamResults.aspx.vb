Imports System.Threading
Imports Microsoft.Reporting.WebForms

Public Class ExamResults
    Inherits System.Web.UI.Page


#Region "Properties"
    Private db As CertDB
    Private l As CertDBLib = New CertDBLib
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim bDBIsNothing = db Is Nothing
        Dim dto As CertDto.CertificationTypeDto = New CertDto.CertificationTypeDto
        Dim dtoUS As CertDto.UserSummaryDto = New CertDto.UserSummaryDto

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


                dto.Populate(Session("CertificationTypeID"), db)

                Me.lblCertificationName.Text = dto.CertificationName
                dtoUS.Populate(Session("UserSummaryID"), db)
                If dtoUS.ExamScore >= dto.PassingScore Then
                    lblExamResults.Text = dto.PassingVerbiage
                    pnlFail.Visible = False
                Else
                    lblExamResults.Text = dto.FailingVerbiage
                    pnlPass.Visible = False
                End If

                Me.lblExamScore.Text = dtoUS.ExamScore

            End If
        Catch ex1 As ThreadAbortException
            'Response.Redirect() will throw this error, we don't need to do anything with it, so we'll capture it here, instead of in the most general exception below
        Catch ex As Exception

            If Not db Is Nothing Then db.Dispose()
            db = Nothing

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If


            l.LogIt("PageLoad Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)
        Finally

            If bDBIsNothing Then
                If Not db Is Nothing Then db.Dispose()
                db = Nothing
            End If

        End Try
    End Sub

    Protected Sub lbTestResultsF_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbTestResultsF.Click


        Try

            Dim iCertificationTypeID As Integer
            Dim iUserSummaryID As Integer

            If Integer.TryParse(Session("CertificationTypeID"), iCertificationTypeID) And Integer.TryParse(Session("UserSummaryID"), iUserSummaryID) Then

                Response.Redirect("~/Exam.aspx?CertificationTypeID=" & iCertificationTypeID.ToString & "&UserSummaryID=" & iUserSummaryID.ToString)

            Else
                Throw New Exception("Invalid CertificationTypeID or UserSummaryID.  Can not parse to integer.")
            End If

        Catch ex1 As ThreadAbortException
        Catch ex As Exception

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            l.LogIt("lbCertificate_Click Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)
        End Try


    End Sub

    Protected Sub lbTestResultsP_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbTestResultsP.Click

        Try

            Dim iCertificationTypeID As Integer
            Dim iUserSummaryID As Integer

            If Integer.TryParse(Session("CertificationTypeID"), iCertificationTypeID) And Integer.TryParse(Session("UserSummaryID"), iUserSummaryID) Then

                Response.Redirect("~/Exam.aspx?CertificationTypeID=" & iCertificationTypeID.ToString & "&UserSummaryID=" & iUserSummaryID.ToString)

            Else
                Throw New Exception("Invalid CertificationTypeID or UserSummaryID.  Can not parse to integer.")
            End If

        Catch ex1 As ThreadAbortException
        Catch ex As Exception

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            l.LogIt("lbCertificate_Click Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)
        End Try

    End Sub

    Protected Sub lbReview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbReview.Click

        Dim bDBIsNothing = db Is Nothing

        Try
            If bDBIsNothing Then db = New CertDB

            Dim iUserSummaryID As Integer

            If Integer.TryParse(Session("UserSummaryID"), iUserSummaryID) Then

                Dim ds As DataSet

                ds = db.SelectCertificationsFirstModuleByUserSummaryID(iUserSummaryID)

                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then

                        Dim iCertificationTypeID As Integer = ds.Tables(0).Rows(0)("CertificationTypeID")
                        Dim iFirstItemID As Integer = ds.Tables(0).Rows(0)("FirstItemID")

                        Response.Redirect("~/Certification.aspx?CertificationTypeID=" & iCertificationTypeID.ToString & "&LastModule=" & iFirstItemID.ToString & "&UserSummaryID=" & iUserSummaryID.ToString)

                    Else
                        Throw New Exception("No rows returned.")
                    End If
                Else
                    Throw New Exception("No tables returned.")
                End If

            Else
                Throw New Exception("Invalid UserSummaryID.  Can not parse to integer.")
            End If

        Catch ex1 As ThreadAbortException
        Catch ex As Exception

            If Not db Is Nothing Then db.Dispose()
            db = Nothing

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            l.LogIt("lbCertificate_Click Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)
        Finally

            If bDBIsNothing Then
                If Not db Is Nothing Then db.Dispose()
                db = Nothing
            End If

        End Try

    End Sub

    Protected Sub lbCertificate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCertificate.Click
        Dim bDBIsNothing = db Is Nothing

        Dim sMsg As String = ""
        Dim iCount As Integer = 0

        Try
            If bDBIsNothing Then db = New CertDB

            Dim iUserSummaryID As Integer

            If Integer.TryParse(Session("UserSummaryID"), iUserSummaryID) Then

                Dim ds As DataSet

                ds = db.ReportCertificate(iUserSummaryID)

                Dim rv As ReportViewer = New ReportViewer
                rv.LocalReport.ReportPath = "Reports/MarineCertificate.rdlc"
                rv.LocalReport.EnableExternalImages = True
                Dim dr As DataRow = ds.Tables(0).Rows(0)

                If ds.Tables(0).Rows.Count > 0 Then
                    rv.LocalReport.DataSources.Add(New ReportDataSource("DataSet1", ds.Tables(0)))
                    rv.LocalReport.Refresh()
                Else
                    Throw New Exception("No data available.")
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
                Response.AddHeader("content-disposition", "attachment; filename=Certificate_" & iUserSummaryID.ToString & "." & extension)
                Response.BinaryWrite(bytes)
                Response.Flush()
                Response.End()
            Else
                Throw New Exception("Invalid UserSummaryID.  Can not parse to integer.")
            End If

        Catch ex1 As ThreadAbortException
        Catch ex As Exception

            If Not db Is Nothing Then db.Dispose()
            db = Nothing


            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            l.LogIt("lbCertificate_Click Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)
        Finally

            If bDBIsNothing Then
                If Not db Is Nothing Then db.Dispose()
                db = Nothing
            End If

        End Try
    End Sub


End Class