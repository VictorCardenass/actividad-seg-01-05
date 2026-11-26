using Microsoft.EntityFrameworkCore;

public class BarberClubDbContext : DbContext
{
    public BarberClubDbContext(DbContextOptions<BarberClubDbContext> options) : base(options)
    {
    }

    public DbSet<Servicio> Servicios => Set<Servicio>();

    protected override void OnModelCreating(ModelBuilder modelo)
    {
        modelo.Entity<Servicio>().Property(s => s.Nombre).HasMaxLength(60);
        modelo.Entity<Servicio>().Property(s => s.Precio).HasPrecision(10, 2);

        modelo.Entity<Servicio>().HasData(
            new Servicio { Id = 1, Nombre = "Corte clásico", Precio = 25000m, Duracion = 30, Activo = true },
            new Servicio { Id = 2, Nombre = "Afeitado premium", Precio = 20000m, Duracion = 20, Activo = true },
            new Servicio { Id = 3, Nombre = "Masaje facial", Precio = 35000m, Duracion = 25, Activo = true });
    }
}
