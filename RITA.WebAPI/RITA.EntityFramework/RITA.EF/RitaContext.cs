
using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFramework;
using RITA.EF.Models;


namespace RITA.EF
{
    [System.Data.Entity.DbConfigurationType(typeof(MySqlEFConfiguration))]
    public partial class RitaContext : DbContext
    {
        public RitaContext() { }

        public RitaContext(DbContextOptions<RitaContext> options) : base(options) { }

        public virtual DbSet<Suite> Suites { get; set; }

        public virtual DbSet<TestCase> TestCases { get; set; }

        public virtual DbSet<ContentType> ContentTypes { get; set; }

        public virtual DbSet<TestType> TestTypes { get; set; }

        public virtual DbSet<TestData> TestData { get; set; }

        public virtual DbSet<Favorite> Favorites { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseMySql(""));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestType>().HasData(new TestType { Id = 1, Name = "regression", CreatedBy = "EF" });

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity(entityType.ClrType).ToTable(entityType.ClrType.Name);
            }

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}