using Phytel.API.DataDomain.Program.DTO;
using System.Collections.Generic;
using System;
using System.Linq;
using Phytel.API.DataDomain.PatientSystem.MongoDB.DataManagement;
using System.Reflection;

namespace Phytel.API.DataDomain.PatientSystem.MongoDB.DataManagement
{
    public class ProceduresManager : IProceduresManager
    {
        public GetMongoProceduresResponse ExecuteProcedure(GetMongoProceduresRequest request)
        {
            try
            {
                GetMongoProceduresResponse response = new GetMongoProceduresResponse();
                response.Success = false;
                IMongoProcedure proc = new MongoProcedureFactory().GetProcedure(request);
                if (proc != null)
                {
                    proc.Execute();
                    response.Results = proc.Results;
                    response.Success = true;            
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:ExecuteProcedure()::" + ex.Message, ex.InnerException);
            }
        }


        public GetMongoProceduresListResponse GetProceduresList(GetMongoProceduresListRequest request)
        {
            try
            {
                GetMongoProceduresListResponse response = new GetMongoProceduresListResponse();
                response.Procedures = new List<Program.DTO.MongoProcedure>();

                foreach (Type mytype in Assembly.GetExecutingAssembly().GetTypes()
                    .Where(mytype => mytype.GetInterfaces().Contains(typeof (IMongoProcedure)) && !mytype.IsAbstract))
                {
                    var allProcedureConsts = GetProcedureConstValues<string>(mytype);
                    response.Procedures.Add(new Program.DTO.MongoProcedure
                    {
                        Name = allProcedureConsts[0],
                        Description = allProcedureConsts.Count > 1 ? allProcedureConsts[1] : null
                    });
                }

                response.Version = 1.0;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:ExecuteProcedure()::" + ex.Message, ex.InnerException);
            }
        }

        public List<T> GetProcedureConstValues<T>(Type type)
        {
            FieldInfo[] fields = type.GetFields(BindingFlags.Public
                                                | BindingFlags.Static
                                                | BindingFlags.FlattenHierarchy);

            return (fields.Where(fieldInfo => fieldInfo.IsLiteral
                                              && !fieldInfo.IsInitOnly
                                              && fieldInfo.FieldType == typeof (T))
                .Select(fi => (T) fi.GetRawConstantValue())).ToList();
        }
    }
}   
