Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Namespace Global.VBGraphics
    Public Class GraphicsWindow
        Implements IDisposable

        Private WithEvents _Form As GraphicsForm
        Private _keys As New Queue(Of KeyInfo)
        Private _currentKey As KeyEventArgs

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
            _CanClose = True
            _Form.Close()
            _Image.Dispose()
        End Sub

        ReadOnly Property IsLiving As Boolean = True
        Property CanClose As Boolean = False

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
            While Not KeyAvailable And IsLiving
                Application.DoEvents()
            End While
        End Sub

        Public Sub EmptyKeys()
            _keys.Clear()
        End Sub

        Private Sub _Form_KeyDown(sender As Object, e As KeyEventArgs) Handles _Form.KeyDown
            _currentKey = e
        End Sub

        Private Sub _Form_KeyPress(sender As Object, e As KeyPressEventArgs) Handles _Form.KeyPress
            _keys.Enqueue(New KeyInfo(_currentKey, e.KeyChar))
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
            If Not _CanClose AndAlso
               e.CloseReason <> CloseReason.WindowsShutDown AndAlso
               e.CloseReason <> CloseReason.ApplicationExitCall Then
                e.Cancel = True
            End If
        End Sub

        Private Sub _Form_FormClosed(sender As Object, e As FormClosedEventArgs) _
            Handles _Form.FormClosed
            _IsLiving = False
        End Sub
    End Class

End Namespace