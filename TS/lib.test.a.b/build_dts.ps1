# make sure to add
#    powershell.exe -File $(ProjectDir)Generate_dts.ps1
# to your pono dll's post build step

cd "..\TS.CodeGenerator.Console\bin\Release\PublishOutput"
# script working directory
#$pwd = split-path -parent $MyInvocation.MyCommand.Definition
#$rootDir = (get-item $pwd )

[Environment]::CurrentDirectory = (pwd)

$outFileName = [System.IO.Path]::GetFullPath("..\..\..\..\lib.test.a.b\bin\Debug\netstandard1.5\lib.test.a.b.d.ts")

$inputDll = [System.IO.Path]::GetFullPath("..\..\..\..\lib.test.a.b\bin\Debug\netstandard1.5\lib.test.a.b.dll")

dotnet TS.CodeGenerator.Console.dll $inputDll $outFileName


cd "..\..\..\..\TS.CodeGenerator.Core\bin\Debug\net462\"


[Environment]::CurrentDirectory = (pwd)
$libPath = [System.IO.Path]::GetFullPath("TS.CodeGenerator.dll")

Write-Host ""
Write-Host "Loading Generator " $libPath
Write-Host ""
[Reflection.Assembly]::LoadFile($libPath)

#example settings
[TS.CodeGenerator.Settings]::MethodReturnTypeFormatString = "Promise<{0}>"
[TS.CodeGenerator.Settings]::ConstEnumsEnabled = $true;

Write-output ""
Write-output `Creating d.ts file from assembly: ` $inputDll
Write-output ""
$assemblyReader = new-object -Typename TS.CodeGenerator.AssemblyReader -ArgumentList $inputDll
#or do this
#$asm= [Reflection.Assembly]::LoadFile($dirDll)
#$assemblyReader = new-object -Typename TS.CodeGenerator.NamespaceAssemblyReader -ArgumentList $asm

#set parameters here
$outStream = $assemblyReader.GenerateTypingsStream()

If (Test-Path $outFileName){
	Remove-Item $outFileName
}

$fs = New-Object IO.FileStream $outFileName ,'Append' 
$sw = New-Object IO.StreamWriter -ArgumentList  $fs
#add addtional lines to script
$sw.WriteLine('/// <reference path="../jquery/jquery.d.ts" />');
$sw.Flush();
$outStream.WriteTo($fs);
$fs.Flush();
$fs.Close();
$outStream.Close();



exit $lastexitcode
