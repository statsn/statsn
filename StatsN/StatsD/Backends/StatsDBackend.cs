using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using StatsN.Core;
using StatsN.Backends;
using StatsN.StatsD.Backends.Aggregations;

namespace StatsN.StatsD.Backends
{
    abstract class StatsDBackend : IBackend
    {

        IObservable<IMetricGroup> PastKeys;


        public void Run(IObservable<Metric> metrics, IObservable<MetaMetric> meta)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("StatsD {0} backend running", GetType().Name));
           
            ConfigurePastKeyStorage(metrics);

            metrics
                .Window(TimeSpan.FromSeconds(3))
                .Subscribe(FlushWindow);
        }

        private void ConfigurePastKeyStorage(IObservable<Metric> metrics)
        {
            var replay = metrics
                .GroupBy(e => e.Name)
                .Select(e => new EmptyMetricGroup(e.Key))
                .Replay();

            replay.Connect();

            PastKeys = replay
                .Timeout(TimeSpan.Zero) // Complete the sequence on subscription
                .Catch<IMetricGroup, TimeoutException>(_ => Observable.Empty<IMetricGroup>());
        }

        private void FlushWindow(IObservable<Metric> events)
        {
            var counters = events.Where(e => e.Namespace == null || e.Namespace == "c");
            var timers = events.Where(e => e.Namespace == null || e.Namespace == "ms");

            FlushCounters(GroupMetrics(counters));
            FlushTimers(GroupMetrics(timers));
        }

        private IObservable<IMetricGroup> GroupMetrics(IObservable<Metric> metrics)
        {
            return metrics
                .GroupBy(e => e.Name)
                .Concat(PastKeys)
                .Distinct(e => e.Key).Select(_ => (IMetricGroup)_);
        }

        private void FlushCounters(IObservable<IMetricGroup> events)
        {
            events
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

        private void FlushTimers(IObservable<IMetricGroup> events)
        {
            events
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
