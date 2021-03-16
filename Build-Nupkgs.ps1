
function Build-OnlyNupkg([string]$nuspecPath){
    $curDirObj = Get-Location
    $curDir = $curDirObj.Path
	$nuget = "$curDirObj\Nuget\Nuget.exe"
    #if($Global:BuildOnlyDiffs){
    #    $fullNpath = Resolve-path $nuspecPath;
    #    
    #    if(-not (Sln-NeedsBuild $fullNpath)){
    #        return;
    #    }
    #}


    Write-Debug "---Building Nuget package from nuspec: $nuspecPath"

	$buildNupkg = '& "'+ $nuget + '" pack "' + $nuspecPath + '" -NoDefaultExcludes -NoPackageAnalysis -NonInteractive '

	$res = Invoke-Expression $buildNupkg
    if($res -ne $null){
        $res | foreach {
            Write-Debug $_
        }
     }
}

#. ..\dev\Tooling\buildtools.ps1
$nuspec = "TS.CodeGenerator.nuspec"
Build-OnlyNupkg -nuspecPath $nuspec
