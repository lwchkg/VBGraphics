# VBGraphics

VBGraphics is a QBasic style graphics and keyboard routines for .NET framework.

VBGraphics provides simple graphics and keyboard handling routines that wraps
around WinForms and GDI+. With these routines, beginning programmers can create
simple games in the QBasic way.

Example project: [Nibbles for VBGraphics](https://github.com/lwchkg/Nibbles_VBGraphics)

See the [documentation](https://lwchkg.github.io/VBGraphics/) for API
documentation and more tutorials.

## Features

### Simple and yet powerful

* Graphics windows can be resized or maximized, with the content automatically
  zoomed to fill the whole window.
* Works correctly on high DPI settings.
* Events are raised when the user try to close the graphics window, when Windows
  shut down, and when the GraphicsWindow object is disposed. User closing of the
  graphics window can be cancelled programatically.

### Extensible

* You can add your favourite polygon routine, Beizer curve routines, or
  whatever, as [extension methods](https://docs.microsoft.com/en-us/dotnet/visual-basic/programming-guide/language-features/procedures/extension-methods),
  without modifying VBGraphics source code.
* The underlying form (wrapped around WinForms) and bitmap of a graphics window
  can be accessed and manipulated.

### Routines available

* Lines, rectangles and ellipses
* Text rendering
* Bitmap image handling
* Interactive text input
* Keyboard routines

## Installation

VBGraphics is available as a NuGet module. Beginners should following the
following steps (or the [video](https://www.youtube.com/watch?v=InU7Qk8RJTc&feature=youtu.be))
for installation.

1. Create a new Visual Basic Console App.
2. Install the extension VBGraphics.
   1. In the Solution Explorer, right click your project (e.g. ConsoleApp1),
      then select “Manage NuGet Packages…”. A new tab will appear.
   2. Click “Browser” in the top-left corner of the tab.
   3. Type “VBGraphics” in the text box below. Press Enter.
   4. Click on the package.
   5. Click “Install”. Make sure you have selected the latest version.
3. Add references System.Drawing and System.Windows.Forms.
   1. In the Solution Explorer, right click your project (e.g. ConsoleApp1),
      then select “Properties”. A new tab will appear.
   2. Click “References”.
   3. Click “Add…”.
   4. Click “Assemblies”. Find the items “System.Drawing” and
      “System.Windows.Forms”. Check both of them, and press “OK”.
   5. In the imported namespace section, check both “System.Drawing” and
      “System.Windows.Forms”.
4. Add some sample code (see below).
5. Change the application type to Windows Form Application.
   1. In the Solution Explorer, right click your project (e.g. ConsoleApp1),
      then select “Properties”. A new tab will appear.
   2. Click “Application”.
   3. Change application type to “Windows Form Application”.

## Sample code

```vbnet
Imports VBGraphics
Imports VBGraphics.BitmapImage
Imports VBGraphics.Shapes
Imports VBGraphics.Text

Module Module1
    Sub Main()
        ' Create a graphics windows with resolution 600 x 400.
        Dim gw As New GraphicsWindow(600, 400)

        ' Draw something in the graphics window...
        gw.DrawLine(0, 0, 599, 399, Color.Red)
        gw.DrawText("Press any key to close...", 0, 370, Color.White, "Cambria", 16)

        ' Make the graphics window closable, and end the program when the window is closed.
        gw.EndProgramOnClose = True
        gw.CanClose = True

        ' Reads a key in the keyboard. Note: if the user closes the window, this statement will not
        ' finish execution.
        gw.ReadKey()

        ' It is a good habit to dispose the graphics window after use.
        gw.Dispose()
    End Sub
End Module
```

## To-dos

* Update documentation site.
* Add tutorials in the documentation site for common tasks.
* Add template project for easier installation.
* Add routines for mouse and touch screen.
* Add more graphics routines. (e.g. Polygons? DrawText overloads?)
