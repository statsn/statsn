using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatsN.Core;

namespace StatsN.Backends
{
    public interface IBackend
    {
        IObservable<DescreteEvent> Descrete { set; }
        IObservable<Measurement> Measures {set;}
        void Run();
    }
}
