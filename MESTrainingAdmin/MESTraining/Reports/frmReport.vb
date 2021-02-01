Imports Microsoft.Reporting.WinForms
Imports System.IO

Public Class frmReport

#Region "Properties"

    Private dwsdb As daWSDB
    Private l As CertLib = New CertLib
    Private sReportType As String
    Private iUserSummaryID As Integer
    Private sLastReportRun As String
    Private sLastReportParameter As String
    Private objLastReportStartDate As Object
    Private objLastReportEndDate As Object
    Private iSortOrder As Integer = 1
    Private iCertificationTypeID As Integer = 0
    Private dtStartDate As DateTime
    Private dtEndDate As DateTime
    Private sCertificateName As String = ""

    Public Property ReportType As String
        Get
            ReportType = sReportType
        End Get
        Set(ByVal value As String)
            sReportType = value
        End Set
    End Property

    Public Property UserSummaryID As Integer
        Get
            UserSummaryID = iUserSummaryID
        End Get
        Set(ByVal value As Integer)
            iUserSummaryID = value
        End Set
    End Property

    Public Property LastReportRun As String
        Get
            LastReportRun = sLastReportRun
        End Get
        Set(ByVal value As String)
            sLastReportRun = value
        End Set
    End Property

    Public Property LastReportParameter As String
        Get
            LastReportParameter = sLastReportParameter
        End Get
        Set(ByVal value As String)
            sLastReportParameter = value
        End Set
    End Property

    Public Property LastReportStartDate As Object
        Get
            If objLastReportStartDate Is Nothing Then
                LastReportStartDate = DBNull.Value
            Else
                LastReportStartDate = objLastReportStartDate
            End If
        End Get
        Set(ByVal value As Object)
            Dim x As DateTime
            If Not value Is DBNull.Value And Not value Is Nothing Then
                If DateTime.TryParse(value.ToString, x) Then
                    objLastReportStartDate = x
                Else
                    Throw New Exception("LastReportStartDate - Invalid data type.  '" & value.ToString & "' is not of type DateTime.")
                End If
            Else
                objLastReportStartDate = Nothing
            End If
        End Set
    End Property

    Public Property LastReportEndDate As Object
        Get
            If objLastReportEndDate Is Nothing Then
                LastReportEndDate = DBNull.Value
            Else
                LastReportEndDate = objLastReportEndDate
            End If
        End Get
        Set(ByVal value As Object)
            Dim x As DateTime
            If Not value Is DBNull.Value And Not value Is Nothing Then
                If DateTime.TryParse(value.ToString, x) Then
                    objLastReportEndDate = x
                Else
                    Throw New Exception("LastReportEndDate - Invalid data type.  '" & value.ToString & "' is not of type DateTime.")
                End If
            Else
                objLastReportEndDate = Nothing
            End If
        End Set
    End Property

    Public Property SortOrder() As Integer
        Get
            SortOrder = iSortOrder
        End Get
        Set(ByVal value As Integer)
            iSortOrder = value
        End Set
    End Property

    Public Property CertificationTypeID() As Integer
        Get
            CertificationTypeID = iCertificationTypeID
        End Get
        Set(ByVal value As Integer)
            iCertificationTypeID = value
        End Set
    End Property

    Public Property StartDate() As DateTime
        Get
            StartDate = dtStartDate
        End Get
        Set(ByVal value As DateTime)
            dtStartDate = value
        End Set
    End Property

    Public Property EndDate() As DateTime
        Get
            EndDate = dtEndDate
        End Get
        Set(ByVal value As DateTime)
            dtEndDate = value
        End Set
    End Property

    Public Property CertificateName() As String
        Get
            CertificateName = sCertificateName
        End Get
        Set(ByVal value As String)
            sCertificateName = value
        End Set
    End Property


#End Region

    Private Sub frmReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Me.Cursor = Cursors.WaitCursor

            Select Case ReportType
                Case "Certificate"
                    BuildCertificateReport()
                Case "Grid Results"
                    BuildGridResultsReport()
                Case "Expiring Certificates"
                    BuildExpiringCertificatesReport()
                Case Else

            End Select

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            l.LogIt("Report Load Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub BuildCertificateReport()
        Dim bDBIsNothing = dwsdb Is Nothing
        If bDBIsNothing Then dwsdb = New daWSDB
        Dim ds As DataSet

        Try
            ds = dwsdb.ReportCertificate(Me.UserSummaryID)

            Dim rv As ReportViewer = New ReportViewer()
            rv.Dock = DockStyle.Fill
            Me.Panel1.Controls.Add(rv)

            rv.LocalReport.ReportPath = Application.StartupPath & "\Reports\" & "RPCCertificate.rdlc"
            rv.LocalReport.DataSources.Add(New ReportDataSource("CertificateReport", ds.Tables(0)))

            Dim dr As DataRow = ds.Tables(0).Rows(0)

            rv.RefreshReport()

            'Generate the pdf file
            Dim warnings() As Warning
            Dim streamIDs() As String
            Dim mimeType As String
            Dim encoding As String
            Dim extension As String
            Dim bytes() As Byte

            Dim PDFFilePath As String = Application.StartupPath & "\PDFs\"
            If Not Directory.Exists(PDFFilePath) Then
                Directory.CreateDirectory(PDFFilePath)
            End If

            Dim fname As String = "Certification" & dr("CertificationNumber") & ".pdf"

            PDFFilePath += fname

            'The DeviceInfo settings should be changed based on the reportType
            'http://msdn2.microsoft.com/en-us/library/ms155397.aspx
            Dim deviceInfo As String = "<DeviceInfo>" & _
                                       "  <OutputFormat>PDF</OutputFormat>" & _
                                       "  <PageWidth>11in</PageWidth>" & _
                                       "  <PageHeight>8.5in</PageHeight>" & _
                                       "  <MarginTop>0in</MarginTop>" & _
                                       "  <MarginLeft>0in</MarginLeft>" & _
                                       "  <MarginRight>0in</MarginRight>" & _
                                       "  <MarginBottom>0in</MarginBottom>" & _
                                       "</DeviceInfo>"

            bytes = rv.LocalReport.Render("PDF", deviceInfo, mimeType, encoding, extension, streamIDs, warnings)

            Dim fs As FileStream = New FileStream(PDFFilePath, FileMode.Create)
            fs.Write(bytes, 0, bytes.Length)
            fs.Close()

            'open certificate directly in PDF.
            System.Diagnostics.Process.Start(PDFFilePath)
        Catch ex As Exception
            Throw New Exception("BuildCertificateReport Error: " & ex.Message)
        Finally
            If bDBIsNothing Then dwsdb = Nothing
        End Try
    End Sub

    Private Sub BuildGridResultsReport()
        Dim bDBIsNothing = dwsdb Is Nothing
        If bDBIsNothing Then dwsdb = New daWSDB
        Dim ds As DataSet
        Dim ReportPath As String = ""

        Try
            Dim rv As ReportViewer = New ReportViewer()
            rv.Dock = DockStyle.Fill
            Me.Panel1.Controls.Add(rv)

            Dim paramList As Generic.List(Of ReportParameter) = New Generic.List(Of ReportParameter)

            Select Case LastReportRun
                Case "All"
                    ds = dwsdb.SelectAllExamsForReporting()
                    paramList.Add(New ReportParameter("Title", "All Exams"))
                Case "Passed Only"
                    ds = dwsdb.SelectAllPassedExamsForReporting()
                    paramList.Add(New ReportParameter("Title", "All Passed Exams"))
                Case "Failed Only"
                    ds = dwsdb.SelectAllFailedExamsForReporting()
                    paramList.Add(New ReportParameter("Title", "All Failed Exams"))
                Case "Active"
                    ds = dwsdb.SelectAllActiveExamsForReporting()
                    paramList.Add(New ReportParameter("Title", "All Active Certificates"))
                Case "First Name"
                    ds = dwsdb.SelectAllExamsByFirstName(Me.LastReportParameter)
                    paramList.Add(New ReportParameter("Title", "All Exams by First Name"))
                    paramList.Add(New ReportParameter("ReportParameter", Me.LastReportParameter))
                    paramList.Add(New ReportParameter("ReportParameterName", "First Name:"))
                Case "Last Name"
                    ds = dwsdb.SelectAllExamsByLastName(Me.LastReportParameter)
                    paramList.Add(New ReportParameter("Title", "All Exams by Last Name"))
                    paramList.Add(New ReportParameter("ReportParameter", Me.LastReportParameter))
                    paramList.Add(New ReportParameter("ReportParameterName", "Last Name:"))
                Case "Date Range"
                    ds = dwsdb.SelectAllExamsByDateRange(Me.LastReportStartDate, Me.LastReportEndDate)
                    paramList.Add(New ReportParameter("Title", "All Exams by Date Range"))
                    paramList.Add(New ReportParameter("StartDate", FormatDateTime(CDate(Me.LastReportStartDate.ToString), DateFormat.ShortDate)))
                    paramList.Add(New ReportParameter("EndDate", FormatDateTime(CDate(Me.LastReportEndDate.ToString), DateFormat.ShortDate)))
                    paramList.Add(New ReportParameter("ReportParameterNameStartDate", "Start Date:"))
                    paramList.Add(New ReportParameter("ReportParameterNameEndDate", "End Date:"))
                Case "Certificate Number"
                    ds = dwsdb.SelectAllExamsByCertificateNumber(Me.LastReportParameter)
                    paramList.Add(New ReportParameter("Title", "All Exams by Certificate Number"))
                    paramList.Add(New ReportParameter("ReportParameter", Me.LastReportParameter))
                    paramList.Add(New ReportParameter("ReportParameterName", "Certificate Number:"))
            End Select

            rv.LocalReport.ReportPath = Application.StartupPath & "\Reports\" & "GridResults.rdlc"
            rv.LocalReport.DataSources.Add(New ReportDataSource("DataSet1", ds.Tables(0)))
            rv.LocalReport.SetParameters(paramList)

            rv.RefreshReport()
        Catch ex As Exception
            Throw New Exception("BuildGridResultsReport Error: " & ex.Message)
        Finally
            If bDBIsNothing Then dwsdb = Nothing
        End Try
    End Sub

    Private Sub BuildExpiringCertificatesReport()
        Dim bDBIsNothing = dwsdb Is Nothing
        If bDBIsNothing Then dwsdb = New daWSDB
        Dim ds As DataSet
        Dim paramList As Generic.List(Of ReportParameter) = New Generic.List(Of ReportParameter)

        Try
            'Get the data
            ds = dwsdb.ReportExpiringCertificates(Me.CertificationTypeID, Me.StartDate, Me.EndDate, Me.SortOrder)


            'Initiate ReportViewer object
            Dim rv As ReportViewer = New ReportViewer()
            rv.Dock = DockStyle.Fill
            Me.Panel1.Controls.Add(rv)

            rv.LocalReport.ReportPath = Application.StartupPath & "\Reports\" & "ExpiringCertificates.rdlc"
            rv.LocalReport.DataSources.Add(New ReportDataSource("DataSet1", ds.Tables(0)))
            rv.LocalReport.DisplayName = "Expiring Certificates Report"

            paramList.Add(New ReportParameter("CertificateName", Me.CertificateName))
            rv.LocalReport.SetParameters(paramList)

            rv.RefreshReport()


        Catch ex As Exception
            Throw New Exception("BuildExpiringCertificatesReport Error: " & ex.Message)
        Finally
            If bDBIsNothing Then dwsdb = Nothing
        End Try
    End Sub

End Class