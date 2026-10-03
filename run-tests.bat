@echo off
setlocal EnableExtensions

set "RIMWORLD_DIR=%~1"
if not defined RIMWORLD_DIR set "RIMWORLD_DIR=D:\SteamLibrary\steamapps\common\RimWorld"

set "STEAMAPPS=%RIMWORLD_DIR%\..\.."
set "WORKSHOP_ROOT=%STEAMAPPS%\workshop\content\294100"
set "MO_ROOT=%WORKSHOP_ROOT%\3219596926"

echo Checking installed Medieval Overhaul source...
if not exist "%MO_ROOT%\About\About.xml" (
    echo [ERROR] Medieval Overhaul was not found:
    echo         %MO_ROOT%
    exit /b 1
)
echo [OK] Medieval Overhaul source is installed.

echo.
echo Validating CCTO balance XML against installed MO 1.6 plant Defs...
call "%~dp0validate-balance.bat" "%MO_ROOT%"
if errorlevel 1 exit /b 1

echo.
echo Running framework + Vanilla/MO balance integration gate...
echo Runtime MO checks use a lightweight Def fixture so unrelated MO Harmony startup
echo behavior cannot contaminate CCTO balance verification.
call "%~dp0run-e2e.bat" "%RIMWORLD_DIR%" with-mo-fixture
exit /b %ERRORLEVEL%
