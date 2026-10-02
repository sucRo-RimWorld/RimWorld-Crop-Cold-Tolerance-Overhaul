@echo off
setlocal EnableExtensions

set "RIMWORLD_DIR=%~1"
if not defined RIMWORLD_DIR set "RIMWORLD_DIR=D:\SteamLibrary\steamapps\common\RimWorld"

set "STEAMAPPS=%RIMWORLD_DIR%\..\.."
set "WORKSHOP_ROOT=%STEAMAPPS%\workshop\content\294100"
set "MO_ROOT=%WORKSHOP_ROOT%\3219596926"
set "VEF_ROOT=%WORKSHOP_ROOT%\2023507013"
set "PROCESSOR_ROOT=%WORKSHOP_ROOT%\3210544395"

echo Checking Medieval Overhaul integration dependencies...
if not exist "%MO_ROOT%\About\About.xml" (
    echo [ERROR] Medieval Overhaul was not found:
    echo         %MO_ROOT%
    exit /b 1
)
if not exist "%VEF_ROOT%\About\About.xml" (
    echo [ERROR] Vanilla Expanded Framework was not found:
    echo         %VEF_ROOT%
    exit /b 1
)
if not exist "%PROCESSOR_ROOT%\About\About.xml" (
    echo [ERROR] [SYR] Processor Framework was not found:
    echo         %PROCESSOR_ROOT%
    exit /b 1
)
echo [OK] Medieval Overhaul and required dependencies are installed.

echo.
echo Validating CCTO balance XML and installed MO source DefNames...
call "%~dp0validate-balance.bat" "%MO_ROOT%"
if errorlevel 1 exit /b 1

echo.
echo Running full framework + Vanilla/MO balance integration gate...
call "%~dp0run-e2e.bat" "%RIMWORLD_DIR%" with-mo
exit /b %ERRORLEVEL%
