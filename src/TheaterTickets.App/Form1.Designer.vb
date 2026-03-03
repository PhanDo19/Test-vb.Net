<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        SplitContainer1 = New SplitContainer()
        TableLayoutPanelNav = New TableLayoutPanel()
        lblTitle = New Label()
        FlowLayoutPanelGreeting = New FlowLayoutPanel()
        lblHello = New Label()
        lblUserName = New Label()
        btnPerformances = New Button()
        btnBooking = New Button()
        btnSeatAssign = New Button()
        btnReport = New Button()
        btnUsers = New Button()
        btnLogout = New Button()
        PanelMain = New Panel()
        PanelLogin = New Panel()
        TableLayoutPanelLogin = New TableLayoutPanel()
        LabelLoginTitle = New Label()
        LabelUser = New Label()
        txtUserLogin = New TextBox()
        LabelPass = New Label()
        txtPassLogin = New TextBox()
        btnLoginMain = New Button()
        lblLoginError = New Label()
        CType(SplitContainer1, ComponentModel.ISupportInitialize).BeginInit()
        SplitContainer1.Panel1.SuspendLayout()
        SplitContainer1.Panel2.SuspendLayout()
        SplitContainer1.SuspendLayout()
        TableLayoutPanelNav.SuspendLayout()
        FlowLayoutPanelGreeting.SuspendLayout()
        PanelMain.SuspendLayout()
        PanelLogin.SuspendLayout()
        TableLayoutPanelLogin.SuspendLayout()
        SuspendLayout()
        ' 
        ' SplitContainer1
        ' 
        SplitContainer1.Dock = DockStyle.Fill
        SplitContainer1.FixedPanel = FixedPanel.Panel1
        SplitContainer1.IsSplitterFixed = True
        SplitContainer1.Location = New Point(0, 0)
        SplitContainer1.Name = "SplitContainer1"
        ' 
        ' SplitContainer1.Panel1
        ' 
        SplitContainer1.Panel1.Controls.Add(TableLayoutPanelNav)
        SplitContainer1.Panel1.Padding = New Padding(10)
        ' 
        ' SplitContainer1.Panel2
        ' 
        SplitContainer1.Panel2.Controls.Add(PanelMain)
        SplitContainer1.Panel2.Padding = New Padding(10)
        SplitContainer1.Size = New Size(1128, 720)
        SplitContainer1.SplitterDistance = 270
        SplitContainer1.TabIndex = 0
        ' 
        ' TableLayoutPanelNav
        ' 
        TableLayoutPanelNav.ColumnCount = 1
        TableLayoutPanelNav.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        TableLayoutPanelNav.Controls.Add(lblTitle, 0, 0)
        TableLayoutPanelNav.Controls.Add(FlowLayoutPanelGreeting, 0, 1)
        TableLayoutPanelNav.Controls.Add(btnPerformances, 0, 2)
        TableLayoutPanelNav.Controls.Add(btnBooking, 0, 3)
        TableLayoutPanelNav.Controls.Add(btnSeatAssign, 0, 4)
        TableLayoutPanelNav.Controls.Add(btnReport, 0, 5)
        TableLayoutPanelNav.Controls.Add(btnUsers, 0, 6)
        TableLayoutPanelNav.Controls.Add(btnLogout, 0, 7)
        TableLayoutPanelNav.Dock = DockStyle.Fill
        TableLayoutPanelNav.Location = New Point(10, 10)
        TableLayoutPanelNav.Name = "TableLayoutPanelNav"
        TableLayoutPanelNav.Padding = New Padding(0, 0, 0, 10)
        TableLayoutPanelNav.RowCount = 9
        TableLayoutPanelNav.RowStyles.Add(New RowStyle(SizeType.Absolute, 60F))
        TableLayoutPanelNav.RowStyles.Add(New RowStyle(SizeType.Absolute, 40F))
        TableLayoutPanelNav.RowStyles.Add(New RowStyle(SizeType.Absolute, 50F))
        TableLayoutPanelNav.RowStyles.Add(New RowStyle(SizeType.Absolute, 50F))
        TableLayoutPanelNav.RowStyles.Add(New RowStyle(SizeType.Absolute, 50F))
        TableLayoutPanelNav.RowStyles.Add(New RowStyle(SizeType.Absolute, 50F))
        TableLayoutPanelNav.RowStyles.Add(New RowStyle(SizeType.Absolute, 50F))
        TableLayoutPanelNav.RowStyles.Add(New RowStyle(SizeType.Absolute, 70F))
        TableLayoutPanelNav.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanelNav.Size = New Size(250, 700)
        TableLayoutPanelNav.TabIndex = 0
        ' 
        ' lblTitle
        ' 
        lblTitle.AutoSize = True
        lblTitle.Dock = DockStyle.Fill
        lblTitle.Font = New Font("Segoe UI", 14F, FontStyle.Bold)
        lblTitle.Location = New Point(3, 3)
        lblTitle.Margin = New Padding(3)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(244, 54)
        lblTitle.TabIndex = 0
        lblTitle.Text = "Hệ thống bán vé Nhà hát"
        lblTitle.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' FlowLayoutPanelGreeting
        ' 
        FlowLayoutPanelGreeting.AutoSize = True
        FlowLayoutPanelGreeting.AutoSizeMode = AutoSizeMode.GrowAndShrink
        FlowLayoutPanelGreeting.Controls.Add(lblHello)
        FlowLayoutPanelGreeting.Controls.Add(lblUserName)
        FlowLayoutPanelGreeting.Dock = DockStyle.Fill
        FlowLayoutPanelGreeting.Location = New Point(3, 63)
        FlowLayoutPanelGreeting.Name = "FlowLayoutPanelGreeting"
        FlowLayoutPanelGreeting.Size = New Size(244, 34)
        FlowLayoutPanelGreeting.TabIndex = 6
        ' 
        ' lblHello
        ' 
        lblHello.AutoSize = True
        lblHello.Font = New Font("Segoe UI", 10F)
        lblHello.Location = New Point(0, 6)
        lblHello.Margin = New Padding(0, 6, 0, 0)
        lblHello.Name = "lblHello"
        lblHello.Size = New Size(77, 19)
        lblHello.TabIndex = 0
        lblHello.Text = "XIN CHÀO,"
        ' 
        ' lblUserName
        ' 
        lblUserName.AutoSize = True
        lblUserName.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        lblUserName.ForeColor = Color.ForestGreen
        lblUserName.Location = New Point(77, 6)
        lblUserName.Margin = New Padding(0, 6, 0, 0)
        lblUserName.Name = "lblUserName"
        lblUserName.Size = New Size(37, 19)
        lblUserName.TabIndex = 1
        lblUserName.Text = "user"
        ' 
        ' btnPerformances
        ' 
        btnPerformances.Dock = DockStyle.Fill
        btnPerformances.Font = New Font("Segoe UI", 11F)
        btnPerformances.Location = New Point(3, 103)
        btnPerformances.Name = "btnPerformances"
        btnPerformances.Size = New Size(244, 44)
        btnPerformances.TabIndex = 1
        btnPerformances.Text = "Quản lý suất diễn"
        btnPerformances.UseVisualStyleBackColor = True
        ' 
        ' btnBooking
        ' 
        btnBooking.Dock = DockStyle.Fill
        btnBooking.Font = New Font("Segoe UI", 11F)
        btnBooking.Location = New Point(3, 153)
        btnBooking.Name = "btnBooking"
        btnBooking.Size = New Size(244, 44)
        btnBooking.TabIndex = 2
        btnBooking.Text = "Đặt vé"
        btnBooking.UseVisualStyleBackColor = True
        ' 
        ' btnSeatAssign
        ' 
        btnSeatAssign.Dock = DockStyle.Fill
        btnSeatAssign.Font = New Font("Segoe UI", 11F)
        btnSeatAssign.Location = New Point(3, 203)
        btnSeatAssign.Name = "btnSeatAssign"
        btnSeatAssign.Size = New Size(244, 44)
        btnSeatAssign.TabIndex = 3
        btnSeatAssign.Text = "Gán ghế"
        btnSeatAssign.UseVisualStyleBackColor = True
        ' 
        ' btnReport
        ' 
        btnReport.Dock = DockStyle.Fill
        btnReport.Font = New Font("Segoe UI", 11F)
        btnReport.Location = New Point(3, 253)
        btnReport.Name = "btnReport"
        btnReport.Size = New Size(244, 44)
        btnReport.TabIndex = 4
        btnReport.Text = "Báo cáo ghế"
        btnReport.UseVisualStyleBackColor = True
        ' 
        ' btnUsers
        ' 
        btnUsers.Dock = DockStyle.Fill
        btnUsers.Font = New Font("Segoe UI", 11F)
        btnUsers.Location = New Point(3, 303)
        btnUsers.Name = "btnUsers"
        btnUsers.Size = New Size(244, 44)
        btnUsers.TabIndex = 5
        btnUsers.Text = "Quản lý người dùng"
        btnUsers.UseVisualStyleBackColor = True
        ' 
        ' btnLogout
        ' 
        btnLogout.Dock = DockStyle.Fill
        btnLogout.Font = New Font("Segoe UI", 10F)
        btnLogout.Location = New Point(3, 353)
        btnLogout.Name = "btnLogout"
        btnLogout.Size = New Size(244, 64)
        btnLogout.TabIndex = 6
        btnLogout.Text = "Đăng xuất"
        btnLogout.UseVisualStyleBackColor = True
        ' 
        ' PanelMain
        ' 
        PanelMain.Controls.Add(PanelLogin)
        PanelMain.Dock = DockStyle.Fill
        PanelMain.Location = New Point(10, 10)
        PanelMain.Name = "PanelMain"
        PanelMain.Size = New Size(834, 700)
        PanelMain.TabIndex = 5
        ' 
        ' PanelLogin
        ' 
        PanelLogin.Controls.Add(TableLayoutPanelLogin)
        PanelLogin.Dock = DockStyle.Fill
        PanelLogin.Location = New Point(0, 0)
        PanelLogin.Name = "PanelLogin"
        PanelLogin.Size = New Size(834, 700)
        PanelLogin.TabIndex = 0
        ' 
        ' TableLayoutPanelLogin
        ' 
        TableLayoutPanelLogin.Anchor = AnchorStyles.None
        TableLayoutPanelLogin.ColumnCount = 2
        TableLayoutPanelLogin.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 120F))
        TableLayoutPanelLogin.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 280F))
        TableLayoutPanelLogin.Controls.Add(LabelLoginTitle, 0, 0)
        TableLayoutPanelLogin.Controls.Add(LabelUser, 0, 1)
        TableLayoutPanelLogin.Controls.Add(txtUserLogin, 1, 1)
        TableLayoutPanelLogin.Controls.Add(LabelPass, 0, 2)
        TableLayoutPanelLogin.Controls.Add(txtPassLogin, 1, 2)
        TableLayoutPanelLogin.Controls.Add(btnLoginMain, 1, 3)
        TableLayoutPanelLogin.Controls.Add(lblLoginError, 1, 4)
        TableLayoutPanelLogin.Location = New Point(540, 530)
        TableLayoutPanelLogin.Name = "TableLayoutPanelLogin"
        TableLayoutPanelLogin.RowCount = 5
        TableLayoutPanelLogin.RowStyles.Add(New RowStyle(SizeType.Absolute, 40F))
        TableLayoutPanelLogin.RowStyles.Add(New RowStyle(SizeType.Absolute, 40F))
        TableLayoutPanelLogin.RowStyles.Add(New RowStyle(SizeType.Absolute, 40F))
        TableLayoutPanelLogin.RowStyles.Add(New RowStyle(SizeType.Absolute, 40F))
        TableLayoutPanelLogin.RowStyles.Add(New RowStyle(SizeType.Absolute, 40F))
        TableLayoutPanelLogin.Size = New Size(400, 200)
        TableLayoutPanelLogin.TabIndex = 0
        ' 
        ' LabelLoginTitle
        ' 
        LabelLoginTitle.AutoSize = True
        TableLayoutPanelLogin.SetColumnSpan(LabelLoginTitle, 2)
        LabelLoginTitle.Font = New Font("Segoe UI", 12F, FontStyle.Bold)
        LabelLoginTitle.Location = New Point(3, 5)
        LabelLoginTitle.Margin = New Padding(3, 5, 3, 5)
        LabelLoginTitle.Name = "LabelLoginTitle"
        LabelLoginTitle.Size = New Size(94, 21)
        LabelLoginTitle.TabIndex = 0
        LabelLoginTitle.Text = "Đăng nhập"
        LabelLoginTitle.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' LabelUser
        ' 
        LabelUser.AutoSize = True
        LabelUser.Dock = DockStyle.Fill
        LabelUser.Location = New Point(3, 40)
        LabelUser.Name = "LabelUser"
        LabelUser.Size = New Size(114, 40)
        LabelUser.TabIndex = 1
        LabelUser.Text = "Tài khoản"
        LabelUser.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' txtUserLogin
        ' 
        txtUserLogin.Dock = DockStyle.Fill
        txtUserLogin.Location = New Point(123, 43)
        txtUserLogin.Name = "txtUserLogin"
        txtUserLogin.Size = New Size(274, 23)
        txtUserLogin.TabIndex = 2
        ' 
        ' LabelPass
        ' 
        LabelPass.AutoSize = True
        LabelPass.Dock = DockStyle.Fill
        LabelPass.Location = New Point(3, 80)
        LabelPass.Name = "LabelPass"
        LabelPass.Size = New Size(114, 40)
        LabelPass.TabIndex = 3
        LabelPass.Text = "Mật khẩu"
        LabelPass.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' txtPassLogin
        ' 
        txtPassLogin.Dock = DockStyle.Fill
        txtPassLogin.Location = New Point(123, 83)
        txtPassLogin.Name = "txtPassLogin"
        txtPassLogin.Size = New Size(274, 23)
        txtPassLogin.TabIndex = 4
        txtPassLogin.UseSystemPasswordChar = True
        ' 
        ' btnLoginMain
        ' 
        btnLoginMain.Anchor = AnchorStyles.Left
        btnLoginMain.Location = New Point(123, 125)
        btnLoginMain.Name = "btnLoginMain"
        btnLoginMain.Size = New Size(120, 30)
        btnLoginMain.TabIndex = 5
        btnLoginMain.Text = "Đăng nhập"
        btnLoginMain.UseVisualStyleBackColor = True
        ' 
        ' lblLoginError
        ' 
        lblLoginError.AutoSize = True
        lblLoginError.Dock = DockStyle.Fill
        lblLoginError.ForeColor = Color.Firebrick
        lblLoginError.Location = New Point(123, 160)
        lblLoginError.Name = "lblLoginError"
        lblLoginError.Size = New Size(274, 40)
        lblLoginError.TabIndex = 6
        lblLoginError.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1128, 720)
        Controls.Add(SplitContainer1)
        Name = "Form1"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Hệ thống bán vé Nhà hát"
        SplitContainer1.Panel1.ResumeLayout(False)
        SplitContainer1.Panel2.ResumeLayout(False)
        CType(SplitContainer1, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer1.ResumeLayout(False)
        TableLayoutPanelNav.ResumeLayout(False)
        TableLayoutPanelNav.PerformLayout()
        FlowLayoutPanelGreeting.ResumeLayout(False)
        FlowLayoutPanelGreeting.PerformLayout()
        PanelMain.ResumeLayout(False)
        PanelLogin.ResumeLayout(False)
        TableLayoutPanelLogin.ResumeLayout(False)
        TableLayoutPanelLogin.PerformLayout()
        ResumeLayout(False)

    End Sub

    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents TableLayoutPanelNav As TableLayoutPanel
    Friend WithEvents PanelMain As Panel
    Friend WithEvents lblTitle As Label
    Friend WithEvents btnPerformances As Button
    Friend WithEvents btnBooking As Button
    Friend WithEvents btnSeatAssign As Button
    Friend WithEvents btnReport As Button
    Friend WithEvents FlowLayoutPanelGreeting As FlowLayoutPanel
    Friend WithEvents lblHello As Label
    Friend WithEvents lblUserName As Label
    Friend WithEvents btnUsers As Button
    Friend WithEvents btnLogout As Button
    Friend WithEvents PanelLogin As Panel
    Friend WithEvents TableLayoutPanelLogin As TableLayoutPanel
    Friend WithEvents LabelLoginTitle As Label
    Friend WithEvents LabelUser As Label
    Friend WithEvents txtUserLogin As TextBox
    Friend WithEvents LabelPass As Label
    Friend WithEvents txtPassLogin As TextBox
    Friend WithEvents btnLoginMain As Button
    Friend WithEvents lblLoginError As Label
End Class
