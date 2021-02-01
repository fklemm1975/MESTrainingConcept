Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Linq
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class SlimBanner
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(sender As Object, e As EventArgs)
        If Not IsPostBack Then
            'Sets all Configurations for Branding that are associated with the Slim Banner Master Page.

            'Agency Title.  Branding Step #3
            If ConfigurationManager.AppSettings("AgencyTitleImage") IsNot Nothing Then
                AgencyTitleImage.Src = ConfigurationManager.AppSettings("AgencyTitleImage").ToString()
            End If
            If ConfigurationManager.AppSettings("AgencyTitleLink") IsNot Nothing Then
                AgencyTitleLink.HRef = ConfigurationManager.AppSettings("AgencyTitleLink").ToString()
            End If
            If ConfigurationManager.AppSettings("AgencyTitleAlternateText") IsNot Nothing Then
                AgencyTitleImage.Alt = ConfigurationManager.AppSettings("AgencyTitleAlternateText").ToString()

                'NO Agency Logo.  Branding Step #5 can be skipped

                'Social Media Buttons Configuration.  Branding Step #6
                'Config is set Nav-SocialMedia.xml file

                'Site Collection Name For Search.  Branding Step #7.
                'Config in the Web.config

                'Branding Step 8.  NO Statewide & Agency Trends/Alert so it may be skipped

            End If
        End If
        'end if postback
    End Sub

    'Event Handler for someone clicking on our Search Button
    Protected Sub Search_Click(sender As Object, e As EventArgs)
        'If we have no input in search box, don't search.
        If searchInputBox.Text = [String].Empty Then
            Return
        Else
            Master.Search(searchInputBox.Text)
        End If
    End Sub

End Class