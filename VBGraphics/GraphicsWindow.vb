Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Linq
Imports System.Windows.Forms

<Assembly: CLSCompliant(True)>
Namespace Global.VBGraphics
    ''' <summary>
    ''' This is the main class of the VBGraphics package. To use VBGraphics, create a GraphicsWinodw
    ''' object and then use its methods. Note that most of the methods are implemented as extension
    ''' methods. So the following is needed at the beginning of your source file:
    ''' <code language="lang-vb">
    ''' Imports VBGraphics
    ''' Imports VBGraphics.BitmapImage
    ''' Imports VBGraphics.Shapes
    ''' Imports VBGraphics.Text
    ''' </code>
    ''' </summary>
    Public Class GraphicsWindow
        Implements IDisposable

        Private WithEvents _Form As GraphicsForm
        Private _keys As New Queue(Of KeyInfo)
        Private _currentKey As KeyEventArgs
        Private _disposing As Boolean = False

        ''' <summary>
        ''' Create a graphics window with the specified width and height, and fills it with black.
        ''' </summary>
        ''' <param name="width">The width of the graphics window, in pixels.</param>
        ''' <param name="height">The height of the graphics window, in pixels.</param>
        Sub New(width As Integer, height As Integer)
            Me.New(width, height, Color.Black)
        End Sub

        ''' <summary>
        ''' Create a graphics window with the specified width and height, and fills it with the
        ''' specified color.
        ''' </summary>
        ''' <param name="width">The width of the graphics window, in pixels.</param>
        ''' <param name="height">The height of the graphics window, in pixels.</param>
        ''' <param name="color">The color to fill in.</param>
        Sub New(width As Integer, height As Integer, color As Color)
            _Image = New Bitmap(width, height)
            Graphics.FromImage(_Image).Clear(color)

            _Form = New GraphicsForm() With {
                .ClientSize = New Size(width, height),
                .AutoScaleMode = AutoScaleMode.None
            }
            _Form.Show()
        End Sub

        ''' <summary>
        ''' <para>Close the graphics window, and release all unmanaged resources used by the window.
        ''' This raises the WindowClosed event, and if EndProgramOnClose is true, also quits the
        ''' program.</para>
        ''' <para>When Dispose is run, the graphics window closes even if CanClose is false. The
        ''' event WindowClosing is not raised.</para>
        ''' </summary>
        Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        Protected Overridable Sub Dispose(disposing As Boolean)
            _disposing = True
            _Form.Close()
            _Image.Dispose()
        End Sub

        ''' <summary>
        ''' WindowClosing is raised when the user close the form, when GraphicsWindow.Form.Close is
        ''' called, and when Application.Exit is called. However, the event is not raised when
        ''' GraphicsWindow is disposed of, nor when Windows shut down.
        ''' </summary>
        Public Event WindowClosing(sender As GraphicsWindow, e As FormClosingEventArgs)

        ''' <summary>
        ''' WindowClosed is raised when the GraphicsWindow is closed, regardless of the reason of
        ''' closing the window.  In the case of disposing GraphicsWindows, the image in
        ''' GraphicsWindow is still usable in the duration of this event.
        ''' </summary>
        Public Event WindowClosed(sender As Object, e As FormClosedEventArgs)

        ''' <summary>
        ''' Gets a boolean indicating whether the graphics windows is open.
        ''' </summary>
        ReadOnly Property IsLiving As Boolean = True

        ''' <summary>
        ''' <para>Gets or sets a boolean indicating whether the GraphicsWindow can be closed by user
        ''' action, Form.Close or Application.Exit. However, this has no effect when the
        ''' GraphicsWindows is disposed, or when Windows shut down.</para>
        ''' <para>To prevent unexpected results, set EndProgramOnClose to the desired value and add
        ''' the listener to WindowClosing and WindowClosed events before setting CanClose to true.
        ''' </para>
        ''' <para>Similarly, set CanClose to false before setting EngProgramOnClose and removing
        ''' event listeners.</para>
        ''' </summary>
        Property CanClose As Boolean = False

        ''' <summary>
        ''' If true, Environment.Exit is called after GraphicsWindow is closed. EndProgramOnClose is
        ''' handled after WindowClosed event is raised.
        ''' </summary>
        Property EndProgramOnClose As Boolean = True

        ''' <summary>
        ''' <para>Gets or sets a boolean indicating whether modifier keys (Ctrl, Alt, Shift) are
        ''' recorded to the key queue.</para>
        ''' <para>Note: The Windows key is not recorded, regardless of the value of this property.
        ''' Also, the Alt key comes with nasty surprises, and it should not be one of the controls
        ''' of your app.</para>
        ''' </summary>
        Property CaptureModifierKeys As Boolean = False

        ''' <summary>
        ''' Gets the Form used by the graphics window.
        ''' </summary>
        ReadOnly Property Form As Form
            Get
                Return _Form
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the image used by the graphics window. Setting this property to an object
        ''' other than a Bitmap is undefined behavior (it may work, but this is not tested).
        ''' </summary>
        ReadOnly Property Image As Image

        ''' <summary>
        ''' Invalidate the graphics window, causing it to be redrawn. This should be called after
        ''' you manipulate the image of the graphics window manually.
        ''' </summary>
        Public Sub Invalidate()
            _Form.Invalidate()
        End Sub

        ''' <summary>
        ''' <para>Create a Graphics object of the image of the graphics window. You can use this
        ''' object to draw on the image manually.</para>
        ''' <para>After drawing, call the Invalidate method of the GraphicsWindow to make the
        ''' graphics window redraw. Otherwise, the operation you do to the image may not be shown on
        ''' the screen.</para>
        ''' </summary>
        ''' <returns>A Graphics object of the image of the graphics window.</returns>
        Public Function CreateGraphics() As Graphics
            Return Graphics.FromImage(Image)
        End Function

        ''' <summary>
        ''' <para>Create a Graphics object of the image of the graphics window, with the smoothing
        ''' mode of lines, curves and edges of filled areas set to antialiasing. You can use this
        ''' object to draw on the image manually.</para>
        ''' <para>After drawing, call the Invalidate method of the GraphicsWindow to make the
        ''' graphics window redraw. Otherwise, the operation you do to the image may not be shown on
        ''' the screen.</para>
        ''' </summary>
        ''' <returns>A Graphics object of the image of the graphics window.</returns>
        Public Function CreateGraphicsWithSmoothing() As Graphics
            Dim g As Graphics = CreateGraphics()
            g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            Return g
        End Function

#Region "Key Handlers"
        ''' <summary>
        ''' Gets a boolean indicating whether keys are present in the queue.
        ''' </summary>
        ReadOnly Property KeyAvailable As Boolean
            Get
                Application.DoEvents()
                Return _keys.Count > 0
            End Get
        End Property

        ''' <summary>
        ''' Returns the next key in the queue as KeyEventArgs. If no key is in the queue, wait until
        ''' the user press the next key.
        ''' </summary>
        Public Function ReadKey() As KeyEventArgs
            WaitUntilKeyAvailable()
            Return ReadKeyIfAvailable()
        End Function

        ''' <summary>
        ''' Returns the next key in the queue as KeyEventArgs, or Nothing if no key is in the queue.
        ''' </summary>
        Public Function ReadKeyIfAvailable() As KeyEventArgs
            Return If(KeyAvailable, _keys.Dequeue().Key, Nothing)
        End Function

        ''' <summary>
        ''' Returns the next key in the queue as Char. If no key is in the queue, wait until the
        ''' user press the next key. Note that some keys are associated the null character.
        ''' </summary>
        Public Function ReadKeyChar() As Char
            WaitUntilKeyAvailable()
            Return ReadKeyCharIfAvailable()
        End Function

        ''' <summary>
        ''' Returns the next key in the queue as Char, or the null character if no key is in the
        ''' queue. Note that some keys are associated the null character, so a null return does not
        ''' indicate that no keys are pressed.
        ''' </summary>
        Public Function ReadKeyCharIfAvailable() As Char
            Return If(KeyAvailable, _keys.Dequeue().KeyChar,
                      Microsoft.VisualBasic.ControlChars.NullChar)
        End Function

        ''' <summary>
        ''' Returns the next key in the queue as KeyInfo structure. If no key is in the queue, wait
        ''' until the user press the next key.
        ''' </summary>
        Public Function ReadKeyInfo() As KeyInfo
            WaitUntilKeyAvailable()
            Return ReadKeyInfoIfAvailable()
        End Function

        ''' <summary>
        ''' Returns the next key in the queue as KeyInfo structure, or Nothing if no key is in the
        ''' queue.
        ''' </summary>
        Public Function ReadKeyInfoIfAvailable() As KeyInfo
            Return If(KeyAvailable, _keys.Dequeue(), Nothing)
        End Function

        ''' <summary>
        ''' If the key queue is empty and the graphics window is open, wait until the user press a
        ''' key.
        ''' </summary>
        Public Sub WaitUntilKeyAvailable()
            While Not KeyAvailable AndAlso IsLiving
            End While
        End Sub

        ''' <summary>
        ''' Empty the key queue.
        ''' </summary>
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