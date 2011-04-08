using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System;

namespace SrkToolkit.Mvvm.Tools {

    /// <summary>
    /// Represent background tasks in a viewmodel.
    /// </summary>
    public class BusyTaskCollection : ObservableCollection<BusyTask> {

        #region Public properties

        /// <summary>
        /// Permits to disable the whole UI for a blocking task.
        /// </summary>
        public bool IsBusy {
            get { return _isBusy; }
            set {
                if (_isBusy != value) {
                    _isBusy = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
                    OnPropertyChanged(new PropertyChangedEventArgs("IsNotBusy"));
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
            get { return _isProcessing; }
            set {
                if (_isProcessing != value) {
                    _isProcessing = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsProcessing"));
                    OnPropertyChanged(new PropertyChangedEventArgs("IsNotProcessing"));
                }
            }
        }

        /// <summary>
        /// Permits to show the user a background task is performing.
        /// </summary>
        public bool IsNotProcessing { get { return !_isProcessing; } }

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
                return message;
            }
        }

        #endregion

        #region Private fields

        private bool _isBusy;
        private bool _isProcessing;

        #endregion

        #region .ctor

        public BusyTaskCollection() : base() {

        }

        #endregion

        #region Public methods

        /// <summary>
        /// Create a task.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isGlobal">pass true to freeze the UI when processing</param>
        public void Add(string key, bool isGlobal) {
            Add(new BusyTask {
                Key = key,
                IsGlobal = isGlobal
            });
        }

        /// <summary>
        /// Create a task.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isGlobal">pass true to freeze the UI when processing</param>
        public void Add(Enum key, bool isGlobal) {
            Add(key.ToString(), isGlobal);
        }

        public void Update(string key, string message, bool isProcessing, BusyTaskType type) {
            var task = this[key];
            task.Message = message;
            task.IsProcessing = isProcessing;
            task.Type = type;
            OnPropertyChanged(new PropertyChangedEventArgs("AggregateMessage"));
        }

        #endregion

        #region Overrides

        protected override void InsertItem(int index, BusyTask item) {
            base.InsertItem(index, item);

            item.PropertyChanged += item_PropertyChanged;

            ComputeStatus();
        }

        protected override void RemoveItem(int index) {
            if (this[index] != null)
                this[index].PropertyChanged -= item_PropertyChanged;

            base.RemoveItem(index);

            ComputeStatus();
        }

        protected override void ClearItems() {
            foreach (var item in this) {
                item.PropertyChanged -= item_PropertyChanged;
            }

            base.ClearItems();

            ComputeStatus();
        }

        #endregion

        #region Internal stuff

        private void item_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            var item = sender as BusyTask;

            if (item != null && e.PropertyName == "IsProcessing") {
                ComputeStatus();
            }
        }

        private void ComputeStatus() {
            bool isbusy = false;
            bool isprocessing = false;

            foreach (var item in this) {
                isprocessing |= item.IsProcessing;
                isbusy |= item.IsGlobal && item.IsProcessing;
            }

            IsBusy = isbusy;
            IsProcessing = isprocessing;
            OnPropertyChanged(new PropertyChangedEventArgs("AggregateMessage"));
        }

        #endregion

    }
}
