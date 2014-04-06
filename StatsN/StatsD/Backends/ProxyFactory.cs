using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatsN.Backends;
using System.Net;
using System.Net.Sockets;

namespace StatsN.StatsD.Backends
{
    class ProxyFactory : IBackendFactory<Proxy>
    {
        public IBackend Create(IReadOnlyDictionary<string, string> config)
        {
            var converter = new PropertySetToIPEndPointConverter();
            var endpoint = converter.Convert(config);

            var proxy = new Proxy(endpoint);

            if (config.ContainsKey("buffer-time"))
            {
                proxy.Buffer(TimeSpan.Parse(config["buffer-time"]));
            }

            if (config.ContainsKey("buffer-count"))
            {
                proxy.Buffer(Int32.Parse(config["buffer-count"]));
            }

            return proxy;
        }
    }
}
