using Microsoft.EntityFrameworkCore; 
public class AppDbContext : DbContext
{
public AppDbContext(){

}
public AppDbContext(DbContextOptions<AppDbContext> options) :
base(options)
{
}
public DbSet<Room> Rooms { get; set; }
public DbSet<Reservation> Reservations { get; set; }

 protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .Build();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(connectionString);
    }
}
}