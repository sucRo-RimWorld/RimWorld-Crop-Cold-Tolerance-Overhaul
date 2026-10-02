@echo off
setlocal EnableExtensions

set "MO_ROOT=%~1"

if defined MO_ROOT (
  powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0Scripts\Validate-BalanceXml.ps1" -RepoRoot "%~dp0" -MedievalOverhaulRoot "%MO_ROOT%"
) else (
  powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0Scripts\Validate-BalanceXml.ps1" -RepoRoot "%~dp0"
)

exit /b %ERRORLEVEL%
