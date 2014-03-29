using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using StatsN.Core;

namespace StatsN
{
    class Host
    {
        public Host(Configuration.Configuration config)
        {
            System.Diagnostics.Debug.WriteLine("Loading starts");
            var frontendLoader = new Plugins.FrontendLoader().Load(config.Frontends);
            
            var metrics = Observable.Merge<Metric>(frontendLoader.Plugins.Select(fe => fe.Events));
            var meta = Observable.Create<MetaMetric>(_ => Disposable.Empty);

            var backendLoader = new Plugins.BackendLoader().Load(config.Backends);
            frontendLoader.Plugins.Subscribe(_ => Task.Factory.StartNew(_.Run));
            backendLoader.Plugins.Subscribe(_ => _.Run(metrics,  meta));

            System.Diagnostics.Debug.WriteLine("Loading complete");
        }
    }
}
