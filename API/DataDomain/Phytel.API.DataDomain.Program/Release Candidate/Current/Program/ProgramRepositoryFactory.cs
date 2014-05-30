using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Program
{
    public class ProgramRepositoryFactory : IProgramRepositoryFactory
    {
        public IProgramRepository GetRepository(IDataDomainRequest request, RepositoryType type)
        {
            IProgramRepository repo = null;

            switch (type)
            {
                case RepositoryType.Program:
                    {
                        repo = new MongoProgramRepository(request.ContractNumber) as IProgramRepository;
                        break;
                    }
                case RepositoryType.PatientProgram:
                    {
                        repo = new MongoPatientProgramRepository(request.ContractNumber) as IProgramRepository;
                        break;
                    }
                case RepositoryType.ContractProgram:
                    {
                        repo = new MongoContractProgramRepository(request.ContractNumber) as IProgramRepository;
                        break;
                    }
                case RepositoryType.Response:
                    {
                        repo = new MongoResponseRepository(request.ContractNumber) as IProgramRepository;
                        break;
                    }
                case RepositoryType.PatientProgramResponse:
                    {
                        repo = new MongoPatientProgramResponseRepository(request.ContractNumber) as IProgramRepository;
                        break;
                    }
                case RepositoryType.PatientProgramAttribute:
                    {
                        repo = new MongoPatientProgramAttributeRepository(request.ContractNumber) as IProgramRepository;
                        break;
                    }
            }

            repo.UserId = request.UserId;
            return repo;
        }

        //public  IProgramRepository GetProgramRepository(IDataDomainRequest request )
        //{
        //    IProgramRepository repo = null;

        //    repo = new MongoProgramRepository(request.ContractNumber) as IProgramRepository;
        //    repo.UserId = request.UserId;
        //    return repo;
        //}

        //public  IProgramRepository GetPatientProgramRepository(IDataDomainRequest request)
        //{
        //    IProgramRepository repo = null;

        //    repo = new MongoPatientProgramRepository(request.ContractNumber) as IProgramRepository;
        //    repo.UserId = request.UserId;
        //    return repo;
        //}

        //public  IProgramRepository GetContractProgramRepository(IDataDomainRequest request)
        //{
        //    IProgramRepository repo = null;

        //    repo = new MongoContractProgramRepository(request.ContractNumber) as IProgramRepository;
        //    repo.UserId = request.UserId;
        //    return repo;
        //}

        //public  IProgramRepository GetStepResponseRepository(IDataDomainRequest request)
        //{
        //    IProgramRepository repo = null;

        //    repo = new MongoResponseRepository(request.ContractNumber) as IProgramRepository;
        //    repo.UserId = request.UserId;
        //    return repo;
        //}

        //public  IProgramRepository GetPatientProgramStepResponseRepository(IDataDomainRequest request)
        //{
        //    IProgramRepository repo = null;

        //    repo = new MongoPatientProgramResponseRepository(request.ContractNumber) as IProgramRepository;
        //    repo.UserId = request.UserId;
        //    return repo;
        //}

        //public  IProgramRepository GetProgramAttributesRepository(IDataDomainRequest request)
        //{
        //    IProgramRepository repo = null;

        //    repo = new MongoPatientProgramAttributeRepository(request.ContractNumber) as IProgramRepository;
        //    repo.UserId = request.UserId;
        //    return repo;
        //}
    }
}
