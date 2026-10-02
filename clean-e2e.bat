@echo off
setlocal EnableExtensions

set "RIMWORLD_DIR=%~1"
if not defined RIMWORLD_DIR set "RIMWORLD_DIR=D:\SteamLibrary\steamapps\common\RimWorld"

set "E2E_MOD_DIR=%~2"
if not defined E2E_MOD_DIR set "E2E_MOD_DIR=%RIMWORLD_DIR%\Mods\CropColdToleranceOverhaul.E2E"

if not exist "%E2E_MOD_DIR%" (
    echo [OK] CCTO E2E test mod is already absent:
    echo      %E2E_MOD_DIR%
    exit /b 0
)

rmdir /S /Q "%E2E_MOD_DIR%"
if errorlevel 1 (
    echo [ERROR] Failed to remove CCTO E2E test mod:
    echo         %E2E_MOD_DIR%
    exit /b 1
)

echo [OK] Removed CCTO E2E test mod:
echo      %E2E_MOD_DIR%
exit /b 0
