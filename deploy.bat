if %1 == Deploy (GOTO Deploy)
if %1 == Debug (GOTO Debug)
GOTO end


:Deploy
copy /Y %2 "\\SRV-BKS\Download\Tools Microarea\Mago4ButlerUpdates\%3"
GOTO end

:Debug
copy /Y %2 "C:\Users\guarnaccia\Documents\Visual Studio 2015\Projects\InternalTools\Mago4Butler\Mago4Butler\bin\x86\RELEASE\%3"
GOTO end

:end



