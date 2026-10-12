$ErrorActionPreference = 'Stop'
$selector = Join-Path $PSScriptRoot '../../Scripts/Resolve-TestAssembly.ps1'
$root = Join-Path ([IO.Path]::GetTempPath()) ('CCTO-Provider-' + [guid]::NewGuid().ToString('N'))
try {
    foreach ($version in @('1.6', '1.7', 'Current')) {
        $folder = Join-Path $root "$version/Assemblies"
        $null = New-Item -Path $folder -ItemType Directory -Force
        [IO.File]::WriteAllBytes((Join-Path $folder 'Quickstarts.dll'), [byte[]]@(1, 2, 3))
    }

    $selected16 = & $selector -ProviderRoot $root -DllName 'Quickstarts.dll' -GameVersion '1.6'
    if ($selected16 -ne (Join-Path $root '1.6/Assemblies/Quickstarts.dll')) { throw '1.6 provider selection changed' }
    $selected17 = & $selector -ProviderRoot $root -DllName 'Quickstarts.dll' -GameVersion '1.7'
    if ($selected17 -ne (Join-Path $root '1.7/Assemblies/Quickstarts.dll')) { throw '1.7 provider selection changed' }

    @'
<loadFolders>
  <v1.6><li>1.6</li></v1.6>
  <v1.7><li>/</li><li>1.7</li></v1.7>
</loadFolders>
'@ | Set-Content -LiteralPath (Join-Path $root 'LoadFolders.xml') -Encoding UTF8
    $loaded = & $selector -ProviderRoot $root -DllName 'Quickstarts.dll' -GameVersion '1.7'
    if ($loaded -ne $selected17) { throw '1.7 load-folder precedence changed' }

    @'
<loadFolders><v1.6><li>1.6</li></v1.6></loadFolders>
'@ | Set-Content -LiteralPath (Join-Path $root 'LoadFolders.xml') -Encoding UTF8
    $fallback = & $selector -ProviderRoot $root -DllName 'Quickstarts.dll' -GameVersion '1.7'
    if ($fallback -ne $selected16) { throw '1.7 v1.6 compatibility fallback changed' }
    Remove-Item -LiteralPath (Join-Path $root '1.6/Assemblies/Quickstarts.dll')
    $failed = $false
    try { $null = & $selector -ProviderRoot $root -DllName 'Quickstarts.dll' -GameVersion '1.7' }
    catch { $failed = $true }
    if (-not $failed) { throw 'Missing declared provider was silently accepted' }

    $failed = $false
    try { $null = & $selector -ProviderRoot $root -DllName 'Quickstarts.dll' -GameVersion '1.8' }
    catch { $failed = $true }
    if (-not $failed) { throw 'Unsupported RimWorld test version was accepted' }
    Write-Output '[OK] CCTO 1.6/1.7 test-provider selection and rejection contracts passed.'
}
finally {
    if (Test-Path -LiteralPath $root) { Remove-Item -LiteralPath $root -Recurse -Force }
}
