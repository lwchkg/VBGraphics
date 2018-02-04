Imports System.Drawing
Imports System.Windows.Forms
Imports VBGraphics
Imports VBGraphics.Shapes
Imports VBGraphics.Text
Imports VBGraphics.BitmapImage

Module Module1

    Sub Screen1(gw As GraphicsWindow)
        gw.Clear(Color.Black)

        gw.DrawRectangle(5, 5, 50, 50, Nothing, Color.Red, 3)
        gw.DrawRectangle(5, 65, 50, 50, Nothing, Color.Red, 1)
        gw.DrawRectangle(5, 125, 50, 50, Nothing, Color.Red, 1.5F)

        gw.DrawRectangleWithSmoothing(5, 185, 50, 50, Nothing, Color.Red, 2.5F)
        gw.DrawRectangleWithSmoothing(5, 245, 50, 50, Nothing, Color.Red, 2.5F)
        gw.DrawRectangleWithSmoothing(5, 305, 50, 50, Nothing, Color.Red, 3.0F)

        gw.DrawRectangle(65, 5, 50, 50, Color.Blue, Color.Red, 3)
        gw.DrawRectangle(65, 65, 50, 50, Color.Blue, Color.Red, 1)
        gw.DrawRectangle(65, 125, 50, 50, Color.Blue, Color.Red, 1.5F)

        gw.DrawRectangleWithSmoothing(65, 185, 50, 50, Color.Blue, Color.Red, 2.5F)
        gw.DrawRectangleWithSmoothing(65, 245, 50, 50, Color.Blue, Color.Red, 2.5F)
        gw.DrawRectangleWithSmoothing(65, 305, 50, 50, Color.Blue, Color.Red, 3.0F)

        gw.DrawEllipse(125, 5, 50, 50, Nothing, Color.Red, 3)
        gw.DrawEllipse(125, 65, 50, 50, Nothing, Color.Red, 1)
        gw.DrawEllipse(125, 125, 50, 50, Nothing, Color.Red, 1.5F)

        gw.DrawEllipseWithSmoothing(125, 185, 50, 50, Nothing, Color.Red, 2.5F)
        gw.DrawEllipseWithSmoothing(125, 245, 50, 50, Nothing, Color.Red, 2.5F)
        gw.DrawEllipseWithSmoothing(125, 305, 50, 50, Nothing, Color.Red, 3.0F)

        gw.DrawEllipse(185, 5, 50, 50, Color.Blue, Color.Red, 3)
        gw.DrawEllipse(185, 65, 50, 50, Color.Blue, Color.Red, 1)
        gw.DrawEllipse(185, 125, 50, 50, Color.Blue, Color.Red, 1.5F)

        gw.DrawEllipseWithSmoothing(185, 185, 50, 50, Color.Blue, Color.Red, 2.5F)
        gw.DrawEllipseWithSmoothing(185, 245, 50, 50, Color.Blue, Color.Red, 2.5F)
        gw.DrawEllipseWithSmoothing(185, 305, 50, 50, Color.Blue, Color.Red, 3.0F)

        gw.DrawImage(My.Resources.DotNetEditor_16, 300, 0)
        gw.DrawImageResized(My.Resources.DotNetEditor_16, 300, 60, 48, 48)
        gw.DrawImageResizedWithSmoothing(My.Resources.DotNetEditor_16, 300, 120, 48, 48)
    End Sub

    Sub Screen2(gw As GraphicsWindow)
        gw.Clear(Color.Black)

        gw.DrawLine(0, 0, 99, 399, Color.Green)
        gw.DrawLine(50, 0, 149, 399, Color.Green, 1.5F)
        gw.DrawLineWithSmoothing(100, 0, 199, 399, Color.Green)
        gw.DrawLineWithSmoothing(150, 0, 249, 399, Color.Green, 1.5F)
        gw.DrawLineWithSmoothing(200, 0, 299, 399, Color.Green, 2.0F)
        gw.DrawLineWithSmoothing(250, 0, 349, 399, Color.Green, 2.5F)
        gw.DrawLineWithSmoothing(300, 0, 399, 399, Color.Green, 3.0F)
    End Sub

    Sub Screen3(gw As GraphicsWindow)
        gw.Clear(Color.Black)

        Dim s As String = "Lorem ipsum sit dolor amat, Lorem ipsum sit dolor amat"
        Dim x As Integer = 10
        Dim y As Integer = 10
        For Each vAlign In {TextFormatFlags.Top, TextFormatFlags.VerticalCenter, TextFormatFlags.Bottom}
            For Each hAlign In {TextFormatFlags.Left, TextFormatFlags.HorizontalCenter, TextFormatFlags.Right}
                gw.DrawRectangle(x, y, 100, 100, Color.Blue, Color.White)
                gw.DrawTextInRectangle(s, x, y, 100, 100, Color.White,
                                       "Arial", 8, FontStyle.Regular, hAlign Or vAlign Or TextFormatFlags.WordBreak)
                x += 110
            Next
            y += 110
            x = 10
        Next

        gw.DrawText(s, x, y, Color.White, "Arial", 8)
    End Sub

    Sub Screen4(gw As GraphicsWindow)
        gw.Clear(Color.Black)

        Dim s As String = "Lorem ipsum sit dolor amat, Lorem ipsum sit dolor amat"
        Dim x As Integer = 10
        Dim y As Integer = 10
        Dim font As New Font("Arial", 8, FontStyle.Italic)

        For Each vAlign In {TextFormatFlags.Top, TextFormatFlags.VerticalCenter, TextFormatFlags.Bottom}
            For Each hAlign In {TextFormatFlags.Left, TextFormatFlags.HorizontalCenter, TextFormatFlags.Right}
                gw.DrawRectangle(x, y, 100, 100, Color.Green, Color.Yellow)
                gw.DrawTextInRectangle(s, x, y, 100, 100, Color.Yellow, font, hAlign Or vAlign Or TextFormatFlags.WordBreak)
                x += 110
            Next
            y += 110
            x = 10
        Next

        gw.DrawText(s, x, y, Color.White, font)
    End Sub

    Sub Main()
        Dim gw = New GraphicsWindow(400, 400)
        Dim g = Graphics.FromImage(gw.Image)

        Screen1(gw)
        gw.Invalidate()
        gw.WaitUntilKeyAvailable()
        gw.EmptyKeys()

        Screen2(gw)
        gw.WaitUntilKeyAvailable()
        gw.EmptyKeys()

        Screen3(gw)
        SendKeys.SendWait("123{BKSP}234{ENTER}345")
        gw.ReadLine(100, 340, 100, Color.White, Color.DimGray, "Arial", 8, FontStyle.Regular, Nothing, Color.DarkRed)

        gw.ReadLine(100, 360, 100, Color.White, Color.DimGray, "Arial", 8, FontStyle.Regular, Nothing, Color.DarkRed, TextFormatFlags.Right)
        gw.WaitUntilKeyAvailable()
        gw.EmptyKeys()

        Screen4(gw)
        gw.WaitUntilKeyAvailable()
        'gw.EmptyKeys()

        While gw.IsLiving
            Dim k = gw.ReadKey()
            If IsNothing(k) Then Exit While
            If k.KeyCode = System.Windows.Forms.Keys.Escape Then Exit While
            gw.Clear(Color.Black)
            Dim s As String = String.Format("Key: {0}, Keyvalue: {1}", k.KeyData, k.KeyValue)
            gw.DrawText(s, 10, 10, Color.White, "Consolas", 12)
            gw.DrawLine(0, 0, 99, 399, Color.White)
            gw.DrawLine(50, 0, 149, 399, Color.White, 1.5F)
            gw.DrawLineWithSmoothing(100, 0, 199, 399, Color.White)
            gw.DrawLineWithSmoothing(150, 0, 249, 399, Color.White, 1.5F)
            gw.DrawLineWithSmoothing(200, 0, 299, 399, Color.White, 2.0F)
            gw.DrawLineWithSmoothing(250, 0, 349, 399, Color.White, 2.5F)
            gw.DrawLineWithSmoothing(300, 0, 399, 399, Color.White, 3.0F)
        End While
    End Sub

End Module
