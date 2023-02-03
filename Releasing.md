
Evaluate next version identifier by running the build app

```batch
cd Tools
.\build.bat
```

Update the `SrkToolkit.Mvvm.AssemblyInfo.cs` file accordingly. 

Run the build script again.

then:

```batch
cd Package
..\tools\nuget.exe push -src https://api.nuget.org/v3/index.json .\SrkToolkit.*.1.2.140.nupkg -apikey xxx
```



