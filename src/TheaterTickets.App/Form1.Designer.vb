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
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.TableLayoutPanelNav = New System.Windows.Forms.TableLayoutPanel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.btnPerformances = New System.Windows.Forms.Button()
        Me.btnBooking = New System.Windows.Forms.Button()
        Me.btnSeatAssign = New System.Windows.Forms.Button()
        Me.btnReport = New System.Windows.Forms.Button()
        Me.PanelMain = New System.Windows.Forms.Panel()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.TableLayoutPanelNav.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer1.IsSplitterFixed = True
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.TableLayoutPanelNav)
        Me.SplitContainer1.Panel1.Padding = New System.Windows.Forms.Padding(10)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.PanelMain)
        Me.SplitContainer1.Panel2.Padding = New System.Windows.Forms.Padding(10)
        Me.SplitContainer1.Size = New System.Drawing.Size(1100, 720)
        Me.SplitContainer1.SplitterDistance = 230
        Me.SplitContainer1.TabIndex = 0
        '
        'TableLayoutPanelNav
        '
        Me.TableLayoutPanelNav.ColumnCount = 1
        Me.TableLayoutPanelNav.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelNav.Controls.Add(Me.lblTitle, 0, 0)
        Me.TableLayoutPanelNav.Controls.Add(Me.btnPerformances, 0, 1)
        Me.TableLayoutPanelNav.Controls.Add(Me.btnBooking, 0, 2)
        Me.TableLayoutPanelNav.Controls.Add(Me.btnSeatAssign, 0, 3)
        Me.TableLayoutPanelNav.Controls.Add(Me.btnReport, 0, 4)
        Me.TableLayoutPanelNav.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelNav.Location = New System.Drawing.Point(10, 10)
        Me.TableLayoutPanelNav.Name = "TableLayoutPanelNav"
        Me.TableLayoutPanelNav.Padding = New System.Windows.Forms.Padding(0, 0, 0, 10)
        Me.TableLayoutPanelNav.RowCount = 6
        Me.TableLayoutPanelNav.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60.0!))
        Me.TableLayoutPanelNav.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
        Me.TableLayoutPanelNav.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
        Me.TableLayoutPanelNav.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
        Me.TableLayoutPanelNav.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
        Me.TableLayoutPanelNav.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelNav.Size = New System.Drawing.Size(210, 700)
        '
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.lblTitle.Margin = New System.Windows.Forms.Padding(3, 3, 3, 3)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(199, 25)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Hệ thống bán vé Nhà hát"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnPerformances
        '
        Me.btnPerformances.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnPerformances.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.btnPerformances.Name = "btnPerformances"
        Me.btnPerformances.Size = New System.Drawing.Size(204, 44)
        Me.btnPerformances.TabIndex = 1
        Me.btnPerformances.Text = "Quản lý suất diễn"
        Me.btnPerformances.UseVisualStyleBackColor = True
        '
        'btnBooking
        '
        Me.btnBooking.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnBooking.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.btnBooking.Name = "btnBooking"
        Me.btnBooking.Size = New System.Drawing.Size(204, 44)
        Me.btnBooking.TabIndex = 2
        Me.btnBooking.Text = "Đặt vé"
        Me.btnBooking.UseVisualStyleBackColor = True
        '
        'btnSeatAssign
        '
        Me.btnSeatAssign.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnSeatAssign.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.btnSeatAssign.Name = "btnSeatAssign"
        Me.btnSeatAssign.Size = New System.Drawing.Size(204, 44)
        Me.btnSeatAssign.TabIndex = 3
        Me.btnSeatAssign.Text = "Gán ghế"
        Me.btnSeatAssign.UseVisualStyleBackColor = True
        '
        'btnReport
        '
        Me.btnReport.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnReport.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.btnReport.Name = "btnReport"
        Me.btnReport.Size = New System.Drawing.Size(204, 44)
        Me.btnReport.TabIndex = 4
        Me.btnReport.Text = "Báo cáo ghế"
        Me.btnReport.UseVisualStyleBackColor = True

        'PanelMain
        '
        Me.PanelMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelMain.Location = New System.Drawing.Point(10, 10)
        Me.PanelMain.Name = "PanelMain"
        Me.PanelMain.Size = New System.Drawing.Size(846, 700)
        Me.PanelMain.TabIndex = 5

        'PanelMain
        '
        Me.PanelMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelMain.Location = New System.Drawing.Point(23, 373)
        Me.PanelMain.Name = "PanelMain"
        Me.PanelMain.Size = New System.Drawing.Size(954, 304)
        Me.PanelMain.TabIndex = 5
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1100, 720)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Hệ thống bán vé Nhà hát"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.TableLayoutPanelNav.ResumeLayout(False)
        Me.TableLayoutPanelNav.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents TableLayoutPanelNav As TableLayoutPanel
    Friend WithEvents PanelMain As Panel
    Friend WithEvents lblTitle As Label
    Friend WithEvents btnPerformances As Button
    Friend WithEvents btnBooking As Button
    Friend WithEvents btnSeatAssign As Button
    Friend WithEvents btnReport As Button
End Class
