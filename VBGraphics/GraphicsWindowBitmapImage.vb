﻿Option Strict On

Imports System.Drawing
Imports System.Runtime.CompilerServices

Namespace Global.VBGraphics

    Public Module BitmapImage
        ''' <summary>
        ''' Draw the image at point (x, y).
        ''' </summary>
        ''' <param name="gw">The graphics object</param>
        ''' <param name="image">The image to draw</param>
        ''' <param name="x">The x-coordinate of the top-left corner of the drawn image</param>
        ''' <param name="y">The y-coordinate of the top-left corner of the drawn image</param>
        <Extension()>
        Sub DrawImage(gw As GraphicsWindow, image As Image, x As Integer, y As Integer)
            gw.CreateGraphics().DrawImage(image, x, y)
            gw.Invalidate()
        End Sub

        ''' <summary>
        ''' Draw the image at point (x, y), where the image is resized to the specified width and
        ''' height. The drawn image is not smoothed.
        ''' </summary>
        ''' <param name="gw">The graphics object</param>
        ''' <param name="image">The image to draw</param>
        ''' <param name="x">The x-coordinate of the top-left corner of the drawn image</param>
        ''' <param name="y">The y-coordinate of the top-left corner of the drawn image</param>
        ''' <param name="width">The resized width</param>
        ''' <param name="height">The resized height</param>
        <Extension()>
        Sub DrawImageResized(gw As GraphicsWindow, image As Image, x As Integer, y As Integer,
                             width As Integer, height As Integer)
            Dim g As Graphics = gw.CreateGraphics()
            g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
            g.DrawImage(image, x, y, width, height)
            gw.Invalidate()
        End Sub

        ''' <summary>
        ''' Draw the image at point (x, y), where the image is resized to the specified width and
        ''' height. The drawn image is smoothed.
        ''' </summary>
        ''' <param name="gw">The graphics object</param>
        ''' <param name="image">The image to draw</param>
        ''' <param name="x">The x-coordinate of the top-left corner of the drawn image</param>
        ''' <param name="y">The y-coordinate of the top-left corner of the drawn image</param>
        ''' <param name="width">The resized width</param>
        ''' <param name="height">The resized height</param>
        <Extension()>
        Sub DrawImageResizedWithSmoothing(gw As GraphicsWindow, image As Image, x As Integer,
                                          y As Integer, width As Integer, height As Integer)
            Dim g As Graphics = gw.CreateGraphics()
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half

            If TypeOf image IsNot Bitmap Then
                ' Vector: just draw the image.
                g.DrawImage(image, x, y, width, height)
            Else
                ' Bitmap, expand picture by 2 pixels for anti-aliasing.
                Dim expandedImage As New Bitmap(image.Width + 2, image.Height + 2)
                Dim eg As Graphics = Graphics.FromImage(expandedImage)
                eg.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
                eg.DrawImage(image, 1, 1)
                eg.DrawImage(image, 1, 0, New Rectangle(0, 0, image.Width, 1), GraphicsUnit.Pixel)
                eg.DrawImage(image, 0, 1, New Rectangle(0, 0, 1, height), GraphicsUnit.Pixel)
                eg.DrawImage(image, 1, image.Height + 1,
                             New Rectangle(0, image.Height - 1, image.Width, 1), GraphicsUnit.Pixel)
                eg.DrawImage(image, image.Width + 1, 1,
                             New Rectangle(image.Width - 1, 0, 1, height), GraphicsUnit.Pixel)
                expandedImage.SetPixel(0, 0, expandedImage.GetPixel(1, 1))
                expandedImage.SetPixel(image.Width + 1, 0, expandedImage.GetPixel(image.Width, 1))
                expandedImage.SetPixel(0, image.Height + 1, expandedImage.GetPixel(1, image.Height))
                expandedImage.SetPixel(image.Width + 1, image.Height + 1,
                                    expandedImage.GetPixel(image.Width, image.Height))

                g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                g.DrawImage(expandedImage, New Rectangle(x, y, width, height),
                            New Rectangle(1, 1, image.Width, image.Height), GraphicsUnit.Pixel)
            End If

            gw.Invalidate()
        End Sub

        ''' <summary>
        ''' Draw the specified rectangular part of the image at point (x, y).
        ''' </summary>
        ''' <param name="gw">The graphics object</param>
        ''' <param name="image">The image to draw</param>
        ''' <param name="x">The x-coordinate of the top-left corner of the drawn image</param>
        ''' <param name="y">The y-coordinate of the top-left corner of the drawn image</param>
        ''' <param name="srcX">The x-coordinate of the top-left corner of the specified part pf the
        ''' source image</param>
        ''' <param name="srcY">The y-coordinate of the top-left corner of the specified part of the
        ''' source image</param>
        ''' <param name="srcWidth">The width of the specified part of the source image</param>
        ''' <param name="srcHeight">The height of the specified part of the source image</param>
        <Extension()>
        Sub DrawImageWithClipping(gw As GraphicsWindow, image As Image, x As Integer, y As Integer,
                                  srcX As Integer, srcY As Integer, srcWidth As Integer,
                                  srcHeight As Integer)
            gw.CreateGraphics().DrawImage(image, x, y,
                                          New Rectangle(srcX, srcY, srcWidth, srcHeight),
                                          GraphicsUnit.Pixel)
            gw.Invalidate()
        End Sub

        ''' <summary>
        ''' Return the graphics window as a bitmap.
        ''' </summary>
        ''' <param name="gw">The graphics window</param>
        ''' <returns>The bitmap of the graphics window</returns>
        <Extension()>
        Function GetBitmap(gw As GraphicsWindow) As Bitmap
            Return New Bitmap(gw.Image)
        End Function

        ''' <summary>
        ''' Return the specified rectangular part of the graphics window as a bitmap.
        ''' </summary>
        ''' <param name="gw">The graphics window</param>
        ''' <param name="x">The x-coordinate of the top-left corner of the specified part</param>
        ''' <param name="y">The y-coordinate of the top-left corner of the specified part</param>
        ''' <param name="width">The width of the specfied part</param>
        ''' <param name="height">The height of the specfied part</param>
        ''' <returns>The bitmap of the specified rectangular part of graphics window</returns>
        <Extension()>
        Function GetBitmap(gw As GraphicsWindow, x As Integer, y As Integer,
                           width As Integer, height As Integer) As Bitmap
            Dim bitmap As Bitmap = New Bitmap(gw.Image)
            Return bitmap.Clone(New Rectangle(x, y, width, height), bitmap.PixelFormat)
        End Function
    End Module

End Namespace