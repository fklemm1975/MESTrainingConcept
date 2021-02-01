Imports System.Drawing
Imports System.Security.Cryptography
Imports System.Threading

Public Class Registration
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not IsPostBack Then
        '    If Session("RegistrationID") IsNot Nothing Then
        '        Dim db As CertDB

        '        Try
        '            db = New CertDB

        '            Dim ds As New DataSet()
        '            ds = db.SelectByIDPublicUserRegistration(Int32.Parse(Session("RegistrationID").ToString()))

        '            Dim dr As DataRow = ds.Tables(0).Rows(0)

        '            txtFName.Text = dr("FirstName").ToString()
        '            txtMName.Text = dr("MiddleName").ToString()
        '            txtLName.Text = dr("LastName").ToString()

        '            txtEmailAddress.Text = dr("EmailAddress").ToString()
        '            txtPassword.Text = ""

        '            txtWorkPhone.Text = dr("WorkPhone").ToString()
        '            txtMobilePhone.Text = dr("MobilePhone").ToString()
        '            txtHomePHone.Text = dr("HomePHone").ToString()
        '            txtFaxNumber.Text = dr("Fax").ToString()

        '            txtDriverLicense.Text = dr("DriverLicence").ToString()
        '            txtCompany.Text = dr("CompanyName").ToString()
        '            txtJobResponsibility.Text = dr("JobResponsibility").ToString()

        '            txtStreet.Text = dr("Street").ToString()
        '            txtCity.Text = dr("City").ToString()
        '            txtState.Text = dr("State").ToString()
        '            txtZipCode.Text = dr("Zip").ToString()
        '            txtCounty.Text = dr("County").ToString()
        '            If dr("UseCompressedVideos") Then
        '                Me.Compressed.Checked = True
        '            Else
        '                Me.Highspeed.Checked = True
        '            End If

        '            imgRegister.ImageUrl = "../Images/update.png"

        '        Catch ex As Exception

        '            Dim sInnerException As String = ""
        '            'If Not ex.InnerException Is Nothing Then
        '            '    sInnerException = sInnerException
        '            'End If

        '            Dim l As CertDBLib = New CertDBLib
        '            l.LogIt("Registration PageLoad Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)

        '            If Not db Is Nothing Then db.Dispose()
        '            db = Nothing

        '        Finally
        '            If Not db Is Nothing Then db.Dispose()
        '            db = Nothing

        '        End Try

        '    Else
        '        txtFirstName.Focus()

        '        imgRegister.ImageUrl = "../Images/register.png"
        '    End If
        'End If
    End Sub

    Protected Sub lbRegister_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Dim db As CertDB

        'Try
        '    db = New CertDB

        '    Dim dto As New CertDto.PublicUserRegistrationDto()

        '    dto.FirstName = txtFirstName.Text
        '    dto.MiddleName = txtMiddleName.Text
        '    dto.LastName = txtLastName.Text

        '    dto.EmailAddress = txtEmailAddress.Text
        '    dto.Password = SHA1Managed.Create().ComputeHash(ASCIIEncoding.ASCII.GetBytes(txtPassword.Text))
        '    dto.TempPassword = SHA1Managed.Create().ComputeHash(ASCIIEncoding.ASCII.GetBytes(txtPassword.Text))

        '    dto.WorkPhone = txtWorkPhone.Text
        '    dto.MobilePhone = txtMobilePhone.Text
        '    dto.HomePhone = txtHomePHone.Text
        '    dto.Fax = txtFaxNumber.Text

        '    dto.DriverLicence = txtDriverLicense.Text
        '    dto.CompanyName = txtCompany.Text
        '    dto.JobResponsibility = txtJobResponsibility.Text

        '    dto.Street = txtStreet.Text
        '    dto.City = txtCity.Text
        '    dto.State = txtState.Text
        '    dto.Zip = txtZipCode.Text
        '    dto.County = txtCounty.Text
        '    dto.UseCompressedVideos = Me.Compressed.Checked
        '    dto.RegisteredBy = Request.UserHostName.ToString()
        '    dto.RegisteredFrom = Request.UserHostAddress.ToString()
        '    dto.RegisteredAt = DateTime.Now

        '    dto.UpdatedBy = ""
        '    dto.UpdatedFrom = ""
        '    dto.UpdatedAt = DateTime.Now

        '    If Session("RegistrationID") Is Nothing Then
        '        Dim iRegistrationID As Integer

        '        Try
        '            If dto.Save(db) Then
        '                iRegistrationID = dto.PublicUserRegistrationID

        '                If iRegistrationID > 0 Then
        '                    Dim ds As DataSet = db.SelectByIDPublicUserRegistration(iRegistrationID)
        '                    Dim dr As DataRow

        '                    If ds.Tables(0).Rows.Count > 0 Then
        '                        dr = ds.Tables(0).Rows(0)

        '                        Session("RegistrationID") = iRegistrationID.ToString

        '                        Session("LastName") = dr("LastName").ToString
        '                        Session("FirstName") = dr("FirstName").ToString
        '                        Session("FullName") = Session("FirstName") + " " + Session("LastName")
        '                        Session("UseCompressedVideos") = dr("UseCompressedVideos")
        '                    End If
        '                    Response.Redirect("~/Default.aspx")
        '                Else
        '                    lblMessage.ForeColor = Color.Red
        '                    lblMessage.Text = "Registration Failed"
        '                End If
        '            Else
        '                Throw New Exception("Save method failed: btnRegister_Click")
        '            End If
        '        Catch ex1 As ThreadAbortException

        '        Catch ex As Exception
        '            lblMessage.ForeColor = Color.Red
        '            lblMessage.Text = "Error: " + ex.Message
        '        End Try
        '    Else
        '        dto.PublicUserRegistrationID = Int32.Parse(Session("RegistrationID").ToString())

        '        If dto.Save Then
        '            Response.Redirect("~/Default.aspx")
        '        Else
        '            lblMessage.ForeColor = Color.Red
        '            lblMessage.Text = "Failed to update your registration"
        '        End If
        '    End If


        'Catch ex1 As ThreadAbortException
        '    'Response.Redirect() will throw this error, we don't need to do anything with it, so we'll capture it here, instead of in the most general exception below
        '    'do nothing

        'Catch ex As Exception

        '    Dim sInnerException As String = ""
        '    'If Not ex.InnerException Is Nothing Then
        '    '    sInnerException = sInnerException
        '    'End If

        '    Dim l As CertDBLib = New CertDBLib

        '    l.LogIt("lbRegister_Click: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)

        '    If Not db Is Nothing Then db.Dispose()
        '    db = Nothing

        'Finally

        '    If Not db Is Nothing Then db.Dispose()
        '    db = Nothing

        'End Try

    End Sub

End Class