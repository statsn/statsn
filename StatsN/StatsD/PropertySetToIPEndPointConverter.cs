using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;

namespace StatsN.StatsD
{
    class PropertySetToIPEndPointConverter
    {
        public IPEndPoint Convert(IReadOnlyDictionary<string, string> config)
        {
            var port = config.ContainsKey("port") ? Int32.Parse(config["port"]) : 8125;
            var address = config.ContainsKey("address") ? IPAddress.Parse(config["address"]) : IPAddress.Loopback;

            return new IPEndPoint(address, port);
        }

    }
}
