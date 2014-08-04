using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program.DTO;

namespace Phytel.API.DataDomain.Program.Test.Stubs
{
    public class StubProgramRepositoryFactory : IProgramRepositoryFactory
    {
        public IProgramRepository GetRepository(Interface.IDataDomainRequest request, RepositoryType type)
        {
            IProgramRepository repo = null;

            switch (type)
            {
                case RepositoryType.Program:
                    {
                        repo = new StubMongoProgramRepository(request.ContractNumber) as IProgramRepository;
                        break;
                    }
                case RepositoryType.PatientProgram:
                    {
                        repo = new StubMongoPatientProgramRepository(request.ContractNumber) as IProgramRepository;
                        break;
                    }
                case RepositoryType.ContractProgram:
                    {
                        repo = new StubMongoContractProgramRepository(request.ContractNumber) as IProgramRepository;
                        break;
                    }
                case RepositoryType.Response:
                    {
                        repo = new StubMongoResponseRepository(request.ContractNumber) as IProgramRepository;
                        break;
                    }
                case RepositoryType.PatientProgramResponse:
                    {
                        repo = new StubMongoPatientProgramResponseRepository(request.ContractNumber) as IProgramRepository;
                        break;
                    }
                case RepositoryType.PatientProgramAttribute:
                    {
                        repo = new StubMongoPatientProgramAttributeRepository(request.ContractNumber) as IProgramRepository;
                        break;
                    }
            }

            repo.UserId = request.UserId;
            return repo;
        }
    }
}
