using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SrkToolkit.Services
{
    /// <summary>
    /// Permits to store references to application-wide services.
    /// </summary>
    public static class ApplicationServices
    {
        private static readonly Dictionary<Guid, object> services = new Dictionary<Guid, object>();

        private static readonly object internals = new object();

        /// <summary>
        /// Determines whether a service is registered.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <returns>
        ///   <c>true</c> if a service has been registered with the specified interface; otherwise, <c>false</c>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsRegistered<TInterface>()
            where TInterface : class
        {
            var type = typeof(TInterface);
            var id = type.GUID;
            lock (internals)
            {
                return services.ContainsKey(id); 
            }
        }

        /// <summary>
        /// Registers the a service with a specified interface type.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="service">The reference to the service instance.</param>
        public static void Register<TInterface, TImplementation>(TImplementation service)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            if (service == null)
                throw new ArgumentNullException("service");

            var type = typeof(TInterface);
            var id = type.GUID;
            lock (internals)
            {
                if (services.ContainsKey(id))
                    throw new ArgumentException("Service of type '" + type.Name + "' is already registered");

                services.Add(id, service);
                TraceEx.Info("ApplicationService", "Registered instance for " + type.Name);
            }
        }

        /// <summary>
        /// Registers the a service with a specified interface type.
        /// The service is automatically instanciated on the first call to it.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="service">The reference to the service instance.</param>
        public static void Register<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : class, TInterface, new()
        {
            var type = typeof(TInterface);
            var implType = typeof(TImplementation);
            var id = type.GUID;
            lock (internals)
            {
                if (services.ContainsKey(id))
                    throw new ArgumentException("Service of type '" + type.Name + "' is already registered");

                services.Add(id, new LazyEmptyService(implType));
                TraceEx.Info("ApplicationService", "Registered implementation for " + type.Name);
            }
        }

        /// <summary>
        /// Registers the a service with a specified interface type.
        /// The service is automatically instanciated on the first call to it.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <param name="factory">The factory for the instantiation.</param>
        public static void Register<TInterface>(Func<TInterface> factory)
            where TInterface : class
        {
            var type = typeof(TInterface);
            var id = type.GUID;
            lock (internals)
            {
                if (services.ContainsKey(id))
                    throw new ArgumentException("Service of type '" + type.Name + "' is already registered");

                services.Add(id, new LazyFactoryService(factory));
                TraceEx.Info("ApplicationService", "Registered factory for " + type.Name);
            }
        }

        /// <summary>
        /// Unregisters a service with from specified interface type.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        public static void Unregister<TInterface>()
            where TInterface : class
        {
            var type = typeof(TInterface);
            var id = type.GUID;
            lock (internals)
            {
                if (services.ContainsKey(id))
                {
                    services.Remove(id);
                    TraceEx.Info("ApplicationService", "Removed instance for " + type.Name);
                }
            }
        }

        /// <summary>
        /// Unregisters a service with the specified reference.
        /// </summary>
        /// <param name="obj">The object to unregister.</param>
        /// <exception cref="ArgumentNullException">if the argument is null</exception>
        public static void Unregister(object obj)
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
                        TraceEx.Info("ApplicationService", "Removed instance for " + (item.Value != null ? item.Value.ToString() : item.Key.ToString()));
                    }
                }
            }
        }

        /// <summary>
        /// Resolves a service instance with a specified interface type.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <returns>
        ///   if a service has been registered with the specified interface, the reference is returned; otherwise, null is returned.
        /// </returns>
        [DebuggerStepThrough]
        public static TInterface Resolve<TInterface>()
            where TInterface : class
        {
            var type = typeof(TInterface);
            var id = type.GUID;
            lock (internals)
            {
                if (services.ContainsKey(id))
                {
                    var obj = services[id];
                    if (obj == null)
                    {
                        return null;
                    }
                    else if (obj is IFactory)
                    {
                        var instance = ((IFactory)obj).Create();
                        services[id] = instance;
                        return (TInterface)instance;
                    }
                    else
                    {
                        return (TInterface)services[id];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Resolves a service instance with a specified interface type.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
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
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
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
                TraceEx.Info("ApplicationService", "Cleared");
            }
        }

        #region Internals

        interface IFactory
        {
            object Create();
        }

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

        #endregion
    }
}
