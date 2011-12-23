using System;
using SrkToolkit.Mvvm.Tools;

namespace SrkToolkit.Mvvm {


    public partial class InteractionViewModelBase : ViewModelBase {

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
        /// <param name="message"></param>
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

        /// <summary>
        /// Update a task status.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isProcessing"></param>
        /// <param name="type"></param>
        /// <param name="message"></param>
        protected void UpdateTask(string key, string message = null, bool isProcessing = false, BusyTaskType type = BusyTaskType.Default) {
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

        /// <summary>
        /// Update a task status.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isProcessing"></param>
        /// <param name="type"></param>
        protected void UpdateTask(string key, Exception exception, string message = null, bool isProcessing = false, BusyTaskType type = BusyTaskType.Error) {
            if (exception != null) {
#if DEBUG
                UpdateTask(key, isProcessing, message ?? exception.Message, type);
#else
                UpdateTask(key, isProcessing, message ?? "An error occured. ", type);
#endif
            } else {
                UpdateTask(key, isProcessing, message, BusyTaskType.Default);
            }
        }

        #endregion

        #region MessageBoxService

        /// <summary>
        /// MessageBox abstraction.
        /// You can replace this for unit-testing.
        /// </summary>
        protected MessageBoxService Mbox {
            get { return _mbox ?? (_mbox = new MessageBoxService()); }
            set { _mbox = value; }
        }
        private MessageBoxService _mbox;

        #endregion
        
    }
}
