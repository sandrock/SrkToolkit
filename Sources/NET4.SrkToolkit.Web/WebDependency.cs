
namespace SrkToolkit.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class WebDependency : IEnumerable<WebDependencyFile>
    {
        public WebDependency()
        {
        }

        public WebDependency(string name)
        {
            this.Name = name;
        }

        public WebDependency(string name, IEnumerable<WebDependencyFile> files)
        {
            this.Name = name;
            this.Files = new List<WebDependencyFile>(files);
        }

        public void Add(WebDependencyFile file)
        {
            if (this.Files == null)
                this.Files = new List<WebDependencyFile>();
            this.Files.Add(file);
        }

        public string Name { get; set; }
        public IList<WebDependencyFile> Files { get; set; }

        public IEnumerator<WebDependencyFile> GetEnumerator()
        {
            return this.Files.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.Files.GetEnumerator();
        }
    }
}
