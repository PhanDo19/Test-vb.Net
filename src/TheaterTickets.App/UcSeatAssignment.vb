Imports System.Windows.Forms
Imports System.Linq
Imports Npgsql

Public Class UcSeatAssignment
    Inherits UserControl

    Private cboBooking As ComboBox
    Private lblPerformance As Label
    Private lblCount As Label
    Private seatPanel As TableLayoutPanel
    Private btnSave As Button
    Private btnReload As Button
    Private lblAssigned As Label
    Private seatButtons As New Dictionary(Of String, Button)()
    Private tip As New ToolTip()

    Private bookingsTable As DataTable
    Private currentPerformanceId As Integer
    Private requiredSeats As Integer
    Private selectedSeats As New List(Of SeatPos)()

    Public Sub New()
        Me.Dock = DockStyle.Fill
        BuildUi()
        LoadBookings()
    End Sub

    Private Sub BuildUi()
        Dim main = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 1, .RowCount = 4, .Padding = New Padding(10)}
        main.RowStyles.Add(New RowStyle(SizeType.Absolute, 90))
        main.RowStyles.Add(New RowStyle(SizeType.Absolute, 50))
        main.RowStyles.Add(New RowStyle(SizeType.Percent, 100))
        main.RowStyles.Add(New RowStyle(SizeType.Absolute, 60))

        Dim header = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 3, .RowCount = 2}
        header.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 110))
        header.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 60))
        header.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 40))
        header.RowStyles.Add(New RowStyle(SizeType.Absolute, 40))
        header.RowStyles.Add(New RowStyle(SizeType.Absolute, 40))

        header.Controls.Add(New Label() With {.Text = "Booking", .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft}, 0, 0)
        cboBooking = New ComboBox() With {.Dock = DockStyle.Fill, .DropDownStyle = ComboBoxStyle.DropDownList, .DisplayMember = "display", .ValueMember = "id"}
        header.Controls.Add(cboBooking, 1, 0)
        Dim pnlReload = New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .FlowDirection = FlowDirection.LeftToRight, .WrapContents = False}
        btnReload = New Button() With {.Text = "Tải lại", .Width = 80}
        AddHandler btnReload.Click, Sub(s, e) LoadBookings()
        pnlReload.Controls.Add(btnReload)
        header.Controls.Add(pnlReload, 2, 0)

        header.Controls.Add(New Label() With {.Text = "Suất diễn", .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft}, 0, 1)
        lblPerformance = New Label() With {.Dock = DockStyle.Fill, .AutoSize = True, .TextAlign = ContentAlignment.MiddleLeft}
        header.Controls.Add(lblPerformance, 1, 1)

        lblCount = New Label() With {.Dock = DockStyle.Fill, .AutoSize = True, .Font = New Font("Segoe UI", 11, FontStyle.Bold), .TextAlign = ContentAlignment.MiddleRight}
        header.Controls.Add(lblCount, 2, 1)

        seatPanel = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 11, .RowCount = 11, .CellBorderStyle = TableLayoutPanelCellBorderStyle.Single}
        seatPanel.GetType().GetProperty("DoubleBuffered", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance).SetValue(seatPanel, True)
        seatPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 40))
        For i = 1 To 10 : seatPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 10)) : Next
        seatPanel.RowStyles.Add(New RowStyle(SizeType.Absolute, 30))
        For i = 1 To 10 : seatPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 10)) : Next

        For c = 1 To 10
            seatPanel.Controls.Add(New Label() With {.Text = c.ToString(), .TextAlign = ContentAlignment.MiddleCenter, .Dock = DockStyle.Fill}, c, 0)
        Next
        For r = 1 To 10
            Dim rowChar As Char = Chr(64 + r)
            seatPanel.Controls.Add(New Label() With {.Text = rowChar, .TextAlign = ContentAlignment.MiddleCenter, .Dock = DockStyle.Fill}, 0, r)
        Next
        ' Create seat buttons once
        For r = 1 To 10
            Dim rowChar As Char = Chr(64 + r)
            For c = 1 To 10
                Dim btn = New Button() With {.Dock = DockStyle.Fill, .Margin = New Padding(2), .Text = c.ToString(), .Tag = New SeatPos(rowChar, c)}
                AddHandler btn.Click, AddressOf OnSeatClick
                seatPanel.Controls.Add(btn, c, r)
                seatButtons(SeatKey(rowChar, c)) = btn
            Next
        Next

        Dim assignedPanel = New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .FlowDirection = FlowDirection.LeftToRight}
        assignedPanel.Controls.Add(New Label() With {.Text = "Ghế đã gán:", .AutoSize = True, .Margin = New Padding(0, 8, 6, 0)})
        lblAssigned = New Label() With {.AutoSize = True, .Font = New Font("Segoe UI", 10, FontStyle.Regular)}
        assignedPanel.Controls.Add(lblAssigned)

        Dim footer = New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .FlowDirection = FlowDirection.LeftToRight}
        footer.Controls.Add(New Label() With {.Text = "⬜ Trống  🟦 Đang chọn  🟥 Đã giữ", .AutoSize = True, .Margin = New Padding(5, 10, 20, 0)})
        btnSave = New Button() With {.Text = "Lưu", .Width = 100}
        AddHandler btnSave.Click, AddressOf OnSave
        footer.Controls.Add(btnSave)

        main.Controls.Add(header, 0, 0)
        main.Controls.Add(assignedPanel, 0, 1)
        main.Controls.Add(seatPanel, 0, 2)
        main.Controls.Add(footer, 0, 3)
        Me.Controls.Add(main)

        AddHandler cboBooking.SelectedIndexChanged, AddressOf OnBookingChanged
    End Sub

    Private Sub LoadBookings()
        Try
            bookingsTable = LoadBookingsWithSeatInfo()
            If Not bookingsTable.Columns.Contains("display") Then
                bookingsTable.Columns.Add("display", GetType(String), "Convert(id, 'System.String') + ' - ' + customer_name")
            End If
            cboBooking.DataSource = bookingsTable
        Catch ex As Exception
            MessageBox.Show($"Lỗi tải booking: {ex.Message}")
        End Try
    End Sub

    Private Sub OnBookingChanged(sender As Object, e As EventArgs)
        If cboBooking.SelectedValue Is Nothing Then Return
        Dim row = bookingsTable.Rows.Cast(Of DataRow)().FirstOrDefault(Function(r) CInt(r("id")) = CInt(cboBooking.SelectedValue))
        If row Is Nothing Then Return
        currentPerformanceId = CInt(row("performance_id"))
        requiredSeats = CInt(row("ticket_count"))
        lblPerformance.Text = $"{row("title")} ({Convert.ToDateTime(row("starts_at")).ToString("dd/MM HH:mm")})"
        lblCount.Text = $"{requiredSeats} ghế (đã gán {row("assigned_count")})"
        BuildSeatGrid()
    End Sub

    Private Sub BuildSeatGrid()
        selectedSeats.Clear()
        For Each btn In seatButtons.Values
            btn.Enabled = True
            btn.BackColor = SystemColors.Control
            tip.SetToolTip(btn, Nothing)
        Next

        Dim taken = LoadSeatAssignments(currentPerformanceId)
        Dim occupied As New Dictionary(Of String, Integer)()
        For Each r As DataRow In taken.Rows
            Dim key = SeatKey(r("seat_row").ToString(), CInt(r("seat_number")))
            occupied(key) = CInt(r("booking_id"))
        Next

        Dim currentId = CInt(cboBooking.SelectedValue)
        For Each kvp In seatButtons
            Dim key = kvp.Key
            Dim btn = kvp.Value
            If occupied.ContainsKey(key) Then
                If occupied(key) = currentId Then
                    btn.BackColor = Color.LightBlue
                    Dim sp = CType(btn.Tag, SeatPos)
                    selectedSeats.Add(New SeatPos(sp.RowChar, sp.Number))
                Else
                    btn.BackColor = Color.LightCoral
                    btn.Enabled = False
                    tip.SetToolTip(btn, "Ghế đã giữ bởi booking khác")
                End If
            End If
        Next
        RefreshAssignedLabel()
    End Sub

    Private Function SeatKey(rowChar As String, num As Integer) As String
        Return rowChar & num.ToString()
    End Function

    Private Sub OnSeatClick(sender As Object, e As EventArgs)
        Dim btn = CType(sender, Button)
        Dim seat = CType(btn.Tag, SeatPos)
        Dim exists = selectedSeats.Any(Function(s) s.RowChar = seat.RowChar AndAlso s.Number = seat.Number)
        If exists Then
            selectedSeats.RemoveAll(Function(s) s.RowChar = seat.RowChar AndAlso s.Number = seat.Number)
            btn.BackColor = SystemColors.Control
        Else
            If selectedSeats.Count >= requiredSeats Then
                MessageBox.Show($"Chỉ chọn tối đa {requiredSeats} ghế")
                Return
            End If
            selectedSeats.Add(New SeatPos(seat.RowChar, seat.Number))
            btn.BackColor = Color.LightBlue
        End If
        RefreshAssignedLabel()
    End Sub

    Private Sub OnSave(sender As Object, e As EventArgs)
        If cboBooking.SelectedValue Is Nothing Then
            MessageBox.Show("Chọn booking")
            Return
        End If
        If MessageBox.Show("Xác nhận lưu ghế cho booking này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Return
        End If
        Try
            SaveSeatAssignments(CInt(cboBooking.SelectedValue), currentPerformanceId, selectedSeats, requiredSeats)
            MessageBox.Show("Đã lưu ghế")
            LoadBookings()
        Catch ex As PostgresException
            MessageBox.Show($"Lỗi DB: {ex.MessageText}")
        Catch ex As Exception
            MessageBox.Show($"Lỗi lưu ghế: {ex.Message}")
        End Try
    End Sub

    Private Sub RefreshAssignedLabel()
        Dim textDisplay As String = If(selectedSeats.Count = 0, "(chưa chọn)",
            String.Join(", ", selectedSeats.OrderBy(Function(s) s.RowChar).ThenBy(Function(s) s.Number).Select(Function(s) $"{s.RowChar}{s.Number}")))
        lblAssigned.Text = textDisplay
    End Sub
End Class
