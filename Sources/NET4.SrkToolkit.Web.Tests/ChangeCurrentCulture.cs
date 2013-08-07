// -----------------------------------------------------------------------
// <copyright file="ChangeCurrentCulture.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

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
