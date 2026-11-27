using Microsoft.AspNetCore.Identity;

public static class Sembrador
{
    public static void Sembrar(BarberClubDbContext db, IPasswordHasher<Usuario> hasher)
    {
        if (db.Usuarios.Any())
        {
            return;
        }

        var ejemplos = new[]
        {
            new { Nombre = "Administrador de ejemplo", Correo = "admin@barberclub.com", Clave = "Admin123!", Rol = RolUsuario.Administrador },
            new { Nombre = "Barbero de ejemplo", Correo = "barbero@barberclub.com", Clave = "Barbero123!", Rol = RolUsuario.Barbero },
            new { Nombre = "Cliente de ejemplo", Correo = "cliente@barberclub.com", Clave = "Cliente123!", Rol = RolUsuario.Cliente }
        };

        foreach (var ejemplo in ejemplos)
        {
            var usuario = new Usuario
            {
                Nombre = ejemplo.Nombre,
                Correo = ejemplo.Correo,
                Rol = ejemplo.Rol
            };
            usuario.PasswordHash = hasher.HashPassword(usuario, ejemplo.Clave);
            db.Usuarios.Add(usuario);
        }

        db.SaveChanges();
    }
}
