
namespace SrkToolkit.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Routing;

    /// <summary>
    /// A file specification in a <see cref="WebDependency" /> package.
    /// </summary>
    public class WebDependencyFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebDependencyFile"/> class.
        /// </summary>
        public WebDependencyFile()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDependencyFile" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="type">The type.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="attributes">The attributes.</param>
        public WebDependencyFile(string path, WebDependencyFileType type, Encoding encoding = null, IDictionary<string, object> attributes = null)
        {
            this.Path = path;
            this.Type = type;
            this.Encoding = encoding;
            this.Attributes = attributes;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDependencyFile" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="type">The type.</param>
        /// <param name="attributes">The attributes.</param>
        public WebDependencyFile(string path, WebDependencyFileType type, params string[] attributes)
        {
            this.Path = path;
            this.Type = type;
            this.Attributes = ReadAttributes(attributes);
        }

        private static IDictionary<string, object> ReadAttributes(string[] attributes)
        {
            if (attributes.Length % 2 != 0)
                throw new ArgumentException("Invalid parameters count", "attributes");

            Dictionary<string, object> parameters = null;
            if (attributes.Length > 0)
            {
                parameters = new Dictionary<string, object>();
                bool isKey = true;
                string key = null;
                foreach (var item in attributes)
                {
                    if (isKey)
                    {
                        key = item;
                    }
                    else
                    {
                        parameters.Add(key, item);
                    }

                    isKey = !isKey;
                }
            }

            return parameters;
        }

        /// <summary>
        /// Gets or sets the path to the file.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the type of file.
        /// </summary>
        public WebDependencyFileType Type { get; set; }

        /// <summary>
        /// Gets or sets the encoding.
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Gets or sets the media.
        /// </summary>
        public WebDependencyMedia? Media { get; set; }

        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        public IDictionary<string, object> Attributes { get; set; }
    }

    /// <summary>
    /// The type of web dependency file.
    /// </summary>
    public enum WebDependencyFileType
    {
        /// <summary>
        /// A javascript file.
        /// </summary>
        Javascript,

        /// <summary>
        /// A CSS file.
        /// </summary>
        Css,
    }

    /// <summary>
    /// The HTML page position to include web dependencies.
    /// </summary>
    public enum WebDependencyPosition
    {
        /// <summary>
        /// In the &lt;head&gt; tag.
        /// </summary>
        Head,

        /// <summary>
        /// At the beginning of the &lt;body&gt; tag.
        /// </summary>
        StartOfPage,

        /// <summary>
        /// At the end of the &lt;body&gt; tag.
        /// </summary>
        EndOfPage,
    }

    /// <summary>
    /// See http://www.w3.org/TR/CSS2/media.html
    /// </summary>
    [Flags]
    public enum WebDependencyMedia
    {
        /// <summary>
        /// Suitable for all devices. 
        /// </summary>
        All        = 0x0001,

        /// <summary>
        /// Intended for braille tactile feedback devices.
        /// </summary>
        Braille    = 0x0002,

        /// <summary>
        /// Intended for paged braille printers. 
        /// </summary>
        Embossed   = 0x0004,

        /// <summary>
        /// Intended for handheld devices (typically small screen, limited bandwidth). 
        /// </summary>
        Handheld   = 0x0008,

        /// <summary>
        /// Intended for paged material and for documents viewed on screen in print preview mode.
        /// </summary>
        Print      = 0x0010,

        /// <summary>
        /// Intended for projected presentations, for example projectors.
        /// </summary>
        Projection = 0x0020,

        /// <summary>
        /// Intended primarily for color computer screens. 
        /// </summary>
        Screen     = 0x0040,

        /// <summary>
        /// Intended for speech synthesizers.
        /// </summary>
        Speech     = 0x0080,

        /// <summary>
        /// Intended for media using a fixed-pitch character grid (such as teletypes, terminals, or portable devices with limited display capabilities).
        /// </summary>
        Tty        = 0x0100,

        /// <summary>
        /// Intended for television-type devices (low resolution, color, limited-scrollability screens, sound available). 
        /// </summary>
        TV         = 0x0200,
    }
}
