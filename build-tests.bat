@echo off
setlocal EnableExtensions EnableDelayedExpansion

set "RIMWORLD_DIR=%~1"
if not defined RIMWORLD_DIR set "RIMWORLD_DIR=D:\SteamLibrary\steamapps\common\RimWorld"

set "TEST_MOD_DIR=%~2"
if not defined TEST_MOD_DIR set "TEST_MOD_DIR=%RIMWORLD_DIR%\Mods\CropColdToleranceOverhaul.Tests"

set "CSC=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
if not exist "%CSC%" set "CSC=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\csc.exe"

if not exist "%CSC%" (
    echo [ERROR] C# compiler was not found.
    exit /b 1
)

set "ROOT=%~dp0"
set "MAIN_DLL=%ROOT%Assemblies\CropColdToleranceOverhaul.dll"
set "SOURCE_DIR=%ROOT%Tests\RimTest"
set "ABOUT_SOURCE=%ROOT%Tests\RimTest\TestMod\About\About.xml"

echo [1/3] Building CCTO...
call "%ROOT%build.bat" "%RIMWORLD_DIR%"
if errorlevel 1 exit /b 1

set "MANAGED=%RIMWORLD_DIR%\RimWorldWin64_Data\Managed"
set "ASSEMBLY_CSHARP=%MANAGED%\Assembly-CSharp.dll"
set "UNITY_CORE=%MANAGED%\UnityEngine.CoreModule.dll"

set "STEAMAPPS=%RIMWORLD_DIR%\..\.."
set "RIMTEST_ROOT=%STEAMAPPS%\workshop\content\294100\3762405308"
set "RIMTEST_DLL="

if exist "%RIMTEST_ROOT%" (
    for /r "%RIMTEST_ROOT%" %%F in (RimTestRedux.dll) do (
        if not defined RIMTEST_DLL set "RIMTEST_DLL=%%~fF"
    )
)

if not defined RIMTEST_DLL (
    echo.
    echo [ERROR] RimTestRedux.dll was not found under:
    echo         %RIMTEST_ROOT%
    echo.
    echo Install/subscribe to RimTest Redux ^(Steam Workshop 3762405308^)
    echo before building the developer test mod.
    exit /b 1
)

if not exist "%MAIN_DLL%" (
    echo [ERROR] CCTO main assembly was not generated:
    echo         %MAIN_DLL%
    exit /b 1
)

set "OUTPUT_DIR=%TEST_MOD_DIR%\Assemblies"
set "ABOUT_DIR=%TEST_MOD_DIR%\About"
set "OUTPUT_DLL=%OUTPUT_DIR%\CropColdToleranceOverhaul.Tests.dll"

if not exist "%OUTPUT_DIR%" mkdir "%OUTPUT_DIR%"
if not exist "%ABOUT_DIR%" mkdir "%ABOUT_DIR%"

copy /Y "%ABOUT_SOURCE%" "%ABOUT_DIR%\About.xml" >nul
if errorlevel 1 (
    echo [ERROR] Failed to copy test mod About.xml.
    exit /b 1
)

set "SOURCES="
for /r "%SOURCE_DIR%" %%F in (*.cs) do (
    set "SOURCES=!SOURCES! "%%~fF""
)

echo.
echo [2/3] Building RimTest suite...
echo RimWorld : %RIMWORLD_DIR%
echo RimTest  : %RIMTEST_DLL%
echo Compiler : %CSC%
echo Output   : %OUTPUT_DLL%
echo.

if exist "%UNITY_CORE%" (
    "%CSC%" /nologo /target:library /optimize+ /out:"%OUTPUT_DLL%" ^
        /reference:"%ASSEMBLY_CSHARP%" ^
        /reference:"%UNITY_CORE%" ^
        /reference:"%MAIN_DLL%" ^
        /reference:"%RIMTEST_DLL%" ^
        !SOURCES!
) else (
    "%CSC%" /nologo /target:library /optimize+ /out:"%OUTPUT_DLL%" ^
        /reference:"%ASSEMBLY_CSHARP%" ^
        /reference:"%MAIN_DLL%" ^
        /reference:"%RIMTEST_DLL%" ^
        !SOURCES!
)

if errorlevel 1 (
    echo.
    echo [ERROR] Test build failed.
    exit /b 1
)

echo.
echo [3/3] Developer test mod is ready:
echo       %TEST_MOD_DIR%
echo.
echo Enable these mods for the in-game test run:
echo   - Harmony
echo   - ilyvion's Laboratory
echo   - RimTest Redux
echo   - Crop Cold Tolerance Overhaul
echo   - [DEV] Crop Cold Tolerance Overhaul Tests
echo.
echo [OK] Test assembly built successfully.
exit /b 0
