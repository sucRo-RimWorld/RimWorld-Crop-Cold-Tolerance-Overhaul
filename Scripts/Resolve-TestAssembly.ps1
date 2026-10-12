param(
    [Parameter(Mandatory = $true)][string]$ProviderRoot,
    [Parameter(Mandatory = $true)][string]$DllName,
    [string]$GameVersion = $env:CCTO_TEST_GAME_VERSION
)
$ErrorActionPreference = 'Stop'

if (-not $GameVersion) {
    $normalConfig = Join-Path $env:USERPROFILE 'AppData/LocalLow/Ludeon Studios/RimWorld by Ludeon Studios/Config/ModsConfig.xml'
    if (-not (Test-Path -LiteralPath $normalConfig)) {
        throw "Normal ModsConfig.xml missing; set CCTO_TEST_GAME_VERSION to 1.6 or 1.7"
    }
    [xml]$normal = Get-Content -LiteralPath $normalConfig -Raw
    $match = [regex]::Match([string]$normal.ModsConfigData.version, '^(1\.[67])(?:\.|$)')
    if (-not $match.Success) {
        throw "Unsupported normal RimWorld test config version; expected 1.6 or 1.7"
    }
    $GameVersion = $match.Groups[1].Value
}
if ($GameVersion -notin @('1.6', '1.7')) {
    throw "Unsupported CCTO_TEST_GAME_VERSION: $GameVersion (expected 1.6 or 1.7)"
}
if (-not (Test-Path -LiteralPath $ProviderRoot -PathType Container)) {
    throw "Test provider root missing: $ProviderRoot"
}

$loader = @('LoadFolders.xml', 'loadFolders.xml') |
    ForEach-Object { Join-Path $ProviderRoot $_ } |
    Where-Object { Test-Path -LiteralPath $_ -PathType Leaf } |
    Select-Object -First 1

if ($loader) {
    [xml]$document = Get-Content -LiteralPath $loader -Raw
    $versionNode = $document.SelectSingleNode("/loadFolders/v$GameVersion")
    if ($null -eq $versionNode -and $GameVersion -eq '1.7') {
        $versionNode = $document.SelectSingleNode('/loadFolders/v1.6')
        if ($null -ne $versionNode) {
            [Console]::Error.WriteLine("[WARN] $DllName uses v1.6 load folders on 1.7 unstable; compatibility probe only.")
        }
    }
    if ($null -eq $versionNode) {
        throw "No load-folder declaration for RimWorld $GameVersion in $loader"
    }
    $resolved = $null
    foreach ($li in $versionNode.SelectNodes('li')) {
        if ($li.Attributes.Count -ne 0) { continue }
        $folder = $li.InnerText.Trim()
        if (-not $folder) { throw "Empty load folder in $loader" }
        if ($folder -eq '/') { $folder = '' }
        if ([IO.Path]::IsPathRooted($folder) -or
            ($folder -split '[\\/]') -contains '..') {
            throw "Unsafe test-provider load folder in $loader"
        }
        $candidate = Join-Path $ProviderRoot (Join-Path $folder ("Assemblies/" + $DllName))
        if (Test-Path -LiteralPath $candidate -PathType Leaf) {
            $resolved = (Resolve-Path -LiteralPath $candidate).Path
        }
    }
    if ($resolved) { Write-Output $resolved; exit 0 }
    throw "No active $DllName found in declared RimWorld $GameVersion load folders"
}

foreach ($folder in @($GameVersion, 'Current', '')) {
    $candidate = Join-Path $ProviderRoot (Join-Path $folder ("Assemblies/" + $DllName))
    if (Test-Path -LiteralPath $candidate -PathType Leaf) {
        Write-Output (Resolve-Path -LiteralPath $candidate).Path
        exit 0
    }
}
if ($GameVersion -eq '1.7') {
    $fallback = Join-Path $ProviderRoot ("1.6/Assemblies/" + $DllName)
    if (Test-Path -LiteralPath $fallback -PathType Leaf) {
        [Console]::Error.WriteLine("[WARN] $DllName uses a 1.6 provider DLL on 1.7 unstable; compatibility probe only.")
        Write-Output (Resolve-Path -LiteralPath $fallback).Path
        exit 0
    }
}
throw "No compatible $DllName for RimWorld $GameVersion under $ProviderRoot"
