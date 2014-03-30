using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using StatsN.Core;
using System.Reactive.Linq;
using System.Reactive.Disposables;


namespace StatsN.StatsD.Frontends
{
    class Udp : StatsDFrontend
    {
        private UdpClient Client;

        public Udp(IPEndPoint ep)
        {
            Client = new UdpClient(ep);
            System.Diagnostics.Debug.WriteLine("UdpListner created on {0}", ep);
        }

        protected override IObservable<string> Listen()
        {
            var inputs = Observable.Create<string>(observer =>
            {
                EmitString(observer);

                return Disposable.Create(Client.Close);
            });

            return inputs;
        }

        private void EmitString(IObserver<String> observer)
        {
            var read = Client.ReceiveAsync();
            read.ContinueWith(result =>
            {
                EmitString(observer);

                var bytes = result.Result.Buffer;
                var str = Encoding.UTF8.GetString(bytes);

                observer.OnNext(str);
            });
        }
    }
}
