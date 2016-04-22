using System;
using System.Collections.Generic;
using AppDomain.Engage.Population.DTO.Context;
using AppDomain.Engage.Population.DTO.Referrals;
using Phytel.API.DataDomain.Patient.DTO;

namespace AppDomain.Engage.Population.DataDomainClient
{
    public interface ICohortDataDomainClient
    {
        string PostPatientReferralDefinition(string patientId, string referralId, UserContext userContext);

        PostReferralDefinitionResponse PostReferralDefinition(ReferralDefinitionData referral, UserContext userContext);
    }
}
