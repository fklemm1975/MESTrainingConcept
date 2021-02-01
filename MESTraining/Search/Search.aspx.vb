'Imports Microsoft.Reporting.WebForms
Imports System.Threading

Public Class Search
    Inherits System.Web.UI.Page

    '#Region "Properties"
    '    Private l As CertDBLib = New CertDBLib
    '    Private db As CertDB

    '    Private Const kUserSummaryID As Integer = 7
    '#End Region

    '    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    '    End Sub

    Protected Sub lbSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Try
        '    BindGrid()
        'Catch ex As Exception

        '    Dim sInnerException As String = ""
        '    If Not ex.InnerException Is Nothing Then
        '        sInnerException = sInnerException
        '    End If

        '    l.LogIt("Search Error: " & ex.Source & vbCrLf & ex.Message & sInnerException)
        'End Try
    End Sub

    '    Private Sub BindGrid()
    '        Dim bDBIsNothing = db Is Nothing
    '        Dim dto As CertDto.SearchDto = New CertDto.SearchDto

    '        Try
    '            If bDBIsNothing Then db = New CertDB

    '            If Not HasSearchValues() Then
    '                Me.lblMessage.Text = "At least 1 field must contain search criteria."
    '                Me.gvSearch.DataSource = Nothing
    '            Else
    '                Me.lblMessage.Text = ""

    '                dto.FirstName = Me.txtFirstName.Text
    '                dto.MiddleName = Me.txtMiddleName.Text
    '                dto.LastName = Me.txtLastName.Text

    '                'SIR 756 - Remove these criteria
    '                'dto.Street = Me.txtStreet.Text
    '                'dto.City = Me.txtCity.Text
    '                'dto.County = Me.txtCounty.Text
    '                'dto.State = Me.txtState.Text
    '                'dto.ZipCode = Me.txtZipCode.Text
    '                dto.Street = ""
    '                dto.City = ""
    '                dto.County = ""
    '                dto.State = ""
    '                dto.ZipCode = ""

    '                dto.Company = Me.txtCompany.Text

    '                If Me.txtIssueDate.Text = "" Then
    '                    dto.IssueDate = DBNull.Value
    '                Else
    '                    dto.IssueDate = CDate(Me.txtIssueDate.Text)
    '                End If

    '                'SIR 756 - Remove these criteria
    '                'dto.DriversLicense = Me.txtDriversLicense.Text
    '                dto.DriversLicense = ""

    '                If Me.txtCertificationNumber.Text.Trim.Length > 0 Then
    '                    dto.CertificationNumber = Me.txtCertificationNumber.Text.PadLeft(6, "0"c)
    '                Else
    '                    dto.CertificationNumber = DBNull.Value
    '                End If

    '                Me.gvSearch.DataSource = dto.SelectCertifiedUsersByOptionalColumns()
    '            End If

    '            Me.gvSearch.DataBind()
    '        Catch ex As Exception

    '            If Not db Is Nothing Then db.Dispose()
    '            db = Nothing


    '            Dim sInnerException As String = ""
    '            If Not ex.InnerException Is Nothing Then
    '                sInnerException = sInnerException
    '            End If

    '            Throw New Exception("BindGrid Error: " & ex.Message & sInnerException)
    '        Finally

    '            If bDBIsNothing Then
    '                If Not db Is Nothing Then db.Dispose()
    '                db = Nothing
    '            End If

    '        End Try
    '    End Sub

    '    Private Function HasSearchValues() As Boolean
    '        'SIR 756 - Remove these criteria
    '        'HasSearchValues = Me.txtFirstName.Text.Trim.Length > 0 Or Me.txtMiddleName.Text.Trim.Length > 0 Or Me.txtLastName.Text.Trim.Length > 0 Or Me.txtStreet.Text.Trim.Length > 0 Or Me.txtCity.Text.Trim.Length > 0 Or Me.txtCounty.Text.Trim.Length > 0 Or Me.txtState.Text.Trim.Length > 0 Or Me.txtZipCode.Text.Trim.Length > 0 Or Me.txtCompany.Text.Trim.Length > 0 Or Me.txtDriversLicense.Text.Trim.Length > 0 Or Me.txtIssueDate.Text.Trim.Length > 0 Or Me.txtCertificationNumber.Text.Trim.Length > 0
    '        HasSearchValues = Me.txtFirstName.Text.Trim.Length > 0 Or Me.txtMiddleName.Text.Trim.Length > 0 Or Me.txtLastName.Text.Trim.Length > 0 Or Me.txtCompany.Text.Trim.Length > 0 Or Me.txtIssueDate.Text.Trim.Length > 0 Or Me.txtCertificationNumber.Text.Trim.Length > 0
    '    End Function

    '    Private Sub gvSearch_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSearch.RowCommand
    '        Dim bDBIsNothing = db Is Nothing

    '        Dim sMsg As String = ""
    '        Dim iCount As Integer = 0

    '        Try
    '            If bDBIsNothing Then db = New CertDB

    '            If e.CommandName = "ViewCertificate" Then
    '                Dim index As Integer = CInt(e.CommandArgument)
    '                Dim row As GridViewRow = Me.gvSearch.Rows(index)
    '                Dim ds As DataSet

    '                ds = db.ReportCertificate(row.Cells.Item(kUserSummaryID).Text)

    '                Dim rv As ReportViewer = New ReportViewer
    '                rv.LocalReport.ReportPath = "Reports/RPCCertificate.rdlc"
    '                rv.LocalReport.EnableExternalImages = True

    '                If ds.Tables(0).Rows.Count > 0 Then
    '                    rv.LocalReport.DataSources.Add(New ReportDataSource("CertificateReport", ds.Tables(0)))
    '                    rv.LocalReport.Refresh()
    '                Else
    '                    Me.lblMessage.Text = "No data available."
    '                    Me.gvSearch.DataSource = Nothing
    '                    Me.gvSearch.DataBind()
    '                    Throw New Exception("No data available.")
    '                End If

    '                'server report as PDF
    '                Dim streamIDs() As String
    '                Dim warnings() As Warning
    '                Dim bytes() As Byte
    '                Dim mimeType As String
    '                Dim encoding As String
    '                Dim extension As String

    '                bytes = rv.LocalReport.Render("PDF", Nothing, mimeType, encoding, extension, streamIDs, warnings)

    '                Response.Buffer = True
    '                Response.Clear()
    '                Response.ContentType = mimeType
    '                Response.AddHeader("content-disposition", "attachment; filename=Certificate_" & row.Cells.Item(kUserSummaryID).Text.ToString & "." & extension)
    '                Response.BinaryWrite(bytes)
    '                Response.Flush()
    '                Response.End()
    '            End If
    '        Catch ex1 As ThreadAbortException
    '        Catch ex As Exception

    '            If Not db Is Nothing Then db.Dispose()
    '            db = Nothing


    '            Dim sInnerException As String = ""
    '            If Not ex.InnerException Is Nothing Then
    '                sInnerException = sInnerException
    '            End If

    '            l.LogIt("gvSearch_RowCommand Error: " & ex.Source & vbCrLf & ex.Message & sInnerException)
    '        Finally

    '            If bDBIsNothing Then
    '                If Not db Is Nothing Then db.Dispose()
    '                db = Nothing
    '            End If

    '        End Try
    '    End Sub

End Class