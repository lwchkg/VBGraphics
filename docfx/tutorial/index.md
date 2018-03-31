<style type="text/css">
    ol > li > ol { list-style-type: lower-alpha; }
</style>

# Installing VBGraphics

<iframe width="656" height="420" src="https://www.youtube.com/embed/InU7Qk8RJTc?rel=0&amp;cc_load_policy=1" frameborder="0" allow="autoplay; encrypted-media" allowfullscreen></iframe>

VBGraphics is available as a NuGet module. Beginners should follow the
following steps (or the video) for installation. If you are using C# instead of
Visual Basic, check the bottom of the page for variations of the instructions.

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

   4. Find the items “System.Drawing” and “System.Windows.Forms”. Check both of
      them, and press “OK”.

   5. In the imported namespace section, check both “System.Drawing” and
      “System.Windows.Forms”.

4. Add some sample code (see below).

5. Change the application type to Windows Form Application.

   1. In the Solution Explorer, right click your project (e.g. ConsoleApp1),
      then select “Properties”. A new tab will appear.

   2. Click “Application”.

   3. Change application type to “Windows Form Application”.

## Sample code

# [VB](#tab/sample1-vb)

```vbnet
Imports VBGraphics

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

# [C#](#tab/sample1-cs)

```csharp
// Added by Visual Studio. Do not touch unless you know what you are doing.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Required by VBGraphics.
using System.Drawing;
using System.Windows.Forms;
using VBGraphics;

namespace vbgraphics_cs_test
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a graphics windows with resolution 600 x 400.
            GraphicsWindow gw = new GraphicsWindow(600, 400);

            // Draw something in the graphics window...
            gw.DrawLine(0, 0, 599, 399, Color.Red);
            gw.DrawText("Press any key to close...", 0, 370, Color.White, "Cambria", 16);

            // Make the graphics window closable, and end the program when the window is closed.
            gw.EndProgramOnClose = true;
            gw.CanClose = true;

            // Reads a key in the keyboard. Note: if the user closes the window, this statement will
            // not finish execution.
            gw.ReadKey();

            // It is a good habit to dispose the graphics window after use.
            gw.Dispose();
        }
    }
}
```

***

## Installation instructions for C#

Installation instructions of VBGraphics in C# are almost the same as those for
Visual Basic. Here is the list of difference:

1. In step (1), you create a “Visual C# Console App” instead of a "Visual Basic
   Console App”.

1. Steps (3) is replaced by the following:

   1. In the Solution Explorer, right click your project (e.g. ConsoleApp1),
      then select “Add” → “References…”. A dialog box will appear.

   2. Find the items “System.Drawing” and “System.Windows.Forms”. Check both of
      them, and press “OK”.

1. For step (5), replace “Windows Form Application” by “Windows Application”.
