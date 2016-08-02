using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using C3.Business;
using C3.Business.Interfaces;
using C3.Data;
using C3.Data.Enum;


namespace C3.Business.Interfaces
{
    public interface IGroupService
    {
        List<Group> GetAllGroupsByContract(Contract contract);
        List<Group> GetAllGroupsByContractNoCache(Contract contract);
        DataTable GetAllGroupsByContractDT(Contract contract);
        DataTable GetAllGroupsByContractDTNoCache(Contract contract);
        DataTable GetGroupsByUserAndProductDT(Contract contract, Guid userId, GroupEntityTypes type);
        DataTable GetGroupByIdDT(Contract contract, int groupdId);
        Group GetGroupById(Contract contract, int groupId);
        List<Group> GetGroupsByUser(Contract contract, Guid userId);
        List<Group> GetGroupsByUser(Contract contract, Guid userId, bool includeAllGroups);
        List<Group> GetGroupsByUserAndProduct(Contract contract, Guid userId, GroupEntityTypes type);
        DataTable GetSchedulesByUser(Contract contract, Guid userId);
        int SaveGroup(Contract contract, int groupId, string name, string schedulesXML);
    }
}
