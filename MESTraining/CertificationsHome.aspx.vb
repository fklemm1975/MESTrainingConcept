Imports System.Threading
Imports Microsoft.Reporting.WebForms
Imports System.IO

Public Class CertificationHome
    Inherits System.Web.UI.Page

#Region "Properties"
    Private db As CertDB
    Private l As CertDBLib = New CertDBLib

    Private Const kUserSummaryID As Integer = 0
    Private Const kCertificationTypeID As Integer = 1
    Private Const kCertificationName As Integer = 2
    Private Const kStartDate As Integer = 3
    Private Const kExamComplete As Integer = 4
    Private Const kLastModule As Integer = 5
    Private Const kExamScore As Integer = 6
    Private Const kTakeExam As Integer = 7
    Private Const kPrint As Integer = 8

    '===== SIR 735 - We're getting rid of the drop down and hard coding this to RPC
    '09/16/2013 - for now hard code this to the UAT Cert, production will change to a 3 for RPC
    '===== The SelectAllCertificationsByUserID stored proc has the CertificationTypeID hard coded.
    '####
    Private iCertificationTypeID = My.Settings.CertificationTypeID
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim bDBIsNothing = db Is Nothing

        Try
            'Let's at least try to catch any error instantiating the db object
            If bDBIsNothing Then db = New CertDB


            If Not IsPostBack Then
                If Session("RegistrationID") Is Nothing Then
                    Response.Redirect("~/Login.aspx")
                End If



                '===== SIR 735 - Only one exam active at a time.  Hard code for RPC.  The following lines need to be commented out for production.  
                '===== Begin
                BuildDropDownList()

                If Me.ddlCertification.Items.Count > 0 Then
                    Me.ddlPanel1.Visible = True
                Else
                    Me.ddlPanel2.Visible = True
                End If
                '===== End


                '===== SIR 735 - Only one exam active at a time.  Hard code for RPC. 
                '===== Do we need the Start or the Continue button?  We're going to let the BindDataGrid handle this since it's already pulling the info we need.
                BindDataGrid()


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

    Private Sub BuildDropDownList()
        Try
            ddlCertification.DataTextField = "CertificationName"
            ddlCertification.DataValueField = "CertificationTypeID"
            ddlCertification.DataSource = db.SelectAllActiveCertificationTypeByUserID() 'Session("RegistrationID"))
            ddlCertification.DataBind()
        Catch ex As Exception
            Throw New Exception("Error building drop down list: " & ex.Message & ex.InnerException.Message)
        End Try
    End Sub

    Private Sub BindDataGrid()
        Try
            '===== SIR 735 - Only one exam active at a time.  Hard code for RPC. 
            '===== We're going to add some code to the stored proc here to deteremine if we need the Start or Continue button and what the URL is.
            '===== The SelectAllCertificationsByUserID stored proc has the CertificationTypeID hard coded.
            Dim ds As DataSet = db.SelectAllCertificationsByUserID(Session("RegistrationID"))
            '####

            Dim s As String = "greg"

            Session("ContinueURL") = ""

            'This is for the grid
            gvMyCertifications.DataSource = ds.Tables(0)
            gvMyCertifications.DataBind()

            'This is the toggle on the lbRPCCertification text
            If ds.Tables(1).Rows(0)("IncompletedCerts") > 0 Then
                ' Just let it always say start
                'Me.btStart.Text = "Continue"
                'Me.imgRPCCertification.ImageUrl = "Images/continue.png"
                'Me.lblIAgree.Visible = False
                Session("ContinueURL") = ds.Tables(2).Rows(0)("ContinueURL").ToString

            Else
                '2020.08.12 - GRP - This shouldn't matter anymore   Me.btStart.Text = "Start"
                'Me.imgRPCCertification.ImageUrl = "Images/IAgree.png"
                'Me.lblIAgree.Visible = True
            End If

        Catch ex As Exception

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            Throw New Exception("Error Binding Data Grid: " & ex.Message & sInnerException)
        End Try
    End Sub


    '    Private Sub btStart_Click(sender As Object, e As EventArgs) Handles btStart.Click
    Private Sub btStartNewCertification_Click(sender As Object, e As EventArgs) Handles btStartNewCertification.Click
        Dim bDBIsNothing = db Is Nothing
        Dim iRegistrationID As Integer


        Try
            If Not Session("RegistrationID").ToString Is Nothing Then

                If bDBIsNothing Then db = New CertDB


                Session("LastModule") = Nothing

                Dim ds As DataSet
                Dim bQuestionsSuccess As Boolean = False

                If Integer.TryParse(Session("RegistrationID").ToString, iRegistrationID) Then

                    'If Me.imgRPCCertification.ImageUrl = "Images/IAgree.png" Then
                    '2020.08.12 - GRP - This shouldn't matter anymore   
                    'If Me.btStart.Text = "Start" Then
                    If True Then
                        Dim dto As CertDto.UserSummaryDto = New CertDto.UserSummaryDto
                        Dim bItemsSuccess As Boolean = False

                        'at the time of starting to review the certification documentation copy the items and questions for that certification.
                        bItemsSuccess = db.CopyCurrentItemsForUser(iRegistrationID, ddlCertification.SelectedValue, 0) 'SIR 735
                        'bItemsSuccess = db.CopyCurrentItemsForUser(iRegistrationID, iCertificationTypeID, 0)            '2020.08.12 - GRP - Going back in 

                        '''' This can go right?   'bQuestionsSuccess = db.CopyCurrentQuestionsForUser(iRegistrationID, ddlCertification.SelectedValue, 0)

                        ds = db.SelectAllItemsOfTypeQuestionByCertificationTypeID(ddlCertification.SelectedValue)  'SIR 735
                        'ds = db.SelectAllItemsOfTypeQuestionByCertificationTypeID(iCertificationTypeID)         '2020.08.12 - GRP - Going back in                            

                        If ds.Tables(0).Rows.Count > 0 Then
                            For Each dr As DataRow In ds.Tables(0).Rows
                                If dr("NumberOfQuestions") > 0 Then '08/05/2013 - GRP - NumberOfQuestions has to be greater than zero or this bombs
                                    bQuestionsSuccess = db.CopyRandomQuestionsForUser(iRegistrationID, ddlCertification.SelectedValue, 0, dr("NumberOfQuestions"), dr("ItemID")) 'SIR 735
                                    'bQuestionsSuccess = db.CopyRandomQuestionsForUser(iRegistrationID, iCertificationTypeID, 0, dr("NumberOfQuestions"), dr("ItemID"))            '2020.08.12 - GRP - Going back in       
                                End If
                            Next
                        End If

                        ' ''copy Follow-Up Questions
                        ' ''04/09/2013 - GRP - Follow up questions are coming from the same pool as the exam questions
                        'bFollowUpQuestionsSuccess = db.CopyFollowUpQuestionsForUser(iRegistrationID, ddlCertification.SelectedValue)   


                        '04/15/2013 - GRP - We need this first.  There's no reason to pass a zero then come back and put this in.  Get the ID and pass it through
                        dto.CertificationTypeID = ddlCertification.SelectedValue   'SIR 735
                        'dto.CertificationTypeID = iCertificationTypeID              '2020.08.12 - GRP - Going back in      
                        dto.UserID = iRegistrationID
                        dto.ExamScore = 0
                        dto.StartDate = DateTime.Now
                        dto.EndDate = DBNull.Value
                        dto.NextModuleItemID = 0
                        dto.Active = 1
                        dto.CreatedByID = iRegistrationID
                        dto.LastModifiedID = iRegistrationID
                        dto.Save(db)




                        db.UpdateAllUserItemsWithUserSummaryID(iRegistrationID, ddlCertification.SelectedValue, dto.UserSummaryID)         'SIR 735
                        db.UpdateAllUserQuestionsWithUserSummaryID(iRegistrationID, ddlCertification.SelectedValue, dto.UserSummaryID)     'SIR 735
                        'db.UpdateAllUserItemsWithUserSummaryID(iRegistrationID, iCertificationTypeID, dto.UserSummaryID)              
                        'db.UpdateAllUserQuestionsWithUserSummaryID(iRegistrationID, iCertificationTypeID, dto.UserSummaryID)        '2020.08.12 - GRP - Going back in  

                        Response.Redirect("~/Certification.aspx?CertificationTypeID=" & ddlCertification.SelectedValue & "&UserSummaryID=" & dto.UserSummaryID)  'SIR 735
                        'Response.Redirect("~/Certification.aspx?CertificationTypeID=" & iCertificationTypeID & "&UserSummaryID=" & dto.UserSummaryID)                         '2020.08.12 - GRP - Going back in   

                        'ElseIf Me.imgRPCCertification.ImageUrl = "Images/continue.png" Then

                        '2020.08.12 - GRP - This shouldn't matter anymore   
                        'ElseIf Me.btStart.Text = "Continue" Then
                    ElseIf False Then

                        Response.Redirect(Session("ContinueURL"))
                    End If

                Else
                    Throw New Exception("lbRPCCertification_Click: Failed to convert Registration ID to an integer.")
                End If
            Else

                If Not db Is Nothing Then db.Dispose()
                db = Nothing

                Throw New Exception("lbRPCCertification_Click: Registration ID has no value.")
            End If


        Catch ex1 As ThreadAbortException
        Catch ex As Exception

            If Not db Is Nothing Then db.Dispose()
            db = Nothing


            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            l.LogIt("lbRPCCertification_Click Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)
        Finally

            If bDBIsNothing Then
                If Not db Is Nothing Then db.Dispose()
                db = Nothing
            End If

        End Try
    End Sub


    Private Sub gvMyCertifications_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMyCertifications.RowCommand
        Dim bDBIsNothing = db Is Nothing

        Dim sMsg As String = ""
        Dim iCount As Integer = 0

        Try
            If bDBIsNothing Then db = New CertDB

            If e.CommandName = "ViewCertificate" Then
                Dim index As Integer = CInt(e.CommandArgument)
                Dim row As GridViewRow = Me.gvMyCertifications.Rows(index)
                Dim ds As DataSet
                Dim iUserSummaryID As Integer = row.Cells.Item(kUserSummaryID).Text

                ds = db.ReportCertificate(iUserSummaryID)

                Dim rv As ReportViewer = New ReportViewer
                rv.LocalReport.ReportPath = "Reports/MarineCertificate.rdlc"
                rv.LocalReport.EnableExternalImages = True
                Dim dr As DataRow = ds.Tables(0).Rows(0)

                If ds.Tables(0).Rows.Count > 0 Then
                    rv.LocalReport.DataSources.Add(New ReportDataSource("DataSet1", ds.Tables(0)))
                    rv.LocalReport.Refresh()
                Else
                    Me.gvMyCertifications.DataSource = Nothing
                    Me.gvMyCertifications.DataBind()
                    Throw New Exception("No data available for " & iUserSummaryID.ToString & ".")
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
                Response.AddHeader("content-disposition", "attachment; filename=Certificate_" & row.Cells.Item(kUserSummaryID).Text.ToString & "." & extension)
                Response.BinaryWrite(bytes)
                Response.Flush()
                Response.End()
            End If
        Catch ex1 As ThreadAbortException
        Catch ex As Exception

            If Not db Is Nothing Then db.Dispose()
            db = Nothing


            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            l.LogIt("gvMyCertifications_RowCommand Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)
        Finally

            If bDBIsNothing Then
                If Not db Is Nothing Then db.Dispose()
                db = Nothing
            End If

        End Try
    End Sub

End Class