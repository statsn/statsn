using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsN.Core
{
    public interface IMetric
    {
        string Name { get; }
        string Namespace { get; }
        Object EntityTag { get; }
        Object ActorTag { get; }

    }
}
