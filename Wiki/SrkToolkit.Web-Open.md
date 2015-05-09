SrkToolit.Web - PageInfo
==============================

Tired of `<title>`, `<meta>`s, opengraph?

Let's build a PageInfo object...

```c#
// from view: Html.GetPageInfo()
// from controller: this.GetPageInfo() 
Html.GetPageInfo()
    .Set(PageInfo.Title, "We connect your community members")
    .Set(PageInfo.Description, "We connect your community members with a online network your members will love.")
    .Set(PageInfo.Canonical, "/en-US")
    .Set(PageInfo.Keywords, "key word")
    .Set(PageInfo.SiteName, "My web site")
    .Set(PageInfo.Language) // uses CultureInfo.CurrentCulture
    .Set(PageInfo.Keywords, keywords)
    .Set(PageInfo.Favicon, "/favicon.ico")
    .Set(PageInfo.RevisitAfter(7))
    .Set(new PageInfoItem("analytics1", "aaaaaaa-bb").Add(new PageInfoObject().MetaWithValue("analytics1")))
```

And generate stuff in the layout view.

```html
<!DOCTYPE html>
<html @Html.Raw(pageInfo.OpenGraph.ToHtmlAttributeNamespaces())>
<head>
    <meta charset="utf-8" />

<!-- BEGIN: PageInfo -->
@Html.Raw(pageInfo.ToString(PageInfoObjectSection.Basic | PageInfoObjectSection.OldMeta, true))
<!-- END: PageInfo -->

<!-- BEGIN: PageInfo.OpenGraph -->
@Html.Raw(pageInfo.OpenGraph.ToHtmlString(null, Environment.NewLine, null))
<!-- END: PageInfo.OpenGraph -->
```

Result: 

```html
<!-- BEGIN: PageInfo -->
<title>We connect your community members</title>
<meta content="We connect your community members with a online network your members will love." name="description" />
<link href="/en-US" rel="canonical" />
<meta content="key word" name="keywords" />
<link href="/favicon.ico" rel="shortcut icon" type="image" />
<meta content="en-US" name="language" />
<meta content="7 days" name="revisit-after" />
<meta content="aaaaaaa-bb" name="analytics1" />
<!-- END: PageInfo -->

<!-- BEGIN: PageInfo.OpenGraph -->
<meta property="og:title" content="We&nbsp;connect&nbsp;your&nbsp;community&nbsp;members" />
<meta property="og:description" content="We&nbsp;connect&nbsp;your&nbsp;community&nbsp;members&nbsp;with&nbsp;a&nbsp;online&nbsp;network&nbsp;your&nbsp;members&nbsp;will&nbsp;love." />
<meta property="og:url" content="&#x2F;en-US" />
<meta property="og:site_name" content="My web site" />
<meta property="og:locale" content="en_US" />
<meta property="og:locale:alternate" content="fr_FR" />
<meta property="og:locale:alternate" content="en_US" />
<!-- END: PageInfo.OpenGraph -->
```
