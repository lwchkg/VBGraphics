$msbuild = "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe"

&$msbuild ..\VBGraphics\VBGraphics.vbproj /property:configuration=Release /property:OutputPath=$PSScriptRoot\VBGraphics\lib\Net452\
nuget pack VBGraphics.nuspec