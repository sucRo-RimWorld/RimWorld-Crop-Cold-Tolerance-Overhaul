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

# Read the current canonical V3 design instead of repeating the obsolete fixed-death table.
$csvPath = Join-Path $RepoRoot "Docs/ColdDamageHpV3Candidates.csv"
if (-not (Test-Path -LiteralPath $csvPath)) { Fail "Missing canonical V3 CSV: $csvPath" }
$designRows = @(Get-Content -LiteralPath $csvPath -Encoding UTF8 |
  Where-Object { $_.Trim() -and -not $_.StartsWith("#") } | ConvertFrom-Csv)
if ($designRows.Count -ne 78) { Fail "Expected 78 canonical V3 Defs" }
$design = @{}
foreach ($r in $designRows) {
  if ($design.ContainsKey($r.defName)) { Fail "Duplicate canonical Def: $($r.defName)" }
  $design[$r.defName] = $r
}
if (@($designRows | Where-Object { $_.mode -eq "hp_damage" }).Count -ne 46 -or
    @($designRows | Where-Object { $_.mode -eq "dormancy" }).Count -ne 32) {
  Fail "Expected 46 HP and 32 dormancy profiles"
}

function Spec([double]$Min, [string]$DefName) {
  $r = $design[$DefName]
  if ($null -eq $r) { Fail "Missing canonical V3 spec for $DefName" }
  if ($r.mode -eq "dormancy") {
    if ($r.hp_injury_onset_C -or $r.full_hp_degree_game_hours -or $r.lethal_dose_variation) {
      Fail "Dormancy row has HP fields: $DefName"
    }
    return @{ Min=$Min; Onset=$null; Dose=$null; Dormancy=$true }
  }
  if ($r.mode -ne "hp_damage" -or -not $r.hp_injury_onset_C -or
      -not $r.full_hp_degree_game_hours -or $r.lethal_dose_variation -ne "0") {
    Fail "Invalid HP design row: $DefName"
  }
  return @{ Min=$Min; Onset=[double]$r.hp_injury_onset_C;
    Dose=[double]$r.full_hp_degree_game_hours; Dormancy=$false }
}

function AssertNoFrameworkConsumerDefData([string[]]$RelativePaths) {
  $textExtensions = @(".cs", ".xml", ".md", ".feature")

  foreach ($relativePath in $RelativePaths) {
    $path = Join-Path $RepoRoot $relativePath
    if (-not (Test-Path -LiteralPath $path)) { continue }

    $item = Get-Item -LiteralPath $path
    if ($item.PSIsContainer) {
      $files = @(Get-ChildItem -LiteralPath $path -Recurse -File | Where-Object {
        $textExtensions -contains $_.Extension
      })
    } else {
      $files = @($item)
    }

    foreach ($file in $files) {
      $content = Get-Content -LiteralPath $file.FullName -Raw -Encoding UTF8
      if ($content -match '\bAMJC_[A-Za-z0-9_]+\b') {
        $relativeFile = $file.FullName.Substring($RepoRoot.Length).TrimStart('\', '/')
        Fail "AMJC-owned Def/data identifier $($Matches[0]) found in CCTO-owned data surface: $relativeFile"
      }
    }
  }

  Ok "Framework-consumer ownership boundary is clean (no AMJC_ identifiers in CCTO data/code/test surfaces)"
}

AssertNoFrameworkConsumerDefData @(
  "Patches",
  "Source",
  "Tests",
  "Docs/ImplementationTable.md"
)

$vanilla = @{
  Plant_Rice        = Spec 10 "Plant_Rice"
  Plant_Potato      = Spec 5 "Plant_Potato"
  Plant_Corn        = Spec 8 "Plant_Corn"
  Plant_Strawberry  = Spec 5 "Plant_Strawberry"
  Plant_Haygrass    = Spec 0 "Plant_Haygrass"
  Plant_Cotton      = Spec 10 "Plant_Cotton"
  Plant_Devilstrand = Spec 8 "Plant_Devilstrand"
  Plant_Healroot    = Spec 0 "Plant_Healroot"
  Plant_Hops        = Spec 5 "Plant_Hops"
  Plant_Smokeleaf   = Spec 5 "Plant_Smokeleaf"
  Plant_Psychoid    = Spec 8 "Plant_Psychoid"
  Plant_TreeCocoa   = Spec 12 "Plant_TreeCocoa"
  Plant_TreeBamboo   = Spec 10 "Plant_TreeBamboo"
  Plant_TreeBirch    = Spec 5 "Plant_TreeBirch"
  Plant_TreeCecropia = Spec 10 "Plant_TreeCecropia"
  Plant_TreeCypress  = Spec 5 "Plant_TreeCypress"
  Plant_Dandelion    = Spec 0 "Plant_Dandelion"
  Plant_Daylily      = Spec 0 "Plant_Daylily"
  Plant_TreeDrago    = Spec 8 "Plant_TreeDrago"
  Plant_TreeMaple    = Spec 5 "Plant_TreeMaple"
  Plant_TreeOak      = Spec 5 "Plant_TreeOak"
  Plant_TreePalm     = Spec 10 "Plant_TreePalm"
  Plant_TreePine     = Spec 0 "Plant_TreePine"
  Plant_TreePoplar   = Spec 5 "Plant_TreePoplar"
  Plant_Rose         = Spec 5 "Plant_Rose"
  Plant_SaguaroCactus = Spec 8 "Plant_SaguaroCactus"
  Plant_TreeTeak     = Spec 12 "Plant_TreeTeak"
  Plant_Timbershroom = Spec 0 "Plant_Timbershroom"
  Plant_Tinctoria    = Spec 5 "Plant_Tinctoria"
  Plant_TreeWillow   = Spec 5 "Plant_TreeWillow"
  Agarilux               = Spec 0 "Agarilux"
  Plant_Agave            = Spec 5 "Plant_Agave"
  Plant_Alocasia         = Spec 10 "Plant_Alocasia"
  Plant_Ambrosia         = Spec 0 "Plant_Ambrosia"
  Plant_Astragalus       = Spec 0 "Plant_Astragalus"
  Plant_Berry            = Spec 0 "Plant_Berry"
  Plant_Brambles         = Spec 0 "Plant_Brambles"
  Bryolux                = Spec 0 "Bryolux"
  Plant_Bush             = Spec 0 "Plant_Bush"
  Plant_Chokevine        = Spec 0 "Plant_Chokevine"
  Plant_Clivia           = Spec 8 "Plant_Clivia"
  Plant_Rafflesia        = Spec 12 "Plant_Rafflesia"
  Glowstool              = Spec 0 "Glowstool"
  Plant_Grass            = Spec 0 "Plant_Grass"
  Plant_ShrubLow         = Spec 8 "Plant_ShrubLow"
  Plant_Moss             = Spec 0 "Plant_Moss"
  Plant_PincushionCactus = Spec 5 "Plant_PincushionCactus"
  Plant_TallGrass        = Spec 0 "Plant_TallGrass"
  Plant_HealrootWild     = Spec 0 "Plant_HealrootWild"
}

$mo = @{
  DankPyon_Plant_Onions      = Spec 5 "DankPyon_Plant_Onions"
  DankPyon_Plant_Lentils     = Spec 5 "DankPyon_Plant_Lentils"
  DankPyon_Plant_Cabbages    = Spec 0 "DankPyon_Plant_Cabbages"
  DankPyon_Plant_Garlic      = Spec 0 "DankPyon_Plant_Garlic"
  DankPyon_Plant_Mushrooms   = Spec 5 "DankPyon_Plant_Mushrooms"
  DankPyon_Plant_Wheat       = Spec 0 "DankPyon_Plant_Wheat"
  DankPyon_Plant_Flax        = Spec 5 "DankPyon_Plant_Flax"
  DankPyon_Plant_Sugarcane   = Spec 10 "DankPyon_Plant_Sugarcane"
  DankPyon_Plant_Carrots     = Spec 0 "DankPyon_Plant_Carrots"
  DankPyon_Plant_Herb        = Spec 5 "DankPyon_Plant_Herb"
  DankPyon_Plant_Tomatoes    = Spec 10 "DankPyon_Plant_Tomatoes"
  DankPyon_Plant_Grape       = Spec 5 "DankPyon_Plant_Grape"
  DankPyon_Plant_Pumpkins    = Spec 10 "DankPyon_Plant_Pumpkins"
  DankPyon_Tree_Apple        = Spec 5 "DankPyon_Tree_Apple"
  DankPyon_Tree_Lemon        = Spec 10 "DankPyon_Tree_Lemon"
  DankPyon_Tree_Mulberry     = Spec 5 "DankPyon_Tree_Mulberry"
  DankPyon_Tree_GriffonBerry = Spec 5 "DankPyon_Tree_GriffonBerry"
  DankPyon_Plant_Mindwort    = Spec 5 "DankPyon_Plant_Mindwort"
  DankPyon_Plant_Poppy       = Spec 5 "DankPyon_Plant_Poppy"
  DankPyon_Plant_Fleawort    = Spec 3 "DankPyon_Plant_Fleawort"
  DankPyon_Plant_FlyAgaric   = Spec 0 "DankPyon_Plant_FlyAgaric"
  DankPyon_Plant_MindwortWild  = Spec 5 "DankPyon_Plant_MindwortWild"
  DankPyon_Plant_PoppyWild     = Spec 5 "DankPyon_Plant_PoppyWild"
  DankPyon_Plant_FleawortWild  = Spec 3 "DankPyon_Plant_FleawortWild"
  DankPyon_Plant_FlyAgaricWild = Spec 0 "DankPyon_Plant_FlyAgaricWild"
  DankPyon_GreatOak            = Spec 5 "DankPyon_GreatOak"
  DankPyon_GreatIter           = Spec 5 "DankPyon_GreatIter"
  DankPyon_GreatFir            = Spec 0 "DankPyon_GreatFir"
  DankPyon_GreatWillow         = Spec 5 "DankPyon_GreatWillow"
}

# These wild alchemy defs inherit from named cultivated defs in MO 1.6.
# Their CCTO extension must therefore be inherited, not appended directly.
$moInheritedExtensions = @{
  DankPyon_Plant_MindwortWild  = "DankPyon_Plant_Mindwort"
  DankPyon_Plant_PoppyWild     = "DankPyon_Plant_Poppy"
  DankPyon_Plant_FleawortWild  = "DankPyon_Plant_Fleawort"
  DankPyon_Plant_FlyAgaricWild = "DankPyon_Plant_FlyAgaric"
}

$moExpectedParentNames = @{
  DankPyon_Plant_MindwortWild  = "DankPyon_MindwortBase"
  DankPyon_Plant_PoppyWild     = "DankPyon_PoppyBase"
  DankPyon_Plant_FleawortWild  = "DankPyon_FleawortBase"
  DankPyon_Plant_FlyAgaricWild = "DankPyon_FlyAgaricBase"
}

function DefFromXPath([string]$Text) {
  if ($Text -match 'defName="([^"]+)"') { return $Matches[1] }
  return $null
}

function ValidatePatch(
  [string]$Path,
  [hashtable]$Expected,
  [string]$Label,
  [hashtable]$InheritedExtensions = @{}
) {
  [xml]$xml = LoadXml $Path
  $extensions = @{}
  $mins = @{}

  foreach ($node in @($xml.SelectNodes("//*[@Class='PatchOperationAddModExtension']"))) {
    $defName = DefFromXPath ([string]$node.xpath)
    if (-not $defName) { continue }
    if ($extensions.ContainsKey($defName)) { Fail "$Label duplicate direct extension: $defName" }

    $ext = $node.value.li
    $onset = $null
    $dose = $null
    if ($null -ne $ext.coldDamageStartTemperature) { $onset = [double]$ext.coldDamageStartTemperature }
    if ($null -ne $ext.coldDamageDegreeHoursToDie) { $dose = [double]$ext.coldDamageDegreeHoursToDie }
    $dormancy = $false
    if ($null -ne $ext.coldDormancy) { $dormancy = ([string]$ext.coldDormancy -eq "true") }
    if ($null -ne $ext.coldDeathTemperature) { Fail "$Label still contains legacy death threshold: $defName" }
    if ($null -ne $ext.coldDamageLethalDoseVariation -and
        [double]$ext.coldDamageLethalDoseVariation -ne 0) {
      Fail "$Label has unapproved HP variation: $defName"
    }
    $extensions[$defName] = @{ Onset=$onset; Dose=$dose; Dormancy=$dormancy }
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

  $expectedDirectExtensionCount = $Expected.Count - $InheritedExtensions.Count
  if ($extensions.Count -ne $expectedDirectExtensionCount) {
    Fail "$Label direct extension count $($extensions.Count), expected $expectedDirectExtensionCount"
  }

  foreach ($defName in $Expected.Keys) {
    if (-not $mins.ContainsKey($defName)) { Fail "$Label missing minGrowthTemperature: $defName" }

    $want = $Expected[$defName]
    $extensionSource = $defName

    if ($InheritedExtensions.ContainsKey($defName)) {
      if ($extensions.ContainsKey($defName)) {
        Fail "$Label inherited target must not append a second extension: $defName"
      }
      $extensionSource = [string]$InheritedExtensions[$defName]
    }

    if (-not $extensions.ContainsKey($extensionSource)) {
      Fail "$Label missing extension source for $defName (source: $extensionSource)"
    }

    $got = $extensions[$extensionSource]

    if ([double]$mins[$defName] -ne [double]$want.Min) {
      Fail "$Label wrong minGrowthTemperature for $defName"
    }

    if ([bool]$got.Dormancy -ne [bool]$want.Dormancy) {
      Fail "$Label wrong dormancy flag for $defName"
    }

    if ($want.Dormancy) {
      if ($null -ne $got.Onset -or $null -ne $got.Dose) {
        Fail "$Label dormant profile has HP injury fields: $defName"
      }
    } else {
      if ($null -eq $got.Onset -or $null -eq $got.Dose -or
          [double]$got.Onset -ne [double]$want.Onset -or
          [double]$got.Dose -ne [double]$want.Dose) {
        Fail "$Label V3 onset/dose mismatch for $defName"
      }
    }
  }

  Ok "$Label balance XML matches expected data ($($Expected.Count) plants)"
}

ValidatePatch (Join-Path $RepoRoot "Patches/Vanilla_ColdTolerance.xml") $vanilla "Vanilla"
ValidatePatch (Join-Path $RepoRoot "Patches/MedievalOverhaul_ColdTolerance.xml") $mo "Medieval Overhaul" $moInheritedExtensions

[xml]$about = LoadXml (Join-Path $RepoRoot "About/About.xml")
$loadAfter = @($about.ModMetaData.loadAfter.li | ForEach-Object { [string]$_ })
if ($loadAfter -notcontains "DankPyon.Medieval.Overhaul") {
  Fail "About.xml is missing DankPyon.Medieval.Overhaul in loadAfter"
}
Ok "About.xml load order is correct"

if ($MedievalOverhaulRoot) {
  [xml]$farm = LoadXml (Join-Path $MedievalOverhaulRoot "1.6/Defs/ThingDefs_Plants/Plants_Cultivated_Farm.xml")
  [xml]$alchemy = LoadXml (Join-Path $MedievalOverhaulRoot "1.6/Defs/ThingDefs_Plants/Plants_Cultivated_Alchemy.xml")
  [xml]$wildAlchemy = LoadXml (Join-Path $MedievalOverhaulRoot "1.6/Defs/ThingDefs_Plants/Plants_Wild_Alchemy.xml")
  [xml]$wildDarkForest = LoadXml (Join-Path $MedievalOverhaulRoot "1.6/Defs/ThingDefs_Plants/Plants_Wild_DarkForest.xml")

  $sourceDefs = @{}
  foreach ($def in @($farm.Defs.ThingDef) + @($alchemy.Defs.ThingDef) + @($wildAlchemy.Defs.ThingDef) + @($wildDarkForest.Defs.ThingDef)) {
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

    if ($moExpectedParentNames.ContainsKey($defName)) {
      $actualParent = [string]$sourceDef.ParentName
      $expectedParent = [string]$moExpectedParentNames[$defName]
      if ($actualParent -ne $expectedParent) {
        Fail "MO source ParentName mismatch for ${defName}: expected $expectedParent, got $actualParent"
      }
    }
  }
  Ok "All $($mo.Count) MO target DefNames, local plant nodes, and wild-alchemy inheritance links match supplied MO 1.6 source"
}

Write-Host ""
Ok "CCTO balance validation passed"
exit 0
