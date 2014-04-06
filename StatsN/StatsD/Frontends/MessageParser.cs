using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatsN.Core;
using System.Reactive.Linq;

namespace StatsN.StatsD.Frontends
{
    class MessageParser
    {
        private const string Counter = "c";
        private const string Set = "s";
        private const string Timer = "ms";
        private const string Guage = "g";



        public IObservable<Metric> Parse(IObservable<string> input)
        {
            return input.SelectMany(message =>
                {
                    return Parse(message);
                });
        }

        private IEnumerable<Metric> Parse(string message)
        {
            return message
                .Split('\n')
                .Where(str => !String.IsNullOrWhiteSpace(str))
                .Select(DisectMessage);
        }

        private Metric DisectMessage(string msg)
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

            return EmitMessage(name, metric, type);
        }

        private Metric EmitMessage(string name, float value, string type)
        {
            Metric metric;
            switch (type)
            {
                case Counter:
                    metric = new Metric(name, namespaceTag: Tags.Counter, count: value);
                    break;
                case Set:
                    metric = new Metric(name, namespaceTag: Tags.Set, count: 1, entityTag: (int)value);
                    break;
                case Timer:
                    metric = new Metric(name, namespaceTag: Tags.Timer, value: value);
                    break;
                case Guage:
                    metric = new Metric(name, namespaceTag: Tags.Gauge, value: value);
                    break;
                default:
                    throw new Exception(String.Format("Unknown message type {0}", type));
            }

            return metric;
        }
    }
}
