SrkToolit.Web - ResultService, IErrorController, error handling
===============================================================

Before you start.

This page is not oriented on "how to use the stuff". It explains how things work internally justifies why things are here, why those things are useful.

There will be another page that explains how to implement this stuff.

If you read what is to follow, you may end-up thinking "WTF is this guy doing?" There is good stuff here but this page does not show it the right way.

The ResultService, read first
---------------------

A few items here have the `ResultService` as a dependency. An object of this kind is able to write special responses in the HttpResponse.

It is used by the error handling class to render MVC actions of your ErrorController (without an ugly redirect).

It is used by the special `AuthorizeAttribute` to write a HTTP 403 when the authenticated user is not authorized to access a resource.

It took me a few year to get the idea to build this class and it is something very useful. 

Many motivations and situations require the ability to "stop here" and quickly return a special page (typically a error page).

When using any class depending on this one, you will have to declare a `ResultService` class in your project inheriting the `BaseResultService` class. This is for your good.

TODO: describe how to declare a ResultService in a new project.

### IResultService interface

This interface looks like a controller: it contains many methods returning an ActionResult. Methods are: Forbidden, NotFound, BadRequest, JsonSuccess, JsonError. Some parts of this toolkit needs a way to render those results. Some parts of your web app will need that too.

### ResultServiceBase class

This class implements the JsonSuccess and JsonError methods. It returns a `JsonNetResult` object with a nice JSON structure.

Here are some overloads.

        public ActionResult JsonSuccess(object data)
        {
            return new JsonNetResult
            {
                Data = new
                {
                    Success = true,
                    ErrorCode = default(string),
                    ErrorMessage = default(string),
                    Data = data,
                },
            };
        }

        public ActionResult JsonError(string errorCode, string errorMessage)
        {
            return new JsonNetResult
            {
                Data = new
                {
                    Success = false,
                    ErrorCode = errorCode,
                    ErrorMessage = errorMessage,
                },
            };
        }

I really like when everything is uniform when I develop. So all my AJAX calls return the same exact JSON structure that contains the desired data, a error code and a error message. When the is no data, the Success field is a simple boolean.

You will ask. "Huh, what's your serializer within JsonNetResult?" JsonNetResult is supports providers so you will have to declare the one your app will use! Now you say "Oh, great."


### ResultService<TErrorController> class (inherits from IResultService and ResultServiceBase)

Most of the code is in this class so you won't need to plus things here and there.

This contains basic code able to render error pages. You have to create your own ErrorController and an associated view Error.cshtml.

### Your implementation of IResultService

You need to create a class in your app inheriting from `ResultService<TErrorController>` (or IResultService and ResultServiceBase for customization).

Add the following property in your base controller.

        public ResultService ResultService
        {
            [DebuggerStepThrough]
            get { return new ResultService(this.HttpContext); }
        }

You will be able to return error pages and JSON easily.

	public ActionResult Entry(int id)
	{
		if (id == 0)
		{
			// show a 404 page with a message
			return this.ResultService.NotFound("This entry does not exist.");
		}

		// continue
	} 

	public ActionResult Entry(int id)
	{
		// code here

		if (this.Request.IsXmlHttpRequest())
		{
			// if AJAX call, return JSON instead of render view
			return this.ResultService.JsonSuccess(model);
		}

		return this.View(model);
	} 

### Handler for HTTP errors

Now that you are here, you're 1 line away for setuping your nice HTTP 500 error page for unhandled exceptions.

        protected virtual void Application_Error()
        {
			#if DEBUG
            bool debug = true;
			#else
            bool debug = false;
			#endif

            Exception exception;
            var errorController = new ErrorController();
            exception = ErrorControllerHandler.Handle(this.Context, errorController, debug);

			// you can log the exception here
        }

This little piece of code will use your ErrorController to render a error page. If this fails, it will use the `BasicHttpErrorResponse` class to output a text/plain error page.

A debug variable set to true will output the stack trace in the error page. 



HTTP errors
---------------------

TODO: document

- BaseErrorController class
	- contains methods to render error pages (404, 403, 500) 
- BasicHttpErrorResponse class
	- when the ErrorController fails, this object can ouput a text/plain error page 
- ErrorControllerHandler class
	- helps configure error handling 
- IErrorController interface
	- allow you to build you own compatible ErrorController



