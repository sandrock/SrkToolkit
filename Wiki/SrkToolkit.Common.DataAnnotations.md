SrkToolkit.Common.DataAnnotations
================

```c#
using SrkToolkit.DataAnnotations;
```

CultureInfoAttribute : ValidationAttribute
-------------

Validates a `CultureInfo` name.

```c#
[CultureInfo]
public string Culture { get; set; }
```



DateRangeAttribute : ValidationAttribute
-------------

Validates a date range.

```c#
[DateRange(Minimum = "2014-01-01T00:00:00", Maximum = "2015-12-31T00:00:00")]
public DateTime Date { get; set; }
```

EmailAddressAttribute : ValidationAttribute
-------------

With .NET 4.5, use `[EmailAddressEx]`. 

Validates one or many email addresses.


```c#
// model definition

[EmailAddress]
public string Email { get; set; }

[EmailAddress(AllowMultiple = true, Maximum = 16)]
public string Emails { get; set; }

// get values from model

var email = Validate.EmailAddress(model.Email);
var emails = Validate.ManyEmailAddresses(model.Emails);
```


TimezoneAttribute
-------------

Validates a .NET timezone name.

```c#
[Timezone]
public string Timezone { get; set; }
```

