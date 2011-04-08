using System;
using SrkToolkit.Mvvm.Tools;

namespace SrkToolkit.Mvvm {
    public class InteractionViewModelBase : ViewModelBase {

        #region Composition parts

        #endregion

        #region View properties

        /// <summary>
        /// This collection contains tasks that are being processed.
        /// Nice properties are Tasks.IsBusy and Tasks.IsProcessing.
        /// Access tasks from the view with 
        ///   - {Binding Tasks[AutoLogin].IsProcessing}
        ///   - {Binding Tasks[AutoLogin].Message}
        /// </summary>
        public BusyTaskCollection Tasks {
            get { return _tasks; }
        }
        private readonly BusyTaskCollection _tasks = new BusyTaskCollection();

        #endregion

        #region Properties

        #endregion

        #region .ctor

        public InteractionViewModelBase() : base() {

        }

        #endregion

        #region Busy logic

        /// <summary>
        /// Initialize a task.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isGlobal"></param>
        protected void CreateTask(string key, bool isGlobal) {
            var task = Tasks[key];
            if (task != null) {
                throw new ArgumentException("a task with this key already exists", "key");
            } else {
                Tasks.Add(new BusyTask {
                    IsGlobal = isGlobal,
                    Key = key
                });
            }
        }

        /// <summary>
        /// Update a task status.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isProcessing"></param>
        /// <param name="type"></param>
        protected void UpdateTask(string key, bool isProcessing = false, string message = null, BusyTaskType type = BusyTaskType.Default) {
            var task = Tasks[key];
            if (task != null) {
                Tasks.Update(key, message, isProcessing, type);
            } else {
                Tasks.Add(new BusyTask {
                    Key = key,
                    IsGlobal = false,
                    IsProcessing = isProcessing,
                    Message = message,
                });
            }
        }

        #endregion
        
    }
}
