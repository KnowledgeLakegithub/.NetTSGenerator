param($installPath, $toolsPath, $package, $project)
$projectFullName = $project.FullName
$debugString = "install.ps1 executing for " + $projectFullName
Write-Host $debugString
# 
#$fileInfo = new-object -typename System.IO.FileInfo -ArgumentList $projectFullName
#$projectDirectory = $fileInfo.DirectoryName
#$tempDirectory = "ViceSoftwareTemp"
#$sourceDirectory = "$projectDirectory\$tempDirectory"
#Write-Host $sourceDirectory
# 
#$destinationDirectory = "$projectDirectory\..\ViceSoftware.Data.Bin"
#Write-Host $destinationDirectory
# 
#if(test-path $sourceDirectory -pathtype container)
#{
# Write-Host "Copying files from $sourceDirectory to $destinationDirectory"
# robocopy $sourceDirectory $destinationDirectory /XO
# 
# Write-Host "Removing $tempDirectory from project."
# $tempDirectoryProjectItem = $project.ProjectItems.Item($tempDirectory)
# $tempDirectoryProjectItem.Remove()
# 
# Write-Host "Deleting $sourceDirectory"
# remove-item $sourceDirectory -recurse
#}
# 
#$debugString = "install.ps1 complete" + $projectFullName
#Write-Host $debugString