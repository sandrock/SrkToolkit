using System.Collections.Generic;
using System.IO;
using System.Net;

namespace SrkToolkit.WildServiceRef {

    /// <summary>
    /// Represents a wrapper to execute HTTP requests.
    /// </summary>
    public interface IHttpWrapper {

        /// <summary>
        /// Download a response as a string.
        /// </summary>
        /// <param name="url">Service action</param>
        /// <returns>HTTP response body as a string.</returns>
        string GetString(string url);

        /// <summary>
        /// Download a response as a string with POST data.
        /// </summary>
        /// <param name="url">Service action</param>
        /// <param name="postParameters">POST data parameters</param>
        /// <returns>HTTP response body as a string.</returns>
        string GetString(string url, Dictionary<string, string> postParameters);

        /// <summary>
        /// Download a response as a string with POST data.
        /// </summary>
        /// <param name="url">Service action</param>
        /// <param name="postStream">POST data stream</param>
        /// <returns>HTTP response body as a string.</returns>
        string GetString(string url, Stream postStream);

        /// <summary>
        /// Get a response stream.
        /// </summary>
        /// <param name="url">Service action</param>
        /// <returns>HTTP response content as a stream.</returns>
        Stream GetStream(string url);

        /// <summary>
        /// Get a response stream with POST data.
        /// </summary>
        /// <param name="url">Service action</param>
        /// <param name="postParameters">POST data parameters</param>
        /// <returns>HTTP response content as a stream.</returns>
        Stream GetStream(string url, Dictionary<string, string> postParameters);

        /// <summary>
        /// Get a response stream with POST data.
        /// </summary>
        /// <param name="url">Service action</param>
        /// <param name="postStream">POST data stream</param>
        /// <returns>HTTP response content as a stream.</returns>
        Stream GetStream(string url, Stream postStream);

        /// <summary>
        /// Get a raw response.
        /// </summary>
        /// <param name="url">Service action</param>
        /// <returns>Whole HTTP response.</returns>
        HttpWebResponse GetResponse(string url);

        /// <summary>
        /// Get a raw response with POST data.
        /// </summary>
        /// <param name="url">Service action</param>
        /// <param name="postParameters">POST data parameters</param>
        /// <returns>Whole HTTP response.</returns>
        HttpWebResponse GetResponse(string url, Dictionary<string, string> postParameters);

        /// <summary>
        /// Get a raw response with POST data.
        /// </summary>
        /// <param name="url">Service action</param>
        /// <param name="postStream">POST data stream</param>
        /// <returns>Whole HTTP response.</returns>
        HttpWebResponse GetResponse(string url, Stream postStream);

        /// <summary>
        /// Get a raw response.
        /// </summary>
        /// <param name="url">Service action</param>
        /// <returns>Whole HTTP response.</returns>
        HttpWebResponse Execute(string url);

        /// <summary>
        /// Get a raw response with POST data.
        /// </summary>
        /// <param name="url">Service action</param>
        /// <param name="postParameters">POST data parameters</param>
        /// <returns>Whole HTTP response.</returns>
        HttpWebResponse Execute(string url, Dictionary<string, string> postParameters);

        /// <summary>
        /// Get a raw response with POST data.
        /// </summary>
        /// <param name="url">Service action</param>
        /// <param name="postStream">POST data stream</param>
        /// <returns>Whole HTTP response.</returns>
        HttpWebResponse Execute(string url, Stream postStream);

    }
}
