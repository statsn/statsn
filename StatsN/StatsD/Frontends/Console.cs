
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsN.StatsD.Frontends
{
    class Console : StatsD
    {
        private bool Running = true;

        protected override void Listen(StatsDMessageParser parser)
        {
            System.Console.WriteLine("StatsD CLI");
            while (Running)
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

        public override void Terminate()
        {
            Running = false;
        }
    }
}
