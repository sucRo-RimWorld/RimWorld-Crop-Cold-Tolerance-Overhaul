# RimWorld 1.6 / 1.7 tests

Use `run-tests.bat "PATH_TO_RIMWORLD"`. The existing 1.6 build/E2E path remains.
On 1.7, `run-e2e.bat` selects `Scripts/Run-17Tests.ps1`: it builds the existing
Quickstart and Pickle steps against the installed root DLLs / UnityStubs using
Roslyn from an installed dotnet SDK. It preserves the shared production DLL
that passed native 1.7.4976 rev1043 tests. A .NET 10 product build must not
silently replace that DLL and break 1.6; production builds remain separate.

The normal full gate has 14 scenarios: five framework/Info Card, six actual
cold/death/dormancy/room-temperature cases, and three loaded-balance cases.
The latter use Core's 49 plants and an explicitly lightweight 29-plant MO
fixture. They do not prove actual Medieval Overhaul runtime compatibility.
The fixture's inherited graphics/ingestibility must be valid, while its plant
temperature values remain unchanged. `run-e2e.bat` without with-mo-fixture keeps
its established eleven framework/cold scenarios.

Tests use fresh result paths, isolated SaveData and hidden Windows desktops,
retaining Vulkan rendering. Every native run must exit successfully and save
its complete Player.log. 1.7 ignores Unity's -logFile: the helper serializes
access to the native log and verifies its PID. Wrong/empty logs, timeout,
nonzero exit and ERROR entries fail. Pickle uses -pickle-no-http because the
automated test does not need its HTTP dashboard. Cleanup retains the temporary
fixtures inside the result directory and checks product DLL/config hashes.

`Scripts/RimWorldRuntime.ps1` and `Scripts/Invoke-RimWorld.ps1` have identical
copies in Environment; keep them synchronized without creating a Mod runtime
dependency. The existing test assertions remain unchanged. SupportedVersions
and README/Workshop copy are repository/local preparation; Steam upload remains
manual and is not implied by a passing test or updated metadata.
