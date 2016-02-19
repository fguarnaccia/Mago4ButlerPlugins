@REM http://wixtoolset.org/documentation/manual/v3/overview/heat.html
@REM %1 configuration (Debug or Release)
@REM %2 solution folder

ECHO Configuration = %1
ECHO SolutionDir = %2
ECHO source directory = "%2Mago4Butler\bin\x86\%1"
ECHO output file = "%2Mago4ButlerSetup\Files.wxs"
ECHO transform file = "%2\Transform.xslt"
"%WIX%bin\heat.exe" dir "%2Mago4Butler\bin\x86\%1" -cg ProductComponents -dr INSTALLFOLDER -var var.SourceDir -gg -g1 -sfrag -scom -sreg -template fragment -t "%2Mago4ButlerSetup\Transform.xslt" -out "%2Mago4ButlerSetup\Files.wxs"