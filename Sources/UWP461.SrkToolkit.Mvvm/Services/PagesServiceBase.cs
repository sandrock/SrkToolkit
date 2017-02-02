// 
// Copyright 2014 SandRock
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 

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
