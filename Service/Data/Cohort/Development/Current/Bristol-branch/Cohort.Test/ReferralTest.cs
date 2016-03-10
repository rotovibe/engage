using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Cohort.DTO;
using Phytel.API.DataDomain.Cohort.DTO.Context;
using Phytel.API.DataDomain.Cohort.DTO.Model;
using Phytel.API.DataDomain.Cohort.DTO.Referrals;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Cohort.Test
{
    [TestClass]
    public class ReferralTest
    {
        [TestMethod]
        public void InsertReferral_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            PostReferralDefinitionRequest request = new PostReferralDefinitionRequest
            {
                Referral = new ReferralData
                { 
                    CohortId = "528aa055d4332317acc50978",
                    Name = "Test Referral",
                    DataSource = "Test soruce",
                    Description = "Test Description"
                },
                Context = context,
                ContractNumber = contractNumber,
                Version = version
            };

            IReferralRepository<IDataDomainRequest> rep = new MongoReferralRepository<IDataDomainRequest>(contractNumber);

            IServiceContext scontext = new ServiceContext {Context = context, Version = version,UserId = "531f2df6072ef727c4d2a3c0", Contract = contractNumber};

            // Act
            IDataReferralManager rm = new DataReferralManager(scontext, rep);
            PostReferralDefinitionResponse response = rm.InsertReferral(request);

            // Assert
            Assert.IsNotNull(response.Version);

        }
    }
}
