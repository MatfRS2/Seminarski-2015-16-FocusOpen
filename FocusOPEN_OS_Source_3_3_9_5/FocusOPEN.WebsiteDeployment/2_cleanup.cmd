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


echo Deleting debug folder...
:CHECK_DEBUG_DIR
if exist debug goto DELETE_DEBUG_DIR
echo Done

echo.
echo.
echo.
echo.


echo Deleting release folder...
:CHECK_RELEASE_DIR
if exist release goto DELETE_RELEASE_DIR
echo Done

echo.
echo.
echo.
echo.


echo Deleting tempbuilddir folder...
:CHECK_TEMPBUILDDIR_DIR
if exist tempbuilddir goto DELETE_TEMPBUILDDIR_DIR
echo Done

goto END

:DELETE_DEBUG_DIR
echo.
rmdir Debug /s /q > nul
goto CHECK_DEBUG_DIR


:DELETE_RELEASE_DIR
echo.
rmdir Release /s /q > nul
goto CHECK_RELEASE_DIR

:DELETE_TEMPBUILDDIR_DIR
echo.
rmdir TempBuildDir /s /q > nul
goto CHECK_TEMPBUILDDIR_DIR


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