using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.Security;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using System;

namespace Phytel.API.AppDomain.NG.Test
{
    [TestClass]
    public class LookUps_Test
    {

        public static string _token;
        public static string _version = "v1";
        public static string _contractNumber = "InHealth001";
        public static string _product = "NG";

        //protected SecurityMongoContext _objectContext;

        //public LookUps_Test(SecurityMongoContext context)
        //{
        //    _objectContext = context;
        //}
        
        [ClassInitialize]
        public static void CreateTestToken()
        {
            SecurityMongoContext _objectContext = new SecurityMongoContext();
            
            MEAPISession session = new MEAPISession
            {
                APIKey = "12345",
                Product = _product,
                SessionLengthInMinutes = 480,
                SessionTimeOut = DateTime.Now.AddMinutes(480),
                UserName = "testUser"
            };

            _objectContext.APISessions.Collection.Insert(session);
        }

        [ClassCleanup]
        public static void DeleteTestToken()
        {
            SecurityMongoContext _objectContext = new SecurityMongoContext();
            
            IMongoQuery query = Query.And(
                            Query.EQ(MEAPISession.IdProperty, ObjectId.Parse(_token)),
                            Query.EQ(MEAPISession.ProductProperty, _product.ToUpper()));

            MEAPISession session = _objectContext.APISessions.Collection.FindOneById(ObjectId.Parse(_token));
            if (session != null)
            {
                _objectContext.APISessions.Collection.Remove(query);
            }
        }
        
        [TestMethod]
        public void GetAllProblems_Test()
        {
            // Arrange

            NGManager ngManager = new NGManager();
            GetAllProblemsRequest request = new GetAllProblemsRequest
            {
                ContractNumber = _contractNumber,
                Token = _token,
                Version = _version
            };
            // Act
            List<LookUp> response = ngManager.GetProblems(request);

            //Assert
            Assert.IsTrue(response.Count > 0);
        }

        #region LookUps - Contact related
        [TestMethod]
        public void GetAllCommModes_Test()
        {
            // Arrange
            NGManager ngManager = new NGManager();
            GetAllCommModesRequest request = new GetAllCommModesRequest
            {
                ContractNumber = _contractNumber,
                Token = _token,
                Version = _version
            };
            // Act
            List<LookUp> response = ngManager.GetAllCommModes(request);

            //Assert
            Assert.IsTrue(response.Count > 0);
        }
        #endregion
    }
}
