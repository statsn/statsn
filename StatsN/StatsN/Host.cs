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

        private readonly Configuration.Configuration Configuration;
 
        public Host(Configuration.Configuration config)
        {
            Configuration = config;
        }

        public void Start()
        {
            System.Diagnostics.Debug.WriteLine("Loading starts");
            var frontendLoader = new Plugins.FrontendLoader().Load(Configuration.Frontends);

            var meta = Observable.Create<MetaMetric>(_ => Disposable.Empty);

            var backendLoader = new Plugins.BackendLoader().Load(Configuration.Backends);

            var metrics = frontendLoader.Plugins.SelectMany(plugin => plugin.Run()).Publish();
            backendLoader.Plugins.Subscribe(backend => backend.Run(metrics, meta));

            metrics.Connect();
            System.Diagnostics.Debug.WriteLine("Loading complete");

        }

    }
}
