using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using wmp3_asset_tracking.Models;

namespace wmp3_asset_tracking.Data
{
    public class AssetContext : DbContext
    {
        private const string ConnectionString =
            "Server=(localdb)\\mssqllocaldb;Database=AssetTrackerDb;Trusted_Connection=True";

        public DbSet<Asset> Assets => Set<Asset>();

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(ConnectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asset>()
                .HasDiscriminator<string>("AssetKind")
                .HasValue<Computer>("Computer")
                .HasValue<MobilePhone>("MobilePhone");
        }
    }
}