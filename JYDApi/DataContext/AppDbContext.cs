using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JYD.DataContext.DbModel;
using System.Reflection.Emit;

namespace JYD.DataContext
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Exploded>().Property(m => m.SEQ_NO).HasColumnType("smallint");
            builder.Entity<Exploded>().Property(m => m.QTY_ALLOCATED).HasColumnType("smallint");
            builder.Entity<Exploded>().Property(m => m.QTY_EXCLUDED).HasColumnType("smallint");

            builder.Entity<UserAuditDB>().HasKey(m => new
            {
                m.USER_ID,
                m.TIME_STAMP
            });


            builder.Entity<Allocation_HDR>().HasKey(m => new
            {
                m.CONTROL_NO,
                m.SEQ_NO
            });

            builder.Entity<Status_Ref>().HasKey(m => new
            {
                m.TABLE_NAME,
                m.STATUS_CODE
            });

            //base.OnModelCreating(builder);
            base.OnModelCreating(builder);

           
        }

        
        public virtual DbSet<UserDB> User { get; set; }
        public  virtual DbSet<Retrieve_UserdtlDB> UserdtlDBs { get; set; }
        public virtual DbSet<UserAuditDB> UserAudit { get; set; }
        public virtual DbSet<Agency_Ref> Agencyref { get; set; }
        public virtual DbSet<DealerDB> Dealer { get; set; }
        public virtual DbSet<ItemDB> Item { get; set; }
        public virtual DbSet<AllocationDtlDB> AllocationDtls { get; set; }
        public virtual DbSet<Validate> Validates { get; set; }
        public virtual DbSet<CheckDuplicate> Duplicates { get; set; }
        public virtual DbSet<Exploded> Explodeds { get; set; }
        public virtual DbSet<NewAllocationDB> NewAllocation { get; set; }
        public virtual DbSet<Allocation_HDR> AllocationHDR { get; set; }
        public virtual DbSet<Allocation_HDR> CancellAllocation { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public  virtual DbSet<Allocation_Inquiry> Alloc_Inquiries { get; set; }
        public virtual DbSet<Status_Ref> Status_Refs { get; set; }
        public virtual DbSet<ReportbyRefNo> RepRefno { get; set; }
        public virtual DbSet<ReportbyPlateNo> RepPlateno { get; set; }
        public virtual DbSet<allochdr> GetAllochdrs { get; set; }
        public virtual DbSet<AllocationException> allocEx { get; set; }
        public virtual DbSet<AllocationException> delallocEx { get; set; }
        public virtual DbSet<AllocationException> AllocationExceptions { get; set; }
        public virtual DbSet<Validate_PlateEngChass> validatePEC { get; set; }
        public virtual DbSet<GetPlateFIFO> fifo { get; set; }
        public virtual DbSet<PlateAllocationReport> PlateAllocationReport { get; set; }
        public virtual DbSet<PlateAllocationReportDtl> PlateAllocationReportDtl { get; set; }
        public virtual DbSet<RevertPlateAllocation> RevertPlateAllocation { get; set; }
        public virtual DbSet<DeletePlateAssignment> DeletePlateAssignment { get; set; }

    }
}
