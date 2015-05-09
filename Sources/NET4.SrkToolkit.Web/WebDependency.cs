
namespace SrkToolkit.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A collection of file specification to include in a web render.
    /// </summary>
    public class WebDependency : IEnumerable<WebDependencyFile>
    {
        private WebDependencyPosition defaultPosition = WebDependencyPosition.EndOfPage;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="WebDependency"/> class.
        /// </summary>
        public WebDependency()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDependency"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public WebDependency(string name)
            : this(name, WebDependencyPosition.EndOfPage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDependency"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="defaultPosition">The default position.</param>
        public WebDependency(string name, WebDependencyPosition defaultPosition)
        {
            this.Name = name;
            this.defaultPosition = defaultPosition;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDependency" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="defaultPosition">The default position.</param>
        /// <param name="order">The order of rendering.</param>
        public WebDependency(string name, WebDependencyPosition defaultPosition, int order)
        {
            this.Name = name;
            this.defaultPosition = defaultPosition;
            this.Order = order;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDependency"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="files">The files.</param>
        public WebDependency(string name, IEnumerable<WebDependencyFile> files)
        {
            this.Name = name;
            this.Files = new List<WebDependencyFile>(files);
        }

        /// <summary>
        /// Adds the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        public void Add(WebDependencyFile file)
        {
            if (this.Files == null)
                this.Files = new List<WebDependencyFile>();
            this.Files.Add(file);
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the default position.
        /// </summary>
        public WebDependencyPosition DefaultPosition
        {
            get { return this.defaultPosition; }
            set { this.defaultPosition = value; }
        }

        /// <summary>
        /// Gets or sets the files.
        /// </summary>
        public IList<WebDependencyFile> Files { get; set; }

        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<WebDependencyFile> GetEnumerator()
        {
            return this.Files.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.Files.GetEnumerator();
        }
    }
}
