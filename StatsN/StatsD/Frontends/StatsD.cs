
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
    abstract class StatsD : IFrontend
    {
        public IObservable<Metric> Events { get; private set; }
        private StatsDMessageParser Parser;

        public StatsD()
        {
            var events = new Subject<Metric>();
            Events = events;
            
            Parser = new StatsDMessageParser(events);
        }

        public void Run()
        {
            Listen(Parser);
        }

        public abstract void Terminate();
        protected abstract void Listen(StatsDMessageParser parser);
    }
}
