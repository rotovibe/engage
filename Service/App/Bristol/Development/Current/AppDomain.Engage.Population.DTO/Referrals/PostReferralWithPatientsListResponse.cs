using System;
using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

using AppDomain.Engage.Population.DTO.Demographics;

namespace AppDomain.Engage.Population.DTO.Referrals
{
    public class PostReferralWithPatientsListResponse : IDomainResponse
    {
        public string ReferralId { get; set; }
        public ProcessedPatientsList ProcessedPatients { get; set; }
        public int SuccessCount { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
