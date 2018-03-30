Option Strict On

Imports System.Drawing
Imports System.Runtime.CompilerServices

Namespace Global.VBGraphics

    Public Module Shapes
        ''' <summary>
        ''' Fill the graphics windows with the specified color.
        ''' </summary>
        ''' <param name="gw">The graphics window object</param>
        ''' <param name="color">The color</param>
        <Extension()>
        Public Sub Clear(gw As GraphicsWindow, color As Color)
            gw.CreateGraphics().Clear(color)
        End Sub

#Region "Line"
        ''' <summary>
        ''' Draw a line from (x1, y1) to (x2, y2) in the graphics window with the specified color
        ''' and without anti-aliasing.
        ''' </summary>
        ''' <param name="gw">The graphics window object</param>
        ''' <param name="x1">The x-coordinate of the first point</param>
        ''' <param name="y1">The y-coordinate of the first point</param>
        ''' <param name="x2">The x-coordinate of the second point</param>
        ''' <param name="y2">y-coordinate of the first point</param>
        ''' <param name="color">The color</param>
        <Extension()>
        Public Sub DrawLine(gw As GraphicsWindow, x1 As Integer, y1 As Integer,
                            x2 As Integer, y2 As Integer, color As Color)
            gw.CreateGraphics().DrawLine(New Pen(color), x1, y1, x2, y2)
            gw.Invalidate()
        End Sub

        ''' <summary>
        ''' Draw a line from (x1, y1) to (x2, y2) in the graphics window with the specified color
        ''' and line width, and without anti-aliasing.
        ''' </summary>
        ''' <param name="gw">The graphics window object</param>
        ''' <param name="x1">The x-coordinate of the first point</param>
        ''' <param name="y1">The y-coordinate of the first point</param>
        ''' <param name="x2">The x-coordinate of the second point</param>
        ''' <param name="y2">y-coordinate of the first point</param>
        ''' <param name="color">The color</param>
        ''' <param name="width">Line width</param>
        <Extension()>
        Public Sub DrawLine(gw As GraphicsWindow, x1 As Integer, y1 As Integer,
                            x2 As Integer, y2 As Integer, color As Color, width As Single)
            gw.CreateGraphics().DrawLine(New Pen(color, width), x1, y1, x2, y2)
            gw.Invalidate()
        End Sub

        ''' <summary>
        ''' Draw a line from (x1, y1) to (x2, y2) in the graphics window with the specified color
        ''' and with anti-aliasing.
        ''' </summary>
        ''' <param name="gw">The graphics window object</param>
        ''' <param name="x1">The x-coordinate of the first point</param>
        ''' <param name="y1">The y-coordinate of the first point</param>
        ''' <param name="x2">The x-coordinate of the second point</param>
        ''' <param name="y2">y-coordinate of the first point</param>
        ''' <param name="color">The color</param>
        <Extension()>
        Public Sub DrawLineWithSmoothing(gw As GraphicsWindow, x1 As Integer, y1 As Integer,
                                         x2 As Integer, y2 As Integer, color As Color)
            gw.CreateGraphicsWithSmoothing().DrawLine(New Pen(color), x1, y1, x2, y2)
            gw.Invalidate()
        End Sub

        ''' <summary>
        ''' Draw a line from (x1, y1) to (x2, y2) in the graphics window with the specified color
        ''' and line width, and with anti-aliasing.
        ''' </summary>
        ''' <param name="gw">The graphics window object</param>
        ''' <param name="x1">The x-coordinate of the first point</param>
        ''' <param name="y1">The y-coordinate of the first point</param>
        ''' <param name="x2">The x-coordinate of the second point</param>
        ''' <param name="y2">y-coordinate of the first point</param>
        ''' <param name="color">The color</param>
        ''' <param name="width">Line width</param>
        <Extension()>
        Public Sub DrawLineWithSmoothing(gw As GraphicsWindow, x1 As Integer, y1 As Integer,
                                         x2 As Integer, y2 As Integer, color As Color,
                                         width As Single)
            gw.CreateGraphicsWithSmoothing().DrawLine(New Pen(color, width), x1, y1, x2, y2)
            gw.Invalidate()
        End Sub
#End Region

#Region "Rectangle"
        ''' <summary>
        ''' Draw a rentangle with the top-left corner at (x, y), and the specified width, height,
        ''' fill color and stroke color.
        ''' </summary>
        ''' <param name="gw">The graphics object</param>
        ''' <param name="x">The x-coordinate of the top-left corner of the rectangle</param>
        ''' <param name="y">The y-coordinate of the top-left corner of the rectangle</param>
        ''' <param name="width">The width of the rectangle</param>
        ''' <param name="height">The height of the rectangle</param>
        ''' <param name="fill">The color to fill, or Nothing for no fill</param>
        ''' <param name="stroke">The color of the stroke, or Nothing for no stroke</param>
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

        ''' <summary>
        ''' Draw a rentangle with the top-left corner at (x, y), and the specified width, height,
        ''' fill color, stroke color, and stroke width. The stroke is drawn without anti-aliasing.
        ''' </summary>
        ''' <param name="gw">The graphics object</param>
        ''' <param name="x">The x-coordinate of the top-left corner of the rectangle</param>
        ''' <param name="y">The y-coordinate of the top-left corner of the rectangle</param>
        ''' <param name="width">The width of the rectangle</param>
        ''' <param name="height">The height of the rectangle</param>
        ''' <param name="fill">The color to fill, or Nothing for no fill</param>
        ''' <param name="stroke">The color of the stroke, or Nothing for no stroke</param>
        ''' <param name="strokeWidth">The width of the stroke</param>
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

        ''' <summary>
        ''' Draw a rentangle with the top-left corner at (x, y), and the specified width, height,
        ''' fill color, and stroke color. The stroke is drawn with anti-aliasing.
        ''' </summary>
        ''' <param name="gw">The graphics object</param>
        ''' <param name="x">The x-coordinate of the top-left corner of the rectangle</param>
        ''' <param name="y">The y-coordinate of the top-left corner of the rectangle</param>
        ''' <param name="width">The width of the rectangle</param>
        ''' <param name="height">The height of the rectangle</param>
        ''' <param name="fill">The color to fill, or Nothing for no fill</param>
        ''' <param name="stroke">The color of the stroke, or Nothing for no stroke</param>
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

        ''' <summary>
        ''' Draw a rentangle with the top-left corner at (x, y), and the specified width, height,
        ''' fill color, stroke color, and stroke width. The stroke is drawn without anti-aliasing.
        ''' </summary>
        ''' <param name="gw">The graphics object</param>
        ''' <param name="x">The x-coordinate of the top-left corner of the rectangle</param>
        ''' <param name="y">The y-coordinate of the top-left corner of the rectangle</param>
        ''' <param name="width">The width of the rectangle</param>
        ''' <param name="height">The height of the rectangle</param>
        ''' <param name="fill">The color to fill, or Nothing for no fill</param>
        ''' <param name="stroke">The color of the stroke, or Nothing for no stroke</param>
        ''' <param name="strokeWidth">The width of the stroke</param>
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
        ''' <summary>
        ''' Draw an ellipse defined by a bounding rectangle, and with the specified fill color and
        ''' stroke color. The bounding rectangle has its top-left corner at (x, y), and the
        ''' specified width and height. The stroke is drawn without anti-aliasing.
        ''' </summary>
        ''' <param name="gw">The graphics object</param>
        ''' <param name="x">The x-coordinate of the top-left corner of the bounding rectangle that
        ''' defines the ellipse.</param>
        ''' <param name="y">The y-coordinate of the top-left corner of the bounding rectangle that
        ''' defines the ellipse.</param>
        ''' <param name="width">The width of the bounding rectangle that defines the ellipse</param>
        ''' <param name="height">The height of the bounding rectangle that defines the ellipse
        ''' </param>
        ''' <param name="fill">The color to fill, or Nothing for no fill</param>
        ''' <param name="stroke">The color of the stroke, or Nothing for no stroke</param>
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

        ''' <summary>
        ''' Draw an ellipse defined by a bounding rectangle, and with the specified fill color,
        ''' stroke color, and stroke width. The bounding rectangle has its top-left corner at
        ''' (x, y), and the specified width and height. The stroke is drawn without anti-aliasing.
        ''' </summary>
        ''' <param name="gw">The graphics object</param>
        ''' <param name="x">The x-coordinate of the top-left corner of the bounding rectangle that
        ''' defines the ellipse.</param>
        ''' <param name="y">The y-coordinate of the top-left corner of the bounding rectangle that
        ''' defines the ellipse.</param>
        ''' <param name="width">The width of the bounding rectangle that defines the ellipse</param>
        ''' <param name="height">The height of the bounding rectangle that defines the ellipse
        ''' </param>
        ''' <param name="fill">The color to fill, or Nothing for no fill</param>
        ''' <param name="stroke">The color of the stroke, or Nothing for no stroke</param>
        ''' <param name="strokeWidth">The width of the stroke</param>
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

        ''' <summary>
        ''' Draw an ellipse defined by a bounding rectangle, and with the specified fill color and
        ''' stroke color. The bounding rectangle has its top-left corner at (x, y), and the
        ''' specified width and height. The stroke is drawn with anti-aliasing.
        ''' </summary>
        ''' <param name="gw">The graphics object</param>
        ''' <param name="x">The x-coordinate of the top-left corner of the bounding rectangle that
        ''' defines the ellipse.</param>
        ''' <param name="y">The y-coordinate of the top-left corner of the bounding rectangle that
        ''' defines the ellipse.</param>
        ''' <param name="width">The width of the bounding rectangle that defines the ellipse</param>
        ''' <param name="height">The height of the bounding rectangle that defines the ellipse
        ''' </param>
        ''' <param name="fill">The color to fill, or Nothing for no fill</param>
        ''' <param name="stroke">The color of the stroke, or Nothing for no stroke</param>
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

        ''' <summary>
        ''' Draw an ellipse defined by a bounding rectangle, and with the specified fill color,
        ''' stroke color, and stroke width. The bounding rectangle has its top-left corner at
        ''' (x, y), and the specified width and height. The stroke is drawn with anti-aliasing.
        ''' </summary>
        ''' <param name="gw">The graphics object</param>
        ''' <param name="x">The x-coordinate of the top-left corner of the bounding rectangle that
        ''' defines the ellipse.</param>
        ''' <param name="y">The y-coordinate of the top-left corner of the bounding rectangle that
        ''' defines the ellipse.</param>
        ''' <param name="width">The width of the bounding rectangle that defines the ellipse</param>
        ''' <param name="height">The height of the bounding rectangle that defines the ellipse
        ''' </param>
        ''' <param name="fill">The color to fill, or Nothing for no fill</param>
        ''' <param name="stroke">The color of the stroke, or Nothing for no stroke</param>
        ''' <param name="strokeWidth">The width of the stroke</param>
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