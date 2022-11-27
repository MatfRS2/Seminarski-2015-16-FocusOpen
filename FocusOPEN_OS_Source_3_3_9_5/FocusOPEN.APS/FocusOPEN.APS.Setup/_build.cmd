@echo off

set MSBUILDPATH=C:\WINDOWS\Microsoft.NET\Framework\v3.5\msbuild.exe
set BUILD_CONFIG=Release
set VSPATH=C:\Program Files (x86)\Microsoft Visual Studio 9.0\Common7\IDE\devenv.exe

for /f "tokens=2-4 delims=/ " %%a in ('date /t') do (set CURRENT_DATE=%%c-%%a-%%b)

echo *******************************************************************
echo FocusOPEN APS Setup Build script
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

echo Deleting installer dir...
rmdir .\Installer /s /q
echo Done

echo Deleting debug dir...
rmdir .\Debug /s /q
echo Done

echo.
echo.

echo Deleting release dir...
rmdir .\Release /s /q
echo Done

echo.
echo.

echo Building APS...
pushd ..\FocusOPEN.APS\
%MSBUILDPATH% FocusOPEN.APS.csproj /t:Rebuild /p:Configuration=%BUILD_CONFIG%;Platform=%PLATFORM%
popd
echo Done

echo.
echo.

echo Building WinService...
pushd ..\FocusOPEN.APS.WinService\
%MSBUILDPATH% FocusOPEN.APS.WinService.csproj /t:Rebuild /p:Configuration=%BUILD_CONFIG%;Platform=%PLATFORM%
popd
echo Done

echo.
echo.

echo Incrementing version...
cscript _incrementversion.vbs FocusOPEN.APS.Setup.vdproj
del FocusOPEN.APS.Setup.vdproj.bak
echo Done

echo.
echo.

echo Building Installer...
"%VSPATH%" ..\Solution\FocusOPEN.APS.sln /build %BUILD_CONFIG% /project FocusOPEN.APS.Setup
echo Done

echo.
echo.

mkdir Installer

pushd .\%BUILD_CONFIG%
move *.* ..\Installer
popd

pushd .\Installer
echo Version: %PLATFORM%, Build: %BUILD_CONFIG%.  Date: %CURRENT_DATE% > APSBuildInfo.txt
popd

rmdir .\Debug /s /q
rmdir .\Release /s /q

echo.
echo.

set CURRENT_DATE=
set MSBUILDPATH=
set VSPATH=
set BUILD_CONFIG=
set PLATFORM=

echo All Done.  Quitting in 3 seconds...

timeout /t:3 > nul