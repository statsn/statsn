
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using StatsN.Frontends;

namespace StatsN.StatsD.Frontends
{
    public class StatsDUdpFactory : IFrontendFactory<Udp>
    {
        public IFrontend Create(IReadOnlyDictionary<string, string> config)
        {
            var port = config.ContainsKey("port") ? Int32.Parse(config["port"]) : 8125;
            var address = config.ContainsKey("address") ? IPAddress.Parse(config["address"]) : IPAddress.Loopback;

            var ep = new IPEndPoint(address, port);
            return new Udp(ep);
        }
    }
}
