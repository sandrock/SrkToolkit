
namespace SrkToolkit.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A file specification in a <see cref="WebDependency"/> package.
    /// </summary>
    public class WebDependencyFile
    {
        public WebDependencyFile()
        {
        }

        public WebDependencyFile(string path, WebDependencyFileType type, Encoding encoding = null)
        {
            this.Path = path;
            this.Type = type;
            this.Encoding = encoding;
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
    }

    public enum WebDependencyFileType
    {
        Javascript,
        Css,
    }

    public enum WebDependencyPosition
    {
        Head,
        StartOfPage,
        EndOfPage,
    }

    /// <summary>
    /// See http://www.w3.org/TR/CSS2/media.html
    /// </summary>
    [Flags]
    public enum WebDependencyMedia
    {
        All        = 0x0001,
        Braille    = 0x0002,
        Embossed   = 0x0004,
        Handheld   = 0x0008,
        Print      = 0x0010,
        Projection = 0x0020,
        Screen     = 0x0040,
        Speech     = 0x0080,
        Tty        = 0x0100,
        TV         = 0x0200,
    }
}
