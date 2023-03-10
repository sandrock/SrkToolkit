
namespace SrkToolkit.AspNetCore
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;

    public static class SrkHttpRequestExtensions
    {
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

        // TODO: support HttpRequest.PrefersJson, but which nuget?
#if !NETSTANDARD
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
#endif
    }
}
