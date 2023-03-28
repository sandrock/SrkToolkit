
namespace SrkToolkit.Web.Tests;

using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.WebEncoders.Testing;
using Moq;
using System;
using System.IO;
using System.Text.Encodings.Web;

/// <summary>
/// Helps generate a HttpContext and all the ASP and MVC objects in order to do component testing. 
/// </summary>
/// <typeparam name="T"></typeparam>
public class AspNetCoreTestContext<T>
{
    private readonly MockRepository mocks;
    private DefaultHttpContext context;
    private ViewDataDictionary<T> viewDataDictionary;
    private HtmlHelper<T> html;
    private T model;
    private ModelStateDictionary modelState;
    private IModelMetadataProvider metadataProvider = new EmptyModelMetadataProvider();
    private UrlHelper url;
    private ActionContext action;
    private RouteData routeData;
    private TempDataDictionary tempData;
    private ITempDataProvider tempDataProvider;

    /// <summary>
    /// Helps generate a HttpContext and all the ASP and MVC objects in order to do component testing.
    /// </summary>
    public AspNetCoreTestContext()
        : this(default(T))
    {
    }

    /// <summary>
    /// Helps generate a HttpContext and all the ASP and MVC objects in order to do component testing.
    /// </summary>
    /// <param name="model"></param>
    public AspNetCoreTestContext(T model)
    {
        this.Model = model;
        this.mocks = new MockRepository(MockBehavior.Default);
    }

    /// <summary>
    /// Gets the <see cref="HttpContext"/> instance (lazy). 
    /// </summary>
    public HttpContext Context
    {
        get
        {
            if (this.context == null)
            {
                this.context = new DefaultHttpContext();
            }

            return this.context;
        }
    }

    /// <summary>
    /// Gets the <see cref="ActionContext"/> instance (lazy). 
    /// </summary>
    public ActionContext ActionContext
    {
        get
        {
            if (this.action == null)
            {
                this.action = new ActionContext(this.Context, this.routeData = new RouteData(), new PageActionDescriptor(), this.ModelState);
            }

            return this.action;
        }
    }

    /// <summary>
    /// Gets the Model instance. 
    /// </summary>
    public T Model
    {
        get => this.model;
        set => this.model = value;
    }

    /// <summary>
    /// Gets the ModelState (lazy). 
    /// </summary>
    public ModelStateDictionary ModelState
    {
        get
        {
            if (this.modelState == null)
            {
                this.modelState = new ModelStateDictionary();
            }

            return this.modelState;
        }
    }

    /// <summary>
    /// Gets the <see cref="ViewData"/> (lazy).
    /// </summary>
    public ViewDataDictionary<T> ViewData
    {
        get
        {
            if (this.viewDataDictionary == null)
            {
                this.viewDataDictionary = new ViewDataDictionary<T>(this.metadataProvider, this.ModelState);
            }

            return this.viewDataDictionary;
        }
    }

    /// <summary>
    /// Gets the <see cref="HtmlHelper"/> (lazy).
    /// </summary>
    public virtual IHtmlHelper<T> Html
    {
        get
        {
            if (this.html == null)
            {
                var optionsAccessor = new OptionsWrapper<MvcViewOptions>(new MvcViewOptions());
                var htmlGenerator = new DefaultHtmlGenerator(
                    this.mocks.Create<IAntiforgery>().Object,
                    optionsAccessor,
                    this.metadataProvider,
                    new UrlHelperFactory(),
                    HtmlEncoder.Default,
                    new DefaultValidationHtmlAttributeProvider(optionsAccessor, this.metadataProvider, new ClientValidatorCache()));
                this.html = new HtmlHelper<T>(
                    htmlGenerator,
                    new CompositeViewEngine(optionsAccessor),
                    this.metadataProvider,
                    new Mock<IViewBufferScope>().Object,
                    HtmlEncoder.Default,
                    new UrlTestEncoder(),
                    new ExpressionTextCache());
                var viewContext = new ViewContext(
                    this.ActionContext,
                    new NullView(),
                    this.ViewData,
                    this.tempData ?? (this.tempData = new TempDataDictionary(this.Context, this.TempDataProvider)),
                    new StringWriter(),
                    new HtmlHelperOptions());
                this.html.Contextualize(viewContext);
            }

            return this.html;
        }
    }

    /// <summary>
    /// Gets the <see cref="UrlHelper"/> (lazy).
    /// </summary>
    public IUrlHelper Url
    {
        get
        {
            if (this.url == null)
            {
                this.url = new UrlHelper(this.ActionContext);
            }

            return this.url;
        }
    }

    public ITempDataDictionary TempData
    {
        get { return this.tempData ?? (this.tempData = new TempDataDictionary(this.Context, this.TempDataProvider)); }
    }

    internal ITempDataProvider TempDataProvider
    {
        get { return this.tempDataProvider ?? (this.tempDataProvider = new Mock<ITempDataProvider>().Object); }
    }
}

/// <summary>
/// Helps generate a HttpContext and all the ASP and MVC objects in order to do component testing.
/// </summary>
public class AspNetCoreTestContext : AspNetCoreTestContext<object>
{
}