@echo off
setlocal EnableExtensions

set "RIMWORLD_DIR=%~1"
if not defined RIMWORLD_DIR set "RIMWORLD_DIR=D:\SteamLibrary\steamapps\common\RimWorld"

echo Validating CCTO balance XML...
call "%~dp0validate-balance.bat"
if errorlevel 1 exit /b 1

echo.
call "%~dp0run-e2e.bat" "%RIMWORLD_DIR%"
exit /b %ERRORLEVEL%
