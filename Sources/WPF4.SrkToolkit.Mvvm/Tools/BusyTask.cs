using System;
using SrkToolkit.Mvvm;

namespace SrkToolkit.Mvvm.Tools {

    /// <summary>
    /// Represent a background task in a viewmodel.
    /// </summary>
    public class BusyTask : ViewModelBase {
        private bool _isProcessing;
        private string _message;
        private BusyTaskType _type;

        public BusyTask()
        {
        }

        public BusyTask(string key)
        {
            this.Key = key;
        }

        /// <summary>
        /// Message to display like "Downloading data... ".
        /// </summary>
        public string Message {
            get { return this._message; }
            set { this.SetValue(ref this._message, value, "Message"); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the task currently processing.
        /// Will set <see cref="IsQueued"/> to false.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the task currently processing; otherwise, <c>false</c>.
        /// </value>
        public bool IsProcessing {
            get { return this._isProcessing; }
            set {
                if (this.SetValue(ref this._isProcessing, value, "IsProcessing")) {
                    this.RaisePropertyChanged("IsNotProcessing");
                }
                this.IsQueued = false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the task not currently processing.
        /// Will set <see cref="IsQueued"/> to false.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the task not currently processing; otherwise, <c>false</c>.
        /// </value>
        public bool IsNotProcessing {
            get { return !this._isProcessing; }
        }

        /// <summary>
        /// Gets or sets a informative value indicating whether this task is queued for execution.
        /// Not used by the BusyTask framework.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this task is queued for execution; otherwise, <c>false</c>.
        /// </value>
        public bool IsQueued { get; set; }

        /// <summary>
        /// Gets or sets the queued action.
        /// Not used by the BusyTask framework.
        /// </summary>
        /// <value>
        /// The queued action.
        /// </value>
        public Action QueuedAction { get; set; }

        /// <summary>
        /// Optionnal task type.
        /// Permit to display a red/green message.
        /// </summary>
        public BusyTaskType Type {
            get { return this._type; }
            set {
                if (this._type != value) {
                    this._type = value;
                    this.RaisePropertyChanged("Type");
                    this.RaisePropertyChanged("IsError");
                    this.RaisePropertyChanged("IsConfirmation");
                }
            }
        }

        /// <summary>
        /// Simple accessor linked to <see cref="Type"/>.
        /// </summary>
        public bool IsError {
            get { return this._type == BusyTaskType.Error; }
        }

        /// <summary>
        /// Simple accessor linked to <see cref="Type"/>.
        /// </summary>
        public bool IsConfirmation {
            get { return this._type == BusyTaskType.Confirmation; }
        }
        
        /// <summary>
        /// Unique key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// If both <see cref="IsGlobal"/> and <see cref="IsProcessing"/>,
        /// permits to freeze the UI with <see cref="BusyTaskCollection.IsBusy"/>.
        /// </summary>
        public bool IsGlobal { get; set; }
    }
}
