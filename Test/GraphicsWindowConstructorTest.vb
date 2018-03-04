Option Strict On

Imports System.Drawing
Imports VBGraphics
Imports Xunit

Public Class GraphicsWindowConstructorTest
    <Fact>
    Public Sub NewWindow()
        Dim size As New Size(100, 100)
        Dim gw As New GraphicsWindow(size.Width, size.Height, Color.Azure)
        gw.EndProgramOnClose = False

        Dim bitmap As New Bitmap(gw.Image)
        Assert.StrictEqual(size, bitmap.Size)

        For y As Integer = 0 To size.Height - 1
            For x As Integer = 0 To size.Width - 1
                Assert.StrictEqual(Color.FromArgb(Color.Azure.ToArgb()), bitmap.GetPixel(x, y))
            Next
        Next

        gw.Dispose()
    End Sub
End Class
