@echo off
setlocal EnableExtensions

set "RIMWORLD_DIR=%~1"
if not defined RIMWORLD_DIR set "RIMWORLD_DIR=D:\SteamLibrary\steamapps\common\RimWorld"

set "TEST_MOD_DIR=%~2"
if not defined TEST_MOD_DIR set "TEST_MOD_DIR=%RIMWORLD_DIR%\Mods\CropColdToleranceOverhaul.Tests"

if not exist "%TEST_MOD_DIR%" (
    echo [OK] Developer test mod is already absent:
    echo      %TEST_MOD_DIR%
    exit /b 0
)

rmdir /S /Q "%TEST_MOD_DIR%"
if errorlevel 1 (
    echo [ERROR] Failed to remove developer test mod:
    echo         %TEST_MOD_DIR%
    exit /b 1
)

echo [OK] Removed developer test mod:
echo      %TEST_MOD_DIR%
exit /b 0
