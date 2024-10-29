echo off
echo;
echo ======= Dropping database... =======
echo;
call DropDatabase.bat
cd "%~dp0"
echo;
echo ======= Database dropped! =======
echo;
echo ======= Wiping migrations... =======
echo;
call WipeMigrations.bat
cd "%~dp0"
echo;
echo ======= Migrations wiped! =======
echo;
echo ======= Creating initial migration... =======
echo;
call CreateMigrations.bat
cd "%~dp0"
echo;
echo ======= Initial migration created! =======
echo;
echo ======= Applying migration to database... =======
echo;
call UpdateDatabase.bat
cd "%~dp0"
echo;
echo ======= Database updated! =======
echo;
echo ======= All set! =======
PAUSE