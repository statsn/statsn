
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
        public IObservable<DescreteEvent> DescreteEvents { get; private set; }
        public IObservable<Measurement> Measures { get; private set; }
        private StatsDMessageParser Parser;

        public StatsD()
        {
            var descretes = new Subject<DescreteEvent>();
            DescreteEvents = descretes;
            var meausures = new Subject<Measurement>();
            Measures = meausures;
            Parser = new StatsDMessageParser(descretes, meausures);
        }

        public void Run()
        {
            Listen(Parser);
        }

        protected abstract void Listen(StatsDMessageParser parser);
    }
}
