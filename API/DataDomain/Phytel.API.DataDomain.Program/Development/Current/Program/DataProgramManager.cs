using Phytel.API.DataDomain.Program.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Program;
using Phytel.API.Interface;
using System.Collections.Generic;
using System;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.DataDomain.Patient.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Configuration;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using System.Linq;

namespace Phytel.API.DataDomain.Program
{
    public static class ProgramDataManager
    {
        public static GetProgramResponse GetProgramByID(GetProgramRequest request)
        {
            GetProgramResponse programResponse = new GetProgramResponse();
            DTO.Program result;

            IProgramRepository<GetProgramResponse> repo = ProgramRepositoryFactory<GetProgramResponse>.GetProgramRepository(request.ContractNumber, request.Context);
            result = repo.FindByID(request.ProgramID) as DTO.Program;

            programResponse.Program = result;
            return (programResponse != null ? programResponse : new GetProgramResponse());
        }

        public static GetAllActiveProgramsResponse GetAllActiveContractPrograms(GetAllActiveProgramsRequest request)
        {
            GetAllActiveProgramsResponse response = new GetAllActiveProgramsResponse();
            List<ProgramInfo> result;

            IProgramRepository<GetAllActiveProgramsResponse> repo =
                Phytel.API.DataDomain.Program.ProgramRepositoryFactory<GetAllActiveProgramsResponse>.GetProgramRepository(request.ContractNumber, request.Context);

            result = repo.GetActiveProgramsInfoList(request);
            response.Programs = result;

            return response;
        }

        public static PutProgramToPatientResponse PutPatientToProgram(PutProgramToPatientRequest request)
        {
            PutProgramToPatientResponse response = new PutProgramToPatientResponse();

            if (!IsValidPatientId(request))
            {
                return FormatExceptionResponse(response, "Patient does not exist or has an invalid id.", "500");
            }

            if (!IsValidContractProgramId(request))
            {
                return FormatExceptionResponse(response, "ContractProgram does not exist or has an invalid identifier.", "500");
            }

            if (!IsContractProgramAssignable(request))
            {
                return FormatExceptionResponse(response, "ContractProgram is not currently active.", "500");
            }

            IProgramRepository<PutProgramToPatientResponse> patProgRepo =
                Phytel.API.DataDomain.Program.ProgramRepositoryFactory<PutProgramToPatientResponse>
                .GetPatientProgramRepository(request.ContractNumber, request.Context);

            object resp = patProgRepo.Insert((object)request);
            response = (PutProgramToPatientResponse)resp;

            return response;
        }

        public static PutProgramActionProcessingResponse PutProgramActionUpdate(PutProgramActionProcessingRequest request)
        {
            try
            {
                PutProgramActionProcessingResponse response = new PutProgramActionProcessingResponse();

                IProgramRepository<PutProgramActionProcessingResponse> patProgRepo =
                    Phytel.API.DataDomain.Program.ProgramRepositoryFactory<PutProgramActionProcessingResponse>
                    .GetPatientProgramRepository(request.ContractNumber, request.Context);

                response.program = (ProgramDetail)patProgRepo.Update((object)request);

                return response;
            }
            catch
            {
                throw;
            }
        }

        private static PutProgramToPatientResponse FormatExceptionResponse(PutProgramToPatientResponse response, string reason, string errorcode)
        {
            response.Status = new ResponseStatus(errorcode, reason);
            response.Outcome = new Outcome() { Reason = reason, Result = 0 };
            return response;
        }

        private static bool IsContractProgramAssignable(PutProgramToPatientRequest p)
        {
            bool result = false;

            IProgramRepository<ContractProgram> contractProgRepo =
                            Phytel.API.DataDomain.Program.ProgramRepositoryFactory<ContractProgram>
                            .GetContractProgramRepository(p.ContractNumber, p.Context);

            ContractProgram c = contractProgRepo.FindByID(p.ContractProgramId) as ContractProgram;

            if (c != null)
            {
                if (c.Status == 1 && c.Delete != true)
                    result = true;
            }

            return result;
        }

        private static bool IsValidPatientId(PutProgramToPatientRequest request)
        {

            bool result = false;
            string path = ConfigurationManager.AppSettings["DDPatientServiceUrl"];
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            IRestClient client = new JsonServiceClient();
            GetPatientDataResponse response = client.Get<GetPatientDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/patient/{4}", 
                path,
                context,
                version,
                contractNumber,
                request.PatientId));

            if (response.Patient != null)
            {
                result = true;
            }

            return result;
        }

        private static bool IsValidContractProgramId(PutProgramToPatientRequest request)
        {
            bool result = false;
            IProgramRepository<PutProgramToPatientResponse> contractProgRepo =
                            Phytel.API.DataDomain.Program.ProgramRepositoryFactory<PutProgramToPatientResponse>
                            .GetContractProgramRepository(request.ContractNumber, request.Context);

            object contractProgram = contractProgRepo.FindByID(request.ContractProgramId);
            if (contractProgram != null)
            {
                result = true;
            }

            return result;
        }

        public static GetProgramDetailsSummaryResponse GetPatientProgramDetailsById(GetProgramDetailsSummaryRequest request)
        {
            try
            {
                GetProgramDetailsSummaryResponse response = null;

                IProgramRepository<GetProgramDetailsSummaryResponse> repo =
                    Phytel.API.DataDomain.Program.ProgramRepositoryFactory<GetProgramDetailsSummaryResponse>
                    .GetPatientProgramRepository(request.ContractNumber, request.Context);

                response = repo.FindByID(request.ProgramId) as GetProgramDetailsSummaryResponse;

                return response;
            }
            catch
            {
                throw;
            }
        }

        public static GetPatientProgramsResponse GetPatientPrograms(GetPatientProgramsRequest request)
        {
            GetPatientProgramsResponse response = null;

            IProgramRepository<GetPatientProgramsResponse> repo =
                Phytel.API.DataDomain.Program.ProgramRepositoryFactory<GetPatientProgramsResponse>
                .GetPatientProgramRepository(request.ContractNumber, request.Context);

            ICollection<SelectExpression> selectExpressions = new List<SelectExpression>();

            // PatientID
            SelectExpression patientSelectExpression = new SelectExpression();
            patientSelectExpression.FieldName = MEPatientProgram.PatientIdProperty;
            patientSelectExpression.Type = SelectExpressionType.EQ;
            patientSelectExpression.Value = request.PatientId;
            patientSelectExpression.ExpressionOrder = 1;
            patientSelectExpression.GroupID = 1;
            selectExpressions.Add(patientSelectExpression);

            APIExpression apiExpression = new APIExpression();
            apiExpression.Expressions = selectExpressions;

            Tuple<string, IEnumerable<object>> patientPrograms = repo.Select(apiExpression);

            if (patientPrograms != null)
            {
                List<ProgramDetail> pds = patientPrograms.Item2.Cast<ProgramDetail>().ToList();
                if (pds.Count > 0)
                {
                    response = new GetPatientProgramsResponse();

                    List<ProgramInfo> lpi = new List<ProgramInfo>();
                    pds.ForEach(pd => lpi.Add(new ProgramInfo
                        {
                            Id = pd.Id,
                            Name = pd.Name,
                            PatientId = pd.PatientId,
                            ProgramState = pd.ProgramState,
                            ShortName = pd.ShortName,
                            Status = pd.Status
                        })
                    );
                    response.programs = lpi;
                }
            }

            return response;
        }
    }
}   
