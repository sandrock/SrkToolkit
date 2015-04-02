@echo off

set msbuild4=%windir%\Microsoft.NET\Framework\v4.0.30319\msbuild
set winrar="%ProgramFiles%\winrar\winrar.exe"
set outdir=%cd%\Binaries
set outdirNet4=%cd%\Binaries\NET4

echo msbuild4:   %msbuild4%
echo winrar:     %winrar%
echo outdir:     %outdir%
echo outdirNet4: %outdirNet4%
echo 

@mkdir %outdir% 2>NUL

cd Sources

%msbuild4% SrkToolkit.sln /nologo /t:ApplicationServices\NET4.SrkToolkit.Services /property:OutputDir="%outdirNet4%" /property:OutputPath="%outdirNet4%" /property:Configuration=Release /verbosity:m

cd ..
