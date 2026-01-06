namespace HallOfGames.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }


        public DbSet<Game> Games { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<GameDeviceCompatibility> GameDeviceCompatibilities { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>(g =>
            {
                g.HasKey(e => e.Id);

                g.Property(e => e.Name).HasMaxLength(200);
                g.Property(e => e.Description).HasMaxLength(2000);
                g.Property(e => e.Cover).HasMaxLength(50);

                g.HasOne(g => g.Category)
                .WithMany(c => c.Games)
                .HasForeignKey(g => g.CategoryId);
            }
            );


            modelBuilder.Entity<Category>(c =>
            {
                c.HasKey(e => e.Id);

                c.Property(e => e.Name).HasMaxLength(50);
            });


            modelBuilder.Entity<Device>(d =>
            {
                d.HasKey(d => d.Id);

                d.Property(d => d.Name).HasMaxLength(50);
                d.Property(d => d.Icon).HasMaxLength(50);
            });


            modelBuilder.Entity<GameDeviceCompatibility>(c =>
            {
                c.HasKey(e => new {e.GameId, e.DeviceId});

                c.HasOne(c => c.Game)
                .WithMany(g => g.CompatibleDevices)
                .HasForeignKey(c => c.GameId)
                .OnDelete(DeleteBehavior.Cascade);

                c.HasOne(c => c.Device)
                .WithMany(d => d.CompetabileGames)
                .HasForeignKey(c => c.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
            });


            base.OnModelCreating(modelBuilder);
        }
    }
}
