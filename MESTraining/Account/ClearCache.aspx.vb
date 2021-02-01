Public Class ClearCache
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' From - http://www.aspdotnetfaq.com/Faq/How-to-clear-your-ASP-NET-applications-Cache.aspx

        Dim keys As List(Of String) = New List(Of String)
        ' retrieve application Cache enumerator
        Dim enumerator As IDictionaryEnumerator = Cache.GetEnumerator
        ' copy all keys that currently exist in Cache

        While enumerator.MoveNext
            keys.Add(enumerator.Key.ToString)

        End While
        ' delete every key from cache
        Dim i As Integer = 0
        Do While (i < keys.Count)
            Cache.Remove(keys(i))
            i = (i + 1)
        Loop
    End Sub

End Class