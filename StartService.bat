@echo off
REM change the directory to batch file directory
cd "%~dp0"/BestStories/

REM run the dotnet run command to load the service
dotnet run

start cmd