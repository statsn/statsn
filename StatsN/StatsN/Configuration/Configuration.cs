using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;

using StatsN.Extentions.Xml;

namespace StatsN.Configuration
{
    class Configuration
    {
        public IEnumerable<PluginConfig> Frontends { get; private set; }
        public IEnumerable<PluginConfig> Backends { get; private set; }

        public Configuration(Uri path)
        {
            LoadFromXml(path);
        }

        private void LoadFromXml(Uri path)
        {
          
            var configDocument = XDocument.Load(path.AbsolutePath);

            Validate(configDocument);

            var config = configDocument.Elements().First(_ => _.Name.LocalName == "config");

            Frontends = config.ElementsAnyNS("frontend")
                .Select(CreatePluginConfigFromNode)
                .ToList()
                .AsReadOnly();

            Backends = config.ElementsAnyNS("backend")
                .Select(CreatePluginConfigFromNode)
                .ToList()
                .AsReadOnly();

            return;
        }

        private static void Validate(XDocument doc)
        {
            var schemas = new XmlSchemaSet();
            schemas.Add("http://tempuri.org/configSchema.xsd", "configSchema.xsd");

            var errors = false;
            doc.Validate(schemas, (o, e) =>
            {
                Console.WriteLine(e.Message);
                errors = true;
            });

            if (errors)
            {
                throw new ConfigurationException();
            }
        }

        private PluginConfig CreatePluginConfigFromNode(XElement node)
        {
            var name = (String)node.Attribute("name");
            var enabled = (Boolean?)node.Attribute("enabled") ?? true;
            var path = (String)node.Attribute("path");
            var properties = node.ElementsAnyNS("properties")
                .SelectMany(el => el.ElementsAnyNS("property"))
                .ToDictionary(el => (String)el.Attribute("name"), el => (string)el.Attribute("value"));

            return new PluginConfig(name, enabled, path, properties);
        }
    }
}
