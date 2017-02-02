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

namespace SrkToolkit.IO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
#if NET40
    using SrkToolkit.Threading.Tasks;
#endif

    /// <summary>
    /// Multi-threaded implementation of a recursive file delete algorithm.
    /// </summary>
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

        /// <summary>
        /// A predicate for deletion.
        /// </summary>
        public Func<FileInfo, bool> DeleteCondition { get; set; }

        public bool CollectExceptions { get; set; }

        /// <summary>
        /// Runs immediately in a blocking fashion.
        /// </summary>
        public void Run()
        {
            this.Execute();
        }

        /// <summary>
        /// Runs in a asynchronous fashion.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="progressCallback">The progress callback.</param>
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
#if NET45
                tasks.Add(Task.Run(() =>
#else
                tasks.Add(TaskEx.Run(() =>
#endif
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
