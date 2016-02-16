
namespace SrkToolkit.Services.Storage
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Runtime.Serialization;

    /// <summary>
    /// Service to access the isolated application storage.
    /// </summary>
    public class LocalStorageService : ILocalStorageService
    {
        /// <summary>
        /// The data (cache and stuff) folder path.
        /// </summary>
        private const string DataFolder = "Data";

        /// <summary>
        /// The tombstone folder path for pages state persistance.
        /// </summary>
        private const string TombstoneFolder = "Tombstone";

        /// <summary>
        /// The services folder path for services state persistance.
        /// </summary>
        private const string ServicesFolder = "Services";

        /// <summary>
        /// The services' files folder path for services state persistance.
        /// </summary>
        private const string ServicesFilesFolder = "Files";

        /// <summary>
        /// Use this lock for all IO operations.
        /// Will not allow parallel operations but will prevent concurency on files.
        /// </summary>
        private static readonly object fileSystemLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalStorageService"/> class.
        /// </summary>
        public LocalStorageService()
        {
        }

        /// <summary>
        /// Saves the service.
        /// </summary>
        /// <typeparam name="T">the service type</typeparam>
        /// <param name="service">The service.</param>
        /// <exception cref="InvalidOperationException">error occured</exception>
        public void SaveService<T>(T service)
            where T : class
        {
            lock (fileSystemLock)
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    var serviceType = typeof(T);
                    var filePath = GetServiceInstanceFilePath(store, serviceType.FullName);

                    // if the file exists, delete it
                    ////if (store.FileExists(filePath))
                    ////    store.DeleteFile(filePath);

                    //var ser = new XmlSerializer(serviceType);
                    var ser = new DataContractSerializer(serviceType);
                    IsolatedStorageFileStream fileStream = null;
                    try
                    {
                        fileStream = store.OpenFile(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                        //ser.Serialize(fileStream, service);
                        ser.WriteObject(fileStream, service);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException("Could not serialize service " + serviceType.Name, ex);
                    }
                    finally
                    {
                        if (fileStream != null)
                            fileStream.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Restores the service.
        /// </summary>
        /// <typeparam name="T">the service type</typeparam>
        /// <returns>
        /// a service instance or null
        /// </returns>
        /// <exception cref="InvalidOperationException">error occured</exception>
        public T RestoreService<T>()
            where T : class
        {
            lock (fileSystemLock)
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    T service = default(T);
                    var serviceType = typeof(T);
                    var filePath = GetServiceInstanceFilePath(store, serviceType.FullName);

                    // if the file does not exist, just return null
                    if (!store.FileExists(filePath))
                        return service;

                    //var ser = new XmlSerializer(serviceType);
                    var ser = new DataContractSerializer(serviceType);
                    IsolatedStorageFileStream fileStream = null;
                    try
                    {
                        fileStream = store.OpenFile(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
                        //service = ser.Deserialize(fileStream) as T;
                        service = ser.ReadObject(fileStream) as T;
                    }
                    catch (Exception ex)
                    {
                        if (fileStream != null)
                            fileStream.Close();
                        try
                        {
                            store.DeleteFile(filePath); // the file may be corrupted, better to delete it
                        }
                        catch
                        {
                        }

                        throw new InvalidOperationException("Could not deserialize service " + serviceType.Name, ex);
                    }
                    finally
                    {
                        if (fileStream != null)
                            fileStream.Close();
                    }

                    return service;
                }
            }
        }

        public void SaveServiceFile<T>(string filename, Stream stream)
        {
            if (filename == null)
                throw new ArgumentNullException("filename");
            if (stream == null)
                throw new ArgumentNullException("stream");

            lock (fileSystemLock)
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    var serviceType = typeof(T);
                    var filePath = GetServiceFileFilePath(store, serviceType.FullName, filename);

                    // if the file exists, delete it
                    ////if (store.FileExists(filePath))
                    ////    store.DeleteFile(filePath);

                    //var ser = new XmlSerializer(serviceType);
                    IsolatedStorageFileStream fileStream = null;
                    try
                    {
                        fileStream = store.OpenFile(filePath, FileMode.Create, FileAccess.Write, FileShare.None);

                        int bytesRead = 0;
                        byte[] buffer = new byte[4096];
                        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fileStream.Write(buffer, 0, bytesRead);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException("Could not save service file '" + serviceType.Name + "/" + filename + "'", ex);
                    }
                    finally
                    {
                        if (fileStream != null)
                            fileStream.Close();
                    }
                }
            }
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Liability of caller")]
        public Stream GetServiceFile<T>(string filename)
        {
            if (filename == null)
                throw new ArgumentNullException("filename");

            lock (fileSystemLock)
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    var serviceType = typeof(T);
                    var filePath = GetServiceFileFilePath(store, serviceType.FullName, filename);

                    // if the file does not exist, just return null
                    if (!store.FileExists(filePath))
                        return null;

                    //var ser = new XmlSerializer(serviceType);
                    var stream = new MemoryStream();
                    IsolatedStorageFileStream fileStream = null;
                    try
                    {
                        fileStream = store.OpenFile(filePath, FileMode.Open, FileAccess.Read, FileShare.None);

                        int bytesRead = 0;
                        byte[] buffer = new byte[4096];
                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            stream.Write(buffer, 0, bytesRead);
                        }

                        return stream;
                    }
                    catch (Exception ex)
                    {
                        stream.Close();
                        throw new InvalidOperationException("Could read serialize service file '" + serviceType.Name + "/" + filename + "'", ex);
                    }
                    finally
                    {
                        if (fileStream != null)
                            fileStream.Close();
                    }
                }
            }
        }

        public bool ServiceFileExists<T>(string filename)
        {
            if (filename == null)
                throw new ArgumentNullException("filename");

            lock (fileSystemLock)
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    var serviceType = typeof(T);
                    var filePath = GetServiceFileFilePath(store, serviceType.FullName, filename);

                    return store.FileExists(filePath);
                }
            }
        }

        public void DeleteServiceFile<T>(string filename)
        {
            if (filename == null)
                throw new ArgumentNullException("filename");

            lock (fileSystemLock)
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    var serviceType = typeof(T);
                    var filePath = GetServiceFileFilePath(store, serviceType.FullName, filename);

                    if (store.FileExists(filePath))
                        store.DeleteFile(filePath);
                }
            }
        }

        public void SaveServiceObject<TService, TObject>(string filename, TObject obj)
            where TObject : class
        {
            if (filename == null)
                throw new ArgumentNullException("filename");
            if (obj == null)
                throw new ArgumentNullException("obj");

            lock (fileSystemLock)
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    var serviceType = typeof(TObject);
                    var filePath = GetServiceFileFilePath(store, serviceType.FullName, filename);

                    // if the file exists, delete it
                    ////if (store.FileExists(filePath))
                    ////    store.DeleteFile(filePath);

                    //var ser = new XmlSerializer(serviceType);
                    var ser = new DataContractSerializer(serviceType);
                    IsolatedStorageFileStream fileStream = null;
                    try
                    {
                        fileStream = store.OpenFile(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                        //ser.Serialize(fileStream, service);
                        ser.WriteObject(fileStream, obj);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException("Could not save service object '" + serviceType.Name + "/" + filename + "'", ex);
                    }
                    finally
                    {
                        if (fileStream != null)
                            fileStream.Close();
                    }
                }
            }
        }

        public TObject GetServiceObject<TService, TObject>(string filename)
            where TObject : class
        {
            if (filename == null)
                throw new ArgumentNullException("filename");

            lock (fileSystemLock)
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    TObject obj = default(TObject);
                    var objType = typeof(TObject);
                    var filePath = GetServiceFileFilePath(store, objType.FullName, filename);

                    // if the file does not exist, just return null
                    if (!store.FileExists(filePath))
                        return obj;

                    //var ser = new XmlSerializer(serviceType);
                    var ser = new DataContractSerializer(objType);
                    IsolatedStorageFileStream fileStream = null;
                    try
                    {
                        fileStream = store.OpenFile(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
                        //service = ser.Deserialize(fileStream) as T;
                        obj = ser.ReadObject(fileStream) as TObject;
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            store.DeleteFile(filePath);
                        }
                        catch
                        {
                        }

                        throw new InvalidOperationException("Could not deserialize service object " + objType.Name, ex);
                    }
                    finally
                    {
                        if (fileStream != null)
                            fileStream.Close();
                    }

                    return obj;
                }
            }
        }

        public bool ServiceObjectExists<TService, TObject>(string filename) where TObject : class
        {
            if (filename == null)
                throw new ArgumentNullException("filename");

            lock (fileSystemLock)
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    var objType = typeof(TObject);
                    var filePath = GetServiceFileFilePath(store, objType.FullName, filename);

                    return store.FileExists(filePath);
                }
            }
        }

        public void DeleteServiceObject<TService, TObject>(string filename) where TObject : class
        {
            if (filename == null)
                throw new ArgumentNullException("filename");

            lock (fileSystemLock)
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    var objType = typeof(TObject);
                    var filePath = GetServiceFileFilePath(store, objType.FullName, filename);

                    if (store.FileExists(filePath))
                        store.DeleteFile(filePath);
                }
            }
        }

        /// <summary>
        /// Gets a service instance file path.
        /// Creates the folders if required.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="serviceTypeFullName">Full name of the service type.</param>
        /// <returns></returns>
        private static string GetServiceInstanceFilePath(IsolatedStorageFile store, string serviceTypeFullName)
        {
            // create folder if it does not exist
            if (!store.DirectoryExists(ServicesFolder))
                store.CreateDirectory(ServicesFolder);

            // return combined path
            return Path.Combine(ServicesFolder, serviceTypeFullName + ".instance");
        }

        /// <summary>
        /// Gets a service's file file path.
        /// Creates the folders if required.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="serviceTypeFullName">Full name of the service type.</param>
        /// <param name="filename">The service's file name.</param>
        /// <returns></returns>
        private static string GetServiceFileFilePath(IsolatedStorageFile store, string serviceTypeFullName, string filename)
        {
            // /Services
            if (!store.DirectoryExists(ServicesFolder))
                store.CreateDirectory(ServicesFolder);

            // /Services/Service.Full.Name
            var serviceDirectoryPath = Path.Combine(ServicesFolder, serviceTypeFullName);
            if (!store.DirectoryExists(serviceDirectoryPath))
                store.CreateDirectory(serviceDirectoryPath);

            // /Services/Service.Full.Name/Files
            var serviceFilesDirectoryPath = Path.Combine(serviceDirectoryPath, ServicesFilesFolder);
            if (!store.DirectoryExists(serviceFilesDirectoryPath))
                store.CreateDirectory(serviceFilesDirectoryPath);

            // /Services/Service.Full.Name/Files/File.Name
            return Path.Combine(serviceFilesDirectoryPath, filename);
        }
    }
}
