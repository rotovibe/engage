using System;
using System.Collections.Generic;
using System.Web.ModelBinding;
using Lucene.Net.Support;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Contact.DTO;
using ServiceStack.Service;

namespace Phytel.API.AppDomain.NG.Test
{
    [TestClass]
    public class NGManager_SearchContactsTests
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NGManager_SearchContacts_Empty_Request_Should_Throw()
        {
            var ngManager = new NGManager();
            ngManager.SearchContacts(null);

        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void NGManager_SearchContacts_Empty_ContactTypeIds_Or_EmptyStatuses_Should_Throw()
        {
            var ngManager = new NGManager();
            var stubRequest = new SearchContactsRequest
            {
                ContractNumber = "dummy1234",
                FirstName = "Abc",
                Take = 10,
                Skip = 0
            };

            ngManager.SearchContacts(stubRequest);

        }
    }
}
