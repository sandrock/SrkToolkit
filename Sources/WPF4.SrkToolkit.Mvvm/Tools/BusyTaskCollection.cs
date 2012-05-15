using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System;

namespace SrkToolkit.Mvvm.Tools {

    /// <summary>
    /// Represent background tasks in a viewmodel.
    /// </summary>
    public class BusyTaskCollection : ObservableCollection<BusyTask> {

        #region Private fields

        private bool _isBusy;
        private bool _isProcessing;

        #endregion

        #region Public properties

        /// <summary>
        /// Permits to disable the whole UI for a blocking task.
        /// </summary>
        public bool IsBusy {
            get { return this._isBusy; }
            set {
                if (this._isBusy != value) {
                    this._isBusy = value;
                    this.OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
                    this.OnPropertyChanged(new PropertyChangedEventArgs("IsNotBusy"));
                }
            }
        }

        /// <summary>
        /// Permits to disable the whole UI for a blocking task.
        /// </summary>
        public bool IsNotBusy { get { return !_isBusy; } }

        /// <summary>
        /// Permits to show the user a background task is performing.
        /// </summary>
        public bool IsProcessing {
            get { return this._isProcessing; }
            set {
                if (this._isProcessing != value) {
                    this._isProcessing = value;
                    this.OnPropertyChanged(new PropertyChangedEventArgs("IsProcessing"));
                    this.OnPropertyChanged(new PropertyChangedEventArgs("IsNotProcessing"));
                }
            }
        }

        /// <summary>
        /// Permits to show the user a background task is performing.
        /// </summary>
        public bool IsNotProcessing { get { return !this._isProcessing; } }

        /// <summary>
        /// Get a task by key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public BusyTask this[Enum key] {
            get {
                return this[key.ToString()];
            }
        }

        /// <summary>
        /// Get a task by key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public BusyTask this[string key] {
            get {
                return this.FirstOrDefault(i => i.Key == key);
            }
        }

        /// <summary>
        /// Message aggreagated form all tasks currently busy.
        /// </summary>
        public string AggregateMessage {
            get {
                var message = string.Empty;
                string sep = string.Empty;
                foreach (var item in this) {
                    if (!string.IsNullOrEmpty(item.Message)) {
                        message += sep + item.Message;
                        sep = Environment.NewLine;
                    }
                }
                return string.IsNullOrEmpty(message) ? null : message.Trim();
            }
        }

        #endregion

        #region .ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="BusyTaskCollection"/> class.
        /// </summary>
        internal BusyTaskCollection() : base() {
        }

        #endregion

        /// <summary>
        /// Occurs when a task state changed.
        /// </summary>
        public event EventHandler StateChangedEvent;

        #region Public methods

        /// <summary>
        /// Create a task.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isGlobal">pass true to freeze the UI when processing</param>
        public void Add(string key, bool isGlobal) {
            this.Add(new BusyTask {
                Key = key,
                IsGlobal = isGlobal
            });
        }

        /// <summary>
        /// Create a task.
        /// </summary>
        /// <param name="key">The task key.</param>
        /// <param name="isGlobal">pass true to freeze the UI when processing</param>
        public void Add(Enum key, bool isGlobal) {
            this.Add(key.ToString(), isGlobal);
        }

        /// <summary>
        /// Updates the specified key.
        /// </summary>
        /// <param name="key">The task key.</param>
        /// <param name="message">The message.</param>
        /// <param name="isProcessing">if set to <c>true</c> [is processing].</param>
        /// <param name="type">The type.</param>
        public void Update(string key, string message, bool isProcessing, BusyTaskType type) {
            var task = this[key];
            task.Message = message;
            task.IsProcessing = isProcessing;
            task.Type = type;
            this.OnPropertyChanged(new PropertyChangedEventArgs("AggregateMessage"));
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Inserts the item.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        protected override void InsertItem(int index, BusyTask item) {
            base.InsertItem(index, item);

            item.PropertyChanged += OnItemPropertyChanged;

            this.ComputeStatus();
        }

        /// <summary>
        /// Removes the item.
        /// </summary>
        /// <param name="index">The index.</param>
        protected override void RemoveItem(int index) {
            if (this[index] != null)
                this[index].PropertyChanged -= OnItemPropertyChanged;

            base.RemoveItem(index);

            this.ComputeStatus();
        }

        /// <summary>
        /// Clears the items.
        /// </summary>
        protected override void ClearItems() {
            foreach (var item in this) {
                item.PropertyChanged -= OnItemPropertyChanged;
            }

            base.ClearItems();

            this.ComputeStatus();
        }

        #endregion

        #region Internal stuff

        private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e) {
            var item = sender as BusyTask;

            if (item != null && e.PropertyName == "IsProcessing") {
                this.ComputeStatus();
            }
        }

        private void ComputeStatus() {
            bool isbusy = false;
            bool isprocessing = false;

            foreach (var item in this) {
                isprocessing |= item.IsProcessing;
                isbusy |= item.IsGlobal && item.IsProcessing;
            }

            this.IsBusy = isbusy;
            this.IsProcessing = isprocessing;
            this.OnPropertyChanged(new PropertyChangedEventArgs("AggregateMessage"));
        
            var handler = this.StateChangedEvent;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        #endregion
    }
}
