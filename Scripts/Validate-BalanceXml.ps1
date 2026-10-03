param(
  [string]$RepoRoot = (Split-Path -Parent $PSScriptRoot),
  [string]$MedievalOverhaulRoot = ""
)

$ErrorActionPreference = "Stop"

function Fail([string]$Message) {
  Write-Host "[FAIL] $Message" -ForegroundColor Red
  exit 1
}

function Ok([string]$Message) {
  Write-Host "[OK] $Message" -ForegroundColor Green
}

function LoadXml([string]$Path) {
  if (-not (Test-Path -LiteralPath $Path)) { Fail "Missing XML file: $Path" }
  try { return [xml](Get-Content -LiteralPath $Path -Raw -Encoding UTF8) }
  catch { Fail "Invalid XML: $Path" }
}

function Spec([double]$Min, [object]$Death, [bool]$Dormancy) {
  return @{ Min=$Min; Death=$Death; Dormancy=$Dormancy }
}

$vanilla = @{
  Plant_Rice        = Spec 10 (-1) $false
  Plant_Potato      = Spec 5  (-2) $false
  Plant_Corn        = Spec 8  (-2) $false
  Plant_Strawberry  = Spec 5  (-9) $false
  Plant_Haygrass    = Spec 0  (-9) $false
  Plant_Cotton      = Spec 10 (-1) $false
  Plant_Devilstrand = Spec 8  (-1) $false
  Plant_Healroot    = Spec 0  (-9) $false
  Plant_Hops        = Spec 5  $null $true
  Plant_Smokeleaf   = Spec 5  (-4) $false
  Plant_Psychoid    = Spec 8  (-1) $false
  Plant_TreeCocoa   = Spec 12 0 $false
  Plant_TreeBamboo   = Spec 8  (-5) $false
  Plant_TreeBirch    = Spec 5  $null $true
  Plant_TreeCecropia = Spec 10 0 $false
  Plant_TreeCypress  = Spec 5  $null $true
  Plant_Dandelion    = Spec 0  $null $true
  Plant_Daylily      = Spec 0  $null $true
  Plant_TreeDrago    = Spec 8  0 $false
  Plant_TreeMaple    = Spec 5  $null $true
  Plant_TreeOak      = Spec 5  $null $true
  Plant_TreePalm     = Spec 10 0 $false
  Plant_TreePine     = Spec 0  (-35) $false
  Plant_TreePoplar   = Spec 5  $null $true
  Plant_Rose         = Spec 5  $null $true
  Plant_SaguaroCactus = Spec 8 (-6) $false
  Plant_TreeTeak     = Spec 12 3 $false
  Plant_Timbershroom = Spec 0  $null $true
  Plant_Tinctoria    = Spec 5  (-4) $false
  Plant_TreeWillow   = Spec 5  $null $true
}

$mo = @{
  DankPyon_Plant_Onions      = Spec 5  (-3) $false
  DankPyon_Plant_Lentils     = Spec 5  (-4) $false
  DankPyon_Plant_Cabbages    = Spec 0  (-6) $false
  DankPyon_Plant_Garlic      = Spec 0  $null $true
  DankPyon_Plant_Mushrooms   = Spec 5  (-1) $false
  DankPyon_Plant_Wheat       = Spec 0  (-6) $false
  DankPyon_Plant_Flax        = Spec 5  (-5) $false
  DankPyon_Plant_Sugarcane   = Spec 10 (-5) $false
  DankPyon_Plant_Carrots     = Spec 0  (-4) $false
  DankPyon_Plant_Herb        = Spec 5  (-3) $false
  DankPyon_Plant_Tomatoes    = Spec 10 (-1) $false
  DankPyon_Plant_Grape       = Spec 5  $null $true
  DankPyon_Plant_Pumpkins    = Spec 10 (-1) $false
  DankPyon_Tree_Apple        = Spec 5  $null $true
  DankPyon_Tree_Lemon        = Spec 10 (-4) $false
  DankPyon_Tree_Mulberry     = Spec 5  $null $true
  DankPyon_Tree_GriffonBerry = Spec 5  $null $true
  DankPyon_Plant_Mindwort    = Spec 5  (-3) $false
  DankPyon_Plant_Poppy       = Spec 5  (-5) $false
  DankPyon_Plant_Fleawort    = Spec 3  (-6) $false
  DankPyon_Plant_FlyAgaric   = Spec 0  $null $true
}

function DefFromXPath([string]$Text) {
  if ($Text -match 'defName="([^"]+)"') { return $Matches[1] }
  return $null
}

function ValidatePatch([string]$Path, [hashtable]$Expected, [string]$Label) {
  [xml]$xml = LoadXml $Path
  $extensions = @{}
  $mins = @{}

  foreach ($node in @($xml.SelectNodes("//*[@Class='PatchOperationAddModExtension']"))) {
    $defName = DefFromXPath ([string]$node.xpath)
    if (-not $defName) { continue }
    if ($extensions.ContainsKey($defName)) { Fail "$Label duplicate extension: $defName" }

    $ext = $node.value.li
    $death = $null
    if ($null -ne $ext.coldDeathTemperature) { $death = [double]$ext.coldDeathTemperature }
    $dormancy = $false
    if ($null -ne $ext.coldDormancy) { $dormancy = ([string]$ext.coldDormancy -eq "true") }

    $extensions[$defName] = @{ Death=$death; Dormancy=$dormancy }
  }

  foreach ($node in @($xml.SelectNodes("//*[@Class='PatchOperationConditional']"))) {
    $pathText = [string]$node.xpath
    if ($pathText -notlike "*/plant/minGrowthTemperature") { continue }

    $defName = DefFromXPath $pathText
    if (-not $defName) { continue }

    $matchValue = [double]$node.match.value.minGrowthTemperature
    $nomatchValue = [double]$node.nomatch.value.minGrowthTemperature
    if ($matchValue -ne $nomatchValue) { Fail "$Label min mismatch inside patch: $defName" }
    if ($mins.ContainsKey($defName)) { Fail "$Label duplicate minGrowthTemperature: $defName" }
    $mins[$defName] = $matchValue
  }

  if ($extensions.Count -ne $Expected.Count) {
    Fail "$Label extension count $($extensions.Count), expected $($Expected.Count)"
  }

  foreach ($defName in $Expected.Keys) {
    if (-not $extensions.ContainsKey($defName)) { Fail "$Label missing extension: $defName" }
    if (-not $mins.ContainsKey($defName)) { Fail "$Label missing minGrowthTemperature: $defName" }

    $want = $Expected[$defName]
    $got = $extensions[$defName]

    if ([double]$mins[$defName] -ne [double]$want.Min) {
      Fail "$Label wrong minGrowthTemperature for $defName"
    }

    if ([bool]$got.Dormancy -ne [bool]$want.Dormancy) {
      Fail "$Label wrong dormancy flag for $defName"
    }

    if ($null -eq $want.Death) {
      if ($null -ne $got.Death) { Fail "$Label unexpected death threshold for $defName" }
    } else {
      if ($null -eq $got.Death -or [double]$got.Death -ne [double]$want.Death) {
        Fail "$Label wrong death threshold for $defName"
      }
    }
  }

  Ok "$Label balance XML matches expected data ($($Expected.Count) crops)"
}

ValidatePatch (Join-Path $RepoRoot "Patches/Vanilla_ColdTolerance.xml") $vanilla "Vanilla"
ValidatePatch (Join-Path $RepoRoot "Patches/MedievalOverhaul_ColdTolerance.xml") $mo "Medieval Overhaul"

[xml]$about = LoadXml (Join-Path $RepoRoot "About/About.xml")
$loadAfter = @($about.ModMetaData.loadAfter.li | ForEach-Object { [string]$_ })
if ($loadAfter -notcontains "DankPyon.Medieval.Overhaul") {
  Fail "About.xml is missing DankPyon.Medieval.Overhaul in loadAfter"
}
Ok "About.xml load order is correct"

if ($MedievalOverhaulRoot) {
  [xml]$farm = LoadXml (Join-Path $MedievalOverhaulRoot "1.6/Defs/ThingDefs_Plants/Plants_Cultivated_Farm.xml")
  [xml]$alchemy = LoadXml (Join-Path $MedievalOverhaulRoot "1.6/Defs/ThingDefs_Plants/Plants_Cultivated_Alchemy.xml")

  $sourceDefs = @{}
  foreach ($def in @($farm.Defs.ThingDef) + @($alchemy.Defs.ThingDef)) {
    if ($null -ne $def.defName) { $sourceDefs[[string]$def.defName] = $def }
  }

  foreach ($defName in $mo.Keys) {
    if (-not $sourceDefs.ContainsKey($defName)) {
      Fail "MO source missing target DefName: $defName"
    }

    $sourceDef = $sourceDefs[$defName]
    if ($null -eq $sourceDef.plant) {
      Fail "MO source target has no local <plant> node required by CCTO XPath: $defName"
    }
  }
  Ok "All $($mo.Count) MO target DefNames and local plant nodes exist in supplied MO 1.6 source"
}

Write-Host ""
Ok "CCTO balance validation passed"
exit 0
