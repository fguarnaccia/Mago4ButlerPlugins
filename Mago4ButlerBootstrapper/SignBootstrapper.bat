@REM %1 Bootstrapper file full path
@REM %2 Certificate file full path

"%WIX%bin\insignia.exe" -ib %1 -o %TEMP%\engine.exe
call C:\DigitalSign.bat %2 %TEMP%\engine.exe
"%WIX%bin\insignia.exe"  -ab %TEMP%\engine.exe %1 -o %1
call C:\DigitalSign.bat %2 %1
del %TEMP%\engine.exe