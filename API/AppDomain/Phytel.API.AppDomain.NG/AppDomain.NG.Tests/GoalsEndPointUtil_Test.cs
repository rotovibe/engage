using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Phytel.API.AppDomain.NG.Command;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.PatientGoal.DTO;
using System.Linq;

namespace Phytel.API.AppDomain.NG.Test
{
    [TestClass]
    public class GoalsEndPointUtil_Test
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetFilteredInterventions_Null_InterventionsData_Should_Throw()
        {
            GoalsEndpointUtil.GetFilteredInterventions(null, new GetInterventionsRequest());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetFilteredInterventions_Null_Request_Should_Throw()
        {
            GoalsEndpointUtil.GetFilteredInterventions(new List<PatientInterventionData>(), null);
        }

        [TestMethod]        
        public void GetFilteredInterventions_AssignedToOthers_Success()
        {
            var mockPatientInterventionDataList = new List<PatientInterventionData>();
            for (int i = 1; i<= 10; i++ )
            {
                mockPatientInterventionDataList.Add(new PatientInterventionData()
                {
                    CreatedById = "1",
                    AssignedToId = "1",
                    StatusId = 1
                });
            }            
            mockPatientInterventionDataList[0].CreatedById = "2";            
            mockPatientInterventionDataList[1].StatusId = 2;

            GetInterventionsRequest request = new GetInterventionsRequest()
            {
                UserId = "2",
                InterventionFilterType = InterventionFilterType.AssignedToOthers
            };

            Assert.IsTrue(GoalsEndpointUtil.GetFilteredInterventions(mockPatientInterventionDataList, request)
                .Count == 1);
        }

        [TestMethod]
        public void GetFilteredInterventions_ClosedByOthers_Success()
        {
            var mockPatientInterventionDataList = new List<PatientInterventionData>();
            for (int i = 1; i <= 10; i++)
            {
                mockPatientInterventionDataList.Add(new PatientInterventionData()
                {
                    CreatedById = "1",
                    AssignedToId = "1",
                    StatusId = 1
                });
            }
            mockPatientInterventionDataList[0].AssignedToId = "2";
            mockPatientInterventionDataList[0].StatusId = 2;

            mockPatientInterventionDataList[1].AssignedToId = "2";
            mockPatientInterventionDataList[1].StatusId = 2;

            GetInterventionsRequest request = new GetInterventionsRequest()
            {
                UserId = "1",
                InterventionFilterType = InterventionFilterType.ClosedByOthers
            };

            Assert.IsTrue(GoalsEndpointUtil.GetFilteredInterventions(mockPatientInterventionDataList, request)
                .Count == 2);
        }

        [TestMethod]
        public void GetFilteredInterventions_MyClosedList_Success()
        {
            var mockPatientInterventionDataList = new List<PatientInterventionData>();
            for (int i = 1; i <= 10; i++)
            {
                mockPatientInterventionDataList.Add(new PatientInterventionData()
                {
                    CreatedById = "1",
                    AssignedToId = "1",
                    StatusId = 2
                });
            }
            mockPatientInterventionDataList[0].AssignedToId = "2";
            mockPatientInterventionDataList[1].AssignedToId = "2";            

            GetInterventionsRequest request = new GetInterventionsRequest()
            {
                UserId = "1",
                InterventionFilterType = InterventionFilterType.MyClosedList
            };

            Assert.IsTrue(GoalsEndpointUtil.GetFilteredInterventions(mockPatientInterventionDataList, request)
                .Count == 8);
        }

        [TestMethod]
        public void GetFilteredInterventions_MyOpenList_Success()
        {
            var mockPatientInterventionDataList = new List<PatientInterventionData>();
            for (int i = 1; i <= 10; i++)
            {
                mockPatientInterventionDataList.Add(new PatientInterventionData()
                {
                    CreatedById = "1",
                    AssignedToId = "1",
                    StatusId = 1
                });
            }
            mockPatientInterventionDataList[0].AssignedToId = "2";
            mockPatientInterventionDataList[1].AssignedToId = "2";
            mockPatientInterventionDataList[2].StatusId = 2;

            GetInterventionsRequest request = new GetInterventionsRequest()
            {
                UserId = "1",
                InterventionFilterType = InterventionFilterType.MyOpenList
            };

            Assert.IsTrue(GoalsEndpointUtil.GetFilteredInterventions(mockPatientInterventionDataList, request)
                .Count == 7);
        }
    }
}
