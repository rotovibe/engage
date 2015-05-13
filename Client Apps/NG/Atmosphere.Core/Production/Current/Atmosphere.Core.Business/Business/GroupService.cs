using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phytel.Framework.SQL.Data;
using C3.Data;
using C3.Data.Enum;
using C3.Business.Interfaces;

namespace C3.Business
{
    public class GroupService : ServiceBase, IGroupService
    {
        #region Private Variables

        private static volatile GroupService _svc = null;
        private static object syncRoot = new Object();


        //FF: I made this public for Dependency Injection Implementation.
        public GroupService() { }

        #endregion

        #region Public Methods

        public static GroupService Instance
        {
            get
            {
                if (_svc == null)
                {
                    lock (syncRoot)
                    {
                        if (_svc == null)
                            _svc = new GroupService();
                    }
                }

                return _svc;
            }
        }


        public List<Group> GetAllGroupsByContract(Contract contract)
        {
            return CachedQueryAll<Group>(contract.ConnectionString, contract.Database, StoredProcedure.GetAllGroupsByContract, Group.Build, new CacheAccessor("C3Cache", string.Format("Groups{0}", contract.ContractId.ToString())));
        }
        
        public List<Group> GetAllGroupsByContractNoCache(Contract contract)
        {
            return QueryAll<Group>(contract.ConnectionString, contract.Database, StoredProcedure.GetAllGroupsByContract, Group.Build);
        }

        public DataTable GetAllGroupsByContractDT(Contract contract)
        {
            DataTable dt = CachedQuery(contract.ConnectionString, contract.Database, StoredProcedure.GetAllGroupsByContract, new CacheAccessor("C3Cache", string.Format("Groups{0}", contract.ContractId.ToString())));
            dt.PrimaryKey = new DataColumn[] {dt.Columns["GroupId"]};
            return dt;
        }

        public DataTable GetAllGroupsByContractDTNoCache(Contract contract)
        {
            DataTable dt = Query(contract.ConnectionString, contract.Database, StoredProcedure.GetAllGroupsByContract);
            dt.PrimaryKey = new DataColumn[] { dt.Columns["GroupId"] };
            return dt;
        }

        //public DataTable GetGroupsByUserDT(Contract contract, Guid userId)
        //{
        //    DataTable dt = CachedQuery(contract.ConnectionString, contract.Database, StoredProcedure.GetGroupsByUser, new CacheAccessor("C3Cache", string.Format("UserGroups{0}", userId.ToString())), userId);
        //    //DataTable dt = Query(contract.ConnectionString, contract.Database, StoredProcedure.GetGroupsByUser, userId);
        //    dt.PrimaryKey = new DataColumn[] {dt.Columns["GroupId"]};
        //    return dt;
        //}
        public DataTable GetGroupsByUserAndProductDT(Contract contract, Guid userId, GroupEntityTypes type)
        {
            DataTable dt = CachedQuery(contract.ConnectionString, contract.Database, StoredProcedure.GetGroupsByUserAndProduct, new CacheAccessor("C3Cache", string.Format("UserGroups{0}{1}{2}", userId.ToString(), (int)type, contract.ContractId.ToString())), userId, (int)type);
            dt.PrimaryKey = new DataColumn[] { dt.Columns["GroupId"] };
            return dt;
            
        }

        public DataTable GetGroupByIdDT(Contract contract , int groupdId)
        {
            DataTable dt = Query(contract.ConnectionString, contract.Database, StoredProcedure.GetGroupById, groupdId);
            dt.PrimaryKey = new DataColumn[] { dt.Columns["GroupId"] };
            return dt;
        }

        public Group GetGroupById(Contract contract, int groupId)
        {
            return Query(contract.ConnectionString, contract.Database, StoredProcedure.GetGroupById, Group.Build, groupId);
        }


        public List<Group> GetGroupsByUser(Contract contract, Guid userId)
        {
            return GetGroupsByUser(contract, userId, false);
        }

        public List<Group> GetGroupsByUser(Contract contract, Guid userId, bool includeAllGroups)
        {
            return QueryAll<Group>(contract.ConnectionString, contract.Database, StoredProcedure.GetGroupsByUser, Group.Build, userId, includeAllGroups);
        }

        public List<Group> GetGroupsByUserAndProduct(Contract contract, Guid userId, GroupEntityTypes type)
        {
            return QueryAll<Group>(contract.ConnectionString, contract.Database, StoredProcedure.GetGroupsByUserAndProduct, Group.Build, userId, (int)type);
        }

        public DataTable GetSchedulesByUser(Contract contract, Guid userId)
        {
            DataTable dt = CachedQuery(contract.ConnectionString, contract.Database, StoredProcedure.GetSchedulesByUser, new CacheAccessor("C3Cache", string.Format("Schedules{0}", userId.ToString())), userId);
            return dt;
        }

       

        public int SaveGroup(Contract contract, int groupId, string name, string schedulesXML)
        {   

           return groupId =  new SqlDataExecutor().Execute<int>(contract.ConnectionString, contract.Database, StoredProcedure.SaveGroup, Convert.ToInt32, 
               new object[]
                {
                    groupId
                    , name
                    , schedulesXML
                }
            );
        }

        public string GetGroupsXML(List<Group> groups)
        {
            string _groups = "";
            StringBuilder groupXML = new StringBuilder();

            //Beginning of XML 
            groupXML.Append("<groups>");

            List<int> ids = new List<int>();
            foreach (Group group in groups)
            {
                if (!ids.Contains(group.GroupId))
                {
                    groupXML.AppendFormat("<group>{0}</group>", group.GroupId);
                }
                ids.Add(group.GroupId);
            }

            groupXML.Append("</groups>");
            _groups = groupXML.ToString();
            return _groups;

        }
        #endregion
    }
}
