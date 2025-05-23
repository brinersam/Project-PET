set "mastername=ProjectPet"
set "projectname=DiscussionsModule"

set "solutionPath=../ProjectPet.sln"
set "filepath=%mastername%.%projectname%.!subfolder!"

set "cmdCreateLib=call dotnet new classlib -n %filepath% -o %projectname%/%filepath%"
set "cmdLinkLib=call dotnet sln %solutionPath% add %projectname%/%filepath%/%filepath%.csproj"

SETLOCAL ENABLEDELAYEDEXPANSION

set "subfolder=Contracts"
%cmdCreateLib%
%cmdLinkLib%
set "subfolder=Infrastructure"
%cmdCreateLib%
%cmdLinkLib%
set "subfolder=Presentation"
%cmdCreateLib%
%cmdLinkLib%
set "subfolder=Domain"
%cmdCreateLib%
%cmdLinkLib%
set "subfolder=Application"
%cmdCreateLib%
%cmdLinkLib%

set "mastername="
set "projectname="

set "solutionPath="
set "filepath="

set "cmdCreateLib="
set "cmdLinkLib="
pause

