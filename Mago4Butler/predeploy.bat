if %1 == Deploy (GOTO Deploy)
if %1 == Debug (GOTO Debug)
GOTO end


:Deploy
"C:\Users\guarnaccia\Documents\Visual Studio 2015\Projects\InternalTools\Mago4Butler\VersionManager\bin\Release\VersionManager.exe" %1 
GOTO end

:Debug
GOTO end

:end



