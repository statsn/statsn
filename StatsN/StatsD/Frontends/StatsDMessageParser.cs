using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatsN.Core;

namespace StatsN.StatsD.Frontends
{
    class StatsDMessageParser
    {
        private IObserver<Metric> Events { get; set; }

        public StatsDMessageParser(IObserver<Metric> events)
        {
            Events = events;
        }

        public void Parse(string message)
        {
            var components = message.Split('\n');

            foreach (var msg in components.Where(str => !String.IsNullOrWhiteSpace(str)))
            {
                DisectMessage(msg.Trim());
            }
        }

        private void DisectMessage(string msg)
        {
            var comp = msg.Split(':');
            var name = comp[0];

            comp = comp[1].Split('|');


            var metric = float.Parse(comp[0]);
            var type = comp[1];
            if (comp.Length == 3)
            {
                metric /= float.Parse(comp[2].TrimStart('@'));
            }

            EmitMessage(name, metric, type);
        }

        private void EmitMessage(string name, float metric, string type)
        {
            Metric evnt;
            switch (type)
            {
                case "c":
                    evnt = new Metric(name, nspace: type, count: metric);
                    break;
                case "s":
                    evnt = new Metric(name, nspace: type, count: 1, entityTag: (int)metric);
                    break;
                case "ms":
                case "g":
                    evnt = new Metric(name, nspace: type, value: metric);
                    break;
                default:
                    throw new Exception(String.Format("Unknown message type {0}", type));
            }

            Events.OnNext(evnt);
        }
    }
}
