using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using StatsN.Core;
using System.Reactive.Disposables;

namespace StatsN.StatsD.Backends
{
    class EmptyMetricGroup : IGroupedObservable<string, Metric>
    {
        public EmptyMetricGroup(string key)
        {
            Key = key;
        }

        public string Key
        {
            get;
            private set;
        }

        public IDisposable Subscribe(IObserver<Metric> observer)
        {
            observer.OnCompleted();
            return Disposable.Empty;
        }
    }
}
