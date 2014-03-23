using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatsN.Core;

namespace StatsN
{
    class StatsDMessageParser
    {
        private IObserver<DescreteEvent> Descretes {get; set;}
        private IObserver<Measurement> Measures { get; set; }

        public StatsDMessageParser(IObserver<DescreteEvent> descretes, IObserver<Measurement> measures){
            Descretes = descretes;
            Measures = measures;
        }

        public void Parse(string message)
        {
            var components = message.Split('\n');

            foreach (var msg in components)
            {
                DisectMessage(msg);
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
            switch (type)
            {
                case "c":
                case "s":
                    var destrete = new DescreteEvent(name, nspace: type, count: metric);
                    Descretes.OnNext(destrete);
                    break;
                case "ms":
                case "g":
                    var measure = new Measurement(name, nspace: type, value: metric);
                    Measures.OnNext(measure);
                    break;
                default:
                    throw new Exception(String.Format("Unknown message type {0}", type));
            }
        }
    }
}
