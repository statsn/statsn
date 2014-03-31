using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsN.Configuration
{
    class ConfigurationException : Exception
    {
        public ConfigurationException()
            : base("Configuration is invalid")
        {
        }
    }
}
