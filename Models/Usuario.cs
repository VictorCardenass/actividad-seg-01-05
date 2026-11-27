public enum RolUsuario
{
    Cliente,
    Barbero,
    Administrador
}

public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Correo { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public RolUsuario Rol { get; set; }
}
