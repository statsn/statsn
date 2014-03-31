using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsN
{
    class Program
    {
        static int Main(string[] args)
        {

            Uri entryUri = new Uri(System.Reflection.Assembly.GetEntryAssembly().Location);



            Configuration.Configuration config;
            try
            {
                config = new Configuration.Configuration(new Uri(entryUri, "config.xml"));
            }
            catch (Configuration.ConfigurationException e)
            {
                Console.WriteLine(e.Message);
                return 1; 
            }


            var host = new Host(config);
            host.Start();

            System.Diagnostics.Debug.WriteLine("Initalization complete - blocking main thread");
            Task.WaitAll(Never());
            return 0;
        }

        static Task Never()
        {
            return new Task(() => { });
        }
    }
}
