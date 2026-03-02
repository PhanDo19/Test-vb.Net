Imports Npgsql
Imports System.Collections.Generic

Public Module AuthService
    Public Property CurrentUser As UserSession

    Private _permissions As HashSet(Of String) = New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

    Public Function IsLoggedIn() As Boolean
        Return CurrentUser IsNot Nothing
    End Function

    Public Function Login(username As String, password As String) As Boolean
        Using conn = GetConnection(), cmd = conn.CreateCommand()
            cmd.CommandText = "SELECT id, username, COALESCE(full_name,'') AS full_name FROM users WHERE username=@u AND is_active = TRUE AND password_hash = crypt(@p, password_hash)"
            cmd.Parameters.AddWithValue("@u", username.Trim())
            cmd.Parameters.AddWithValue("@p", password)
            conn.Open()
            Using reader = cmd.ExecuteReader()
                If Not reader.Read() Then
                    Return False
                End If
                CurrentUser = New UserSession() With {
                    .Id = reader.GetGuid(0),
                    .Username = reader.GetString(1),
                    .FullName = reader.GetString(2)
                }
            End Using
        End Using
        LoadPermissions(CurrentUser.Id)
        Return True
    End Function

    Public Sub Logout()
        CurrentUser = Nothing
        _permissions = New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
    End Sub

    Public Function HasPermission(code As String) As Boolean
        If String.IsNullOrWhiteSpace(code) Then Return False
        Return _permissions.Contains(code)
    End Function

    Public Sub RequirePermission(code As String)
        If Not HasPermission(code) Then
            Throw New UnauthorizedAccessException("Bạn không có quyền thực hiện thao tác này.")
        End If
    End Sub

    Private Sub LoadPermissions(userId As Guid)
        _permissions = New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        Using conn = GetConnection(), cmd = conn.CreateCommand()
            cmd.CommandText = "
SELECT DISTINCT p.code
FROM user_roles ur
JOIN role_permissions rp ON rp.role_id = ur.role_id
JOIN permissions p ON p.id = rp.permission_id
WHERE ur.user_id = @uid;"
            cmd.Parameters.AddWithValue("@uid", userId)
            conn.Open()
            Using reader = cmd.ExecuteReader()
                While reader.Read()
                    _permissions.Add(reader.GetString(0))
                End While
            End Using
        End Using
    End Sub

    Public Function CurrentPermissions() As IReadOnlyCollection(Of String)
        Return _permissions
    End Function
End Module
