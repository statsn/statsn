using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsN
{
    class PluginUnloadableException : Exception
    {
        public PluginUnloadableException() : base() {}
        public PluginUnloadableException(string message):  base(message) {}
    }
}
