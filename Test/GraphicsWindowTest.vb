Option Strict On

Imports System.Drawing
Imports System.Windows.Forms
Imports VBGraphics
Imports Xunit

Public Class GraphicsWindowTest
    Implements IDisposable

    ReadOnly gw As GraphicsWindow

    Public Sub New()
        gw = New GraphicsWindow(100, 100)
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
    End Sub

    Protected Overridable Sub Dispose(disposing As Boolean)
        gw.Dispose()
        GC.SuppressFinalize(Me)
    End Sub

    <Fact>
    Public Sub Clear()
        Dim bitmap As New Bitmap(gw.Image)
        Dim size As Size = bitmap.Size

        For y As Integer = 0 To size.Height - 1
            For x As Integer = 0 To size.Width - 1
                Assert.StrictEqual(Color.FromArgb(Color.Black.ToArgb()), bitmap.GetPixel(x, y))
            Next
        Next

        gw.Clear(Color.Azure)
        bitmap = New Bitmap(gw.Image)
        Assert.StrictEqual(size, bitmap.Size)

        For y As Integer = 0 To size.Height - 1
            For x As Integer = 0 To size.Width - 1
                Assert.StrictEqual(Color.FromArgb(Color.Azure.ToArgb()), bitmap.GetPixel(x, y))
            Next
        Next
    End Sub

    <Fact>
    Public Sub Key()
        gw.Form.BringToFront()
        Application.DoEvents()

        Assert.False(gw.KeyAvailable)
        Assert.Null(gw.ReadKeyIfAvailable)

        SendKeys.SendWait("12+3")

        Assert.True(gw.KeyAvailable)
        Assert.StrictEqual(Keys.D1, gw.ReadKey().KeyData)
        Assert.True(gw.KeyAvailable)
        Assert.StrictEqual(Keys.D2, gw.ReadKey().KeyData)
        Assert.True(gw.KeyAvailable)
        Assert.StrictEqual(Keys.D3 Or Keys.Shift, gw.ReadKeyIfAvailable().KeyData)

        Assert.False(gw.KeyAvailable)
        Assert.Null(gw.ReadKeyIfAvailable)
    End Sub

    <Fact>
    Public Sub KeyChar()
        gw.Form.BringToFront()
        Application.DoEvents()

        Assert.False(gw.KeyAvailable)
        Assert.StrictEqual(ControlChars.NullChar, gw.ReadKeyCharIfAvailable)

        SendKeys.SendWait("12+3")

        Assert.True(gw.KeyAvailable)
        Assert.StrictEqual("1"c, gw.ReadKeyChar())
        Assert.True(gw.KeyAvailable)
        Assert.StrictEqual("2"c, gw.ReadKeyChar())
        Assert.True(gw.KeyAvailable)
        Assert.StrictEqual("#"c, gw.ReadKeyCharIfAvailable())

        Assert.False(gw.KeyAvailable)
        Assert.StrictEqual(ControlChars.NullChar, gw.ReadKeyCharIfAvailable)
    End Sub

    <Fact>
    Public Sub KeyInfo()
        gw.Form.BringToFront()
        Application.DoEvents()

        Assert.False(gw.KeyAvailable)
        Assert.StrictEqual(New KeyInfo(Nothing, ControlChars.NullChar), gw.ReadKeyInfoIfAvailable)

        SendKeys.SendWait("12+3")

        Assert.True(gw.KeyAvailable)
        Dim key1 As KeyInfo = gw.ReadKeyInfo()
        Assert.StrictEqual(Keys.D1, key1.Key.KeyData)
        Assert.StrictEqual("1"c, key1.KeyChar)

        Assert.True(gw.KeyAvailable)
        Dim key2 As KeyInfo = gw.ReadKeyInfo()
        Assert.StrictEqual(Keys.D2, key2.Key.KeyData)
        Assert.StrictEqual("2"c, key2.KeyChar)

        Assert.True(gw.KeyAvailable)
        Dim key3 As KeyInfo = gw.ReadKeyInfo()
        Assert.StrictEqual(Keys.D3 Or Keys.Shift, key3.Key.KeyData)
        Assert.StrictEqual("#"c, key3.KeyChar)

        Assert.False(gw.KeyAvailable)
        Assert.StrictEqual(New KeyInfo(Nothing, ControlChars.NullChar), gw.ReadKeyInfoIfAvailable)
    End Sub

    <Fact>
    Public Sub SpecialKeys()
        gw.Form.BringToFront()
        Application.DoEvents()

        Assert.False(gw.KeyAvailable)
        Assert.StrictEqual(New KeyInfo(Nothing, ControlChars.NullChar), gw.ReadKeyInfoIfAvailable)

        ' Only tests some of the keys because the rest cannot be emulated with SendKeys. Keys that
        ' are not tested are Pause, Numpad 5 (without number lock), Ctrl, Alt, Shift, Menu, F10,
        ' and F17 to F24. Note that F10 is not tested because it activates the menu bar and
        ' and therefore makes tests to fail.
        Dim keysToTest As Keys() = {
            Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8, Keys.F9,
            Keys.F11, Keys.F12, Keys.F13, Keys.F14, Keys.F15, Keys.F16, Keys.Up, Keys.Down,
            Keys.Left, Keys.Right, Keys.PageUp, Keys.PageDown, Keys.Home, Keys.End, Keys.Insert,
            Keys.Delete, Keys.CapsLock, Keys.NumLock, Keys.Scroll
        }
        SendKeys.SendWait("{F1}{F2}{F3}{F4}{F5}{F6}{F7}{F8}{F9}{F11}{F12}{F13}{F14}{F15}{F16}{UP}" +
                          "{DOWN}{LEFT}{RIGHT}{PGUP}{PGDN}{HOME}{END}{INS}{DEL}{CAPSLOCK}" +
                          "{NUMLOCK}{SCROLLLOCK}")

        For Each keyToTest In keysToTest
            Assert.True(gw.KeyAvailable)
            Dim key As KeyInfo = gw.ReadKeyInfo()
            Assert.StrictEqual(keyToTest, key.Key.KeyData)
            Assert.StrictEqual(ControlChars.NullChar, key.KeyChar)
        Next

        ' Cannot assert that the key buffer is empty here because caps lock, number lock and scroll
        ' lock get registered in the buffer twice. We have manually tested that if those keys are
        ' pressed in the keyboard instead of by SendKeys, each of them registers only once in the
        ' buffer.
    End Sub

    <Fact>
    Public Sub EmptyKeys()
        gw.Form.BringToFront()
        Application.DoEvents()

        Assert.False(gw.KeyAvailable)
        Assert.Null(gw.ReadKeyIfAvailable)

        SendKeys.SendWait("12+3")
        Assert.True(gw.KeyAvailable)

        gw.EmptyKeys()
        Assert.False(gw.KeyAvailable)
    End Sub

    <Fact>
    Public Sub CloseForm()
        gw.CanClose = True
        gw.Form.Close()
        Assert.False(gw.IsLiving)
    End Sub

    <Fact>
    Public Sub CloseFormCancelled()
        gw.Form.Close()
        Assert.True(gw.IsLiving)
    End Sub

    <Fact>
    Public Sub DisposeGraphics()
        gw.Dispose()
        Assert.False(gw.IsLiving)
    End Sub

    <Fact>
    Public Sub GetImage()
        Dim size As Size = gw.Image.Size

        gw.Clear(Color.Azure)
        Dim bitmap As Bitmap = gw.GetBitmap()
        Assert.StrictEqual(Size, bitmap.Size)

        For y As Integer = 0 To Size.Height - 1
            For x As Integer = 0 To Size.Width - 1
                Assert.StrictEqual(Color.FromArgb(Color.Azure.ToArgb()), bitmap.GetPixel(x, y))
            Next
        Next
    End Sub
End Class
