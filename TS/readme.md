This will build a d.ts file from a .net assembly.

Now In Nuget Form:
[https://www.nuget.org/packages/TS.CodeGenerator]

example post build step ps1:

execute: powershell -file "$(SolutionDir)\MY.Contracts\build_dts.ps1"

build_dts.psl:
```powershell
$pwd = split-path -parent $MyInvocation.MyCommand.Definition

[Environment]::CurrentDirectory = $pwd

$dirDll = ([System.IO.Path]::Combine($pwd, "bin\debug"))

if (![System.IO.Directory]::Exists($dirDll)) {
    $dirDll = [System.IO.Path]::Combine($pwd, "..\..\bin\MY.contracts")

    if (![System.IO.Directory]::Exists($dirDll)) {
        throw "Cannot find $dirDll"
    }
}
$dirDll = [System.IO.Path]::Combine($dirDll, "MY.Contracts.dll")

Write-Host $dirDll

$outFileName = "MY.Contracts.d.ts"
$filePath = ([System.IO.Path]::Combine($pwd, $outFileName))



Write-Host "Creating d.ts file from assembly: "+$dirDll

[Reflection.Assembly]::LoadFile("TS.CodeGenerator.dll")
$assemblyReader = new-object -Typename TS.CodeGenerator.AssemblyReader -ArgumentList $dirDll

$outStream = $assemblyReader.GenerateTypingsStream()

If (Test-Path $filePath){
	Remove-Item $filePath
}

$fs = New-Object IO.FileStream $filePath ,'Append' 
$sw = New-Object IO.StreamWriter -ArgumentList  $fs
$sw.WriteLine('/// <reference path="../jquery/jquery.d.ts" />');
$sw.Flush();
$outStream.WriteTo($fs);
$fs.Flush();
$fs.Close();
$outStream.Close();

#copy it where you want it
$outPath = (get-item $pwd ).parent.FullName + "\MY.Web\Scripts\typings\MY\" + $outFileName ;
Write-Host "Copying generated dts: " + $outPath 
copy-item  -Force $filePath $outPath



exit $lastexitcode

```
