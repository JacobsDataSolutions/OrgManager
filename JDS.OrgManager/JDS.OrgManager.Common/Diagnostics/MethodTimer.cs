// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace JDS.OrgManager.Common.Diagnostics
{
    public static class MethodTimer
    {
        public static (decimal min, decimal max, decimal average) TimeIt(Action action, int iterations = 1)
        {
            if (iterations < 1)
            {
                throw new InvalidOperationException();
            }
            var times = new List<decimal>();
            var watch = new Stopwatch();
            for (var i = 0; i < iterations; i++)
            {
                watch.Restart();
                action();
                watch.Stop();
                times.Add(watch.ElapsedMilliseconds);
            }
            return (times.Min(), times.Max(), times.Average());
        }
    }
}