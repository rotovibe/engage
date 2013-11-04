using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;

namespace Phytel.API.AppDomain.NG
{
    public static class NGManager
    {
        public static PatientResponse GetPatientByID(int patientID)
        {
            //Execute call(s) to Patient Data Domain

            return new DTO.PatientResponse { ID = patientID, FirstName = "Test", LastName = "Tester" };
        }
    }
}
