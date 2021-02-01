Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Configuration
Imports System.Data
Imports System.Text

Public Class Main
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Sets all Configurations for Branding that are associated with the Main Master Page.

            'Social Media Buttons Configuration.  Branding Step #6
            'Links Set in the Nav-SocialMedia.xml file

            'Footer Link Customization.  Branding Steps #12.
            'Config set in Nav-Footer.sitemap

            'Footer Phone and Email.  Branding Step #13.  Config set in Web.Config
            If Not (ConfigurationManager.AppSettings("FooterAddress") Is Nothing) Then
                FooterAddress.InnerText = ConfigurationManager.AppSettings("FooterAddress").ToString()
            End If
            If Not (ConfigurationManager.AppSettings("FooterPhone") Is Nothing) Then
                FooterPhone.InnerText = ConfigurationManager.AppSettings("FooterPhone").ToString()
            End If
            'Binds all our ~ links in the Head Section of the Page.
            Page.Header.DataBind()
        End If
    End Sub

    Public Sub Search(searchText As String)
        If searchText = String.Empty Then Return
        Dim siteToSearch As String = "gpedakpy1y0"  'Maryland.gov statewide
        Dim friendlyName As String = String.Empty

        'Set our Site Collection Name.  If null or empty, set it to statewide.
        If ConfigurationManager.AppSettings("SiteCollectionName") IsNot Nothing Then
            siteToSearch = ConfigurationManager.AppSettings("SiteCollectionName").ToString()
        Else
            siteToSearch = String.Empty
        End If

        'Set our Site Friendly Name.  If null or empty, it won't be appended.
        If ConfigurationManager.AppSettings("AgencyName") IsNot Nothing Then
            friendlyName = ConfigurationManager.AppSettings("AgencyName").ToString()
        Else
            friendlyName = String.Empty
        End If

        Dim url As StringBuilder = New StringBuilder("http://www.maryland.gov/pages/search.aspx?q=")
        url.Append(searchText)

        'Appends our Search Collection
        If siteToSearch <> String.Empty Then
            url.Append("&site=")
            url.Append(siteToSearch)
        End If

        'Sets the Friendly name shown on the Search page
        If friendlyName <> String.Empty Then
            url.Append("&name=")
            url.Append(friendlyName)
        End If

        Response.Redirect(url.ToString(), True)  'Transfer to the search page
    End Sub
End Class