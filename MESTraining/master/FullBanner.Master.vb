Public Class FullBanner
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            'Sets all Configurations for Branding that are associated with the Full Banner Master Page.

            'Agency Title.  Branding Step #3.  Config in Web.Config
            If ConfigurationManager.AppSettings("AgencyTitleImage") IsNot Nothing Then
                AgencyTitleImage.Src = ConfigurationManager.AppSettings("AgencyTitleImage").ToString()
            End If
            If ConfigurationManager.AppSettings("AgencyTitleLink") IsNot Nothing Then
                AgencyTitleLink.HRef = ConfigurationManager.AppSettings("AgencyTitleLink").ToString()
            End If
            If ConfigurationManager.AppSettings("AgencyTitleAlternateText") IsNot Nothing Then
                AgencyTitleImage.Alt = ConfigurationManager.AppSettings("AgencyTitleAlternateText").ToString()
            End If

            'Agency Logo.  Branding Step #5.  Config in Web.Config
            If ConfigurationManager.AppSettings("AgencyLogoImage") IsNot Nothing Then
                AgencyLogoImage.Src = ConfigurationManager.AppSettings("AgencyLogoImage").ToString()
            End If
            If ConfigurationManager.AppSettings("AgencyLogoLink") IsNot Nothing Then
                AgencyLogoLink.HRef = ConfigurationManager.AppSettings("AgencyLogoLink").ToString()
            End If
            If ConfigurationManager.AppSettings("AgencyLogoAlternateText") IsNot Nothing Then
                AgencyLogoImage.Alt = ConfigurationManager.AppSettings("AgencyLogoAlternateText").ToString()
            End If


            'Social Media Buttons Configuration for Header.  Branding Step #6
            'Set in the Nav-SocialMedia.xml file

            'Site Collection Name For Search.  Branding Step #7.
            'Config in Web.Config

            'Branding Step #8.  Agency Alert Panel.
            'Set Visible="true" in HTML to enable display of Alerts for Agency.
        End If
    End Sub

    'Event Handler for someone clicking on our Search Button
    Protected Sub Search_Click(sender As Object, e As EventArgs) Handles searchButton.Click
        'If we have no input in search box, don't search.
        If searchInputBox.Text = [String].Empty Then
            Return
        Else
            Master.Search(searchInputBox.Text)
        End If
    End Sub

End Class