param(
  [Parameter(Mandatory = $true)]
  [string]$SummaryPath
)

$ErrorActionPreference = "Stop"

function Fail([string]$Message) {
  Write-Host "[FAIL] $Message" -ForegroundColor Red
  exit 1
}

if (-not (Test-Path -LiteralPath $SummaryPath)) {
  Fail "Pickle summary was not produced: $SummaryPath"
}

try {
  $summary = Get-Content -LiteralPath $SummaryPath -Raw -Encoding UTF8 | ConvertFrom-Json
}
catch {
  Fail "Failed to parse Pickle summary: $($_.Exception.Message)"
}

$required = @(
  "Loaded Vanilla crop Defs match the CCTO balance table",
  "Loaded Medieval Overhaul crop Defs match the CCTO balance table"
)

if ([int]$summary.total -ne 13) {
  Fail "Integration suite scenario count is $($summary.total), expected 13."
}

if ([int]$summary.passed -ne 13 -or [int]$summary.failed -ne 0 -or [int]$summary.skipped -ne 0) {
  Fail "Integration suite is not a clean 13/13 pass. passed=$($summary.passed), failed=$($summary.failed), skipped=$($summary.skipped)"
}

$names = @($summary.scenarios | ForEach-Object { [string]$_.name })
foreach ($scenario in $required) {
  if ($names -notcontains $scenario) {
    Fail "Required balance scenario is missing from Pickle summary: $scenario"
  }
}

Write-Host "[OK] Fresh Pickle integration summary contains all 13 required passing scenarios." -ForegroundColor Green
exit 0
