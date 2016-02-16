using System.Configuration;
using AutoMapper;
using Phytel.API.AppDomain.NG.DTO.Utilization;
using Phytel.API.Common;
using Phytel.API.DataDomain.PatientNote.DTO.Response.Utilization;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Linq;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.PatientNote.DTO;

namespace Phytel.API.AppDomain.NG.Notes
{
    public class UtilizationManager : ManagerBase, IUtilizationManager
    {
        #region endpoint addresses
        protected static readonly string DdPatientNoteUrl = ConfigurationManager.AppSettings["DDPatientNoteUrl"];
        #endregion

        public GetPatientUtilizationResponse GetPatientUtilization(GetPatientUtilizationRequest request)
        {
            try
            {
                var result = new GetPatientUtilizationResponse();

                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Notes/Utilization/{Id}", "GET")]
                IRestClient client = new JsonServiceClient();
                var url = Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Notes/Utilizations/{5}",
                                                        DdPatientNoteUrl,
                                                        "NG",
                                                        request.Version,
                                                        request.ContractNumber,
                                                        request.PatientId,
                                                        request.Id), request.UserId);

                GetPatientUtilizationDataResponse ddResponse = client.Get<GetPatientUtilizationDataResponse>(url);

                if (ddResponse != null && ddResponse.Utilization != null)
                {
                    result.Utilization = Mapper.Map<PatientUtilization>(ddResponse.Utilization);
                }

                return result;
            }
            catch (WebServiceException ex) { throw ex; }
        }

        public DeletePatientUtilizationResponse DeletePatientUtilization(DeletePatientUtilizationRequest request)
        {
            try
            {
                var result = new DeletePatientUtilizationResponse();

                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Notes/Utilization/{Id}", "GET")]
                IRestClient client = new JsonServiceClient();
                var url = Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Notes/Utilizations/{5}",
                                                        DdPatientNoteUrl,
                                                        "NG",
                                                        request.Version,
                                                        request.ContractNumber,
                                                        request.PatientId,
                                                        request.Id), request.UserId);

                DeletePatientUtilizationDataResponse ddResponse = client.Delete<DeletePatientUtilizationDataResponse>(url);

                if (ddResponse != null)
                {
                    result.Success = true;
                }

                return result;
            }
            catch (WebServiceException ex) { throw ex; }
        }

        public GetPatientUtilizationsResponse GetPatientUtilizations(GetPatientUtilizationsRequest request)
        {
            try
            {
                var result = new GetPatientUtilizationsResponse();

                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Notes/", "GET")]
                IRestClient client = new JsonServiceClient();
                var url = Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Notes/Utilizations/",
                    DdPatientNoteUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId), request.UserId);

                GetAllPatientUtilizationDataResponse ddResponse = client.Get<GetAllPatientUtilizationDataResponse>(url);

                if (ddResponse == null || ddResponse.Utilizations == null) return result;
                var ulist = ddResponse.Utilizations.Select(Mapper.Map<PatientUtilization>).ToList();

                result.Utilizations = request.Count > 0 ? ulist.Take(request.Count).ToList() : ulist;

                return result;
            }
            catch (WebServiceException ex)
            {
                throw ex;
            }
        }

        public PostPatientUtilizationResponse InsertPatientUtilization(PostPatientUtilizationRequest request)
        {
            try
            {
                var result = new PostPatientUtilizationResponse();

                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Notes/", "GET")]
                IRestClient client = new JsonServiceClient();
                var url = Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Notes/Utilizations/",
                    DdPatientNoteUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId), request.UserId);

                var ddRequest = new PostPatientUtilizationDataRequest
                {
                    Context = request.ContractNumber,
                    ContractNumber = request.ContractNumber,
                    PatientId = request.PatientId,
                    PatientUtilization = Mapper.Map<PatientUtilizationData>(request.Utilization),
                    UserId = request.UserId,
                    Version = request.Version
                };

                PostPatientUtilizationDataResponse ddResponse = client.Post<PostPatientUtilizationDataResponse>(url,
                    ddRequest);

                if (ddResponse == null || ddResponse.Utilization == null) return result;
                result.Utilization = Mapper.Map<PatientUtilization>(ddResponse.Utilization);
                result.Result = true;

                return result;
            }
            catch (WebServiceException ex)
            {
                throw ex;
            }
        }

        public PutPatientUtilizationResponse UpdatePatientUtilization(PutPatientUtilizationRequest request)
        {
            try
            {
                var result = new PutPatientUtilizationResponse();

                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Notes/", "GET")]
                IRestClient client = new JsonServiceClient();
                var url = Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Notes/Utilizations/{5}",
                    DdPatientNoteUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.UtilizationId), request.UserId);

                var ddRequest = new PutPatientUtilizationDataRequest
                {
                    Context = request.ContractNumber,
                    ContractNumber = request.ContractNumber,
                    PatientId = request.PatientId,
                    PatientUtilization = Mapper.Map<PatientUtilizationData>(request.Utilization),
                    UserId = request.UserId,
                    Version = request.Version
                };

                PutPatientUtilizationDataResponse ddResponse = client.Put<PutPatientUtilizationDataResponse>(url,
                    ddRequest);

                if (ddResponse == null || ddResponse.Status != null) return result;
                result.Utilization = Mapper.Map<PatientUtilization>(ddResponse.Utilization);
                result.Result = true;

                return result;
            }
            catch (WebServiceException ex)
            {
                throw ex;
            }
        }
    }
}
