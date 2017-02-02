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

namespace SrkToolkit.Mvvm.Tools
{
    using System;
    using SrkToolkit.Mvvm;

    /// <summary>
    /// Represent a background task in a viewmodel.
    /// </summary>
    public class BusyTask : ViewModelBase
    {
        private bool _isProcessing;
        private string _message;
        private BusyTaskType _type;

        /// <summary>
        /// Contains a description.
        /// Use the property <see cref="Description"/> instead.
        /// </summary>
        private string description;

        /// <summary>
        /// Contains the display name.
        /// Use the property <see cref="Name"/> instead.
        /// </summary>
        private string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="BusyTask"/> class.
        /// </summary>
        public BusyTask()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusyTask"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public BusyTask(string key)
        {
            this.Key = key;
        }

        /// <summary>
        /// Message to display like "Downloading data... ".
        /// </summary>
        public string Message
        {
            get { return this._message; }
            set { this.SetValue(ref this._message, value, "Message"); }
        }

        /// <summary>
        /// Gets or sets a description.
        /// Not used by the BusyTask framework.
        /// </summary>
        public string Description
        {
            get { return this.description; }
            set { this.SetValue(ref this.description, value, "Description"); }
        }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.SetValue(ref this.name, value, "Name"); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the task currently processing.
        /// Will set <see cref="IsQueued"/> to false.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the task currently processing; otherwise, <c>false</c>.
        /// </value>
        public bool IsProcessing
        {
            get { return this._isProcessing; }
            set
            {
                if (this.SetValue(ref this._isProcessing, value, "IsProcessing"))
                {
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
        public bool IsNotProcessing
        {
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
        public BusyTaskType Type
        {
            get { return this._type; }
            set
            {
                if (this._type != value)
                {
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
        public bool IsError
        {
            get { return this._type == BusyTaskType.Error; }
        }

        /// <summary>
        /// Simple accessor linked to <see cref="Type"/>.
        /// </summary>
        public bool IsConfirmation
        {
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
