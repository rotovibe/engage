
namespace Phytel.Engage.Integrations.Repo.EF
{
    using System;
    using System.Collections.Generic;

    public class C3Group
    {
        public C3Group()
        {
            this.C3GroupEntity = new HashSet<C3GroupEntity>();
        }

        public int C3GroupId { get; set; }
        public string Name { get; set; }
        public bool EnableAll { get; set; }
        public bool DeleteFlag { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public string Description { get; set; }

        public virtual ICollection<C3GroupEntity> C3GroupEntity { get; set; }
    }
}
