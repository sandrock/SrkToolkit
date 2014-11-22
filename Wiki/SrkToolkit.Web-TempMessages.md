SrkToolit.Web - TempMessages enhancement
========================================


ASP MVC's TempData is great. It helps displaying messages to the user after a redirection ([learn more](https://duckduckgo.com/?q=asp+mvc+tempdata)). 

What's wrong with TempData?
---------------------------

Nothing! Well... It's basic. 

Simple message mechanism
------------------------

What do you do with tempdata? Add confirmation messages after submitting a form, showing more details after redirecting the a different page. 

This simple extension provides a model to have nice temp *messages*. A message is a piece of (localized) text, with a kind (info/error/warning/confirmation), that may contain HTML. The `SrkToolkit.Web.Models.TempMessage` class contains that.


    public class TempMessage
    {
        public string Message { get; set; }

        public TempMessageKind Kind { get; set; }

        public bool IsMarkup { get; set; }
    }

### Post messages

Now let's play. Let's look at a typical POST action.

	using SrkToolkit.Web;
	
	[HttpPost]
	public ActionResult Save(MyModel model)
	{
		if (this.ModelState.IsValid)
		{
			// send a confirmation to the user
			this.TempData.AddConfirmation("Entry saved!");
			
			// and a little tip
			this.TempData.AddInformation("Don't forget you can always edit this entry again.");
			
			return this.RedirectToAction("Index");
		}

		return this.View(model);
	}

### Display messages

See how easy it was to add many messages? Let's display that in our masterpage.

	<ul class="tempdatagroup">
	@foreach (var group in this.TempMessages().GroupBy(m => m.Kind))
	{
	    foreach (var item in group)
	    {
	    <li class="tempdata-@group.Key">
	        @if (item.IsMarkup)
	        {
	        @MvcHtmlString.Create(item.Message)
	        }
	        else
	        {
	        @item.Message
	        }
	    </li>
	    }
	}
	</ul>

I usually use CSS to show well-visible eye-catching messages.
