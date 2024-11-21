set "mastername=ProjectPet"

set "solutionPath=../ProjectPet.sln"
set "filepath=%mastername%.!subfolder!"

set "cmdCreateLib=call dotnet new classlib -n %filepath% -o Core/%filepath%"
set "cmdLinkLib=call dotnet sln %solutionPath% add Core/%filepath%/%filepath%.csproj"

SETLOCAL ENABLEDELAYEDEXPANSION

set "subfolder=Core"
%cmdCreateLib%
%cmdLinkLib%
set "subfolder=Framework"
%cmdCreateLib%
%cmdLinkLib%
set "subfolder=SharedKernel"
%cmdCreateLib%
%cmdLinkLib%

set "mastername="

set "solutionPath="
set "filepath="

set "cmdCreateLib="
set "cmdLinkLib="
pause
