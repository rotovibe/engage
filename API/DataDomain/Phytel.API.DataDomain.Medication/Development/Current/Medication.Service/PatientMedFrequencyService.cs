using System;
using System.Collections.Generic;
using Phytel.API.DataDomain.Medication.DTO;

namespace Phytel.API.DataDomain.Medication.Service
{
    public class PatientMedFrequencyService : ServiceBase
    {
        public IPatientMedFrequencyDataManager Manager { get; set; }

        public GetPatientMedFrequenciesDataResponse Get(GetPatientMedFrequenciesDataRequest request)
        {
            var response = new GetPatientMedFrequenciesDataResponse { Version = request.Version };
            try
            {
                RequireUserId(request);
                response.PatientMedFrequenciesData = Manager.GetPatientMedFrequencies(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public PostPatientMedFrequencyDataResponse Post(PostPatientMedFrequencyDataRequest request)
        {
            var response = new PostPatientMedFrequencyDataResponse { Version = request.Version };
            try
            {
                RequireUserId(request);
                response.Id = Manager.InsertPatientMedFrequency(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }
    }
}