﻿// 
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

namespace SrkToolkit.Web.Open
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

#if ASPMVCCORE

#else
    using System.Web.Mvc;
#endif

#if ASPMVCCORE
#else
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SrkOpenGraphHtmlExtensions
    {
        private HtmlHelper html;

        public SrkOpenGraphHtmlExtensions(HtmlHelper html)
        {
            this.html = html;
        }
    }
#endif
}
