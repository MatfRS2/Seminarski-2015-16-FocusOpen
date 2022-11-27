@echo off

net stop FocusOPENAPS

echo.
echo.
echo.
echo.
echo.
echo.

c:\Windows\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe ".\bin\Debug\FocusOPEN.APS.WinService.exe" /u

echo.
echo.
echo.
echo.
echo.
echo.

timeout /t:5