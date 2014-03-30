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

        IObservable<IGroupedObservable<string, Metric>> PastKeys;


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
                .Catch<IGroupedObservable<string, Metric>, TimeoutException>(_ => Observable.Empty<IGroupedObservable<string, Metric>>());
        }

        private void FlushWindow(IObservable<Metric> events)
        {
            var counters = events.Where(e => e.NamespaceTag == Tags.Counter);
            var timers = events.Where(e => e.NamespaceTag == Tags.Timer);

            FlushCounters(GroupMetrics(counters));
            FlushTimers(GroupMetrics(timers));
        }

        private IObservable<IGroupedObservable<string, Metric>> GroupMetrics(IObservable<Metric> metrics)
        {
            return metrics
                .GroupBy(e => e.Name)
                .Concat(PastKeys)
                .Distinct(e => e.Key).Select(_ => (IGroupedObservable<string, Metric>)_);
        }

        private void FlushCounters(IObservable<IGroupedObservable<string, Metric>> events)
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

        private void FlushTimers(IObservable<IGroupedObservable<string, Metric>> events)
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
