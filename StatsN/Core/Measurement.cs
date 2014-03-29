using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsN.Core
{
    public class Measurement : IMetric
    {
        public string Name { get; private set; }
        public string Namespace { get; private set; }
        public Object EntityTag { get; private set; }
        public Object ActorTag { get; private set; }

        public readonly float Value;
        
        public Measurement(
            string name,
            string nspace = null,
            float value = 1,
            object entityTag = null,
            object actorTag = null
        )
        {
            Name = name;
            Namespace = nspace;
            Value = value;
            EntityTag = entityTag;
            ActorTag = actorTag;
        }
    }
}
