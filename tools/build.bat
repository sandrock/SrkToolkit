
@ECHO OFF

echo:
echo ==========================
echo SrkToolkit package builder
echo ==========================
echo:

set currentDirectory=%CD%
cd ..
cd Package
set outputDirectory=%CD%
cd %currentDirectory%
set nuget=%CD%\..\tools\nuget.exe
set msbuild4="%windir%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"
set vincrement=%CD%\..\tools\Vincrement.exe


echo Check CLI apps
echo -----------------------------

if not exist %nuget% (
 echo ERROR: nuget could not be found, verify path. exiting.
 echo Configured as: %nuget%
 pause
 goto end
)

if not exist %msbuild4% (
 echo ERROR: msbuild 4 could not be found, verify path. exiting.
 echo Configured as: %msbuild4%
 pause
 goto end
)

if not exist %vincrement% (
 echo ERROR: vincrement could not be found, verify path. exiting.
 echo Configured as: %vincrement%
 pause
 goto end
)

echo Everything is fine.

REM echo:
REM echo Clean output directory
REM echo -----------------------------
REM cd ..
REM if exist lib (
REM  rmdir /s /q lib
REM  if not %ERRORLEVEL% == 0 (
REM   echo ERROR: clean failed. exiting.
REM   pause
REM   goto end
REM  )
REM )
REM echo Done.

pause

echo:
echo Build solution
echo -----------------------------
cd ..
cd Sources
set solutionDirectory=%CD%
REM %msbuild4% "SrkToolkit - VS11.sln" /p:Configuration=Release /nologo /verbosity:q
REM 
REM if not %ERRORLEVEL% == 0 (
REM  echo ERROR: build failed. exiting.
REM  cd %currentDirectory%
REM  pause
REM  goto end
REM )
REM echo Done.

echo Please build the solution in RELEASE configuration.
pause

echo:
echo Copy libs
echo -----------------------------
cd %currentDirectory%
cd ..

mkdir %outputDirectory%\SrkToolkit.Services
mkdir %outputDirectory%\SrkToolkit.Services\lib
mkdir %outputDirectory%\SrkToolkit.Services\lib\net40
xcopy /Q /Y Binaries\NET4\SrkToolkit.Services.dll %outputDirectory%\SrkToolkit.Services\lib\net40
xcopy /Q /Y Binaries\NET4\SrkToolkit.Services.xml %outputDirectory%\SrkToolkit.Services\lib\net40

mkdir %outputDirectory%\SrkToolkit.Common
mkdir %outputDirectory%\SrkToolkit.Common\lib
mkdir %outputDirectory%\SrkToolkit.Common\lib\net40
xcopy /Q /Y Binaries\NET4\SrkToolkit.Common.dll %outputDirectory%\SrkToolkit.Common\lib\net40
xcopy /Q /Y Binaries\NET4\SrkToolkit.Common.xml %outputDirectory%\SrkToolkit.Common\lib\net40
mkdir %outputDirectory%\SrkToolkit.Common\lib\net40\fr
xcopy /Q /Y Binaries\NET4\fr\SrkToolkit.Common.* %outputDirectory%\SrkToolkit.Common\lib\net40\fr
mkdir %outputDirectory%\SrkToolkit.Common\lib\net45
xcopy /Q /Y Binaries\NET45\SrkToolkit.Common.dll %outputDirectory%\SrkToolkit.Common\lib\net45
xcopy /Q /Y Binaries\NET45\SrkToolkit.Common.xml %outputDirectory%\SrkToolkit.Common\lib\net45
mkdir %outputDirectory%\SrkToolkit.Common\lib\net45\fr
xcopy /Q /Y Binaries\NET45\fr\SrkToolkit.Common.* %outputDirectory%\SrkToolkit.Common\lib\net45\fr
mkdir %outputDirectory%\SrkToolkit.Common\lib\netstandard2.0
xcopy /Q /Y Sources\NSTD.SrkToolkit.Common\bin\Release\netstandard2.0\SrkToolkit.Common.dll %outputDirectory%\SrkToolkit.Common\lib\netstandard2.0
xcopy /Q /Y Sources\NSTD.SrkToolkit.Common\bin\Release\netstandard2.0\SrkToolkit.Common.xml %outputDirectory%\SrkToolkit.Common\lib\netstandard2.0
xcopy /Q /Y Sources\NSTD.SrkToolkit.Common\bin\Release\netstandard2.0\SrkToolkit.Common.deps.json %outputDirectory%\SrkToolkit.Common\lib\netstandard2.0
mkdir %outputDirectory%\SrkToolkit.Common\lib\netstandard2.0\fr
xcopy /Q /Y Sources\NSTD.SrkToolkit.Common\bin\Release\netstandard2.0\fr\SrkToolkit.Common.* %outputDirectory%\SrkToolkit.Common\lib\netstandard2.0\fr

mkdir %outputDirectory%\SrkToolkit.Web.AspMvc4\
mkdir %outputDirectory%\SrkToolkit.Web.AspMvc4\lib
mkdir %outputDirectory%\SrkToolkit.Web.AspMvc4\lib\net45
xcopy /Q /Y Binaries\NET45\SrkToolkit.Web.Mvc4.dll %outputDirectory%\SrkToolkit.Web.AspMvc4\lib\net45
xcopy /Q /Y Binaries\NET45\SrkToolkit.Web.Mvc4.xml %outputDirectory%\SrkToolkit.Web.AspMvc4\lib\net45

mkdir %outputDirectory%\SrkToolkit.Web.AspMvc5\
mkdir %outputDirectory%\SrkToolkit.Web.AspMvc5\lib
mkdir %outputDirectory%\SrkToolkit.Web.AspMvc5\lib\net45
xcopy /Q /Y Binaries\NET45\SrkToolkit.Web.Mvc5.dll %outputDirectory%\SrkToolkit.Web.AspMvc5\lib\net45
xcopy /Q /Y Binaries\NET45\SrkToolkit.Web.Mvc5.xml %outputDirectory%\SrkToolkit.Web.AspMvc5\lib\net45

mkdir %outputDirectory%\SrkToolkit.Domain\
mkdir %outputDirectory%\SrkToolkit.Domain\lib
mkdir %outputDirectory%\SrkToolkit.Domain\lib\net40
xcopy /Q /Y Binaries\NET4\SrkToolkit.Domain.dll %outputDirectory%\SrkToolkit.Domain\lib\net40
xcopy /Q /Y Binaries\NET4\SrkToolkit.Domain.xml %outputDirectory%\SrkToolkit.Domain\lib\net40
mkdir %outputDirectory%\SrkToolkit.Domain\lib\net45
xcopy /Q /Y Binaries\NET45\SrkToolkit.Domain.dll %outputDirectory%\SrkToolkit.Domain\lib\net45
xcopy /Q /Y Binaries\NET45\SrkToolkit.Domain.xml %outputDirectory%\SrkToolkit.Domain\lib\net45
xcopy /Q /Y Sources\NSTD.SrkToolkit.Domain\bin\Release\netstandard2.0\SrkToolkit.Domain.dll %outputDirectory%\SrkToolkit.Domain\lib\netstandard2.0
xcopy /Q /Y Sources\NSTD.SrkToolkit.Domain\bin\Release\netstandard2.0\SrkToolkit.Domain.xml %outputDirectory%\SrkToolkit.Domain\lib\netstandard2.0
xcopy /Q /Y Sources\NSTD.SrkToolkit.Domain\bin\Release\netstandard2.0\SrkToolkit.Domain.deps.json %outputDirectory%\SrkToolkit.Domain\lib\netstandard2.0
mkdir %outputDirectory%\SrkToolkit.Domain\lib\netstandard2.0\fr
xcopy /Q /Y Sources\NSTD.SrkToolkit.Domain\bin\Release\netstandard2.0\fr\SrkToolkit.Common.* %outputDirectory%\SrkToolkit.Domain\lib\netstandard2.0\fr

mkdir %outputDirectory%\SrkToolkit.Domain.AspMvc4\
mkdir %outputDirectory%\SrkToolkit.Domain.AspMvc4\lib
mkdir %outputDirectory%\SrkToolkit.Domain.AspMvc4\lib\net45
xcopy /Q /Y Binaries\NET45\SrkToolkit.Domain.AspMvc4.dll %outputDirectory%\SrkToolkit.Domain.AspMvc4\lib\net45
xcopy /Q /Y Binaries\NET45\SrkToolkit.Domain.AspMvc4.xml %outputDirectory%\SrkToolkit.Domain.AspMvc4\lib\net45

mkdir %outputDirectory%\SrkToolkit.Domain.AspMvc5\
mkdir %outputDirectory%\SrkToolkit.Domain.AspMvc5\lib
mkdir %outputDirectory%\SrkToolkit.Domain.AspMvc5\lib\net45
xcopy /Q /Y Binaries\NET45\SrkToolkit.Domain.AspMvc5.dll %outputDirectory%\SrkToolkit.Domain.AspMvc5\lib\net45
xcopy /Q /Y Binaries\NET45\SrkToolkit.Domain.AspMvc5.xml %outputDirectory%\SrkToolkit.Domain.AspMvc5\lib\net45
echo Done.




echo:
echo Increment version number
echo -----------------------------

echo Hit return to continue...
pause 
%vincrement% -file=%outputDirectory%\version.txt 0.0.1 %outputDirectory%\version.txt
if not %ERRORLEVEL% == 0 (
 echo ERROR: vincrement. exiting.
 cd %currentDirectory%
 pause
 goto end
)
set /p version=<%outputDirectory%\version.txt
echo Done: %version%

@REM set version=%version%-pre
@REM echo Done: %version%



echo:
echo Build NuGet package
echo -----------------------------

echo VERIFY RELEASE NOTES in nuspec files.
echo Hit return to continue...
pause 
cd %outputDirectory%
%nuget% pack SrkToolkit.Common\SrkToolkit.Common.nuspec -Version %version%
%nuget% pack SrkToolkit.Domain\SrkToolkit.Domain.nuspec -Version %version%
%nuget% pack SrkToolkit.Web.AspMvc4\SrkToolkit.Web.AspMvc4.nuspec -Version %version%
%nuget% pack SrkToolkit.Web.AspMvc5\SrkToolkit.Web.AspMvc5.nuspec -Version %version%
%nuget% pack SrkToolkit.Domain.AspMvc4\SrkToolkit.Domain.AspMvc4.nuspec -Version %version%
%nuget% pack SrkToolkit.Domain.AspMvc5\SrkToolkit.Domain.AspMvc5.nuspec -Version %version%
echo Done.




echo:
echo Push NuGet package
echo -----------------------------

echo Hit return to continue...
pause 
cd %outputDirectory%
%nuget% push SrkToolkit.Common.%version%.nupkg -Source https://www.nuget.org
%nuget% push SrkToolkit.Domain.%version%.nupkg -Source https://www.nuget.org
%nuget% push SrkToolkit.Web.AspMvc4.%version%.nupkg -Source https://www.nuget.org
%nuget% push SrkToolkit.Web.AspMvc5.%version%.nupkg -Source https://www.nuget.org
%nuget% push SrkToolkit.Domain.AspMvc4.%version%.nupkg -Source https://www.nuget.org
%nuget% push SrkToolkit.Domain.AspMvc5.%version%.nupkg -Source https://www.nuget.org
echo Done.


pause

:end

cd %currentDirectory%



