using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Subjects;
using System.Reflection;
using StatsN.Frontends;

namespace StatsN.Plugins
{
    class FrontendLoader : PluginLoader<IFrontend, IFrontendFactory>
    {
        protected override IFrontend CreatePlugin(IFrontendFactory factory, Configuration.PluginConfig config)
        {
            System.Diagnostics.Debug.WriteLine("Creating frontend plugin using {0}", factory.GetType());
            return factory.Create(config.Properties);
        }

        protected override Type GenericFactoryInterface
        {
            get { return typeof(IFrontendFactory<>); }
        }

        protected override IFrontendFactory CreateDefaultFactory(Type pluginType)
        {
            return new DefaultFrontendFactory(pluginType);
        }
    }
}
