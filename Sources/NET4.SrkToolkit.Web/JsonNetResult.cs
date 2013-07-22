
namespace SrkToolkit.Web
{
    using System;
    using System.Diagnostics;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// 
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
        public JsonNetResult()
        {
        }

        public static Action<object, HttpResponseBase> Serializer { get; set; }

        public Encoding ContentEncoding { get; set; }

        public string ContentType { get; set; }

        public object Data { get; set; }

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

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            HttpResponseBase response = context.HttpContext.Response;

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
