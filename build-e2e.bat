@echo off
setlocal EnableExtensions EnableDelayedExpansion

set "RIMWORLD_DIR=%~1"
if not defined RIMWORLD_DIR set "RIMWORLD_DIR=D:\SteamLibrary\steamapps\common\RimWorld"

set "E2E_MOD_DIR=%~2"
if not defined E2E_MOD_DIR set "E2E_MOD_DIR=%RIMWORLD_DIR%\Mods\CropColdToleranceOverhaul.E2E"

set "CSC=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
if not exist "%CSC%" set "CSC=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\csc.exe"

if not exist "%CSC%" (
    echo [ERROR] C# compiler was not found.
    exit /b 1
)

set "ROOT=%~dp0"
set "MAIN_DLL=%ROOT%Assemblies\CropColdToleranceOverhaul.dll"
set "MANAGED=%RIMWORLD_DIR%\RimWorldWin64_Data\Managed"
set "ASSEMBLY_CSHARP=%MANAGED%\Assembly-CSharp.dll"
set "UNITY_CORE=%MANAGED%\UnityEngine.CoreModule.dll"
set "UNITY_MATH=%MANAGED%\Unity.Mathematics.dll"
set "NETSTANDARD=%MANAGED%\netstandard.dll"
set "STEAMAPPS=%RIMWORLD_DIR%\..\.."

if not exist "%UNITY_MATH%" (
    echo [ERROR] Required RimWorld managed assembly was not found:
    echo         %UNITY_MATH%
    exit /b 1
)

if not exist "%NETSTANDARD%" (
    echo [ERROR] Required RimWorld managed assembly was not found:
    echo         %NETSTANDARD%
    exit /b 1
)

echo [1/5] Building CCTO...
call "%ROOT%build.bat" "%RIMWORLD_DIR%"
if errorlevel 1 exit /b 1

set "PICKLE_ROOT=%STEAMAPPS%\workshop\content\294100\3791648678"
set "QUICKSTARTS_ROOT=%STEAMAPPS%\workshop\content\294100\3793646067"

set "PICKLE_DLL="
if exist "%PICKLE_ROOT%\1.6\Assemblies\RimWorks.Pickle.dll" (
    set "PICKLE_DLL=%PICKLE_ROOT%\1.6\Assemblies\RimWorks.Pickle.dll"
)
if not defined PICKLE_DLL if exist "%PICKLE_ROOT%" (
    for /r "%PICKLE_ROOT%" %%F in (RimWorks.Pickle.dll) do (
        if not defined PICKLE_DLL set "PICKLE_DLL=%%~fF"
    )
)

set "QUICKSTARTS_DLL="
if exist "%QUICKSTARTS_ROOT%\1.6\Assemblies\Quickstarts.dll" (
    set "QUICKSTARTS_DLL=%QUICKSTARTS_ROOT%\1.6\Assemblies\Quickstarts.dll"
)
if not defined QUICKSTARTS_DLL if exist "%QUICKSTARTS_ROOT%" (
    for /r "%QUICKSTARTS_ROOT%" %%F in (Quickstarts.dll) do (
        if not defined QUICKSTARTS_DLL set "QUICKSTARTS_DLL=%%~fF"
    )
)

if not defined PICKLE_DLL (
    echo.
    echo [ERROR] RimWorks.Pickle.dll was not found under:
    echo         %PICKLE_ROOT%
    echo.
    echo Install/subscribe to Pickle ^(Steam Workshop 3791648678^) first.
    exit /b 1
)

if not defined QUICKSTARTS_DLL (
    echo.
    echo [ERROR] Quickstarts.dll was not found under:
    echo         %QUICKSTARTS_ROOT%
    echo.
    echo Install/subscribe to Quickstarts ^(Steam Workshop 3793646067^) first.
    exit /b 1
)

if not exist "%MAIN_DLL%" (
    echo [ERROR] CCTO main assembly was not generated:
    echo         %MAIN_DLL%
    exit /b 1
)

set "ABOUT_DIR=%E2E_MOD_DIR%\About"
set "ASSEMBLIES_DIR=%E2E_MOD_DIR%\Assemblies"
set "PICKLE_ASSEMBLIES_DIR=%E2E_MOD_DIR%\Pickle\Assemblies"
set "FEATURES_DIR=%E2E_MOD_DIR%\Pickle\Features"

if not exist "%ABOUT_DIR%" mkdir "%ABOUT_DIR%"
if not exist "%ASSEMBLIES_DIR%" mkdir "%ASSEMBLIES_DIR%"
if not exist "%PICKLE_ASSEMBLIES_DIR%" mkdir "%PICKLE_ASSEMBLIES_DIR%"
if not exist "%FEATURES_DIR%" mkdir "%FEATURES_DIR%"

copy /Y "%ROOT%Tests\E2E\TestMod\About\About.xml" "%ABOUT_DIR%\About.xml" >nul
if errorlevel 1 exit /b 1

copy /Y "%ROOT%Tests\E2E\TestMod\Pickle\Features\*.feature" "%FEATURES_DIR%\" >nul
if errorlevel 1 (
    echo [ERROR] Failed to copy Pickle feature files.
    exit /b 1
)

set "QUICKSTART_OUTPUT=%ASSEMBLIES_DIR%\CropColdToleranceOverhaul.E2E.dll"
set "STEPS_OUTPUT=%PICKLE_ASSEMBLIES_DIR%\CropColdToleranceOverhaul.E2E.Steps.dll"

echo.
echo [2/5] Building deterministic Quickstarts fixture...
echo Quickstarts : %QUICKSTARTS_DLL%

if exist "%UNITY_CORE%" (
    "%CSC%" /nologo /target:library /optimize+ /out:"%QUICKSTART_OUTPUT%" ^
        /reference:"%ASSEMBLY_CSHARP%" ^
        /reference:"%UNITY_CORE%" ^
        /reference:"%UNITY_MATH%" ^
        /reference:"%NETSTANDARD%" ^
        /reference:"%QUICKSTARTS_DLL%" ^
        "%ROOT%Tests\E2E\CctoColdToleranceQuickstart.cs"
) else (
    "%CSC%" /nologo /target:library /optimize+ /out:"%QUICKSTART_OUTPUT%" ^
        /reference:"%ASSEMBLY_CSHARP%" ^
        /reference:"%UNITY_MATH%" ^
        /reference:"%NETSTANDARD%" ^
        /reference:"%QUICKSTARTS_DLL%" ^
        "%ROOT%Tests\E2E\CctoColdToleranceQuickstart.cs"
)

if errorlevel 1 (
    echo.
    echo [ERROR] Quickstarts fixture build failed.
    exit /b 1
)

echo.
echo [3/5] Building Pickle step assembly...
echo Pickle      : %PICKLE_DLL%

if exist "%UNITY_CORE%" (
    "%CSC%" /nologo /target:library /optimize+ /out:"%STEPS_OUTPUT%" ^
        /reference:"%ASSEMBLY_CSHARP%" ^
        /reference:"%UNITY_CORE%" ^
        /reference:"%UNITY_MATH%" ^
        /reference:"%NETSTANDARD%" ^
        /reference:"%MAIN_DLL%" ^
        /reference:"%PICKLE_DLL%" ^
        "%ROOT%Tests\E2E\ColdToleranceSteps.cs"
) else (
    "%CSC%" /nologo /target:library /optimize+ /out:"%STEPS_OUTPUT%" ^
        /reference:"%ASSEMBLY_CSHARP%" ^
        /reference:"%UNITY_MATH%" ^
        /reference:"%NETSTANDARD%" ^
        /reference:"%MAIN_DLL%" ^
        /reference:"%PICKLE_DLL%" ^
        "%ROOT%Tests\E2E\ColdToleranceSteps.cs"
)

if errorlevel 1 (
    echo.
    echo [ERROR] Pickle step build failed.
    exit /b 1
)

echo.
echo [4/5] Verifying generated E2E layout...
if not exist "%QUICKSTART_OUTPUT%" exit /b 1
if not exist "%STEPS_OUTPUT%" exit /b 1
if not exist "%FEATURES_DIR%\cold-tolerance.feature" exit /b 1
if not exist "%FEATURES_DIR%\framework.feature" exit /b 1
if exist "%ROOT%Tests\E2E\TestMod\Pickle\Features\balance.feature" (
    if not exist "%FEATURES_DIR%\balance.feature" exit /b 1
)

echo.
echo [5/5] CCTO E2E test mod is ready:
echo       %E2E_MOD_DIR%
echo.
echo Enable these mods before running:
echo   - Harmony
echo   - RimLogging
echo   - Pickle
echo   - Quickstarts
echo   - Crop Cold Tolerance Overhaul
echo   - [DEV] Crop Cold Tolerance Overhaul E2E
echo.
echo [OK] E2E build succeeded.
exit /b 0
