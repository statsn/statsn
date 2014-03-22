using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsN.Configuration
{
    class PluginConfig
    {
        public String Name { get; private set; }
        public Boolean Enabled { get; private set; }
        public string Path { get; private set; }

        public IReadOnlyDictionary<string, string> Properties;

        public PluginConfig(string name, bool enabled, string path, IReadOnlyDictionary<string, string> properties)
        {
            Name = name;
            Enabled = enabled;
            Path = path;
            Properties = properties;
        }
    }
}
