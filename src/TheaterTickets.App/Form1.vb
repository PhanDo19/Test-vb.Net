Public Class Form1
    Private currentControl As UserControl
    Private activeButton As Button

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ShowControl(New UcPerformanceMaster(), btnPerformances)
    End Sub

    Private Sub btnPerformances_Click(sender As Object, e As EventArgs) Handles btnPerformances.Click
        ShowControl(New UcPerformanceMaster(), btnPerformances)
    End Sub

    Private Sub btnBooking_Click(sender As Object, e As EventArgs) Handles btnBooking.Click
        ShowControl(New UcBooking(), btnBooking)
    End Sub

    Private Sub btnSeatAssign_Click(sender As Object, e As EventArgs) Handles btnSeatAssign.Click
        ShowControl(New UcSeatAssignment(), btnSeatAssign)
    End Sub

    Private Sub btnReport_Click(sender As Object, e As EventArgs) Handles btnReport.Click
        ShowControl(New UcSeatReport(), btnReport)
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
End Class
