
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsN.StatsD.Frontends
{
    class Console : StatsD
    {
        protected override void Listen(StatsDMessageParser parser)
        {
            System.Console.WriteLine("StatsD CLI");
            while (true)
            {
                try
                {
                    var line = System.Console.ReadLine();
                    parser.Parse(line);
                }
                catch (IndexOutOfRangeException)
                {
                    

                }
            }
        }
    }
}
