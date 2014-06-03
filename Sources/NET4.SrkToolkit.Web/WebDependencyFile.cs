
namespace SrkToolkit.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

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

        public string Path { get; set; }
        public WebDependencyFileType Type { get; set; }
        public Encoding Encoding { get; set; }
    }

    public enum WebDependencyFileType
    {
        Javascript,
        Css,
    }

    public enum WebDependencyPosition
    {
        EndOfPage,
    }
}
