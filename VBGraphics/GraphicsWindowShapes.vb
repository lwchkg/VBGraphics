Option Strict On

Imports System.Drawing
Imports System.Runtime.CompilerServices

Namespace Global.VBGraphics

    Public Module Shapes
        <Extension()>
        Public Sub Clear(gw As GraphicsWindow, color As Color)
            gw.CreateGraphics().Clear(color)
        End Sub

#Region "Line"
        <Extension()>
        Public Sub DrawLine(gw As GraphicsWindow, x1 As Integer, y1 As Integer,
                            x2 As Integer, y2 As Integer, color As Color)
            gw.CreateGraphics().DrawLine(New Pen(color), x1, y1, x2, y2)
            gw.Invalidate()
        End Sub

        <Extension()>
        Public Sub DrawLine(gw As GraphicsWindow, x1 As Integer, y1 As Integer,
                            x2 As Integer, y2 As Integer, color As Color, width As Single)
            gw.CreateGraphics().DrawLine(New Pen(color, width), x1, y1, x2, y2)
            gw.Invalidate()
        End Sub

        <Extension()>
        Public Sub DrawLineWithSmoothing(gw As GraphicsWindow, x1 As Integer, y1 As Integer,
                                         x2 As Integer, y2 As Integer, color As Color)
            gw.CreateGraphicsWithSmoothing().DrawLine(New Pen(color), x1, y1, x2, y2)
            gw.Invalidate()
        End Sub

        <Extension()>
        Public Sub DrawLineWithSmoothing(gw As GraphicsWindow, x1 As Integer, y1 As Integer,
                                         x2 As Integer, y2 As Integer, color As Color,
                                         width As Single)
            gw.CreateGraphicsWithSmoothing().DrawLine(New Pen(color, width), x1, y1, x2, y2)
            gw.Invalidate()
        End Sub
#End Region

#Region "Rectangle"
        <Extension()>
        Public Sub DrawRectangle(gw As GraphicsWindow, x As Integer, y As Integer,
                                 width As Integer, height As Integer, fill As Color?,
                                 stroke As Color?)
            If fill.HasValue Then
                gw.CreateGraphics().FillRectangle(New SolidBrush(fill.Value), x, y, width, height)
            End If

            If stroke.HasValue Then
                gw.CreateGraphics().DrawRectangle(New Pen(stroke.Value), x, y, width, height)
            End If
            gw.Invalidate()
        End Sub

        <Extension()>
        Public Sub DrawRectangle(gw As GraphicsWindow, x As Integer, y As Integer,
                                 width As Integer, height As Integer, fill As Color?,
                                 stroke As Color, strokeWidth As Single)
            If fill.HasValue Then
                gw.CreateGraphics().FillRectangle(New SolidBrush(fill.Value), x, y, width, height)
            End If

            gw.CreateGraphics().DrawRectangle(New Pen(stroke, strokeWidth), x, y, width, height)
            gw.Invalidate()
        End Sub

        <Extension()>
        Public Sub DrawRectangleWithSmoothing(gw As GraphicsWindow, x As Integer, y As Integer,
                                              width As Integer, height As Integer, fill As Color?,
                                              stroke As Color?)
            If fill.HasValue Then
                gw.CreateGraphicsWithSmoothing().FillRectangle(New SolidBrush(fill.Value),
                                                               x, y, width, height)
            End If

            If stroke.HasValue Then
                gw.CreateGraphicsWithSmoothing().DrawRectangle(New Pen(stroke.Value),
                                                               x, y, width, height)
            End If
            gw.Invalidate()
        End Sub

        <Extension()>
        Public Sub DrawRectangleWithSmoothing(gw As GraphicsWindow, x As Integer, y As Integer,
                                              width As Integer, height As Integer, fill As Color?,
                                              stroke As Color, strokeWidth As Single)
            If fill.HasValue Then
                gw.CreateGraphicsWithSmoothing().FillRectangle(New SolidBrush(fill.Value),
                                                               x, y, width, height)
            End If

            gw.CreateGraphicsWithSmoothing().DrawRectangle(New Pen(stroke, strokeWidth),
                                                           x, y, width, height)
            gw.Invalidate()
        End Sub
#End Region

#Region "Ellipse"
        <Extension()>
        Public Sub DrawEllipse(gw As GraphicsWindow, x As Integer, y As Integer,
                               width As Integer, height As Integer, fill As Color?,
                               stroke As Color?)
            If fill.HasValue Then
                gw.CreateGraphics().FillEllipse(New SolidBrush(fill.Value), x, y, width, height)
            End If

            If stroke.HasValue Then
                gw.CreateGraphics().DrawEllipse(New Pen(stroke.Value), x, y, width, height)
            End If
            gw.Invalidate()
        End Sub

        <Extension()>
        Public Sub DrawEllipse(gw As GraphicsWindow, x As Integer, y As Integer,
                               width As Integer, height As Integer, fill As Color?,
                               stroke As Color, strokeWidth As Single)
            If fill.HasValue Then
                gw.CreateGraphics().FillEllipse(New SolidBrush(fill.Value), x, y, width, height)
            End If

            gw.CreateGraphics().DrawEllipse(New Pen(stroke, strokeWidth), x, y, width, height)
            gw.Invalidate()
        End Sub

        <Extension()>
        Public Sub DrawEllipseWithSmoothing(gw As GraphicsWindow, x As Integer, y As Integer,
                                            width As Integer, height As Integer, fill As Color?,
                                            stroke As Color?)
            If fill.HasValue Then
                gw.CreateGraphicsWithSmoothing().FillEllipse(New SolidBrush(fill.Value),
                                                             x, y, width, height)
            End If

            If stroke.HasValue Then
                gw.CreateGraphicsWithSmoothing().DrawEllipse(New Pen(stroke.Value),
                                                             x, y, width, height)
            End If
            gw.Invalidate()
        End Sub

        <Extension()>
        Public Sub DrawEllipseWithSmoothing(gw As GraphicsWindow, x As Integer, y As Integer,
                                            width As Integer, height As Integer, fill As Color?,
                                            stroke As Color, strokeWidth As Single)
            If fill.HasValue Then
                gw.CreateGraphicsWithSmoothing().FillEllipse(New SolidBrush(fill.Value),
                                                             x, y, width, height)
            End If

            gw.CreateGraphicsWithSmoothing().DrawEllipse(New Pen(stroke, strokeWidth),
                                                         x, y, width, height)
            gw.Invalidate()
        End Sub
    End Module
#End Region

End Namespace