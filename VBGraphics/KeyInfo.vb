Option Strict On

Imports System.Windows.Forms

Namespace Global.VBGraphics
    Public Structure KeyInfo
        Dim Key As KeyEventArgs
        Dim KeyChar As Char
        Sub New(key As KeyEventArgs, keyChar As Char)
            Me.Key = key
            Me.KeyChar = keyChar
        End Sub
    End Structure
End Namespace