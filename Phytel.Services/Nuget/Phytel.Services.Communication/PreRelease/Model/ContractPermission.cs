
namespace Phytel.API.DataDomain.Communication.DTO
{
    public class ContractPermission
    {
        public int RoleID { get; set; }
		public string RoleName { get; set; }
		public string ContactCategoryCode { get; set; }
		public int ParentObjectID { get; set; }
		public string ParentObjectName { get; set; }
		public int ChildObjectID { get; set; }
		public string ChildObjectName { get; set; }
		public int ParentObjectTypeCode { get; set; }
		public int ChildObjectTypeCode { get; set; }
		public string ChildNodeName { get; set; }
		public string ChildNodeXMLAction { get; set; }
		public string ChildNodeXMLStatus { get; set; }
		public int RightsFlags { get; set; }
		public int SequenceNbr { get; set; }
    }
}
