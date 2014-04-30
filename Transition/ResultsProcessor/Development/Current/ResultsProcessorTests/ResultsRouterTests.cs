using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using ResultsProcessor;
using Phytel.Framework.ASE.Bus.Data;

namespace ResultsProcessorTests
{
    [TestFixture]
    public class ResultsRouterTests
    {
        #region Setup

        public ResultsQueueRouter router;

        [SetUp]
        public void Setup()
        {
            router = new ResultsQueueRouter();
        }

        #endregion

        #region Tests

        [Test]
        public void AllSuccessful_AllSuccess_ReturnsTrue()
        {
            Tuple<List<SubscriberQueue>, List<PublishResult>> allTrue = CreateAllSuccessTuple();
            bool result = router.AllSuccessful(allTrue);

            Assert.IsTrue(result);
         }

        [Test]
        public void AllSuccessful_NotAllSuccess_ReturnsFalse()
        {
            Tuple<List<SubscriberQueue>, List<PublishResult>> notAllTrue = CreateNotAllSuccessTuple();
            bool result = router.AllSuccessful(notAllTrue);

            Assert.IsFalse(result);
        }

        #endregion

        #region Private Helper Methods

        private Tuple<List<SubscriberQueue>, List<PublishResult>> CreateAllSuccessTuple()
        {
            SubscriberQueue subQueue = new SubscriberQueue();
            PublishResult pubResult = new PublishResult();
            pubResult.Type = Phytel.Framework.ASE.Bus.Common.ResultType.Debug;

            List<SubscriberQueue> item1 = new List<SubscriberQueue>();
            item1.Add(subQueue);
            List<PublishResult> item2 = new List<PublishResult>();
            item2.Add(pubResult);

            return new Tuple<List<SubscriberQueue>, List<PublishResult>>(item1, item2);
        }

        private Tuple<List<SubscriberQueue>, List<PublishResult>> CreateNotAllSuccessTuple()
        {
            SubscriberQueue subQueue = new SubscriberQueue();
            PublishResult pubResult = new PublishResult();
            pubResult.Type = Phytel.Framework.ASE.Bus.Common.ResultType.Error;

            List<SubscriberQueue> item1 = new List<SubscriberQueue>();
            item1.Add(subQueue);
            List<PublishResult> item2 = new List<PublishResult>();
            item2.Add(pubResult);

            return new Tuple<List<SubscriberQueue>, List<PublishResult>>(item1, item2);
        }

        #endregion
    }
}
