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
            Task.WaitAll(Never());
        }

        static Task Never()
        {
            return new Task(() => { });
        }
    }
}
