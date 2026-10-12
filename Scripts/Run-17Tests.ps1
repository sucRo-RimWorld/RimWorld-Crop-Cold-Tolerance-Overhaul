param([string]$GameRoot='D:\SteamLibrary\steamapps\common\RimWorld',[switch]$CoreOnly)
$ErrorActionPreference='Stop'
. (Join-Path $PSScriptRoot 'RimWorldRuntime.ps1')
$runtime=Get-RimWorldRuntime $GameRoot
if(-not $runtime.Modern){throw 'This entry is for RimWorld 1.7'}
$root=Split-Path -Parent $PSScriptRoot
$production=Join-Path $root 'Assemblies\CropColdToleranceOverhaul.dll'
if(-not (Test-Path -LiteralPath $production)){throw 'Shared production DLL is required'}
$stamp=Get-Date -Format 'yyyyMMdd-HHmmss-fff'
$result=Join-Path $root ('TestResults\RimWorld17-'+$stamp)
New-Item -ItemType Directory -Force -Path $result | Out-Null
$mods=Join-Path $GameRoot 'Mods'
$fixture=Join-Path $mods 'CCTO.RuntimeTests17'
$mo=Join-Path $mods 'CCTO.RuntimeMOFixture17'
if((Test-Path -LiteralPath $fixture) -or (Test-Path -LiteralPath $mo)){throw 'Existing temporary CCTO 1.7 fixture requires inspection'}
$before=(Get-FileHash -LiteralPath $production).Hash
$normal=Join-Path $env:USERPROFILE 'AppData\LocalLow\Ludeon Studios\RimWorld by Ludeon Studios\Config'
$originals=@{}
foreach($file in @('ModsConfig.xml','Prefs.xml')){$originals[$file]=(Get-FileHash -LiteralPath (Join-Path $normal $file)).Hash}
$workshop=[IO.Path]::GetFullPath((Join-Path $GameRoot '..\..\workshop\content\294100'))
$quick=Find-RimWorldModAssembly (Join-Path $workshop '3793646067') 'Quickstarts.dll'
$pickle=Find-RimWorldModAssembly (Join-Path $workshop '3791648678') 'RimWorks.Pickle.dll'
$owned=@()
try {
 New-Item -ItemType Directory -Path $fixture | Out-Null; $owned+=$fixture
 New-Item -ItemType Directory -Force -Path "$fixture\About","$fixture\Assemblies","$fixture\Pickle\Assemblies","$fixture\Pickle\Features" | Out-Null
 [xml]$about=Get-Content (Join-Path $root 'Tests\E2E\TestMod\About\About.xml') -Raw
 $about.ModMetaData.packageId='sucro.cropcoldtoleranceoverhaul.e2e17'
 $about.Save("$fixture\About\About.xml")
 Copy-Item -Path (Join-Path $root 'Tests\E2E\TestMod\Pickle\Features\*.feature') -Destination "$fixture\Pickle\Features"
 Invoke-RimWorldTestCompile $GameRoot "$fixture\Assemblies\CropColdToleranceOverhaul.E2E.dll" @(Join-Path $root 'Tests\E2E\CctoColdToleranceQuickstart.cs') @($quick)
 Invoke-RimWorldTestCompile $GameRoot "$fixture\Pickle\Assemblies\CropColdToleranceOverhaul.E2E.Steps.dll" @(Join-Path $root 'Tests\E2E\ColdToleranceSteps.cs') @($production,$pickle)
 $ids=@('brrainz.harmony','ludeon.rimworld')
 if(-not $CoreOnly) {
  New-Item -ItemType Directory -Path $mo | Out-Null; $owned+=$mo
  Copy-Item -LiteralPath (Join-Path $root 'Tests\E2E\MOFixture\About'),(Join-Path $root 'Tests\E2E\MOFixture\Defs') -Destination $mo -Recurse
  $ids+='sucro.cropcoldtoleranceoverhaul.mofixture'
 }
 $ids+=@('rimworks.rimlogging','rimworks.pickle','rimworks.quickstarts','sucro.cropcoldtoleranceoverhaul','sucro.cropcoldtoleranceoverhaul.e2e17')
 $save=Join-Path $result 'SaveData'
 $config=Join-Path $save 'Config'
 New-Item -ItemType Directory -Path $config -Force | Out-Null
 [xml]$xml=Get-Content (Join-Path $normal 'ModsConfig.xml') -Raw
 $active=$xml.SelectSingleNode('/ModsConfigData/activeMods')
 $active.RemoveAll()
 foreach($id in $ids){$li=$xml.CreateElement('li');$li.InnerText=$id;[void]$active.AppendChild($li)}
 $xml.ModsConfigData.version=$runtime.Version.Split(' ')[0]
 $xml.Save((Join-Path $config 'ModsConfig.xml'))
 [xml]$prefs=Get-Content (Join-Path $normal 'Prefs.xml') -Raw
 foreach($pair in @(@('devMode','True'),@('resetModsConfigOnCrash','False'),@('fullscreen','False'),@('runInBackground','True'))){$n=$prefs.DocumentElement.SelectSingleNode($pair[0]);if(-not $n){$n=$prefs.CreateElement($pair[0]);[void]$prefs.DocumentElement.AppendChild($n)};$n.InnerText=$pair[1]}
 $prefs.Save((Join-Path $config 'Prefs.xml'))
 $report=Join-Path $result 'Reports'
 $filter='framework.feature,cold-tolerance.feature'
 if(-not $CoreOnly){$filter+=',balance.feature'}
 $arguments='-savedatafolder="'+$save+'" -pickle-run="'+$filter+'" -pickle-mode=fast -pickle-report-dir="'+$report+'" -pickle-no-browser -pickle-no-http -pickle-run-timeout=4'
 Invoke-RimWorldIsolated $GameRoot $arguments (Join-Path $result 'Player.log') 300
 $summary=Get-Content (Join-Path $report 'summary.json') -Raw | ConvertFrom-Json
 $expected=if($CoreOnly){11}else{14}
 if($summary.total -ne $expected -or $summary.passed -ne $expected -or $summary.failed -ne 0 -or $summary.skipped -ne 0 -or $summary.exitReason -ne 'passed'){throw 'Missing/failing CCTO scenarios'}
 Write-Output "[OK] CCTO 1.7 $expected/$expected; runtime ERROR 0. Evidence: $result"
} finally {
 foreach($target in $owned) {
  $full=[IO.Path]::GetFullPath($target)
  if([IO.Path]::GetDirectoryName($full) -ne [IO.Path]::GetFullPath($mods)){throw 'Fixture cleanup outside Mods refused'}
  Move-Item -LiteralPath $full -Destination (Join-Path $result ([IO.Path]::GetFileName($full)))
 }
 if((Get-FileHash -LiteralPath $production).Hash -ne $before){throw 'Production DLL changed'}
 foreach($file in $originals.Keys){if((Get-FileHash -LiteralPath (Join-Path $normal $file)).Hash -ne $originals[$file]){throw 'Normal configuration changed'}}
}
