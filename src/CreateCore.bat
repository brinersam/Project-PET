set "mastername=ProjectPet"
set "projectname=Core"
set "filepath=%mastername%.!subfolder!"
set "cmd=call dotnet new classlib -n %filepath% -o %projectname%\%filepath%"

SETLOCAL ENABLEDELAYEDEXPANSION

set "subfolder=Core"
%cmd%
set "subfolder=Framework"
%cmd%
set "subfolder=SharedKernel"
%cmd%
set "subfolder=Domain"

set "mastername="
set "projectname="
set "filepath="
set "cmd="
pause
