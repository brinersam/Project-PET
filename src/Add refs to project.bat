set "mastername=ProjectPet"
set "projectname=Pets"
set "bigpath=%mastername%.%projectname%.!subfolder!"
set "filepath=%projectname%/%bigpath%/%bigpath%.csproj"
set "cmd=call dotnet sln ../ProjectPet.sln add %filepath%"

SETLOCAL ENABLEDELAYEDEXPANSION

set "subfolder=Contracts"
%cmd%
set "subfolder=Infrastructure"
%cmd%
set "subfolder=Presentation"
%cmd%
set "subfolder=Domain"
%cmd%
set "subfolder=Application"
%cmd%

set "mastername="
set "projectname="
set "bigpath="
set "filepath="
set "cmd="
pause
