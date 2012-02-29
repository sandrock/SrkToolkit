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

        /// <summary>
        /// Message to display like "Downloading data... ".
        /// </summary>
        public string Message {
            get { return _message; }
            set { SetValue(ref _message, value, "Message"); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the task currently processing.
        /// Will set <see cref="IsQueued"/> to false.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the task currently processing; otherwise, <c>false</c>.
        /// </value>
        public bool IsProcessing {
            get { return _isProcessing; }
            set {
                if (SetValue(ref _isProcessing, value, "IsProcessing")) {
                    RaisePropertyChanged("IsNotProcessing");
                }
                IsQueued = false;
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
            get { return !_isProcessing; }
        }

        public bool IsQueued { get; set; }

        /// <summary>
        /// Optionnal task type.
        /// Permit to display a red/green message.
        /// </summary>
        public BusyTaskType Type {
            get { return _type; }
            set {
                if (_type != value) {
                    _type = value;
                    RaisePropertyChanged("Type");
                    RaisePropertyChanged("IsError");
                    RaisePropertyChanged("IsConfirmation");
                }
            }
        }

        /// <summary>
        /// Simple accessor linked to <see cref="Type"/>.
        /// </summary>
        public bool IsError {
            get { return _type == BusyTaskType.Error; }
        }

        /// <summary>
        /// Simple accessor linked to <see cref="Type"/>.
        /// </summary>
        public bool IsConfirmation {
            get { return _type == BusyTaskType.Confirmation; }
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
