using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using StatsN.Core;
using System.Reactive.Linq;


namespace StatsN.StatsD.Frontends
{
    class Udp : StatsD
    {
        private UdpClient Client;

        public Udp(IPEndPoint ep)
        {
            Client = new UdpClient(ep);
            System.Diagnostics.Debug.WriteLine("UdpListner created on {0}", ep);
        }

        public override void Terminate()
        {
            Client.Close();
        }

        protected override async void Listen(StatsDMessageParser parser)
        {
            
            try
            {
                while (true)    
                {
                    var result = await Client.ReceiveAsync();

                    var str = Encoding.UTF8.GetString(result.Buffer);
                    System.Console.WriteLine(str);
                    parser.Parse(str);
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
