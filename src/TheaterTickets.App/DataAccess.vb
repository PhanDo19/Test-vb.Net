Imports System.Data
Imports Npgsql
Imports System.Configuration

Module DataAccess
    Private ReadOnly SeatPrices As New Dictionary(Of String, Decimal) From {
        {"standard", 100000D},
        {"vip", 150000D},
        {"double", 180000D}
    }

    Public Function GetConnectionString() As String
        Dim cs = ConfigurationManager.ConnectionStrings("DefaultConnection")
        If cs Is Nothing Then
            Throw New InvalidOperationException("Chưa cấu hình DefaultConnection trong App.config")
        End If
        Return cs.ConnectionString
    End Function

    Public Function GetConnection() As NpgsqlConnection
        Return New NpgsqlConnection(GetConnectionString())
    End Function

    Public Function GetSeatPrice(category As String) As Decimal
        If Not SeatPrices.ContainsKey(category) Then Return 0D
        Return SeatPrices(category)
    End Function

    Public Function LoadPerformances(nameFilter As String, fromDate As DateTime?, toDate As DateTime?) As DataTable
        Dim dt As New DataTable()
        Using conn = GetConnection(), cmd = conn.CreateCommand()
            Dim sql As String = "SELECT id, title, starts_at, duration_minutes FROM performances WHERE 1=1"
            If Not String.IsNullOrWhiteSpace(nameFilter) Then
                sql &= " AND LOWER(title) LIKE @name"
                cmd.Parameters.AddWithValue("@name", "%" & nameFilter.ToLower().Trim() & "%")
            End If
            If fromDate.HasValue Then
                sql &= " AND starts_at >= @from"
                cmd.Parameters.AddWithValue("@from", fromDate.Value)
            End If
            If toDate.HasValue Then
                sql &= " AND starts_at <= @to"
                cmd.Parameters.AddWithValue("@to", toDate.Value)
            End If
            sql &= " ORDER BY starts_at"
            cmd.CommandText = sql
            conn.Open()
            Using da As New NpgsqlDataAdapter(cmd)
                da.Fill(dt)
            End Using
        End Using
        Return dt
    End Function

    Public Sub InsertPerformance(title As String, startAt As DateTime, duration As Integer)
        Using conn = GetConnection(), cmd = conn.CreateCommand()
            cmd.CommandText = "INSERT INTO performances(title, starts_at, duration_minutes) VALUES(@t,@s,@d)"
            cmd.Parameters.AddWithValue("@t", title.Trim())
            cmd.Parameters.AddWithValue("@s", startAt)
            cmd.Parameters.AddWithValue("@d", duration)
            conn.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Sub UpdatePerformance(id As Integer, title As String, startAt As DateTime, duration As Integer)
        Using conn = GetConnection(), cmd = conn.CreateCommand()
            cmd.CommandText = "UPDATE performances SET title=@t, starts_at=@s, duration_minutes=@d WHERE id=@id"
            cmd.Parameters.AddWithValue("@t", title.Trim())
            cmd.Parameters.AddWithValue("@s", startAt)
            cmd.Parameters.AddWithValue("@d", duration)
            cmd.Parameters.AddWithValue("@id", id)
            conn.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Sub DeletePerformance(id As Integer)
        Using conn = GetConnection(), cmd = conn.CreateCommand()
            cmd.CommandText = "DELETE FROM performances WHERE id=@id"
            cmd.Parameters.AddWithValue("@id", id)
            conn.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Function LoadBookings(Optional searchTitle As String = Nothing) As DataTable
        Dim dt As New DataTable()
        Using conn = GetConnection(), cmd = conn.CreateCommand()
            Dim sql As String = "SELECT b.id, p.title AS performance, p.starts_at, b.customer_name, b.category, b.ticket_count, b.total_amount FROM bookings b JOIN performances p ON p.id=b.performance_id WHERE 1=1"
            If Not String.IsNullOrWhiteSpace(searchTitle) Then
                sql &= " AND LOWER(p.title) LIKE @title"
                cmd.Parameters.AddWithValue("@title", "%" & searchTitle.ToLower().Trim() & "%")
            End If
            sql &= " ORDER BY b.created_at DESC"
            cmd.CommandText = sql
            conn.Open()
            Using da As New NpgsqlDataAdapter(cmd)
                da.Fill(dt)
            End Using
        End Using
        Return dt
    End Function

    Public Function LoadPerformancesLookup() As DataTable
        Dim dt As New DataTable()
        Using conn = GetConnection(), cmd = conn.CreateCommand()
            cmd.CommandText = "SELECT id, title, starts_at FROM performances ORDER BY starts_at"
            conn.Open()
            Using da As New NpgsqlDataAdapter(cmd)
                da.Fill(dt)
            End Using
        End Using
        Return dt
    End Function

    Public Function InsertBooking(performanceId As Integer, customer As String, category As String, ticketCount As Integer) As Integer
        Dim price = GetSeatPrice(category)
        Dim total = price * ticketCount
        Using conn = GetConnection(), cmd = conn.CreateCommand()
            cmd.CommandText = "INSERT INTO bookings(performance_id, customer_name, category, ticket_count, total_amount) VALUES(@p,@c,@cat::seat_category,@n,@total) RETURNING id"
            cmd.Parameters.AddWithValue("@p", performanceId)
            cmd.Parameters.AddWithValue("@c", customer.Trim())
            cmd.Parameters.AddWithValue("@cat", category)
            cmd.Parameters.AddWithValue("@n", ticketCount)
            cmd.Parameters.AddWithValue("@total", total)
            conn.Open()
            Dim newId = CInt(cmd.ExecuteScalar())
            Return newId
        End Using
    End Function

    Public Function LoadBookingsWithSeatInfo() As DataTable
        Dim dt As New DataTable()
        Using conn = GetConnection(), cmd = conn.CreateCommand()
            cmd.CommandText = "SELECT b.id, b.customer_name, b.category, b.ticket_count, p.title, p.starts_at, COALESCE(COUNT(sa.id),0) AS assigned_count, b.performance_id FROM bookings b JOIN performances p ON p.id=b.performance_id LEFT JOIN seat_assignments sa ON sa.booking_id=b.id GROUP BY b.id, p.title, p.starts_at, b.performance_id"
            conn.Open()
            Using da As New NpgsqlDataAdapter(cmd)
                da.Fill(dt)
            End Using
        End Using
        Return dt
    End Function

    Public Function LoadSeatAssignments(performanceId As Integer) As DataTable
        Dim dt As New DataTable()
        Using conn = GetConnection(), cmd = conn.CreateCommand()
            cmd.CommandText = "SELECT booking_id, seat_row, seat_number FROM seat_assignments WHERE performance_id=@p"
            cmd.Parameters.AddWithValue("@p", performanceId)
            conn.Open()
            Using da As New NpgsqlDataAdapter(cmd)
                da.Fill(dt)
            End Using
        End Using
        Return dt
    End Function

    Public Function LoadAssignedSeats(bookingId As Integer) As List(Of String)
        Dim seats As New List(Of String)()
        Using conn = GetConnection(), cmd = conn.CreateCommand()
            cmd.CommandText = "SELECT seat_row, seat_number FROM seat_assignments WHERE booking_id=@b ORDER BY seat_row, seat_number"
            cmd.Parameters.AddWithValue("@b", bookingId)
            conn.Open()
            Using reader = cmd.ExecuteReader()
                While reader.Read()
                    seats.Add(reader.GetString(0) & reader.GetInt32(1).ToString())
                End While
            End Using
        End Using
        Return seats
    End Function

    Public Function LoadSeatStats() As DataTable
        Dim dt As New DataTable()
        Using conn = GetConnection(), cmd = conn.CreateCommand()
            cmd.CommandText = "
SELECT p.id,
       p.title,
       p.starts_at,
       SUM(CASE WHEN b.category = 'standard' THEN b.ticket_count ELSE 0 END) AS standard_tickets,
       SUM(CASE WHEN b.category = 'vip' THEN b.ticket_count ELSE 0 END) AS vip_tickets,
       SUM(CASE WHEN b.category = 'double' THEN b.ticket_count ELSE 0 END) AS double_tickets,
       SUM(b.ticket_count) AS total_tickets
FROM performances p
LEFT JOIN bookings b ON b.performance_id = p.id
GROUP BY p.id, p.title, p.starts_at
ORDER BY p.starts_at;"
            conn.Open()
            Using da As New NpgsqlDataAdapter(cmd)
                da.Fill(dt)
            End Using
        End Using
        Return dt
    End Function

    Public Sub SaveSeatAssignments(bookingId As Integer, performanceId As Integer, seats As List(Of SeatPos), expectedCount As Integer)
        If seats.Count <> expectedCount Then
            Throw New InvalidOperationException($"Cần chọn đủ {expectedCount} ghế")
        End If
        Using conn = GetConnection()
            conn.Open()
            Using tx = conn.BeginTransaction()
                Using delCmd = conn.CreateCommand()
                    delCmd.Transaction = tx
                    delCmd.CommandText = "DELETE FROM seat_assignments WHERE booking_id=@b"
                    delCmd.Parameters.AddWithValue("@b", bookingId)
                    delCmd.ExecuteNonQuery()
            End Using

            For Each seat In seats
                Using insCmd = conn.CreateCommand()
                    insCmd.Transaction = tx
                    insCmd.CommandText = "INSERT INTO seat_assignments(booking_id, performance_id, seat_row, seat_number) VALUES(@b,@p,@r,@n)"
                    insCmd.Parameters.AddWithValue("@b", bookingId)
                    insCmd.Parameters.AddWithValue("@p", performanceId)
                    insCmd.Parameters.AddWithValue("@r", seat.RowChar)
                    insCmd.Parameters.AddWithValue("@n", seat.Number)
                    insCmd.ExecuteNonQuery()
                End Using
            Next
            tx.Commit()
            End Using
        End Using
    End Sub

    Public Function LoadUsers() As DataTable
        Dim dt As New DataTable()
        Using conn = GetConnection(), cmd = conn.CreateCommand()
            cmd.CommandText = "
SELECT u.id,
       u.username,
       u.full_name,
       u.is_active,
       COALESCE(string_agg(r.code, ',' ORDER BY r.code), '') AS roles
FROM users u
LEFT JOIN user_roles ur ON ur.user_id = u.id
LEFT JOIN roles r ON r.id = ur.role_id
GROUP BY u.id, u.username, u.full_name, u.is_active
ORDER BY u.username;"
            conn.Open()
            Using da As New NpgsqlDataAdapter(cmd)
                da.Fill(dt)
            End Using
        End Using
        Return dt
    End Function

    Public Function LoadRoles() As DataTable
        Dim dt As New DataTable()
        Using conn = GetConnection(), cmd = conn.CreateCommand()
            cmd.CommandText = "SELECT id, code, name FROM roles ORDER BY id"
            conn.Open()
            Using da As New NpgsqlDataAdapter(cmd)
                da.Fill(dt)
            End Using
        End Using
        Return dt
    End Function

    Public Function LoadUserRoleIds(userId As Guid) As List(Of Integer)
        Dim roles As New List(Of Integer)()
        Using conn = GetConnection(), cmd = conn.CreateCommand()
            cmd.CommandText = "SELECT role_id FROM user_roles WHERE user_id = @u"
            cmd.Parameters.AddWithValue("@u", userId)
            conn.Open()
            Using reader = cmd.ExecuteReader()
                While reader.Read()
                    roles.Add(reader.GetInt32(0))
                End While
            End Using
        End Using
        Return roles
    End Function

    Public Function CreateUser(username As String, fullName As String, password As String, isActive As Boolean, roleIds As List(Of Integer)) As Guid
        Using conn = GetConnection()
            conn.Open()
            Using tx = conn.BeginTransaction()
                Dim userId As Guid
                Using cmd = conn.CreateCommand()
                    cmd.Transaction = tx
                    cmd.CommandText = "INSERT INTO users(username, full_name, password_hash, is_active) VALUES(@u,@f, crypt(@p, gen_salt('bf')), @a) RETURNING id"
                    cmd.Parameters.AddWithValue("@u", username.Trim())
                    cmd.Parameters.AddWithValue("@f", fullName.Trim())
                    cmd.Parameters.AddWithValue("@p", password)
                    cmd.Parameters.AddWithValue("@a", isActive)
                    userId = CType(cmd.ExecuteScalar(), Guid)
                End Using

                For Each roleId In roleIds
                    Using cmdRole = conn.CreateCommand()
                        cmdRole.Transaction = tx
                        cmdRole.CommandText = "INSERT INTO user_roles(user_id, role_id) VALUES(@u,@r)"
                        cmdRole.Parameters.AddWithValue("@u", userId)
                        cmdRole.Parameters.AddWithValue("@r", roleId)
                        cmdRole.ExecuteNonQuery()
                    End Using
                Next
                tx.Commit()
                Return userId
            End Using
        End Using
    End Function

    Public Sub UpdateUser(userId As Guid, fullName As String, isActive As Boolean, roleIds As List(Of Integer))
        Using conn = GetConnection()
            conn.Open()
            Using tx = conn.BeginTransaction()
                Using cmd = conn.CreateCommand()
                    cmd.Transaction = tx
                    cmd.CommandText = "UPDATE users SET full_name=@f, is_active=@a WHERE id=@u"
                    cmd.Parameters.AddWithValue("@f", fullName.Trim())
                    cmd.Parameters.AddWithValue("@a", isActive)
                    cmd.Parameters.AddWithValue("@u", userId)
                    cmd.ExecuteNonQuery()
                End Using

                Using delCmd = conn.CreateCommand()
                    delCmd.Transaction = tx
                    delCmd.CommandText = "DELETE FROM user_roles WHERE user_id=@u"
                    delCmd.Parameters.AddWithValue("@u", userId)
                    delCmd.ExecuteNonQuery()
                End Using

                For Each roleId In roleIds
                    Using cmdRole = conn.CreateCommand()
                        cmdRole.Transaction = tx
                        cmdRole.CommandText = "INSERT INTO user_roles(user_id, role_id) VALUES(@u,@r)"
                        cmdRole.Parameters.AddWithValue("@u", userId)
                        cmdRole.Parameters.AddWithValue("@r", roleId)
                        cmdRole.ExecuteNonQuery()
                    End Using
                Next
                tx.Commit()
            End Using
        End Using
    End Sub

    Public Sub UpdateUserPassword(userId As Guid, password As String)
        Using conn = GetConnection(), cmd = conn.CreateCommand()
            cmd.CommandText = "UPDATE users SET password_hash = crypt(@p, gen_salt('bf')) WHERE id=@u"
            cmd.Parameters.AddWithValue("@p", password)
            cmd.Parameters.AddWithValue("@u", userId)
            conn.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub
End Module
