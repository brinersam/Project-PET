set "mastername=ProjectPet"
set "projectname=Species"
set "filepath=%mastername%.%projectname%.!subfolder!"
set "cmd=call dotnet new classlib -n %filepath% -o %projectname%\%filepath%"

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
set "filepath="
set "cmd="
pause

