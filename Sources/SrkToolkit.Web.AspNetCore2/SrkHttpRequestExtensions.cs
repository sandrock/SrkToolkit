
namespace SrkToolkit.AspNetCore
{

#if ASPMVCCORE
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
#endif
    
#if ASPMVC
    using System.Web;
    using System.Web.Mvc;
#endif


    using System;
    using System.Diagnostics;
    using System.Linq;

    public static class SrkHttpRequestExtensions
    {
#if ASPMVCCORE
        public static bool PrefersJson(this HttpRequestMessage request)
        {
            if (request.Headers.Accept.Any())
            {
                foreach (var header in request.Headers.Accept)
                {
                    if ("application/json".Equals(header.MediaType, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
#endif

/*
        // TODO: support HttpRequest.PrefersJson, but which nuget?
        public static bool PrefersJson(this HttpRequest request)
        {
            Debug.Assert(request != null, nameof(request) + " != null");
            if (request.Headers.Accept.Any())
            {
                foreach (var header in request.Headers.Accept)
                {
                    if ("application/json".Equals(header, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
*/

    }
}
