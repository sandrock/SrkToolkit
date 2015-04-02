SrkToolit.Web - Action Filters
==============================

Before you start.

This page is not oriented on "how to use the stuff". It explains how things work internally justifies why things are here, why those things are useful.

There will be another page that explains how to implement this stuff.

If you read what is to follow, you may end-up thinking "WTF is this guy doing?" There is good stuff here but this page does not show it the right way.

SrkToolkit.Web.Filters.AuthorizeAttribute
---------------------

The "proper" attribute to allow access only to authenticated users.

What's wrong with `System.Web.Mvc.AuthorizeAttribute`? Well, you know how it works: it asks the user to log in. Hmm, this is not *exactly* what we want.

This piece of code does not handle well the difference between authentication and authorization.

This special attribute work this way:

- if not authenticated, the attributes uses the behavior from `System.Web.Mvc.AuthorizeAttribute`
- if authenticated and not authorized, this attributes shows a HTTP 403 page

IMO asking the authenticated user to authenticate when not authorized to access a resource is stupid.

People talk about that:

- SO question [Why does AuthorizeAttribute redirect to the login page for authentication and authorization failures?
](http://stackoverflow.com/questions/238437/why-does-authorizeattribute-redirect-to-the-login-page-for-authentication-and-au)
- A nice override that covers 2 majors issues with the original attribute. Well explained. [Thoughts on ASP.NET MVC Authorization and Security](https://www.simple-talk.com/dotnet/asp.net/thoughts-on-asp.net-mvc-authorization-and-security-/)
- What about JSON calls? When you want the resource or a 401 and never a redirect. [Prevent Forms Authentication Login Page Redirect When You Don’t Want It](http://haacked.com/archive/2011/10/04/prevent-forms-authentication-login-page-redirect-when-you-donrsquot-want.aspx/)
- discussion on aspnetwebstack [HttpForbidden for authenticated, unauthorized requests](https://aspnetwebstack.codeplex.com/discussions/356872)

> it makes more sense to return a 403 Forbidden when the user is authenticated but not authorized.

BTW, adding a TempData message when overriding this attribute might be nice.

FYI, here is a citation of the related HTTP status codes concerning authentication and authorization.

> 401 Unauthorized<br />
    Similar to 403 Forbidden, but *specifically for use when authentication is required and has failed or has not yet been provided.* The response must include a WWW-Authenticate header field containing a challenge applicable to the requested resource. See Basic access authentication and Digest access authentication.
> 
> 403 Forbidden<br />
    The request was a valid request, but the server is refusing to respond to it. Unlike a 401 Unauthorized response, *authenticating will make no difference.* On servers where authentication is required, this commonly means that the provided credentials were successfully authenticated but that the credentials still do not grant the client permission to access the resource (e.g., a recognized user attempting to access restricted content).
> 
> -- <cite>[Wikipedia: List of HTTP status codes](https://en.wikipedia.org/wiki/Http_status_codes)</cite>

The attribute SHOULD emit a 401 login page when not authentication. The current ASP MVC implementation is wrong (IMO) because it returns a 401 with a redirect to the login page (which will have a 200). "Hmm, strange, I'm not authenticated and I get a 200 that tells me I must authenticate...". The `System.Web.Mvc.AuthorizeAttribute` SHOULD fix the problem but this is a TODO.

The attribute SHOULD emit a 403 Forbidden page when *authenticated and not authorized*. The current ASP MVC implementation is wrong again. The `System.Web.Mvc.AuthorizeAttribute` fixes the problem.
