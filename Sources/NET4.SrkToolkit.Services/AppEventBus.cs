using System;
using System.Collections.Generic;

namespace SrkToolkit.Services {
    public class AppEventBus {
        internal class Subscription : IDisposed {
            internal Guid EventType { get; set; }
            internal object Action { get; set; }
            internal AppEventBus Bus { get; set; }
            internal object Registree { get; set; }
            void IDisposable.Dispose() {
                if (this.Bus != null)
                    this.Bus.Unregister(this);
                this.Bus = null;
            }
            bool IDisposed.IsDisposed {
                get { return this.Bus == null; }
            }
        }

        private static AppEventBus instance;
        private readonly Dictionary<Guid, List<Subscription>> subscriptions = new Dictionary<Guid, List<Subscription>>();

        public AppEventBus() {
        }

        public static AppEventBus Instance {
            get { return instance ?? (instance = new AppEventBus()); }
        }

        private List<Subscription> GetListForType(Guid id) {
            if (!subscriptions.ContainsKey(id))
                subscriptions[id] = new List<Subscription>();
            return subscriptions[id];
        }

        public void Publish<T>(object sender, T args) {
            var id = typeof(T).GUID;
            foreach (var item in GetListForType(id)) {
                if (item.Registree != null && item.Registree == sender)
                    continue;

                var evt = (Action<object, T>)item.Action;
                evt.Invoke(sender, args);
            }
        }

        public IDisposed Register<T>(Action<object, T> action) {
            if (action == null)
                throw new ArgumentNullException("action");

            var id = typeof(T).GUID;
            var reg = new Subscription {
                Bus = this, Action = action, EventType = id
            };
            this.GetListForType(id).Add(reg);
            return reg;
        }

        public IDisposed Register<T>(Action<object, T> action, object registree) {
            if (action == null)
                throw new ArgumentNullException("action");
            if (registree == null)
                throw new ArgumentNullException("registree");

            var id = typeof(T).GUID;
            var reg = new Subscription {
                Bus = this, Action = action, EventType = id, Registree = registree
            };
            this.GetListForType(id).Add(reg);
            return reg;
        }
    
        internal void Unregister(Subscription subscription)
        {
            var list = GetListForType(subscription.EventType);
            if (list.Contains(subscription))
                list.Remove(subscription);
        }
    }
}
