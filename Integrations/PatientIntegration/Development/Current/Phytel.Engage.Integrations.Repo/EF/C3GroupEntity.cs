namespace Phytel.Engage.Integrations.Repo.EF
{
    using System;
    using System.Collections.Generic;

    public class C3GroupEntity
    {
        public int C3GroupEntityId { get; set; }
        public int C3GroupId { get; set; }
        public int EntityId { get; set; }
        public int C3GroupEntityTypeId { get; set; }
        public bool DeleteFlag { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }

        public virtual C3Group C3Group { get; set; }
    }
}
