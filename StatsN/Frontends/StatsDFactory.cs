
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsN
{
    class StatsDFactory : IFrontendFactory<StatsD>
    {
        public IFrontend Create(IReadOnlyDictionary<string, string> config)
        {
            return new StatsD();
        }
    }
}
