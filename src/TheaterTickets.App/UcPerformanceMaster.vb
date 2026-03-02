Imports System.Windows.Forms

Public Class UcPerformanceMaster
    Inherits UserControl

    Private txtTitle As TextBox
    Private dtStart As DateTimePicker
    Private numDuration As NumericUpDown
    Private dgv As DataGridView
    Private txtSearch As TextBox
    Private dtFrom As DateTimePicker
    Private dtTo As DateTimePicker
    Private btnAdd As Button
    Private btnUpdate As Button
    Private btnDelete As Button
    Private btnClear As Button
    Private btnSearch As Button
    Private _selectedId As Integer = -1
    Private ReadOnly canManage As Boolean = AuthService.HasPermission("PERFORMANCE_MANAGE")

    Public Sub New()
        Me.Dock = DockStyle.Fill
        BuildUi()
        ApplyPermissions()
        LoadGrid()
    End Sub

    Private Sub BuildUi()
        Dim main = New TableLayoutPanel() With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 1,
            .RowCount = 3,
            .Padding = New Padding(12)
        }
        main.RowStyles.Add(New RowStyle(SizeType.Absolute, 170)) ' info + CRUD
        main.RowStyles.Add(New RowStyle(SizeType.Absolute, 70))  ' search
        main.RowStyles.Add(New RowStyle(SizeType.Percent, 100))  ' grid

        ' Group thông tin + CRUD
        Dim groupInfo = New GroupBox() With {.Dock = DockStyle.Fill, .Text = "Thông tin suất diễn"}
        Dim infoLayout = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 4, .RowCount = 2, .Padding = New Padding(6)}
        infoLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 120))
        infoLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 20))
        infoLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 120))
        infoLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 20))
        infoLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 55))
        infoLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 45))

        infoLayout.Controls.Add(New Label() With {.Text = "Tên vở diễn", .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft}, 0, 0)
        txtTitle = New TextBox() With {.Dock = DockStyle.Fill, .Margin = New Padding(4, 6, 4, 6)}
        infoLayout.Controls.Add(txtTitle, 1, 0)
        infoLayout.SetColumnSpan(txtTitle, 3)

        infoLayout.Controls.Add(New Label() With {.Text = "Thời gian bắt đầu", .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft}, 0, 1)
        dtStart = New DateTimePicker() With {.Format = DateTimePickerFormat.Custom, .CustomFormat = "yyyy-MM-dd HH:mm", .Dock = DockStyle.Fill, .Margin = New Padding(4, 2, 4, 2)}
        infoLayout.Controls.Add(dtStart, 1, 1)

        infoLayout.Controls.Add(New Label() With {.Text = "Thời lượng (phút)", .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft}, 2, 1)
        numDuration = New NumericUpDown() With {.Minimum = 10, .Maximum = 600, .Value = 90, .Dock = DockStyle.Fill, .Margin = New Padding(4, 2, 4, 2)}
        infoLayout.Controls.Add(numDuration, 3, 1)

        Dim actions = New FlowLayoutPanel() With {.Dock = DockStyle.Bottom, .FlowDirection = FlowDirection.LeftToRight, .WrapContents = False, .Height = 40, .Padding = New Padding(0, 5, 0, 0)}
        btnAdd = New Button() With {.Text = "Thêm mới", .Width = 100}
        btnUpdate = New Button() With {.Text = "Sửa", .Width = 80}
        btnDelete = New Button() With {.Text = "Xóa", .Width = 80}
        btnClear = New Button() With {.Text = "Làm mới", .Width = 90}
        AddHandler btnAdd.Click, AddressOf OnAdd
        AddHandler btnUpdate.Click, AddressOf OnUpdate
        AddHandler btnDelete.Click, AddressOf OnDelete
        AddHandler btnClear.Click, AddressOf OnClear
        actions.Controls.AddRange({btnAdd, btnUpdate, btnDelete, btnClear})

        groupInfo.Controls.Add(infoLayout)
        groupInfo.Controls.Add(actions)

        ' Group tìm kiếm
        Dim groupSearch = New GroupBox() With {.Dock = DockStyle.Fill, .Text = "Tìm kiếm"}
        Dim search = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 7, .RowCount = 1, .Padding = New Padding(6)}
        search.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 60))
        search.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 40))
        search.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 30))
        search.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 120))
        search.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 35))
        search.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 140))
        search.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 80))

        search.Controls.Add(New Label() With {.Text = "Tìm tên", .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft}, 0, 0)
        txtSearch = New TextBox() With {.Dock = DockStyle.Fill}
        search.Controls.Add(txtSearch, 1, 0)
        search.Controls.Add(New Label() With {.Text = "Từ", .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleCenter}, 2, 0)
        dtFrom = New DateTimePicker() With {.Format = DateTimePickerFormat.Short, .ShowCheckBox = True, .Dock = DockStyle.Fill}
        search.Controls.Add(dtFrom, 3, 0)
        search.Controls.Add(New Label() With {.Text = "Đến", .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleCenter}, 4, 0)
        dtTo = New DateTimePicker() With {.Format = DateTimePickerFormat.Short, .ShowCheckBox = True, .Dock = DockStyle.Fill}
        search.Controls.Add(dtTo, 5, 0)
        btnSearch = New Button() With {.Text = "Tìm", .Dock = DockStyle.Fill}
        AddHandler btnSearch.Click, AddressOf OnSearch
        search.Controls.Add(btnSearch, 6, 0)
        groupSearch.Controls.Add(search)

        dgv = New DataGridView() With {.Dock = DockStyle.Fill, .ReadOnly = True, .AllowUserToAddRows = False, .SelectionMode = DataGridViewSelectionMode.FullRowSelect, .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill}
        AddHandler dgv.SelectionChanged, AddressOf OnRowSelected

        main.Controls.Add(groupInfo, 0, 0)
        main.Controls.Add(groupSearch, 0, 1)
        main.Controls.Add(dgv, 0, 2)
        Me.Controls.Add(main)
    End Sub

    Private Sub ApplyPermissions()
        btnAdd.Enabled = canManage
        btnUpdate.Enabled = canManage
        btnDelete.Enabled = canManage
    End Sub

    Private Sub LoadGrid()
        Try
            Dim fromDate As DateTime? = If(dtFrom IsNot Nothing AndAlso dtFrom.Checked, dtFrom.Value, Nothing)
            Dim toDate As DateTime? = If(dtTo IsNot Nothing AndAlso dtTo.Checked, dtTo.Value, Nothing)
            dgv.DataSource = LoadPerformances(txtSearch?.Text, fromDate, toDate)
            If dgv.Columns.Contains("id") Then dgv.Columns("id").HeaderText = "Mã"
            If dgv.Columns.Contains("title") Then dgv.Columns("title").HeaderText = "Tên vở diễn"
            If dgv.Columns.Contains("starts_at") Then dgv.Columns("starts_at").HeaderText = "Thời gian"
            If dgv.Columns.Contains("duration_minutes") Then dgv.Columns("duration_minutes").HeaderText = "Thời lượng (phút)"
            For Each col As DataGridViewColumn In dgv.Columns
                col.SortMode = DataGridViewColumnSortMode.Automatic
            Next
        Catch ex As Exception
            MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}")
        End Try
    End Sub

    Private Function ValidateInput() As Boolean
        If String.IsNullOrWhiteSpace(txtTitle.Text) Then
            MessageBox.Show("Nhập tên vở diễn")
            Return False
        End If
        If numDuration.Value <= 0 Then
            MessageBox.Show("Thời lượng phải > 0")
            Return False
        End If
        Return True
    End Function

    Private Sub OnAdd(sender As Object, e As EventArgs)
        If Not canManage Then
            MessageBox.Show("Bạn không có quyền thêm/sửa suất diễn.")
            Return
        End If
        If Not ValidateInput() Then Return
        Try
            InsertPerformance(txtTitle.Text, dtStart.Value, CInt(numDuration.Value))
            LoadGrid()
            OnClear(Nothing, EventArgs.Empty)
        Catch ex As Exception
            MessageBox.Show($"Lỗi thêm mới: {ex.Message}")
        End Try
    End Sub

    Private Sub OnUpdate(sender As Object, e As EventArgs)
        If Not canManage Then
            MessageBox.Show("Bạn không có quyền thêm/sửa suất diễn.")
            Return
        End If
        If _selectedId <= 0 Then
            MessageBox.Show("Chọn bản ghi cần sửa")
            Return
        End If
        If Not ValidateInput() Then Return
        Try
            UpdatePerformance(_selectedId, txtTitle.Text, dtStart.Value, CInt(numDuration.Value))
            LoadGrid()
        Catch ex As Exception
            MessageBox.Show($"Lỗi cập nhật: {ex.Message}")
        End Try
    End Sub

    Private Sub OnDelete(sender As Object, e As EventArgs)
        If Not canManage Then
            MessageBox.Show("Bạn không có quyền xóa suất diễn.")
            Return
        End If
        If _selectedId <= 0 Then
            MessageBox.Show("Chọn bản ghi cần xóa")
            Return
        End If
        If MessageBox.Show("Xóa suất diễn?", "Xác nhận", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            Try
                DeletePerformance(_selectedId)
                LoadGrid()
                OnClear(Nothing, EventArgs.Empty)
            Catch ex As Exception
                MessageBox.Show($"Lỗi xóa: {ex.Message}")
            End Try
        End If
    End Sub

    Private Sub OnClear(sender As Object, e As EventArgs)
        _selectedId = -1
        txtTitle.Text = String.Empty
        dtStart.Value = DateTime.Now
        numDuration.Value = 90
        dgv.ClearSelection()
    End Sub

    Private Sub OnSearch(sender As Object, e As EventArgs)
        LoadGrid()
    End Sub

    Private Sub OnRowSelected(sender As Object, e As EventArgs)
        If dgv.CurrentRow Is Nothing OrElse dgv.CurrentRow.DataBoundItem Is Nothing Then Return
        Dim row = CType(dgv.CurrentRow.DataBoundItem, DataRowView)
        _selectedId = Convert.ToInt32(row("id"))
        txtTitle.Text = row("title").ToString()
        dtStart.Value = Convert.ToDateTime(row("starts_at"))
        numDuration.Value = Convert.ToDecimal(row("duration_minutes"))
    End Sub
End Class
