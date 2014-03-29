using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using StatsN.Core;
using StatsN.Backends;
using StatsN.StatsD.Backends.Aggregations;


namespace StatsN.StatsD.Backends
{
   
    class Console : StatsDBackend
    {
        protected override void FlushTimers(string key, MeasurementMetrics metrics)
        {
            System.Console.WriteLine("{0,-10} {1,-5}", key, metrics.Count);
        }

        protected override void FlushCounters(string key, float sum)
        {
            System.Console.WriteLine("{0,-10} {1,-5} {2:0.00}", key, sum, sum / 10.0);
        }
    }
}
