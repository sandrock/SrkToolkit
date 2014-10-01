﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SrkToolkit.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    public partial class SrkHttpApplication : HttpApplication
    {
        /// <summary>
        /// Called when the first resource (such as a page) in an ASP.NET application is requested. The Application_Start method is called only one time during the life cycle of an application. You can use this method to perform startup tasks such as loading data into the cache and initializing static values.
        /// </summary>
        public virtual void Application_Start(object sender, EventArgs e) { }

        /// <summary>
        /// Called once per lifetime of the application before the application is unloaded.
        /// </summary>
        public virtual void Application_End(object sender, EventArgs e) { }

        /// <summary>
        /// Called once for every instance of the HttpApplication class after all modules have been created.
        /// </summary>
        public virtual void Application_Init(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs as the first event in the HTTP pipeline chain of execution when ASP.NET responds to a request.
        /// </summary>
        public virtual void Application_BeginRequest(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs when a security module has established the identity of the user.
        /// </summary>
        public virtual void Application_AuthenticateRequest(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs when a security module has established the identity of the user.
        /// </summary>
        public virtual void Application_PostAuthenticateRequest(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs when a security module has verified user authorization.
        /// </summary>
        public virtual void Application_AuthorizeRequest(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs when the user for the current request has been authorized.
        /// </summary>
        public virtual void Application_PostAuthorizeRequest(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs when ASP.NET finishes an authorization event to let the caching modules serve requests from the cache, bypassing execution of the event handler (for example, a page or an XML Web service).
        /// </summary>
        public virtual void Application_ResolveRequestCache(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs when ASP.NET bypasses execution of the current event handler and allows a caching module to serve a request from the cache.
        /// </summary>
        public virtual void Application_PostResolveRequestCache(object sender, EventArgs e) { }

        /// <summary>
        /// Infrastructure. Occurs when the handler is selected to respond to the request.
        /// </summary>
        public virtual void Application_MapRequestHandler(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs when ASP.NET has mapped the current request to the appropriate event handler.
        /// </summary>
        public virtual void Application_PostMapRequestHandler(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs when ASP.NET acquires the current state (for example, session state) that is associated with the current request.
        /// </summary>
        public virtual void Application_AcquireRequestState(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs when the request state (for example, session state) that is associated with the current request has been obtained.
        /// </summary>
        public virtual void Application_PostAcquireRequestState(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs just before ASP.NET starts executing an event handler (for example, a page or an XML Web service).
        /// </summary>
        public virtual void Application_PreRequestHandlerExecute(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs when the ASP.NET event handler (for example, a page or an XML Web service) finishes execution.
        /// </summary>
        public virtual void Application_PostRequestHandlerExecute(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs after ASP.NET finishes executing all request event handlers. This event causes state modules to save the current state data.
        /// </summary>
        public virtual void Application_ReleaseRequestState(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs when ASP.NET has completed executing all request event handlers and the request state data has been stored.
        /// </summary>
        public virtual void Application_PostReleaseRequestState(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs when ASP.NET finishes executing an event handler in order to let caching modules store responses that will be used to serve subsequent requests from the cache.
        /// </summary>
        public virtual void Application_UpdateRequestCache(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs when ASP.NET finishes updating caching modules and storing responses that are used to serve subsequent requests from the cache.
        /// </summary>
        public virtual void Application_PostUpdateRequestCache(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs just before ASP.NET performs any logging for the current request.
        /// </summary>
        public virtual void Application_LogRequest(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs when ASP.NET has completed processing all the event handlers for the LogRequest event.
        /// </summary>
        public virtual void Application_PostLogRequest(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs as the last event in the HTTP pipeline chain of execution when ASP.NET responds to a request.
        /// </summary>
        public virtual void Application_EndRequest(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs just before ASP.NET sends HTTP headers to the client.
        /// </summary>
        public virtual void Application_PreSendRequestHeaders(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs just before ASP.NET sends content to the client.
        /// </summary>
        public virtual void Application_PreSendRequestContent(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs when the application is disposed.
        /// </summary>
        public virtual void Application_Disposed(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs when an unhandled exception is thrown.
        /// </summary>
        public virtual void Application_Error(object sender, EventArgs e) { }

        /// <summary>
        /// The Session_OnStart event is used to perform any initialization work for a session such as setting up default values for session variables.
        /// </summary>
        public virtual void Session_Start(object sender, EventArgs e) { }

        /// <summary>
        /// The Session_OnEnd event is used to perform any cleanup work for a session such as disposing of resources used by the session.
        /// </summary>
        public virtual void Session_End(object sender, EventArgs e) { }
    }
}
