using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ATARK.Models.Entity
{
    public class AppDbContext : DbContext
    {
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Fish> Fishs { get; set; }
        public DbSet<Pool> Pools { get; set; }
        public DbSet<Herd> Herds { get; set; }
        public DbSet<ClosedWaterSupplyInstallation> ClosedWaterSupplyInstallations { get; set; }
        public DbSet<Milking> Milkings { get; set; }
        public DbSet<KindOfFish> KindsOfFishs { get; set; }
        public DbSet<Pregnancy> Pregnancys { get; set; }
        public DbSet<StateOfTheSystem> StatesOfSystems { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Fish>()
                .HasOne(m => m.PoolNow)
                .WithMany(t => t.FishsNow)
                .HasForeignKey(m => m.PoolNowId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Fish>()
                .HasOne(m => m.RelocationPool)
                .WithMany(t => t.RelocationFishs)
                .HasForeignKey(m => m.RelocationPoolId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pool>()
               .HasMany(q => q.FishsNow)
               .WithOne(a => a.PoolNow)
               .HasForeignKey(a => a.PoolNowId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pool>()
               .HasMany(q => q.RelocationFishs)
               .WithOne(a => a.RelocationPool)
               .HasForeignKey(a => a.RelocationPoolId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClosedWaterSupplyInstallation>()
               .HasOne(q => q.StateOfTheSystem)
               .WithOne(a => a.ClosedWaterSupplyInstallation)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StateOfTheSystem>()
               .HasOne(q => q.ClosedWaterSupplyInstallation)
               .WithOne(a => a.StateOfTheSystem)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
