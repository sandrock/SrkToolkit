using System;
using SrkToolkit.Mvvm;

namespace SrkToolkit.Mvvm.Tools {

    /// <summary>
    /// Represent a background task in a viewmodel.
    /// </summary>
    public class BusyTask : ViewModelBase {

        /// <summary>
        /// Message to display like "Downloading data... ".
        /// </summary>
        public string Message {
            get { return _message; }
            set { SetValue(ref _message, value, "Message"); }
        }
        private string _message;

        /// <summary>
        /// Is the task currently processing.
        /// Will set <see cref="IsQueued"/> to false.
        /// </summary>
        public bool IsProcessing {
            get { return _isProcessing; }
            set {
                if (SetValue(ref _isProcessing, value, "IsProcessing")) {
                    RaisePropertyChanged("IsNotProcessing");
                }
                IsQueued = false;
            }
        }
        private bool _isProcessing;
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
        private BusyTaskType _type;

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
