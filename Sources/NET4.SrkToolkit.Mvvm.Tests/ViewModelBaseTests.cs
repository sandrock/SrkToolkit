using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SrkToolkit.Mvvm;

namespace NET4.SrkToolkit.Mvvm.Tests
{
    [TestClass]
    public class ViewModelBaseTests
    {
        [TestClass]
        public class PropertyChangedEvent
        {
            [TestMethod, TestCategory(Category.Unit)]
            public void ChangingPropertyValueViaSetValueNotifiesChange()
            {
                // prepare
                var target = new ViewModelA();
                bool notified = false;
                target.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == "ViaSetValue")
                        notified = true;
                };

                // execute
                target.ViaSetValue = true;

                // verify
                Assert.AreEqual(true, target.ViaSetValue);
                Assert.IsTrue(notified);
            }

            [TestMethod, TestCategory(Category.Unit)]
            public void ChangingPropertyValueViaRaisePropertyChangedNotifiesChange()
            {
                // prepare
                var target = new ViewModelA();
                bool notified = false;
                target.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == "ViaRaisePropertyChanged")
                        notified = true;
                };

                // execute
                target.ViaRaisePropertyChanged = true;

                // verify
                Assert.AreEqual(true, target.ViaRaisePropertyChanged);
                Assert.IsTrue(notified);
            }
        }

        public class ViewModelA : ViewModelBase
        {
            private bool viaSetValue;
            private bool viaRaisePropertyChanged;

            public bool ViaSetValue
            {
                get { return this.viaSetValue; }
                set { this.SetValue(ref this.viaSetValue, value, "ViaSetValue"); }
            }

            public bool ViaRaisePropertyChanged
            {
                get { return this.viaRaisePropertyChanged; }
                set
                {
                    if (this.viaRaisePropertyChanged != value)
                    {
                        this.viaRaisePropertyChanged = value;
                        this.RaisePropertyChanged("ViaRaisePropertyChanged");
                    }
                }
            }
        }
    }
}
