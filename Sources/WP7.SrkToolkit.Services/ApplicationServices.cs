
namespace SrkToolkit.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// Permits to store references to application-wide services.
    /// </summary>
    public static class ApplicationServices
    {
#if SILVERLIGHT || NETFX_CORE
        private static readonly Dictionary<string, IFactory> factories = new Dictionary<string, IFactory>();
        private static readonly Dictionary<string, object> services = new Dictionary<string, object>();
#else
        private static readonly Dictionary<Guid, IFactory> factories = new Dictionary<Guid, IFactory>();
        private static readonly Dictionary<Guid, object> services = new Dictionary<Guid, object>();
#endif

        /// <summary>
        /// The lock reference to access <see cref="services"/> and <see cref="factories"/>.
        /// </summary>
        private static readonly object internals = new object();
        private static readonly object syncRoot = new object();

        /// <summary>
        /// Returns a lock cookie to allow thread-safe manipulations.
        /// </summary>
        /// <returns></returns>
        public static IDisposable SyncRoot()
        {
            Monitor.Enter(syncRoot);
            return new Lock(() => Monitor.Exit(syncRoot));
        }

        /// <summary>
        /// Determines whether a service is registered (the factory exists).
        /// </summary>
        /// <typeparam name="TInterface">The resolving type (interface).</typeparam>
        /// <returns>
        ///   <c>true</c> if a service has been registered with the specified interface; otherwise, <c>false</c>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsRegistered<TInterface>()
            where TInterface : class
        {
            var type = typeof(TInterface);
            var id = GetTypeId(type);
            lock (internals)
            {
                return factories.ContainsKey(id) && factories[id] != null || services.ContainsKey(id) && services[id] != null;
            }
        }

        /// <summary>
        /// Determines whether a service is registered (the factory exists).
        /// </summary>
        /// <typeparam name="TInterface">The resolving type (interface).</typeparam>
        /// <returns>
        ///   <c>true</c> if a service has been registered with the specified interface; otherwise, <c>false</c>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsRegistered<TInterface>(Action<bool> action)
            where TInterface : class
        {
            if (action == null)
                throw new ArgumentNullException("action");

            var type = typeof(TInterface);
            var id = GetTypeId(type);
            lock (internals)
            {
                var result = factories.ContainsKey(id) && factories[id] != null;
                action(result);
                return result;
            }
        }

        /// <summary>
        /// Determines whether a service is instantiated (the instance exists).
        /// </summary>
        /// <typeparam name="TInterface">The resolving type (interface).</typeparam>
        /// <returns>
        ///   <c>true</c> if a service has been registered with the specified interface; otherwise, <c>false</c>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsReady<TInterface>()
            where TInterface : class
        {
            var type = typeof(TInterface);
            var id = GetTypeId(type);
            lock (internals)
            {
                return services.ContainsKey(id) && services[id] != null;
            }
        }

        /// <summary>
        /// Registers the a service instance with a specified interface type.
        /// No factory is registered.
        /// </summary>
        /// <typeparam name="TInterface">The resolving type (interface).</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation (for internal consistency).</typeparam>
        /// <param name="service">The reference to the service instance.</param>
        /// <exception cref="ArgumentException">If service or factory already exists</exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Register<TInterface, TImplementation>(TImplementation service)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            if (service == null)
                throw new ArgumentNullException("service");

            var type = typeof(TInterface);
            var id = GetTypeId(type);
            lock (internals)
            {
                ThrowIfRegisteredOrInstanciated(type, id);

                services.Add(id, service);
                TraceEx.Info("ApplicationServices", "Registered instance for " + type.Name);
            }
        }

        /// <summary>
        /// Registers a service with a specified resolving type and instance.
        /// </summary>
        /// <typeparam name="TImplementation">The resolving type (interface).</typeparam>
        /// <param name="service">The reference to the service instance.</param>
        /// <exception cref="ArgumentException">If service or factory already exists</exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Register<TImplementation>(TImplementation service)
            where TImplementation : class
        {
            if (service == null)
                throw new ArgumentNullException("service");

            var type = typeof(TImplementation);
            var id = GetTypeId(type);
            lock (internals)
            {
                ThrowIfRegisteredOrInstanciated(type, id);

                services.Add(id, service);
                TraceEx.Info("ApplicationServices", "Registered instance for " + type.Name);
            }
        }

        /// <summary>
        /// Registers a service with a specified interface type.
        /// The service is automatically instanciated on the first call to it using the default constructor.
        /// </summary>
        /// <typeparam name="TInterface">The resolving type (interface).</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <exception cref="ArgumentException">If service or factory already exists</exception>
        public static void Register<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : class, TInterface, new()
        {
            var type = typeof(TInterface);
            var implType = typeof(TImplementation);
            var id = GetTypeId(type);
            lock (internals)
            {
                ThrowIfRegisteredOrInstanciated(type, id);

                factories.Add(id, new LazyEmptyService(implType));
                TraceEx.Info("ApplicationServices", "Registered factory for " + type.Name);
            }
        }

        /// <summary>
        /// Registers a service with a specified interface type.
        /// The service is automatically instanciated on the first call to it.
        /// </summary>
        /// <typeparam name="TInterface">The resolving type (interface).</typeparam>
        /// <param name="factory">The factory for the instantiation.</param>
        /// <exception cref="ArgumentException">If service or factory already exists</exception>
        public static void Register<TInterface>(Func<TInterface> factory)
            where TInterface : class
        {
            var type = typeof(TInterface);
            var id = GetTypeId(type);
            lock (internals)
            {
                ThrowIfRegisteredOrInstanciated(type, id);

#if SILVERLIGHT || NETFX_CORE
                factories.Add(id, new LazyFactoryService(() => factory()));
#else
                factories.Add(id, new LazyFactoryService(factory));
#endif
                TraceEx.Info("ApplicationServices", "Registered factory for " + type.Name);
            }
        }

        /// <summary>
        /// Registers a service with a specified interface type.
        /// The service is automatically instanciated on ALL call to it.
        /// </summary>
        /// <typeparam name="TInterface">The resolving type (interface).</typeparam>
        /// <param name="factory">The factory for the instantiation.</param>
        /// <exception cref="ArgumentException">If service or factory already exists</exception>
        public static void RegisterFactory<TInterface>(Func<TInterface> factory)
            where TInterface : class
        {
            var type = typeof(TInterface);
            var id = GetTypeId(type);
            lock (internals)
            {
                ThrowIfRegisteredOrInstanciated(type, id);

#if SILVERLIGHT || NETFX_CORE
                factories.Add(id, new FactoryFactoryService(() => factory()));
#else
                factories.Add(id, new FactoryFactoryService(factory));
#endif
                TraceEx.Info("ApplicationServices", "Registered factory for " + type.Name);
            }
        }

        /// <summary>
        /// Unregisters a service with from specified interface type.
        /// Drops the instance if it exists (does not call <see cref="IDisposable.Dispose"/>).
        /// </summary>
        /// <typeparam name="TInterface">The resolving type (interface).</typeparam>
        public static void Unregister<TInterface>()
            where TInterface : class
        {
            var type = typeof(TInterface);
            var id = GetTypeId(type);
            lock (internals)
            {
                if (services.ContainsKey(id))
                {
                    services.Remove(id);
                    TraceEx.Info("ApplicationServices", "Removed instance for " + type.Name);
                }

                if (factories.ContainsKey(id))
                {
                    factories.Remove(id);
                    TraceEx.Info("ApplicationServices", "Removed factory for " + type.Name);
                }
            }
        }

        /// <summary>
        /// Drops an instance of a service with the specified reference (does not call <see cref="IDisposable.Dispose"/>).
        /// Does not unregister the factory (the object will be recreated if resolved).
        /// Internaly calls <see cref="DropInstance"/>.
        /// </summary>
        /// <param name="obj">The object to unregister.</param>
        /// <exception cref="ArgumentNullException">if the argument is null</exception>
        [Obsolete("Use the DropInstance method or any overload. The behavior of this method has changed!")]
        public static void Unregister(object obj)
        {
            DropInstance(obj);
        }

        /// <summary>
        /// Drops an instance of a service with the specified reference (does not call <see cref="IDisposable.Dispose"/>).
        /// Does not unregister the factory (the object will be recreated if resolved).
        /// </summary>
        /// <param name="obj">The object to drop.</param>
        /// <exception cref="ArgumentNullException">if the argument is null</exception>
        public static void DropInstance(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            lock (internals)
            {
                foreach (var item in services.ToArray())
                {
                    if (item.Value == obj)
                    {
                        services.Remove(item.Key);
                        TraceEx.Info("ApplicationServices", "Removed instance for " + (item.Value != null ? item.Value.ToString() : item.Key.ToString()));
                    }
                }
            }
        }

        /// <summary>
        /// Drops the instance of a service registered with the TInterface type (does not call <see cref="IDisposable.Dispose"/>).
        /// Does not unregister the factory (the object will be recreated if resolved).
        /// </summary>
        public static void DropInstance<TInterface>()
            where TInterface : class
        {
            var type = typeof(TInterface);
            var id = GetTypeId(type);
            lock (internals)
            {
                if (services.ContainsKey(id))
                {
                    var instance = services[id];
                    services[id] = null;
                    TraceEx.Info("ApplicationServices", "Removed instance of " + (instance != null ? instance.ToString() : id.ToString()));
                }
            }
        }

        /// <summary>
        /// Resolves a service instance with a specified interface type.
        /// </summary>
        /// <typeparam name="TInterface">The resolving type (interface).</typeparam>
        /// <returns>
        ///   if a service has been registered with the specified interface, the reference is returned; otherwise, null is returned.
        /// </returns>
        [DebuggerStepThrough]
        public static TInterface Resolve<TInterface>()
            where TInterface : class
        {
            var type = typeof(TInterface);
            var id = GetTypeId(type);
            lock (internals)
            {
                object obj;
                IFactory factory;
                if (services.ContainsKey(id) && (obj = services[id]) != null)
                {
                    return (TInterface)services[id];
                }
                else if (factories.ContainsKey(id) && (factory = factories[id]) != null)
                {
                    var instance = factory.Create();
                    if (!(factory is FactoryFactoryService))
                        services[id] = instance;
                    TraceEx.Info("ApplicationServices", "Created instance of " + (instance != null ? instance.ToString() : id.ToString()));
                    return (TInterface)instance;
                }
            }

            return null;
        }

        /// <summary>
        /// Resolves a service instance with a specified interface type.
        /// </summary>
        /// <typeparam name="TInterface">The resolving type (interface).</typeparam>
        /// <returns>
        ///   if a service has been registered with the specified interface, the reference is returned; otherwise, null is returned.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     if no service of the specified type is registered
        /// </exception>
        [DebuggerStepThrough]
        public static TInterface ResolveOrThrow<TInterface>()
            where TInterface : class
        {
            var obj = Resolve<TInterface>();
            if (obj != null)
                return obj;

            throw new ArgumentException("No service of type '" + typeof(TInterface).FullName + "' is registered");
        }

        /// <summary>
        /// Executes an action on a service if its registered.
        /// </summary>
        /// <typeparam name="T">The resolving type (interface).</typeparam>
        /// <param name="action">The action.</param>
        /// <returns>
        ///   <b>true</b> if the action was executed; otherwise, <b>false</b>
        /// </returns>
        [DebuggerStepThrough]
        public static bool ExecuteIfRegistered<T>(Action<T> action)
            where T : class
        {
            var obj = Resolve<T>();
            if (obj != null)
            {
                action(obj);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Executes an action on a service if it's registered and has been initialized.
        /// </summary>
        /// <typeparam name="T">The resolving type (interface).</typeparam>
        /// <param name="action">The action.</param>
        /// <returns>
        ///   <b>true</b> if the action was executed; otherwise, <b>false</b>
        /// </returns>
        [DebuggerStepThrough]
        public static bool ExecuteIfReady<T>(Action<T> action)
            where T : class
        {
            var type = typeof(T);
            var id = GetTypeId(type);
            lock (internals)
            {
                if (services.ContainsKey(id))
                {
                    var obj = services[id];
                    if (obj == null)
                    {
                        return false;
                    }
                    else
                    {
                        action((T)services[id]);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Clears all instances (and calls <see cref="IDisposable.Dispose"/>).
        /// </summary>
        public static void ClearInstances()
        {
            lock (internals)
            {
                if (services.Count > 0)
                {
                    foreach (var item in services.Values)
                    {
                        var disposable = item as IDisposable;
                        if (disposable != null)
                            disposable.Dispose();
                    }
                }

                services.Clear();
                TraceEx.Info("ApplicationServices", "Cleared");
            }
        }

        /// <summary>
        /// Clears all instances and factories (and calls <see cref="IDisposable.Dispose"/>).
        /// </summary>
        public static void Clear()
        {
            lock (internals)
            {
                if (services.Count > 0)
                {
                    foreach (var item in services.Values)
                    {
                        var disposable = item as IDisposable;
                        if (disposable != null)
                            disposable.Dispose();
                    }
                }

                services.Clear();
                factories.Clear();
                TraceEx.Info("ApplicationServices", "Cleared");
            }
        }

#if SILVERLIGHT || NETFX_CORE
        private static void ThrowIfRegisteredOrInstanciated(Type type, string id)
#else
        private static void ThrowIfRegisteredOrInstanciated(Type type, Guid id)
#endif
        {
            if (services.ContainsKey(id))
                throw new ArgumentException("Service of type '" + type.Name + "' is already registered. Use DropInstance, IsReady or Unregister method.");

            if (factories.ContainsKey(id))
                throw new ArgumentException("Service of type '" + type.Name + "' is already registered. Use DropInstance, IsRegistered or Unregister method.");
        }

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

        #region Internals

        /// <summary>
        /// Contract to create new service instances.
        /// </summary>
        interface IFactory
        {
            /// <summary>
            /// Creates an instance.
            /// </summary>
            /// <returns></returns>
            object Create();
        }

        /// <summary>
        /// Instantiates a service using <see cref="System.Activator"/> and the default constructor.
        /// </summary>
        class LazyEmptyService : IFactory
        {
            private readonly Type type;

            public LazyEmptyService(Type type)
            {
                this.type = type;
            }

            object IFactory.Create()
            {
                return Activator.CreateInstance(this.type);
            }
        }

        /// <summary>
        /// Instantiates a service using a delegate.
        /// </summary>
        class LazyFactoryService : IFactory
        {
            private readonly Func<object> factory;

            public LazyFactoryService(Func<object> factory)
            {
                this.factory = factory;
            }

            object IFactory.Create()
            {
                return this.factory();
            }
        }

        /// <summary>
        /// Instantiates a service using a delegate.
        /// </summary>
        class FactoryFactoryService : IFactory
        {
            private readonly Func<object> factory;

            public FactoryFactoryService(Func<object> factory)
            {
                this.factory = factory;
            }

            object IFactory.Create()
            {
                return this.factory();
            }
        }

        sealed class Lock : IDisposable
        {
            private Action action;
            private bool isDisposed;

            public Lock(Action action)
            {
                this.action = action;
            }

            public void Dispose()
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool disposing)
            {
                if (!this.isDisposed)
                {
                    if (disposing)
                    {
                        if (this.action != null)
                            this.action();
                        this.action = null;
                    }

                    this.isDisposed = true;
                }
            }
        }

        #endregion
    }
}
