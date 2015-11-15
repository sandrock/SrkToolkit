
namespace SrkToolkit.Mvvm
{
    using System;
    using SrkToolkit.Mvvm.Tools;

    /// <summary>
    /// Higher-level ViewModel base with tasks and MessageBox abstraction.
    /// </summary>
    public partial class InteractionViewModelBase : ViewModelBase
    {
        private readonly BusyTaskCollection _tasks = new BusyTaskCollection();
        private InteractionViewModelBase baseViewModel;
        private IMessageBoxService _mbox;

        /// <summary>
        /// Initializes a new instance of the <see cref="InteractionViewModelBase"/> class.
        /// Make sure you instantiate this in the UI thread so that the dispatcher can attach.
        /// </summary>
        public InteractionViewModelBase()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InteractionViewModelBase"/> class.
        /// Make sure you instantiate this in the UI thread so that the dispatcher can attach.
        /// </summary>
        public InteractionViewModelBase(InteractionViewModelBase interactionViewModelBase)
            : base()
        {
            this.baseViewModel = interactionViewModelBase;
        }

        /// <summary>
        /// This collection contains tasks that are being processed.
        /// Nice properties are Tasks.IsBusy and Tasks.IsProcessing.
        /// Access tasks from the view with 
        ///   - {Binding Tasks[AutoLogin].IsProcessing}
        ///   - {Binding Tasks[AutoLogin].Message}
        /// </summary>
        public BusyTaskCollection Tasks
        {
            get { return _tasks; }
        }

        /// <summary>
        /// MessageBox abstraction.
        /// You can replace this for unit-testing.
        /// </summary>
        protected IMessageBoxService Mbox
        {
            get { return _mbox ?? (_mbox = new MessageBoxService()); }
            set { _mbox = value; }
        }

        /// <summary>
        /// Initialize a task.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isGlobal"></param>
        protected BusyTask CreateTask(string key, bool isGlobal)
        {
            var task = Tasks[key];
            if (task != null)
            {
                throw new ArgumentException("a task with this key already exists", "key");
            }
            else
            {
                task = new BusyTask
                {
                    IsGlobal = isGlobal,
                    Key = key,
                };
                this.Tasks.Add(task);
                return task;
            }
        }

        /// <summary>
        /// Initialize a task.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isGlobal"></param>
        /// <param name="name">the display name</param>
        protected BusyTask CreateTask(string key, bool isGlobal, string name)
        {
            var task = Tasks[key];
            if (task != null)
            {
                throw new ArgumentException("a task with this key already exists", "key");
            }
            else
            {
                task = new BusyTask
                {
                    IsGlobal = isGlobal,
                    Key = key,
                    Name = name,
                };
                this.Tasks.Add(task);
                return task;
            }
        }

        /// <summary>
        /// Update a task status.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isProcessing"></param>
        /// <param name="type"></param>
        /// <param name="message"></param>
        protected void UpdateTask(string key, bool isProcessing = false, string message = null, BusyTaskType type = BusyTaskType.Default)
        {
            var task = this.Tasks[key];
            if (task != null)
            {
                this.Tasks.Update(key, message, isProcessing, type);
            }
            else
            {
                task = new BusyTask
                {
                    Key = key,
                    IsGlobal = false,
                    IsProcessing = isProcessing,
                    Message = message,
                };
                this.Tasks.Add(task);
            }
        }

        /// <summary>
        /// Update a task status.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isProcessing"></param>
        /// <param name="type"></param>
        /// <param name="message"></param>
        protected void UpdateTask(string key, string message = null, bool isProcessing = false, BusyTaskType type = BusyTaskType.Default)
        {
            var task = Tasks[key];
            if (task != null)
            {
                Tasks.Update(key, message, isProcessing, type);
            }
            else
            {
                task = new BusyTask
                {
                    Key = key,
                    IsGlobal = false,
                    IsProcessing = isProcessing,
                    Message = message,
                };
                Tasks.Add(task);
            }
        }

        /// <summary>
        /// Update a task status with an exception message.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The message.</param>
        /// <param name="isProcessing">if set to <c>true</c> [is processing].</param>
        /// <param name="type">The type.</param>
        protected void UpdateTask(string key, Exception exception, string message = null, bool isProcessing = false, BusyTaskType type = BusyTaskType.Error)
        {
            if (exception != null)
            {
#if DEBUG
                UpdateTask(key, isProcessing, message ?? exception.Message, type);
#else
                UpdateTask(key, isProcessing, message ?? "An error occured. ", type);
#endif
            }
            else
            {
                UpdateTask(key, isProcessing, message, BusyTaskType.Default);
            }
        }
    }
}
