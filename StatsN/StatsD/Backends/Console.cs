using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using StatsN.Backends;


namespace StatsN.StatsD.Backends
{
    class Console : IBackend
    {
        public IObservable<Core.DescreteEvent> Descrete
        {
            get; set;
        }

        public IObservable<Core.Measurement> Measures
        {
            get;
            set;
        }

        public void Run()
        {
            System.Console.WriteLine("Console backend running");
            Count();
        }

        private void Count()
        {
            Descrete.Buffer(TimeSpan.FromSeconds(10))
                    .Subscribe(buffered =>
                    {
                        var grouped = buffered.GroupBy(e => e.Name);
                        foreach (var grouping in grouped)
                        {
                            System.Console.WriteLine("{0} {1}", grouping.Key, grouping.Sum(e => e.Count));
                        }
                    });
        }
    }
}
