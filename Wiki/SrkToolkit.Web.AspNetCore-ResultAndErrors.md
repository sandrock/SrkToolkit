SrkToolkit.Web.AspNetCore - ResultService, IErrorController, error handling
===========================================================================

This page covers the ASP.NET Core version of the result and error handling classes.
For the MVC5 version see [SrkToolkit.Web.AspMvc5-ResultAndErrors](SrkToolkit.Web.AspMvc5-ResultAndErrors.md).


The ResultService
-----------------

`ResultServiceBase` and `ResultService` help return uniform HTTP responses from controllers
and filters without scattering response logic across the codebase.


### IResultService interface

Methods: `Forbidden`, `NotFound`, `BadRequest`, `Gone`, `MethodNotAllowed`, `JsonSuccess`,
`JsonError`. Any component that needs to produce these responses depends on this interface.


### ResultServiceBase class

Implements the JSON methods. All AJAX responses share the same envelope:

    {
        "Success": true | false,
        "ErrorCode": "...",
        "ErrorMessage": "...",
        "Data": { ... }
    }

`JsonResult` is used internally, so the serializer respects whatever the app configured in
startup (`AddNewtonsoftJson()`, `AddJsonOptions(...)`, etc.).


### ResultService class (inherits ResultServiceBase, implements IResultService)

Implements `Forbidden`, `NotFound`, `BadRequest`, `Gone`, `MethodNotAllowed`, `Error`
by returning the appropriate Core action result (`ForbidResult`, `NotFoundResult`,
`StatusCodeResult`, etc.).

The actual error view is rendered by the status-code pages middleware — configure it once
in startup and all status codes are handled automatically (see startup section below).

Usage in a base controller:

    protected ResultService ResultService
        => new ResultService(this.HttpContext);

    public ActionResult Entry(int id)
    {
        if (id == 0)
        {
            return this.ResultService.NotFound();
        }

        // continue
    }

    public ActionResult Entry(int id)
    {
        // ...

        if (this.Request.IsXmlHttpRequest())
        {
            return this.ResultService.JsonSuccess(model);
        }

        return this.View(model);
    }


HTTP errors
-----------

### IErrorController interface

Plain interface (no `IController` dependency). Implement it in your error controller.
Methods match HTTP codes: `Forbidden`, `NotFound`, `Gone`, `BadRequest`,
`MethodNotAllowed`, `Internal`.


### BaseErrorController class

Extend this in your app's `ErrorController`. It provides:

- `Show(int code)` — primary action, handles both `UseStatusCodePagesWithReExecute` and
  `UseExceptionHandler` routes. Reads the original URL and exception from middleware
  features automatically.
- Per-code actions (`Forbidden`, `NotFound`, `Gone`, `BadRequest`, `MethodNotAllowed`,
  `Internal`) — optional named entry points. Each creates a fresh `HttpErrorModel` and
  delegates to `Work`.
- `Work(action, model, code)` — shared implementation: sets the status code, fills
  `Title` and `Message` from the embedded resource file if not already set, checks
  whether the request is AJAX/JSON (returns JSON envelope), otherwise returns
  `View("Error", model)`.
- `OnErrorResponseReady(action, model, code)` — empty virtual hook for logging or metrics.
- `IncludeExceptionDetails` flag — set to true in development to expose stack traces.

Code names and descriptions displayed on the error page come from an embedded resource
file (`HttpErrorMessages.resx`) and are automatically localised when the request culture
is set. A French translation (`HttpErrorMessages.fr.resx`) is included out of the box.

To customise a specific error, override the action method:

    public class ErrorController : BaseErrorController
    {
        public override ActionResult NotFound()
        {
            return this.Work("NotFound", HttpErrorModel.Create(404, "Page not found", "The page you requested does not exist."), 404);
        }

        protected override void OnErrorResponseReady(string action, HttpErrorModel model, int code)
        {
            _logger.LogWarning("HTTP {Code} for {Path}", code, model.UrlPath);
        }
    }


### Using HttpErrorExtensions when you cannot inherit BaseErrorController

If your controller already extends another base class, use the `HttpErrorExtensions`
static extension methods instead. They expose the same logic without requiring inheritance.

    using SrkToolkit.Web.HttpErrors;

    public class ErrorController : MyBaseController
    {
        // Minimum: wire the generic Show action
        [Route("Error/Show/{code:int}")]
        public ActionResult Show(int code) => this.HttpErrorShow(code);
    }

To include exception details in development:

    [Route("Error/Show/{code:int}")]
    public ActionResult Show(int code)
        => this.HttpErrorShow(code, includeExceptionDetails: _env.IsDevelopment());

To override a specific code or add logging:

    [Route("Error/Show/{code:int}")]
    public ActionResult Show(int code)
        => this.HttpErrorWork(
            "Show",
            HttpErrorModel.Create(code, null, null),
            code,
            onReady: (action, model, c) => _logger.LogWarning("HTTP {Code} for {Path}", c, model.UrlPath));

`HttpErrorWork` is also callable directly for per-code named actions:

    public ActionResult Forbidden()
        => this.HttpErrorWork("Forbidden", HttpErrorModel.Create(403, null, null), 403);


### Startup configuration

Wire up both middlewares so that unhandled exceptions and non-success status codes both
reach the error controller:

    // Unhandled exceptions → ErrorController.Show(500)
    app.UseExceptionHandler("/Error/Show/500");

    // Non-success status codes (404, 403, …) → ErrorController.Show({code})
    app.UseStatusCodePagesWithReExecute("/Error/Show/{0}");

Both routes reach `Show(int code)` on `BaseErrorController`, which reads the original
path and exception from the ASP.NET Core middleware features automatically.

Set `IncludeExceptionDetails` in development to expose stack traces:

    if (app.Environment.IsDevelopment())
    {
        errorController.IncludeExceptionDetails = true;
    }


### BasicHttpErrorResponse class

Last-resort fallback. Writes a plain-text (`text/plain`) error page directly to the
response when the MVC pipeline cannot render a view (e.g. during exception middleware).

    await BasicHttpErrorResponse.Execute(context, originalError, extraError, message);

`Execute` is async (`Task`) because `HttpResponse.WriteAsync` is the Core API.


### AuthorizeAttribute

See `SrkToolkit.Web-ActionFilters.md` (TODO: split to AspMvc5 / AspNetCore pages).
