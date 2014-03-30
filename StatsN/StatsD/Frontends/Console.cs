
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Disposables;

using StatsN.Core;

namespace StatsN.StatsD.Frontends
{
    class Console : StatsDFrontend
    {
        private bool Running = true;

        protected override IObservable<string> Listen()
        {
            var inputs = Observable.Create<string>(observer =>
                {
                    var stream = System.Console.OpenStandardInput();
                    var reader = new StreamReader(stream);

                    EmitString(reader, observer);
                    
                    return Disposable.Create(() =>
                        {
                            reader.Dispose();
                            stream.Dispose();
                        });
                });

            return inputs;
        }

        private void EmitString(StreamReader reader, IObserver<String> obserer)
        {
            var read = reader.ReadLineAsync();
            read.ContinueWith(result =>
                {
                    EmitString(reader, obserer);
                    obserer.OnNext(result.Result);
                }
            );
        }
    }
}
