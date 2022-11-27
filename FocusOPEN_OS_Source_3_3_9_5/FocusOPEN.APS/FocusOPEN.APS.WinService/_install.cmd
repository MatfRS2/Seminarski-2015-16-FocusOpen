@echo off

c:\Windows\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe ".\bin\Debug\FocusOPEN.APS.WinService.exe"

echo.
echo.
echo.
echo.
echo.
echo.

net start FocusOPENAPS

echo.
echo.
echo.
echo.
echo.
echo.

timeout /t:5