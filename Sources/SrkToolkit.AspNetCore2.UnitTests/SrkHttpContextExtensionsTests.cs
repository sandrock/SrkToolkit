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

namespace SrkToolkit.Web.Tests
{
    using SrkToolkit.Web.Fakes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class SrkHttpContextExtensionsTests
    {
        public class SetTimezoneMethod
        {
            [Fact]
            public void WorksWithTzObject()
            {
                var tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
                var http = new BasicHttpContext();
                SrkHttpContextExtensions.SetTimezone(http, tz);

                Assert.NotNull(http.Items["Timezone"]);
                Assert.Equal(tz, http.Items["Timezone"]);
            }

            [Fact]
            public void WorksWithTzName()
            {
                var tzName = "Romance Standard Time";
                var tz = TimeZoneInfo.FindSystemTimeZoneById(tzName);
                var http = new BasicHttpContext();
                SrkHttpContextExtensions.SetTimezone(http, tzName);

                Assert.NotNull(http.Items["Timezone"]);
                Assert.Equal(tz, http.Items["Timezone"]);
            }

            [Fact, ExpectedException(typeof(ArgumentException))]
            public void NullTzName()
            {
                string tzName = null;
                var http = new BasicHttpContext();
                SrkHttpContextExtensions.SetTimezone(http, tzName);
            }

            [Fact, ExpectedException(typeof(ArgumentException))]
            public void EmptyTzName()
            {
                string tzName = string.Empty;
                var http = new BasicHttpContext();
                SrkHttpContextExtensions.SetTimezone(http, tzName);
            }

            [Fact, ExpectedException(typeof(TimeZoneNotFoundException))]
            public void InvalidTzName()
            {
                string tzName = "Lunar Standard Time";
                var http = new BasicHttpContext();
                SrkHttpContextExtensions.SetTimezone(http, tzName);
            }

            [Fact]
            public void GetterWorks()
            {
                var tzName = "Romance Standard Time";
                var tz = TimeZoneInfo.FindSystemTimeZoneById(tzName);
                var http = new BasicHttpContext();
                SrkHttpContextExtensions.SetTimezone(http, tzName);
                var result = SrkHttpContextExtensions.GetTimezone(http);

                Assert.NotNull(result);
                Assert.Equal(tz, result);
            }
        }
    }
}
