using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StatsN.Extentions.Xml
{
    public static class Xml
    {
        public static IEnumerable<XElement> ElementsAnyNS(this XElement source, string localName)
        {
            return source.Elements().Where(e => e.Name.LocalName == localName);
        }
    }
}
