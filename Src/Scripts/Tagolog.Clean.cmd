@echo off
nant -buildfile:"%~dp0NAnt\Tagolog.Clean.nant.build"
if %errorlevel% equ 0 goto success
pause
:success
