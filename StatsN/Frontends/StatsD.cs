using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using StatsN.Core;
using System.Reactive.Subjects;


namespace StatsN
{
    class StatsD : IFrontend
    {
        public IObservable<DescreteEvent> DescreteEvents { get; private set; }
        public IObservable<Measurement> Measures { get; private set; }

        public StatsD(IPEndPoint ep)
        {
            var client = new UdpClient(ep);
            System.Diagnostics.Debug.WriteLine("UdpListner created on {0}", ep);

            var descretes = new Subject<DescreteEvent>();
            DescreteEvents = descretes;
            var meausures = new Subject<Measurement>();
            Measures = meausures;
            var parser = new StatsDMessageParser(descretes, meausures);

            Listen(client, parser);
        }

        private async void Listen(UdpClient client, StatsDMessageParser parser)
        {
            try
            {
                while (true)
                {
                    var result = await client.ReceiveAsync();
                    parser.Parse(Encoding.UTF8.GetString(result.Buffer));
                }
            }
            catch (ObjectDisposedException)
            {
            }
            catch (SocketException)
            {
            }
        }
    }
}
