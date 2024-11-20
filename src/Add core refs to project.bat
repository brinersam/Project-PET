set "mastername=ProjectPet"
set "projectname=Core"
set "bigpath=%mastername%.!subfolder!"
set "filepath=%projectname%/%bigpath%/%bigpath%.csproj"
set "cmd=call dotnet sln ../ProjectPet.sln add %filepath%"

SETLOCAL ENABLEDELAYEDEXPANSION

set "subfolder=Core"
%cmd%
set "subfolder=SharedKernel"
%cmd%
set "subfolder=Framework"
%cmd%

set "mastername="
set "projectname="
set "bigpath="
set "filepath="
set "cmd="
pause
