#make sure to add
#    powershell.exe -File $(ProjectDir)Generate_dts.ps1
# to your pono dll's post build step

#script working directory
$pwd = split-path -parent $MyInvocation.MyCommand.Definition
$rootDir = (get-item $pwd )

#parameters
$inputDLL = "lib.test.a.b.dll"
$outFileName = "lib.test.a.b.d.ts"


[Environment]::CurrentDirectory = $pwd
$dirDll = ([System.IO.Path]::Combine($pwd, "bin\debug"))
if (![System.IO.Directory]::Exists($dirDll)) {
    $dirDll = [System.IO.Path]::Combine($pwd, "..\..\bin\debug")
    if (![System.IO.Directory]::Exists($dirDll)) {
        throw "Cannot find $dirDll"
    }
}
$dirDll = [System.IO.Path]::Combine($dirDll, $inputDLL)

$filePath = ([System.IO.Path]::Combine($pwd, $outFileName))
$cg = [System.IO.Path]::Combine($pwd, "..\packages\TS.CodeGenerator.1.0.0.17\tools\TS.CodeGenerator.dll");
$libPath = ([System.IO.Path]::GetFullPath($cg));


Write-Host ""
Write-Host "Loading Generator " $libPath
Write-Host ""
[Reflection.Assembly]::LoadFile($libPath)

Write-Host ""
Write-Host "Creating d.ts file from assembly: " $dirDll
Write-Host ""
$assemblyReader = new-object -Typename TS.CodeGenerator.AssemblyReader -ArgumentList $dirDll
#or do this
#$asm= [Reflection.Assembly]::LoadFile($dirDll)
#$assemblyReader = new-object -Typename TS.CodeGenerator.NamespaceAssemblyReader -ArgumentList $asm

#set parameters here
$outStream = $assemblyReader.GenerateTypingsStream()

If (Test-Path $filePath){
	Remove-Item $filePath
}

$fs = New-Object IO.FileStream $filePath ,'Append' 
$sw = New-Object IO.StreamWriter -ArgumentList  $fs
#add addtional lines to script
$sw.WriteLine('/// <reference path="../jquery/jquery.d.ts" />');
$sw.Flush();
$outStream.WriteTo($fs);
$fs.Flush();
$fs.Close();
$outStream.Close();


#Copy to as many places as you like
$outPath = $rootDir.parent.FullName + "\" + $outFileName ;
Write-Host ""
Write-Host "Copying generated dts: " + $outPath 
Write-Host ""
copy-item  -Force $filePath $outPath

write-host "Created DTS"
write-host (gc $filePath)
write-host "cleaning up"

#cleanup
If (Test-Path $filePath){
	Remove-Item $filePath
}

exit $lastexitcode