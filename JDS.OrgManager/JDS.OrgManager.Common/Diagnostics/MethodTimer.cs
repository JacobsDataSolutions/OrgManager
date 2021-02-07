using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
