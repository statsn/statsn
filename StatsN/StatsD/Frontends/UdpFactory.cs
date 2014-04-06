
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using StatsN.Frontends;

namespace StatsN.StatsD.Frontends
{
    public class UdpFactory : IFrontendFactory<Udp>
    {
        public IFrontend Create(IReadOnlyDictionary<string, string> config)
        {
            var converter = new PropertySetToIPEndPointConverter();
            var endpoint = converter.Convert(config);
   
            return new Udp(endpoint);
        }
    }
}
