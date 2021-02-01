Imports System.Drawing
Imports System.Security.Cryptography
Imports System.Threading


Public Class MESRegistrationNew
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        lblMessage.Text = ""


        If Not IsPostBack Then
            If Session("RegistrationID") IsNot Nothing Then
                Dim db As CertDB

                Try
                    Dim dto As New CertDto.UserDto()

                    db = New CertDB
                    dto.Populate(Session("RegistrationID"), db)

                    ' 10/29/19 - GRP - Just populate the dto object

                    'Dim ds As New DataSet()
                    'ds = db.SelectByIDPublicUserRegistration(Int32.Parse(Session("RegistrationID").ToString()))

                    'Dim dr As DataRow = ds.Tables(0).Rows(0)

                    txtFirstName.Text = dto.FName
                    txtMiddleName.Text = dto.MName
                    txtLastName.Text = dto.LName

                    txtEmailAddress.Text = dto.EmailAddress
                    txtPassword.Text = ""

                    txtWorkPhone.Text = dto.WorkPhone
                    txtMobilePhone.Text = dto.MobilePhone
                    txtCompany.Text = dto.CompanyName
                    txtStreet.Text = dto.Street
                    txtCity.Text = dto.City
                    txtState.Text = dto.State
                    txtZipCode.Text = dto.ZipCode
                    Me.Compressed.Checked = dto.UseCompressedVideos

                    If dto.UseCompressedVideos Then
                        Me.Compressed.Checked = True
                    Else
                        Me.Highspeed.Checked = True
                    End If

                    'ibRegister.ImageUrl = "../Images/update.png"
                    btRegister.Text = "Update"

                Catch ex As Exception

                    Dim sInnerException As String = ""
                    If Not ex.InnerException Is Nothing Then
                        sInnerException = sInnerException
                    End If

                    Dim l As CertDBLib = New CertDBLib
                    l.LogIt("Registration PageLoad Error: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)

                    If Not db Is Nothing Then db.Dispose()
                    db = Nothing

                Finally
                    If Not db Is Nothing Then db.Dispose()
                    db = Nothing

                End Try

            Else
                txtFirstName.Focus()

                'ibRegister.ImageUrl = "../Images/register.png"
                btRegister.Text = "Register"

            End If
        End If
    End Sub

    'Private Sub ibRegister_Click(sender As Object, e As ImageClickEventArgs) Handles ibRegister.Click
    '    Dim db As CertDB

    '    Try
    '        db = New CertDB

    '        Dim dto As New CertDto.UserDto()

    '        dto.FName = txtFirstName.Text
    '        dto.MName = txtMiddleName.Text
    '        dto.LName = txtLastName.Text

    '        dto.EmailAddress = txtEmailAddress.Text
    '        dto.Password = SHA1Managed.Create().ComputeHash(ASCIIEncoding.ASCII.GetBytes(txtPassword.Text))
    '        'dto.TempPassword = SHA1Managed.Create().ComputeHash(ASCIIEncoding.ASCII.GetBytes(txtPassword.Text))


    '        dto.WorkPhone = txtWorkPhone.Text
    '        dto.MobilePhone = txtMobilePhone.Text
    '        dto.CompanyName = txtCompany.Text

    '        dto.Street = txtStreet.Text
    '        dto.City = txtCity.Text
    '        dto.State = txtState.Text
    '        dto.ZipCode = txtZipCode.Text

    '        dto.UseCompressedVideos = Me.Compressed.Checked

    '        dto.CreatedByID = 0     'Request.UserHostName.ToString()
    '        dto.CreatedByDate = DateTime.Now

    '        dto.LastModifiedByID = 0        '""
    '        dto.LastModifiedDate = DateTime.Now

    '        If Session("RegistrationID") Is Nothing Then
    '            Dim iRegistrationID As Integer

    '            Try
    '                If dto.Save(db) Then
    '                    iRegistrationID = dto.UserID

    '                    If iRegistrationID > 0 Then
    '                        Dim ds As DataSet = db.SelectByIDPublicUserRegistration(iRegistrationID)
    '                        Dim dr As DataRow

    '                        If ds.Tables(0).Rows.Count > 0 Then
    '                            dr = ds.Tables(0).Rows(0)

    '                            Session("RegistrationID") = iRegistrationID.ToString

    '                            Session("LastName") = dr("LastName").ToString
    '                            Session("FirstName") = dr("FirstName").ToString
    '                            Session("FullName") = Session("FirstName") + " " + Session("LastName")
    '                            Session("UseCompressedVideos") = dr("UseCompressedVideos")
    '                        End If
    '                        Response.Redirect("~/Default.aspx")
    '                    Else
    '                        lblMessage.ForeColor = Color.Red
    '                        lblMessage.Text = "Registration Failed"
    '                    End If
    '                Else
    '                    Throw New Exception("Save method failed: btnRegister_Click")
    '                End If
    '            Catch ex1 As ThreadAbortException

    '            Catch ex As Exception
    '                lblMessage.ForeColor = Color.Red
    '                lblMessage.Text = "Error: " + ex.Message
    '            End Try
    '        Else
    '            dto.UserID = Int32.Parse(Session("RegistrationID").ToString())

    '            If dto.Save Then
    '                Response.Redirect("~/Default.aspx")
    '            Else
    '                lblMessage.ForeColor = Color.Red
    '                lblMessage.Text = "Failed to update your registration"
    '            End If
    '        End If


    '    Catch ex1 As ThreadAbortException
    '        'Response.Redirect() will throw this error, we don't need to do anything with it, so we'll capture it here, instead of in the most general exception below
    '        'do nothing

    '    Catch ex As Exception

    '        Dim sInnerException As String = ""
    '        If Not ex.InnerException Is Nothing Then
    '            sInnerException = sInnerException
    '        End If

    '        Dim l As CertDBLib = New CertDBLib

    '        lblMessage.Text = "Error: " & ex.Message

    '        l.LogIt("lbRegister_Click: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)

    '        If Not db Is Nothing Then db.Dispose()
    '        db = Nothing

    '    Finally

    '        If Not db Is Nothing Then db.Dispose()
    '        db = Nothing

    '    End Try
    'End Sub

    Private Sub btRegister_Click(sender As Object, e As EventArgs) Handles btRegister.Click
        Dim db As CertDB
        Dim sMsg As String = ""

        Try
            db = New CertDB

            Dim dto As New CertDto.UserDto()

            If VerifyRequiredFields(sMsg) Then

                dto.FName = txtFirstName.Text
                dto.MName = txtMiddleName.Text
                dto.LName = txtLastName.Text

                dto.EmailAddress = txtEmailAddress.Text
                dto.Password = SHA1Managed.Create().ComputeHash(ASCIIEncoding.ASCII.GetBytes(txtPassword.Text))
                'dto.TempPassword = SHA1Managed.Create().ComputeHash(ASCIIEncoding.ASCII.GetBytes(txtPassword.Text))


                dto.WorkPhone = txtWorkPhone.Text
                dto.MobilePhone = txtMobilePhone.Text
                dto.CompanyName = txtCompany.Text

                dto.Street = txtStreet.Text
                dto.City = txtCity.Text
                dto.State = txtState.Text
                dto.ZipCode = txtZipCode.Text

                dto.UseCompressedVideos = Me.Compressed.Checked

                dto.CreatedByID = 0     'Request.UserHostName.ToString()
                dto.CreatedByDate = DateTime.Now

                dto.LastModifiedByID = 0        '""
                dto.LastModifiedDate = DateTime.Now

                If Session("RegistrationID") Is Nothing Then
                    Dim iRegistrationID As Integer

                    Try
                        If dto.Save(db) Then
                            iRegistrationID = dto.UserID

                            If iRegistrationID > 0 Then
                                Dim ds As DataSet = db.SelectByIDPublicUserRegistration(iRegistrationID)
                                Dim dr As DataRow

                                If ds.Tables(0).Rows.Count > 0 Then
                                    dr = ds.Tables(0).Rows(0)

                                    Session("RegistrationID") = iRegistrationID.ToString

                                    Session("LastName") = dr("LastName").ToString
                                    Session("FirstName") = dr("FirstName").ToString
                                    Session("FullName") = Session("FirstName") + " " + Session("LastName")
                                    Session("UseCompressedVideos") = False
                                End If
                                Response.Redirect("~/Default.aspx")
                            Else
                                lblMessage.ForeColor = Color.Red
                                lblMessage.Text = "Registration Failed"
                            End If
                        Else
                            Throw New Exception("Save method failed: btnRegister_Click")
                        End If
                    Catch ex1 As ThreadAbortException

                    Catch ex As Exception
                        lblMessage.ForeColor = Color.Red
                        lblMessage.Text = "Error: " + ex.Message
                    End Try
                Else
                    dto.UserID = Int32.Parse(Session("RegistrationID").ToString())

                    If dto.Save Then
                        Response.Redirect("~/Default.aspx")
                    Else
                        lblMessage.ForeColor = Color.Red
                        lblMessage.Text = "Failed to update your registration"
                    End If
                End If

            Else

            End If



        Catch ex1 As ThreadAbortException
            'Response.Redirect() will throw this error, we don't need to do anything with it, so we'll capture it here, instead of in the most general exception below
            'do nothing

        Catch ex As Exception

            Dim sInnerException As String = ""
            If Not ex.InnerException Is Nothing Then
                sInnerException = sInnerException
            End If

            Dim l As CertDBLib = New CertDBLib

            lblMessage.Text = "Error: " & ex.Message

            l.LogIt("lbRegister_Click: " & ex.Source & vbCrLf & vbCrLf & ex.Message & sInnerException)

            If Not db Is Nothing Then db.Dispose()
            db = Nothing

        Finally

            If Not db Is Nothing Then db.Dispose()
            db = Nothing

        End Try
    End Sub

    Public Function VerifyRequiredFields(ByRef sMsg As String) As Boolean
        Dim retval As Boolean = True

        Try
            'First clear out all of the error messages, if any.
            Me.lblFirstNameMsg.Text = ""
            Me.lblLastNameMsg.Text = ""
            Me.lblEmailAddressMsg.Text = ""
            Me.lblPasswordMsg.Text = ""
            Me.lblConfirmPasswordMsg.Text = ""
            Me.lblCompanyNameMsg.Text = ""


            If Me.txtFirstName.Text.Trim.Length = 0 Then
                retval = False
                Me.lblFirstNameMsg.Text = "First Name is required."
            End If

            If Me.txtLastName.Text.Trim.Length = 0 Then
                retval = False
                Me.lblLastNameMsg.Text = "Last Name is required."
            End If

            If Me.txtEmailAddress.Text.Trim.Length = 0 Then
                retval = False
                Me.lblEmailAddressMsg.Text = "Email is required."
            End If

            If Me.txtPassword.Text.Trim.Length = 0 Then
                retval = False
                Me.lblPasswordMsg.Text = "Password is required."
            End If

            If Me.txtConfirmPassword.Text.Trim.Length = 0 Then
                retval = False
                Me.lblConfirmPasswordMsg.Text = "Confirm Password is required."
            End If

            If Me.txtPassword.Text.Trim.Length > 0 And Me.txtConfirmPassword.Text.Trim.Length > 0 Then
                If Me.txtPassword.Text.Trim <> Me.txtConfirmPassword.Text.Trim Then
                    retval = False
                    Me.lblPasswordMsg.Text = "Confirm Password does not match Password."
                End If
            End If

            If Me.txtCompany.Text.Trim.Length = 0 Then
                retval = False
                Me.lblCompanyNameMsg.Text = "Company is required."
            End If

        Catch ex As Exception
            retval = False
            Throw New Exception("Error verifying required fields: " & ex.Message)
        End Try

        Return retval

    End Function

End Class

