
namespace SrkToolkit.Web.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    public class BasicHttpRequest : HttpRequestBase
    {
        public BasicHttpRequest()
        {
        }

        public override string[] AcceptTypes
        {
            get
            {
                return this.AcceptTypesCollection;
            }
        }

        public string[] AcceptTypesCollection { get; set; }
    }
}
