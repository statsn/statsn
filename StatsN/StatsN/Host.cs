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
            
            var meta = Observable.Create<MetaMetric>(_ => Disposable.Empty);

            var backendLoader = new Plugins.BackendLoader().Load(config.Backends);

            var metrics = frontendLoader.Plugins.SelectMany(plugin => plugin.Run()).Publish();
            backendLoader.Plugins.Subscribe(_ => _.Run(metrics,  meta));

            metrics.Connect();
            System.Diagnostics.Debug.WriteLine("Loading complete");
        }
    }
}
