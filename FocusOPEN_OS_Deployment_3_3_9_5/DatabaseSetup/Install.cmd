@echo off

set FocusOPENDB_SQLSERVER=(LOCAL)



echo **************************************************************
echo FocusOPEN Database Setup
echo **************************************************************

echo.

echo Installing FocusOPEN Database to %FOCUSOPENDB_SQLSERVER%.  Press any key to continue...
pause > nul

echo.
echo.
echo.

echo **************************************************************
echo Step 1: Create Database
echo **************************************************************

echo.
echo.
echo.

osql -n -E -S %FocusOPENDB_SQLSERVER% -i 1_FocusOPEN_CreateDatabase.SQL

echo.
echo.
echo.

echo **************************************************************
echo Step 2: Create Schema
echo **************************************************************

echo.
echo.
echo.

osql -n -E -d FocusOPEN -S %FocusOPENDB_SQLSERVER% -i 2_FocusOPEN_CreateSchema.SQL

echo.
echo.
echo.

echo **************************************************************
echo Step 3: Insert Data
echo **************************************************************

echo.
echo.
echo.

osql -n -E -d FocusOPEN -S %FocusOPENDB_SQLSERVER% -i 3_FocusOPEN_InsertData.SQL

echo.
echo.
echo.

echo **************************************************************
echo.
echo.

Echo Done. Press any key to exit...

set FocusOPENDB_SQLSERVER=

pause > nul