Imports System.Windows.Forms

Public Class UcSeatReport
    Inherits UserControl

    Private dgv As DataGridView

    Public Sub New()
        Me.Dock = DockStyle.Fill
        BuildUi()
        LoadData()
    End Sub

    Private Sub BuildUi()
        dgv = New DataGridView() With {
            .Dock = DockStyle.Fill,
            .ReadOnly = True,
            .AllowUserToAddRows = False,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        }
        Me.Controls.Add(dgv)
    End Sub

    Private Sub LoadData()
        Try
            dgv.DataSource = LoadSeatStats()
            If dgv.Columns.Contains("starts_at") Then
                dgv.Columns("starts_at").DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"
            End If
            If dgv.Columns.Contains("id") Then dgv.Columns("id").HeaderText = "Mã"
            If dgv.Columns.Contains("title") Then dgv.Columns("title").HeaderText = "Vở diễn"
            If dgv.Columns.Contains("starts_at") Then dgv.Columns("starts_at").HeaderText = "Thời gian"
            If dgv.Columns.Contains("standard_tickets") Then dgv.Columns("standard_tickets").HeaderText = "Ghế thường"
            If dgv.Columns.Contains("vip_tickets") Then dgv.Columns("vip_tickets").HeaderText = "Ghế VIP"
            If dgv.Columns.Contains("double_tickets") Then dgv.Columns("double_tickets").HeaderText = "Ghế đôi"
            If dgv.Columns.Contains("total_tickets") Then dgv.Columns("total_tickets").HeaderText = "Tổng vé"
            For Each col As DataGridViewColumn In dgv.Columns
                col.SortMode = DataGridViewColumnSortMode.Automatic
            Next
        Catch ex As Exception
            MessageBox.Show($"Lỗi tải báo cáo: {ex.Message}")
        End Try
    End Sub
End Class
