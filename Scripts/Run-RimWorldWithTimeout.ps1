param(
    [Parameter(Mandatory = $true)]
    [string]$ExePath,

    [Parameter(Mandatory = $true)]
    [string]$SavedataFolder,

    [Parameter(Mandatory = $true)]
    [string]$ReportDir,

    [Parameter(Mandatory = $true)]
    [string]$LogPath,

    [string]$RunFilter = "framework.feature,cold-tolerance.feature",

    [int]$TimeoutSeconds = 300
)

$ErrorActionPreference = "Stop"
. (Join-Path $PSScriptRoot 'RimWorldRuntime.ps1')

if (-not (Test-Path -LiteralPath $ExePath)) {
    Write-Host "[ERROR] RimWorld executable was not found:" -ForegroundColor Red
    Write-Host "        $ExePath"
    exit 2
}

$logDir = Split-Path -Parent $LogPath
if (-not (Test-Path -LiteralPath $logDir)) {
    New-Item -ItemType Directory -Force -Path $logDir | Out-Null
}
if (Test-Path -LiteralPath $LogPath) {
    Remove-Item -LiteralPath $LogPath -Force
}

$arguments = @(
    '-savedatafolder="' + $SavedataFolder + '"',
    '-logFile "' + $LogPath + '"',
    '-pickle-run="' + $RunFilter + '"',
    '-pickle-mode=fast',
    '-pickle-report-dir="' + $ReportDir + '"',
    '-pickle-no-browser',
    '-pickle-no-http',
    '-pickle-run-timeout=4'
) -join ' '

try {
    Invoke-RimWorldIsolated (Split-Path -Parent $ExePath) $arguments $LogPath $TimeoutSeconds
    exit 0
}
catch [TimeoutException] {
    Write-Output ('[ERROR] '+$_.Exception.Message)
    exit 124
}
catch {
    Write-Output ('[ERROR] '+$_.Exception.Message)
    exit 2
}
