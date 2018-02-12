Option Strict On

Imports System.Windows.Forms

Namespace Global.VBGraphics
    Public Structure KeyInfo
        ReadOnly Property Key As KeyEventArgs
        ReadOnly Property KeyChar As Char
        Sub New(key As KeyEventArgs, keyChar As Char)
            Me.Key = key
            Me.KeyChar = keyChar
        End Sub
    End Structure
End Namespace