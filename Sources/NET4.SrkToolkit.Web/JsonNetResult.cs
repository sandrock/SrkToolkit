
namespace SrkToolkit.Web
{
    using System;
    using System.Diagnostics;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// A <see cref="ActionResult"/> implementation provider-oriented to render JSON responses.
    /// </summary>
    /// <remarks>
    /// To use Newtonwoft.Json, call
    /// 
    /// JsonNetResult.Serializer = (obj, response) => 
    /// {
    ///     JsonTextWriter writer = new JsonTextWriter(response.Output)
    ///     {
    ///         Formatting = this.Formatting
    ///     };
    ///     JsonSerializer serializer = JsonSerializer.Create(this.SerializerSettings);
    ///     serializer.Serialize(writer, this.Data);
    ///     writer.Flush();
    /// }
    /// </remarks>
    public class JsonNetResult : ActionResult
    {
        private int httpStatusCode = 200;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetResult"/> class.
        /// </summary>
        public JsonNetResult()
        {
        }

        /// <summary>
        /// Gets or sets the JSON serializer.
        /// Defaults to <see cref="DataContractJsonSerializer"/>.
        /// Easily replacable by any other.
        /// </summary>
        /// <value>
        /// The JSON serializer.
        /// </value>
        public static Action<object, HttpResponseBase> Serializer { get; set; }

        /// <summary>
        /// Gets or sets the content encoding.
        /// Defaults to unicode is not specified.
        /// </summary>
        /// <value>
        /// The content encoding.
        /// </value>
        public Encoding ContentEncoding { get; set; }

        /// <summary>
        /// Gets or sets the type of the content.
        /// Defaults to application/jsonif this value is null.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the data to serialize.
        /// </summary>
        public object Data { get; set; }

        public int HttpStatusCode
        {
            get { return this.httpStatusCode; }
            set { this.httpStatusCode = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is in debug mode.
        /// Debug mode will instruct the serializer to indent JSON.
        /// </summary>
        public static bool IsDebug
        {
            get
            {
                Func<bool> isDebug = () => false;
                IsDebugCheck(ref isDebug);

                return isDebug();
            }
        }

        [Conditional("DEBUG")]
        private static void IsDebugCheck(ref Func<bool> func)
        {
            func = () => true;
        }

        /// <summary>
        /// Processing of the result of an action method.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <exception cref="System.ArgumentNullException">context</exception>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            HttpResponseBase response = context.HttpContext.Response;
            response.StatusCode = this.HttpStatusCode;

            response.ContentType = !string.IsNullOrEmpty(this.ContentType)
                                 ? ContentType
                                 : "application/json";

            if (this.ContentEncoding != null)
                response.ContentEncoding = this.ContentEncoding;

            if (this.Data != null)
            {
                if (Serializer != null)
                {
                    Serializer(this.Data, response);
                }
                else
                {
                    var ser = new DataContractJsonSerializer(this.Data.GetType());
                    ser.WriteObject(response.OutputStream, this.Data);
                }
            }
        }
    }
}
