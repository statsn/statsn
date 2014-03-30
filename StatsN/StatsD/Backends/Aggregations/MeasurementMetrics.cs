using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsN.StatsD.Backends.Aggregations
{
    class MeasurementMetrics
    {
        /*
            mean_90: 23,
            upper_90: 45,
            sum_90: 1035,
            std: 14.430869689661812,
            upper: 50,
            lower: 1,
            count: 50,
            count_ps: 5,
            sum: 1275,
            mean: 25.5,
            median: 25.5
            */

        public readonly double Upper, Lower, Sum, Mean, Median, P90, Std;
        public readonly long Count;

        public MeasurementMetrics(
            double upper = double.NegativeInfinity, 
            double lower = double.PositiveInfinity,
            double sum = 0,
            long count = 0,
            double mean = 0,
            double median = 0,
            double std = 0,
            double p90 = 0
            )
        {
            Count = count;

            Upper = upper;
            Lower = lower;
            
            P90 = p90;
            Mean = mean;
            Median = median;

            Sum = sum;
            Std = std;
        }
    }
}
