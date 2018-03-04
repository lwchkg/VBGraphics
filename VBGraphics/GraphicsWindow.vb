Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Linq
Imports System.Windows.Forms

<Assembly: CLSCompliant(True)>
Namespace Global.VBGraphics
    Public Class GraphicsWindow
        Implements IDisposable

        Private WithEvents _Form As GraphicsForm
        Private _keys As New Queue(Of KeyInfo)
        Private _currentKey As KeyEventArgs
        Private _disposing As Boolean = False

        Sub New(width As Integer, height As Integer)
            Me.New(width, height, Color.Black)
        End Sub

        Sub New(width As Integer, height As Integer, color As Color)
            _Image = New Bitmap(width, height)
            Graphics.FromImage(_Image).Clear(color)

            _Form = New GraphicsForm() With {
                .ClientSize = New Size(width, height),
                .AutoScaleMode = AutoScaleMode.None
            }
            _Form.Show()
        End Sub

        Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        Protected Overridable Sub Dispose(disposing As Boolean)
            _disposing = True
            _Form.Close()
            _Image.Dispose()
        End Sub

        ' WindowClosing is raised when the user close the form, when GraphicsWindow.Form.Close is
        ' called, and when Application.Exit is called. However, the event is not raised when
        ' GraphicsWindow is disposed of, nor when Windows shut down.
        Public Event WindowClosing(sender As GraphicsWindow, e As FormClosingEventArgs)

        ' WindowClosed is always raised when the GraphicsWindow is closed. In the case of disposing
        ' GraphicsWindows, the image in GraphicsWindow is disposed after this event is raised.
        Public Event WindowClosed(sender As Object, e As FormClosedEventArgs)

        ReadOnly Property IsLiving As Boolean = True

        ' Whether GraphicsWindow can be closed by user action, Form.Close or Application.Exit.
        ' However, this is ignored when GraphicsWindows is disposed of, and when Windows shut down.
        Property CanClose As Boolean = False

        ' If true, calls Environment.Exit after GraphicsWindow is closed. EndProgramOnClose is
        ' handled after WindowClosed event is raised.
        Property EndProgramOnClose As Boolean = True

        Property CaptureModifierKeys As Boolean = False

        ReadOnly Property Form As Form
            Get
                Return _Form
            End Get
        End Property

        ReadOnly Property Image As Image

        Public Sub Invalidate()
            _Form.Invalidate()
        End Sub

        Public Function CreateGraphics() As Graphics
            Return Graphics.FromImage(Image)
        End Function

        Public Function CreateGraphicsWithSmoothing() As Graphics
            Dim g As Graphics = CreateGraphics()
            g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            Return g
        End Function

#Region "Key Handlers"
        ReadOnly Property KeyAvailable As Boolean
            Get
                Application.DoEvents()
                Return _keys.Count > 0
            End Get
        End Property

        Public Function ReadKey() As KeyEventArgs
            WaitUntilKeyAvailable()
            Return ReadKeyIfAvailable()
        End Function

        Public Function ReadKeyIfAvailable() As KeyEventArgs
            Return If(KeyAvailable, _keys.Dequeue().Key, Nothing)
        End Function

        Public Function ReadKeyChar() As Char
            WaitUntilKeyAvailable()
            Return ReadKeyCharIfAvailable()
        End Function

        Public Function ReadKeyCharIfAvailable() As Char
            Return If(KeyAvailable, _keys.Dequeue().KeyChar,
                      Microsoft.VisualBasic.ControlChars.NullChar)
        End Function

        Public Function ReadKeyInfo() As KeyInfo
            WaitUntilKeyAvailable()
            Return ReadKeyInfoIfAvailable()
        End Function

        Public Function ReadKeyInfoIfAvailable() As KeyInfo
            Return If(KeyAvailable, _keys.Dequeue(), Nothing)
        End Function

        Public Sub WaitUntilKeyAvailable()
            While Not KeyAvailable AndAlso IsLiving
            End While
        End Sub

        Public Sub EmptyKeys()
            _keys.Clear()
        End Sub

        Private Shared ReadOnly nonKeyPressKeys As Keys() = {
            Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8, Keys.F9,
            Keys.F10, Keys.F11, Keys.F12, Keys.F13, Keys.F14, Keys.F15, Keys.F16, Keys.F17,
            Keys.F18, Keys.F19, Keys.F20, Keys.F21, Keys.F22, Keys.F23, Keys.F24, Keys.Up,
            Keys.Down, Keys.Left, Keys.Right, Keys.PageUp, Keys.PageDown, Keys.Home, Keys.End,
            Keys.Insert, Keys.Delete, Keys.Pause, Keys.Clear, Keys.CapsLock, Keys.NumLock,
            Keys.Scroll, Keys.Apps
        }

        Private Shared ReadOnly modifierKeys As Keys() = {
            Keys.ControlKey, Keys.ShiftKey, Keys.Menu
        }

        Private Sub _Form_KeyDown(sender As Object, e As KeyEventArgs) Handles _Form.KeyDown
            _currentKey = e
            If nonKeyPressKeys.Contains(e.KeyCode) OrElse
               _CaptureModifierKeys AndAlso modifierKeys.Contains(e.KeyCode) Then
                _keys.Enqueue(New KeyInfo(_currentKey, Microsoft.VisualBasic.ControlChars.NullChar))
            End If
        End Sub

        Private Sub _Form_KeyPress(sender As Object, e As KeyPressEventArgs) Handles _Form.KeyPress
            _keys.Enqueue(New KeyInfo(_currentKey, e.KeyChar))
        End Sub

        Private Sub _Form_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) _
            Handles _Form.PreviewKeyDown
            e.IsInputKey = True
        End Sub
#End Region

        Private Sub _Form_Paint(sender As Object, e As PaintEventArgs) Handles _Form.Paint
            Dim scale As Double = Math.Min(e.ClipRectangle.Width / _Image.Width,
                                           e.ClipRectangle.Height / _Image.Height)

            Dim width As Integer = CInt(Math.Round(_Image.Width * scale))
            Dim height As Integer = CInt(Math.Round(_Image.Height * scale))

            Dim x As Integer = (e.ClipRectangle.Width - width) \ 2
            Dim y As Integer = (e.ClipRectangle.Height - height) \ 2

            Dim clearBrush As New SolidBrush(Color.Black)
            Dim g As Graphics = e.Graphics
            If width < e.ClipRectangle.Width Then
                g.FillRectangle(clearBrush, 0, 0, x, e.ClipRectangle.Height)
                g.FillRectangle(clearBrush, x + width, 0,
                                e.ClipRectangle.Width - x - width,
                                e.ClipRectangle.Height)
            End If
            If height < e.ClipRectangle.Height Then
                g.FillRectangle(clearBrush, 0, 0, e.ClipRectangle.Width, y)
                g.FillRectangle(clearBrush, 0, y + height,
                                e.ClipRectangle.Width,
                                e.ClipRectangle.Height - y - height)
            End If

            g.DrawImage(_Image, x, y, width, height)
        End Sub

        Private Sub _Form_Resize(sender As Object, e As EventArgs) Handles _Form.Resize
            _Form.Invalidate()
        End Sub

        Private Sub _Form_Closing(sender As Object, e As FormClosingEventArgs) _
            Handles _Form.FormClosing
            If _disposing OrElse e.CloseReason = CloseReason.WindowsShutDown Then
                Return
            End If

            If _CanClose OrElse e.CloseReason = CloseReason.ApplicationExitCall Then
                RaiseEvent WindowClosing(Me, e)
            Else
                e.Cancel = True
            End If
        End Sub

        Private Sub _Form_FormClosed(sender As Object, e As FormClosedEventArgs) _
            Handles _Form.FormClosed
            _IsLiving = False
            RaiseEvent WindowClosed(Me, e)
            If EndProgramOnClose Then
                Environment.Exit(0)
            End If
        End Sub
    End Class

End Namespace