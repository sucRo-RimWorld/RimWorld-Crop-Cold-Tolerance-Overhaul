param(
    [string]$ModsConfigPath = "$env:USERPROFILE\AppData\LocalLow\Ludeon Studios\RimWorld by Ludeon Studios\Config\ModsConfig.xml"
)

$ErrorActionPreference = "Stop"

$required = @(
    "brrainz.harmony",
    "ludeon.rimworld",
    "DankPyon.Medieval.Overhaul",
    "sucro.cropcoldtoleranceoverhaul"
)

$developmentOnly = @(
    "sucro.cropcoldtoleranceoverhaul.e2e",
    "sucro.cropcoldtoleranceoverhaul.mofixture",
    "sucro.cropcoldtoleranceoverhaul.tests",
    "rimworks.pickle",
    "rimworks.quickstarts",
    "ilyvion.rimtestredux"
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

$enabledDev = @(
    $developmentOnly |
        Where-Object { $active -contains $_.ToLowerInvariant() }
)

if ($missing.Count -gt 0) {
    Write-Host "[ACTION REQUIRED] Enable these mods for the full normal-game CCTO smoke test:" -ForegroundColor Yellow
    foreach ($packageId in $missing) {
        Write-Host "  - $packageId"
    }
}

if ($enabledDev.Count -gt 0) {
    Write-Host "[ACTION REQUIRED] Disable these development/test helpers for the normal-game smoke test:" -ForegroundColor Yellow
    foreach ($packageId in $enabledDev) {
        Write-Host "  - $packageId"
    }
}

if ($missing.Count -gt 0 -or $enabledDev.Count -gt 0) {
    Write-Host ""
    Write-Host "The script does not modify your normal RimWorld mod list."
    Write-Host "Change the active list in RimSort/RimWorld, save it, then rerun prepare-smoke.bat."
    exit 1
}

Write-Host "[OK] Normal smoke-test profile has CCTO + Medieval Overhaul active and CCTO test helpers disabled." -ForegroundColor Green
exit 0
