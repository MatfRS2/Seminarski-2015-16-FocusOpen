@echo off

SET MSBUILDPATH=C:\WINDOWS\Microsoft.NET\Framework\v3.5\msbuild.exe

echo *******************************************************************
echo FocusOPEN APS Console App Build script
echo *******************************************************************

echo.
echo.
echo.

echo What type of build config should be created:
echo.
echo 1) Release (default)
echo 2) Debug
echo.
choice /C 12 /N /T:5 /D 1 /M "Please choose an option: "

if errorlevel == 2 set BUILD_CONFIG=Debug
if errorlevel == 1 set BUILD_CONFIG=Release

echo.
echo.
echo.

echo What type of build should be created:
echo.
echo 1) x86 (default)
echo 2) x64
echo.
choice /C 12 /N /T:5 /D 1 /M "Please choose an option: "

if errorlevel == 2 set PLATFORM=x64
if errorlevel == 1 set PLATFORM=x86

echo.
echo.
echo.

echo Building for platform: %PLATFORM%, build config: %BUILD_CONFIG%

timeout /t 3 > nul

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
set VSPATH=
set BUILD_CONFIG=
set PLATFORM=

echo All done. Quitting...
timeout /t:5