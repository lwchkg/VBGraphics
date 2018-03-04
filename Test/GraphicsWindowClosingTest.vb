Option Strict On

Imports System.Windows.Forms
Imports VBGraphics
Imports Xunit

Public Class GraphicsWindowClosingTest
    Implements IDisposable

    Dim WithEvents gw As GraphicsWindow
    Dim cancelClose As Boolean = False
    Dim windowClosingCount As Integer = 0
    Dim windowClosedCount As Integer = 0

    Private Sub gw_WindowClosing(sender As GraphicsWindow, e As FormClosingEventArgs) Handles gw.WindowClosing
        windowClosingCount += 1
        If cancelClose Then
            e.Cancel = True
        End If
    End Sub

    Private Sub gw_WindowClosed(sender As Object, e As FormClosedEventArgs) Handles gw.WindowClosed
        windowClosedCount += 1
    End Sub

    Public Sub New()
        gw = New GraphicsWindow(100, 100)
        gw.EndProgramOnClose = False
        gw.CanClose = True
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
    End Sub

    Protected Overridable Sub Dispose(disposing As Boolean)
        gw.Dispose()
        GC.SuppressFinalize(Me)
    End Sub

    <Fact>
    Public Sub CloseGraphicsWindowNotCancelled()
        Assert.StrictEqual(0, windowClosingCount)
        Assert.StrictEqual(0, windowClosedCount)
        cancelClose = False
        gw.Form.Close()
        Assert.StrictEqual(1, windowClosingCount)
        Assert.StrictEqual(1, windowClosedCount)
        Assert.False(gw.IsLiving)
    End Sub

    <Fact>
    Public Sub CloseGraphicsWindowCancelled()
        Assert.StrictEqual(0, windowClosingCount)
        Assert.StrictEqual(0, windowClosedCount)
        cancelClose = True
        gw.Form.Close()
        Assert.StrictEqual(1, windowClosingCount)
        Assert.StrictEqual(0, windowClosedCount)
        Assert.True(gw.IsLiving)
    End Sub

    <Fact>
    Public Sub DisposeGraphicsWindow()
        Assert.StrictEqual(0, windowClosingCount)
        Assert.StrictEqual(0, windowClosedCount)
        gw.CanClose = False
        cancelClose = True

        gw.Dispose()
        Assert.StrictEqual(0, windowClosingCount)
        Assert.StrictEqual(1, windowClosedCount)
        Assert.False(gw.IsLiving)

        ' Dispose a second time. The events should not be called.
        gw.Dispose()
        Assert.StrictEqual(0, windowClosingCount)
        Assert.StrictEqual(1, windowClosedCount)
        Assert.False(gw.IsLiving)
    End Sub
End Class
