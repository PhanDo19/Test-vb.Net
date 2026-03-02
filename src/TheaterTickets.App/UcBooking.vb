Imports System.Data
Imports System.Windows.Forms
Imports Npgsql

Public Class UcBooking
    Inherits UserControl

    Private cboPerformance As ComboBox
    Private txtCustomer As TextBox
    Private cboCategory As ComboBox
    Private numTickets As NumericUpDown
    Private lblTotal As Label
    Private btnBook As Button
    Private dgv As DataGridView
    Private txtSearch As TextBox

    Public Sub New()
        Me.Dock = DockStyle.Fill
        BuildUi()
        LoadPerformancesToCombo()
        LoadGrid()
    End Sub

    Private Sub BuildUi()
        Dim main = New TableLayoutPanel() With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 1,
            .RowCount = 3,
            .Padding = New Padding(10)
        }
        main.RowStyles.Add(New RowStyle(SizeType.Absolute, 200))
        main.RowStyles.Add(New RowStyle(SizeType.Absolute, 60))
        main.RowStyles.Add(New RowStyle(SizeType.Percent, 100))

        Dim form = New TableLayoutPanel() With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 2,
            .RowCount = 5,
            .Padding = New Padding(5)
        }
        form.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 180))
        form.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))

        form.Controls.Add(New Label() With {.Text = "Suất diễn", .TextAlign = ContentAlignment.MiddleLeft, .Dock = DockStyle.Fill}, 0, 0)
        cboPerformance = New ComboBox() With {.Dock = DockStyle.Fill, .DropDownStyle = ComboBoxStyle.DropDownList, .DisplayMember = "title", .ValueMember = "id"}
        form.Controls.Add(cboPerformance, 1, 0)

        form.Controls.Add(New Label() With {.Text = "Tên khách hàng", .TextAlign = ContentAlignment.MiddleLeft, .Dock = DockStyle.Fill}, 0, 1)
        txtCustomer = New TextBox() With {.Dock = DockStyle.Fill}
        form.Controls.Add(txtCustomer, 1, 1)

        form.Controls.Add(New Label() With {.Text = "Loại ghế", .TextAlign = ContentAlignment.MiddleLeft, .Dock = DockStyle.Fill}, 0, 2)
        cboCategory = New ComboBox() With {.Dock = DockStyle.Left, .DropDownStyle = ComboBoxStyle.DropDownList, .Width = 200}
        cboCategory.Items.AddRange(New Object() {"standard", "vip", "double"})
        cboCategory.SelectedIndex = 0
        form.Controls.Add(cboCategory, 1, 2)

        form.Controls.Add(New Label() With {.Text = "Số lượng vé", .TextAlign = ContentAlignment.MiddleLeft, .Dock = DockStyle.Fill}, 0, 3)
        numTickets = New NumericUpDown() With {.Dock = DockStyle.Left, .Minimum = 1, .Maximum = 100, .Value = 1, .Width = 120}
        form.Controls.Add(numTickets, 1, 3)

        form.Controls.Add(New Label() With {.Text = "Tổng tiền", .TextAlign = ContentAlignment.MiddleLeft, .Dock = DockStyle.Fill}, 0, 4)
        lblTotal = New Label() With {.Dock = DockStyle.Left, .AutoSize = True, .Font = New Font("Segoe UI", 11, FontStyle.Bold)}
        form.Controls.Add(lblTotal, 1, 4)

        AddHandler cboCategory.SelectedIndexChanged, AddressOf Recalc
        AddHandler numTickets.ValueChanged, AddressOf Recalc
        Recalc(Nothing, EventArgs.Empty)

        Dim search = New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .FlowDirection = FlowDirection.LeftToRight}
        search.Controls.Add(New Label() With {.Text = "Tìm suất diễn", .AutoSize = True, .Margin = New Padding(0, 8, 5, 0)})
        txtSearch = New TextBox() With {.Width = 220}
        search.Controls.Add(txtSearch)
        Dim btnSearch = New Button() With {.Text = "Tìm", .Width = 80}
        AddHandler btnSearch.Click, Sub(s, e) LoadGrid()
        search.Controls.Add(btnSearch)
        btnBook = New Button() With {.Text = "Đặt vé", .Width = 100, .Margin = New Padding(20, 3, 3, 3)}
        AddHandler btnBook.Click, AddressOf OnBook
        search.Controls.Add(btnBook)

        dgv = New DataGridView() With {
            .Dock = DockStyle.Fill,
            .ReadOnly = True,
            .AllowUserToAddRows = False,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        }

        main.Controls.Add(form, 0, 0)
        main.Controls.Add(search, 0, 1)
        main.Controls.Add(dgv, 0, 2)
        Me.Controls.Add(main)
    End Sub

    Private Sub LoadPerformancesToCombo()
        Try
            Dim dt = LoadPerformancesLookup()
            cboPerformance.DataSource = dt
        Catch ex As Exception
            MessageBox.Show($"Lỗi tải suất diễn: {ex.Message}")
        End Try
    End Sub

    Private Sub LoadGrid()
        Try
            dgv.DataSource = LoadBookings(txtSearch.Text)
            If dgv.Columns.Contains("id") Then dgv.Columns("id").HeaderText = "Mã"
            If dgv.Columns.Contains("performance") Then dgv.Columns("performance").HeaderText = "Suất diễn"
            If dgv.Columns.Contains("starts_at") Then dgv.Columns("starts_at").HeaderText = "Thời gian"
            If dgv.Columns.Contains("customer_name") Then dgv.Columns("customer_name").HeaderText = "Khách hàng"
            If dgv.Columns.Contains("category") Then dgv.Columns("category").HeaderText = "Loại ghế"
            If dgv.Columns.Contains("ticket_count") Then dgv.Columns("ticket_count").HeaderText = "Số vé"
            If dgv.Columns.Contains("total_amount") Then dgv.Columns("total_amount").HeaderText = "Tổng tiền"
            For Each col As DataGridViewColumn In dgv.Columns
                col.SortMode = DataGridViewColumnSortMode.Automatic
            Next
        Catch ex As Exception
            MessageBox.Show($"Lỗi tải booking: {ex.Message}")
        End Try
    End Sub

    Private Sub Recalc(sender As Object, e As EventArgs)
        Dim cat = cboCategory.SelectedItem.ToString()
        Dim price = GetSeatPrice(cat)
        Dim total = price * numTickets.Value
        lblTotal.Text = total.ToString("N0")
    End Sub

    Private Function ValidateInput() As Boolean
        If cboPerformance.SelectedValue Is Nothing Then
            MessageBox.Show("Chọn suất diễn")
            Return False
        End If
        If String.IsNullOrWhiteSpace(txtCustomer.Text) Then
            MessageBox.Show("Nhập tên khách hàng")
            Return False
        End If
        Return True
    End Function

    Private Sub OnBook(sender As Object, e As EventArgs)
        If Not ValidateInput() Then Return
        If MessageBox.Show("Xác nhận đặt vé?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Return
        End If
        Try
            Dim newId = InsertBooking(CInt(cboPerformance.SelectedValue), txtCustomer.Text, cboCategory.SelectedItem.ToString(), CInt(numTickets.Value))
            MessageBox.Show($"Đặt vé thành công. Booking ID: {newId}")
            LoadGrid()
        Catch ex As PostgresException
            MessageBox.Show($"Lỗi DB: {ex.MessageText}")
        Catch ex As Exception
            MessageBox.Show($"Lỗi đặt vé: {ex.Message}")
        End Try
    End Sub
End Class
