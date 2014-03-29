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
    abstract class StatsDBackend : IBackend
    {
        public void Run(IObservable<Metric> metrics, IObservable<MetaMetric> meta)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("StatsD {0} backend running", GetType().Name));
            metrics
                .Window(TimeSpan.FromSeconds(10))
                .Subscribe(FlushWindow);
        }

        private void FlushWindow(IObservable<Metric> events)
        {
            var counters = events.Where(e => e.Namespace == null || e.Namespace == "c");
            var timers = events.Where(e => e.Namespace == null || e.Namespace == "ms");

            FlushCounters(counters);
            FlushTimers(timers);
        }

        private void FlushCounters(IObservable<Metric> events)
        {
            events.GroupBy(e => e.Name)
               .Subscribe(grouping =>
               {
                   var count = grouping.Sum(e => e.Count);

                   count.Subscribe(sum =>
                   {
                       var key = grouping.Key;
                       FlushCounters(key, sum);
                   });
               });
        }

        
        private void FlushTimers(IObservable<Metric> events)
        {
            events
                .GroupBy(e => e.Name)
                .SelectMany(group =>
                    {
                        var aggregator = new Aggregations.MeasurementAggregator();
                        
                        return aggregator.Aggregate(group).Select(value => new { group.Key, Metrics = value });
                    })
                .Subscribe(_ =>
                {
                    var key = _.Key;
                    var metrics = _.Metrics;

                    FlushTimers(key, metrics);
                });
        }

        protected abstract void FlushTimers(string key, MeasurementMetrics metrics);

        protected abstract void FlushCounters(string key, float sum);
    }
}
