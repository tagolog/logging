@echo off
nant -buildfile:"%~dp0NAnt\Tagolog.NuGet.Pack.nant.build"
if %errorlevel% equ 0 goto success
pause
:success
