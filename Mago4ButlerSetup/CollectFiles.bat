@REM http://wixtoolset.org/documentation/manual/v3/overview/heat.html
@REM %1 configuration (Debug or Release)
@REM %2 solution folder
@REM %3 platform

ECHO Configuration = %1
ECHO SolutionDir = %2
ECHO Platform = %3
ECHO source directory = "%2Mago4Butler\bin\%3\%1"
ECHO output file = "%2Mago4ButlerSetup\Files.wxs"
ECHO transform file = "%2Mago4ButlerSetup\Transform.xslt"
"%WIX%bin\heat.exe" dir "%2Mago4Butler\bin\%3\%1" -configuration %1 -platform %3 -cg ProductComponents -dr INSTALLLOCATION -var var.M4BSourceDir -gg -g1 -sfrag -scom -sreg -srd -template fragment -t "%2Mago4ButlerSetup\Transform.xslt" -out "%2Mago4ButlerSetup\Files.wxs"