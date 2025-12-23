using Microsoft.EntityFrameworkCore;

namespace SiteCrestBackend.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Chemical> Chemicals => Set<Chemical>();
        public DbSet<Pathway> Pathways => Set<Pathway>();
        public DbSet<ChemicalPathway> ChemicalPathways => Set<ChemicalPathway>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChemicalPathway>().HasKey(cp => new { cp.ChemicalId, cp.PathwayId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
