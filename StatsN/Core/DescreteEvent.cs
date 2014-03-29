using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsN.Core
{
    public class DescreteEvent : IMetric
    {
        public string Name { get; private set;}
        public string Namespace { get; private set; }
        public Object EntityTag { get; private set; }
        public Object ActorTag { get; private set; }
        
        public readonly float Count;
        
        public DescreteEvent(
            string name, 
            string nspace = null,
            float count = 1, 
            object entityTag = null, 
            object actorTag = null
)
        {
            Name = name;
            Namespace = nspace;
            Count = count;
            EntityTag = entityTag;
            ActorTag = actorTag;
        }
    }
}
