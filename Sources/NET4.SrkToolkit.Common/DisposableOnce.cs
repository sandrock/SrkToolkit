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

namespace SrkToolkit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Diagnostics;
    using SrkToolkit.Internals;

    /// <summary>
    /// A disposable object that will cann a delegate on disposal.
    /// </summary>
    public sealed class DisposableOnce : IDisposable
    {
        private readonly object sync = new object();
        private bool isDisposed;
        private Action onDisposed1;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableOnce"/> class.
        /// </summary>
        /// <param name="onDisposed">The configuration disposed.</param>
        /// <exception cref="System.ArgumentNullException">The delegate to call on dispose</exception>
        public DisposableOnce(Action onDisposed)
        {
            if (onDisposed == null)
                throw new ArgumentNullException("onDisposed");

            this.onDisposed1 = onDisposed;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DisposableOnce"/> class.
        /// </summary>
        ~DisposableOnce()
        {
            lock (this.sync)
            {
                this.onDisposed1 = null;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            lock (this.sync)
            {
                if (!this.isDisposed)
                {
                    this.isDisposed = true;

                    if (disposing)
                    {
                        if (this.onDisposed1 != null)
                        {
                            try
                            {
                                this.onDisposed1();
                            }
                            catch (Exception ex)
                            {
                                Trace.WriteLine("DisposableOnce caught exception in dispose callback" + Environment.NewLine + ex.ToString());
                                throw;
                            }

                            this.onDisposed1 = null;
                        }
                    }
                } 
            }
        }
    }
}
