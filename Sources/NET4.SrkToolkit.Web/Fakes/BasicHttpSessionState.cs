
namespace SrkToolkit.Web.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    public class BasicHttpSessionState : HttpSessionStateBase
    {
        private Dictionary<string, object> values = new Dictionary<string, object>();

        public BasicHttpSessionState()
        {
        }

        public BasicHttpSessionState(Dictionary<string, object> values)
        {
            this.values = values;
        }

        public override void Add(string name, object value)
        {
            this.values.Add(name, value);
        }

        public override object this[string name]
        {
            get
            {
                return this.values.ContainsKey(name) ? this.values[name] : null;
            }
            set
            {
                this.values[name] = value;
            }
        }

        public override void Clear()
        {
            this.values.Clear();
        }
    }
}
