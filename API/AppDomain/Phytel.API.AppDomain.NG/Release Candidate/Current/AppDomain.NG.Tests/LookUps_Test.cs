using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.Security;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using System;
using Phytel.API.Common.CustomObjects;

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
            List<IdNamePair> response = ngManager.GetProblems(request);

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
            List<IdNamePair> response = ngManager.GetAllCommModes(request);

            //Assert
            Assert.IsTrue(response.Count > 0);
        }
        #endregion

        [TestMethod]
        public void GetLookUpByType()
        {
            // Arrange
            NGManager ngManager = new NGManager();
            GetLookUpsRequest request = new GetLookUpsRequest
            {
                ContractNumber = _contractNumber,
                Token = _token,
                Version = _version,
                TypeName = "intercategory"
            };
            // Act
            List<IdNamePair> response = ngManager.GetLookUps(request);

            //Assert
            Assert.IsTrue(response.Count > 0);
        }

        [TestMethod]
        public void GetCareManagers()
        {
            // Arrange
            NGManager ngManager = new NGManager();
            GetAllCareManagersRequest request = new GetAllCareManagersRequest
            {
                ContractNumber = _contractNumber,
                Token = _token,
                Version = _version,
            };
            // Act
            List<Contact> response = ngManager.GetCareManagers(request);

            //Assert
            Assert.IsTrue(response.Count > 0);
        }
        
    }
}
