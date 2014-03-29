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
    class Console : IBackend
    {
        public IObservable<IMetric> Events
        {
            get;
            set;
        }

        public void Run()
        {
            System.Console.WriteLine("Console backend running");

            Events
                .Window(TimeSpan.FromSeconds(1))
                .Subscribe(FlushWindow);
        }

        private void FlushWindow(IObservable<IMetric> events)
        {

            var counters = events.Where(e => e.Namespace == null || e.Namespace == "c").Events();
            var timers = events.Where(e => e.Namespace == null || e.Namespace == "ms").Measures();

            
            FlushCounters(counters);
            FlushTimers(timers);
        }

        private void FlushCounters(IObservable<DescreteEvent> events)
        {
            events.Subscribe((_) => { }, () =>
                {
                    System.Console.WriteLine("\n{0,-10} {1,-5} {2}", "Key", "Count", "Rate");
                });
            events.GroupBy(e => e.Name)
               .Subscribe(grouping =>
               {
                   var count = grouping.Sum(e => e.Count);

                   count.Subscribe(sum =>
                   {
                       System.Console.WriteLine("{0,-10} {1,-5} {2:0.00}", grouping.Key, sum, sum / 10.0);
                   });
               });
        }

        private void FlushTimers(IObservable<Measurement> events)
        {
            /*
            mean_90: 23,
            upper_90: 45,
            sum_90: 1035,
            std: 14.430869689661812,
            upper: 50,
            lower: 1,
            count: 50,
            count_ps: 5,
            sum: 1275,
            mean: 25.5,
            median: 25.5
            */

            events
                .GroupBy(e => e.Name)
                .SelectMany(group =>
                    {
                        var aggregator = new Aggregations.MeasurementAggregator();
                        
                        return aggregator.Aggregate(group).Select(value => new { group.Key, Metrics = value });
                    })
                .Subscribe(_ =>
                {
                    System.Console.WriteLine("{0,-10} {1,-5}", _.Key, _.Metrics.Mean);
                });
        }


    }
}
