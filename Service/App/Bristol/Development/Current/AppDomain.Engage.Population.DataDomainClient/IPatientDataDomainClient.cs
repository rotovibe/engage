using System.Collections.Generic;
using AppDomain.Engage.Population.DTO.Context;
using AppDomain.Engage.Population.DTO.Demographics;
using Phytel.API.DataDomain.Patient.DTO;

namespace AppDomain.Engage.Population.DataDomainClient
{
    public interface IPatientDataDomainClient
    {
        //string PostPatientDetails(PatientData patients);

        ProcessedPatientsList PostPatientsListDetails(List<Patient> patientDataList,UserContext userContext);

       
    }
}