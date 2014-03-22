using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsN
{
    class Host
    {
        public Host(Configuration.Configuration config)
        {
            var loader = new FrontendLoader(config.Frontends);
        }
    }
}
