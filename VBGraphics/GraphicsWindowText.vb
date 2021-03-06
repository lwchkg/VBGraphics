﻿Option Strict On

Imports System.Drawing
Imports System.Windows.Forms
Imports System.Runtime.CompilerServices

Namespace Global.VBGraphics
    Public Module Text
        ''' <summary>
        ''' Draw the specified text with the top-left corner at (x, y), using the specified color
        ''' and font.
        ''' </summary>
        ''' <param name="gw">The graphics window object</param>
        ''' <param name="text">The text to draw</param>
        ''' <param name="x">The x-coordinate of the top-left corner of the text</param>
        ''' <param name="y">The y-coordinate of the top-left corner of the text</param>
        ''' <param name="color">The color of the text</param>
        ''' <param name="font">The font of the text</param>
        <Extension()>
        Sub DrawText(gw As GraphicsWindow, text As String, x As Integer, y As Integer,
                     color As Color, font As Font)
            TextRenderer.DrawText(gw.CreateGraphics, text, font, New Point(x, y), color)
            gw.Invalidate()
        End Sub

        ''' <summary>
        ''' Draw the specified text with the top-left corner at (x, y), using the specified color,
        ''' and with the font defined by its name, size and style.
        ''' </summary>
        ''' <param name="gw">The graphics window object</param>
        ''' <param name="text">The text to draw</param>
        ''' <param name="x">The x-coordinate of the top-left corner of the text</param>
        ''' <param name="y">The y-coordinate of the top-left corner of the text</param>
        ''' <param name="color">The color of the text</param>
        ''' <param name="fontName">The name of the font</param>
        ''' <param name="emSize">The em-size of the font, in points. 1 point = 4/3 pixels.</param>
        ''' <param name="fontStyle">The font style</param>
        <Extension()>
        Sub DrawText(gw As GraphicsWindow, text As String, x As Integer, y As Integer,
                     color As Color, fontName As String, emSize As Single,
                     Optional fontStyle As FontStyle = FontStyle.Regular)
            Dim font As New Font(fontName, emSize, fontStyle)
            TextRenderer.DrawText(gw.CreateGraphics, text, font, New Point(x, y), color)
            gw.Invalidate()
        End Sub

        ''' <summary>
        ''' Draw the specified text with a defined bounding rectangle, using the specified color and
        ''' font. The bounding rectangle is defined by the coordinates of the top-left corner and
        ''' also its width and height.
        ''' </summary>
        ''' <param name="gw">The graphics window object</param>
        ''' <param name="text">The text to draw</param>
        ''' <param name="x">The x-coordinate of the top-left corner of the bounding rectangle
        ''' </param>
        ''' <param name="y">The y-coordinate of the top-left corner of the bounding rectangle
        ''' </param>
        ''' <param name="width">The width of the bounding rectangle</param>
        ''' <param name="height">The height of the bounding rectangle</param>
        ''' <param name="color">The color of the text</param>
        ''' <param name="font">The font of the text</param>
        ''' <param name="flags">A flag that controls the paragraph formatting of the text</param>
        <Extension()>
        Sub DrawTextInRectangle(gw As GraphicsWindow, text As String, x As Integer, y As Integer,
                                width As Integer, height As Integer, color As Color, font As Font,
                                Optional flags As TextFormatFlags = TextFormatFlags.Default)
            Using g As Graphics = gw.CreateGraphics
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit
                TextRenderer.DrawText(g, text, font, New Rectangle(x, y, width, height),
                                      color, flags)
            End Using
            gw.Invalidate()
        End Sub

        ''' <summary>
        ''' Draw the specified text with a defined bounding rectangle, using the specified color, 
        ''' and with the font defined by its name, size and style. The bounding rectangle is defined
        ''' by the coordinates of the top-left corner and also its width and height.
        ''' </summary>
        ''' <param name="gw">The graphics window object</param>
        ''' <param name="text">The text to draw</param>
        ''' <param name="x">The x-coordinate of the top-left corner of the bounding rectangle
        ''' </param>
        ''' <param name="y">The y-coordinate of the top-left corner of the bounding rectangle
        ''' </param>
        ''' <param name="width">The width of the bounding rectangle</param>
        ''' <param name="height">The height of the bounding rectangle</param>
        ''' <param name="color">The color of the text</param>
        ''' <param name="fontName">The name of the font</param>
        ''' <param name="emSize">The em-size of the font, in points. 1 point = 4/3 pixels.</param>
        ''' <param name="fontStyle">The font style</param>
        ''' <param name="flags">A flag that controls the paragraph formatting of the text</param>
        <Extension()>
        Sub DrawTextInRectangle(gw As GraphicsWindow, text As String, x As Integer, y As Integer,
                                width As Integer, height As Integer,
                                color As Color, fontName As String, emSize As Single,
                                Optional fontStyle As FontStyle = FontStyle.Regular,
                                Optional flags As TextFormatFlags = TextFormatFlags.Default)
            Dim font As New Font(fontName, emSize, fontStyle)
            DrawTextInRectangle(gw, text, x, y, width, height, color, font, flags)
        End Sub

        ''' <summary>
        ''' Display a text box, and ask the user to input a line of text. The text box has its
        ''' top-left corner at (x, y), with the specified width, background color and foreground
        ''' color. The text box can optionally change its color after the user has finished
        ''' inputting.
        ''' </summary>
        ''' <param name="gw">The graphics window object</param>
        ''' <param name="x">The x-coordinate of the top-left corner of the text box</param>
        ''' <param name="y">The y-coordinate of the top-left corner of the text box</param>
        ''' <param name="width">The width of the text box</param>
        ''' <param name="foreColor">The foreground color of the text box</param>
        ''' <param name="backColor">The background color of the text box</param>
        ''' <param name="fontName">The name of the font</param>
        ''' <param name="emSize">The em-size of the font, in points. 1 point = 4/3 pixels.</param>
        ''' <param name="fontStyle">The font style</param>
        ''' <param name="foreColorAfterFinish">The foreground color of the text box after the user
        ''' has finished inputting</param>
        ''' <param name="backColorAfterFinish">The background color of the text box after the user
        ''' has finished inputting</param>
        ''' <param name="flags">A flag that controls the paragraph formatting of the text</param>
        ''' <returns>The user inputt</returns>
        <Extension()>
        Function ReadLine(gw As GraphicsWindow, x As Integer, y As Integer, width As Integer,
                          foreColor As Color, backColor As Color,
                          fontName As String, emSize As Single,
                          Optional fontStyle As FontStyle = FontStyle.Regular,
                          Optional foreColorAfterFinish As Color? = Nothing,
                          Optional backColorAfterFinish As Color? = Nothing,
                          Optional flags As TextFormatFlags = TextFormatFlags.Left) As String
            Dim font = New Font(fontName, emSize, fontStyle)
            Dim textBox = New CustomTextBox() With {
                .Location = New Point(-100000, -100000),  ' HACK: offscreen trick
                .width = width,
                .foreColor = foreColor,
                .backColor = backColor,
                .BorderStyle = BorderStyle.None,
                .font = font
            }

            gw.Form.Controls.Add(textBox)
            textBox.Focus()
            textBox.ProcessKeyPressBuffer(gw)

            Do
                DrawTextBox(gw, x, y, textBox, foreColor, backColor, font, flags)
                Application.DoEvents()
            Loop Until textBox.IsInputFinished

            If foreColorAfterFinish.HasValue OrElse backColorAfterFinish.HasValue Then
                DrawTextBox(gw, x, y, textBox,
                    If(foreColorAfterFinish.HasValue, foreColorAfterFinish.Value, foreColor),
                    If(backColorAfterFinish.HasValue, backColorAfterFinish.Value, backColor),
                    font, flags)
            End If

            Dim ret = textBox.Text
            gw.Form.Controls.Remove(textBox)
            textBox.Dispose()

            Return ret
        End Function

        Private Sub DrawTextBox(gw As GraphicsWindow, x As Integer, y As Integer,
                                textBox As TextBox, foreColor As Color, backColor As Color,
                                font As Font, flags As TextFormatFlags)
            Dim graphics As Graphics = gw.CreateGraphics()
            Dim rect = New Rectangle(x, y, textBox.Width, textBox.Height)
            graphics.FillRectangle(New SolidBrush(backColor), rect)
            TextRenderer.DrawText(graphics, textBox.Text, font, rect, foreColor, flags)
            gw.Invalidate()
        End Sub

        Private Class CustomTextBox
            Inherits TextBox

            Public Property IsInputFinished As Boolean = False

            Private Sub CustomTextBox_KeyPress(sender As Object, e As KeyPressEventArgs) _
                Handles Me.KeyPress
                If e.KeyChar = Microsoft.VisualBasic.ControlChars.Cr Then
                    IsInputFinished = True
                    e.Handled = True
                End If
            End Sub

            Public Sub ProcessKeyPressBuffer(gw As GraphicsWindow)
                Do While Not IsInputFinished And gw.KeyAvailable()
                    Dim ch As Char = gw.ReadKeyChar()
                    Select Case ch
                        Case Microsoft.VisualBasic.ControlChars.NullChar
                            Continue Do
                        Case Microsoft.VisualBasic.ControlChars.Cr
                            IsInputFinished = True
                        Case Microsoft.VisualBasic.ControlChars.Back
                            Text = Text.Remove(Text.Length - 1, 1)
                        Case Else
                            AppendText(ch)
                    End Select
                Loop
            End Sub
        End Class
    End Module

End Namespace