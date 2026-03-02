Public Class SeatPos
    Public Property RowChar As Char
    Public Property Number As Integer

    Public Sub New(rowChar As Char, number As Integer)
        Me.RowChar = rowChar
        Me.Number = number
    End Sub
End Class

Public Class UserSession
    Public Property Id As Guid
    Public Property Username As String
    Public Property FullName As String
End Class
