using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsN
{
    public interface IFrontendFactory 
    {
        IFrontend Create(IReadOnlyDictionary<string, string> config);
    }

    public interface IFrontendFactory<out TFrontend> : IFrontendFactory
        where TFrontend : IFrontend 
    {
     
    }
}
