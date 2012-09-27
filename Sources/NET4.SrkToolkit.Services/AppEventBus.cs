using System;
using System.Collections.Generic;

namespace SrkToolkit.Services
{
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
            internal Guid EventType { get; set; }
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

        private readonly Dictionary<Guid, List<Subscription>> subscriptions = new Dictionary<Guid, List<Subscription>>();

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
            var id = typeof(T).GUID;

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

            var id = typeof(T).GUID;
            var reg = new Subscription
            {
                Bus = this,
                Action = action,
                EventType = id
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

            var id = typeof(T).GUID;
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

        private List<Subscription> GetListForType(Guid id)
        {
            if (!subscriptions.ContainsKey(id))
                subscriptions[id] = new List<Subscription>();
            return subscriptions[id];
        }
    }
}
