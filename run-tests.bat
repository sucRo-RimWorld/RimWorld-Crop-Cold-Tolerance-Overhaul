@echo off
setlocal EnableExtensions

set "RIMWORLD_DIR=%~1"
if not defined RIMWORLD_DIR set "RIMWORLD_DIR=D:\SteamLibrary\steamapps\common\RimWorld"

call "%~dp0run-e2e.bat" "%RIMWORLD_DIR%"
exit /b %ERRORLEVEL%
