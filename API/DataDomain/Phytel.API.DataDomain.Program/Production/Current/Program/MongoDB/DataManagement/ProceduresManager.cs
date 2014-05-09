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
using MongoDB.Bson;
using Phytel.API.Common;
using Phytel.API.DataDomain.Program.MongoDB.DataManagement;

namespace Phytel.API.DataDomain.Program.MongoDB.DataManagement
{
    public class ProceduresManager : IProceduresManager
    {
        public PostMongoProceduresResponse ExecuteProcedure(PostMongoProceduresRequest request)
        {
            try
            {
                PostMongoProceduresResponse response = new PostMongoProceduresResponse();

                IMongoProcedure proc = new MongoProcedureFactory().GetProcedure(request);
                proc.Execute();
                response.Results = proc.Results;
                response.Success = true;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:ExecuteProcedure()::" + ex.Message, ex.InnerException);
            }
        }
    }
}   
