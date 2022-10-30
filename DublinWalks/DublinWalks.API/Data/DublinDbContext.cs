using DublinWalks.API.Modals.Domain;
using Microsoft.EntityFrameworkCore;

namespace DublinWalks.API.Data
{
    public class DublinDbContext : DbContext 
    {
        public DublinDbContext(DbContextOptions<DublinDbContext> Options) : base(Options) 
        {

        }

        public DbSet<Region> Regions { get; set; }

        public DbSet<Walk> Walks { get; set; }
        public DbSet<WalkDifficulty> WalkDifficulty { get; set; }

    }
}
