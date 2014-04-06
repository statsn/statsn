using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Linq;
using StatsN.Backends;
using StatsN.Core;

namespace StatsN.StatsD.Backends
{
    class Proxy : IBackend
    {
        readonly UdpClient Client;
        readonly IPEndPoint Endpoint;

        private TimeSpan? BufferTimeout;
        private int? BufferCount;

        public Proxy(IPEndPoint endpoint)
        {
            Client = new UdpClient();
            
            Endpoint = endpoint;
            Client.Connect(Endpoint);
        }

        public Proxy Buffer(TimeSpan timespan)
        {
            BufferTimeout = timespan;
            return this;
        }

        public Proxy Buffer(int count)
        {
            BufferCount = count;
            return this;
        }

        public void Run(IObservable<Core.Metric> metrics, IObservable<Core.MetaMetric> meta)
        {
            ApplyBuffering(metrics)
                .Where(message => !String.IsNullOrWhiteSpace(message))
                .Subscribe(message =>
            {
                
                var bytes = Encoding.UTF8.GetBytes(message);
                Client.BeginSend(bytes, bytes.Length, (_) => { }, null);
               
            });
        }

        private IObservable<string> ApplyBuffering(IObservable<Metric> metrics)
        {

            if (BufferTimeout == null && BufferCount == null)
                return Serialize(metrics);

            if (BufferTimeout != null && BufferCount != null)
                return Serialize(metrics.Buffer(BufferTimeout.Value, BufferCount.Value));

            if (BufferTimeout != null)
                return Serialize(metrics.Buffer(BufferTimeout.Value));

            if (BufferCount != null)
                return Serialize(metrics.Buffer(BufferCount.Value));

            System.Diagnostics.Debug.Assert(false, "No conditions met");

            return null;
        }

        private IObservable<string> Serialize(IObservable<Metric> metrics)
        {
            return metrics.Select(Serialize);
        }

        private IObservable<string> Serialize(IObservable<IEnumerable<Metric>> metrics)
        {
            return metrics.Select(m => String.Join("\n", m.Select(Serialize)));
        }

        private string Serialize(Metric metric)
        {
            if (metric.NamespaceTag == Tags.Counter)
                return String.Format("{0}:{1}|c", metric.Name, metric.Count);

            if (metric.NamespaceTag == Tags.Timer)
                return String.Format("{0}:{1}|ms", metric.Name, metric.Value);

            if (metric.NamespaceTag == Tags.Gauge)
                return String.Format("{0}:{1}|g", metric.Name, metric.Value);

            if (metric.NamespaceTag == Tags.Set)
                return String.Format("{0}:{1}|s", metric.Name, metric.EntityTag);

            return null;
        }
    }
}
