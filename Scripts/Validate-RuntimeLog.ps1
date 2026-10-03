param(
    [Parameter(Mandatory = $true)]
    [string]$LogPath,

    [Parameter(Mandatory = $true)]
    [string]$ModIdPrefixes
)

$ErrorActionPreference = "Stop"

function Fail([string]$Message) {
    Write-Host "[FAIL] $Message" -ForegroundColor Red
    exit 1
}

if (-not (Test-Path -LiteralPath $LogPath)) {
    Fail "Runtime log was not produced: $LogPath"
}

$text = Get-Content -LiteralPath $LogPath -Raw -Encoding UTF8
$prefixes = @(
    $ModIdPrefixes.Split(';') |
        ForEach-Object { $_.Trim().ToLowerInvariant() } |
        Where-Object { -not [string]::IsNullOrWhiteSpace($_) }
)

if ($prefixes.Count -eq 0) {
    Fail "No mod-id prefixes were supplied for runtime log validation."
}

$errors = New-Object System.Collections.Generic.List[string]
$blocks = [regex]::Matches(
    $text,
    '(?ms)^Timestamp:\s*.*?(?=^Timestamp:\s*|\z)'
)

foreach ($match in $blocks) {
    $block = $match.Value
    if ($block -notmatch '(?m)^Level:\s*ERROR\s*$') {
        continue
    }

    $idMatch = [regex]::Match($block, '(?mi)^mod_id:\s*([^\r\n]+)')
    $channelMatch = [regex]::Match($block, '(?mi)^Channel:\s*Mod\.([^\r\n]+)')

    $candidates = @()
    if ($idMatch.Success) {
        $candidates += $idMatch.Groups[1].Value.Trim().ToLowerInvariant()
    }
    if ($channelMatch.Success) {
        $candidates += $channelMatch.Groups[1].Value.Trim().ToLowerInvariant()
    }

    $owned = $false
    foreach ($candidate in $candidates) {
        foreach ($prefix in $prefixes) {
            if ($candidate.StartsWith($prefix, [System.StringComparison]::OrdinalIgnoreCase)) {
                $owned = $true
                break
            }
        }
        if ($owned) { break }
    }

    if ($owned) {
        $errors.Add($block.Trim())
    }
}

# Fallback for CCTO Log.Error messages if a logger format changes and the
# structured Level/mod_id wrapper is unavailable.
$fallbackPatterns = @(
    '\[CCTO\].*ColdToleranceExtension entries',
    '\[CCTO\].*ColdToleranceExtension but is not a plant ThingDef',
    '\[CCTO\].*compatibility failed while drawing'
)
foreach ($pattern in $fallbackPatterns) {
    foreach ($match in [regex]::Matches($text, $pattern, [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)) {
        $value = $match.Value.Trim()
        if (-not $errors.Contains($value)) {
            $errors.Add($value)
        }
    }
}

if ($errors.Count -gt 0) {
    Write-Host "[FAIL] CCTO-origin runtime ERROR entries were found:" -ForegroundColor Red
    $limit = [Math]::Min($errors.Count, 5)
    for ($i = 0; $i -lt $limit; $i++) {
        Write-Host ""
        Write-Host $errors[$i]
    }
    if ($errors.Count -gt $limit) {
        Write-Host ""
        Write-Host "... plus $($errors.Count - $limit) more CCTO runtime ERROR entries."
    }
    exit 1
}

Write-Host "[OK] No CCTO-origin runtime ERROR entries were found." -ForegroundColor Green
exit 0
