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
    using SrkToolkit.Mvvm.Commands;

    [TestClass]
    public class RelayCommandTests
    {
        [TestMethod]
        public void ExecuteWorks()
        {
            // prepare
            bool executed = false;
            var execute = new Action(() => executed = true);

            // execute
            var target = new RelayCommand(execute);
            target.Execute(null);

            // verify
            Assert.IsTrue(executed, "Command did not execute");
        }

        [TestMethod]
        public void ExecuteGenericWorks()
        {
            // prepare
            bool executed = false;
            int param = 42, paramReceived = int.MinValue;
            var execute = new Action<int>((i) =>
            {
                executed = true;
                paramReceived = i;
            });

            // execute
            var target = new RelayCommand<int>(execute);
            target.Execute(param);

            // verify
            Assert.IsTrue(executed, "Command did not execute");
            Assert.AreEqual(param, paramReceived, "Command did not pass parameter");
        }

        [TestMethod]
        public void CanExecuteWorks()
        {
            // prepare
            bool executed = false;
            var execute = new Action(() => { });
            var can = new Func<bool>(() =>
            {
                executed = true;
                return false;
            });
            bool result;

            // execute
            var target = new RelayCommand(execute, can, false);
            result = target.CanExecute(null);

            // verify
            Assert.IsTrue(executed, "Command did not test canexecute");
            Assert.IsFalse(result, "Command did not pass parameter");
        }

        [TestMethod]
        public void CanExecuteGenericWorks()
        {
            // prepare
            bool executed = false;
            int param = 42, paramReceived = int.MinValue;
            var execute = new Action<int>((i) => { });
            var can = new Predicate<int>((i) =>
            {
                executed = true;
                paramReceived = i;
                return false;
            });
            bool result;

            // execute
            var target = new RelayCommand<int>(execute, can, false);
            result = target.CanExecute(param);

            // verify
            Assert.IsTrue(executed, "Command did not test canexecute");
            Assert.IsFalse(result, "Command did not pass parameter");
        }

        [TestMethod]
        public void CanExecutePreventsExecuteWorks()
        {
            // prepare
            bool executed = false;
            bool execed = false;
            var execute = new Action(() =>
            {
                execed = true;
            });
            var can = new Func<bool>(() =>
            {
                executed = true;
                return false;
            });

            // execute
            var target = new RelayCommand(execute, can, true);
            target.Execute(null);

            // verify
            Assert.IsFalse(execed, "Command executed execute");
        }

        [TestMethod]
        public void CanExecutePreventsExecuteGenericWorks()
        {
            // prepare
            bool executed = false;
            bool execed = false;
            int param = 42, paramReceived = int.MinValue;
            var execute = new Action<int>((i) =>
            {
                execed = true;
            });
            var can = new Predicate<int>((i) =>
            {
                executed = true;
                paramReceived = i;
                return false;
            });

            // execute
            var target = new RelayCommand<int>(execute, can, true);
            target.Execute(param);

            // verify
            Assert.IsFalse(execed, "Command executed execute");
        }

        [TestMethod]
        public void CanExecutePreventsNotExecuteWorks()
        {
            // prepare
            bool executed = false;
            bool execed = false;
            var execute = new Action(() =>
            {
                execed = true;
            });
            var can = new Func<bool>(() =>
            {
                executed = true;
                return false;
            });

            // execute
            var target = new RelayCommand(execute, can, false);
            target.Execute(null);

            // verify
            Assert.IsTrue(execed, "Command executed execute");
        }

        [TestMethod]
        public void CanExecutePreventsNotExecuteGenericWorks()
        {
            // prepare
            bool executed = false;
            bool execed = false;
            int param = 42, paramReceived = int.MinValue;
            var execute = new Action<int>((i) =>
            {
                execed = true;
            });
            var can = new Predicate<int>((i) =>
            {
                executed = true;
                paramReceived = i;
                return false;
            });

            // execute
            var target = new RelayCommand<int>(execute, can, false);
            target.Execute(param);

            // verify
            Assert.IsTrue(execed, "Command executed execute");
        }
    }
}
