
namespace SrkToolkit.Mvvm.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Services around pages.
    /// </summary>
    public class PagesServiceBase
    {
        private static PagesServiceBase defaultInstance;

        private readonly Dictionary<Type, Type> modelToPage = new Dictionary<Type, Type>();

        public PagesServiceBase()
        {
        }

        public static PagesServiceBase Default
        {
            get { return defaultInstance ?? (defaultInstance = new PagesServiceBase()); }
        }

        public PagesServiceBase RegisterPageAndModel<TPage, TModel>()
        {
            return this.RegisterPageAndModel<TPage, TModel>(null);
        }

        public PagesServiceBase RegisterPageAndModel<TPage, TModel>(string url)
        {
            var page = typeof(TPage);
            var model = typeof(TModel);
            if (!this.modelToPage.ContainsKey(model))
            {
                this.modelToPage.Add(model, page);
            }

            return this;
        }

        public object CreateModel<TPage>()
        {
            var page = typeof(TPage);
            Type modelType = null;
            foreach (var item in this.modelToPage)
            {
                if (item.Value == page)
                {
                    modelType = item.Key;
                    break;
                }
            }

            if (modelType != null)
            {
                return Activator.CreateInstance(modelType);
            }
            else
            {
                return null;
            }
        }

        public Type GetModelsPageType<TModel>()
        {
            var model = typeof(TModel);
            if (this.modelToPage.ContainsKey(model))
            {
                return this.modelToPage[model];
            }

            return null;
        }
    }
}
