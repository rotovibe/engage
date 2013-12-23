using System;
using System.Net;
using Phytel.API.DataDomain.Program;
using Phytel.API.DataDomain.Program.DTO;

namespace Phytel.API.DataDomain.Program.Service
{
    public class ProgramService : ServiceStack.ServiceInterface.Service
    {
        public GetProgramResponse Post(GetProgramRequest request)
        {
            GetProgramResponse response = new GetProgramResponse();
            try
            {
                response = ProgramDataManager.GetProgramByID(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public GetAllActiveProgramsResponse Get(GetAllActiveProgramsRequest request)
        {
            GetAllActiveProgramsResponse response = new GetAllActiveProgramsResponse();
            try
            {
                response = ProgramDataManager.GetAllActiveContractPrograms(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public PutProgramToPatientResponse Put(PutProgramToPatientRequest request)
        {
            PutProgramToPatientResponse response = new PutProgramToPatientResponse();
            try
            {
                response = ProgramDataManager.PutPatientToProgram(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public GetProgramResponse Get(GetProgramRequest request)
        {
            GetProgramResponse response = new GetProgramResponse();
            try
            {
                response = ProgramDataManager.GetProgramByID(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        //public GetAllProgramsResponse Post(GetAllProgramsRequest request)
        //{
        //    GetAllProgramsResponse response = new GetAllProgramsResponse();
        //    try
        //    {
        //        response = ProgramDataManager.GetProgramList(request);
        //        response.Version = request.Version;
        //    }
        //    catch (Exception ex)
        //    {
        //        //TODO: Log this to C3 database via ASE
        //        base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
        //    }
        //    return response;
        //}
    }
}