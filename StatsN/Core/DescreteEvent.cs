using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsN.Core
{
    public struct DescreteEvent
    {
        public readonly string Name;
        public readonly string Namespace;
        public readonly float Count;
        public readonly Object EntityTag;
        public readonly Object ActorTag;
        
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
