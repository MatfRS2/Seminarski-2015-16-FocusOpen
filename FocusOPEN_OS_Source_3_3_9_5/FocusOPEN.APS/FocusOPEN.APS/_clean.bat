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


echo Deleting BIN folder...
:CHECK_BIN_DIR
if exist BIN goto DELETE_BIN_DIR
echo Done

echo.
echo.
echo.
echo.


echo Deleting OBJ folder...
:CHECK_OBJ_DIR
if exist OBJ goto DELETE_OBJ_DIR
echo Done

goto END


:DELETE_BIN_DIR
echo.
rmdir BIN /s /q > nul
goto CHECK_BIN_DIR


:DELETE_OBJ_DIR
echo.
rmdir OBJ /s /q > nul
goto CHECK_OBJ_DIR


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