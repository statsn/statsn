using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StatsN.Core;
using StatsN;
using StatsN.StatsD.Frontends;

namespace FrontendTests
{
    [TestClass]
    public class StatsDMesageParserTests
    {
        IList<Metric> Metrics;

        MessageParser Parser;


        [TestInitialize]
        public void Before()
        {
            Metrics = new List<Metric>();

            var observer = MockObserverToCollection(Metrics);

            Parser = new MessageParser(observer);
        }

        private IObserver<T> MockObserverToCollection<T> (ICollection<T> collection){
            var mock = new Mock<IObserver<T>>(MockBehavior.Strict);

            mock.Setup(o => o.OnNext(It.IsAny<T>()))
                    .Callback<T>(collection.Add);

            return mock.Object;
        }

        [TestMethod]
        public void CounterMessage()
        {
            var message = "foo.bar:1|c";
            Parser.Parse(message);

            Assert.AreEqual(Metrics.Count, 1);

            var descrete = Metrics[0];
            Assert.AreEqual(descrete.Name, "foo.bar");
            Assert.AreEqual(descrete.Namespace, "c");
            Assert.AreEqual(descrete.Count, 1, 1e-10);
        }

        [TestMethod]
        public void CounterMessageWithSample()
        {
            var message = "foo.bar:1|c|@0.1";
            Parser.Parse(message);


            Assert.AreEqual(Metrics.Count, 1);
            var descrete = Metrics[0];
            Assert.AreEqual(descrete.Name, "foo.bar");
            Assert.AreEqual(descrete.Namespace, "c");
            Assert.AreEqual(descrete.Count, 10, 1e-10);
        }

        [TestMethod]
        public void SetMessage()
        {
            var message = "foo.bar:123|s";
            Parser.Parse(message);

            Assert.AreEqual(Metrics.Count, 1);
            var descrete = Metrics[0];
            Assert.AreEqual(descrete.Name, "foo.bar");
            Assert.AreEqual(descrete.Namespace, "s");
            Assert.AreEqual(descrete.Count, 1, 1e-10);
            Assert.AreEqual(descrete.EntityTag, 123);
        }

        [TestMethod]
        public void GuageMessage()
        {
            var message = "foo.bar:521|g";

            Parser.Parse(message);

            Assert.AreEqual(Metrics.Count, 1);
            

            var measure = Metrics[0];
            Assert.AreEqual(measure.Name, "foo.bar");
            Assert.AreEqual(measure.Namespace, "g");
            Assert.AreEqual(measure.Value, 521, 1e-10);
        }

        [TestMethod]
        public void TimerMessage()
        {
            var message = "foo.bar:521|ms";

            Parser.Parse(message);

            Assert.AreEqual(Metrics.Count, 1);

            var measure = Metrics[0];
            Assert.AreEqual(measure.Name, "foo.bar");
            Assert.AreEqual(measure.Namespace, "ms");
            Assert.AreEqual(measure.Value, 521, 1e-10);
        }

        [TestMethod]
        public void MultipleMessages()
        {
            var message = "foo.bar:521|ms\nbaz:1|c\nbob:4|g";

            Parser.Parse(message);

            Assert.AreEqual(Metrics.Count, 3);
        }
    }
}
