
namespace SrkToolkit.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// ViewModel representing a HTTP error with details and description.
    /// </summary>
    public class HttpErrorModel
    {
        private static Dictionary<int, string> definitions;
        private static Dictionary<int, string> names;

        static HttpErrorModel()
        {
            definitions = new Dictionary<int, string>();
            definitions.Add(100, "This means that the server has received the request headers, and that the client should proceed to send the request body.");
            definitions.Add(101, "This means the requester has asked the server to switch protocols and the server is acknowledging that it will do so.");
            definitions.Add(102, "This code indicates that the server has received and is processing the request, but no response is available yet.");
            definitions.Add(200, "Standard response for successful HTTP requests.");
            definitions.Add(201, "The request has been fulfilled and resulted in a new resource being created.");
            definitions.Add(202, "The request has been accepted for processing, but the processing has not been completed.");
            definitions.Add(203, "The server successfully processed the request, but is returning information that may be from another source.");
            definitions.Add(204, "The server successfully processed the request, but is not returning any content.");
            definitions.Add(205, "The server successfully processed the request, but is not returning any content. Unlike a 204 response, this response requires that the requester reset the document view.");
            definitions.Add(206, "The server is delivering only part of the resource due to a range header sent by the client.");
            definitions.Add(207, "The message body that follows is an XML message and can contain a number of separate response codes.");
            definitions.Add(208, "The members of a DAV binding have already been enumerated in a previous reply to this request, and are not being included again.");
            definitions.Add(300, "Indicates multiple options for the resource that the client may follow.");
            definitions.Add(301, "This and all future requests should be directed to the given URI.");
            definitions.Add(302, "The HTTP/1.0 specification (RFC 1945) required the client to perform a temporary redirect.");
            definitions.Add(303, "The response to the request can be found under another URI using a GET method.");
            definitions.Add(304, "Indicates that the resource has not been modified since the version specified by the request headers If-Modified-Since or If-Match.");
            definitions.Add(305, "The requested resource is only available through a proxy, whose address is provided in the response.");
            definitions.Add(307, "In this case, the request should be repeated with another URI; however, future requests should still use the original URI.");
            definitions.Add(308, "The request, and all future requests should be repeated using another URI.");
            definitions.Add(400, "The request cannot be fulfilled due to bad syntax.");
            definitions.Add(401, "Similar to 403 Forbidden, but specifically for use when authentication is required and has failed or has not yet been provided. The response must include a WWW-Authenticate header field containing a challenge applicable to the requested resource.");
            definitions.Add(403, "The request was a valid request, but the server is refusing to respond to it.");
            definitions.Add(404, "The requested resource could not be found but may be available again in the future.");
            definitions.Add(405, "A request was made of a resource using a request method not supported by that resource.");
            definitions.Add(406, "The requested resource is only capable of generating content not acceptable according to the Accept headers sent in the request.");
            definitions.Add(407, "The client must first authenticate itself with the proxy.");
            definitions.Add(408, "The server timed out waiting for the request.");
            definitions.Add(409, "Indicates that the request could not be processed because of conflict in the request, such as an edit conflict in the case of multiple updates.");
            definitions.Add(410, "Indicates that the resource requested is no longer available and will not be available again.");
            definitions.Add(411, "The request did not specify the length of its content, which is required by the requested resource.");
            definitions.Add(412, "The server does not meet one of the preconditions that the requester put on the request.");
            definitions.Add(413, "The request is larger than the server is willing or able to process.");
            definitions.Add(414, "The URI provided was too long for the server to process.");
            definitions.Add(415, "The request entity has a media type which the server or resource does not support.");
            definitions.Add(416, "The client has asked for a portion of the file, but the server cannot supply that portion.");
            definitions.Add(417, "The server cannot meet the requirements of the Expect request-header field.");
            definitions.Add(418, "This code was defined in 1998 as one of the traditional IETF April Fools' jokes, in RFC 2324, Hyper Text Coffee Pot Control Protocol, and is not expected to be implemented by actual HTTP servers.");
            definitions.Add(420, "Not part of the HTTP standard, but returned by the Twitter Search and Trends API when the client is being rate limited.");
            definitions.Add(422, "The request was well-formed but was unable to be followed due to semantic errors.");
            definitions.Add(423, "The resource that is being accessed is locked.");
            definitions.Add(424, "The request failed due to failure of a previous request (e.g., a PROPPATCH).");
            definitions.Add(426, "The client should switch to a different protocol such as TLS/1.0.");
            definitions.Add(428, "The origin server requires the request to be conditional.");
            definitions.Add(429, "The user has sent too many requests in a given amount of time.");
            definitions.Add(431, "The server is unwilling to process the request because either an individual header field, or all the header fields collectively, are too large.");
            definitions.Add(440, "A Microsoft extension. Indicates that your session has expired.");
            definitions.Add(450, "A Microsoft extension. This error is given when Windows Parental Controls are turned on and are blocking access to the given webpage.");
            definitions.Add(451, "Defined in the internet draft \"A New HTTP Status Code for Legally-restricted Resources\".");
            definitions.Add(494, "Nginx internal code similar to 431 but it was introduced earlier.");
            definitions.Add(500, "A generic error message, given when an unexpected condition was encountered and no more specific message is suitable.");
            definitions.Add(501, "The server either does not recognize the request method, or it lacks the ability to fulfill the request.");
            definitions.Add(502, "The server was acting as a gateway or proxy and received an invalid response from the upstream server.");
            definitions.Add(503, "The server is currently unavailable (because it is overloaded or down for maintenance).");
            definitions.Add(504, "The server was acting as a gateway or proxy and did not receive a timely response from the upstream server.");
            definitions.Add(505, "The server does not support the HTTP protocol version used in the request.");
            definitions.Add(506, "Transparent content negotiation for the request results in a circular reference.");
            definitions.Add(507, "The server is unable to store the representation needed to complete the request.");
            definitions.Add(508, "The server detected an infinite loop while processing the request (sent in lieu of 208 Not Reported).");
            definitions.Add(511, "The client needs to authenticate to gain network access. Intended for use by intercepting proxies used to control access to the network.");

            names = new Dictionary<int, string>();
            names.Add(100, "Continue");
            names.Add(101, "Switching protocols");
            names.Add(102, "Processing");
            names.Add(200, "OK");
            names.Add(201, "Created");
            names.Add(202, "Accepted");
            names.Add(203, "Non-Authoritative Information");
            names.Add(204, "No Content");
            names.Add(205, "Reset Content");
            names.Add(206, "Partial Content");
            names.Add(207, "Multi-Status");
            names.Add(208, "Already Reported");
            names.Add(300, "Multiple Choices");
            names.Add(301, "Moved Permanently");
            names.Add(302, "Found");
            names.Add(303, "See Other");
            names.Add(304, "Not Modified");
            names.Add(305, "Use Proxy");
            names.Add(307, "Temporary Redirect");
            names.Add(308, "Permanent Redirect");
            names.Add(400, "Bad Request");
            names.Add(401, "Unauthorized");
            names.Add(402, "Payment Required");
            names.Add(403, "Forbidden");
            names.Add(404, "Not Found");
            names.Add(405, "Method Not Allowed");
            names.Add(406, "Not Acceptable");
            names.Add(407, "Proxy Authentication Required");
            names.Add(408, "Request Timeout");
            names.Add(409, "Conflict");
            names.Add(410, "Gone");
            names.Add(411, "Length Required");
            names.Add(412, "Precondition Failed");
            names.Add(413, "Request Entity Too Large");
            names.Add(414, "Request-URI Too Long");
            names.Add(415, "Unsupported Media Type");
            names.Add(416, "Requested Range Not Satisfiable");
            names.Add(417, "Expectation Failed");
            names.Add(418, "I'm a teapot");
            names.Add(420, "Method Failure");
            names.Add(422, "Unprocessable Entity");
            names.Add(423, "Locked");
            names.Add(424, "Failed Dependency");
            names.Add(426, "Upgrade Required");
            names.Add(428, "Precondition Required");
            names.Add(429, "Too Many Requests");
            names.Add(431, "Request Header Fields Too Large");
            names.Add(440, "Login Timeout");
            names.Add(450, "Blocked by Windows Parental Controls");
            names.Add(451, "Unavailable For Legal Reasons");
            names.Add(494, "Request Header Too Large");
            names.Add(500, "Internal Server Error");
            names.Add(501, "Not Implemented");
            names.Add(502, "Bad Gateway");
            names.Add(503, "Service Unavailable");
            names.Add(504, "Gateway Timeout");
            names.Add(505, "HTTP Version Not Supported");
            names.Add(506, "Variant Also Negotiates");
            names.Add(507, "Insufficient Storage");
            names.Add(508, "Loop Detected");
            names.Add(511, "Network Authentication Required");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpErrorModel"/> class.
        /// </summary>
        public HttpErrorModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpErrorModel"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        public HttpErrorModel(string title, string message)
        {
            this.Title = title;
            this.Message = message;
        }

        /// <summary>
        /// Gets or sets the main page title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the main message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the HTTP code.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets the URL path.
        /// </summary>
        public string UrlPath { get; set; }

        /// <summary>
        /// Gets or sets the top link title.
        /// </summary>
        public string TopLinkTitle { get; set; }

        /// <summary>
        /// Gets or sets the top link href.
        /// </summary>
        public string TopLinkHref { get; set; }

        /// <summary>
        /// Gets or sets the error action.
        /// </summary>
        public string ErrorAction { get; set; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        public Exception Exception { get; set; }

        public string CodeName { get; set; }
        public string CodeDefinition { get; set; }

        public static HttpErrorModel Create(int code, string title, string message)
        {
            return new HttpErrorModel(title, message)
            {
                CodeName = names.ContainsKey(code) ? names[code] : null,
                CodeDefinition = definitions.ContainsKey(code) ? definitions[code] : null,
            };
        }
    }
}
