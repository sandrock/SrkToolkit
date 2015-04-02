SrkToolkit
==========

Here goes another set of libraries to help out. Separated into a few assemblies to avoid loading too much stuff in your app.

Assemblies
--------------------

- [SrkToolkit.Common](SrkToolkit.Common.md)
	- .NET 4.0
	- .NET 4.5
- SrkToolkit.Common.Unsafe
	- .NET 4.0
- [SrkToolkit.Web (for ASP MVC)](SrkToolkit.Web.md)
	- .NET 4.0 for MVC 3
	- .NET 4.5 for MVC4 
- SrkToolit.WebForms
	- .NET 4.0 
- SrkToolit.Xaml
	- .NET 4.0
	- Windows Phone 7
	- Windows Phone 7.1
	- Silverlight 4
- SrkToolkit.Mvvm
	- .NET 4.0
	- Windows Phone 7
	- Windows Phone 7.1
	- Silverlight 4
- SrkToolkit.Services
	- .NET 4.0
	- Windows Phone 7
	- Windows Phone 7.1
	- Silverlight 4
- SrkToolkit.Domain
	- .NET 4.0
	- .NET 4.5

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
 
 - date and time display (based on timezone ,standard formats, <time /> tag...)
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
 - `HttpRequest.IsXmlHttpRequest()`
 - `HttpRequest.IsUrlLocalToHost()`

And more...

Signed code
--------------------

All assemblies are signed. The real key is not in source control; I keep it to ensure my builds are only made by myself ([yes, I know that not 100% true][1]).

The latest build can be found here: _build not configured yet, sorry_.

Want unsigned assemblies? Get the code, remove the option and build.

Index
--------------------

- [SrkToolkit.Common](SrkToolkit.Common.md)
- [SrkToolkit.Web](SrkToolkit.Web.md)































[1]: http://ianpicknell.blogspot.fr/2010/02/tampering-with-strong-named-assembly.html
