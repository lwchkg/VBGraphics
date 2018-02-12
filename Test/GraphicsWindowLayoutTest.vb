Option Strict On

Imports System.Drawing
Imports System.IO
Imports System.Reflection
Imports System.Windows.Forms
Imports VBGraphics
Imports VBGraphics.Shapes
Imports VBGraphics.Text
Imports VBGraphics.BitmapImage
Imports Xunit
Imports Xunit.Abstractions
Imports Xunit.Sdk

Public Class GraphicsWindowLayoutTest
    Implements IDisposable

    ReadOnly gw As GraphicsWindow
    ReadOnly testName As String
    ReadOnly helper As ITestOutputHelper

    ' Tolerance of layout test, which is the absolute difference of R + G + B + A.
    Dim allowedTotalDiff As Long = 0L
    Dim allowedNumPixelsWithDiff As Integer = 0

    Public Sub New(testOutputHelper As ITestOutputHelper)
        gw = New GraphicsWindow(400, 400)
        Me.helper = testOutputHelper
        Dim helper = CType(testOutputHelper, TestOutputHelper)
        testName = CType(helper.GetType().
                             GetField("test", BindingFlags.NonPublic Or BindingFlags.Instance).
                             GetValue(helper), ITest).DisplayName
        gw.Form.Text = testName
    End Sub

    Private Sub CreateDirectoryIfNecessary(path As String)
        If Not Directory.Exists(path) Then
            Directory.CreateDirectory(path)
        End If
    End Sub

    Private Function ColorDifference(a As Color, b As Color) As Integer
        Return Math.Abs(CInt(a.R) - CInt(b.R)) +
               Math.Abs(CInt(a.G) - CInt(b.G)) +
               Math.Abs(CInt(a.B) - CInt(b.B)) +
               Math.Abs(CInt(a.A) - CInt(b.A))
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
    End Sub

    Protected Overridable Sub Dispose(disposing As Boolean)
        Dim pathOfTest = testName.Replace(".", Path.DirectorySeparatorChar)
        Dim pathPrefix As String = Path.GetFullPath(
            Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "LayoutTestOutput",
                         pathOfTest))
        Dim pathExpected As String = pathPrefix + "-expected.png"
        Dim pathActual As String = pathPrefix + "-actual.png"

        Dim bitmapActual As Bitmap = New Bitmap(gw.Image)
        gw.Dispose()
        GC.SuppressFinalize(Me)

        If Not File.Exists(pathExpected) Then
            Dim ciVar As String = Environment.GetEnvironmentVariable("CI")
            Dim isCI As Boolean = ciVar IsNot Nothing AndAlso ciVar.ToUpper() = "TRUE"
            If isCI Then
                Const cannotFindTestExpectionInCI As String =
                    "Cannot open file {0} while CI=True is set in the environment. Remove the environment variable if you want to write a new test expectation instead."
                Assert.True(False, String.Format(cannotFindTestExpectionInCI, pathExpected))
            Else
                CreateDirectoryIfNecessary(Path.GetDirectoryName(pathExpected))
                bitmapActual.Save(pathExpected)
            End If
        Else
            Dim bitmapExpected As Bitmap = New Bitmap(Image.FromFile(pathExpected))

            If bitmapExpected.Size <> bitmapActual.Size Then
                bitmapActual.Save(pathActual)
                Assert.StrictEqual(bitmapExpected.Size, bitmapActual.Size)
            End If

            Dim totalDiff As Long = 0L
            Dim numPixelsWithDiff As Integer = 0
            For y As Integer = 0 To bitmapExpected.Height - 1
                For x As Integer = 0 To bitmapExpected.Width - 1
                    Dim expectedColor As Color = bitmapExpected.GetPixel(x, y)
                    Dim actualColor As Color = bitmapActual.GetPixel(x, y)
                    Dim diff As Integer = ColorDifference(expectedColor, actualColor)
                    If diff > 0 Then
                        totalDiff += diff
                        numPixelsWithDiff += 1
                    End If
                Next
            Next

            ' Output actual image if there is a difference, even if the test passes.
            If totalDiff > 0 Then
                bitmapActual.Save(pathActual)
            End If

            If totalDiff > allowedTotalDiff Or numPixelsWithDiff > allowedNumPixelsWithDiff Then
                Const imageDifferent As String = "Images are different. Total difference: {0} (Allowance: {1}). Number of pixels with difference: {2} (Allowance: {3})."
                Assert.True(False, String.Format(imageDifferent, totalDiff, allowedTotalDiff,
                                                 numPixelsWithDiff, allowedNumPixelsWithDiff))
            End If
        End If
    End Sub

    <Fact>
    Public Sub Rectangle()
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

        gw.DrawRectangle(125, 5, 50, 50, Nothing, Color.Red)
        gw.DrawRectangle(125, 65, 50, 50, Color.Blue, Nothing)
        gw.DrawRectangle(125, 125, 50, 50, Color.Blue, Color.Red)

        gw.DrawRectangleWithSmoothing(125, 185, 50, 50, Nothing, Color.Red)
        gw.DrawRectangleWithSmoothing(125, 245, 50, 50, Color.Blue, Nothing)
        gw.DrawRectangleWithSmoothing(125, 305, 50, 50, Color.Blue, Color.Red)

        gw.DrawRectangle(185, 5, 50, 50, Nothing, Nothing)
        gw.DrawRectangleWithSmoothing(185, 65, 50, 50, Nothing, Nothing)
    End Sub

    <Fact>
    Public Sub Ellipse()
        gw.DrawEllipse(5, 5, 50, 50, Nothing, Color.Red, 3)
        gw.DrawEllipse(5, 65, 50, 50, Nothing, Color.Red, 1)
        gw.DrawEllipse(5, 125, 50, 50, Nothing, Color.Red, 1.5F)

        gw.DrawEllipseWithSmoothing(5, 185, 50, 50, Nothing, Color.Red, 2.5F)
        gw.DrawEllipseWithSmoothing(5, 245, 50, 50, Nothing, Color.Red, 2.5F)
        gw.DrawEllipseWithSmoothing(5, 305, 50, 50, Nothing, Color.Red, 3.0F)

        gw.DrawEllipse(65, 5, 50, 50, Color.Blue, Color.Red, 3)
        gw.DrawEllipse(65, 65, 50, 50, Color.Blue, Color.Red, 1)
        gw.DrawEllipse(65, 125, 50, 50, Color.Blue, Color.Red, 1.5F)

        gw.DrawEllipseWithSmoothing(65, 185, 50, 50, Color.Blue, Color.Red, 2.5F)
        gw.DrawEllipseWithSmoothing(65, 245, 50, 50, Color.Blue, Color.Red, 2.5F)
        gw.DrawEllipseWithSmoothing(65, 305, 50, 50, Color.Blue, Color.Red, 3.0F)

        gw.DrawEllipse(125, 5, 50, 50, Nothing, Color.Red)
        gw.DrawEllipse(125, 65, 50, 50, Color.Blue, Nothing)
        gw.DrawEllipse(125, 125, 50, 50, Color.Blue, Color.Red)

        gw.DrawEllipseWithSmoothing(125, 185, 50, 50, Nothing, Color.Red)
        gw.DrawEllipseWithSmoothing(125, 245, 50, 50, Color.Blue, Nothing)
        gw.DrawEllipseWithSmoothing(125, 305, 50, 50, Color.Blue, Color.Red)

        gw.DrawEllipse(185, 5, 50, 50, Nothing, Nothing)
        gw.DrawEllipseWithSmoothing(185, 65, 50, 50, Nothing, Nothing)
    End Sub

    <Fact>
    Public Sub Line()
        gw.DrawLine(0, 0, 99, 399, Color.Green)
        gw.DrawLine(50, 0, 149, 399, Color.Green, 1.5F)
        gw.DrawLineWithSmoothing(100, 0, 199, 399, Color.Green)
        gw.DrawLineWithSmoothing(150, 0, 249, 399, Color.Green, 1.5F)
        gw.DrawLineWithSmoothing(200, 0, 299, 399, Color.Green, 2.0F)
        gw.DrawLineWithSmoothing(250, 0, 349, 399, Color.Green, 2.5F)
        gw.DrawLineWithSmoothing(300, 0, 399, 399, Color.Green, 3.0F)
    End Sub

    <Fact>
    Public Sub TextAlign()
        Dim s As String = "Lorem ipsum sit dolor amat, Lorem ipsum sit dolor amat"
        Dim x As Integer = 10
        Dim y As Integer = 10
        For Each vAlign In {TextFormatFlags.Top, TextFormatFlags.VerticalCenter,
                            TextFormatFlags.Bottom}
            For Each hAlign In {TextFormatFlags.Left, TextFormatFlags.HorizontalCenter,
                                TextFormatFlags.Right}
                gw.DrawRectangle(x, y, 100, 100, Color.Blue, Color.White)
                gw.DrawTextInRectangle(s, x, y, 100, 100, Color.White,
                                       "Arial", 8, FontStyle.Regular,
                                       hAlign Or vAlign Or TextFormatFlags.WordBreak)
                x += 110
            Next
            y += 110
            x = 10
        Next

        gw.DrawText(s, x, y, Color.White, "Arial", 8)
    End Sub

    <Fact>
    Public Sub TextAlign2()
        ' Set tolerance. This is necessary because text are rendered differently in different
        ' platforms. View actual images before accepting a PR.
        allowedTotalDiff = 175000L
        allowedNumPixelsWithDiff = 3500

        Dim s As String = "Lorem ipsum sit dolor amat, Lorem ipsum sit dolor amat"
        Dim x As Integer = 10
        Dim y As Integer = 10
        Dim font As New Font("Arial", 8, FontStyle.Italic)

        For Each vAlign In {TextFormatFlags.Top, TextFormatFlags.VerticalCenter,
                            TextFormatFlags.Bottom}
            For Each hAlign In {TextFormatFlags.Left, TextFormatFlags.HorizontalCenter,
                                TextFormatFlags.Right}
                gw.DrawRectangle(x, y, 100, 100, Color.Green, Color.Yellow)
                gw.DrawTextInRectangle(s, x, y, 100, 100, Color.Yellow, font,
                                       hAlign Or vAlign Or TextFormatFlags.WordBreak)
                x += 110
            Next
            y += 110
            x = 10
        Next

        gw.DrawText(s, x, y, Color.White, font)
    End Sub

    <Fact>
    Public Sub ReadLine()
        ' Set tolerance. This is necessary because text are rendered differently in different
        ' platforms. View actual images before accepting a PR.
        allowedTotalDiff = 20000L
        allowedNumPixelsWithDiff = 300

        Const keys As String = "123{BKSP}ABC{ENTER}"

        gw.Form.BringToFront()
        Application.DoEvents()

        SendKeys.SendWait(keys)
        gw.ReadLine(5, 5, 200, Color.Red, Color.Blue, "Arial", 16)

        SendKeys.SendWait(keys)
        gw.ReadLine(5, 55, 200, Color.Red, Color.Blue, "Arial", 16, FontStyle.Italic,
                    Color.Yellow, Color.DarkGreen, TextFormatFlags.HorizontalCenter)
    End Sub

    <Fact>
    Public Sub ImageTest()
        gw.DrawRectangle(0, 60, 400, 60, Color.Blue, Nothing)
        gw.DrawRectangle(0, 120, 400, 60, Color.Green, Nothing)
        gw.DrawRectangle(0, 180, 400, 60, Color.Red, Nothing)

        Dim bitmap As New Bitmap(16, 16, Imaging.PixelFormat.Format32bppArgb)
        For y As Integer = 0 To 15
            For x As Integer = 0 To 15
                Dim r As Integer = If((y And 1) > 0, 192, 0) + If((y And 2) > 0, 63, 0)
                Dim g As Integer = If((x And 2) > 0, 192, 0) + If((y And 2) > 0, 63, 0)
                Dim b As Integer = If((x And 1) > 0, 192, 0) + If((y And 2) > 0, 63, 0)
                Dim a As Integer = 17 * (x Mod 4 + 4 * (y \ 4))
                bitmap.SetPixel(x, y, Color.FromArgb(a, r, g, b))
            Next
        Next

        For y As Integer = 5 To 195 Step 60
            gw.DrawImage(bitmap, 5, y)
            gw.DrawImageResized(bitmap, 65, y, 48, 48)
            gw.DrawImageResizedWithSmoothing(bitmap, 125, y, 48, 48)
            gw.DrawImageWithClipping(bitmap, 185, y, 2, 2, 10, 10)
        Next y

        Dim bitmapExpected As New Bitmap(16, 16, Imaging.PixelFormat.Format24bppRgb)
        Graphics.FromImage(bitmapExpected).DrawImage(bitmap, 0, 0)

        ' Row 1 column 1: must be same as original image.
        Dim bitmapActual As Bitmap = gw.GetBitmap(5, 5, 16, 16)
        For y As Integer = 0 To 15
            For x As Integer = 0 To 15
                Assert.StrictEqual(bitmapExpected.GetPixel(x, y), bitmapActual.GetPixel(x, y))
            Next
        Next

        ' Row 1 column 3: every pixel must be enlarged exactly 3 times.
        bitmapActual = gw.GetBitmap(65, 5, 48, 48)
        For y As Integer = 0 To 47
            For x As Integer = 0 To 47
                Assert.StrictEqual(bitmapExpected.GetPixel(x \ 3, y \ 3),
                                   bitmapActual.GetPixel(x, y))
            Next
        Next

        ' Row 1 column 4: cropped original image.
        bitmapActual = gw.GetBitmap(185, 5, 10, 10)
        For y As Integer = 0 To 9
            For x As Integer = 0 To 9
                Assert.StrictEqual(bitmapExpected.GetPixel(x + 2, y + 2),
                                   bitmapActual.GetPixel(x, y))
            Next
        Next
    End Sub
End Class
