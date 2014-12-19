using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Phytel.API.DataDomain.Contract.Data.Map
{
    public class ContractMap : EntityTypeConfiguration<Contract>
    {
        public ContractMap()
        {
            // Primary Key
            this.HasKey(t => t.ContractID);

            // Properties
            this.Property(t => t.ContractNumber)
                .HasMaxLength(30);
            this.Property(t => t.ContractName)
                            .HasMaxLength(50);
            this.Property(t => t.StatusCode)
                            .HasMaxLength(5);

            // Table & Column Mappings
            this.ToTable("Contracts");
            this.Property(t => t.ContractID).HasColumnName("ContractID");
            this.Property(t => t.ContractNumber).HasColumnName("ContractNumber");
            this.Property(t => t.ContractName).HasColumnName("ContractName");
            this.Property(t => t.StatusCode).HasColumnName("StatusCode");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
        }
    }
}
