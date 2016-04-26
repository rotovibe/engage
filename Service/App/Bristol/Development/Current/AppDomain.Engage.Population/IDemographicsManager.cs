using AppDomain.Engage.Population.DTO.Context;
using AppDomain.Engage.Population.DTO.Demographics;
using AppDomain.Engage.Population.DTO.Referrals;
using System.Collections.Generic;


namespace AppDomain.Engage.Population
{
    public interface IDemographicsManager
    {
        UserContext UserContext { get; set; }
        

        PostReferralWithPatientsListResponse InsertBulkPatients(List<Patient> patientslist);

    }
}