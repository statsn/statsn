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
        public readonly string Namespace;
        public readonly Object EntityTag;
        public readonly Object ActorTag;
        public readonly Double Value;
        
        public readonly float Count;
        
        public Metric(
            string name, 
            string nspace = null,
            float count = 1,
            Double value = Double.NaN,
            object entityTag = null, 
            object actorTag = null
        )
        {
            Name = name;
            Namespace = nspace;
            Count = count;
            Value = value;
            EntityTag = entityTag;
            ActorTag = actorTag;
        }
    }
}
