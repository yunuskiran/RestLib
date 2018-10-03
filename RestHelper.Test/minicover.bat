@echo off

dotnet clean
dotnet build /p:DebugType=Full
dotnet minicover instrument --workdir ../ --assemblies RestHelper.Test/bin/debug/**/*.dll --sources RestHelper/*.cs 

dotnet minicover reset --workdir ../

dotnet test --no-build
dotnet minicover uninstrument --workdir ../
dotnet minicover report --workdir ../ --threshold 70

set /p temp="Hit enter to continue"