using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using StatsN.Core;

namespace StatsN
{
    class Host
    {
        public Host(Configuration.Configuration config)
        {
            System.Diagnostics.Debug.WriteLine("Loading starts");
            var frontendLoader = new Plugins.FrontendLoader().Load(config.Frontends);
            
            var descrete = Observable.Merge<DescreteEvent>(frontendLoader.Plugins.Select(fe => fe.DescreteEvents));
            var measure = Observable.Merge<Measurement>(frontendLoader.Plugins.Select(fe => fe.Measures));

            var backendLoader = new Plugins.BackendLoader(descrete, measure).Load(config.Backends);
            frontendLoader.Plugins.Subscribe(_ => Task.Factory.StartNew(_.Run));
            backendLoader.Plugins.Subscribe(_ => _.Run());

            System.Diagnostics.Debug.WriteLine("Loading complete");
        }
    }
}
