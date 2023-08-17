// -----------------------------------------------------------------------
// <copyright file="ObservableCollectionExtensionsTests.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using Xunit;

    public class ObservableCollectionExtensionsTests
    {
        public class AddRangeMethod
        {
            [Fact]
            public void ThrowsWhenArg0IsNull()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    SrkObservableCollectionExtensions.AddRange<string>(null, new string[0]);
                });
            }

            [Fact]
            public void ThrowsWhenArg1IsNull()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    SrkObservableCollectionExtensions.AddRange<string>(new ObservableCollection<string>(), null);
                });
            }

            [Fact]
            public void CorrectlyAddsElements()
            {
                var target = new ObservableCollection<string>();
                var elements = new string[] { "a", "b", };

                SrkObservableCollectionExtensions.AddRange<string>(target, elements);

                Assert.Equal(2, target.Count);
            }

            [Fact]
            public void ReturnsSelf()
            {
                var target = new ObservableCollection<string>();
                var elements = new string[] { };

                var result = SrkObservableCollectionExtensions.AddRange<string>(target, elements);

                Assert.Equal(target, result);
            }
        }

        public class RemoveAllMethod
        {
            [Fact]
            public void ThrowsWhenArg0IsNull()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    SrkObservableCollectionExtensions.RemoveAll<string>(null, s => true);
                });
            }

            [Fact]
            public void ThrowsWhenArg1IsNull()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    SrkObservableCollectionExtensions.RemoveAll<string>(new ObservableCollection<string>(), null);
                });
            }

            [Fact]
            public void RemovesElements()
            {
                var elements = new string[] { "a", "b", "c", };
                var target = new ObservableCollection<string>(elements);
                Func<string, bool> predicate = c => c == "b";

                SrkObservableCollectionExtensions.RemoveAll(target, predicate);
                
                Assert.Equal(2, target.Count);
            }

            [Fact]
            public void RemovesSameElements()
            {
                var elements = new string[] { "a", "b", "b", "c", };
                var target = new ObservableCollection<string>(elements);
                Func<string, bool> predicate = c => c == "b";

                SrkObservableCollectionExtensions.RemoveAll(target, predicate);
                
                Assert.Equal(2, target.Count);
            }

            [Fact]
            public void ReturnsSelf()
            {
                var elements = new string[] { "a", "b", "c", };
                var target = new ObservableCollection<string>(elements);
                Func<string, bool> predicate = c => c == "b";

                var result = SrkObservableCollectionExtensions.RemoveAll<string>(target, predicate);

                Assert.Equal(target, result);
            }
        }
    }
}
