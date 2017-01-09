using System;
using System.Configuration;
using Phytel.API.Common;
using Phytel.API.Common.Format;
using Phytel.API.DataDomain.Patient.DTO;

namespace Phytel.API.DataDomain.Patient.Service
{
    public class PatientService : ServiceStack.ServiceInterface.Service
    {
        public ICommonFormatterUtil CommonFormatterUtil { get; set; }
        public IHelpers Helpers { get; set; }
        public IPatientDataManager PatientManager { get; set; }

        public GetPatientDataResponse Get(GetPatientDataRequest request)
        {
            GetPatientDataResponse response = new GetPatientDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientDD:Get()::Unauthorized Access");

                response = PatientManager.GetPatientByID(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetPatientDataResponse Get(GetPatientDataByNameDOBRequest request)
        {
            GetPatientDataResponse response = new GetPatientDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientDD:Get()::Unauthorized Access");

                response = PatientManager.GetPatientDataByNameDOB(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetPatientSSNDataResponse Get(GetPatientSSNDataRequest request)
        {
            GetPatientSSNDataResponse response = new GetPatientSSNDataResponse();
            try
            {

                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientDD:Get()::Unauthorized Access");

                response = PatientManager.GetPatientSSN(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetPatientsDataResponse Post(GetPatientsDataRequest request)
        {
            GetPatientsDataResponse response = new GetPatientsDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientDD:Post()::Unauthorized Access");

                response = PatientManager.GetPatients(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetAllPatientsDataResponse Get(GetAllPatientsDataRequest request)
        {
            GetAllPatientsDataResponse response = new GetAllPatientsDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientDD:Get()::Unauthorized Access");

                response.PatientsData = PatientManager.GetAllPatients(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutUpdatePatientDataResponse Put(PutUpdatePatientDataRequest request)
        {
            PutUpdatePatientDataResponse response = new PutUpdatePatientDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientDD:Put()::Unauthorized Access");

                response = PatientManager.UpdatePatient(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutPatientDataResponse Put(PutPatientDataRequest request)
        {
            PutPatientDataResponse response = new PutPatientDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientDD:Put()::Unauthorized Access");

                response = PatientManager.InsertPatient(request);
                response.Version = request.Version;
                //throw new Exception("Just a test error");
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            
            return response;
        }

        public PutCohortPatientViewDataResponse Put(PutCohortPatientViewDataRequest request)
        {
            PutCohortPatientViewDataResponse response = new PutCohortPatientViewDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientDD:Put()::Unauthorized Access");

                response = PatientManager.InsertCohortPatientView(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutUpdateCohortPatientViewResponse Put(PutUpdateCohortPatientViewRequest request)
        {
            PutUpdateCohortPatientViewResponse response = new PutUpdateCohortPatientViewResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientDD:Put()::Unauthorized Access");

                response = PatientManager.UpdateCohortPatientViewProblem(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutPatientPriorityResponse Put(PutPatientPriorityRequest request)
        {
            PutPatientPriorityResponse response = new PutPatientPriorityResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientDD:Put()::Unauthorized Access");

                response = PatientManager.UpdatePatientPriority(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutPatientFlaggedResponse Put(PutPatientFlaggedRequest request)
        {
            PutPatientFlaggedResponse response = new PutPatientFlaggedResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientDD:Put()::Unauthorized Access");

                response = PatientManager.UpdatePatientFlagged(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutPatientSystemIdDataResponse Put(PutPatientSystemIdDataRequest request)
        {
            PutPatientSystemIdDataResponse response = new PutPatientSystemIdDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientDD:Put()::Unauthorized Access");

                response = PatientManager.UpdatePatientSystem(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutInitializePatientDataResponse Put(PutInitializePatientDataRequest request)
        {
            PutInitializePatientDataResponse response = new PutInitializePatientDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientDD:Put()::Unauthorized Access");

                response = PatientManager.InitializePatient(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetCohortPatientsDataResponse Get(GetCohortPatientsDataRequest request)
        {
            GetCohortPatientsDataResponse response = new GetCohortPatientsDataResponse();

            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientDD:Get()::Unauthorized Access");

                response = PatientManager.GetCohortPatients(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetCohortPatientViewResponse Get(GetCohortPatientViewRequest request)
        {
            GetCohortPatientViewResponse response = new GetCohortPatientViewResponse();

            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientDD:Get()::Unauthorized Access");

                response = PatientManager.GetCohortPatientView(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public InsertBatchPatientsDataResponse Post(InsertBatchPatientsDataRequest request)
        {
            InsertBatchPatientsDataResponse response = new InsertBatchPatientsDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientDD:Post()::Unauthorized Access");

                response = PatientManager.InsertBatchPatients(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        #region Delete
        public DeletePatientDataResponse Delete(DeletePatientDataRequest request)
        {
            DeletePatientDataResponse response = new DeletePatientDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientDD:PatientDelete()::Unauthorized Access");

                response = PatientManager.DeletePatient(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public DeletePatientUserByPatientIdDataResponse Delete(DeletePatientUserByPatientIdDataRequest request)
        {
            DeletePatientUserByPatientIdDataResponse response = new DeletePatientUserByPatientIdDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientDD:PatientUserDelete()::Unauthorized Access");

                response = PatientManager.DeletePatientUserByPatientId(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public DeleteCohortPatientViewDataResponse Delete(DeleteCohortPatientViewDataRequest request)
        {
            DeleteCohortPatientViewDataResponse response = new DeleteCohortPatientViewDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientDD:DeleteCohortPatientView()::Unauthorized Access");

                response = PatientManager.DeleteCohortPatientViewByPatientId(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        } 
        #endregion

        #region UndoDelete
        public UndoDeletePatientDataResponse Put(UndoDeletePatientDataRequest request)
        {
            UndoDeletePatientDataResponse response = new UndoDeletePatientDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientDD:UndoPatientDelete()::Unauthorized Access");

                response = PatientManager.UndoDeletePatient(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public UndoDeletePatientUsersDataResponse Put(UndoDeletePatientUsersDataRequest request)
        {
            UndoDeletePatientUsersDataResponse response = new UndoDeletePatientUsersDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientDD:UndoPatientUserDelete()::Unauthorized Access");

                response = PatientManager.UndoDeletePatientUser(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public UndoDeleteCohortPatientViewDataResponse Put(UndoDeleteCohortPatientViewDataRequest request)
        {
            UndoDeleteCohortPatientViewDataResponse response = new UndoDeleteCohortPatientViewDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientDD:UndoDeleteCohortPatientView()::Unauthorized Access");

                response = PatientManager.UndoDeleteCohortPatientView(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }
        #endregion

        public SyncPatientInfoDataResponse Put(SyncPatientInfoDataRequest request)
        {
            var response = new SyncPatientInfoDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientDD:SyncPatientInfoDataRequest()::Unauthorized Access");

                response = PatientManager.SyncPatient(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public AddPCMToCohortPatientViewDataResponse Put(AddPCMToCohortPatientViewDataRequest request )
        {
            var response = new AddPCMToCohortPatientViewDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientDD:AddPCMToCohortPatientViewDataResponse()::Unauthorized Access");

                var managerResponse = PatientManager.AddPcmToCohortPatientView(request);
                response.IsSuccessful = managerResponse.IsSuccessful;
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                var aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public RemovePCMFromCohortPatientViewDataResponse Delete(RemovePCMFromCohortPatientViewDataRequest request)
        {
            var response = new RemovePCMFromCohortPatientViewDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientDD:RemovePCMFromCohortPatientViewDataRequest()::Unauthorized Access");

                var managerResponse = PatientManager.RemovePcmFromCohortPatientView(request);
                response.IsSuccessful = managerResponse.IsSuccessful;
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                var aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public AssignContactsToCohortPatientViewDataResponse Put(AssignContactsToCohortPatientViewDataRequest request)
        {
            var response = new AssignContactsToCohortPatientViewDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientDD:AssignContactsToCohortPatientViewDataRequest()::Unauthorized Access");

                var managerResponse = PatientManager.AssignContactsToCohortPatientView(request);
                response.IsSuccessful = managerResponse.IsSuccessful;
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);

                var aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }
    }
}