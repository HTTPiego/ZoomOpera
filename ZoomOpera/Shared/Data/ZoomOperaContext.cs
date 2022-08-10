using Microsoft.EntityFrameworkCore;
using ZoomOpera.Shared.Entities;

namespace ZoomOpera.Server.Data
{
    public class ZoomOperaContext : DbContext
    {   
        public DbSet<ImageMapCoordinate> ImageMapCoordinates { get; set; }
        public DbSet<ImageMap> ImageMaps { get; set; }
        public DbSet<OperaImage> OperaImages { get; set; }
        public DbSet<Opera> Operas { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<MonitorPlatform> MonitorPlatforms { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public ZoomOperaContext() { }
        public ZoomOperaContext(DbContextOptions<ZoomOperaContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<Location>().
            //    HasOne(location => location.Opera).
            //    WithOne(opera => opera.Location).
            //    HasForeignKey<Opera>(opera => opera.LocationId).
            //    OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Level>().
            //    HasMany(level => level.MonitorPlatforms).
            //    WithOne(platform => platform.Level).
            //    IsRequired().
            //    HasForeignKey(platform => platform.LevelId).
            //    OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Level>().
            //    HasMany(level => level.Locations).
            //    WithOne(location => location.Level).
            //    IsRequired().
            //    HasForeignKey(location => location.LevelId).
            //    OnDelete(DeleteBehavior.Cascade);


            //modelBuilder.Entity<Building>().
            //    HasMany(building => building.Levels).
            //    WithOne(level => level.Building).
            //    IsRequired().
            //    HasForeignKey(level => level.BuildingId).
            //    OnDelete(DeleteBehavior.Cascade);


            //modelBuilder.Entity<Level>().
            //    HasOne<Building>(level => level.Building).
            //    WithMany(building => building.Levels).
            //    HasForeignKey(level => level.BuildingId);
        }
    }
}
