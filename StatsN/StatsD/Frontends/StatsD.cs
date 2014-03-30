
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Subjects;
using StatsN.Core;

using StatsN.Frontends;

namespace StatsN.StatsD.Frontends
{
    abstract class StatsDFrontend : IFrontend
    {   
        public IObservable<Metric> Run()
        {
            var parser = new StatsDMessageParser();
            return parser.Parse(Listen());
        }

        protected abstract IObservable<string> Listen();
    }
}
