using System.ComponentModel.DataAnnotations;

public class ReservaCrearDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Elige un servicio.")]
    public int ServicioId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Elige un barbero.")]
    public int BarberoId { get; set; }

    [Required(ErrorMessage = "Indica la fecha y la hora.")]
    public DateTime? Inicio { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    public string NombreCliente { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [RegularExpression(@"^[0-9+\s-]{7,20}$", ErrorMessage = "Escribe un teléfono válido (solo números, +, espacios o guiones).")]
    public string Telefono { get; set; } = string.Empty;
}

public class ReservaRespuestaDto
{
    public int Id { get; set; }
    public int ServicioId { get; set; }
    public int BarberoId { get; set; }
    public DateTime Inicio { get; set; }
    public string NombreCliente { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}

public class BarberoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
}
