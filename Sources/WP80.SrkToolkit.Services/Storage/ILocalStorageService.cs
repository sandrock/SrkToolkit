
namespace SrkToolkit.Services.Storage
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Service to access the isolated application storage.
    /// </summary>
    public interface ILocalStorageService
    {
        /// <summary>
        /// Restores the service.
        /// </summary>
        /// <typeparam name="T">the service type</typeparam>
        /// <returns>
        ///   a service instance or null
        /// </returns>
        /// <exception cref="InvalidOperationException">error occured</exception>
        T RestoreService<T>() where T : class;

        /// <summary>
        /// Saves the service.
        /// </summary>
        /// <typeparam name="T">the service type</typeparam>
        /// <param name="service">The service.</param>
        /// <exception cref="InvalidOperationException">error occured</exception>
        void SaveService<T>(T service) where T : class;

        void SaveServiceFile<T>(string filename, Stream stream);

        Stream GetServiceFile<T>(string filename);

        bool ServiceFileExists<T>(string filename);

        void DeleteServiceFile<T1>(string filename);

        void SaveServiceObject<TService, TObject>(string filename, TObject obj) where TObject : class;

        TObject GetServiceObject<TService, TObject>(string filename) where TObject : class;

        bool ServiceObjectExists<TService, TObject>(string filename) where TObject : class;

        void DeleteServiceObject<TService, TObject>(string filename) where TObject : class;
    }
}
