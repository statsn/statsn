using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Subjects;
using System.Reflection;

namespace StatsN
{
    class FrontendLoader
    {
        public IObservable<IFrontend> Frontends { get; private set; }
        private ISubject<IFrontend> FrontendsSubject { get; set; }

        public FrontendLoader(IEnumerable<Configuration.PluginConfig> plugins)
        {
            Frontends = FrontendsSubject = new ReplaySubject<IFrontend>();
            foreach (var plugin in plugins.Where(config => config.Enabled))
            {
                try
                {
                    FrontendsSubject.OnNext(CreatePlugin(plugin));
                }
                catch (PluginUnloadableException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private IFrontend CreatePlugin(Configuration.PluginConfig config)
        {
            if (config.Path != null)
            {
                Assembly.Load(config.Path);
            }
            var frontendType = FindPluginType(config);
            var factoryType = FindPluginFactoryType(config, frontendType);

            IFrontendFactory factory;
            if (factoryType != null)
                factory = new FrontendFactory(frontendType);
            else
                factory = (IFrontendFactory)Activator.CreateInstance(factoryType);

            System.Diagnostics.Debug.WriteLine("Creating frontend plugin using {0}", factoryType);
            return factory.Create(config.Properties);
        }

        private Type FindPluginFactoryType(Configuration.PluginConfig config, Type frontendType)
        {
            var factoryTypes = frontendType.Assembly
                .GetTypes()
                .Where(type => type.GetInterfaces().Any(interfaceType =>
                    interfaceType.IsGenericType &&
                    interfaceType.GetGenericTypeDefinition() == typeof(IFrontendFactory<>) &&
                    interfaceType.GetGenericArguments()[0] == frontendType
                    ));

            if (factoryTypes.Count() > 1)
                throw new PluginUnloadableException(String.Format("{0} has ambigous factories", config.Name));
            return factoryTypes.FirstOrDefault();
        }

        private static Type FindPluginType(Configuration.PluginConfig config)
        {

            var types = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(asm => asm.GetTypes()
                .Where(type => type.Name.EndsWith(config.Name, StringComparison.InvariantCultureIgnoreCase)));

            if (types.Count() == 0)
                throw new PluginUnloadableException(String.Format("Couldn't find type {0}", config.Name));
            else
                types = types.Where(type => typeof(IFrontend).IsAssignableFrom(type));

            if (types.Count() == 0)
                throw new PluginUnloadableException(String.Format("Couldn't {0} does not implement IFrontend", config.Name));

            if (types.Count() > 1)
                throw new PluginUnloadableException(String.Format("{0} is ambiguous", config.Name));

            var frontendType = types.First();
            return frontendType;
        }
    }
}
