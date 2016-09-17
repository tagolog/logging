@echo off
nant -buildfile:"%~dp0NAnt\Tagolog.Rebuild.nant.build"
if %errorlevel% equ 0 goto success
pause
:success
