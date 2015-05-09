SrkToolit.Web - Html helpers
============================

Before you start.

This page is not oriented on "how to use the stuff". It explains how things work internally, justifies why things are here, why those things are useful.

There will be another page that explains how to implement this stuff.

If you read what is to follow, you may end-up thinking "WTF is this guy doing?" There is good stuff here but this page does not show it the right way.

DisplayDate, DisplayDateTime, DisplayTime
-----------------------------------------

### Goal

Help display dates in the best way possible. Bringing a uniform way of displaying dates and times in your ASP MVC apps.

There are 3 things to display: date or time or date and time. This is why there are 3 helpers. (there are a few more in fact)

Here are typical outputs. 

    @Html.DisplayDate(date)
    <time datetime="2013-04-19T16:12:09.0000000Z" title="19 April 2013" class="future not-today display-date">19 April 2013</time>

    @Html.DisplayTime(date)
    <time datetime="2013-04-19T16:12:09.0000000Z" title="Fri, 19 Apr 2013 18:12:09 GMT" class="future today display-time">18:12:09</time>

    @Html.DisplayDateTime(date)
    <time datetime="2013-04-19T16:12:09.0000000Z" title="19/04/2013 18:12:09" class="past not-today display-datetime">19 April 2013 18:12:09</time>

You can notice:

- The [HTML5 `<time>` tag][1] is used. There is an optional parameter to remove it. The standard attribute `datetime` is filled in the correct format.
- The element has CSS classes so you can show pretty dates.
- There is a tooltip.

There are two main formats: 
- long (Tuesday, 2nd December 2013 13:45:12) 
- and short (02/12/2013 15:45) with `Html.DisplayShortDate`

### Requirements

Dates and times is a HARD subject. Many many unexpected situations may occur. I always build my apps to handle dates and times by displaying stuff in the *user's timezone* (which I ask he/she to set) and by *storing only UTC dates*. That's the basic stuff for international apps. And for local apps, it's not a big deal to build it the international way and it will be a lot easier to do it now.

So you must know how time works on our planet.

[MSDN: Coding Best Practices Using DateTime in the .NET Framework](http://msdn.microsoft.com/en-us/library/ms973825.aspx)  
[SO: How to elegantly deal with timezones](http://stackoverflow.com/questions/7577389/how-to-elegantly-deal-with-timezones/7583604#7583604)

### Usage

You may declare your display time zone. Not declaring it will display all dates converted to UTC.

    @{
        this.SetTimezone("Romance Standard Time"); // you may ask your user for that
    }

Then use the extensions to show well-formatted dates.

    It's @Html.DisplayTime(DateTime.UtcNow) now.
    Date created: @Html.DisplayDate(Model.DateCreatedUtc)

Be careful about the `DateTime.Kind` property.

* if your DateTime.Kind is Utc, it will be converted to the specified time zone (from utc)
* if your DateTime.Kind is Local, it will be converted to the specified time zone (from the system's time)
* if your DateTime.Kind is Unknown, it will not be converted (assuming the date is already set in the user's time zone)

Note: Entity Framework creates DateTime objects with Kind=Unknown. In my domain layers, I always do `this.DateCreatedUtc = item.DateCreatedUtc.AsUtc();` in order to ensure Kind is Utc. You may alter EF code generation to create DateTime correctly with a post-set property code.  


### About the HTML5 <time> tag

In HTML5, there is a new <time> tag that supports writing dates in a compliant way.

Why is it cool?

- you may want to apply a style to all your dates and times in CSS. The `time` selector is better than `span.datetime`.
- HTML5 is cool?

Why is it not?

- any compatibility issues? I don't know what IE lt 7 would do...

Some lectures:  [W3C HTML5 `<time>` tag][1], [W3C <time>'s datetime attribute format][2], [RFC 3339 for date formats][3], [article on HTML5Doctor][4]

### A tooltip, why?

When displaying dates or times, there may be a confusions. Is this time in 12 or 24 format? "7/6/2013" Is the month June or July? Here are some questions that users may ask themselves. A tooltip is quite the way to help. I believe it should display a non-confusing full date and time text. By non-confusion, I think of something culture-independant. 

The greatest to lowest unit format is nice because it's logical. year, month, day, hours, minutes, seconds. 2014-04-08 14:16:18.

Using words is another nice way. The main confusion is the day/month / month/day one. Displaying the month as a word would remove the ambiguity but would force the user to know bits of the language used.

### CSS classes  

The first class `display-date`, `display-datetime`, `display-time` indicates the type of display.

The second class indicates where the date is located in reference to now `future`, `past`. Plus there is a `today` / `not-today` class.

This is of no big use on a public website. However I use it a lot in my back-offices: past dates are gray, future are black. Users unconsciously notice it and are able to identify dates faster.  

### TimeZones

Todo: find a nice way to deal with `DateTime`s with a `DateTimeKind` of `Unknown`.

Done: set the current timezone. `DateTime`s often come from models as UTC (but not all always) and we often want to display them in the user's timezone.

    Html.GetTimezone();
    Html.SetTimezone("Romance Standard Time");

OpenGraph extension
-------------------

OpenGraph is a piece of HTML code that helps websites display links to your website with decorative information (title, description, thumbnails, video...) See [ogp.me](http://ogp.me/) and [developers.facebook.com/docs/opengraph](https://developers.facebook.com/docs/opengraph/).

A simple object will help you fill the OpenGraph description of your site and pages and two HTML extensions will allow you to write the namespaces and `<meta>` tags easily.

I use the `ViewBag` to store the page title and description, an optional picture url too. You may proceed differently.

If a page does not have information, some code will let you define default information.

View 1:

	@{
	    this.ViewBag.Title = "Welcome to MySite";
	    this.ViewBag.Description = "Discover new ways to... blah blah.";
	}

View 2:

	@{
	    this.ViewBag.Title = "How to perform a rotation in vacuum";
	    this.ViewBag.Image = "/Images/promo.png";
	}

_Layout.cshtml:

	@{
	    var openGraphData = this.ViewContext.ViewData["openGraphData"] as SrkToolkit.Web.Open.OpenGraphObject;
	    if (openGraphData == null)
	    {
	        openGraphData = new SrkToolkit.Web.Open.OpenGraphObject(this.ViewBag.Title, this.Request.Url)
	            .SetSiteName(ApplicationStrings.AppName)
	            .SetLocale(this.Culture);

			// do we have a description? if not, put one
	        if (this.ViewBag.Description is string)
	        {
	            openGraphData.SetDescription(this.ViewBag.Description ?? "This web site will help you discover...");
	        }

			// do we have a picture? if not, put one
	        Uri imageUrl = null;
	        if (this.ViewBag.Picture is string)
	        {
	            if (this.ViewBag.Picture == "")
	            {
					// the view want no OpenGraph picture
	            }
	            else if (this.ViewBag.Picture.StartsWith("/"))
	            {
					// local picture
	                imageUrl = new Uri(ApplicationStrings.SiteRootAddress + this.ViewBag.Picture.Substring(1), UriKind.Absolute);
	            }
	            else if (this.ViewBag.Picture.StartsWith("http://") || this.ViewBag.Picture.StartsWith("https://"))
	            {
					// remote picture
	                imageUrl = new Uri(this.ViewBag.Picture);
	            }
	            else
	            {
					// default picture
	                imageUrl = new Uri(ApplicationStrings.SiteRootAddress + this.ViewBag.Picture, UriKind.Absolute);
	            }
	        }
	        else if (this.ViewBag.Picture == null)
	        {
                // default picture
	            imageUrl = new Uri(ApplicationStrings.SiteRootAddress + "Images/Logo.png", UriKind.Absolute);
	        }

	        if (imageUrl != null)
	        {
	            openGraphData.SetImage(imageUrl);
	        }
	    }
	}
	<html @MvcHtmlString.Create(openGraphData.ToHtmlAttributeNamespaces())>
	<head>
	<title>this.ViewBag.Title -- MySite</title>
	@MvcHtmlString.Create(openGraphData.ToString())
	</head>

Example render:

	<html  xmlns:og="http://ogp.me/ns#" >

    <meta property="og:title" content="Welcome to MySite" />
    <meta property="og:url" content="http://mysite.com/" />
    <meta property="og:site_name" content="MySite.com" />
    <meta property="og:locale" content="fr_FR" />
    <meta property="og:image" content="/Images/Logo.png" />



Html.CallLink("+33 123456789")
------------------------------

Helps you display a phone line.

    @Html.CallLink("+33 123456789")
    <a class="tel" href="tel:+33 123456789">+33 123456789</a>
    
    @Html.CallLink("+33 123456789" new { @class = "hello world", })
    <a class="hello world" href="tel:+33 123456789">+33 123456789</a>

Html.DescriptionFor(model => model.Value)
-----------------------------------------

    public class TestModel
    {
        [Display(Description = "Desc a2a")]
        public string Description2 { get; set; }
    }
    
    @Html.DescriptionFor(model => model.Description2) // "Desc a2a"


@Html.ValidationSummaryEx()
---------------------------

Enhancement of Html.ValidationSummary() that shows no HTML when there are no errors to display.


@Html.CssClass()
----------------

Helps write conditional CSS classes.

    public class TestModel
    {
        public bool IsEnabled { get; set; }
        public bool? HasUser { get; set; }
    }
    
    <div class="item @Html.CssClass(model.IsEnabled, "IsEnabled")"></div>
    <div class="item @Html.CssClass(model.IsEnabled, "IsEnabled", "IsNotEnabled")"></div>
    
    <div class="item @Html.CssClass(model.HasUser, "HasUser", "HasNoUser", "MayHasUser")"></div>
    
    @Html.CssClass(<bool  value>, "if-true")
    @Html.CssClass(<bool  value>, "if-true", "if-false")
    @Html.CssClass(<bool? value>, "if-true", "if-false", "if-null")

@Html.JsDate(dateTime)
----------------------

    @Html.JsDate(datetime)
    outputs: new Date(13912786171000)


[1]: http://www.w3.org/TR/html-markup/time.html
[2]: http://www.w3.org/TR/html-markup/datatypes.html#common.data.time-datetime
[3]: http://tools.ietf.org/html/rfc3339
[4]: http://html5doctor.com/the-time-element/