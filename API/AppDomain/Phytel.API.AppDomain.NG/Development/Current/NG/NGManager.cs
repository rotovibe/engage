using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.Programs;
using Phytel.API.Common.CustomObject;
using Phytel.API.DataDomain.CareMember.DTO;
using Phytel.API.DataDomain.Cohort.DTO;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.DataDomain.Scheduling.DTO;
using Phytel.API.Interface;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Hosting;
using AutoMapper;
using MongoDB.Bson;
using Phytel.API.AppDomain.NG.Command;
using Phytel.API.Common.Extensions;
using Phytel.API.DataDomain.Contact.DTO.CareTeam;
using DD = Phytel.API.DataDomain.Program.DTO;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.DataDomain.Contact.DTO.ContactTypeLookUp;
using ServiceStack.Common;
using ServiceStack.Common.Utils;
using ServiceStack.FluentValidation;
using ServiceStack.FluentValidation.Results;
using ServiceStack.Text;
using ServiceStack.Validation;

namespace Phytel.API.AppDomain.NG
{
    public class NGManager : INGManager
    {
        #region dependencies
        public IPlanElementUtils PlanElementUtils { get; set; }
        public IEndpointUtils EndpointUtils { get; set; }
        public IContactEndpointUtil ContactEndpointUtil { get; set; }
        #endregion

        #region Endpoint addresses
       
        protected static readonly string DDPatientServiceURL = ConfigurationManager.AppSettings["DDPatientServiceUrl"];
        protected static readonly string DDPatientProblemServiceUrl = ConfigurationManager.AppSettings["DDPatientProblemServiceUrl"];
        protected static readonly string DDLookupServiceUrl = ConfigurationManager.AppSettings["DDLookupServiceUrl"];
        protected static readonly string DDProgramServiceUrl = ConfigurationManager.AppSettings["DDProgramServiceUrl"];
        protected static readonly string DDCohortServiceUrl = ConfigurationManager.AppSettings["DDCohortServiceUrl"];
        protected static readonly string DDPatientSystemUrl = ConfigurationManager.AppSettings["DDPatientSystemUrl"];
        protected static readonly string DDContactServiceUrl = ConfigurationManager.AppSettings["DDContactServiceUrl"];
        protected static readonly string DDCareMemberUrl = ConfigurationManager.AppSettings["DDCareMemberUrl"];
        protected static readonly string DDPatientNoteUrl = ConfigurationManager.AppSettings["DDPatientNoteUrl"];
        protected static readonly string DDPatientObservationsServiceUrl = ConfigurationManager.AppSettings["DDPatientObservationUrl"];
        protected static readonly string DDPatientGoalsServiceUrl = ConfigurationManager.AppSettings["DDPatientGoalUrl"];
        protected static readonly string DDSchedulingUrl = ConfigurationManager.AppSettings["DDSchedulingUrl"];

        protected static readonly string DDContactTypeLookupServiceURL = ConfigurationManager.AppSettings["DDContactTypeLookupServiceURL"];

        #endregion

        public void LogException(Exception ex)
        {
            string _aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
            Common.Helper.LogException(int.Parse(_aseProcessID), ex);
        }

        #region Patient
        public NG.DTO.GetPatientResponse GetPatient(NG.DTO.GetPatientRequest request)
        {
            NG.DTO.GetPatientResponse pResponse = new NG.DTO.GetPatientResponse();

            try
            {
                //Execute call(s) to Patient Data Domain
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/patient/{4}",
                                                                                            DDPatientServiceURL,
                                                                                            "NG",
                                                                                            request.Version,
                                                                                            request.ContractNumber,
                                                                                            request.PatientID), request.UserId);

                GetPatientDataResponse response = client.Get<GetPatientDataResponse>(url);

                if (response != null && response.Patient != null)
                {
                    // Commented this part out, as there is a separate endpoint to get patient systems ids.
                    //Phytel.API.DataDomain.PatientSystem.DTO.GetPatientSystemDataResponse sysResponse = null;

                    //if (string.IsNullOrEmpty(response.Patient.DisplayPatientSystemId) == false)
                    //{
                    //    client = new JsonServiceClient();
                    //    string patientSystemUrl = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientSystem/{4}",
                    //                                                                DDPatientSystemUrl,
                    //                                                                "NG",
                    //                                                                request.Version,
                    //                                                                request.ContractNumber,
                    //                                                                response.Patient.DisplayPatientSystemId), request.UserId);
                    //    sysResponse = client.Get<Phytel.API.DataDomain.PatientSystem.DTO.GetPatientSystemDataResponse>(patientSystemUrl);
                    //}

                    var patient = new NG.DTO.Patient
                    {
                        Id = response.Patient.Id,
                        FirstName = response.Patient.FirstName,
                        LastName = response.Patient.LastName,
                        DOB = NGUtils.IsDateValid(response.Patient.DOB) ? response.Patient.DOB : string.Empty,
                        Gender = response.Patient.Gender,
                        MiddleName = response.Patient.MiddleName,
                        Suffix = response.Patient.Suffix,
                        PreferredName = response.Patient.PreferredName,
                        Priority = response.Patient.PriorityData,
                        Flagged = Convert.ToInt32(response.Patient.Flagged),
                        Background = response.Patient.Background,
                        ClinicalBackground = response.Patient.ClinicalBackground,
                        LastFourSSN = response.Patient.LastFourSSN,
                        DataSource = response.Patient.DataSource,
                        ReasonId = response.Patient.ReasonId,
                        StatusId = response.Patient.StatusId,
                        StatusDataSource = response.Patient.StatusDataSource,
                        MaritalStatusId = response.Patient.MaritalStatusId,
                        Protected = response.Patient.Protected,
                        DeceasedId = response.Patient.DeceasedId,
                        Prefix = response.Patient.Prefix
                    };

                    var contact =
                        GetContactByPatientId(new GetContactByPatientIdRequest
                        {
                            ContractNumber = request.ContractNumber,
                            PatientID = request.PatientID,
                            UserId = request.UserId,
                            Version = request.Version
                        });

                    if (contact != null)
                    {
                        patient.ContactId = contact.Id;
                    }

                    pResponse.Patient = patient;

                    // Commented this part out, as there is a separate endpoint to get patient systems ids.
                    //if (sysResponse != null && sysResponse.PatientSystemData != null)
                    //{
                    // //   pResponse.Patient.DisplaySystemId = sysResponse.PatientSystem.SystemId;
                    // //   pResponse.Patient.DisplaySystemName = sysResponse.PatientSystem.SystemName;
                    //    pResponse.Patient.DisplayLabel = sysResponse.PatientSystemData.DisplayLabel;
                    //}


                    // Add the recently accessed patient to the User's(Contact) recent list. NIGHT-911.
                    PutRecentPatientResponse contactDDResponse = null;
                    //[Route("/{Context}/{Version}/{ContractNumber}/Patient/Contact/Recent", "PUT")]
                    string contactDDUrl = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/Contact/Recent",
                                                                                    DDContactServiceUrl,
                                                                                    "NG",
                                                                                    request.Version,
                                                                                    request.ContractNumber), request.UserId);

                    contactDDResponse = client.Put<PutRecentPatientResponse>(contactDDUrl, new PutRecentPatientRequest
                                        {
                                            ContactId  = request.UserId,
                                            PatientId = request.PatientID,
                                            Context = "NG",
                                            ContractNumber = request.ContractNumber, 
                                            UserId = request.UserId,
                                            Version = request.Version
                                        } as object);
                    if (contactDDResponse != null && !contactDDResponse.SuccessData)
                    {
                        LogException(new Exception("AD:GetPatient():: Failed to add the patient in the contact's recent list."));
                    }
                }
                return pResponse;
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:GetPatient()::" + wse.Message, wse.InnerException);
            }
        }

        public GetPatientSSNResponse GetPatientSSN(GetPatientSSNRequest request)
        {
            GetPatientSSNResponse result = new GetPatientSSNResponse();
            try
            {
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/SSN",
                                                                                            DDPatientServiceURL,
                                                                                            "NG",
                                                                                            request.Version,
                                                                                            request.ContractNumber,
                                                                                            request.PatientId), request.UserId);

                GetPatientSSNDataResponse response = client.Get<GetPatientSSNDataResponse>(url);

                if (response != null)
                {
                    result.SSN = response.SSN;
                }
                return result;
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:GetPatientSSN()::" + wse.Message, wse.InnerException);
            }
        }

        public PutPatientDetailsUpdateResponse UpsertPatient(PutPatientDetailsUpdateRequest request)
        {
            try
            {
                PutPatientDetailsUpdateResponse response = new PutPatientDetailsUpdateResponse();
                if (request.Patient != null)
                {
                    IRestClient client = new JsonServiceClient();
                    //[Route("/{Context}/{Version}/{ContractNumber}/Patient", "PUT")]
                    string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient",
                                                                                    DDPatientServiceURL,
                                                                                    "NG",
                                                                                    request.Version,
                                                                                    request.ContractNumber), request.UserId);

                    PatientData data = new PatientData
                    {
                        Id = request.Patient.Id,
                        FirstName = request.Patient.FirstName,
                        LastName = request.Patient.LastName,
                        MiddleName = request.Patient.MiddleName,
                        PreferredName = request.Patient.PreferredName,
                        Suffix = request.Patient.Suffix,
                        DOB = request.Patient.DOB,
                        PriorityData = request.Patient.Priority,
                        Gender = request.Patient.Gender,
                        FullSSN = request.Patient.FullSSN,
                        Background = request.Patient.Background,
                        ClinicalBackground = request.Patient.ClinicalBackground,
                        DataSource = request.Patient.DataSource,
                        ReasonId = request.Patient.ReasonId,
                        StatusId = request.Patient.StatusId,
                        StatusDataSource = request.Patient.StatusDataSource,
                        MaritalStatusId = request.Patient.MaritalStatusId,
                        Protected = request.Patient.Protected,
                        DeceasedId = request.Patient.DeceasedId,
                        Prefix = request.Patient.Prefix
                    };
                    PutUpdatePatientDataResponse dataDomainResponse =
                        client.Put<PutUpdatePatientDataResponse>(url, new PutUpdatePatientDataRequest
                        {
                            Context = "NG",
                            ContractNumber = request.ContractNumber,
                            PatientData = data,
                            Insert = request.Insert,
                            InsertDuplicate = request.InsertDuplicate,
                            UserId = request.UserId,
                            Version = request.Version
                        } as object);
                    if (dataDomainResponse != null)
                    {
                        response.Id = dataDomainResponse.Id;
                        if (dataDomainResponse.Outcome == null)
                        {
                            // Proceed with syncing contacts only if there is no duplicate patient 
                            var contact = GetContactByPatientId(new GetContactByPatientIdRequest
                            {
                                ContractNumber = request.ContractNumber,
                                UserId = request.UserId,
                                PatientID = request.Patient.Id,
                                Version = 1.0
                            });
                            if (contact != null)
                            {
                                //Sync Contact 
                                SyncContactByPatientData(contact.Id, data, request.Version, request.ContractNumber,
                                    request.UserId);
                            }
                            else
                            {
                                InsertContactByPatient(request.Patient, request.ContractNumber, request.UserId, request.Version);
                            }
                        }
                        else
                        {
                            response.Outcome = new DTO.Outcome
                            {
                                Result = dataDomainResponse.Outcome.Result,
                                Reason = dataDomainResponse.Outcome.Reason
                            };
                        }
                    }
                }
                return response;
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:UpsertPatient()::" + wse.Message, wse.InnerException);
            }
        }

        public PutPatientFlaggedUpdateResponse PutPatientFlaggedUpdate(PutPatientFlaggedUpdateRequest request)
        {
            try
            {
                PutPatientFlaggedUpdateResponse response = null;

                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/patient/{4}/flagged/{5}",
                                                                                DDPatientServiceURL,
                                                                                "NG",
                                                                                request.Version,
                                                                                request.ContractNumber,
                                                                                request.PatientId,
                                                                                request.Flagged), request.UserId);

                PutPatientFlaggedResponse dataDomainResponse =
                    client.Put<PutPatientFlaggedResponse>(url, new PutPatientFlaggedResponse { } as object);
                if (dataDomainResponse != null && dataDomainResponse.Success)
                {
                    response = new PutPatientFlaggedUpdateResponse();
                }
                return response;
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:PutPatientFlaggedUpdate()::" + wse.Message, wse.InnerException);
            }
        }

        public GetInitializePatientResponse GetInitializePatient(GetInitializePatientRequest request)
        {
            try
            {
                GetInitializePatientResponse response = new GetInitializePatientResponse();

                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/Initialize", "PUT")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/patient/Initialize",
                                                                                DDPatientServiceURL,
                                                                                "NG",
                                                                                request.Version,
                                                                                request.ContractNumber), request.UserId);

                PutInitializePatientDataResponse dataDomainResponse =
                    client.Put<PutInitializePatientDataResponse>(url, new PutInitializePatientDataRequest
                    {
                        Context = "NG",
                        ContractNumber = request.ContractNumber,
                        UserId = request.UserId,
                        Version = request.Version
                    } as object);

                if (dataDomainResponse != null && dataDomainResponse.PatientData != null)
                {
                    response = new GetInitializePatientResponse { 
                        Patient = new Patient { Id = dataDomainResponse.PatientData.Id },
                        Version = dataDomainResponse.Version
                    };
                }
                return response;
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:GetInitializePatient()::" + wse.Message, wse.InnerException);
            }
        }

       
        public PostDeletePatientResponse DeletePatient(PostDeletePatientRequest request)
        {
            PostDeletePatientResponse response = new PostDeletePatientResponse();
            try
            {
                var contact = GetContactByPatientId(new GetContactByPatientIdRequest
                {
                    ContractNumber = request.ContractNumber,
                    PatientID = request.Id,
                    UserId = request.UserId,
                    Version = request.Version
                });
                if (contact == null)
                    throw new ApplicationException("The patient contact card was not found");

                

                IRestClient client = new JsonServiceClient();
                INGUnitOfWork uow = new NGUnitOfWork();

                INGCommand deletePatientCommand = new PatientCommand(request, client);
                uow.Execute(deletePatientCommand);

                INGCommand deletePatientUserCommand = new PatientUsersCommand(request, client);
                uow.Execute(deletePatientUserCommand);

                INGCommand deleteCPVCommand = new CohortPatientViewCommand(request, client);
                uow.Execute(deleteCPVCommand);


                //Commenting the Delete Command as a Contact should not be deleted. 
                //INGCommand deleteContactCommand = new ContactCommand(request, client);
                //uow.Execute(deleteContactCommand);

                //INGCommand deleteCareMemberCommand = new CareMembersCommand(request, client);
                //uow.Execute(deleteCareMemberCommand);

                INGCommand deleteCareTeamCommand = new CareTeamCommand(request,ContactEndpointUtil,contact.Id);
                uow.Execute(deleteCareTeamCommand);

                INGCommand deletePatientNoteCommand = new PatientNotesCommand(request, client);
                uow.Execute(deletePatientNoteCommand);

                INGCommand deletePatientUtilizationsCommand = new PatientUtilizationsCommand(request, client);
                uow.Execute(deletePatientUtilizationsCommand);

                INGCommand deletePatientSystemCommand = new PatientSystemsCommand(request, client);
                uow.Execute(deletePatientSystemCommand);

                INGCommand deletePatientObservationCommand = new PatientObservationsCommand(request, client);
                uow.Execute(deletePatientObservationCommand);

                INGCommand deletePatientGoalCommand = new PatientGoalsCommand(request, client);
                uow.Execute(deletePatientGoalCommand);

                INGCommand deletePatientToDosCommand = new PatientToDosCommand(request, client);
                uow.Execute(deletePatientToDosCommand);

                INGCommand deletePatientAllergiesCommand = new PatientAllergiesCommand(request, client);
                uow.Execute(deletePatientAllergiesCommand);

                INGCommand deletePatientMedSuppsCommand = new PatientMedSuppsCommand(request, client);
                uow.Execute(deletePatientMedSuppsCommand);

                INGCommand deletePatientProgramCommand = new PatientProgramsCommand(request, client);
                uow.Execute(deletePatientProgramCommand);


                

                if (contact != null)
                {
                    var contactId = contact.Id;
                    var dereferencePatientInContactCommand = new DereferencePatientInContactCommand(contactId,request, ContactEndpointUtil);
                    uow.Execute(dereferencePatientInContactCommand);
                }
                return response;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:DeletePatient()::" + ex.Message, ex.InnerException);
            }
        }
        #endregion

        #region Cohort 
        public List<Phytel.API.AppDomain.NG.DTO.Cohort> GetCohorts(NG.DTO.GetAllCohortsRequest request)
        {
            try
            {
                List<Phytel.API.AppDomain.NG.DTO.Cohort> response = new List<Phytel.API.AppDomain.NG.DTO.Cohort>();

                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Cohorts",
                                                                    DDCohortServiceUrl,
                                                                    "NG",
                                                                    request.Version,
                                                                    request.ContractNumber), request.UserId);

                GetAllCohortsDataResponse dataDomainResponse = client.Get<GetAllCohortsDataResponse>(url);

                List<CohortData> cohorts = dataDomainResponse.Cohorts;

                foreach (CohortData c in cohorts)
                {
                    Phytel.API.AppDomain.NG.DTO.Cohort cohort = new Phytel.API.AppDomain.NG.DTO.Cohort();
                    cohort.ID = c.ID;
                    cohort.SName = c.SName;
                    response.Add(cohort);
                }
                return response;
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:GetCohorts()::" + wse.Message, wse.InnerException);
            }
        }

        public GetCohortPatientsResponse GetCohortPatients(GetCohortPatientsRequest request)
        {
            GetCohortPatientsResponse pResponse = new GetCohortPatientsResponse();
            pResponse.Patients = new List<CohortPatient>();

            try
            {
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/CohortPatients/{4}?Skip={5}&Take={6}&SearchFilter={7}",
                                                                                            DDPatientServiceURL,
                                                                                            "NG",
                                                                                            request.Version,
                                                                                            request.ContractNumber,
                                                                                            request.CohortID,
                                                                                            request.Skip,
                                                                                            request.Take,
                                                                                            request.SearchFilter), request.UserId);

                // Route("/{Context}/{Version}/{ContractNumber}/CohortPatients/{CohortID}", "GET")]
                GetCohortPatientsDataResponse qResponse = client.Get<GetCohortPatientsDataResponse>(url);

                //take qResponse Patient details and map them to "Patient" in the GetCohortPatientsResponse
                qResponse.CohortPatients.ForEach(x => pResponse.Patients.Add(new CohortPatient
                {
                    Id = x.Id,
                    DOB = x.DOB,
                    FirstName = x.FirstName,
                    Gender = x.Gender,
                    LastName = x.LastName,
                    MiddleName = x.MiddleName,
                    PreferredName = x.PreferredName,
                    Suffix = x.Suffix
                }));

                if (qResponse.Status != null)
                {
                    pResponse.Status = qResponse.Status;
                }

                return pResponse;
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:GetCohortPatients()::" + wse.Message, wse.InnerException);
            }
        }

        #endregion

        #region Setting
        public GetAllSettingsResponse GetAllSettings(GetAllSettingsRequest request)
        {
            try
            {
                GetAllSettingsResponse response = new GetAllSettingsResponse();
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Settings", "GET")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Settings",
                                                                        DDLookupServiceUrl,
                                                                        "NG",
                                                                        request.Version,
                                                                        request.ContractNumber), request.UserId);

                GetAllSettingsDataResponse dataDomainResponse = client.Get<GetAllSettingsDataResponse>(url);
                if (dataDomainResponse != null)
                {
                    response.Settings = dataDomainResponse.SettingsData;
                }
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Programs

        public PostProgramAttributesChangeResponse PostProgramAttributeChanges(
            PostProgramAttributesChangeRequest request)
        {
            try
            {
                PostProgramAttributesChangeResponse response = new PostProgramAttributesChangeResponse
                {
                    PlanElems = new PlanElements
                    {
                        Actions = new List<Actions>(),
                        Modules = new List<Module>(),
                        Programs = new List<Phytel.API.AppDomain.NG.DTO.Program>(),
                        Steps = new List<Step>()
                    }
                };

                var aReq = new PostProcessActionRequest
                {
                    PatientId = request.PatientId,
                    ProgramId = request.ProgramId,
                    ContractNumber = request.ContractNumber,
                    Token = request.Token,
                    UserId = request.UserId,
                    Version = request.Version
                };

                Phytel.API.AppDomain.NG.DTO.Program pg = EndpointUtils.RequestPatientProgramDetail(aReq);
                if (pg == null) throw new Exception("Program is null.");

                if (PlanElementUtils.UpdatePlanElementAttributes(pg, request.PlanElement, request.UserId,
                    response.PlanElems))
                {
                    var pD = NGUtils.FormatProgramDetail(pg);
                    response.Outcome = EndpointUtils.SaveProgramAttributeChanges(request, pD);
                }
                else
                {
                    response.Outcome = new Phytel.API.AppDomain.NG.DTO.Outcome
                    {
                        Reason = "PlanElement is not in the correct state to allow change.",
                        Result = 2
                    };
                }

                return response;
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:PostPatientToProgram()::" + wse.Message, wse.InnerException);
            }
        }

        public GetActiveProgramsResponse GetActivePrograms(GetActiveProgramsRequest request)
        {
            GetActiveProgramsResponse pResponse = new GetActiveProgramsResponse();

            try
            {
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Programs/Active",
                        DDProgramServiceUrl,
                        "NG",
                        request.Version,
                        request.ContractNumber), request.UserId);

                GetAllActiveProgramsResponse dataDomainResponse;

                var adProgs = new List<DTO.ProgramInfo>();

                try
                {
                    dataDomainResponse =
                        client.Get<GetAllActiveProgramsResponse>(url);   

                    dataDomainResponse.Programs.ForEach(p => adProgs.Add(new DTO.ProgramInfo
                    {
                        AttrEndDate = p.AttrEndDate,
                        ElementState = p.ElementState,
                        Id = p.Id,
                        Name = p.Name,
                        PatientId = p.PatientId,
                        ProgramState = p.ProgramState,
                        ShortName = p.ShortName,
                        Status = p.Status
                    }));
                }
                catch (Exception ex)
                {
                    throw new WebServiceException(ex.Message, ex.InnerException);
                }

                pResponse.Programs = adProgs;
                pResponse.Version = 1;
                return pResponse;
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:GetActivePrograms()::" + wse.Message, wse.InnerException);
            }
        }

        public PostPatientToProgramsResponse PostPatientToProgram(PostPatientToProgramsRequest request)
        {
            try
            {
                PostPatientToProgramsResponse response = new PostPatientToProgramsResponse();

                // get PCMId from service call.
                string primaryCM = EndpointUtils.GetPrimaryCareManagerForPatient(request);

                DD.PutProgramToPatientResponse dataDomainResponse = EndpointUtils.AssignPatientToProgram(request, primaryCM);

                if (dataDomainResponse.program != null)
                {
                    response.Program = new DTO.ProgramInfo()
                    {
                        Id = dataDomainResponse.program.Id,
                        Name = dataDomainResponse.program.Name,
                        ProgramState = dataDomainResponse.program.ProgramState,
                        ShortName = dataDomainResponse.program.ShortName,
                        Status = dataDomainResponse.program.Status,
                        PatientId = dataDomainResponse.program.PatientId,
                        ElementState = dataDomainResponse.program.ElementState
                    };
                }

                response.Result = new DTO.Outcome()
                {
                    Reason = dataDomainResponse.Outcome.Reason,
                    Result = dataDomainResponse.Outcome.Result
                };

                if (dataDomainResponse.Status != null)
                    response.Status = dataDomainResponse.Status;

                return response;
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:PostPatientToProgram()::" + wse.Message, wse.InnerException);
            }
        }



        public GetPatientProgramDetailsSummaryResponse GetPatientProgramDetailsSummary(GetPatientProgramDetailsSummaryRequest request)
        {
            try
            {
                GetPatientProgramDetailsSummaryResponse result = new GetPatientProgramDetailsSummaryResponse();

                DD.GetProgramDetailsSummaryResponse resp = EndpointUtils.RequestPatientProgramDetailsSummary(request);

                if (resp != null)
                {
                    if (resp.Program != null)
                    {
                        result.Program = new Phytel.API.AppDomain.NG.DTO.Program
                        {
                            Id = resp.Program.Id,
                            Client = resp.Program.Client,
                            Name = resp.Program.Name,
                            ContractProgramId = resp.Program.ContractProgramId,
                            Description = resp.Program.Description,
                            EligibilityEndDate = resp.Program.EligibilityEndDate,
                            EligibilityRequirements = resp.Program.EligibilityRequirements,
                            EligibilityStartDate = resp.Program.EligibilityStartDate,
                            StartDate = resp.Program.StartDate,
                            EndDate = resp.Program.EndDate,
                            AttrStartDate = resp.Program.AttrStartDate,
                            AttrEndDate = resp.Program.AttrEndDate,
                            AuthoredBy = resp.Program.AuthoredBy,
                            TemplateName = resp.Program.TemplateName,
                            TemplateVersion = resp.Program.TemplateVersion,
                            ProgramVersion = resp.Program.ProgramVersion,
                            ProgramVersionUpdatedOn = resp.Program.ProgramVersionUpdatedOn,
                            PatientId = resp.Program.PatientId,
                            ProgramState = resp.Program.ProgramState,
                            ShortName = resp.Program.ShortName,
                            Status = resp.Program.Status,
                            Version = resp.Program.Version,
                            Completed = resp.Program.Completed,
                            Enabled = resp.Program.Enabled,
                            Next = resp.Program.Next,
                            Order = resp.Program.Order,
                            Previous = resp.Program.Previous,
                            ElementState = resp.Program.ElementState,
                            StateUpdatedOn =resp.Program.StateUpdatedOn,
                            SpawnElement = getSpawnElement(resp.Program),
                            SourceId = resp.Program.SourceId,
                            CompletedBy = resp.Program.CompletedBy,
                            DateCompleted = resp.Program.DateCompleted,
                            Modules = getModuleInfo(resp, request),
                            Objectives = GetObjectivesInfo(resp.Program.ObjectivesData),
                            AssignById = resp.Program.AssignBy,
                            AssignDate = resp.Program.AssignDate,
                            AssignToId = resp.Program.AssignTo,
                            Attributes = getAttributes(resp.Program.Attributes)
                        };

                        if (resp.Status != null)
                            result.Status = resp.Status;
                    }
                }
                return result;
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:GetPatientProgramDetailsSummary()::" + wse.Message, wse.InnerException);
            }
        }

        private List<ObjectiveInfo> GetObjectivesInfo(List<DD.ObjectiveInfoData> list)
        {
            try
            {
                List<ObjectiveInfo> objs = new List<ObjectiveInfo>();

                if (list != null)
                {
                    list.ForEach(r =>
                    {
                        objs.Add(new ObjectiveInfo
                            {
                                Id = r.Id.ToString(),
                                Unit = r.Unit,
                                Status = r.Status,
                                Value = r.Value
                            });
                    });
                }

                return objs;
        }
            catch (Exception ex)
            {
                throw new WebServiceException("AD:GetObjectivesInfo()::" + ex.Message, ex.InnerException);
            }
        }

        public Phytel.API.AppDomain.NG.DTO.GetPatientProgramsResponse GetPatientPrograms(Phytel.API.AppDomain.NG.DTO.GetPatientProgramsRequest request)
        {
            try
            {
                Phytel.API.AppDomain.NG.DTO.GetPatientProgramsResponse result = new Phytel.API.AppDomain.NG.DTO.GetPatientProgramsResponse();

                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Programs/",
                    DDProgramServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.Token), request.UserId);

                DD.GetPatientProgramsDataResponse resp =
                    client.Get<DD.GetPatientProgramsDataResponse>(url);

                if (resp != null)
                {
                    if (resp.programs != null)
                    {
                        List<Phytel.API.AppDomain.NG.DTO.ProgramInfo> adPs = new List<Phytel.API.AppDomain.NG.DTO.ProgramInfo>();
                        resp.programs.ForEach(p => adPs.Add(new Phytel.API.AppDomain.NG.DTO.ProgramInfo
                        {
                            Id = p.Id,
                            Name = p.Name,
                            PatientId = p.PatientId,
                            ProgramState = p.ProgramState,
                            ShortName = p.ShortName,
                            Status = p.Status,
                            ElementState = p.ElementState,
                            AttrEndDate = p.AttrEndDate,
                            ProgramSourceId = p.ProgramSourceId
                        }));

                        result.Programs = adPs;
                        if (resp.Status != null)
                            result.Status = resp.Status;
                    }
                }
                return result;
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:GetPatientPrograms()::" + wse.Message, wse.InnerException);
            }
        }

        public GetPatientActionDetailsResponse GetPatientActionDetails(GetPatientActionDetailsRequest request)
        {
            try
            {
                GetPatientActionDetailsResponse result = new GetPatientActionDetailsResponse();
                // [Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Program/{PatientProgramId}/Module/{PatientModuleId}/Action/{PatientActionId}", "GET")]
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Program/{5}/Module/{6}/Action/{7}",
                    DDProgramServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.PatientProgramId,
                    request.PatientModuleId,
                    request.PatientActionId), request.UserId);

                DD.GetPatientActionDetailsDataResponse resp = client.Get<DD.GetPatientActionDetailsDataResponse>(url);

                if (resp != null)
                {
                    if (resp.ActionData != null)
                    {
                        result.Action = getActionInfo(resp.ActionData, request, true);

                        if (resp.Status != null)
                            result.Status = resp.Status;
                    }
                }
                return result;
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:GetPatientActionDetails()::" + wse.Message, wse.InnerException);
            }
        }


        public PostRemovePatientProgramResponse RemovePatientProgram(PostRemovePatientProgramRequest request)
        {
            try
            {
                PostRemovePatientProgramResponse response = new PostRemovePatientProgramResponse();
                IRestClient client = new JsonServiceClient();

                INGCommand deletePatientProgramCommand = new PatientProgramCommand(request, client);
                deletePatientProgramCommand.Execute();

                #region InsertANote
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Note", "POST")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/patient/{4}/note",
                                                                        DDPatientNoteUrl,
                                                                        "NG",
                                                                        request.Version,
                                                                        request.ContractNumber,
                                                                        request.PatientId), request.UserId);

                string noteText = string.Format("Program titled {0} was removed for the following reason: {1}", request.ProgramName, string.IsNullOrEmpty(request.Reason) ? "unknown" : request.Reason);
                PatientNoteData noteData = new PatientNoteData
                {
                    Text = noteText,
                    TypeId = Constants.GeneralNoteTypeId,
                    CreatedById = request.UserId,
                    CreatedOn = DateTime.UtcNow,
                    PatientId = request.PatientId
                };
                InsertPatientNoteDataResponse noteDDResponse = client.Post<InsertPatientNoteDataResponse>(url, new InsertPatientNoteDataRequest
                {
                    PatientNote = noteData,
                    Context = "NG",
                    ContractNumber = request.ContractNumber,
                    Version = request.Version,
                    UserId = request.UserId,
                    PatientId = request.PatientId
                } as object); 
                #endregion

                #region RemoveProgramReferenceInGoals
                // [Route("/{Context}/{Version}/{ContractNumber}/Goal/RemoveProgram/{ProgramId}/Update", "PUT")]
                string goalUrl = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Goal/RemoveProgram/{4}/Update",
                                            DDPatientGoalsServiceUrl,
                                            "NG",
                                            request.Version,
                                            request.ContractNumber,
                                            request.Id), request.UserId);

                RemoveProgramInPatientGoalsDataResponse goalDDResponse = client.Put<RemoveProgramInPatientGoalsDataResponse>(goalUrl, new RemoveProgramInPatientGoalsDataRequest
                {
                    ProgramId = request.Id,
                    Context = "NG",
                    ContractNumber = request.ContractNumber,
                    Version = request.Version,
                    UserId = request.UserId
                });
                #endregion  

                #region RemoveProgramReferenceInNotes
                //[Route("/{Context}/{Version}/{ContractNumber}/Note/RemoveProgram/{ProgramId}/Update", "PUT")]
                string notesURL = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Note/RemoveProgram/{4}/Update",
                                            DDPatientNoteUrl,
                                            "NG",
                                            request.Version,
                                            request.ContractNumber,
                                            request.Id), request.UserId);
                RemoveProgramInPatientNotesDataResponse notesDDResponse = client.Put<RemoveProgramInPatientNotesDataResponse>(notesURL, new RemoveProgramInPatientNotesDataRequest
                {
                    ProgramId = request.Id,
                    Context = "NG",
                    ContractNumber = request.ContractNumber,
                    Version = request.Version,
                    UserId = request.UserId
                });
                #endregion

                #region RemoveProgramReferenceInToDos
                //[Route("/{Context}/{Version}/{ContractNumber}/Scheduling/ToDo/RemoveProgram/{ProgramId}/Update", "PUT")]
                string schUrl = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Scheduling/ToDo/RemoveProgram/{4}/Update",
                                            DDSchedulingUrl,
                                            "NG",
                                            request.Version,
                                            request.ContractNumber,
                                            request.Id), request.UserId);
                RemoveProgramInToDosDataResponse todoDDResponse = client.Put<RemoveProgramInToDosDataResponse>(schUrl, new RemoveProgramInToDosDataRequest
                {
                    ProgramId = request.Id,
                    Context = "NG",
                    ContractNumber = request.ContractNumber,
                    Version = request.Version,
                    UserId = request.UserId
                });
                #endregion

                return response;
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:RemovePatientProgram()::" + wse.Message, wse.InnerException);
            }
        }

        #endregion

        #region LookUp
        public List<IdNamePair> GetProblems(GetAllProblemsRequest request)
        {
            try
            {
                List<IdNamePair> response = new List<IdNamePair>();

                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/problems",
                                                            DDLookupServiceUrl,
                                                            "NG",
                                                            request.Version,
                                                            request.ContractNumber), request.UserId);

                //[Route("/{Context}/{Version}/{ContractNumber}/problems", "GET")]
                Phytel.API.DataDomain.LookUp.DTO.GetAllProblemsDataResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.LookUp.DTO.GetAllProblemsDataResponse>(url);

                List<ProblemData> problems = dataDomainResponse.Problems;

                foreach (ProblemData c in problems)
                {
                    IdNamePair lookUp = new IdNamePair();
                    lookUp.Id = c.Id;
                    lookUp.Name = c.Name;
                    response.Add(lookUp);
                }

                return response;
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:GetProblems()::" + wse.Message, wse.InnerException);
            }
        }

        public List<IdNamePair> GetAllCommModes(GetAllCommModesRequest request)
        {
            try
            {
                List<IdNamePair> response = new List<IdNamePair>();
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/commmodes",
                                                                    DDLookupServiceUrl,
                                                                    "NG",
                                                                    request.Version,
                                                                    request.ContractNumber), request.UserId);

                // [Route("/{Context}/{Version}/{ContractNumber}/commmodes", "GET")]
                Phytel.API.DataDomain.LookUp.DTO.GetAllCommModesDataResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.LookUp.DTO.GetAllCommModesDataResponse>(url);

                List<IdNamePair> dataList  = dataDomainResponse.CommModes;
                if (dataList != null && dataList.Count > 0)
                {
                    response = dataList;
                }
                return response;
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:GetAllCommModes()::" + wse.Message, wse.InnerException);
            }
        }

        public List<StatesLookUp> GetAllStates(GetAllStatesRequest request)
        {
            try
            {
                List<StatesLookUp> response = new List<StatesLookUp>();
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/states",
                                                                        DDLookupServiceUrl,
                                                                        "NG",
                                                                        request.Version,
                                                                        request.ContractNumber), request.UserId);

                // [Route("/{Context}/{Version}/{ContractNumber}/states", "GET")]
                Phytel.API.DataDomain.LookUp.DTO.GetAllStatesDataResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.LookUp.DTO.GetAllStatesDataResponse>(url);

                List<StateData> dataList = dataDomainResponse.States;

                foreach (StateData d in dataList)
                {
                    StatesLookUp lookUp = new StatesLookUp();
                    lookUp.Id = d.Id;
                    lookUp.Name = d.Name;
                    lookUp.Code = d.Code;
                    response.Add(lookUp);
                }
                return response;
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:GetAllStates()::" + wse.Message, wse.InnerException);
            }
        }

        public List<IdNamePair> GetAllTimesOfDays(GetAllTimesOfDaysRequest request)
        {
            try
            {
                List<IdNamePair> response = new List<IdNamePair>();
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/timesOfDays",
                                                                    DDLookupServiceUrl,
                                                                    "NG",
                                                                    request.Version,
                                                                    request.ContractNumber), request.UserId);
                //[Route("/{Context}/{Version}/{ContractNumber}/timesOfDays", "GET")]
                Phytel.API.DataDomain.LookUp.DTO.GetAllTimesOfDaysDataResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.LookUp.DTO.GetAllTimesOfDaysDataResponse>(url);

                List<IdNamePair> dataList = dataDomainResponse.TimesOfDays;
                if (dataList != null && dataList.Count > 0)
                {
                    response = dataList;
                }
                return response;
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:GetAllTimesOfDays()::" + wse.Message, wse.InnerException);
            }
        }

        public List<CommTypeLookUp> GetAllCommTypes(GetAllCommTypesRequest request)
        {
            try
            {
                List<CommTypeLookUp> response = new List<CommTypeLookUp>();
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/commtypes",
                                                                    DDLookupServiceUrl,
                                                                    "NG",
                                                                    request.Version,
                                                                    request.ContractNumber), request.UserId);

                // [Route("/{Context}/{Version}/{ContractNumber}/commtypes", "GET")]
                Phytel.API.DataDomain.LookUp.DTO.GetAllCommTypesDataResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.LookUp.DTO.GetAllCommTypesDataResponse>(url);

                List<CommTypeData> dataList = dataDomainResponse.CommTypes;

                foreach (CommTypeData d in dataList)
                {
                    CommTypeLookUp lookUp = new CommTypeLookUp();
                    lookUp.Id = d.Id;
                    lookUp.Name = d.Name;
                    lookUp.CommModes = d.CommModes;
                    response.Add(lookUp);
                }
                return response;
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:GetAllCommTypes()::" + wse.Message, wse.InnerException);
            }
        }

        public List<LanguagesLookUp> GetAllLanguages(GetAllLanguagesRequest request)
        {
            try
            {
                List<LanguagesLookUp> response = new List<LanguagesLookUp>();
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/languages",
                                                                    DDLookupServiceUrl,
                                                                    "NG",
                                                                    request.Version,
                                                                    request.ContractNumber), request.UserId);

                // [Route("/{Context}/{Version}/{ContractNumber}/languages", "GET")]
                Phytel.API.DataDomain.LookUp.DTO.GetAllLanguagesDataResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.LookUp.DTO.GetAllLanguagesDataResponse>(url);

                List<Phytel.API.DataDomain.LookUp.DTO.LanguageData> dataList = dataDomainResponse.Languages;

                if (dataList != null && dataList.Count > 0)
                {
                    // Get all active languages only.
                    List<Phytel.API.DataDomain.LookUp.DTO.LanguageData> activeLanguages = dataList.Where(d => d.Active == true).ToList();

                    foreach (Phytel.API.DataDomain.LookUp.DTO.LanguageData d in activeLanguages)
                    {
                        LanguagesLookUp lookUp = new LanguagesLookUp();
                        lookUp.Id = d.Id;
                        lookUp.Name = d.Name;
                        lookUp.Code = d.Code;
                        response.Add(lookUp);
                    }
                }
                return response;
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:GetAllLanguages()::" + wse.Message, wse.InnerException);
            }
        }

        public List<TimeZonesLookUp> GetAllTimeZones(GetAllTimeZonesRequest request)
        {
            try
            {
                List<TimeZonesLookUp> response = new List<TimeZonesLookUp>();
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/timeZones",
                                                                            DDLookupServiceUrl,
                                                                            "NG",
                                                                            request.Version,
                                                                            request.ContractNumber), request.UserId);

                //[Route("/{Context}/{Version}/{ContractNumber}/timeZones", "GET")]
                Phytel.API.DataDomain.LookUp.DTO.GetAllTimeZonesDataResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.LookUp.DTO.GetAllTimeZonesDataResponse>(url);

                List<TimeZoneData> dataList = dataDomainResponse.TimeZones;

                foreach (TimeZoneData d in dataList)
                {
                    TimeZonesLookUp lookUp = new TimeZonesLookUp();
                    lookUp.Id = d.Id;
                    lookUp.Name = d.Name;
                    lookUp.DefaultZone = d.Default;
                    response.Add(lookUp);
                }
                return response;
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:GetAllTimeZones()::" + wse.Message, wse.InnerException);
            }
        }

        public List<IdNamePair> GetLookUps(GetLookUpsRequest request)
        {
            try
            {
                List<IdNamePair> response = new List<IdNamePair>();
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Type/{4}",
                                                                        DDLookupServiceUrl,
                                                                        "NG",
                                                                        request.Version,
                                                                        request.ContractNumber,
                                                                        request.TypeName), request.UserId);

                //[Route("/{Context}/{Version}/{ContractNumber}/Type/{Name}", "GET")]
                Phytel.API.DataDomain.LookUp.DTO.GetLookUpsDataResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.LookUp.DTO.GetLookUpsDataResponse>(url);

                List<IdNamePair> dataList = dataDomainResponse.LookUpsData;
                if (dataList != null && dataList.Count > 0)
                {
                    response = dataList;
                }
                return response;
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:GetLookUps()::" + wse.Message, wse.InnerException);
            }
        }

        public List<LookUpDetails> GetLookUpDetails(GetLookUpDetailsRequest request)
        {
            try
            {
                List<LookUpDetails> response = new List<LookUpDetails>();
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/LookUp/Details/Type/{Name}", "GET")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/LookUp/Details/Type/{4}",
                                                                        DDLookupServiceUrl,
                                                                        "NG",
                                                                        request.Version,
                                                                        request.ContractNumber,
                                                                        request.TypeName), request.UserId);

                Phytel.API.DataDomain.LookUp.DTO.GetLookUpDetailsDataResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.LookUp.DTO.GetLookUpDetailsDataResponse>(url);

                List<LookUpDetailsData> dataList = dataDomainResponse.LookUpDetailsData;
                if (dataList != null && dataList.Count > 0)
                {
                    List<LookUpDetails> list = new List<LookUpDetails>();
                    dataList.ForEach(m => 
                    {
                        LookUpDetails details  = new LookUpDetails {
                            Id = m.Id,
                            Name = m.Name,
                            IsDefault = m.IsDefault
                        };
                        list.Add(details);
                    });
                    response = list;
                }
                return response;
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:GetLookUpDetails()::" + wse.Message, wse.InnerException);
            }
        }

        public List<ObjectivesLookUp> GetAllObjectives(GetAllObjectivesRequest request)
        {
            try
            {
                List<ObjectivesLookUp> response = new List<ObjectivesLookUp>();
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Objectives",
                                                                    DDLookupServiceUrl,
                                                                    "NG",
                                                                    request.Version,
                                                                    request.ContractNumber), request.UserId);

                // [Route("/{Context}/{Version}/{ContractNumber}/Objectives", "GET")]
                Phytel.API.DataDomain.LookUp.DTO.GetAllObjectivesDataResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.LookUp.DTO.GetAllObjectivesDataResponse>(url);

                List<Phytel.API.DataDomain.LookUp.DTO.ObjectiveData> dataList = dataDomainResponse.ObjectivesData;

                if (dataList != null && dataList.Count > 0)
                {
                    foreach (Phytel.API.DataDomain.LookUp.DTO.ObjectiveData d in dataList)
                    {
                        ObjectivesLookUp lookUp = new ObjectivesLookUp();
                        lookUp.Id = d.Id;
                        lookUp.Name = d.Name;
                        lookUp.Categories = d.CategoriesData;
                        response.Add(lookUp);
                    }
                }
                return response;
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:GetAllObjectives()::" + wse.Message, wse.InnerException);
            }
        }

        #endregion

        #region Contact
        public Contact GetContactByPatientId(GetContactByPatientIdRequest request)
        {
            Contact contact = null;
            try
            {
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Contact",
                        DDContactServiceUrl,
                        "NG",
                        request.Version,
                        request.ContractNumber,
                        request.PatientID), request.UserId);

                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Contact", "GET")]
                GetContactByPatientIdDataResponse dataDomainResponse;
                    dataDomainResponse =
                        client.Get<GetContactByPatientIdDataResponse>(url);

                if (dataDomainResponse != null && dataDomainResponse.Contact != null)
                {
                    var mappedData = Mapper.Map<Contact>(dataDomainResponse.Contact);
                    if (!string.IsNullOrEmpty(mappedData.PatientId))
                        mappedData.IsPatient = true;

                    if (!string.IsNullOrEmpty(mappedData.UserId))
                        mappedData.IsUser = true;
                        
                    contact = mappedData;
                }
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:GetContactByPatientId()::" + wse.Message, wse.InnerException);
            }
            return contact;
        }

        public UpdateContactResponse PutUpdateContact(DTO.UpdateContactRequest request)
        {
            try
            {
                if (request == null)
                    throw new ArgumentNullException("request");
                CheckForRequiredFields(request.Contact);
                ContactData cData = Mapper.Map<ContactData>(request.Contact);
                UpdateContactResponse response = new UpdateContactResponse();

                //[Route("/{Context}/{Version}/{ContractNumber}/Contacts/{Id}", "PUT")]
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Contacts/{4}",
                                                                                DDContactServiceUrl,
                                                                                "NG",
                                                                                request.Version,
                                                                                request.ContractNumber,
                                                                                request.Id), request.UserId);
               
                UpdateContactDataResponse dataDomainResponse =
                    client.Put<UpdateContactDataResponse>(url, new UpdateContactDataRequest
                                                                                {
                                                                                   ContactData = cData,
                                                                                   Context = "NG",
                                                                                   ContractNumber = request.ContractNumber,
                                                                                   Version = request.Version,
                                                                                   UserId = request.UserId
                                                                                } as object);
                
                response.Version = dataDomainResponse.Version;
                if (dataDomainResponse.SuccessData)
                {
                    // If the update was successful, send back the updated map for new phone inserts.
                    if (dataDomainResponse.UpdatedPhoneData != null && dataDomainResponse.UpdatedPhoneData.Count > 0)
                    {
                        List<CleanupId> updatedPhones = new List<CleanupId>();
                        foreach (CleanupIdData c in dataDomainResponse.UpdatedPhoneData)
                        {
                            updatedPhones.Add(new CleanupId { OldId = c.OldId, NewId = c.NewId });
                        }
                        response.UpdatedPhone = updatedPhones;
                    }

                    // If the update was successful, send back the updated map for new phone emails.
                    if (dataDomainResponse.UpdatedEmailData != null && dataDomainResponse.UpdatedEmailData.Count > 0)
                    {
                        List<CleanupId> updatedEmails = new List<CleanupId>();
                        foreach (CleanupIdData c in dataDomainResponse.UpdatedEmailData)
                        {
                            updatedEmails.Add(new CleanupId { OldId = c.OldId, NewId = c.NewId });
                        }
                        response.UpdatedEmail = updatedEmails;
                    }

                    // If the update was successful, send back the updated map for new address inserts.
                    if (dataDomainResponse.UpdatedAddressData != null && dataDomainResponse.UpdatedAddressData.Count > 0)
                    {
                        List<CleanupId> updatedAddresses = new List<CleanupId>();
                        foreach (CleanupIdData c in dataDomainResponse.UpdatedAddressData)
                        {
                            updatedAddresses.Add(new CleanupId { OldId = c.OldId, NewId = c.NewId });
                        }
                        response.UpdatedAddress = updatedAddresses;
                    }
                }
                else 
                {
                    response.Status = dataDomainResponse.Status;
                }

                //Sync Patient 
                if (!string.IsNullOrEmpty(request.Contact.PatientId))
                {
                    SyncPatientByContactData(cData,request.Version, request.ContractNumber, request.UserId);
                }

                return response;
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:PutUpdateContact()::" + wse.Message, wse.InnerException);
            }
        }


        public InsertContactResponse InsertContact(InsertContactRequest request)
        {
            InsertContactResponse insertContactResponse = new InsertContactResponse();
            try
            {
                if(request == null)
                    throw new ArgumentNullException("request");
                CheckForRequiredFields(request.Contact);
                var contactData = Mapper.Map<ContactData>(request.Contact);


                //Check if Request has a timeZone. ENG-1991 (Remove the Default TimeZone)
                //if (string.IsNullOrEmpty(contactData.TimeZoneId))
                //{
                //    string defaultTimeZone = null;
                //    var tzDataRequest = new GetTimeZoneDataRequest { ContractNumber = request.ContractNumber, Version = request.Version, Context = "NG", UserId = request.UserId };
                //    var tz = getDefaultTimeZone(tzDataRequest);
                //    if (tz != null)
                //    {
                //        defaultTimeZone = tz.Id;
                //    }

                //    contactData.TimeZoneId = defaultTimeZone;
                //}

                //Check for modes.
                if (contactData.Modes.IsNullOrEmpty())
                {
                    var commModesData = new List<CommModeData>();
                    var commRequest = new GetAllCommModesRequest { ContractNumber = request.ContractNumber, UserId = request.UserId, Version = request.Version };
                    List<IdNamePair> modesLookUp = GetAllCommModes(commRequest);

                    if (modesLookUp != null && modesLookUp.Count > 0)
                    {
                        foreach (var l in modesLookUp)
                        {
                           
                            commModesData.Add(new CommModeData { ModeId = l.Id, OptOut = false, Preferred = false });
                        }
                    }

                    contactData.Modes = commModesData;
                }

                if (contactData.StatusId == 0)
                    contactData.StatusId = (int)DataDomain.Contact.DTO.Status.Active;

                if (string.IsNullOrEmpty(contactData.Gender))
                    contactData.Gender = "N";


                InsertContactDataRequest ddRequest = new InsertContactDataRequest()
                {
                    ContactData = contactData,
                    UserId = request.UserId,
                    ContractNumber = request.ContractNumber,
                    Context = "NG",
                    Version = request.Version
                };
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Contacts", "POST")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Contacts",
                    DDContactServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber
                    ), request.UserId);

                InsertContactDataResponse dataDomainResponse = client.Post<InsertContactDataResponse>(url, ddRequest);

                if (dataDomainResponse != null)
                {
                    insertContactResponse.Version = dataDomainResponse.Version;
                    insertContactResponse.Id = dataDomainResponse.Id;
                    insertContactResponse.Status = dataDomainResponse.Status;
                }
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:InsertContact()::" + wse.Message, wse.InnerException);
            }
            return insertContactResponse;
        }

        private void CheckForRequiredFields(Contact c)
        {
            if (c != null)
            {
                if (string.IsNullOrEmpty(c.ContactTypeId))
                    throw new Exception("The Contact Type Id cannot be null.");
                else
                {
                    if (string.Compare(c.ContactTypeId, Constants.PersonContactTypeId, true) == 0 && (string.IsNullOrEmpty(c.FirstName) || string.IsNullOrEmpty(c.LastName)))
                        throw new Exception("A contact of Person type cannot have empty First and Last name.");
                }
            }
            else
            {
                throw new ArgumentNullException("Contact");    
            }
        }


        public List<PhoneData> GetPhonesData(List<Phone> phonelist)
        {
            try
            {
                List<PhoneData> phonesData = null;
                List<Phone> phones = phonelist;
                phonesData = phones.Select(Mapper.Map<PhoneData>).ToList();
                return phonesData;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:GetPhonesData()::" + ex.Message, ex.InnerException);
            }
        }

      
        public GetRecentPatientsResponse GetRecentPatients(GetRecentPatientsRequest request)
        {
            GetRecentPatientsResponse response = null;
            try
            {
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Contact/{ContactId}", "GET")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Contact/{4}",
                                                       DDContactServiceUrl,
                                                       "NG",
                                                       request.Version,
                                                       request.ContractNumber,
                                                       request.UserId), request.UserId);

                GetContactByContactIdDataResponse dataDomainResponse = client.Get<GetContactByContactIdDataResponse>(url);
                if (dataDomainResponse != null && dataDomainResponse.Contact != null)
                {
                    List<CohortPatient> patients = null;
                    List<string> recentPatientIds = dataDomainResponse.Contact.RecentsList;
                    if (recentPatientIds != null && recentPatientIds.Count > 0)
                    {
                        //[Route("/{Context}/{Version}/{ContractNumber}/Patients/Ids", "POST")]
                        string patientDDURL = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patients/Ids",
                                                                                        DDPatientServiceURL,
                                                                                        "NG",
                                                                                        request.Version,
                                                                                        request.ContractNumber), request.UserId);

                        GetPatientsDataResponse patientDDResponse =
                            client.Post<GetPatientsDataResponse>(patientDDURL, new GetPatientsDataRequest
                            {
                                Context = "NG",
                                ContractNumber = request.ContractNumber,
                                Version = request.Version,
                                UserId = request.UserId,
                                PatientIds = recentPatientIds
                            } as object);

                        if (patientDDResponse != null && patientDDResponse.Patients != null)
                        {   
                            patients = new List<CohortPatient>();
                            foreach (string id in recentPatientIds)
                            {
                                PatientData pd;
                                if (patientDDResponse.Patients.TryGetValue(id, out pd))
                                {
                                    patients.Add(new CohortPatient
                                    {
                                        Id = pd.Id,
                                        FirstName = pd.FirstName,
                                        LastName = pd.LastName,
                                        MiddleName = pd.MiddleName,
                                        PreferredName = pd.PreferredName,
                                        DOB = pd.DOB,
                                        Gender = pd.Gender,
                                        Suffix = pd.Suffix
                                    });
                                }
                            }
                        }
                    }
                    response = new GetRecentPatientsResponse();
                    response.ContactId = dataDomainResponse.Contact.Id;
                    response.Limit = dataDomainResponse.Limit;
                    response.Patients = patients;
                    response.Version = dataDomainResponse.Version;
                }
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:GetRecentPatients()::" + wse.Message, wse.InnerException);
            }
            return response;
        }

        

        public SearchContactsResponse SearchContacts(SearchContactsRequest request)
        {
            var response = new SearchContactsResponse();
           
            if(request == null)
                throw new ArgumentNullException("request");

            if (request.ContactTypeIds.IsNullOrEmpty() && request.ContactStatuses.IsNullOrEmpty())
                throw new Exception("Please provide atleast ContactTypeIds or ContactStatuses to filter");

            try
            {
                var normalizedTake = NormalizeTake(request.Take);
                var normalizeSkip = NormalizeSkip(request.Skip);
                var contactTypeIds =  request.ContactTypeIds;// == null ? new List<string>()  : BuildContactTypeIds(request.ContactTypes);
                var contactStatuses = request.ContactStatuses ==null ? null: request.ContactStatuses.Select(s => (DataDomain.Contact.DTO.Status)s).ToList();
                var contactSubTypeIds = request.ContactSubTypeIds;

                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/SearchContacts",
                                                                                DDContactServiceUrl,
                                                                                "NG",
                                                                                request.Version,
                                                                                request.ContractNumber), request.UserId);

                var dataDomainResponse =
                    client.Post<DataDomain.Contact.DTO.SearchContactsDataResponse>(url, new SearchContactsDataRequest
                    {
                        Take = normalizedTake,
                        Skip = normalizeSkip,
                        ContactSubTypeIds = contactSubTypeIds,
                        ContactTypeIds = contactTypeIds,
                        ContactStatuses = contactStatuses,
                        FirstName =  request.FirstName,
                        LastName = request.LastName,
                        FilterType = (DataDomain.Contact.DTO.FilterType)request.FilterType
                    } as object);


                if (dataDomainResponse != null)
                {

                    response.TotalCount = dataDomainResponse.TotalCount;

                    if (!dataDomainResponse.Contacts.IsNullOrEmpty())
                    {
                        response.Contacts = dataDomainResponse.Contacts.Select(Mapper.Map<Contact>).ToList();
                    }
                    
                   
                }

            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:SearchContacts()::" + wse.Message, wse.InnerException);
            }
            return response;

        }

        #endregion

        #region ContactTypeLookUp

        public NG.DTO.GetContactTypeLookupResponse GetContactTypeLookup(NG.DTO.GetContactTypeLookupRequest request)
        {
            NG.DTO.GetContactTypeLookupResponse ctResponse = new NG.DTO.GetContactTypeLookupResponse();

            try
            {
                //Execute call(s) to ContactTypeLookup Data Domain
                IRestClient client = new JsonServiceClient();
                
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/ContactTypeLookUps?grouptype={4}&flattendata=true",
                                                                                           DDContactServiceUrl,
                                                                                           "NG",
                                                                                           request.Version,
                                                                                           request.ContractNumber,
                                                                                           request.GroupType), request.UserId);

                GetContactTypeLookUpDataResponse response = client.Get<GetContactTypeLookUpDataResponse>(url);

                if (response != null && response.ContactTypeLookUps != null)
                {
                   
                    var mapData = response.ContactTypeLookUps.Select(Mapper.Map<ContactTypeLookUp>).ToList();

                    ctResponse.ContactTypeLookUps = mapData;
                }
                return ctResponse;
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:GetContactTypeLookup()::" + wse.Message, wse.InnerException);
            }
        }

      

        #endregion ContactTypeLookUp

        #region Private methods
        private List<Module> getModuleInfo(DD.GetProgramDetailsSummaryResponse resp, IAppDomainRequest request)
        {
            try
            {
                List<Module> modules = null;
                if (resp.Program.Modules != null)
                {
                    modules = new List<Module>();
                    resp.Program.Modules.ForEach(r => modules.Add(new Module
                    {
                        Id = r.Id,
                        ProgramId = r.ProgramId,
                        Description = r.Description,
                        Name = r.Name,
                        Status = (int)r.Status,
                        Completed = r.Completed,
                        Enabled = r.Enabled,
                        Next = r.Next,
                        Order = r.Order,
                        Previous = r.Previous,
                        SpawnElement = getSpawnElement(r),
                        SourceId = r.SourceId,
                        AssignById = r.AssignBy,
                        AssignDate = r.AssignDate,
                        AssignToId = r.AssignTo,
                        AttrStartDate = r.AttrStartDate,
                        AttrEndDate = r.AttrEndDate,
                        ElementState = r.ElementState,
                        StateUpdatedOn = r.StateUpdatedOn,
                        CompletedBy = r.CompletedBy,
                        DateCompleted = r.DateCompleted,
                        Objectives = GetObjectivesInfo(r.Objectives),
                        Actions = getActionsInfo(r, request, false)
                    }));
                }
                return modules;
            }
            catch (Exception ex)
            {
                throw new WebServiceException("AD:getModuleInfo()::" + ex.Message, ex.InnerException);
            }
        }


        private List<Actions> getActionsInfo(DD.ModuleDetail r, IAppDomainRequest request, bool includeSteps)
        {
            try
            {
                List<Actions> action = null;
                if (r.Actions != null)
                {
                    action = r.Actions.Select(a => getActionInfo(a, request, includeSteps)).ToList();
                }
                return action;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:getActionsInfo()::" + ex.Message, ex.InnerException);
            }
        }

        private Actions getActionInfo(DD.ActionsDetail a, IAppDomainRequest request, bool includeSteps)
        {
            Actions action = null;
            if (a != null)
            {
                action = new Actions
                {
                    CompletedBy = a.CompletedBy,
                    Description = a.Description,
                    Id = a.Id,
                    ModuleId = a.ModuleId,
                    Name = a.Name,
                    Status = (int)a.Status,
                    Completed = a.Completed,
                    Enabled = a.Enabled,
                    Next = a.Next,
                    Order = a.Order,
                    Previous = a.Previous,
                    SpawnElement = getSpawnElement(a),
                    SourceId = a.SourceId,
                    AssignById = a.AssignBy,
                    AssignDate = a.AssignDate,
                    AssignToId = a.AssignTo,
                    AttrStartDate = a.AttrStartDate,
                    AttrEndDate = a.AttrEndDate,
                    ElementState = a.ElementState,
                    StateUpdatedOn = a.StateUpdatedOn,
                    DateCompleted = a.DateCompleted,
                    Archived = a.Archived,
                    ArchivedDate = a.ArchivedDate,
                    ArchiveOriginId = a.ArchiveOriginId,
                    Objectives = GetObjectivesInfo(a.Objectives),
                    DeleteFlag = a.DeleteFlag
                };
                if (includeSteps)
                {
                    action.Steps = getStepsInfo(a, request);
                }
                else
                {
                    action.Steps = new List<Step>();
                }
            }
            return action;
        }

        private List<Step> getStepsInfo(DD.ActionsDetail a, IAppDomainRequest request)
        {
            List<Step> steps = a.Steps.Select(s => new Step
            {
                Description = s.Description,
                Id = s.Id,
                SourceId = s.SourceId,
                ActionId = s.ActionId,
                Notes = s.Notes,
                Question = s.Question,
                Status = (int)s.Status,
                Title = s.Title,
                Text = s.Text,
                StepTypeId = s.StepTypeId,
                Completed = s.Completed,
                ControlType = s.ControlType,
                Enabled = s.Enabled,
                Header = s.Header,
                Next = s.Next,
                Order = s.Order,
                Previous = s.Previous,
                IncludeTime = s.IncludeTime,
                SelectType = s.SelectType,
                AssignById = s.AssignBy,
                AssignDate = s.AssignDate,
                ElementState = s.ElementState,
                StateUpdatedOn = s.StateUpdatedOn,
                Responses = getResponses(s, request),
                SelectedResponseId = s.SelectedResponseId,
                CompletedBy = s.CompletedBy,
                DateCompleted = s.DateCompleted,
                SpawnElement = getSpawnElement(s)
            }).ToList();

            return steps;
        }

        private List<Response> getResponses(DD.StepsDetail s, IAppDomainRequest request)
        {
            List<Response> resps = null;
            if (s.Responses != null && s.Responses.Count > 0)
            {
                resps = s.Responses.Select(z => new Response
                {
                    Id = z.Id,
                    NextStepId = z.NextStepId,
                    Nominal = z.Nominal,
                    Order = z.Order,
                    Required = z.Required,
                    StepId = z.StepId,
                    Text = z.Text,
                    Value = z.Value,
                    SpawnElement = getADSpawnElement(z.SpawnElement)
                }).ToList();
            }
            else
            {
                // get the responses for step.
                resps = new List<Response>();
                List<DD.StepResponse> rsps = PlanElementEndpointUtil.GetResponsesForStep(s.Id, request);
                rsps.ForEach(r => resps.Add(new Response
                {
                    Id = r.Id,
                    NextStepId = r.NextStepId,
                    Nominal = r.Nominal,
                    Order = r.Order,
                    Required = r.Required,
                    SpawnElement = getADSpawnElement(r.Spawn),
                    StepId = r.StepId,
                    Text = r.Text,
                    Value = r.Value
                }));
            }
            return resps;
        }

        private List<SpawnElement> getADSpawnElement(List<DD.SpawnElementDetail> sed)
        {
            List<SpawnElement> se = new List<SpawnElement>();
            if (sed != null)
            {
                sed.ForEach(x =>
                {
                    se.Add(new SpawnElement
                    {
                        ElementId = x.ElementId,
                        ElementType = x.ElementType,
                        Tag = x.Tag
                    });
                });
            }
            return se;
        }

        private List<SpawnElement> getSpawnElement(DD.PlanElementDetail planElement)
        {
            List<SpawnElement> spawn = new List<SpawnElement>();

            if (planElement.SpawnElement != null)
            {
                spawn = planElement.SpawnElement.Select(s => new SpawnElement
                {
                    ElementId = s.ElementId,
                    ElementType = s.ElementType,
                    Tag = s.Tag
                }).ToList();
            }
            return spawn;
        }

        private ProgramAttribute getAttributes(DD.ProgramAttributeData programAttributeData)
        {
            ProgramAttribute programAttribute = null;
            if(programAttributeData != null)
            {
                programAttribute = new ProgramAttribute
                {
                    //AssignedBy = programAttributeData.AssignedBy, Sprint 12
                    //AssignedOn = programAttributeData.AssignedOn, Sprint 12
                    Completed = (int)programAttributeData.Completed,
                    CompletedBy = programAttributeData.CompletedBy,
                    DateCompleted = programAttributeData.DateCompleted,
                    DidNotEnrollReason = programAttributeData.DidNotEnrollReason,
                    Eligibility = (int)programAttributeData.Eligibility,
                    //AttrEndDate = programAttributeData.AttrEndDate, Sprint 12
                    Enrollment = (int)programAttributeData.Enrollment,
                    GraduatedFlag = (int)programAttributeData.GraduatedFlag,
                    Id = programAttributeData.Id.ToString(),
                    IneligibleReason = programAttributeData.IneligibleReason,
                    Locked = (int)programAttributeData.Locked,
                    OptOut = programAttributeData.OptOut,
                    OverrideReason = programAttributeData.OverrideReason,
                    PlanElementId = programAttributeData.PlanElementId.ToString(),
                    Population = programAttributeData.Population,
                    RemovedReason = programAttributeData.RemovedReason,
                    //AttrStartDate = programAttributeData.AttrStartDate, Sprint 12
                    Status = (int)programAttributeData.Status
                };
            }
            return programAttribute;
        }

        private TimeZonesLookUp getDefaultTimeZone(GetTimeZoneDataRequest request)
        {
            TimeZonesLookUp response = null;
            try
            {
                response = new TimeZonesLookUp();
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/TimeZone/Default",
                                                                        DDLookupServiceUrl,
                                                                        "NG",
                                                                        request.Version,
                                                                        request.ContractNumber), request.UserId);

                //  [Route("/{Context}/{Version}/{ContractNumber}/TimeZone/Default", "GET")]
                Phytel.API.DataDomain.LookUp.DTO.GetTimeZoneDataResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.LookUp.DTO.GetTimeZoneDataResponse>(url);

                TimeZoneData data = dataDomainResponse.TimeZone;
                response.Id = data.Id;
                response.Name = data.Name;
                response.DefaultZone = data.Default;
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:GetDefaultTimeZone()::" + wse.Message, wse.InnerException);
            }
            return response;
        }
       
        private int NormalizeTake(int? take)
        {
            var normalizedValue = 100;

            if (take > 0)
                normalizedValue = take.Value;

            return normalizedValue;

        }

        private int NormalizeSkip(int skip)
        {
            var normalizedSkip = 0;

            if (skip > 0)
                 normalizedSkip = skip;

            return normalizedSkip;

        }

        private void SyncContactByPatientData(string contactId,PatientData data, double version, string contractNumber, string userId)
        {
            if (data == null)
                return;


            IRestClient client = new JsonServiceClient();
            var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Contacts/{4}/Sync",
                                                                            DDContactServiceUrl,
                                                                            "NG",
                                                                            version,
                                                                            contractNumber,contactId), userId);

            var mappedRequest = Mapper.Map<DataDomain.Contact.DTO.SyncContactInfoData>(data);

            var dataDomainResponse =
                client.Put<DataDomain.Contact.DTO.SyncContactInfoDataResponse>(url, new SyncContactInfoDataRequest
                {
                    ContactInfo =  mappedRequest

                } as object);

        }

        private void SyncPatientByContactData(ContactData data, double version, string contractNumber, string userId)
        {
            if (data == null)
                return;


            IRestClient client = new JsonServiceClient();
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patients/{4}/Sync",
                                                                            DDPatientServiceURL,
                                                                            "NG",
                                                                            version,
                                                                            contractNumber, data.PatientId), userId);

            var mappedRequest = Mapper.Map<DataDomain.Patient.DTO.SyncPatientInfoData>(data);

            var dataDomainResponse =
                client.Put<DataDomain.Patient.DTO.SyncPatientInfoData>(url, new SyncPatientInfoDataRequest
                {
                    PatientInfo = mappedRequest

                } as object);




        }

        private void InsertContactByPatient(Patient patient,string contractNumber, string userId, double version)
        {
            // Get the default TimeZone that is set in TimeZone LookUp table. ENG-1991 (Remove the Default TimeZone)
            //string defaultTimeZone = null;
            //GetTimeZoneDataRequest tzDataRequest = new GetTimeZoneDataRequest { ContractNumber = contractNumber, Version = version, Context = "NG", UserId = userId };
            //TimeZonesLookUp tz = getDefaultTimeZone(tzDataRequest);
            //if (tz != null)
            //{
            //    defaultTimeZone = tz.Id;
            //}

            //Get all the available comm modes in the lookup.
            //List<CommModeData> commModeData = new List<CommModeData>();
            List<CommMode> commModes = new List<CommMode>();
            GetAllCommModesRequest commRequest = new GetAllCommModesRequest { ContractNumber = contractNumber, UserId = userId, Version = version };
            List<IdNamePair> modesLookUp = GetAllCommModes(commRequest);
            if (modesLookUp != null && modesLookUp.Count > 0)
            {
                foreach (IdNamePair l in modesLookUp)
                {
                   // commModeData.Add(new CommModeData { ModeId = l.Id, OptOut = false, Preferred = false });
                    commModes.Add(new CommMode { LookUpModeId = l.Id, OptOut = false, Preferred = false });
                }
            }

            //Insert Contact
            var insertContactRequest = new InsertContactRequest
            {
                Contact = new Contact
                {
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                    MiddleName = patient.MiddleName,
                    PreferredName = patient.PreferredName,
                    Suffix = patient.Suffix,
                    PatientId = patient.Id,
                    DeceasedId = patient.DeceasedId,
                    Prefix = patient.Prefix,
                    Gender = patient.Gender,
                    StatusId = patient.StatusId,
                    ContactTypeId = Constants.PersonContactTypeId,
                    DataSource = patient.DataSource,
                    //TimeZoneId = defaultTimeZone,
                    Modes = commModes

                },
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version
            };

            InsertContact(insertContactRequest);
        }

        #endregion

        #region Obsolete

        /// <summary>
        /// DEPRECATED - SHOULD NOT BE USED AFTER CONTACTS REFACTORING
        /// </summary>
        /// <param name="context"></param>
        /// <param name="version"></param>
        /// <param name="contractNumber"></param>
        /// <param name="patientId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// 
        /// 
        /// 
        [Obsolete("Use InsertContactByPatient")]
        private Contact insertContactForPatient(string context, double version, string contractNumber, string patientId, string userId)
        {

            Contact newContact = null;
            try
            {
                // Get the default TimeZone that is set in TimeZone LookUp table. 
                string defaultTimeZone = null;
                GetTimeZoneDataRequest tzDataRequest = new GetTimeZoneDataRequest { ContractNumber = contractNumber, Version = version, Context = context, UserId = userId };
                TimeZonesLookUp tz = getDefaultTimeZone(tzDataRequest);
                if (tz != null)
                {
                    defaultTimeZone = tz.Id;
                }

                //Get all the available comm modes in the lookup.
                List<CommModeData> commModeData = new List<CommModeData>();
                List<CommMode> commMode = new List<CommMode>();
                GetAllCommModesRequest commRequest = new GetAllCommModesRequest { ContractNumber = contractNumber, UserId = userId, Version = version };
                List<IdNamePair> modesLookUp = GetAllCommModes(commRequest);
                if (modesLookUp != null && modesLookUp.Count > 0)
                {
                    foreach (IdNamePair l in modesLookUp)
                    {
                        commModeData.Add(new CommModeData { ModeId = l.Id, OptOut = false, Preferred = false });
                        commMode.Add(new CommMode { LookUpModeId = l.Id, OptOut = false, Preferred = false });
                    }
                }

                //[Route("/{Context}/{Version}/{ContractNumber}/Contacts", "POST")]
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Contacts",
                                                                                DDContactServiceUrl,
                                                                                context,
                                                                                version,
                                                                                contractNumber), userId);

                ContactData contactData = new ContactData { TimeZoneId = defaultTimeZone, Modes = commModeData, PatientId = patientId, ContactTypeId = Constants.PersonContactTypeId };
                InsertContactDataResponse dataDomainResponse =
                    client.Post<InsertContactDataResponse>(url, new InsertContactDataRequest
                    {
                        ContactData = contactData,
                        Context = context,
                        ContractNumber = contractNumber,
                        Version = version,
                        UserId = userId
                    } as object);

                if (dataDomainResponse != null && !string.IsNullOrEmpty(dataDomainResponse.Id))
                {
                    newContact = new Contact();
                    newContact.Id = dataDomainResponse.Id;
                    newContact.PatientId = patientId;
                    newContact.TimeZoneId = defaultTimeZone;
                    newContact.Modes = commMode;
                }
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:InsertContactForPatient()::" + wse.Message, wse.InnerException);
            }
            return newContact;
        }


        #endregion
    }
}
