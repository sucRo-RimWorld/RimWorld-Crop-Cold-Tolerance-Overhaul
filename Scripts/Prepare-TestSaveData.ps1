param(
    [string]$OutputRoot,
    [string]$SourceModsConfigPath = "$env:USERPROFILE\AppData\LocalLow\Ludeon Studios\RimWorld by Ludeon Studios\Config\ModsConfig.xml"
)

$ErrorActionPreference = "Stop"

if ([string]::IsNullOrWhiteSpace($OutputRoot)) {
    Write-Host "[ERROR] OutputRoot is required." -ForegroundColor Red
    exit 2
}

if (-not (Test-Path -LiteralPath $SourceModsConfigPath)) {
    Write-Host "[ERROR] RimWorld ModsConfig.xml was not found:" -ForegroundColor Red
    Write-Host "        $SourceModsConfigPath"
    exit 2
}

try {
    [xml]$source = Get-Content -LiteralPath $SourceModsConfigPath -Raw
}
catch {
    Write-Host "[ERROR] Failed to parse the normal RimWorld ModsConfig.xml:" -ForegroundColor Red
    Write-Host "        $($_.Exception.Message)"
    exit 2
}

$required = @(
    "brrainz.harmony",
    "ludeon.rimworld",
    "rimworks.rimlogging",
    "rimworks.pickle",
    "rimworks.quickstarts",
    "sucro.cropcoldtoleranceoverhaul",
    "sucro.cropcoldtoleranceoverhaul.e2e"
)

$configDir = Join-Path $OutputRoot "Config"
$configPath = Join-Path $configDir "ModsConfig.xml"
$sourceConfigDir = Split-Path -Parent $SourceModsConfigPath
$sourcePrefsPath = Join-Path $sourceConfigDir "Prefs.xml"
$testPrefsPath = Join-Path $configDir "Prefs.xml"

New-Item -ItemType Directory -Force -Path $configDir | Out-Null

$doc = New-Object System.Xml.XmlDocument
$declaration = $doc.CreateXmlDeclaration("1.0", "utf-8", $null)
$null = $doc.AppendChild($declaration)

$root = $doc.CreateElement("ModsConfigData")
$null = $doc.AppendChild($root)

$version = $doc.CreateElement("version")
$version.InnerText = [string]$source.ModsConfigData.version
$null = $root.AppendChild($version)

$activeMods = $doc.CreateElement("activeMods")
foreach ($packageId in $required) {
    $li = $doc.CreateElement("li")
    $li.InnerText = $packageId
    $null = $activeMods.AppendChild($li)
}
$null = $root.AppendChild($activeMods)

$knownExpansions = $doc.CreateElement("knownExpansions")
foreach ($expansion in @($source.ModsConfigData.knownExpansions.li)) {
    $value = ([string]$expansion).Trim()
    if ([string]::IsNullOrWhiteSpace($value)) {
        continue
    }

    $li = $doc.CreateElement("li")
    $li.InnerText = $value
    $null = $knownExpansions.AppendChild($li)
}
$null = $root.AppendChild($knownExpansions)

$settings = New-Object System.Xml.XmlWriterSettings
$settings.Indent = $true
$settings.Encoding = New-Object System.Text.UTF8Encoding($false)

$writer = [System.Xml.XmlWriter]::Create($configPath, $settings)
try {
    $doc.Save($writer)
}
finally {
    $writer.Dispose()
}


if (-not (Test-Path -LiteralPath $sourcePrefsPath)) {
    Write-Host "[ERROR] RimWorld Prefs.xml was not found:" -ForegroundColor Red
    Write-Host "        $sourcePrefsPath"
    exit 2
}

try {
    [xml]$prefs = Get-Content -LiteralPath $sourcePrefsPath -Raw
}
catch {
    Write-Host "[ERROR] Failed to parse RimWorld Prefs.xml:" -ForegroundColor Red
    Write-Host "        $($_.Exception.Message)"
    exit 2
}

$devModeNode = $prefs.SelectSingleNode("//devMode")
if ($null -eq $devModeNode) {
    $devModeNode = $prefs.CreateElement("devMode")
    $devModeNode.InnerText = "True"
    $null = $prefs.DocumentElement.AppendChild($devModeNode)
}
else {
    $devModeNode.InnerText = "True"
}

$writer = [System.Xml.XmlWriter]::Create($testPrefsPath, $settings)
try {
    $prefs.Save($writer)
}
finally {
    $writer.Dispose()
}

Write-Host "[OK] Prepared isolated CCTO test save-data profile."
Write-Host "     $OutputRoot"
Write-Host "[OK] Dev mode enabled in isolated test Prefs.xml."
Write-Host "     $testPrefsPath"
Write-Host ""
Write-Host "Active test mods:"
foreach ($packageId in $required) {
    Write-Host "  - $packageId"
}

exit 0
