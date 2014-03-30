using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsN.Core
{
    public struct Metric
    {
        public readonly string Name;
        public readonly Object EntityTag;
        public readonly Object ActorTag;
        public readonly Object NamespaceTag;
        public readonly Double Value;
        public readonly float Count;
        
        public Metric(
            string name, 
            float count = 1,
            Double value = Double.NaN,
            object entityTag = null, 
            object actorTag = null,
            object namespaceTag  = null
        )
        {
            Name = name;
            NamespaceTag = namespaceTag;
            Count = count;
            Value = value;
            EntityTag = entityTag;
            ActorTag = actorTag;
        }
    }
}
