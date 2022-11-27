@echo off

echo Cleaning up files and folders...

echo.
echo.
echo.

if exist *.zip del *.zip > nul
echo Deleted zip files


echo.
echo.
echo.
echo.


echo Deleting DEBUG folder...
:CHECK_DEBUG_DIR
if exist DEBUG goto DELETE_DEBUG_DIR
echo Done

echo.
echo.
echo.
echo.


echo Deleting RELEASE folder...
:CHECK_RELEASE_DIR
if exist RELEASE goto DELETE_RELEASE_DIR
echo Done

goto END


:DELETE_DEBUG_DIR
echo.
rmdir DEBUG /s /q > nul
goto CHECK_DEBUG_DIR


:DELETE_RELEASE_DIR
echo.
rmdir RELEASE /s /q > nul
goto CHECK_RELEASE_DIR


:END

echo.
echo.
echo.
echo.

echo Cleanup Complete.  All done... About to quit.

echo.
echo.
echo.

timeout /t:5