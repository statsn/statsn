using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Linq;
using StatsN.Core;

namespace StatsN.StatsD.Backends.Aggregations
{
    class MeasurementAggregator
    {
        public IObservable<MeasurementMetrics> Aggregate(IObservable<Measurement> source)
        {
            return Observable.Create<MeasurementMetrics>(sink =>
            {
                var producer = new Observer(sink);
                return source.SubscribeSafe(producer);
            });
        }

        class Observer : IObserver<Measurement>
        {
            private double Upper = Double.NegativeInfinity, Lower = Double.PositiveInfinity, Sum, Mean, M2;
            private long Count;

            ICollection<Double> Items = new LinkedList<double>();

            private IObserver<MeasurementMetrics> _observer;

            public Observer(IObserver<MeasurementMetrics> observer)
            {
                _observer = observer;
            }

            public void OnCompleted()
            {
                var variance = M2 / (Count - 1);
                var std_dev = Math.Sqrt(variance);

                double median;
                double p90;
                CalculateMedianAndP90(out median, out p90);

                var result = new MeasurementMetrics(
                   upper: Upper,
                   lower: Lower,
                   sum: Sum,
                   count: Count,
                   mean: Mean,
                   std: std_dev,
                   median: median,
                   p90: p90
                   );

                _observer.OnNext(result);
                _observer.OnCompleted();
            }

            private void CalculateMedianAndP90(out double median, out double p90)
            {
                median = 0;
                p90 = 0;

                var p90index = (int)Math.Ceiling(Count * 0.9);

                var median1index = (Count - 1) / 2;
                var median2index = (Count) / 2;


                var items = Items.OrderBy(_ => _);

                long index = 0;
                foreach (var val in items.Take(p90index))
                {

                    if (index == median1index || index == median2index)
                    {
                        median += val;
                    }
                    else if (index == p90index)
                    {

                        p90 = val;
                    }
                    index++;
                }

                if (median1index != median2index)
                    median *= 0.5;
            }

            public void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public void OnNext(Measurement value)
            {
                var delta = value.Value - Mean;
                Count += 1;
                Mean += delta / Count;
                M2 += delta * (value.Value - Mean);

                Sum += value.Value;
                Upper = Math.Max(Upper, value.Value);
                Lower = Math.Min(Lower, value.Value);

                Items.Add(value.Value);
            }
        }
    }
}
