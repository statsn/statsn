using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsN
{
    class Program
    {
        static void Main(string[] args)
        {

            Uri entryUri = new Uri(System.Reflection.Assembly.GetEntryAssembly().Location);
            var config = new Configuration.Configuration(new Uri(entryUri, "config.xml"));

            var host = new Host(config);


            var client = new System.Net.Sockets.UdpClient();

            Task.Delay(2000).Wait();

            var str =
@"a:10|ms
a:11|ms
a:20|ms
a:10|ms
a:15|ms
a:20|ms
b:10|ms
b:11|ms
b:11|ms
b:10|ms
";

            var bytes = Encoding.UTF8.GetBytes(str);
            client.Send(bytes, bytes.Count(), new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, 9871));

            client.Send(bytes, bytes.Count(), new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, 9871));


            System.Diagnostics.Debug.WriteLine("Initalization complete - blocking main thread");
            Task.WaitAll(Never());
        }

        static Task Never()
        {
            return new Task(() => { });
        }
    }
}
