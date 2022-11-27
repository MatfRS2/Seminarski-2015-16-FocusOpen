@echo off

set MSBUILDPATH=C:\WINDOWS\Microsoft.NET\Framework\v3.5\msbuild.exe
set BUILD_CONFIG=Release

echo *******************************************************************
echo FocusOPEN APS WinService Build script
echo Config: %BUILD_CONFIG%
echo *******************************************************************

echo.
echo.
echo.

echo What type of build should be created:
echo.
echo 1) x86 (default)
echo 2) x64
echo.
choice /C 12 /N /T:7 /D 1 /M "Please choose an option: "

if errorlevel == 2 set PLATFORM=x64
if errorlevel == 1 set PLATFORM=x86

echo.
echo.
echo.

echo Building for platform: %PLATFORM%

echo.
echo.
echo.

Call _uninstall.cmd

echo.
echo.
echo.
echo.

if exist *.zip del *.zip > nul
echo Deleted zip files

echo.
echo.
echo.
echo.

echo Deleting bin folder...
:CHECK_BIN_DIR
if exist bin goto DELETE_BIN_DIR
echo Done

echo.
echo.
echo.
echo.

echo Deleting obj folder...
:CHECK_OBJ_DIR
if exist obj goto DELETE_OBJ_DIR
echo Done

echo.
echo.
echo.
echo.

echo Building in 5 seconds...
timeout /t:5

echo.
echo.
echo.
echo.

echo Building app...
for %%f in (*.csproj) do %MSBUILDPATH% %%f /t:Rebuild /p:Configuration=%BUILD_CONFIG%;Platform=%PLATFORM%
echo Done

echo.
echo.
echo.
echo.

choice /C NY /T 5 /D Y /M "Do you want to install the service? "

if errorlevel 2 call _install.cmd

echo.
echo.
echo.
echo.

goto END

:DELETE_BIN_DIR
echo.
rmdir bin /s /q > nul
goto CHECK_BIN_DIR

:DELETE_OBJ_DIR
echo.
rmdir obj /s /q > nul
goto CHECK_OBJ_DIR

:END
set MSBUILDPATH=
set BUILD_CONFIG=
set PLATFORM=

echo All done. Quitting...
timeout /t:5