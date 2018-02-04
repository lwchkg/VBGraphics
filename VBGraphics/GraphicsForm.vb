Option Strict On

Imports System.Windows.Forms
Public Class GraphicsForm
    Inherits Form

    Public Sub New()
        InitializeComponent()
        ' Disable background painting
        Me.SetStyle(ControlStyles.Opaque, True)
    End Sub

    Private Sub InitializeComponent()
        Me.SuspendLayout()
        '
        'GraphicsForm
        '
        Me.ClientSize = New System.Drawing.Size(282, 253)
        Me.Name = "GraphicsForm"

        Me.ResumeLayout(False)
    End Sub
End Class
