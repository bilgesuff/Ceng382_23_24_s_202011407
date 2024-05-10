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
}