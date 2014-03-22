using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsN
{
    class FrontendFactory : IFrontendFactory
    {
        Type Type { get; set; }
        public FrontendFactory(Type type)
        {
            Type = type;
        }

        public IFrontend Create(IReadOnlyDictionary<string, string> config)
        {
            return (IFrontend)Activator.CreateInstance(Type);
        }
    }
}
