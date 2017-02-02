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

namespace SrkToolkit.Mvvm.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SrkToolkit.Mvvm.Tools;

    [TestClass]
    public class BusyTaskCollectionTests
    {
        [TestClass]
        public class AddTaskMethod
        {
            [TestMethod, TestCategory(Category.Unit)]
            public void AddWithValuesWorks()
            {
                // prepare 
                string key = ";;";
                bool isGlobal = true;
                var target = new BusyTaskCollection();

                // execute
                target.Add(key, isGlobal);

                // varify
                Assert.AreEqual(1, target.Count);
                Assert.AreEqual(key, target[0].Key);
                Assert.AreEqual(isGlobal, target[0].IsGlobal);
            }

            [TestMethod, TestCategory(Category.Unit)]
            public void AddWithTaskWorks()
            {
                // prepare 
                var task = new BusyTask();
                var target = new BusyTaskCollection();

                // execute
                target.Add(task);

                // varify
                Assert.AreEqual(task, target[0]);
            }
        }

        [TestClass]
        public class Indexer
        {
            [TestMethod, TestCategory(Category.Unit)]
            public void IndexerWorks()
            {
                // prepare
                string key = "aaaa";
                var task = new BusyTask(key);
                var target = new BusyTaskCollection();
                target.Add(task);
                BusyTask result = null;

                // execute
                result = target[key];

                // verify
                Assert.AreSame(task, result);
            }

            [TestMethod, TestCategory(Category.Unit)]
            public void IndexerFailsWithWrongKey()
            {
                // prepare
                string key = "aaaa";
                var target = new BusyTaskCollection();
                BusyTask result = null;

                // execute
                result = target[key];

                // verify
                Assert.IsNull(result);
            }

            [TestMethod, TestCategory(Category.Unit)]
            public void IndexerFailsWithNullKey()
            {
                // prepare
                string key = null;
                var target = new BusyTaskCollection();
                BusyTask result = null;

                // execute
                result = target[key];

                // verify
                Assert.IsNull(result);
            }
        }

        [TestClass]
        public class UpdateMethod
        {
            [TestMethod, TestCategory(Category.Unit)]
            public void UpdateWorks()
            {
                // prepare
                string key = "aaaa";
                string message = "Hello world";
                var task = new BusyTask(key);
                var target = new BusyTaskCollection();
                target.Add(task);

                // execute
                target.Update(key, message, true, BusyTaskType.Error);

                // verify
                Assert.AreEqual(message, task.Message);
                Assert.IsTrue(task.IsProcessing);
                Assert.AreEqual(BusyTaskType.Error, task.Type);
            }
        }

        [TestClass]
        public class AggregateMessageProperty
        {
            [TestMethod, TestCategory(Category.Unit)]
            public void DefaultsToNullWhenZeroTasks()
            {
                // prepare
                var target = new BusyTaskCollection();
                string result = null;

                // execute
                result = target.AggregateMessage;

                // verify
                Assert.IsNull(result);
            }
        }
    }
}
