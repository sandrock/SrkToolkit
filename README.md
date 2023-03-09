
SrkToolkit
==========

Here goes another set of libraries to help out. Separated into a few assemblies to avoid loading too much stuff in your app.

License: Apache License Version 2.0


Branches
--------------------

- `release/2.0` next major release (WIP)
- `release/1.2` current stable release
- `release/1.1` old release (hotfixes only)



Assemblies
--------------------

| Assembly                                                 | FX                           | Nuget                                                     | Remark |
|----------------------------------------------------------|------------------------------|-----------------------------------------------------------|--------|
| [SrkToolkit.Common](Wiki/SrkToolkit.Common.md)           | net40, net45, netstandard2.0 | [nuget](https://www.nuget.org/packages/SrkToolkit.Common) |        |
| SrkToolkit.Common.Unsafe                                 | net40                        | |        |
| [SrkToolkit.Domain](Wiki/SrkToolkit.Domain.md)           | net40, net45, netstandard2.0 | [nuget](https://www.nuget.org/packages/SrkToolkit.Domain) |        |
| [SrkToolkit.Web (for ASP MVC 3)](Wiki/SrkToolkit.Web.md) | net40                        | [nuget mvc4](https://www.nuget.org/packages/SrkToolkit.Web.AspMvc4) |        |
| [SrkToolkit.Web (for ASP MVC 4)](Wiki/SrkToolkit.Web.md) | net45                        | [nuget mvc5](https://www.nuget.org/packages/SrkToolkit.Web.AspMvc5) |        |
| SrkToolkit.Domain.AspMvc3                                | net40,                       | [nuget mvc4](https://www.nuget.org/packages/SrkToolkit.Domain.AspMvc4) |        |
| SrkToolkit.Domain.AspMvc4                                | net45                        | [nuget mvc5](https://www.nuget.org/packages/SrkToolkit.Domain.AspMvc5) |        |
| SrkToolit.WebForms                                       | net40                        | |        |
| SrkToolit.Xaml                                           | net40, wp70, wp71, sl4       | |        |
| SrkToolkit.Mvvm                                          | net40, wp70, wp71, sl4       | |        |
| SrkToolkit.Services                                      | net40, wp70, wp71, sl4       | |        |

[See all nuget packages](https://www.nuget.org/packages?q=Tags%3A%22SrkToolkit%22)


Content at-a-glance
--------------------

### extend the framework to write code faster

  - date manipulations, DataAnnotations
  - string manipulations (trim with suffix, `AsNullIfEmpty()`, `AddHtmlLineBreaks()`, `TrimTextToWord()`, `HtmlParagraphizify()`, `RemoveDiacritics()`, `MakeUrlFriendly()`) 
  - `NameValueCollection.ToDictionary()`
  - ObservableCollection<T>: `AddRange(IEnumerable<T>)`, `RemoveAll(Func<T, bool>)`
  - StopwatchExtensions: fluent methods to avoid writing many lines of code when using it  
  - email address decomposition (account, tags, local part, domain part) and validation
  - no fancy stuff like `StringExtension.IsEmpty()`)

And more...

### extensions for ASP MVC
 
 - [date and time display helpers](Wiki/SrkToolkit.Web-HtmlHelpers.md) (based on timezone, standard formats, <time /> tag...)
 - PageInfo is the one object to set your page title, description, meta tags, opengraph values...
 - fixed `AuthorizeAttribute` to return a "HTTP 403 Forbidden" instead of 401 auth page when already authenticated
 - 1-line error handling code for customized http-accept-aware error pages
 - wrapper for tempmessages to show nice informations, warnings and errors (extension methods for both controllers and view)
 - `DecimalModelBinder` with "friendly" decimal separator detection (useful for stupid cultures like French)
 - OpenGraph extensions to easily ouput tags in your page
 - `BaseSessionService` class that allows auto-populating new and expired sessions in a lazy way
 - `ResultService` class that allows oupting error pages from actions
 - `Html.DescriptionFor()` to show the Description property from the `[DisplayAttribute(Name = "", Description = "<desc here>")]`
 - `Html.Submit()` 'cauz it's missing
 - `UrlHelper.SetQueryString(string url, params string[] keysAndValues)` to add/change query strings from a raw url
 - `JsonNetResult`: ActionResult class that allows you to specify your favorite JSON serializer
 - `HttpRequest`: `.IsXmlHttpRequest()`, `.IsUrlLocalToHost(url)`, `IsPost()`...

And more...

Signed code
--------------------

All assemblies are signed. The real key is not in source control; I keep it to ensure my builds are only made by myself ([yes, I know that is not 100% true][1]).

The latest build can be found here: https://www.nuget.org/packages?q=Tags%3A%22srktoolkit%22

Want unsigned assemblies? Get the code, remove the option and build.



[1]: http://ianpicknell.blogspot.fr/2010/02/tampering-with-strong-named-assembly.html
