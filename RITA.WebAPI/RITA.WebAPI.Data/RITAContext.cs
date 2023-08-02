using Microsoft.EntityFrameworkCore;
using RITA.WebAPI.Data.Models;

namespace RITA.WebAPI.Data
{
    public partial class RITAContext : DbContext
    {
        public RITAContext() { }

        public RITAContext(DbContextOptions<RITAContext> options) : base(options) { }

        public virtual DbSet<CommonFields> CommonFields { get; set; } = null!;
        public virtual DbSet<RequestField> RequestFields { get; set; } = null!;
        public virtual DbSet<ResponseField> ResponseFields { get; set; } = null!;
        public virtual DbSet<Suite> Suites { get; set; } = null!;
        public virtual DbSet<TestCase> TestCases { get; set; } = null!;
        public virtual DbSet<TestData> TestDatas { get; set; } = null!;
        public virtual DbSet<TestType> TestTypes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = test; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False");
                //optionsBuilder.UseSqlServer("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = test; Integrated Security = True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}