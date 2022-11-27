@echo off

SET MSBUILDPATH=C:\WINDOWS\Microsoft.NET\Framework\v3.5\msbuild.exe
SET BUILD_CONFIG=Release

echo *******************************************************************
echo Build script.  Config: %BUILD_CONFIG%
echo *******************************************************************

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
for %%f in (*.csproj) do %MSBUILDPATH% %%f /t:Rebuild /p:Configuration=%BUILD_CONFIG%
echo Done

echo.
echo.
echo.
echo.

echo Creating zip
cd bin\
"C:\Program Files\WinZip\wzzip.exe" -p -r FocusOPEN_CategoryCountUpdater.zip %BUILD_CONFIG%/
move FocusOPEN_CategoryCountUpdater.zip ..\
ECHO Done zipping

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
echo All done. Quitting...
timeout /T:5