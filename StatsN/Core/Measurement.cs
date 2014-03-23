using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsN.Core
{
    public struct Measurement
    {
        public readonly string Name;
        public readonly string Namespace;
        public readonly float Value;
        public readonly Object EntityTag;
        public readonly Object ActorTag;

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
