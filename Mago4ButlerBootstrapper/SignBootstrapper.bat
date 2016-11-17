@REM %1 Bootstrapper file full path
@REM %2 Certificate file full path

"%WIX%bin\insignia.exe" -ib %1 -o %TEMP%\engine.exe
@REM call DigitalSign.bat %2 %TEMP%\engine.exe
call ..\DigitalSignerClient\bin\Release\Microarea.DigitalSignerClient.exe %TEMP%\engine.exe
"%WIX%bin\insignia.exe"  -ab %TEMP%\engine.exe %1 -o %1
@REM call DigitalSign.bat %2 %1
call ..\DigitalSignerClient\bin\Release\Microarea.DigitalSignerClient.exe %1
@REM del %TEMP%\engine.exe