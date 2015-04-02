// -----------------------------------------------------------------------
// <copyright file="ObservableCollectionExtensionsTests.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.ObjectModel;

    public class ObservableCollectionExtensionsTests
    {
        [TestClass]
        public class AddRangeMethod
        {
            [TestMethod, ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsWhenArg0IsNull()
            {
                SrkObservableCollectionExtensions.AddRange<string>(null, new string[0]);
            }

            [TestMethod, ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsWhenArg1IsNull()
            {
                SrkObservableCollectionExtensions.AddRange<string>(new ObservableCollection<string>(), null);
            }

            [TestMethod]
            public void CorrectlyAddsElements()
            {
                var target = new ObservableCollection<string>();
                var elements = new string[] { "a", "b", };

                SrkObservableCollectionExtensions.AddRange<string>(target, elements);

                Assert.AreEqual(2, target.Count);
            }

            [TestMethod]
            public void ReturnsSelf()
            {
                var target = new ObservableCollection<string>();
                var elements = new string[] { };

                var result = SrkObservableCollectionExtensions.AddRange<string>(target, elements);

                Assert.AreEqual(target, result);
            }
        }

        [TestClass]
        public class RemoveAllMethod
        {
            [TestMethod, ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsWhenArg0IsNull()
            {
                SrkObservableCollectionExtensions.RemoveAll<string>(null, s => true);
            }

            [TestMethod, ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsWhenArg1IsNull()
            {
                SrkObservableCollectionExtensions.RemoveAll<string>(new ObservableCollection<string>(), null);
            }

            [TestMethod]
            public void RemovesElements()
            {
                var elements = new string[] { "a", "b", "c", };
                var target = new ObservableCollection<string>(elements);
                Func<string, bool> predicate = c => c == "b";

                SrkObservableCollectionExtensions.RemoveAll(target, predicate);
                
                Assert.AreEqual(2, target.Count);
            }

            [TestMethod]
            public void RemovesSameElements()
            {
                var elements = new string[] { "a", "b", "b", "c", };
                var target = new ObservableCollection<string>(elements);
                Func<string, bool> predicate = c => c == "b";

                SrkObservableCollectionExtensions.RemoveAll(target, predicate);
                
                Assert.AreEqual(2, target.Count);
            }

            [TestMethod]
            public void ReturnsSelf()
            {
                var elements = new string[] { "a", "b", "c", };
                var target = new ObservableCollection<string>(elements);
                Func<string, bool> predicate = c => c == "b";

                var result = SrkObservableCollectionExtensions.RemoveAll<string>(target, predicate);

                Assert.AreEqual(target, result);
            }
        }
    }
}
