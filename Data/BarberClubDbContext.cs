using Microsoft.EntityFrameworkCore;

public class BarberClubDbContext : DbContext
{
    public BarberClubDbContext(DbContextOptions<BarberClubDbContext> options) : base(options)
    {
    }

    public DbSet<Servicio> Servicios => Set<Servicio>();
    public DbSet<Barbero> Barberos => Set<Barbero>();
    public DbSet<Reserva> Reservas => Set<Reserva>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelo)
    {
        modelo.Entity<Servicio>().Property(s => s.Nombre).HasMaxLength(60);
        modelo.Entity<Servicio>().Property(s => s.Precio).HasPrecision(10, 2);

        modelo.Entity<Servicio>().HasData(
            new Servicio { Id = 1, Nombre = "Corte clásico", Precio = 25000m, Duracion = 30, Activo = true },
            new Servicio { Id = 2, Nombre = "Afeitado premium", Precio = 20000m, Duracion = 20, Activo = true },
            new Servicio { Id = 3, Nombre = "Masaje facial", Precio = 35000m, Duracion = 25, Activo = true });

        modelo.Entity<Barbero>(e =>
        {
            e.Property(b => b.Nombre).HasMaxLength(80).IsRequired();
            e.HasData(
                new Barbero { Id = 1, Nombre = "Carlos", Activo = true },
                new Barbero { Id = 2, Nombre = "Andrés", Activo = true },
                new Barbero { Id = 3, Nombre = "Miguel", Activo = true });
        });

        modelo.Entity<Reserva>(e =>
        {
            e.Property(r => r.Inicio).HasColumnType("timestamp without time zone");  
            e.Property(r => r.NombreCliente).HasMaxLength(100).IsRequired();
            e.Property(r => r.Telefono).HasMaxLength(20).IsRequired();
            e.Property(r => r.Estado).HasMaxLength(20).IsRequired();

            e.HasOne(r => r.Servicio).WithMany().HasForeignKey(r => r.ServicioId);
            e.HasOne(r => r.Barbero).WithMany(b => b.Reservas).HasForeignKey(r => r.BarberoId);

            // La base de datos también impide repetir barbero + hora
            e.HasIndex(r => new { r.BarberoId, r.Inicio }).IsUnique();
        });
        modelo.Entity<Usuario>().Property(u => u.Nombre).HasMaxLength(60);
        modelo.Entity<Usuario>().Property(u => u.Correo).HasMaxLength(120);
        modelo.Entity<Usuario>().Property(u => u.PasswordHash).HasMaxLength(200);
        modelo.Entity<Usuario>().Property(u => u.Rol).HasConversion<string>().HasMaxLength(20);
        modelo.Entity<Usuario>().HasIndex(u => u.Correo).IsUnique();
    }
}