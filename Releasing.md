
Releasing
================

1. Evaluate next version.
2. Change all CSPROJ files to use the new version number (`2\.0\.(\d+)-preview1+` -> `2.0.666-preview2`)
3. Build
4. Commit
5. Build
6. Update the `SrkToolkit.Mvvm.AssemblyInfo.cs` file accordingly. (???) 
7. publish   
```batch
find . -wholename '*/Release/*2.0.147-*.nupkg' \
     -exec dotnet nuget push "{}" -s https://api.nuget.org/v3/index.json --api-key XXX \;
```



