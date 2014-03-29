using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;


namespace StatsN.Core
{
    public static class ObservableExtensions
    {
        public static IObservable<TOut> As<TOut, TIn>(this IObservable<TIn> sequence)
            where TIn : class
            where TOut : class
        {
            return sequence.Where(x => x is TOut)
                .Select(x => x as TOut);
        }

        public static  IObservable<DescreteEvent> Events(this IObservable<IMetric> sequence)
        {
            return sequence.As<DescreteEvent, IMetric>();
        }

        public static IObservable<Measurement> Measures(this IObservable<IMetric> sequence)
        {
            return sequence.As<Measurement, IMetric>();
        }
    }
}
