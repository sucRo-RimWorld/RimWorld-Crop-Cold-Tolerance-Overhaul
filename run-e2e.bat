@echo off
setlocal EnableExtensions

set "RIMWORLD_DIR=%~1"
if not defined RIMWORLD_DIR set "RIMWORLD_DIR=D:\SteamLibrary\steamapps\common\RimWorld"

set "ROOT=%~dp0"
set "RIMWORLD_EXE=%RIMWORLD_DIR%\RimWorldWin64.exe"
set "REPORT_DIR=%ROOT%TestResults\Pickle"

call "%ROOT%build-e2e.bat" "%RIMWORLD_DIR%"
if errorlevel 1 exit /b 1

if not exist "%RIMWORLD_EXE%" (
    echo [ERROR] RimWorld executable was not found:
    echo         %RIMWORLD_EXE%
    exit /b 1
)

echo.
echo Checking active development mod set...
powershell -NoProfile -ExecutionPolicy Bypass -File "%ROOT%Scripts\Check-TestModList.ps1"
if errorlevel 1 (
    echo.
    echo [STOP] Automated test run was not started.
    exit /b 1
)

if not exist "%REPORT_DIR%" mkdir "%REPORT_DIR%"

echo.
echo Running CCTO Pickle E2E suite...
echo Report: %REPORT_DIR%
echo.
echo IMPORTANT:
echo The required development mods must already be enabled in the active RimWorld mod list.
echo Pickle will generate a deterministic Quickstarts map for each scenario,
echo run the tests, write reports, and exit RimWorld automatically.
echo.

start /wait "" "%RIMWORLD_EXE%" ^
    -pickle-run="framework.feature,cold-tolerance.feature" ^
    -pickle-mode=fast ^
    -pickle-report-dir="%REPORT_DIR%" ^
    -pickle-no-browser ^
    -pickle-run-timeout=10

set "RESULT=%ERRORLEVEL%"

echo.
if "%RESULT%"=="0" (
    echo [OK] Pickle E2E suite passed.
) else if "%RESULT%"=="1" (
    echo [FAIL] One or more Pickle scenarios failed.
) else if "%RESULT%"=="2" (
    echo [ERROR] Pickle test runner failed or no scenarios were discovered.
) else (
    echo [ERROR] RimWorld/Pickle exited with code %RESULT%.
)

echo Report directory:
echo   %REPORT_DIR%
exit /b %RESULT%
