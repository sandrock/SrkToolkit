SrkToolkit.Web - Model Binders
==============================

This page explains how the model binders work internally and why they exist.


DecimalModelBinder
------------------

### The problem

In France (and some French-speaking countries) there is a mismatch between the official
number format and what users actually type. The official decimal separator is the comma
(e.g. `1 234,56`) but the French keyboard has a dot in the numeric keypad, not a comma.
When the app is set to `fr-FR`, a user who types `1234.56` triggers a validation error
because the framework sees a dot where it expects a comma.

More broadly, users move between locales, copy-paste numbers from spreadsheets, or simply
type the "wrong" separator by habit. A strict `decimal.Parse` with the request culture
rejects all of that.

### What it does

`DecimalModelBinder<T>` inspects the raw input string and applies heuristics to infer
intent based on the number and position of `,` and `.` characters, without requiring the
user to match the culture exactly.

**Comma-decimal cultures (e.g. `fr-FR`, where `,` is the decimal separator):**

- One comma + one dot → whichever comes first is the thousands separator; the other is
  the decimal separator. `1,123.456` → `1123.456`; `1.123,456` → `1123.456`.
- Multiple commas + one dot → commas are thousands separators. `1,222,123.456` → `1222123.456`.
- Multiple dots + one comma → dots are thousands separators, comma is decimal.
  `1.222.123,456` → `1222123.456`.
- One comma + no dot → comma is the decimal separator. `123,456` → `123.456`.
- Multiple commas + no dot → all commas are thousands separators. `1,234,567` → `1234567`.

**Dot-decimal cultures (e.g. `en-US`, where `.` is the decimal separator):**

- Spaces are replaced by the culture's group separator when appropriate.
- A comma in the input is treated as a thousands separator when the native decimal
  separator already appears, or as a decimal separator when it does not.

Inputs that are genuinely ambiguous (e.g. multiple commas and multiple dots) are rejected
with a validation error.

### `DecimalModelBinderState`

`BindModelImpl` is the core parsing method, deliberately separated from the MVC binding
machinery so that unit tests can call it directly and inspect the `Errors` list without
needing a `ModelBindingContext`.

### Registration

**ASP.NET Core** — insert `DecimalModelBinderProvider` at index 0 so it takes precedence
over the built-in numeric binders:

    services.AddControllersWithViews(options =>
        options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider()));

**ASP.NET MVC 5** — register in `Application_Start`:

    DecimalModelBinder.Register(ModelBinders.Binders);
