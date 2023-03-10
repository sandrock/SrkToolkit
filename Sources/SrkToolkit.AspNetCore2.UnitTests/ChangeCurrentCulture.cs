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

namespace SrkToolkit.Web.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Globalization;
    using System.Threading;

    public class ChangeCurrentCulture : IDisposable
    {
        private CultureInfo restoreCulture;
        private CultureInfo restoreUiCulture;

        private bool disposed;

        public ChangeCurrentCulture(CultureInfo culture)
        {
            this.restoreCulture = Thread.CurrentThread.CurrentCulture;
            this.restoreUiCulture = Thread.CurrentThread.CurrentUICulture;

            this.ApplyCulture(culture, culture);
        }

        private void ApplyCulture(CultureInfo culture, CultureInfo uiCulture)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = uiCulture;
        }

        #region IDisposable members

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.ApplyCulture(this.restoreCulture, this.restoreUiCulture);
                }

                this.disposed = true;
            }
        }

        #endregion
    }
}
