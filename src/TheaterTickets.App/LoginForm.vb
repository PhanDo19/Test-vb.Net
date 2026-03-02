Imports System.Windows.Forms

Public Class LoginForm
    Inherits Form

    Private txtUser As TextBox
    Private txtPass As TextBox
    Private lblError As Label
    Private btnLogin As Button
    Private btnCancel As Button

    Public Sub New()
        Me.Text = "Đăng nhập"
        Me.Width = 360
        Me.Height = 240
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False

        BuildUi()
    End Sub

    Private Sub BuildUi()
        Dim layout = New TableLayoutPanel() With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 2,
            .RowCount = 5,
            .Padding = New Padding(12)
        }
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 110))
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))
        For i = 0 To 4
            layout.RowStyles.Add(New RowStyle(SizeType.Absolute, If(i = 3, 30, 40)))
        Next

        layout.Controls.Add(New Label() With {.Text = "Tài khoản", .TextAlign = ContentAlignment.MiddleLeft, .Dock = DockStyle.Fill}, 0, 0)
        txtUser = New TextBox() With {.Dock = DockStyle.Fill}
        layout.Controls.Add(txtUser, 1, 0)

        layout.Controls.Add(New Label() With {.Text = "Mật khẩu", .TextAlign = ContentAlignment.MiddleLeft, .Dock = DockStyle.Fill}, 0, 1)
        txtPass = New TextBox() With {.Dock = DockStyle.Fill, .UseSystemPasswordChar = True}
        layout.Controls.Add(txtPass, 1, 1)

        lblError = New Label() With {.ForeColor = Color.Firebrick, .AutoSize = True, .Dock = DockStyle.Fill}
        layout.SetColumnSpan(lblError, 2)
        layout.Controls.Add(lblError, 0, 2)

        Dim buttons = New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .FlowDirection = FlowDirection.RightToLeft}
        btnLogin = New Button() With {.Text = "Đăng nhập", .Width = 100}
        btnCancel = New Button() With {.Text = "Thoát", .Width = 80}
        AddHandler btnLogin.Click, AddressOf OnLogin
        AddHandler btnCancel.Click, AddressOf OnCancel
        buttons.Controls.AddRange({btnLogin, btnCancel})
        layout.SetColumnSpan(buttons, 2)
        layout.Controls.Add(buttons, 0, 3)

        Me.Controls.Add(layout)
        Me.AcceptButton = btnLogin
        Me.CancelButton = btnCancel
    End Sub

    Private Sub OnLogin(sender As Object, e As EventArgs)
        lblError.Text = ""
        If String.IsNullOrWhiteSpace(txtUser.Text) OrElse String.IsNullOrWhiteSpace(txtPass.Text) Then
            lblError.Text = "Nhập tài khoản và mật khẩu."
            Return
        End If
        Try
            Dim ok = AuthService.Login(txtUser.Text, txtPass.Text)
            If ok Then
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Else
                lblError.Text = "Sai tài khoản hoặc mật khẩu."
            End If
        Catch ex As Exception
            lblError.Text = $"Lỗi: {ex.Message}"
        End Try
    End Sub

    Private Sub OnCancel(sender As Object, e As EventArgs)
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class
