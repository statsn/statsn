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
   
    class Console : AggregationBackend
    {

      
        protected override void FlushTimers(string key, MeasurementMetrics metrics)
        {
            System.Console.WriteLine("{0, -10} {1, -5} {2,-5:0.##} {3,-5:0.##}", key, metrics.Count, metrics.Mean, metrics.Median);
        }

        protected override void FlushCounters(string key, float sum)
        {
            System.Console.WriteLine("{0,-10} {1,-5:0.##} {2:0.##}", key, sum, sum / 10.0);
        }
    }
}
