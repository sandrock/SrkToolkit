
namespace SrkToolkit.IO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using SrkToolkit.Threading.Tasks;

    public class RecursiveDelete
    {
        private string path;
        private bool childrenOnly;
        private List<Task> tasks = new List<Task>();

        private readonly object runningLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="RecursiveDelete"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="childrenOnly">if set to <c>true</c> to delete children of the path only (not the directory containing them).</param>
        public RecursiveDelete(string path, bool childrenOnly)
        {
            this.path = Path.GetFullPath(path);
            this.childrenOnly = childrenOnly;
        }

        public Func<FileInfo, bool> DeleteCondition { get; set; }

        public bool CollectExceptions { get; set; }

        public void Run()
        {
            this.Execute();
        }

        public void RunAsync(Action callback, Action<int, int> progressCallback)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                this.Execute();
                
                if (callback != null)
                    callback();
            });
        }

        private void Execute()
        {
            lock (this.runningLock)
            {
                this.Recurse(this.path, deletePath: !this.childrenOnly);
            }
        }

        private void Recurse(string path, bool deletePath)
        {
            if (File.Exists(path) && deletePath)
            {
                File.Delete(path);
            }

            if (Directory.Exists(path))
            {
                var deleteMe = deletePath;
                var filesPath = path;
                var files = Directory.GetFiles(path);
                var folders = Directory.GetDirectories(path);
                tasks.Add(TaskEx.Run(() =>
                    {
                        foreach (var file in files)
                        {
                            var filePath = Path.Combine(filesPath, file);
                            File.Delete(filePath);
                        }
                    }).ContinueWith((a1) =>
                    {
                        foreach (var folder in folders)
                        {
                            var folderPath = Path.Combine(filesPath, folder);
                            this.Recurse(folderPath, true);
                        }
                    }).ContinueWith((a2) =>
                    {
                        if (deleteMe)
                        {
                            Directory.Delete(filesPath);
                        }
                    }));
            }
        }
    }
}
