﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Phytel.Engage.Integrations.Repo
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ContractEntities : DbContext
    {
        public ContractEntities()
            : base("name=ContractEntities")
        {
        // Get the ObjectContext related to this DbContext
        var objectContext = (this as IObjectContextAdapter).ObjectContext;
    
        // Sets the command timeout for all the commands
        objectContext.CommandTimeout = 120;
        }
    
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ContactEntity> ContactEntities { get; set; }
        public virtual DbSet<C3Group> C3Group { get; set; }
        public virtual DbSet<C3GroupEntity> C3GroupEntity { get; set; }
        public virtual DbSet<KeyData> KeyDatas { get; set; }
        public virtual DbSet<ReportPatient> ReportPatients { get; set; }
        public virtual DbSet<IntegrationPatientXref> IntegrationPatientXrefs { get; set; }
        public virtual DbSet<C3Patient> C3Patient { get; set; }
        public virtual DbSet<C3ProblemList> C3ProblemList { get; set; }
        public virtual DbSet<C3NoteAction> C3NoteAction { get; set; }
        public virtual DbSet<C3NoteCategory> C3NoteCategory { get; set; }
        public virtual DbSet<C3NotePatient> C3NotePatient { get; set; }
        public virtual DbSet<Phone> Phones { get; set; }
        public virtual DbSet<SubscriberEntity> SubscriberEntities { get; set; }
        public virtual DbSet<CommCategory> CommCategories { get; set; }
        public virtual DbSet<PatientRegistryHdr> PatientRegistryHdrs { get; set; }
        public virtual DbSet<PatientPersonalPhysician> PatientPersonalPhysicians { get; set; }
        public virtual DbSet<C3GroupEntityType> C3GroupEntityType { get; set; }
        public virtual DbSet<Subscriber> Subscribers { get; set; }
        public virtual DbSet<SubscriberSolution> SubscriberSolutions { get; set; }
    }
}
