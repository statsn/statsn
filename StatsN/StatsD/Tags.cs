using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsN.StatsD
{
    public static class Tags
    {

        public readonly static Object Counter = new object();
        public readonly static Object Timer = new object();
        public readonly static Object Gauge = new object();
        public readonly static Object Set = new object();
    }
}
