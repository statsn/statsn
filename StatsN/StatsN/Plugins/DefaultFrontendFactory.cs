using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatsN.Frontends;

namespace StatsN
{
    class DefaultFrontendFactory : IFrontendFactory
    {
        Type Type { get; set; }
        public DefaultFrontendFactory(Type type)
        {
            Type = type;
        }

        public IFrontend Create(IReadOnlyDictionary<string, string> config)
        {
            return (IFrontend)Activator.CreateInstance(Type);
        }
    }
}
