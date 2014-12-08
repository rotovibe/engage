using Phytel.API.DataDomain.Contract.DTO;
using System;

namespace Phytel.API.DataDomain.Contract
{
    public interface IContractDataManager
    {
        //GetAllCareManagersDataResponse GetCareManagers(GetAllCareManagersDataRequest request);
        ContractData GetContracts(GetContractsDataRequest request);
        //ContractData GetContractByUserId(GetContractByUserIdDataRequest request);
        //PutContractDataResponse InsertContract(PutContractDataRequest request);
        //SearchContractsDataResponse SearchContracts(SearchContractsDataRequest request);
        //PutUpdateContractDataResponse UpdateContract(PutUpdateContractDataRequest request);
        //PutRecentPatientResponse AddRecentPatient(PutRecentPatientRequest request);
        //GetContractByContractIdDataResponse GetContractByContractId(GetContractByContractIdDataRequest request);
        //DeleteContractByPatientIdDataResponse DeleteContractByPatientId(DeleteContractByPatientIdDataRequest request);
        //UndoDeleteContractDataResponse UndoDeleteContract(UndoDeleteContractDataRequest request);
    }
}
