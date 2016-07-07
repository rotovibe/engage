using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;



namespace Phytel.API.AppDomain.NG.Test.Contact
{
    [TestClass]
    public class CohortsRulesProcessorTests
    {       
        
        [TestInitialize]
        public void SetUp()
        {           
        }

        [TestMethod]
        public void CohortsRulesProcessor_StartQueueProcessorThread_Success()
        {
            //Arrange
            var mockCareMemberCohortRuleFactory = new Mock<ICareMemberCohortRuleFactory>();
            var mockContactDataController = new Mock<IContactEndpointUtil>();
            var mockLogger = new Mock<ILogger>();
            var mockCohortRuleUtil = new Mock<ICohortRuleUtil>();
            
            var cohortRulesProcessor = new CohortRulesProcessor(mockCareMemberCohortRuleFactory.Object,mockContactDataController.Object,mockCohortRuleUtil.Object,mockLogger.Object );

            //Act
            cohortRulesProcessor.Start();

            //Assert
            Assert.IsTrue(cohortRulesProcessor.QueueProcessorRunning);   
            cohortRulesProcessor.Stop();         
        }

        [TestMethod]
        public void CohortsRulesProcessor_StopQueueProcessorThread_Success()
        {
            //Arrange
            var mockCareMemberCohortRuleFactory = new Mock<ICareMemberCohortRuleFactory>();
            var mockContactDataController = new Mock<IContactEndpointUtil>();
            var mockLogger = new Mock<ILogger>();
            var mockCohortRuleUtil = new Mock<ICohortRuleUtil>();
            var cohortRulesProcessor = new CohortRulesProcessor(mockCareMemberCohortRuleFactory.Object, mockContactDataController.Object, mockCohortRuleUtil.Object, mockLogger.Object);
            cohortRulesProcessor.Start();
            
            //Act
            cohortRulesProcessor.Stop();
            Thread.Sleep(200);
            
            //Assert
            Assert.IsTrue(!cohortRulesProcessor.QueueProcessorRunning);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void CohortsRulesProcessor_EnqueueCohortRuleWhenProcessingThreadNotRunning_ShouldThrow()
        {
            //Arrange
            var mockCareMemberCohortRuleFactory = new Mock<ICareMemberCohortRuleFactory>();
            var mockContactDataController = new Mock<IContactEndpointUtil>();
            var mockLogger = new Mock<ILogger>();
            var mockCohortRuleUtil = new Mock<ICohortRuleUtil>();
            var cohortRulesProcessor = new CohortRulesProcessor(mockCareMemberCohortRuleFactory.Object, mockContactDataController.Object, mockCohortRuleUtil.Object, mockLogger.Object);

 
            //Assert
            cohortRulesProcessor.EnqueueCohorRuleCheck(new CohortRuleCheckData());

        }

        [TestMethod]
        public void CohortsRulesProcessor_EnqueueCohortRule_Success()
        {
            //Arrange
            var mockCareMemberCohortRuleFactory = new Mock<ICareMemberCohortRuleFactory>();
            var mockContactDataController = new Mock<IContactEndpointUtil>();
            var mockLogger = new Mock<ILogger>();
            var mockCohortRuleUtil = new Mock<ICohortRuleUtil>();
            var cohortRulesProcessor = new CohortRulesProcessor(mockCareMemberCohortRuleFactory.Object, mockContactDataController.Object, mockCohortRuleUtil.Object, mockLogger.Object);
            cohortRulesProcessor.Start();
            //Act
            for (int i = 0; i < 20; i++)
            {
                cohortRulesProcessor.EnqueueCohorRuleCheck(new CohortRuleCheckData());
            }
            
            //Assert
            Assert.IsTrue(cohortRulesProcessor.GetQueueCount()>0);
            cohortRulesProcessor.Stop();
           
        }

        [TestMethod]
        public void CohortsRulesProcessor_DequeueCohortRule_Success()
        {
            //Arrange
            var mockCareMemberCohortRuleFactory = new Mock<ICareMemberCohortRuleFactory>();
            var mockContactDataController = new Mock<IContactEndpointUtil>();
            var mockLogger = new Mock<ILogger>();
            var mockCohortRuleUtil = new Mock<ICohortRuleUtil>();
            var cohortRulesProcessor = new CohortRulesProcessor(mockCareMemberCohortRuleFactory.Object, mockContactDataController.Object, mockCohortRuleUtil.Object, mockLogger.Object);
            cohortRulesProcessor.Start();
            for (int i = 0; i <= 20; i++)
            {
                cohortRulesProcessor.EnqueueCohorRuleCheck(new CohortRuleCheckData());
            }
            //Act
            Thread.Sleep(200);

            //Assert
            Assert.IsTrue(cohortRulesProcessor.GetQueueCount() < 20);
            cohortRulesProcessor.Stop();

        }
    }
}
