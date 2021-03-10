// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using Microsoft.Extensions.Logging;
using System;
using Xunit.Abstractions;

namespace JDS.OrgManager.Common.Testing
{
    public class XUnitLogger : ILogger
    {
        private readonly string _categoryName;

        private readonly ITestOutputHelper _testOutputHelper;

        public XUnitLogger(ITestOutputHelper testOutputHelper, string categoryName)
        {
            _testOutputHelper = testOutputHelper;
            _categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state)
            => NoopDisposable.Instance;

        public bool IsEnabled(LogLevel logLevel)
            => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _testOutputHelper.WriteLine($"{_categoryName} [{eventId}] {formatter(state, exception)}");
            if (exception != null)
                _testOutputHelper.WriteLine(exception.ToString());
        }

        private class NoopDisposable : IDisposable
        {
            public static NoopDisposable Instance = new NoopDisposable();

            public void Dispose()
            { }
        }
    }
}