Imports System.Random


Public Class Player
    Public Property Name As String
    Public Property index As Integer
    Dim rand As New Random


    Public Sub New(name As String)


        Me.Name = name
        Me.index = rand.Next()


    End Sub

    Public Overrides Function ToString() As String
        Return Me.Name
    End Function

    Public Sub changeIndex(pl As Player)
        pl.index = rand.next
    End Sub


End Class
