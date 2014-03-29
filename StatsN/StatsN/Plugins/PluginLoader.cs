using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using StatsN.Configuration;


namespace StatsN.Plugins
{
    abstract class PluginLoader<TPluginInterface, TFactoryInterface>
    {
        public IObservable<TPluginInterface> Plugins { get; private set; }
        private IObserver<TPluginInterface> PluginsObservable { get; set; }

        private Type PluginInterface { get { return typeof(TPluginInterface); } }
        protected abstract Type GenericFactoryInterface { get; }
        protected abstract TPluginInterface CreatePlugin(TFactoryInterface factory, PluginConfig config);
        protected abstract TFactoryInterface CreateDefaultFactory(Type pluginType);

        protected PluginLoader()
        {
            var pluginStream = Observable.Create<TPluginInterface>(observer =>
                {
                    PluginsObservable = observer;
                    return Disposable.Create(() => { });

                })
                .Replay();

            pluginStream.Connect();
            Plugins = pluginStream;
        }

        public PluginLoader<TPluginInterface, TFactoryInterface> Load(IEnumerable<PluginConfig> plugins)
        {
            foreach (var plugin in plugins.Where(config => config.Enabled))
            {
                try
                {
                    var factory = CreateFactory(plugin);
                    PluginsObservable.OnNext(CreatePlugin(factory, plugin));
                }
                catch (PluginUnloadableException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
            return this;
        }

        protected TFactoryInterface CreateFactory(PluginConfig config)
        {
            var frontendType = FindPluginType(config);
            var factoryType = FindPluginFactoryType(config, frontendType);

            TFactoryInterface factory;
            if (factoryType == null)
                factory = CreateDefaultFactory(frontendType);
            else
                factory = (TFactoryInterface)Activator.CreateInstance(factoryType);
            return factory;
        }

        private Type FindPluginType(PluginConfig config)
        {
            if (config.Path != null)
            {
                Assembly.Load(config.Path);
            }

            var types = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(asm => asm.GetTypes()
                .Where(type => type.FullName.EndsWith(config.Name, StringComparison.InvariantCultureIgnoreCase)));

            if (types.Count() == 0)
                throw new PluginUnloadableException(String.Format("Couldn't find type {0}", config.Name));
            else
                types = types.Where(t => PluginInterface.IsAssignableFrom(t));

            if (types.Count() == 0)
                throw new PluginUnloadableException(String.Format("Couldn't {0} does not implement {1}", config.Name, PluginInterface.Name));

            if (types.Count() > 1)
                throw new PluginUnloadableException(String.Format("{0} is ambiguous", config.Name));

            return types.First();
        }

        private Type FindPluginFactoryType(PluginConfig config, Type pluginType)
        {
            var factoryTypes = pluginType.Assembly
                .GetTypes()
                .Where(type => type.GetInterfaces().Any(interfaceType =>
                    interfaceType.IsGenericType &&
                    interfaceType.GetGenericTypeDefinition() == GenericFactoryInterface &&
                    interfaceType.GetGenericArguments()[0] == pluginType
                    ));

            if (factoryTypes.Count() > 1)
                throw new PluginUnloadableException(String.Format("{0} has ambigous factories", config.Name));
            return factoryTypes.FirstOrDefault();
        }
    }
}
