Public Class Form1
    Private currentControl As UserControl
    Private activeButton As Button
    Private Const PERM_PERFORMANCE As String = "PERFORMANCE_MANAGE"
    Private Const PERM_BOOKING As String = "BOOKING_CREATE"
    Private Const PERM_SEAT As String = "SEAT_ASSIGN"
    Private Const PERM_REPORT As String = "REPORT_VIEW"
    Private Const PERM_USER_ADMIN As String = "USER_ADMIN"

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        UpdateAuthUi()
        CenterLoginPanel()
    End Sub

    Private Sub btnPerformances_Click(sender As Object, e As EventArgs) Handles btnPerformances.Click
        If Not RequirePermission(PERM_PERFORMANCE) Then Return
        ShowControl(New UcPerformanceMaster(), btnPerformances)
    End Sub

    Private Sub btnBooking_Click(sender As Object, e As EventArgs) Handles btnBooking.Click
        If Not RequirePermission(PERM_BOOKING) Then Return
        ShowControl(New UcBooking(), btnBooking)
    End Sub

    Private Sub btnSeatAssign_Click(sender As Object, e As EventArgs) Handles btnSeatAssign.Click
        If Not RequirePermission(PERM_SEAT) Then Return
        ShowControl(New UcSeatAssignment(), btnSeatAssign)
    End Sub

    Private Sub btnReport_Click(sender As Object, e As EventArgs) Handles btnReport.Click
        If Not RequirePermission(PERM_REPORT) Then Return
        ShowControl(New UcSeatReport(), btnReport)
    End Sub

    Private Sub btnUsers_Click(sender As Object, e As EventArgs) Handles btnUsers.Click
        If Not RequirePermission(PERM_USER_ADMIN) Then Return
        ShowControl(New UcUserManagement(), btnUsers)
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        AuthService.Logout()
        ClearCurrentControl()
        UpdateAuthUi()
    End Sub

    Private Sub btnLoginMain_Click(sender As Object, e As EventArgs) Handles btnLoginMain.Click
        If String.IsNullOrWhiteSpace(txtUserLogin.Text) OrElse String.IsNullOrWhiteSpace(txtPassLogin.Text) Then
            MessageBox.Show("Nhập tài khoản và mật khẩu.")
            Return
        End If
        Try
            If AuthService.Login(txtUserLogin.Text, txtPassLogin.Text) Then
                txtPassLogin.Text = ""
                lblLoginError.Text = ""
                UpdateAuthUi()
            Else
                lblLoginError.Text = "Sai tài khoản hoặc mật khẩu."
            End If
        Catch ex As Exception
            lblLoginError.Text = $"Lỗi đăng nhập: {ex.Message}"
        End Try
    End Sub

    Private Sub ShowControl(ctrl As UserControl, Optional navButton As Button = Nothing)
        If currentControl IsNot Nothing Then
            PanelMain.Controls.Remove(currentControl)
            currentControl.Dispose()
        End If
        currentControl = ctrl
        ctrl.Dock = DockStyle.Fill
        PanelMain.Controls.Add(ctrl)
        If navButton IsNot Nothing Then SetActiveButton(navButton)
    End Sub

    Private Sub SetActiveButton(btn As Button)
        If activeButton IsNot Nothing Then
            activeButton.BackColor = SystemColors.Control
            activeButton.ForeColor = SystemColors.ControlText
            activeButton.Font = New Font(activeButton.Font, FontStyle.Regular)
        End If
        activeButton = btn
        activeButton.BackColor = Color.LightSteelBlue
        activeButton.ForeColor = Color.Black
        activeButton.Font = New Font(activeButton.Font, FontStyle.Bold)
    End Sub

    Private Sub UpdateAuthUi()
        Dim loggedIn = AuthService.IsLoggedIn()
        SplitContainer1.Panel1Collapsed = Not loggedIn
        PanelLogin.Visible = Not loggedIn
        PanelLogin.BringToFront()
        ' Keep main panel enabled so login controls work
        If loggedIn Then
            lblUserName.Text = AuthService.CurrentUser.Username
            btnLogout.Enabled = True
        Else
            lblUserName.Text = "guest"
            lblLoginError.Text = ""
            txtPassLogin.Text = ""
            txtUserLogin.Focus()
            CenterLoginPanel()
            btnLogout.Enabled = False
        End If

        btnPerformances.Enabled = loggedIn AndAlso AuthService.HasPermission(PERM_PERFORMANCE)
        btnBooking.Enabled = loggedIn AndAlso AuthService.HasPermission(PERM_BOOKING)
        btnSeatAssign.Enabled = loggedIn AndAlso AuthService.HasPermission(PERM_SEAT)
        btnReport.Enabled = loggedIn AndAlso AuthService.HasPermission(PERM_REPORT)
        btnUsers.Enabled = loggedIn AndAlso AuthService.HasPermission(PERM_USER_ADMIN)

        If Not loggedIn Then
            ClearCurrentControl()
            Return
        End If

        If currentControl Is Nothing Then
            ShowFirstAvailableControl()
        End If
    End Sub

    Private Function RequirePermission(code As String) As Boolean
        If Not AuthService.HasPermission(code) Then
            MessageBox.Show("Bạn không có quyền truy cập chức năng này.")
            Return False
        End If
        Return True
    End Function

    Private Sub ShowFirstAvailableControl()
        If btnPerformances.Enabled Then
            ShowControl(New UcPerformanceMaster(), btnPerformances)
        ElseIf btnBooking.Enabled Then
            ShowControl(New UcBooking(), btnBooking)
        ElseIf btnSeatAssign.Enabled Then
            ShowControl(New UcSeatAssignment(), btnSeatAssign)
        ElseIf btnReport.Enabled Then
            ShowControl(New UcSeatReport(), btnReport)
        ElseIf btnUsers.Enabled Then
            ShowControl(New UcUserManagement(), btnUsers)
        Else
            MessageBox.Show("Tài khoản hiện không có quyền truy cập chức năng nào.")
        End If
    End Sub

    Private Sub ClearCurrentControl()
        If currentControl IsNot Nothing Then
            PanelMain.Controls.Remove(currentControl)
            currentControl.Dispose()
            currentControl = Nothing
        End If
        activeButton = Nothing
    End Sub

    Private Sub CenterLoginPanel()
        If PanelLogin Is Nothing OrElse TableLayoutPanelLogin Is Nothing Then Return
        Dim x = Math.Max(0, (PanelLogin.Width - TableLayoutPanelLogin.Width) \ 2)
        Dim y = Math.Max(0, (PanelLogin.Height - TableLayoutPanelLogin.Height) \ 2)
        TableLayoutPanelLogin.Location = New Point(x, y)
    End Sub

    Private Sub PanelLogin_Resize(sender As Object, e As EventArgs) Handles PanelLogin.Resize
        CenterLoginPanel()
    End Sub
End Class
