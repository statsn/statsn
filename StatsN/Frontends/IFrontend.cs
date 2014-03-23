﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatsN.Core;
namespace StatsN.Frontends
{
    public interface IFrontend
    {
        IObservable<DescreteEvent> DescreteEvents { get; }
        IObservable<Measurement> Measures { get; }
        void Run();
    }
}
