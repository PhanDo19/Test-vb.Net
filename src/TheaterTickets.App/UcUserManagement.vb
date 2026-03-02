Imports System.Data
Imports System.Windows.Forms

Public Class UcUserManagement
    Inherits UserControl

    Private dgv As DataGridView
    Private txtUsername As TextBox
    Private txtFullName As TextBox
    Private txtPassword As TextBox
    Private chkActive As CheckBox
    Private clbRoles As CheckedListBox
    Private btnAdd As Button
    Private btnUpdate As Button
    Private btnReset As Button
    Private lblHint As Label

    Private rolesTable As DataTable
    Private selectedUserId As Guid = Guid.Empty

    Public Sub New()
        Me.Dock = DockStyle.Fill
        BuildUi()
        RefreshRoles()
        RefreshUsers()
    End Sub

    Private Sub BuildUi()
        Dim main = New TableLayoutPanel() With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 1,
            .RowCount = 2,
            .Padding = New Padding(12)
        }
        main.RowStyles.Add(New RowStyle(SizeType.Absolute, 220))
        main.RowStyles.Add(New RowStyle(SizeType.Percent, 100))

        Dim group = New GroupBox() With {.Dock = DockStyle.Fill, .Text = "Quản lý người dùng"}
        Dim form = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 4, .RowCount = 4, .Padding = New Padding(6)}
        form.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 120))
        form.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 40))
        form.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 120))
        form.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 60))
        form.RowStyles.Add(New RowStyle(SizeType.Absolute, 32))
        form.RowStyles.Add(New RowStyle(SizeType.Absolute, 32))
        form.RowStyles.Add(New RowStyle(SizeType.Absolute, 32))
        form.RowStyles.Add(New RowStyle(SizeType.Percent, 100))

        form.Controls.Add(New Label() With {.Text = "Tài khoản", .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft}, 0, 0)
        txtUsername = New TextBox() With {.Dock = DockStyle.Fill}
        form.Controls.Add(txtUsername, 1, 0)

        form.Controls.Add(New Label() With {.Text = "Họ tên", .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft}, 0, 1)
        txtFullName = New TextBox() With {.Dock = DockStyle.Fill}
        form.Controls.Add(txtFullName, 1, 1)

        form.Controls.Add(New Label() With {.Text = "Mật khẩu", .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft}, 0, 2)
        txtPassword = New TextBox() With {.Dock = DockStyle.Fill, .UseSystemPasswordChar = True}
        form.Controls.Add(txtPassword, 1, 2)

        chkActive = New CheckBox() With {.Text = "Hoạt động", .Checked = True, .Dock = DockStyle.Left}
        form.Controls.Add(chkActive, 1, 3)

        form.Controls.Add(New Label() With {.Text = "Vai trò", .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft}, 2, 0)
        clbRoles = New CheckedListBox() With {.Dock = DockStyle.Fill, .CheckOnClick = True}
        form.Controls.Add(clbRoles, 3, 0)
        form.SetRowSpan(clbRoles, 3)

        lblHint = New Label() With {.Text = "Để trống mật khẩu nếu không đổi.", .Dock = DockStyle.Fill, .ForeColor = Color.DimGray}
        form.Controls.Add(lblHint, 3, 3)

        Dim actions = New FlowLayoutPanel() With {.Dock = DockStyle.Bottom, .FlowDirection = FlowDirection.LeftToRight, .Height = 40}
        btnAdd = New Button() With {.Text = "Thêm", .Width = 90}
        btnUpdate = New Button() With {.Text = "Cập nhật", .Width = 90}
        btnReset = New Button() With {.Text = "Làm mới", .Width = 90}
        AddHandler btnAdd.Click, AddressOf OnAdd
        AddHandler btnUpdate.Click, AddressOf OnUpdate
        AddHandler btnReset.Click, AddressOf OnReset
        actions.Controls.AddRange({btnAdd, btnUpdate, btnReset})

        group.Controls.Add(form)
        group.Controls.Add(actions)

        dgv = New DataGridView() With {
            .Dock = DockStyle.Fill,
            .ReadOnly = True,
            .AllowUserToAddRows = False,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        }
        AddHandler dgv.SelectionChanged, AddressOf OnRowSelected

        main.Controls.Add(group, 0, 0)
        main.Controls.Add(dgv, 0, 1)
        Me.Controls.Add(main)
    End Sub

    Private Sub RefreshRoles()
        rolesTable = DataAccess.LoadRoles()
        clbRoles.DataSource = rolesTable
        clbRoles.DisplayMember = "name"
        clbRoles.ValueMember = "id"
    End Sub

    Private Sub RefreshUsers()
        dgv.DataSource = DataAccess.LoadUsers()
        If dgv.Columns.Contains("id") Then dgv.Columns("id").Visible = False
        If dgv.Columns.Contains("username") Then dgv.Columns("username").HeaderText = "Tài khoản"
        If dgv.Columns.Contains("full_name") Then dgv.Columns("full_name").HeaderText = "Họ tên"
        If dgv.Columns.Contains("is_active") Then dgv.Columns("is_active").HeaderText = "Hoạt động"
        If dgv.Columns.Contains("roles") Then dgv.Columns("roles").HeaderText = "Vai trò"
    End Sub

    Private Sub OnRowSelected(sender As Object, e As EventArgs)
        If dgv.CurrentRow Is Nothing OrElse dgv.CurrentRow.DataBoundItem Is Nothing Then Return
        Dim row = CType(dgv.CurrentRow.DataBoundItem, DataRowView)
        selectedUserId = CType(row("id"), Guid)
        txtUsername.Text = row("username").ToString()
        txtUsername.Enabled = False
        txtFullName.Text = row("full_name").ToString()
        chkActive.Checked = Convert.ToBoolean(row("is_active"))
        txtPassword.Text = ""

        Dim roleIds = DataAccess.LoadUserRoleIds(selectedUserId)
        For i = 0 To clbRoles.Items.Count - 1
            Dim drv = CType(clbRoles.Items(i), DataRowView)
            Dim roleId = Convert.ToInt32(drv("id"))
            clbRoles.SetItemChecked(i, roleIds.Contains(roleId))
        Next
    End Sub

    Private Function SelectedRoleIds() As List(Of Integer)
        Dim roles As New List(Of Integer)()
        For Each item In clbRoles.CheckedItems
            Dim drv = CType(item, DataRowView)
            roles.Add(Convert.ToInt32(drv("id")))
        Next
        Return roles
    End Function

    Private Sub OnAdd(sender As Object, e As EventArgs)
        If String.IsNullOrWhiteSpace(txtUsername.Text) Then
            MessageBox.Show("Nhập tài khoản")
            Return
        End If
        If String.IsNullOrWhiteSpace(txtPassword.Text) Then
            MessageBox.Show("Nhập mật khẩu")
            Return
        End If
        Try
            DataAccess.CreateUser(txtUsername.Text, txtFullName.Text, txtPassword.Text, chkActive.Checked, SelectedRoleIds())
            RefreshUsers()
            OnReset(Nothing, EventArgs.Empty)
        Catch ex As Exception
            MessageBox.Show($"Lỗi thêm người dùng: {ex.Message}")
        End Try
    End Sub

    Private Sub OnUpdate(sender As Object, e As EventArgs)
        If selectedUserId = Guid.Empty Then
            MessageBox.Show("Chọn người dùng cần cập nhật")
            Return
        End If
        Try
            DataAccess.UpdateUser(selectedUserId, txtFullName.Text, chkActive.Checked, SelectedRoleIds())
            If Not String.IsNullOrWhiteSpace(txtPassword.Text) Then
                DataAccess.UpdateUserPassword(selectedUserId, txtPassword.Text)
            End If
            RefreshUsers()
            MessageBox.Show("Đã cập nhật")
        Catch ex As Exception
            MessageBox.Show($"Lỗi cập nhật: {ex.Message}")
        End Try
    End Sub

    Private Sub OnReset(sender As Object, e As EventArgs)
        selectedUserId = Guid.Empty
        txtUsername.Text = ""
        txtUsername.Enabled = True
        txtFullName.Text = ""
        txtPassword.Text = ""
        chkActive.Checked = True
        For i = 0 To clbRoles.Items.Count - 1
            clbRoles.SetItemChecked(i, False)
        Next
        dgv.ClearSelection()
    End Sub
End Class
