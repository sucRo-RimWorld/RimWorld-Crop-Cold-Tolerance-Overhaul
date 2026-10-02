param(
    [string]$ModsConfigPath = "$env:USERPROFILE\AppData\LocalLow\Ludeon Studios\RimWorld by Ludeon Studios\Config\ModsConfig.xml"
)

$required = @(
    "brrainz.harmony",
    "rimworks.rimlogging",
    "rimworks.pickle",
    "rimworks.quickstarts",
    "sucro.cropcoldtoleranceoverhaul",
    "sucro.cropcoldtoleranceoverhaul.e2e"
)

if (-not (Test-Path -LiteralPath $ModsConfigPath)) {
    Write-Host "[ERROR] RimWorld ModsConfig.xml was not found:" -ForegroundColor Red
    Write-Host "        $ModsConfigPath"
    exit 2
}

try {
    [xml]$config = Get-Content -LiteralPath $ModsConfigPath -Raw
}
catch {
    Write-Host "[ERROR] Failed to parse ModsConfig.xml:" -ForegroundColor Red
    Write-Host "        $($_.Exception.Message)"
    exit 2
}

$active = @(
    $config.ModsConfigData.activeMods.li |
        ForEach-Object { ([string]$_).Trim().ToLowerInvariant() }
)

$missing = @(
    $required |
        Where-Object { $active -notcontains $_.ToLowerInvariant() }
)

if ($missing.Count -gt 0) {
    Write-Host "[ACTION REQUIRED] Enable the following mods before running CCTO automated tests:" -ForegroundColor Yellow
    foreach ($packageId in $missing) {
        Write-Host "  - $packageId"
    }

    Write-Host ""
    Write-Host "Use RimSort/RimWorld to enable them, save the active mod list, then rerun run-tests.bat."
    Write-Host "The script does not rewrite your normal gameplay mod list automatically."
    exit 1
}

Write-Host "[OK] Required CCTO automated-test mods are active."
exit 0
