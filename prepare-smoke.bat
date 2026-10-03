@echo off
setlocal EnableExtensions

set "RIMWORLD_DIR=%~1"
if not defined RIMWORLD_DIR set "RIMWORLD_DIR=D:\SteamLibrary\steamapps\common\RimWorld"

set "ROOT=%~dp0"

echo Removing generated CCTO test mods...
call "%ROOT%clean-e2e.bat" "%RIMWORLD_DIR%"
if errorlevel 1 exit /b 1

call "%ROOT%clean-tests.bat" "%RIMWORLD_DIR%"
if errorlevel 1 exit /b 1

echo.
echo Building shipping CCTO assembly...
call "%ROOT%build.bat" "%RIMWORLD_DIR%"
if errorlevel 1 exit /b 1

echo.
echo Checking the normal RimWorld mod profile...
powershell -NoProfile -ExecutionPolicy Bypass -File "%ROOT%Scripts\Check-SmokeModList.ps1"
if errorlevel 1 exit /b 1

echo.
echo [OK] CCTO normal-game smoke environment is ready.
echo.
echo Launch RimWorld normally. Do not use the isolated E2E save-data profile.
echo Check the startup log for new CCTO errors, then inspect:
echo   Vanilla rice  - minimum growth 10 C, cold death -1 C
echo   Vanilla hops  - minimum growth 5 C, dormancy temperature 5 C
echo   MO wheat      - minimum growth 0 C, cold death -6 C
echo   MO apple tree - minimum growth 5 C, dormancy temperature 5 C
exit /b 0
