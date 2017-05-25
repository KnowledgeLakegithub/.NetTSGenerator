# make sure to add
#    powershell.exe -File $(ProjectDir)Generate_dts.ps1
# to your pono dll's post build step

cd "..\..\..\..\TS.CodeGenerator.Console\bin\Release\PublishOutput\"
# script working directory
$pwd = split-path -parent $MyInvocation.MyCommand.Definition
$rootDir = (get-item $pwd )

[Environment]::CurrentDirectory = (pwd)

$outFileName = [System.IO.Path]::GetFullPath("..\..\..\..\lib.test.a.b\bin\Debug\netstandard1.5\lib.test.a.b.d.ts")

$inputDll = [System.IO.Path]::GetFullPath("..\..\..\..\lib.test.a.b\bin\Debug\netstandard1.5\lib.test.a.b.dll")

dotnet TS.CodeGenerator.Console.dll $inputDll $outFileName