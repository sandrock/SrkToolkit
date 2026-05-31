
Dev Notes
=========

Remarks and pending items identified during code review sessions.
This file is local only — do not commit.


CI / Build
----------

- `Build-Binaries.bat` and `tools/build.bat` are **obsolete v1 artefacts**.
  They reference old project names (`NSTD.*`, `NET4.*`, `NET45.*`), msbuild v4,
  nuget.exe and .nuspec files. None of that applies to v2.
  The v2 build is `dotnet build -c Release` (packages via `GeneratePackageOnBuild`).

- Release process (`Releasing.md`) is **fully manual**: version bumps in every csproj,
  build, push. No automation.


Completed this session
----------------------

- Bumped all target frameworks to net48 / net8.0 across all projects.
- Added `Directory.Build.props` with `Microsoft.NETFramework.ReferenceAssemblies`
  so net48 builds on Linux without a Windows Developer Pack.
- Updated CI (`.github/workflows/dotnet.yml`) to .NET 8.0.x, added Windows job
  for net48 tests.
- Fixed `DateRangeAttribute`: strict ISO 8601 parsing, InvariantCulture, no
  ErrorMessageResourceName mutation, expanded tests (28 tests).
- Fixed `RecursiveDelete`: removed dead `#if NET40 / #if NET45` conditionals.
- Fixed `PageInfoObject` (AspNetCore renderer): set TagRenderMode.SelfClosing for
  meta/link tags (TagBuilder.WriteTo defaults to Normal).
- Fixed `WebDependencies`: pinned StringWriter.NewLine = "\r\n"; removed stray
  extra sb.WriteLine() in CSS branch (double newline bug).
- Added `BasicResult.AddError(IResultError)` + tests (26 new tests).
- Fixed `BasicResult<T>.AddError`: detail consumed as format arg via params clash.
- Fixed `BaseResult.Errors` setter: was not nulling IBaseResult proxy on assignment.
  Added tests (17 new).
- Issue #22 drops committed: removed csproj placeholder entries for MVC5-only files
  not being ported (IntegerModelBinder, SrkTagBuilderExtensions, SrkHttpApplication,
  Fakes, ErrorControllerHandler, ResultService, JsonNetResult).
  SrkHtmlExtensions: inlined TagBuilder.WriteTo replacing removed ToHtmlString ext.


Test baseline — net8.0
----------------------

SrkToolkit.Domain.CoreUnitTests     72 /  72  CLEAN
SrkToolkit.Services.CoreUnitTests   22 /  22  CLEAN
SrkToolkit.AspNetCore2.UnitTests   117 / 131  14 failures
SrkToolkit.Common.CoreUnitTests    409 / 428  19 failures


Remaining failures — AspNetCore2.UnitTests (14)
-----------------------------------------------

SrkDomainControllerExtensions (2):
  - ValidResult_TempData_Works
  - InvalidResult_TempData_Works

SrkHtmlExtensions.DisplayDate / DisplayDateTime (12):
  - All UserIsUtc / UserIsRomance combinations (arg: Utc, Local, User)
  - Long-date format differs by OS/ICU version ("Tuesday, 29 January 2013"
    vs "29 January 2013").


Remaining failures — Common.CoreUnitTests (19)
----------------------------------------------

SrkTimeZoneInfoExtensions (4):
  - Timezone conversion tests — system TZ sensitive (Linux default != test expectation).

CultureInfoHelper.GetCountries (5):
  - HasUSA, HasUK, HasFrance, HasChina, HasChina2
  - ICU data differences between Linux and Windows .NET runtimes.

DisposableOnce (1):
  - GarbageCollectorDoesNotCallDelegate — GC timing, inherently flaky.

SrkStringTransformer.HtmlParagraphizify (2):
  - Many, CombinedWithLineBreaks — HTML output mismatch (not investigated).

StringReplacer (7):
  - ModelWithCulture, SimpleWithCulture, ModelWithTimezone1/2,
    ModelWithReplacerCulture_FrenchFrance, ModelWithReplacerCulture_EnglishUSA
  - Culture/timezone-sensitive formatting; likely same root as TimeZoneInfo.


Issue #22 — remaining work in SrkToolkit.Web.AspNetCore2
---------------------------------------------------------

Note: AspNetCore2 uses AspMvc5 as shared source via file links. Files physically
live in AspNetCore2 directory; AspMvc5 links them in for the net48 build.

TO MIGRATE (csproj placeholder entries remain):

- Mvc/DecimalModelBinder.cs           moderate  port to async IModelBinder API

DROPPED (csproj entries removed this session):

- Mvc/IntegerModelBinder.cs       [Obsolete]+#if DEBUG, Core handles it natively
- SrkTagBuilderExtensions.cs      ToHtmlString inlined at call sites in SrkHtmlExtensions
- SrkHttpApplication.cs           HttpApplication has no Core equivalent
- Fakes (3 files)                 never implemented; use DefaultHttpContext / Moq
- HttpErrors/ErrorControllerHandler.cs  HttpApplication.Application_Error, not portable
- Services/ResultService.cs       IController.Execute pattern, not portable
- JsonNetResult.cs                Core JsonResult is sufficient


Dead code
---------

- RetryLogic.cs and RetryLogicState.cs: dead `#if NET45` / `#if NET40` blocks still present.


Test coverage gaps
------------------

- `SrkToolkit.Domain.AspNetCore2` has no test project.
- `SrkToolkit.Web.AspMvc5` has no test project.


Ideas
-----

- Create a `Samples/` directory at repo root containing small runnable projects
  demonstrating how to use the library (e.g. PageInfo, WebDependencies, Domain result
  pattern, DateRangeAttribute). Would serve as both documentation and integration tests.
