using AutoMapper;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Search;
using Phytel.API.AppDomain.NG.Search;
using Phytel.API.AppDomain.NG.Search.LuceneStrategy;
using Phytel.API.Common.CustomObject;
using Phytel.API.DataDomain.Allergy.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using Phytel.API.Common;
using Phytel.API.DataDomain.Medication.DTO;

namespace Phytel.API.AppDomain.NG.Medication
{
    public class MedicationManager : ManagerBase, IMedicationManager
    {
        public IMedicationEndpointUtil EndpointUtil { get; set; }
        public ISearchManager SearchManager { get; set; }

        #region PatientMedSupp - Posts
        public List<PatientMedSupp> GetPatientMedSupps(GetPatientMedSuppsRequest request)
        {
            List<PatientMedSupp> patientMedSupps = null;
            try
            {
                List<PatientMedSuppData> data = EndpointUtil.GetPatientMedSupps(request);
                if (data != null && data.Count > 0)
                {
                    patientMedSupps = new List<PatientMedSupp>();
                    data.ForEach(a => patientMedSupps.Add(Mapper.Map<PatientMedSupp>(a)));
                }
                return patientMedSupps;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetPatientMedSupps()::" + ex.Message, ex.InnerException);
            }
        }

        public PatientMedSupp SavePatientMedSupp(PostPatientMedSuppRequest request)
        {
            PatientMedSupp patientMedSupp = null;
            try
            {
                if (request.PatientMedSupp != null)
                {
                    // Populate calculated NDC codes and Pharm classes in the request object before save.
                    bool calculateNDCAndPharm = false;
                    if (request.Insert)
                    {
                        calculateNDCAndPharm = true;
                    }
                    else
                    {
                        // On update, check for ReCalculateNDC flag.
                        if (request.RecalculateNDC)
                        {
                            calculateNDCAndPharm = true;
                        }
                    }
                    if (calculateNDCAndPharm)
                    {
                        if(!string.IsNullOrEmpty(request.PatientMedSupp.Name) && !string.IsNullOrEmpty(request.PatientMedSupp.Form) && !string.IsNullOrEmpty(request.PatientMedSupp.Strength) && !string.IsNullOrEmpty(request.PatientMedSupp.Route))
                        {
                            PatientMedSupp pms = EndpointUtil.GetMedicationDetails(request);
                            if (pms != null)
                            {
                                request.PatientMedSupp.NDCs = pms.NDCs;
                                request.PatientMedSupp.PharmClasses = pms.PharmClasses;
                            }
                        }
                    }
                    PatientMedSuppData data = EndpointUtil.SavePatientMedSupp(request);
                    if (data != null)
                    {
                        patientMedSupp = Mapper.Map<PatientMedSupp>(data);
                    }
                }
                return patientMedSupp;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:SavePatientMedSupp()::" + ex.Message, ex.InnerException);
            }
        }
        #endregion
    }
}
