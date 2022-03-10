
$thisDir = $PSScriptRoot;
$solutionFilePath = "$thisDir/../AzureServiceBusMessageTool/AzureServiceBusMessageTool.sln" | Resolve-Path


$global:ErrorActionPreference = "stop"
if(!(Test-Path $solutionFilePath ))
{
    throw "Invalid path to solution file '${solutionFilePath}'"
}

$publishedDir = "${thisDir}\..\temp-published-artifacts"
Write-Host "Published dir '${publishedDir}'"
if(Test-Path $publishedDir)
{

    $publishedDir = $publishedDir | Resolve-Path
    Write-Host "Removing (stale) artefacts in dir '${publishedDir}'"
    Remove-Item -Path $publishedDir -Recurse -Force
}
else{
    Write-Host "Published dir '${publishedDir}' does not exist"
}

$null = New-Item -ItemType Directory $publishedDir
$publishedDir = $publishedDir | Resolve-Path

Write-Host "Building and publishing code to directory '$publishedDir'";
dotnet publish `
    -o $publishedDir `
    --nologo `
    --verbosity quiet `
    --self-contained `
    --runtime win-x64 `
    -p:PublishSingleFile=true `
    $solutionFilePath

Write-Host "Finished publishing code to directory '$publishedDir'";
# find Main.exe file and get file vesion

$mainExeFile = @(Get-ChildItem $publishedDir | Where-Object {$_.Name -eq "Main.exe"})
if($mainExeFile.Length -ne 1)
{
    throw "Should find one 'Main.exe' file but found $($mainExeFile.Length)"
}
$mainExeFileVersion = (Get-Command $mainExeFile.FullName).FileVersionInfo.FileVersion
Write-Host "Version is '${mainExeFileVersion}'"
$destinationFilePath = "${thisDir}\..\ServiceBusMessageTool_${mainExeFileVersion}.zip"
if(Test-Path $destinationFilePath)
{
    Remove-Item $destinationFilePath -Force
}
Write-Host "Compressing artifact '$destinationFilePath'..."
Compress-Archive -CompressionLevel Optimal -Path "${publishedDir}/*" -DestinationPath $destinationFilePath

# remove old published dir
Remove-Item -Path $publishedDir -Recurse -Force
Write-Host "All done!"