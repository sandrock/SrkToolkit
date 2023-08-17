
Releasing
================

1. Evaluate next version identifier.
2. Update release notes for each project
3. Change all CSPROJ files to use the new version number (`<Version>2\.\d+\.(\d+)-preview1+</Version>` -> `<Version>2.0.666-preview2</Version>`)
4. Change all CSPROJ files around `<FileVersion>2.\d+.\d+.0</FileVersion>`
5. Update the `SrkToolkit.Mvvm.AssemblyInfo.cs` file accordingly. (???) 
6. Build and run unit tests
4. Commit, if everything OK
5. Build nugets    
```bash
dotnet build Sources/SrkToolkit-v2.sln -c Release -v q
```
7. publish nugets   
```batch
find . -wholename '*/Release/*2.0.147-*.nupkg' \
     -exec dotnet nuget push "{}" -s https://api.nuget.org/v3/index.json --api-key XXX \;
```



