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

namespace SrkToolkit.Services
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A basic subscribe/publish event bus.
    /// </summary>
    public class AppEventBus
    {
        /// <summary>
        /// A subscription container.
        /// </summary>
        internal class Subscription : IDisposed
        {
#if SILVERLIGHT || NETFX_CORE
            internal string EventType { get; set; }
#else
            internal Guid EventType { get; set; }
#endif
            internal object Action { get; set; }
            internal AppEventBus Bus { get; set; }
            internal object Registree { get; set; }

            void IDisposable.Dispose()
            {
                if (this.Bus != null)
                    this.Bus.Unregister(this);
                this.Bus = null;
            }

            bool IDisposed.IsDisposed
            {
                get { return this.Bus == null; }
            }
        }

        private static AppEventBus instance;

#if SILVERLIGHT || NETFX_CORE
        private readonly Dictionary<string, List<Subscription>> subscriptions = new Dictionary<string, List<Subscription>>();
#else
        private readonly Dictionary<Guid, List<Subscription>> subscriptions = new Dictionary<Guid, List<Subscription>>();
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="AppEventBus"/> class.
        /// </summary>
        public AppEventBus()
        {
        }

        /// <summary>
        /// Gets the default instance.
        /// </summary>
        public static AppEventBus Instance
        {
            get { return instance ?? (instance = new AppEventBus()); }
        }

        /// <summary>
        /// Publishes the specified event.
        /// </summary>
        /// <typeparam name="T">the type of event</typeparam>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The arguments.</param>
        public void Publish<T>(object sender, T args)
        {
            var id = GetTypeId(typeof(T));
            
            foreach (var item in this.GetListForType(id))
            {
                if (item.Registree != null && item.Registree == sender)
                    continue;

                var evt = (Action<object, T>)item.Action;
                evt.Invoke(sender, args);
            }
        }

        /// <summary>
        /// Registers for an event.
        /// </summary>
        /// <typeparam name="T">the type of event</typeparam>
        /// <param name="action">The action.</param>
        /// <returns>object to dispose to unsubscribe</returns>
        /// <exception cref="ArgumentNullException">argument is null</exception>
        public IDisposed Register<T>(Action<object, T> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            var id = GetTypeId(typeof(T));
            var reg = new Subscription
            {
                Bus = this,
                Action = action,
                EventType = id,
            };
            this.GetListForType(id).Add(reg);

            return reg;
        }

        /// <summary>
        /// Registers for an event and a registree (preventing echo).
        /// </summary>
        /// <typeparam name="T">the type of event</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="registree">The registree.</param>
        /// <returns>object to dispose to unsubscribe</returns>
        /// <exception cref="ArgumentNullException">argument is null</exception>
        public IDisposed Register<T>(Action<object, T> action, object registree)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (registree == null)
                throw new ArgumentNullException("registree");

            var id = GetTypeId(typeof(T));
            var reg = new Subscription
            {
                Bus = this,
                Action = action,
                EventType = id,
                Registree = registree
            };
            this.GetListForType(id).Add(reg);

            return reg;
        }

        /// <summary>
        /// Unregisters from the specified subscription.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        internal void Unregister(Subscription subscription)
        {
            var list = GetListForType(subscription.EventType);
            if (list.Contains(subscription))
                list.Remove(subscription);
        }

#if SILVERLIGHT || NETFX_CORE
        private List<Subscription> GetListForType(string id)
        {
            if (!subscriptions.ContainsKey(id))
                subscriptions[id] = new List<Subscription>();
            return subscriptions[id];
        }
#else
        private List<Subscription> GetListForType(Guid id)
        {
            if (!subscriptions.ContainsKey(id))
                subscriptions[id] = new List<Subscription>();
            return subscriptions[id];
        }
#endif

#if SILVERLIGHT || NETFX_CORE
        private static string GetTypeId(Type type)
        {
            var id = type.FullName;
            return id;
        }
#else
        private static Guid GetTypeId(Type type)
        {
            var id = type.GUID;
            return id;
        }
#endif
    }
}
