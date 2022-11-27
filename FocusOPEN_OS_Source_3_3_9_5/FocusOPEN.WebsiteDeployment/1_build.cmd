@echo off

set MSBUILDPATH=C:\WINDOWS\Microsoft.NET\Framework\v3.5\msbuild.exe
set PROJECT_BUILD_CONFIG=
set DEPLOY_BUILD_CONFIG=

echo *******************************************************************
echo * FocusOPEN Build Script                                          *
echo *******************************************************************

echo.
echo.
echo.
echo.

echo What type of build should be created:
echo.
echo 1) Debug (default)
echo 2) Release
echo.
choice /C 123 /N /T:7 /D 1 /M "Please choose an option: "

echo.
echo.
echo.
echo.

if errorlevel == 2 set DEPLOY_BUILD_CONFIG=Release
if errorlevel == 1 set DEPLOY_BUILD_CONFIG=Debug

if %DEPLOY_BUILD_CONFIG%==Release set PROJECT_BUILD_CONFIG=Release
if %DEPLOY_BUILD_CONFIG%==Debug set PROJECT_BUILD_CONFIG=Debug

echo Using project configuration: %PROJECT_BUILD_CONFIG%, deploy configuration: %DEPLOY_BUILD_CONFIG%

timeout /t:5

echo.
echo.
echo.
echo.

echo Cleaning up...
Call 2_cleanup.cmd
echo Done

echo.
echo.
echo.
echo.

echo About to build...
timeout /t:5

echo.
echo.
echo.
echo.

echo Building website
%MSBUILDPATH% ..\FocusOPEN.Website\FocusOPEN.Website.csproj /t:Rebuild /p:Configuration=%PROJECT_BUILD_CONFIG%
echo Done building website

echo.
echo.
echo.
echo.

echo Building web deployment...
%MSBUILDPATH% .\FocusOPEN.WebsiteDeployment.wdproj /t:Rebuild /p:Configuration=%DEPLOY_BUILD_CONFIG%
echo Done building web deployment

echo.
echo.
echo.
echo.

echo Finished building. Waiting 3 seconds...
timeout /t:5

echo.
echo.

set MSBUILDPATH=
set PROJECT_BUILD_CONFIG=
set DEPLOY_BUILD_CONFIG=