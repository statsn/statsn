﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatsN.Core;

namespace StatsN.Backends
{
    public interface IBackendFactory
    {
        IBackend Create(IReadOnlyDictionary<string, string> config, IObservable<DescreteEvent> descrete, IObservable<Measurement> measures);
    }

    public interface IBackendFactory<out TBackend> : IBackendFactory
        where TBackend : IBackend
    {

    }
}
