Set-StrictMode -Version '2.0'

Function AddEnvVariable
{
    param(
        [ValidateNotNullOrEmpty()]
        [string] $name,

        [ValidateNotNullOrEmpty()]
        [string] $value
    )
    Write-Host "Debug: Adding ad-hoc env variable '$name' = '$value'"
    Add-Content -Path $env:GITHUB_ENV -Value "$name=$value"
}



Function GetPipelineCommonConfiguration
{

    $ErrorActionPreference = "Stop"
    $thisScriptPath = $PSScriptRoot
    $projectRootDir = "${thisScriptPath}/../../" | Resolve-Path
    $projectSolutionDir = "${thisScriptPath}/../../AzureServiceBusMessageTool" |  Resolve-Path
    $solutionFileName = "AzureServiceBusMessageTool.sln";
    $solutionFilePath = "${ProjectSolutionDir}/${solutionFileName}" ;
    
    If( ! (Test-path -PathType Container -Path $projectSolutionDir) )
    {
        throw "solution directory path '$ProjectSolutionDir' is invalid"
    }

    If( ! (Test-path -PathType Leaf -Path $solutionFilePath) )
    {
        throw "solution file path '$solutionFilePath' is invalid"
    }

    return [PSCustomObject]@{       
        ProjectSolutionDirectory = $projectSolutionDir
        ProjectSolutionFileName = $solutionFileName
        ProjectSolutionFilePath = $solutionFilePath     
    }
}

Function Install-DotnetToolIfNotInstalled
{
    param(
        [ValidateNotNullOrEmpty()]
        [string] $dotnetToolName
    )

    $ErrorActionPreference = "Stop"

    Function IsToolInstalled
    {
        param(
            [ValidateNotNullOrEmpty()]
            [string] $dotnetToolName
        )

        $ErrorActionPreference = "Stop"

        $regexPattern = "\b${dotnetToolName}\b"
        $installedToolsString = dotnet tool list -g | out-string # with 'out-string' make output a single string instead of array of strings
        $matchResult = [Regex]::Match($installedToolsString, $regexPattern)
        If($matchResult.Success)
        {
            return $true
        }
        return $false
    }

    Write-Host "Installing dotnet tool '$dotnetToolName' if not installed already"
    If(IsToolInstalled $dotnetToolName)
    {
        write-Host "Ok: '$dotnetToolName' is already installed"
        return
    }

    Write-Host "Tool '$dotnetToolName' not found in system, installing ..."
    dotnet tool install -g $dotnetToolName

    # verify that now tool is installed
    If(IsToolInstalled $dotnetToolName)
    {
        write-Host "'$dotnetToolName' was installed successfully"
        return
    }

    Write-Error "Dotnet tool '$dotnetToolName' could not be installed"
}

Function Write-PipelineWarning
{
    param(
        [ValidateNotNullOrEmpty()]
        [object] $objectToDisplay,

        [int] $skipFrameCount = 0
    )

    $stack = Get-PSCallStack
    $upperFrame=$stack[1 + $skipFrameCount]
    $line=$upperFrame.ScriptLineNumber
    $funcName=$upperFrame.FunctionName
    $scriptName=$upperFrame.ScriptName
    If($scriptName)
    {
        $scriptName = Split-Path -Path ${scriptName} -Leaf -ErrorAction Ignore
    }
    else
    {
        $scriptName = "<console>"
    }

    Write-Host "::warning file=${scriptName},line=${line}::${scriptName}:${line}, ${funcName}():  ${objectToDisplay}"
}

