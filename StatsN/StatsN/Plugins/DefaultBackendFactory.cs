using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatsN.Core;
using StatsN.Backends;

namespace StatsN
{
    class DefaultBackendFactory : IBackendFactory
    {
        Type Type { get; set; }
        public DefaultBackendFactory(Type type)
        {
            Type = type;
        }

        public IBackend Create(IReadOnlyDictionary<string, string> config, IObservable<IMetric> descrete)
        {
            var backend = (IBackend)Activator.CreateInstance(Type);
            backend.Events = descrete;
            return backend;
        }
    }
}
