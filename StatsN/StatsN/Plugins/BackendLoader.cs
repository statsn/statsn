using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatsN.Core;
using StatsN.Backends;

namespace StatsN.Plugins
{
    class BackendLoader : PluginLoader<IBackend, IBackendFactory>
    {
        protected override IBackend CreatePlugin(IBackendFactory factory, Configuration.PluginConfig config)
        {
            System.Diagnostics.Debug.WriteLine("Creating Backend plugin using {0}", factory.GetType());
            return factory.Create(config.Properties);
        }

        protected override Type GenericFactoryInterface
        {
            get { return typeof(IBackendFactory<>); }
        }

        protected override IBackendFactory CreateDefaultFactory(Type pluginType)
        {
            return new DefaultBackendFactory(pluginType);
        }
    }
}
